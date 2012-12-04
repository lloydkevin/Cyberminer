using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace minesweepers.Models
{
	public class SearchEntry
	{
		public virtual int ID { get; protected set; }

		[Required]
		[RegularExpression(@"^https?\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$", ErrorMessage = "Please enter a valid URL")]
		public virtual string URL { get; set; }

		[Required]
		public virtual string Descriptor { get; set; }
		public virtual string[] SearchWords
		{
			get
			{
				var words = new List<string>();

				words.AddRange(ExpressionParser.GetWords(Descriptor));
				words.RemoveAll(x => ExpressionParser.NOISE_WORDS.Contains(x));

				return words.ToArray();
			}
		}

		public override bool Equals(object obj)
		{
			var entry = obj as SearchEntry;

			if (entry == null)
				return false;

			return ID.Equals(entry.ID);
		}

		public override int GetHashCode()
		{
			return this.ID.GetHashCode();
		}
	}
}