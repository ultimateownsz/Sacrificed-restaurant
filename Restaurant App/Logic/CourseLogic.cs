// logic for filtering products by category
// this is used to filter products by category type in the ProductView.cs file

using Project;

public class CourseLogic
{
    public static string GetValidCourse()
    {
        List<string> courses = new List<string>{"Main", "Dessert", "Appetizer", "Beverage"};

        while (true)
        {
            string banner = "Choose course:\n\n";
            var courseName = SelectionPresent.Show(courses, banner, false).text;

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