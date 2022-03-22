using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace s4ex3___Error_logging_class_with_dynamic_database
{
    class Program
    {
        static string LOCAL_FB_NAME = "local_database.sqlite";
        static string MAIN_FB_NAME = "main_database.sqlite";

        static void Main(string[] args)
        {
            List<Error> errors = new List<Error>()
            {
                new Error()
                {
                    ErrorType = ErrorType.HARDWARE_WARNING,
                    Description = "Brakuje miejsca na dysku twardym",
                    Source = "Dysk twardy"
                },
                new Error()
                {
                    ErrorType = ErrorType.SOFTWARE_CRITICAL,
                    Description = "Program VisualStudioCode przestał działać",
                    Source = "VisualStudioCode.exe"
                },
                new Error()
                {
                    ErrorType = ErrorType.SOFTWARE_WARNING,
                    Description = "Znaleziono przestarzałe sterowniki Java",
                    Source = "Java.exe"
                }
            };
            
            // 1. ZAPISYWANIE TYLKO DO GLOWNEJ BAZY DANYCH
            DatabaseManager databaseManager = new DatabaseManager();
            databaseManager.SavaData(errors);
            
            // 2. ZAPISYWANIE TYLKO DO LOKALNEJ BAZY DANYCH
            // DatabaseManager databaseManager = new DatabaseManager();
            // databaseManager.CloseMainSession();
            // databaseManager.SavaData(errors);
            
            // 3. MIGRACJA Z BAZY LOKALNEJ DO GŁOWNEJ I USUNIĘCIE LOKALNEJ BAZY
            // DatabaseManager databaseManager = new DatabaseManager();
            // databaseManager.SavaData(errors);
        }
}

        {
            private ISessionFactory mainSessionFactory;
            private ISession mainSession;

            public DatabaseManager()
            {
                if (File.Exists(MAIN_FB_NAME))
                    mainSessionFactory = UpdateSessionFactory(MAIN_FB_NAME, typeof(ErrorMap));
                else 
                    mainSessionFactory = CreateSessionFactory(MAIN_FB_NAME, typeof(ErrorMap));
                mainSession = mainSessionFactory.OpenSession();
            }

            public void CloseMainSession()
            {
                mainSession.Close();
                mainSessionFactory.Close();
            }

            public void SavaData(List<Error> errors)
            {
                if (mainSession.IsOpen) 
                {
                    var errorsToSave = errors;
                    
                    if (File.Exists(LOCAL_FB_NAME)) 
                    {
                        Console.WriteLine("LOCAL DATABASE FOUND -> MOVING DATA FROM LOCAL DB AND SAVING TO MAIN DB");
                        
                        List<Error> localErrors = GetDataFromLocalDatabase();
                        errorsToSave.AddRange(localErrors);
                    }
                    SaveToDatabase(mainSession,errorsToSave);
                    
                    mainSession.Close();
                    mainSessionFactory.Close();
                }
                
                else
                {
                    ISession localSession;
                    ISessionFactory localSessionFactory;
                    
                    if (File.Exists(LOCAL_FB_NAME))
                    {
                        Console.WriteLine("CANNOT ACCESS MAIN SESSION... SWITCHING TO LOCAL DATABASE");
                        
                        localSessionFactory = UpdateSessionFactory(LOCAL_FB_NAME, typeof(ErrorMap));
                    }
                    else
                    {
                        localSessionFactory = CreateSessionFactory(LOCAL_FB_NAME, typeof(ErrorMap));
                    }
                    localSession = localSessionFactory.OpenSession();
                    SaveToDatabase(localSession, errors);

                    Console.WriteLine("== DATA SUCCESSFULLY SAVED TO MAIN DATABASE ==");
                    
                    localSession.Close();
                    localSessionFactory.Close();
                }
            }

            private List<Error> GetDataFromLocalDatabase()
            {
                var localSessionFactory = UpdateSessionFactory(LOCAL_FB_NAME, typeof(ErrorMap));
                var localSession = localSessionFactory.OpenSession();
                var localData = localSession.Query<Error>().ToList();
                
                localData.ForEach(e => Console.WriteLine(e));
                
                localSession.Close();
                File.Delete(LOCAL_FB_NAME);
                return localData;
            }

            private void SaveToDatabase(ISession session,List<Error> errors)
            {
                var transaction = session.BeginTransaction();
                
                errors.ForEach(e => session.Save(e));
                transaction.Commit();
            }
        }

        static void BuildSchema(Configuration config)
        {
            new SchemaExport(config)
                .Create(true, true);
        }

        static ISessionFactory CreateSessionFactory(string DatabaseName, params Type[] mappingTypes)
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard
                    .UsingFile(DatabaseName))
                .Mappings(m =>
                {
                    foreach (var mappingType in mappingTypes)
                        m.FluentMappings.Add(mappingType);
                })
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }
        
        static void UpdateSchema(Configuration config)
        {
            new SchemaExport(config)
                .Create(true, false);
        }

        static ISessionFactory UpdateSessionFactory(string DatabaseName, params Type[] mappingTypes)
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard
                    .UsingFile(DatabaseName))
                .Mappings(m =>
                {
                    foreach (var mappingType in mappingTypes)
                        m.FluentMappings.Add(mappingType);
                })
                .ExposeConfiguration(UpdateSchema)
                .BuildSessionFactory();
        }

        public enum ErrorType
        {
            HARDWARE_WARNING,
            HARDWARE_CRITICAL,
            SOFTWARE_WARNING,
            SOFTWARE_CRITICAL,
        }

        public class Error
        {
            public virtual int Id { get; protected set; }
            public virtual ErrorType ErrorType { get; set; }
            public virtual string Source { get; set; }
            public virtual string Description { get; set; }


            public override string ToString() => $"Id: {Id}, {Source}, {ErrorType}, {Description}";
        }

        public class ErrorMap : ClassMap<Error>
        {
            public ErrorMap()
            {
                Table("Errors");

                Id(x => x.Id)
                    .GeneratedBy.Identity();

                Map(x => x.ErrorType)
                    .Length(64)
                    .Not.Nullable();
                Map(x => x.Source)
                    .Length(64)
                    .Not.Nullable();
                Map(x => x.Description)
                    .Length(64)
                    .Not.Nullable();
            }
        }
    }
}