// logic for filtering products by category
// this is used to filter products by category type in the ProductView.cs file

namespace Restaurant;

public class CourseLogic
{
    public static string? GetValidCourse()
    {
        List<string> courses = new List<string>{"Main", "Dessert", "Appetizer", "Beverage"};

        while (true)
        {
            var courseName = SelectionPresent.Show(courses, banner: "ADMIN MENU").ElementAt(0).text;
            if (courseName == null) 
                return "REQUEST_PROCESS_EXIT";
            if (courseName != "")
            {
                return courseName;
            }
            else
            {
                return null;
            }
        }
    }
} 