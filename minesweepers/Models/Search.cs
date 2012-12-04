using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace minesweepers.Models
{
	public enum SearchOperator
	{
		AND,
		OR,
		NOT
	}

	public class OperatorValidation : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			var val = value as string;
			if (val != null)
			{
				return !val.StartsWith("NOT", StringComparison.CurrentCulture);
			}
			return true;
		}
	}

	public class Search
	{
		public virtual int ID { get; protected set; }

		public Search()
		{
			ResultsPerPage = 5;
		}

		[Required]
		[DisplayNameAttribute("Search")]
		[OperatorValidation(ErrorMessage="Search query cannot start with NOT operator")]
		public virtual string Query { get; set; }

		
		[Required]
		[Range(1, 100)]
		[DisplayName("Results per Page")]
		public virtual int ResultsPerPage { get; set; }

		public virtual int Frequency { get; set; }
		
		
		
		//public Search()
		//{
		//  ID = Guid.NewGuid();
		//}
	}
}