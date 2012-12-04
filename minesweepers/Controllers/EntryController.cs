using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using minesweepers.Models;
using NHibernate.Linq;
using PagedList;

namespace minesweepers.Controllers
{
	public class EntryController : BaseController
	{
		public ActionResult Index(int? page)
		{
			var pageIndex = (page ?? 1) - 1; //MembershipProvider expects a 0 for the first page
			var pageSize = 2;
			var pageNumber = page ?? 1;

			var paged = session.Query<SearchEntry>();

			var list = session.Query<SearchEntry>().ToPagedList(pageNumber, pageSize);

			return View(list);
		}

		public ActionResult Details(int id)
		{
			return View();
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(SearchEntry entry)
		{
			try
			{
				if (ModelState.IsValid)
				{
					using (var trans = session.BeginTransaction())
					{
						session.SaveOrUpdate(entry);
						trans.Commit();
					}
				}

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		//
		// GET: /Entry/Edit/5

		public ActionResult Edit(int id)
		{
			var entry = session.Get<SearchEntry>(id);
			return View(entry);
		}

		//
		// POST: /Entry/Edit/5

		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				if (ModelState.IsValid)
				{
					using (var trans = session.BeginTransaction())
					{
						var entry = session.Get<SearchEntry>(id);
						if (TryUpdateModel(entry))
						{
							session.SaveOrUpdate(entry);
							trans.Commit();
						}
					}
				}

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		//
		// GET: /Entry/Delete/5

		public ActionResult Delete(int id)
		{
			var entry = session.Get<SearchEntry>(id);
			return View(entry);
		}

		//
		// POST: /Entry/Delete/5

		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			try
			{
				if (ModelState.IsValid)
				{
					using (var trans = session.BeginTransaction())
					{
						var entry = session.Get<SearchEntry>(id);
						session.Delete(entry);
						trans.Commit();
					}
				}

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}
