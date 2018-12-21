using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Xml;

namespace Common.DBFasade
{
    public class NhibernateSessionManager
    {
        private const string SESSION_KEY = "::_SESSION_KEY_::";
        private ISessionFactory sessionFactory;
        private IConfigurationRoot configuration;
        public NhibernateSessionManager()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                configuration = builder.Build();

                InitSessionFactory();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RollbackSession()
        {
            ISession contextSession = GetSession();
            if (contextSession != null)
            {
                if (contextSession.Transaction != null && contextSession.Transaction.IsActive)
                {
                    contextSession.Transaction.Rollback();
                }
            }
        }

        public void CloseSession()
        {
            if (CurrentSessionContext.HasBind(sessionFactory))
            {
                var session = sessionFactory.GetCurrentSession();
                var transaction = session.Transaction;
                if (transaction != null && transaction.IsActive)
                {
                    transaction.Commit();
                }
                session = CurrentSessionContext.Unbind(sessionFactory);
                session.Close();
            }

            //ISession contextSession = GetSession();
            //if ((contextSession != null) && contextSession.IsOpen)
            //{
            //    try
            //    {
            //        contextSession.Flush();
            //        if (contextSession.Transaction != null && contextSession.Transaction.IsActive)
            //        {
            //            contextSession.Transaction.Commit();
            //        }
            //    }
            //    catch
            //    {
            //        if (contextSession.Transaction != null && contextSession.Transaction.IsActive)
            //        {
            //            contextSession.Transaction.Rollback();
            //        }
            //    }
            //    finally
            //    {
            //        contextSession.Close();
            //    }
            //}
        }


        public ISession GetSession()
        {
            if (!CurrentSessionContext.HasBind(sessionFactory))
            {
                CurrentSessionContext.Bind(sessionFactory.OpenSession());
            }
            return this.sessionFactory.GetCurrentSession();

        }

        private void InitSessionFactory()
        {
            string connectionString = configuration.GetConnectionString("artDB");
            var entityMappingTypes = GetMappingAssemblies();

            string dbType = configuration.GetSection("databaseype").Value;

            if (dbType.Contains("mssql"))
            {
                sessionFactory = Fluently.Configure()
              .Database(MsSqlConfiguration.MsSql2012
              .Dialect<MsSql2012Dialect>()
              .ConnectionString(connectionString)
              .ShowSql())
              .Mappings(m => entityMappingTypes.ForEach(e =>
              {
                  m.FluentMappings.AddFromAssembly(e)
                  .Conventions.Add(Table.Is(x => x.EntityType.Name.ToLower()));
              }))
              .CurrentSessionContext("call")
              .ExposeConfiguration(BuildSchema)
              //.Conventions.Add(Table.Is(x => x.EntityType.Name.ToLower()))
              .BuildSessionFactory();                
            }
            else
            {
                sessionFactory = Fluently.Configure()
                   .Database(PostgreSQLConfiguration.Standard
                   .Dialect<PostgreSQL83Dialect>()
                   .ConnectionString(connectionString)
                   .ShowSql())
                   .Mappings(m => entityMappingTypes.ForEach(e =>
                   {
                       m.FluentMappings.AddFromAssembly(e)
                       .Conventions.Add(Table.Is(x => x.EntityType.Name.ToLower()));
                   }))
                   .CurrentSessionContext("call")
                   .ExposeConfiguration(BuildSchema)
                   //.Conventions.Add(Table.Is(x => x.EntityType.Name.ToLower()))
                   .BuildSessionFactory();
            }
        }


        List<Assembly> GetMappingAssemblies()
        {
            List<string> mappingAssembly = configuration.GetSection("mappingAssembly").Value.Split("|").ToList();
            List<Assembly> assemblies = new List<Assembly>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (mappingAssembly.Contains(assembly.GetName().Name))
                {
                    assemblies.Add(assembly);
                }
            }
            return assemblies;
        }


        private static void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            var dbSchemaExport = new SchemaUpdate(config);
            dbSchemaExport.Execute(true, true);
        }

        public static NhibernateSessionManager Instance
        {
            get
            {
                return Nested.SessionManager;
            }
        }

        private class Nested
        {
            internal static readonly NhibernateSessionManager SessionManager = new NhibernateSessionManager();
        }
    }

    public class CollectionAccessConvention : ICollectionConvention
    {
        public void Apply(ICollectionInstance instance)
        {
            instance.Access.CamelCaseField(CamelCasePrefix.Underscore);
            instance.AsSet();
        }
    }

    public class XMLTypeConvention : IUserTypeConvention
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Type == typeof(XmlDocument));
        }


        public void Apply(IPropertyInstance instance)
        {
            instance.CustomType<NHibernate.Type.XDocType>();
        }
    }
}
