using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace minesweepers.Models
{
	 

	public class ExpressionTreeNode<T>
	{
		public ExpressionTreeNode<T> Left { get; set; }
		public ExpressionTreeNode<T> Right { get; set; }
		public ExpressionTreeNode<T> Parent { get; set; }
		public T Expression { get; set; }
		public SearchOperator Operator { get; set; }

	}
}