using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace s4ex2___CRUD_hibernate_with_database
{
class Program
    {
        static void Main(string[] args)
        {
            var sessionFactory = CreateSessionFactory(typeof(EmployeeFileMap), typeof(EmployeeMap));
            Employee e1 = new Employee("Kamil", "Chrobok", "IT", "Oświęcim");
            Employee e2 = new Employee("Jan", "Kowalski", "IT", "Babice");
            Employee e3 = new Employee("Ktos", "Jakis", "Marketing", "Katowice");

            List<Employee> list= new List<Employee>()
            {
                e1, e2, e3
            };

            using(var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    EmployeeFile employeeFile = new EmployeeFile()
                    {
                        Name = "EmployeeFile",
                        Employees = list
                    };
                    session.Save(employeeFile);
                    transaction.Commit();
                }
                
                session.Query<EmployeeFile>().ToList().ForEach(e => Console.WriteLine(e));
            }
        }

        static void BuildSchema(Configuration config)
        {
            new SchemaExport(config)
                .Create(false, true);
        }

        static ISessionFactory CreateSessionFactory(params Type[] mappingTypes)
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

    public class Employee
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Workplace { get; set; }

        public virtual string City { get; set; }

        public virtual EmployeeFile EmployeeList { get; set; }
        
        public Employee()
        {
            
        }
        public Employee(string Name, string Surname, string Workplace, string City)
        {
            if (Validate(Name) && Validate(Surname) && Validate(City))
            {
                this.Name = Name;
                this.Surname = Surname;
                this.Workplace = Workplace;
                this.City = City;
            }
        }

        //Show
        public override string ToString()
        {
            return $"ID: {Id} | name: {Name} | surname: {Surname} | workplace: {Workplace} | city: {City}";
        }

        public virtual bool Validate(string text)
        {
            Regex regex = new Regex("^[A-Za-z]{3,}$");

            if(!regex.IsMatch(text)){
                //Console.WriteLine($"Invalid data [{text}]");
                return false; 
            }
            return true;
        }
        
        public virtual bool IsMatch(int searchId){
            if(searchId == Id){
                return true;
            }
            return false;
        }
    }

    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table("Employee");
            Id(x => x.Id)
                .GeneratedBy.Identity();
            Map(x => x.Name)
                .Length(64)
                .Not.Nullable();
            Map(x => x.Surname)
                .Length(64)
                .Not.Nullable();
            Map(x => x.Workplace)
                .Length(64)
                .Not.Nullable();
            Map(x => x.City)
                .Length(64)
                .Not.Nullable();
            References(x => x.EmployeeList)
                .LazyLoad();
        }
    }
    
    public class EmployeeFile
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }

        public virtual IList<Employee> Employees { get; set; }

        public override string ToString()
        {
            return $"ID: {Id} | name: {Name} | employees : {Employees}";
        }
    }
    
    public class EmployeeFileMap : ClassMap<EmployeeFile>
    {
        public EmployeeFileMap()
        {
            Table("EmployeeFile");
            Id(x => x.Id)
                .GeneratedBy.Identity();
            
            Map(x => x.Name)
                .Length(64);

            HasMany(x => x.Employees)            
                .Inverse();
        }
    }
}