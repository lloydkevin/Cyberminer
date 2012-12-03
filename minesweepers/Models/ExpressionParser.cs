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

		public ExpressionParser(ISession session)
		{
			this.session = session;
		}

		public IList<SearchEntry> Evaluate(string expression)
		{
			expression = expression.Trim();
			var list = new List<SearchEntry>();
			if (HasOperator(expression, "or"))
			{
				var parts = GetParts(expression, "or");
				foreach (var part in parts)
				{
					var results = Evaluate(part);
					list.AddRange(results);
				}
			}

			else if (HasOperator(expression, "and"))
			{
				var parts = GetParts(expression, "and");
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

			else if (HasOperator(expression, "not"))
			{
				var parts = GetParts(expression, "not");

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
					entries.RemoveAll(x => !x.Descriptor.Contains(word));
					list.AddRange(entries);
				}
			
			}

			//return session.QueryOver<SearchEntry>().WhereRestrictionOn(x => x.Descriptor).IsLike(expression, MatchMode.Anywhere).List();
			return list;
		}

		private static string[] GetParts(string expression, string op)
		{
			
			return expression.Split(new string[] { op.ToLower() }, StringSplitOptions.RemoveEmptyEntries);
		}

		private static bool HasOperator(string expression, string op)
		{
			var words = GetWords(expression).Select(x => x.ToLower());
			
			return words.Contains(op.ToLower());
		}

		private static string[] GetWords(string expression)
		{
			var words = expression.Split(new char[] { '.', '?', '!', ' ', ';', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
			return words;
		}

		
	}
}