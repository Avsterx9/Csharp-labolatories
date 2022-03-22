using System;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;


namespace s4ex1___CRUD_hibernate
{
class Program
    {
        static void Main(string[] args)
        {
            var sessionFactory = CreateSessionFactory(typeof(SignalMap));

            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var s1 = new Signal() {Description = "Cannoct establish internet connection", Source = "System Windows", EventType = SignalType.ERROR};
                    var s2 = new Signal() {Description = "Not enough hard drive memory", Source = "Disk C", EventType = SignalType.WARNING};
                    var s3 = new Signal() {Description = "system update is available", Source = "System Windows", EventType = SignalType.INFORMATION};
                    var s4 = new Signal() {Description = "Graphic driver update is available", Source = "GeForceExperience", EventType = SignalType.INFORMATION};
                    var s5 = new Signal() {Description = "Program has been stopped", Source = "Adobe Premiere Pro", EventType = SignalType.CRITICAL_ERROR};
                    
                    session.Save(s1);
                    session.Save(s2);
                    session.Save(s3);
                    session.Save(s4);
                    session.Save(s5);

                    session.Delete(s2);

                    s3.Description = "System Windows update is available";
                    session.Update(s3);

                    
                    
                    transaction.Commit();
                }

                session.Query<Signal>().ToList().ForEach(e => Console.WriteLine(e));
            }

            void BuildSchema(Configuration config)
            {
                new SchemaExport(config)
                    .Create(false, true);
            }

            ISessionFactory CreateSessionFactory(params Type[] mappingTypes)
            {
                return Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard
                        .UsingFile("database.sqlite"))
                    .Mappings(m =>
                    {
                        foreach (var mappingType in mappingTypes)
                            m.FluentMappings.Add(mappingType);
                    })
                    .ExposeConfiguration(BuildSchema)
                    .BuildSessionFactory();
            }
        }
    }
    public enum SignalType
    {
        INFORMATION,
        ERROR,
        WARNING,
        CRITICAL_ERROR  
    }
    public class Signal
    {

        public Signal()
        {
            Date = DateTime.Now;
        }

        public virtual DateTime Date { get; }
        public virtual string Source { get; set; }
        public virtual SignalType EventType { get; set; }
        public virtual string Description { get; set; }
        public virtual int Id { get; protected set; }

        public override string ToString() => $"Id: {Id}, {Date}, {Source}, {EventType}, {Description}";
    }

    public class SignalMap : ClassMap<Signal>
    {
        public SignalMap()
        {
            Table("Signal");

            Id(x => x.Id)
                .GeneratedBy.Identity();
            Map(x => x.Date)
                .Length(64)
                .Not.Nullable();
            Map(x => x.Source)
                .Length(64)
                .Not.Nullable();
            Map(x => x.EventType)
                .Index("EventType")
                .Not.Nullable();
            Map(x => x.Description)
                .Length(64)
                .Not.Nullable();
        }
    }
}