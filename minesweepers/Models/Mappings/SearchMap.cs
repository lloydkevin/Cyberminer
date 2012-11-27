using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;

namespace minesweepers.Models.Mappings
{
	public class SearchMap : ClassMap<Search>
	{
		public SearchMap()
		{
			Id(x => x.ID);
			Map(x => x.Query);
			Map(x => x.Frequency);
			Map(x => x.Operator);
		}
	}
}