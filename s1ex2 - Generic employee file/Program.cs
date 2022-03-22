using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ex2___Generic_employee_file
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            CONFIGURATION MANAGER
            */
            
            // var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // var settings = config.AppSettings.Settings;
            // settings.Remove("shift");
            // settings.Add("shift", "2"); //=> liczba przesuniec
            // config.Save(ConfigurationSaveMode.Modified);
            // ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);

            EmployeeFile<Employee> employeeFile = new EmployeeFile<Employee>(new JSONManager<Employee>());

            // employeeFile.AddEmployee(new Employee(1, "Kamil", "Chrobok", "IT", "Oswiecim"));
            // employeeFile.AddEmployee(new Employee(2, "Krystian", "Chrobok", "IT", "Oswiecim"));
            // employeeFile.AddEmployee(new Employee(3, "Tomasz", "Jakis", "Produkcja", "Zator"));
            // employeeFile.AddEmployee(new Employee(4, "Kacper", "Nazwisko", "Management", "Katowice"));

            //employeeFile.ShowDatabase();

            //employeeFile.SaveFile("D:/test.txt");
            //employeeFile.ReadFile("D:/test.txt");
            //employeeFile.ShowDatabase();

            //employeeFile.SaveEncryptedFile("D:/test.txt");
            // employeeFile.ReadEncryptedFile("D:/test.txt");
            // employeeFile.ShowDatabase();

            // employeeFile.SaveFile("D:/testjson.json");
            // employeeFile.ReadFile("D:/testjson.json");

            //employeeFile.SaveEncryptedFile("D:/testjson.json");
            //employeeFile.ReadEncryptedFile("D:/testjson.json");

            employeeFile.ShowDatabase();
        }
    }
}

public class EmployeeFile<T> where T: Employee{

    private List<T> employeeList;

    private IFileManager<T> iFileManager;

    public EmployeeFile(IFileManager<T> iFileManager){
        employeeList = new List<T>();
        this.iFileManager = iFileManager;
    }

    public void AddEmployee(T employee){
        if(SearchById(employee.id)){
            Console.WriteLine($"Pracownik z id {employee.id} już istnieje");
            return;
        } else{
            if(employee.Validate()){
                employeeList.Add(employee); 
            } else{
                return;
            }
        }
    }

    public void DeleteEmployeeById(int employeeId){
        var employee = employeeList.SingleOrDefault(x => x.id == employeeId);
        if(employee != null){
            employeeList.Remove(employee);
            Console.WriteLine($"[DELETE] Znaleziono i usunięto pracownika: {employee.name} {employee.surname} o indeksie {employeeId}");
        } else{
            Console.WriteLine($"[DELETE] Nie znaleziono obiektu o indeksie {employeeId}");
        }
    }

    public void ShowDatabase(){
        foreach(T employee in employeeList){
            employee.Show();
        }
    }

    public bool SearchById(int employeeId){
        var employee = employeeList.SingleOrDefault(x => x.id == employeeId);
        return (employee!=null) ? true:false;
    }

    public void SaveFile(string filename){
        iFileManager.Save(employeeList, filename);
    }

    public void ReadFile(string filename){
        iFileManager.Read(employeeList, filename);
    }

    public void SaveEncryptedFile(string filename){
        iFileManager.SaveEncrypted(employeeList, filename);
    }

    public void ReadEncryptedFile(string filename){
        iFileManager.ReadEncrypted(employeeList, filename);
    }
}

public class Employee{

    public int id {get;set;}
    public string name {get;set;}
    public string surname{get;set;}
    public string workplace{get;set;}
    public string city{get;set;}

    public Employee(){

    }

    public Employee(int id, string name, string surname, string workplace, string city){
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.workplace = workplace;
            this.city = city;
    }

    public Boolean IsMatch(int searchId){
        if(searchId == id){
            return true;
        } else{
            return false;
        }
    }

    public void Show(){
        Console.WriteLine($"ID: {id} | name: {name} | surname: {surname} | workplace: {workplace} | city: {city}");
    }

    public bool Validate(){
        Regex regex = new Regex("^[A-Za-z]{3,}$");

        if(!regex.IsMatch(name)){
            Console.WriteLine($"Invalid name [{name}]");
            return false; 
        }

        if(!regex.IsMatch(surname)){
            Console.WriteLine($"Invalid name [{surname}]");
            return false; 
        }

        if(!regex.IsMatch(city)){
            Console.WriteLine($"Invalid name [{city}]");
            return false; 
        }
        return true;
    }
}

public class CaesarCipher{

    private int shift = Int32.Parse(ConfigurationManager.AppSettings["shift"]);

    public string Encrypt(string word){
        string shiftedWord = string.Empty;
    
        for(int i = 0;i < word.Length; i++){
            char c = word[i];
            c = (char)(c + shift);
            if(c > 'Z' & c < 'a'){
                c = (char)(c - 26);
            }
            if(c > 'z'){
                c = (char)(c - 26);
            }

            shiftedWord += c;
        }
        return shiftedWord;
    }

