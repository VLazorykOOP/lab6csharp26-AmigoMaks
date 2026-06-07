using System;
using System.Collections;
using System.Collections.Generic;

public class InvalidShipDataException : Exception
{
    public InvalidShipDataException(string message) : base(message) { }
}

public interface IShip : IComparable<IShip>
{
    string Name { get; set; }
    int YearBuilt { get; set; }
    void Show();
}

public interface IMilitary
{
    void FireWeapon();
}

public interface ICommercial
{
    void LoadCargo();
}

public class Steamboat : IShip, ICommercial
{
    public string Name { get; set; }
    public int YearBuilt { get; set; }
    public int EnginePower { get; set; }

    public Steamboat(string name, int yearBuilt, int enginePower)
    {
        if (yearBuilt < 1700 || yearBuilt > DateTime.Now.Year)
            throw new InvalidShipDataException("Invalid year for Steamboat.");

        Name = name;
        YearBuilt = yearBuilt;
        EnginePower = enginePower;
    }

    public void Show()
    {
        Console.WriteLine($"Steamboat: {Name}, Year: {YearBuilt}, Power: {EnginePower} HP");
    }

    public void LoadCargo()
    {
        Console.WriteLine($"{Name} is loading commercial cargo.");
    }

    public void BlowHorn()
    {
        Console.WriteLine($"{Name} is blowing its loud horn!");
    }

    public int CompareTo(IShip other)
    {
        return YearBuilt.CompareTo(other.YearBuilt);
    }
}

public class Sailboat : IShip, ICommercial
{
    public string Name { get; set; }
    public int YearBuilt { get; set; }
    public int SailArea { get; set; }

    public Sailboat(string name, int yearBuilt, int sailArea)
    {
        Name = name;
        YearBuilt = yearBuilt;
        SailArea = sailArea;
    }

    public void Show()
    {
        Console.WriteLine($"Sailboat: {Name}, Year: {YearBuilt}, Sail Area: {SailArea} sq.m.");
    }

    public void LoadCargo()
    {
        Console.WriteLine($"{Name} is loading small cargo.");
    }

    public void DropAnchor()
    {
        Console.WriteLine($"{Name} dropped the anchor manually.");
    }

    public int CompareTo(IShip other)
    {
        return YearBuilt.CompareTo(other.YearBuilt);
    }
}

public class Corvette : IShip, IMilitary
{
    public string Name { get; set; }
    public int YearBuilt { get; set; }
    public int MissilesCount { get; set; }

    public Corvette(string name, int yearBuilt, int missilesCount)
    {
        Name = name;
        YearBuilt = yearBuilt;
        MissilesCount = missilesCount;
    }

    public void Show()
    {
        Console.WriteLine($"Corvette: {Name}, Year: {YearBuilt}, Missiles: {MissilesCount}");
    }

    public void FireWeapon()
    {
        Console.WriteLine($"{Name} is firing missiles!");
    }

    public void EnableRadar()
    {
        Console.WriteLine($"{Name} activated military radar system.");
    }

    public int CompareTo(IShip other)
    {
        return YearBuilt.CompareTo(other.YearBuilt);
    }
}

public class Fleet : IEnumerable<IShip>
{
    private List<IShip> _ships = new List<IShip>();

    public void AddShip(IShip ship)
    {
        _ships.Add(ship);
    }

    public IEnumerator<IShip> GetEnumerator()
    {
        return _ships.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public interface IPersona : IComparable<IPersona>, ICloneable
{
    string LastName { get; set; }
    DateTime BirthDate { get; set; }
    int GetAge();
    void ShowInfo();
}

public abstract class BasePersona : IPersona
{
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }

    public BasePersona(string lastName, DateTime birthDate)
    {
        LastName = lastName;
        BirthDate = birthDate;
    }

    public int GetAge()
    {
        DateTime today = DateTime.Today;
        int age = today.Year - BirthDate.Year;
        if (BirthDate.Date > today.AddYears(-age)) age--;
        return age;
    }

    public abstract void ShowInfo();

    public int CompareTo(IPersona other)
    {
        return GetAge().CompareTo(other.GetAge());
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}

public class Enrollee : BasePersona
{
    public string Faculty { get; set; }

    public Enrollee(string lastName, DateTime birthDate, string faculty) : base(lastName, birthDate)
    {
        Faculty = faculty;
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"[Enrollee] {LastName}, Age: {GetAge()}, Faculty: {Faculty}");
    }
}

public class Student : BasePersona
{
    public string Faculty { get; set; }
    public int Course { get; set; }

