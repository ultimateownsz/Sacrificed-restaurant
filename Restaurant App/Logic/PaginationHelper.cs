using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Logic
{
    public static class PaginationHelper
    {
        public static T? Paginate<T>(List<T> items, int itemsPerPage, Func<T, string> formatItem)
        {
            int currentPage = 0;
            int totalPages = (int)Math.Ceiling((double)items.Count / itemsPerPage);

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Page {currentPage + 1} of {totalPages}\n");

                // Get items for the current page
                var pageItems = items.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();

                // Display items
                for (int i = 0; i < pageItems.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {formatItem(pageItems[i])}");
                }

                // Add navigation options
                if (currentPage > 0) Console.WriteLine("P. Previous Page");
                if (currentPage < totalPages - 1) Console.WriteLine("N. Next Page");
                Console.WriteLine("B. Back");

                // Handle user input
                Console.Write("\nSelect an option: ");
                string input = Console.ReadLine()?.Trim();

                if (int.TryParse(input, out int selectedIndex) &&
                    selectedIndex > 0 && selectedIndex <= pageItems.Count)
                {
                    return pageItems[selectedIndex - 1];
                }
                else if (input?.ToUpper() == "P" && currentPage > 0)
                {
                    currentPage--;
                }
                else if (input?.ToUpper() == "N" && currentPage < totalPages - 1)
                {
                    currentPage++;
                }
                else if (input?.ToUpper() == "B")
                {
                    return default; // User chose to cancel
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    Console.ReadKey();
                }
            }
        }
    }
}
