using System;
using System.Collections.Generic;
using System.Linq;

namespace ex3___LINQ_operations
{
    class Program
    {
        static void Main(string[] args)
        {
            var students = new List<Student>()
            {
                new Student()
                    {indexNumber = 43255, age = 22, semester = 5, sex = "male", yearOfStudy = 3, studentClass = "IO2"},
                new Student()
                    {indexNumber = 43523, age = 20, semester = 3, sex = "female", yearOfStudy = 2, studentClass = "D7"},
                new Student()
                    {indexNumber = 43777, age = 19, semester = 2, sex = "male", yearOfStudy = 1, studentClass = "C6"},
                new Student()
                {
                    indexNumber = 43992, age = 22, semester = 5, sex = "female", yearOfStudy = 3, studentClass = "IO2"
                },
                new Student()
                {
                    indexNumber = 66542, age = 25, semester = 5, sex = "female", yearOfStudy = 3, studentClass = "IO2"
                },
                new Student()
                    {indexNumber = 64521, age = 21, semester = 5, sex = "female", yearOfStudy = 3, studentClass = "IO2"}
            };

            var degrees = new List<Degree>()
            {
                new Degree() {subject = "NJPO", semester = 5, grade = 4, yearOfGraduation = 3, indexNumber = 43255},
                new Degree() {subject = "NJPO", semester = 5, grade = 5, yearOfGraduation = 3, indexNumber = 43992},
                new Degree() {subject = "NJPO", semester = 5, grade = 2, yearOfGraduation = 3, indexNumber = 66542},
                new Degree() {subject = "NJPO", semester = 5, grade = 4, yearOfGraduation = 3, indexNumber = 64521},

                new Degree() {subject = "AMIW", semester = 5, grade = 5, yearOfGraduation = 3, indexNumber = 43255},
                new Degree() {subject = "AMIW", semester = 5, grade = 3, yearOfGraduation = 3, indexNumber = 43992},
                new Degree() {subject = "AMIW", semester = 5, grade = 3, yearOfGraduation = 3, indexNumber = 66542},
                new Degree() {subject = "AMIW", semester = 5, grade = 4, yearOfGraduation = 3, indexNumber = 64521},

                new Degree() {subject = "AK", semester = 3, grade = 5, yearOfGraduation = 2, indexNumber = 43523},

                new Degree() {subject = "PTC", semester = 2, grade = 4, yearOfGraduation = 2, indexNumber = 43777}
            };

            var q =
                from s in students
                join d in degrees
                    on s.indexNumber equals d.indexNumber into degreeTab
                select new
                {
                    Student = s,
                    Degree = degreeTab
                };

            foreach (var v in q)
            {
                Console.WriteLine("StudentIndex " + v.Student.indexNumber + ":");
                foreach (var p in v.Degree)
                {
                    Console.WriteLine("\t" + p.subject + " | " + p.grade);
                }
            }

            Console.WriteLine("\n==========================\n");

            var avgAge =
                from student in students
                let avgAgeOnYear = students.Where(x => student.yearOfStudy == x.yearOfStudy)
                    .Average(x => x.age)
                where student.age > avgAgeOnYear
                select student.indexNumber;

            foreach (var c in avgAge)
            {
                Console.WriteLine(c);
            }

            Console.WriteLine("\n==========================\n");

            var avgGrade =
                from student in q
                let avgGradeOnYear = q.Where(x => x.Student.yearOfStudy == student.Student.yearOfStudy)
                    .Average(x => x.Degree.Average(z => z.grade))
                let avgGradeForStudent = student.Degree.Average(x => x.grade)
                where avgGradeForStudent > avgGradeOnYear
                select student.Student.indexNumber;

            foreach (var s in avgGrade)
            {
                Console.WriteLine(s);
            }
        }
    }
}

public class Student
{
    public int indexNumber { get; set; }
    public int age { get; set; }
    public string sex { get; set; }
    public int yearOfStudy { get; set; }
    public int semester { get; set; }
    public string studentClass { get; set; }

    public Student()
    {
    }
}

public class Degree
{
    public string subject { get; set; }
    public int grade { get; set; }
    public int yearOfGraduation { get; set; }

    public int semester { get; set; }

    //indexNumber dodajemy w celu lepszego połączenia klas
    public int indexNumber { get; set; }

    public Degree()
    {
    }
}