using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using minesweepers.Models;

namespace minesweepers.Controllers
{
	public class EntryController : BaseController
	{
		//
		// GET: /Entry/

		public ActionResult Index()
		{
			var list = session.QueryOver<SearchEntry>().List();

			return View(list);
		}

		//
		// GET: /Entry/Details/5

		public ActionResult Details(int id)
		{
			return View();
		}

		//
		// GET: /Entry/Create

		public ActionResult Create()
		{
			return View();
		}

		//
		// POST: /Entry/Create

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
			return View();
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
