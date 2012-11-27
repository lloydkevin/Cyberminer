using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;

namespace minesweepers.Models.Mappings
{
	public class SearchEntryMap : ClassMap<SearchEntry>
	{
		public SearchEntryMap()
		{
			Id(x => x.ID);
			Map(x => x.URL);
			Map(x => x.Descriptor);
		}
	}
}