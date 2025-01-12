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
                ControlHelpPresent.DisplayFeedback(cancelMessage, "bottom", "error");
                // Console.WriteLine($"\n{cancelMessage}");
            }
        }

        /// <summary>
        /// Handles exceptions for functions that return a value with Escape key support.
        /// </summary>
        public static T EscapeKeyWithResult<T>(Func<T> func, T fallback = default!, string fallbackMessage = "Returning to the previous menu...")
        {
            try
            {
                return func();
            }
            catch (OperationCanceledException)
            {
                if (!string.IsNullOrEmpty(fallbackMessage))
                {
                    ControlHelpPresent.DisplayFeedback(fallbackMessage, "bottom", "error");
                }
                return fallback; // Ensure this fallback value is returned correctly
            }
        }
    }