public static class TryCatchHelper
    {
        /// <summary>
        /// Handles exceptions for void actions with Escape key support.
        /// </summary>
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

        /// <summary>
        /// Handles exceptions for functions that return a value with Escape key support.
        /// </summary>
        public static T EscapeKeyWithResult<T>(Func<T> action, T defaultValue = default, string cancelMessage = "Returning to the previous menu...")
        {
            try
            {
                return action();
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"\n{cancelMessage}");
                Thread.Sleep(1500);
                return defaultValue; // Return specified default value
            }
        }
    }