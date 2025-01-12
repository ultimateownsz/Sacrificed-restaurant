namespace Restaurant;

public class RequestLogic
{
    public static string FormatAccount(RequestModel request)
    {
        return $"Order product: {request.ProductID})";
    }

    public static List<string> GenerateMenuOptions(List<RequestModel> accounts, int currentPage, int totalPages)
    {
        var options = accounts.Select(FormatAccount).ToList();
        if (currentPage > 0) options.Add("Previous page");
        if (currentPage < totalPages - 1) options.Add("Next page");
        options.Add("Back");
        return options;
    }
}