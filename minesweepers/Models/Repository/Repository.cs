using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using System.IO;
using NHibernate.Tool.hbm2ddl;
using System.Diagnostics;

namespace minesweepers.Models.Repository
{
	public class Repository
	{
		private const string DB_FILE = "~/App_Data/database.db";

		public static ISessionFactory CreateSessionFactory()
		{
			string path = HttpContext.Current.Server.MapPath(DB_FILE);

			return Fluently.Configure()
				.Database(
					SQLiteConfiguration.Standard.UsingFile(path)
					.UseReflectionOptimizer()
					.ShowSql()
				)
				
				.Mappings(m =>
					m.FluentMappings.AddFromAssemblyOf<minesweepers.MvcApplication>())
				.ExposeConfiguration(BuildSchema)
				.BuildSessionFactory();
		}

		private static void BuildSchema(Configuration config)
		{
			string path = HttpContext.Current.Server.MapPath(DB_FILE);
			config.SetInterceptor(new SqlStatementInterceptor());

			var se = new SchemaExport(config);

			// delete the existing db on each run
			if (File.Exists(path))
			{
				se.Execute(true, false, false);
			}
			else
			{
				se.Create(true, true);
			}

			// this NHibernate tool takes a configuration (with mapping info in)
			// and exports a database schema from it
			//new SchemaExport(config)
			//  .Execute(true, false, false);
				//.Create(true, true);
		}

	}

	public class SqlStatementInterceptor : EmptyInterceptor
	{
		public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
		{
			Debug.WriteLine(sql.ToString());
			return sql;
		}


	}


}