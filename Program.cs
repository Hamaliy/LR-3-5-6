using System;

namespace ConsoleApp1
{
    public delegate double DelegateDivide();

    interface IMultiply
    {
        double Multiply();
    }

    interface IDivideByThirty
    {
        double DivideByThirty();
    }

    public enum BodyType
    {
        Sedan,
        Hatchback,
        Sportcar,
        Minivan
    }

    class Parameter
    {
        //public delegate void Delegate(string s);
        public event Action <string> OutOfFuel;
        
        public double mileage;
        public double fuel;
        public int seats;
        public double Mileage
        {
            get
            {
                return mileage;
            }
            set
            {
                if (value < 0)
                {
                    mileage = 0;
                }
                else
                {
                    mileage = value;
                }
            }
        }
        public double Fuel
        {
            get
            {
                return fuel;
            }
            set
            { 
                if (value <= 0 && OutOfFuel != null)
                {
                    OutOfFuel($"A car is out of fuel");
                }
                fuel = value;
            }
        }
        public int Seats
        {
            get
            {
                return seats;
            }
            set
            {
                seats = value;
            }
        }

        public Parameter(double mileage, double fuel, int seats)
        {
            this.mileage = mileage;
            this.fuel = fuel;
            try
            {
                if (seats < 1)
                {
                    throw new Exception("The amount of seats cannot be less than 1");
                }
                this.seats = seats;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                this.seats = 1;
            }
        }
    }

    class Vehicle : IComparable<Vehicle>, IDivideByThirty, IEquatable<Vehicle>, IMultiply
    {
        public static event Action <string> DBT;

        protected static int counter = 1;

        public Parameter parameter;
        private int id;
        private BodyType body;

        public BodyType Body
        {
            get { return body; }
            set { body = value; }
        }

        public int Id
        {
            get
            {
                return id;
            }
            protected set
            {
                id = value;
            }
        }

        public Vehicle(Parameter parameter) : this(parameter, BodyType.Sedan)
        { 

        }

        public Vehicle(Parameter parameter, BodyType body)
        {
            this.id = counter++;
            this.parameter = parameter;
            this.body = body;

        }

        public virtual string ShowInfo()
        {
            return "Id: " + Id + " Mileage: " + parameter.Mileage + " Fuel: " + parameter.Fuel + " Seats: " + parameter.Seats;
        }

        public int CompareTo(Vehicle obj)
        {
            return this.parameter.Mileage.CompareTo(obj.parameter.Mileage);
        }

        public bool Equals(Vehicle other)
        {
            if (other == null)
            {
                return false;
            }
            return (this.parameter.Fuel == other.parameter.Fuel);
        }

        public double DivideByThirty()
        {
            Console.WriteLine(parameter.Mileage / 30);
            if (DBT != null)
            {
                DBT($"Succefully divided by 30");
            }
            return parameter.Mileage /= 30;
        }

        public double Multiply()
        {
            return parameter.Fuel *= 20;
        }
    }

    class Car : Vehicle
    { 

        private string name;

        public Car(Parameter parameter, string name, BodyType body) : base(parameter, body)
        {
            this.name = name;
        }

        public override string ShowInfo()
        {
            return $"{base.ShowInfo()} Brand: {name} Bodytype: {Body}";
            // return string.Format("{0} Brand: {1} Bodytype: {2}", base.ShowInfo(), name, Body);
            // return base.ShowInfo() + " Brand: " + name + " Bodytype: " + Body;
        }
    }

    class Bus : Vehicle
    {
        private string name;
        private string colour;

        private string[] arr;

        public int Length
        {
            get
            {
                return arr.Length;
            }
        }

        public string this[int index]
        {
            set
            {
                if (index < Length)
                    arr[index] = value;
            }
            get
            {
                if (index < Length)
                {
                    return arr[index];
                }
                else return "Wrong index.";
            }
        }

        public Bus(Parameter parameter, string name, string colour, params string[] Arr) : base(parameter)
        {
            arr = new string[Arr.Length];
            for (int i = 0; i < Arr.Length; i++)
            {
                this[i] = Arr[i];
            }
            this.colour = colour;
            this.name = name;
        }

        public override string ShowInfo()
        {
            return base.ShowInfo() + " Brand: " + name + " Colour: " + colour;
        }
    }

    class Program
    {
        static void PrintInfoAboutVehicles(Vehicle[] vehicles)
        {
            foreach (Vehicle v in vehicles)
            {
                Console.WriteLine(v.ShowInfo());
            }
        }
        static void Main(string[] args)
        {
            DelegateDivide del;

            Car c1 = new Car(new Parameter(2500, seats: 2, fuel: 200), "Mazda", BodyType.Sportcar);
            Car c2 = new Car(new Parameter(3500, seats: -2, fuel: 120), "Citroen", BodyType.Hatchback);
            Car c3 = new Car(new Parameter(2200, seats: 3, fuel: 210), "Dodge", BodyType.Sedan);
            Car c4 = new Car(new Parameter(5000, seats: 6, fuel: 300), "Volkswagen", BodyType.Minivan);

            //Bus b1 = new Bus(new Parameter(mileage: 10000, fuel: 980, seats: 35), "Volvo", "Red", "Popov", "Istomin", "White", "Daniels", "Anderson");
            //Bus b2 = new Bus(new Parameter(7300, 1500, 20), "Lancia", "White", 5);

            Vehicle[] vehicles = new Vehicle[]
            {
                c1,
                c2,
                c3,
                c4,
               // b1
            };

            Console.WriteLine("\nInitial info: ");
            Console.WriteLine("-----------------");
            PrintInfoAboutVehicles(vehicles);
            Console.WriteLine("-----------------\n\n");

            Array.Sort(vehicles);
            Console.WriteLine("Sorted by mileage: ");
            Console.WriteLine("-----------------");
            PrintInfoAboutVehicles(vehicles);
            Console.WriteLine("-----------------\n\n");


            Vehicle.DBT += s => Console.WriteLine(s);


            Console.WriteLine("Delegate of dividing mileage by 30: ");
            Console.WriteLine("-----------------");

            del = c1.DivideByThirty;
            del += c2.DivideByThirty;
            del += c3.DivideByThirty;
            del += c4.DivideByThirty;

            del();

            Console.WriteLine("-----------------\n\n");
  
            c1.parameter.Fuel = 0;
            //c1.parameter.OutOfFuel += ShowMessage;
            c1.parameter.OutOfFuel += delegate (string a)
            {
                Console.WriteLine(a);
            };

            foreach (Vehicle t in vehicles)
            {
                t.Multiply();
            }

            Console.WriteLine("Fuel multiplied by 20: ");
            Console.WriteLine("-----------------");
            PrintInfoAboutVehicles(vehicles);
            Console.WriteLine("-----------------");
            
            Console.WriteLine("\n\nIs the amount of seats in first car and second equal? - {0}", Equals(c1, c2));

            Console.ReadKey();
        }
    }
}