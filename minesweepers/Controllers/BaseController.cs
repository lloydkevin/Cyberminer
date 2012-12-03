using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using minesweepers.Models;

namespace minesweepers.Controllers
{
	public abstract class BaseController : Controller
	{
		protected ISessionFactory sessionFactory = nHibernateSetup.CreateSessionFactory();
		protected ISession session;

		public BaseController()
		{
			session = sessionFactory.OpenSession();
		}

		protected override void Dispose(bool disposing)
		{
			session.Dispose();
			sessionFactory.Dispose();
			base.Dispose(disposing);
		}
	}
}
