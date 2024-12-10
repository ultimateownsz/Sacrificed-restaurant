using System;
using Project.Logic;
using Project;

namespace Presentation
{
    public static class AdminTableControlPresent
    {
        public static void Show()
        {
            Console.Clear();
            Console.WriteLine("Navigate the grid with arrow keys. Press Enter to toggle table state.");
            Console.WriteLine("Press Esc to return to the admin menu.");
            Thread.Sleep(1000);
            
            while (true)
            {
                // Use GridPresent.Show() for admin mode
                bool isDisplayed = GridPresent.Show(true); // `true` indicates admin mode

                if (!isDisplayed)
                {
                    Console.WriteLine("Error displaying grid.");
                    Thread.Sleep(1000);

                    break;
                }

                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Enter)
                {
                    Console.WriteLine("Enter the table number to toggle its state:");
                    if (int.TryParse(Console.ReadLine(), out int selectedTable))
                    {
                        var reservations = AdminTableControlLogic.GetReservationsForTable(selectedTable);

                        if (reservations.Count > 0)
                        {
                            Console.WriteLine("Warning: Table has active reservations. Proceed? (y/n)");
                            if (Console.ReadKey(true).Key != ConsoleKey.Y)
                                continue;

                            foreach (var res in reservations)
                            {
                                var reservation = Access.Reservations.Read().First(r => r.ID == res);
                                var table = Access.Places.Read().First(p => p.ID == reservation.PlaceID);

                                int newTable = AdminTableControlLogic.GetAvailableTable(table.Capacity.Value, selectedTable);

                                if (newTable > 0)
                                {
                                    reservation.PlaceID = newTable;
                                    Access.Reservations.Update(reservation);
                                }
                                else
                                {
                                    Access.Reservations.Delete(res);
                                }
                            }
                        }

                        AdminTableControlLogic.ToggleTableActiveState(selectedTable);
                        Console.WriteLine($"Table {selectedTable} state toggled.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid table number.");
                    }
                }
                else if (key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
    }
}
