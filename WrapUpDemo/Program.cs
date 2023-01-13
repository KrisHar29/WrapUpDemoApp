
List<PersonModel> people = new List<PersonModel>
{
    new PersonModel{ FirstName = "Pixel", LastName = "Bak", Email = "pixel@pixel.com"},
    new PersonModel{ FirstName = "Teddy", LastName = "Bak", Email = "teddy@teddy.com"},
    new PersonModel{ FirstName = "PiTe", LastName = "Bak", Email = "PiTe@PiTe.com"}
};

List<CarModel> cars = new List<CarModel>
{
    new CarModel {Manufacturer = "Toyota", Model = "Yaris" },
    new CarModel {Manufacturer = "Ford", Model = "Focus" },
    new CarModel {Manufacturer = "Heck", Model = "Focus" },
    new CarModel {Manufacturer = "Seat", Model = "Leon" }
};

DataAccess<PersonModel> peopleData = new DataAccess<PersonModel>();

DataAccess<CarModel> carData = new DataAccess<CarModel>();
carData.BadEntryFound += CarData_BadEntryFound;

void CarData_BadEntryFound(object? sender, CarModel e)
{
    Console.WriteLine($"Bad entry found for {e.Manufacturer } { e.Model }");
}

peopleData.SaveToCSV(people, @"C:\Users\Krostoffer\OneDrive\Desktop\TimCorey\people.csv");
carData.SaveToCSV(cars, @"C:\Users\Krostoffer\OneDrive\Desktop\TimCorey\cars.csv");
Console.ReadLine();


public  class DataAccess<T> where T : new()
{
    public event EventHandler<T> BadEntryFound;
   

    public  void SaveToCSV(List<T> items, string filePath)
    {
        List<string> rows = new List<string>();
        T entry = new T();
        var cols = entry.GetType().GetProperties();
        
        string row = "";

        foreach (var col in cols)
        {
            row += $",{col.Name }";
        }
        row = row.Substring(1);
        rows.Add(row);

        foreach (var item in items)
        {
            row = "";
            bool badWordDetected = false;

            foreach (var col in cols)
            {
                string val = col.GetValue(item, null).ToString();
                badWordDetected = BadWordDetector(val);
                if (badWordDetected == true)
                {
                    BadEntryFound?.Invoke(this, item);
                    break;
                }
                row += $",{ val }";
            }

            if (badWordDetected == false)
            {
                row = row.Substring(1);
                rows.Add(row);
            }
        }
        
        File.WriteAllLines(filePath, rows);
    }

    private bool BadWordDetector(string stringToTest)
    {
        bool output = false;
        string lowerCaseTest = stringToTest.ToLower();
        if (lowerCaseTest==("darn") || lowerCaseTest==("heck"))
        {
            output = true;
        }
        return output;
    }
}

