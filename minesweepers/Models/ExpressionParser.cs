using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace minesweepers.Models
{
	public class ExpressionParser
	{
		private ISession session;

		/// <summary>
		/// https://code.google.com/p/solrmarc/source/browse/trunk/test/data/smoketest/solr/conf/stopwords.txt?r=1333
		/// </summary>
		public static string[] NOISE_WORDS =  {
			"a",
			"an",
			"and",
			"are",
			"as",
			"at",
			"be",
			"but",
			"by",
			"for",
			"if",
			"in",
			"into",
			"is",
			"it",
			"no",
			"not",
			"of",
			"on",
			"or",
			"s",
			"such",
			"t",
			"that",
			"the",
			"their",
			"then",
			"there",
			"these",
			"they",
			"this",
			"to",
			"was",
			"will",
			"with"
	};

		public ExpressionParser(ISession session)
		{
			this.session = session;
		}

		public IList<SearchEntry> Evaluate(string expression)
		{
			expression = expression.Trim();
			var list = new List<SearchEntry>();
			if (HasOperator(expression, "OR"))
			{
				var parts = GetParts(expression, "OR");
				foreach (var part in parts)
				{
					var results = Evaluate(part);
					list.AddRange(results);
				}
			}

			else if (HasOperator(expression, "AND"))
			{
				var parts = GetParts(expression, "AND");
				foreach (var part in parts)
				{
					var results = Evaluate(part);

					if (list.Count == 0)
					{
						list.AddRange(results);
					}
					else
					{
						list = list.Intersect(results).ToList();
					}
				}
			}

			else if (HasOperator(expression, "NOT"))
			{
				var parts = GetParts(expression, "NOT");

				int count = 0;
				foreach (var part in parts)
				{
					var results = Evaluate(part);

					if (count == 0 && parts.Length > 1)
					{
						list.AddRange(results);
					}
					else
					{
						list.RemoveAll(x => results.Contains(x));
					}

					count++;
				}
			}

			else
			{

				string[] words = GetWords(expression);

				foreach (var word in words)
				{
					List<SearchEntry> entries =
						session.Query<SearchEntry>()
							.Where(x => x.Descriptor.Contains(word)).ToList();
						//session.QueryOver<SearchEntry>()
						//.WhereRestrictionOn(x => x.Descriptor).IsLike(word, MatchMode.Anywhere).List().ToList();

					// Cater for case sensitive since sql is not
					//entries.RemoveAll(x => !x.Descriptor.Contains(word));
					entries.RemoveAll(x => !x.SearchWords.Contains(word));
					list.AddRange(entries);
				}
			
			}

			//return session.QueryOver<SearchEntry>().WhereRestrictionOn(x => x.Descriptor).IsLike(expression, MatchMode.Anywhere).List();
			return list.OrderBy(x => x.Descriptor).ToList();
		}

		private static string[] GetParts(string expression, string op)
		{
			return expression.Split(new string[] { op.ToUpper() }, StringSplitOptions.RemoveEmptyEntries);
		}

		private static bool HasOperator(string expression, string op)
		{
			var words = GetWords(expression);
			
			return words.Contains(op.ToUpper());
		}

		public static string[] GetWords(string expression)
		{
			var words = expression.Split(new char[] { '.', '?', '!', ' ', ';', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
			return words;
		}

		
	}
}