    public string Decrypt(string word){
        string shiftedWord = string.Empty;
    
        for(int i = 0;i < word.Length; i++){
            char c = word[i];
            c = (char)(c - shift);
            if(c < 'A'){
                c = (char)(c + 26);
            }
            if(c < 'a' & c > 'Z'){
                c = (char)(c + 26);
            }
            shiftedWord += c;
        }
        return shiftedWord;
    }
}

//Builder
public interface IFileManager<T>{
    void Save(List<T> employees, string filename);
    void Read(List<T> employees, string filename);
    void SaveEncrypted(List<T> employees, string filename);
    void ReadEncrypted(List<T> employees, string filename);
}


//Zapisywanie w TXT

public class TXTManager<T> : IFileManager<T> where T: Employee{

    private CaesarCipher caesarCipher = new CaesarCipher();

    public void Save(List<T> employees, string filename){
        using(var outFile = new StreamWriter(filename)){
            foreach(T e in employees){
                outFile.WriteLine($"{e.id} {e.name} {e.surname} {e.workplace} {e.city}");
            }
        }
    }

    public void Read(List<T> employees, string filename){
        using(var file = new StreamReader(filename)){
            string line;

            string[] emp;
            while((line = file.ReadLine()) != null){
                emp = line.Split(' ');
                Employee employee = new Employee(Int32.Parse(emp[0]), emp[1], emp[2], emp[3], emp[4]);

                bool containsItem = employees.Any(em => em.id == employee.id);
                if(!containsItem){
                    employees.Add((T)employee);
                }
            }
        }
    }
    public void SaveEncrypted(List<T> employees, string filename){
        using(var outFile = new StreamWriter(filename)){
            foreach(T e in employees){
                string encryptedName = caesarCipher.Encrypt(e.name);
                string encryptedSurname = caesarCipher.Encrypt(e.surname);
                string encryptedWorkplace = caesarCipher.Encrypt(e.workplace);
                string encryptedCity = caesarCipher.Encrypt(e.city);
                outFile.WriteLine($"{e.id} {encryptedName} {encryptedSurname} {encryptedWorkplace} {encryptedCity}");
            }
        }
    }

    public void ReadEncrypted(List<T> employees, string filename){
        using(var file = new StreamReader(filename)){
            string line;

            string[] emp;
            while((line = file.ReadLine()) != null){
                emp = line.Split(' ');
                Employee employee = new Employee(Int32.Parse(emp[0]), 
                    caesarCipher.Decrypt(emp[1]), 
                    caesarCipher.Decrypt(emp[2]), 
                    caesarCipher.Decrypt(emp[3]), 
                    caesarCipher.Decrypt(emp[4]));

                bool containsItem = employees.Any(em => em.id == employee.id); //sprawdz czy istnieje
                if(!containsItem){
                    employees.Add((T)employee);
                }
            }
        }
    }
}

//TODO => zaimplementować zapisywanie w XML i JSON

public class JSONManager<T> : IFileManager<T> where T: Employee{
    public void Save(List<T> employees, string filename){
        string json = System.Text.Json.JsonSerializer.Serialize(employees);
        File.WriteAllText(filename, json);
    }
    public void Read(List<T> employees, string filename){
        string json = File.ReadAllText(filename);
        var empList = JsonConvert.DeserializeObject<List<Employee>>(json);
        
        foreach(Employee employee in empList){
            bool containsItem = employees.Any(em => em.id == employee.id);
            if(!containsItem){
                employees.Add((T)employee);
            }
        }
    }

    public void SaveEncrypted(List<T> employees, string filename){

        var caesarCipher = new CaesarCipher();
        var encryptedEmployees = new List<Employee>();


        foreach(Employee e in employees){
            string encryptedName = caesarCipher.Encrypt(e.name);
            string encryptedSurname = caesarCipher.Encrypt(e.surname);
            string encryptedWorkplace = caesarCipher.Encrypt(e.workplace);
            string encryptedCity = caesarCipher.Encrypt(e.city);
            Employee encryptedEmployee = new Employee(e.id, encryptedName, encryptedSurname, encryptedWorkplace, encryptedCity);
            encryptedEmployees.Add(encryptedEmployee);
        }

        string json = System.Text.Json.JsonSerializer.Serialize(encryptedEmployees);
        File.WriteAllText(filename, json);
    }

    public void ReadEncrypted(List<T> employees, string filename){
        string json = File.ReadAllText(filename);
        var empList = JsonConvert.DeserializeObject<List<Employee>>(json);
        var caesarCipher = new CaesarCipher();
        
        foreach(Employee employee in empList){
            string decryptedName = caesarCipher.Decrypt(employee.name);
            string decryptedSurname = caesarCipher.Decrypt(employee.surname);
            string decryptedWorkplace = caesarCipher.Decrypt(employee.workplace);
            string decryptedCity = caesarCipher.Decrypt(employee.city);

            Employee decryptedEmployee = new Employee(employee.id, decryptedName, decryptedSurname, decryptedWorkplace, decryptedCity);

            bool containsItem = employees.Any(em => em.id == decryptedEmployee.id);
            if(!containsItem){
                employees.Add((T)decryptedEmployee);
            }
        }
    }
}