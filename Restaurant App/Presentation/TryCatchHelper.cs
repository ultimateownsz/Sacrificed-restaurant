public static class TryCatchHelper
    {
        public static void EscapeKeyException(Action action, string cancelMessage = "Returning to the previous menu...")
        {
            try
            {
                action();
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"\n{cancelMessage}");
            }
        }
    }