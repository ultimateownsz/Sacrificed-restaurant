// logic for filtering products by category
// this is used to filter products by category type in the ProductView.cs file

public class CourseLogic
{
    public static string GetValidString()
    {
        List<string> courses = new List<string>{"main", "dessert", "appetizer", "beverage"};
        while (true)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Enter course name: ", Console.ForegroundColor);
            Console.ForegroundColor = ConsoleColor.White;
            var courseName = Console.ReadLine().ToLower();

            if (!string.IsNullOrWhiteSpace(courseName) && !courseName.Any(char.IsDigit) && courses.Contains(courseName))
            {
                return char.ToUpper(courseName[0]) + courseName.Substring(1);
            }

            Console.Clear();            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Enter vourse name: {courseName}", Console.ForegroundColor);
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nInvalid course name...");
            Console.WriteLine("Press any key to retry or ESCAPE to go back");
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.B)
            {
                return null;
            }
        }
    }
} 