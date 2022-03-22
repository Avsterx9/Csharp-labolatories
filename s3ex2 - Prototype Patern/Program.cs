using System;
using System.Collections.Generic;

namespace s3ex2___Prototype_Patern
{
    class Program
    {
        static void Main(string[] args)
        {
            CarManager carManager = new CarManager();

            //standardowe
            carManager["Audi"] = new Car("Audi", "1998", "a4");
            carManager["Toyota"] = new Car("Toyota", "2011", "Avensis");
            carManager["BMW"] = new Car("BMW", "1998", "e39");

            //nowe
            carManager["dreamCar"] = new Car("MyCompany", "2022", "MyModel");

            Car car1 = carManager["Audi"].Clone() as Car;

        }
    }

    public abstract class CarPrototype
    {
        public abstract CarPrototype Clone();
    }

    public class Car : CarPrototype
    {
        string make;
        string yearOfproduction;
        string model;

        public Car(string make, string yearOfproduction, string model)
        {
            this.make = make;
            this.yearOfproduction = yearOfproduction;
            this.model = model;
        }

        public override CarPrototype Clone()
        {
            Console.WriteLine($"Colonning object {make} {yearOfproduction} {model}");

            return this.MemberwiseClone() as CarPrototype;
        }
    }

    public class CarManager
    {
        private Dictionary<string, CarPrototype> cars = new Dictionary<string, CarPrototype>();

        public CarPrototype this[string key]
        {
            get { return cars[key]; }
            set { cars.Add(key, value); }
        }
    }
}