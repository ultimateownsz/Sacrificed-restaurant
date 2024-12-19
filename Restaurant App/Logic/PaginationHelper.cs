using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Logic
{
    public static class PaginationHelper
    {
        public static string PaginateWithArrowKeys<T>(
            List<T> items,
            int itemsPerPage,
            Func<T, string> formatItem
        )
        {
            int currentPage = 0;
            int totalPages = (int)Math.Ceiling((double)items.Count / itemsPerPage);

            while (true)
            {
                // Clear console and show current page info
                Console.Clear();
                Console.WriteLine($"Page {currentPage + 1} of {totalPages}\n");

                // Get items for the current page
                var pageItems = items.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();

                // Display items
                var menuOptions = pageItems.Select(formatItem).ToList();

                // Add navigation options
                if (currentPage > 0) menuOptions.Add("<< Previous Page");
                if (currentPage < totalPages - 1) menuOptions.Add("Next Page >>");
                menuOptions.Add("Back");

                // Display options and capture user selection
                var selection = SelectionPresent.Show(menuOptions, "Select an option:\n");

                // Handle navigation options
                if (selection.text == "Back")
                    return null;

                if (selection.text == "Next Page >>")
                {
                    currentPage = Math.Min(currentPage + 1, totalPages - 1);
                    continue;
                }

                if (selection.text == "<< Previous Page")
                {
                    currentPage = Math.Max(currentPage - 1, 0);
                    continue;
                }

                // Return selected item (non-navigation option)
                return selection.text;
            }
        }
    }
}
