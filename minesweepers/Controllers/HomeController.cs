using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using minesweepers.Models;
using System.Diagnostics;
using NHibernate.Criterion;
using NHibernate.Linq;
using minesweepers.Models.ViewModel;
using PagedList;

namespace minesweepers.Controllers
{
	public class HomeController : BaseController
	{
		//
		// GET: /Home/

		public ActionResult Index()
		{
			HttpContext.Cache.Remove("entries");
			HttpContext.Cache.Remove("query");

			return View(new ResultsViewModel());
		}

		[HttpGet]
		public ActionResult Results(ResultsViewModel results, int? page)
		{
			if (ModelState.IsValid)
			{
				var query = HttpContext.Cache["query"] as string;
				if (query != null && query != results.Search.Query)
				{
					HttpContext.Cache.Remove("entries");
					HttpContext.Cache.Remove("query");
				}

				var entries = System.Web.HttpContext.Current.Cache["entries"] as List<SearchEntry>;

				if (entries == null)
				{
					IncrementFrequency(results.Search);				
					var parser = new ExpressionParser(session);
					entries = parser.Evaluate(results.Search.Query).Distinct().ToList();
					HttpContext.Cache["entries"] = entries;
					HttpContext.Cache["query"] = results.Search.Query;
				}

				results.Results = entries;
				int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
				var pageNumber = page ?? 1;
				results.PagedResults = entries.ToPagedList(pageNumber, results.Search.ResultsPerPage);

				return View("Results", results);
			}

			// for validation
			return View("Index");
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

			var list = session.Query<Search>()
				.Where(x => x.Query.StartsWith(term))
				.OrderByDescending(x => x.Frequency)
				.Take(10)
				.ToList();


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
