using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace minesweepers.Models
{
	public class SearchEntry
	{
		public virtual int ID { get; protected set; }
		public virtual string URL { get; set; }
		public virtual string Descriptor { get; set; }

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