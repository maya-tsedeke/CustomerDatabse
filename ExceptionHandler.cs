using System;
namespace fs15_12_Customer_Database
{
    public class ExceptionHandler
    {
        public static void HandleException(Exception ex)
        {
            // Handle the exception based on your requirements (e.g., log, display error message, etc.)
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
