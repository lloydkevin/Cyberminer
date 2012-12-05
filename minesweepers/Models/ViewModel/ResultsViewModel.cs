using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace minesweepers.Models.ViewModel
{
	public class ResultsViewModel
	{
		public Search Search { get; set; }
		public IEnumerable<SearchEntry> Results { get; set; }
		public IPagedList<SearchEntry> PagedResults { get; set; }

		public string QueryWords
		{
			get
			{
				var words = ExpressionParser.GetWords(Search.Query);
				return string.Join(" ", words);
			}
		}

		public ResultsViewModel()
		{
			Search = new Search();
		}
	}
}