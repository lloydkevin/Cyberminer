using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
		public virtual string Query { get; set; }
		public virtual int Frequency { get; set; }
		public virtual SearchOperator Operator { get; set; }
		
		//public Search()
		//{
		//  ID = Guid.NewGuid();
		//}
	}
}