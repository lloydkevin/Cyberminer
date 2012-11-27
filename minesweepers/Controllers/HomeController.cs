using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using minesweepers.Models;
using System.Diagnostics;
using NHibernate.Criterion;

namespace minesweepers.Controllers
{
	public class HomeController : BaseController
	{
		//
		// GET: /Home/

		public ActionResult Index()
		{
			using (var trans = session.BeginTransaction())
			{
				var list = session.QueryOver<Search>().List<Search>();

				foreach (var item in list)
				{
					Debug.WriteLine(item.ID);
				}
				
			}

			return View();
		}

		[HttpPost]
		public ActionResult Index(Search search)
		{
			if (ModelState.IsValid)
			{
				IncrementFrequency(search);

				var entries = new List<SearchEntry>();
				var parser = new ExpressionParser(session);
				entries = parser.Evaluate(search.Query).Distinct().ToList();



				return View("Results", entries);
			}

			return View();
		}


		private void IncrementFrequency(Search search)
		{
			using (var trans = session.BeginTransaction())
			{
				var list = session.QueryOver<Search>().Where(x => x.Query == search.Query).List();
				var item = list.FirstOrDefault();

				if (item != null)
				{
					search = item;
					search.Frequency++;
				}

				session.SaveOrUpdate(search);
				trans.Commit();
			}
		}

		//
		// GET: /Home/Details/5

		public ActionResult Details(int id)
		{
			return View();
		}

		//
		// GET: /Home/Create

		public ActionResult Create()
		{
			return View();
		}

		//
		// POST: /Home/Create

		[HttpPost]
		public ActionResult Create(FormCollection collection)
		{
			try
			{
				// TODO: Add insert logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		//
		// GET: /Home/Edit/5

		public ActionResult Edit(int id)
		{
			return View();
		}

		[HttpGet]
		public JsonResult AutoComplete(string term)
		{

			var entries = new[] { new { label = "", value = "" } }.ToList();
			entries.Clear();

			var list = session.QueryOver<Search>()
				//.WhereRestrictionOn(x => x.Query).IsLike(term, MatchMode.Start)
				.OrderBy(x => x.Frequency).Desc
				.Take(100)
				.List();

			
			foreach (var item in list)
			{
				if (item.Query.StartsWith(term, StringComparison.CurrentCulture))
					entries.Add(new { label = item.Query, value = item.Query });
			}

			return this.Json(entries, JsonRequestBehavior.AllowGet);
		}

		//
		// POST: /Home/Edit/5

		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add update logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		//
		// GET: /Home/Delete/5

		public ActionResult Delete(int id)
		{
			return View();
		}

		//
		// POST: /Home/Delete/5

		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add delete logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}
