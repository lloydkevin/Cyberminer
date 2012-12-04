using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace minesweepers.Models
{
	public enum SearchOperator
	{
		AND,
		OR,
		NOT
	}


	public class Search
	{
		public virtual int ID { get; protected set; }

		public Search()
		{
			ResultsPerPage = 1;
		}

		[DisplayNameAttribute("Search")]
		public virtual string Query { get; set; }
		public virtual int Frequency { get; set; }
		[DisplayName("Results per Page")]
		public virtual int ResultsPerPage { get; set; }
		
		
		//public Search()
		//{
		//  ID = Guid.NewGuid();
		//}
	}
}