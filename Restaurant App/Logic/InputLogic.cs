public static class InputLogic
{
    public static (long? result, string? error) ParseValidLong(string input, long minValue = 0)
    {
        if (long.TryParse(input, out long result) && result >= minValue)
        {
            return (result, null);
        }
        return (null, $"Input must be a valid long number and greater than or equal to {minValue}");
    }

    public static (int? result, string? error) ParseValidInteger(string input, int minValue = 0)
    {
        if (int.TryParse(input, out var result) && result >= minValue)
        {
            return (result, null);
        }
        return (null, $"Input must be a valid number greater than or equal to {minValue}.");
    }
    
    // possible use for reservations

    // public static (DateTime? result, string? error) ParseValidDate(string input)
    // {
    //     if (DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var date))
    //     {
    //         return (date.Date, null);  // return only the date without the time
    //     }
    //     return (null, "Invalid date format. Please use dd/MM/yyyy");
    // }

    // public static int ConvertDateToInteger(DateTime? date)
    // {
    //     if (!date.HasValue)
    //         throw new ArgumentNullException(nameof(date), "Date cannot be null.");
        
    //     // convert date to the correct int format "ddMMyyyy"
    //     return int.Parse(date.Value.ToString("ddMMyyyy"));
    // }

    public static (int?, string? error) ParseValidateYear(string input, int minYear)
    {
        if (int.TryParse(input, out int result) && result >= minYear)
        {
            return (result, null);
        }
        return (null, $"Input must be  valid year and greater than or equal to {minYear}");
    }

    public static (int? result, string? error) ParseValidMonth(string input)
    {
        if (input == "q") return (null, null);
        
        if (int.TryParse(input, out int result) && result >= 1 && result <= 12)
        {
            return (result, null);
        }
        return (null, "Input must be a valid month (1-12)");
    }
    public static (string? result, string? error) ParseValidString(string input)
    {
        return !string.IsNullOrWhiteSpace(input)
            ? (input, null)
            : (null, "Input cannot be empty or whitespace.");
    }


}