    public Student(string lastName, DateTime birthDate, string faculty, int course) : base(lastName, birthDate)
    {
        Faculty = faculty;
        Course = course;
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"[Student] {LastName}, Age: {GetAge()}, Faculty: {Faculty}, Course: {Course}");
    }
}

public class Teacher : BasePersona
{
    public string Faculty { get; set; }
    public string Position { get; set; }
    public int Experience { get; set; }

    public Teacher(string lastName, DateTime birthDate, string faculty, string position, int experience) : base(lastName, birthDate)
    {
        Faculty = faculty;
        Position = position;
        Experience = experience;
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"[Teacher] {LastName}, Age: {GetAge()}, Faculty: {Faculty}, Pos: {Position}, Exp: {Experience} yrs");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== TASK 1 & 4: SHIPS HIERARCHY, INTERFACES, FOREACH, PATTERN MATCHING ===\n");

        Fleet myFleet = new Fleet();
        myFleet.AddShip(new Steamboat("Titanic", 1912, 46000));
        myFleet.AddShip(new Sailboat("Black Pearl", 1700, 800));
        myFleet.AddShip(new Corvette("Mazepa", 2022, 16));

        foreach (IShip ship in myFleet)
        {
            ship.Show();

            if (ship is IMilitary militaryShip)
            {
                militaryShip.FireWeapon();
            }

            if (ship is ICommercial commercialShip)
            {
                commercialShip.LoadCargo();
            }

            switch (ship)
            {
                case Steamboat st:
                    st.BlowHorn();
                    break;
                case Sailboat sb:
                    sb.DropAnchor();
                    break;
                case Corvette cv:
                    cv.EnableRadar();
                    break;
            }
            Console.WriteLine();
        }

        Console.WriteLine("=== TASK 2: PERSONA HIERARCHY WITH .NET INTERFACES ===\n");

        IPersona[] people = new IPersona[]
        {
            new Enrollee("Shevchenko", new DateTime(2006, 5, 12), "CS"),
            new Student("Kosach", new DateTime(2004, 2, 25), "CS", 3),
            new Teacher("Hrushevskyi", new DateTime(1975, 9, 29), "CS", "Docent", 15)
        };

        Array.Sort(people);

        Console.WriteLine("All Personas (Sorted by Age via IComparable):");
        foreach (var person in people)
        {
            person.ShowInfo();
        }

        int minAge = 18;
        int maxAge = 30;
        Console.WriteLine($"\nSearch results ({minAge} - {maxAge} years):");

        foreach (var person in people)
        {
            if (person.GetAge() >= minAge && person.GetAge() <= maxAge)
            {
                person.ShowInfo();
            }
        }

        Console.WriteLine("\n=== TASK 3: EXCEPTION HANDLING ===\n");

        try
        {
            Console.WriteLine("Attempting to create a Steamboat from year 1000...");
            Steamboat invalidShip = new Steamboat("OldShip", 1000, 500);
        }
        catch (InvalidShipDataException ex)
        {
            Console.WriteLine($"Custom Exception Caught: {ex.Message}");
        }

        try
        {
            Console.WriteLine("\nAttempting to store an integer in a string array...");
            string[] stringArray = new string[5];
            object[] objectArray = stringArray;
            objectArray[0] = 42;
        }
        catch (ArrayTypeMismatchException ex)
        {
            Console.WriteLine($"Standard Exception Caught: {ex.GetType().Name} - {ex.Message}");
        }
    }
}
