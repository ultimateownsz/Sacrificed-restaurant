using Presentation;
using Project;
using Project.Logic;
using Project.Presentation;

class Program
{
    static void Main()
    {
        var user = Access.Users.GetBy<int>("ID", 2);
        var orders = MakingReservations.TakeOrders(DateTime.Now, user, -1, 1);
        MakingReservations.PrintReceipt(orders, -1, user);
    }
    //Console.OutputEncoding = System.Text.Encoding.Unicode;
}
