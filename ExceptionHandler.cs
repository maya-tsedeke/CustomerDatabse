using System;

namespace fs15_12_Customer_Database
{
    public class ExceptionHandler
    {
        public static void HandleException(Exception ex)
        {
            if (ex is EmailNotFoundException)
            {
                Console.WriteLine("Email not found: " + ex.Message);
            }
            else if (ex is UserNotFoundException)
            {
                Console.WriteLine("User not found: " + ex.Message);
            }
            else
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }

    public class EmailNotFoundException : Exception
    {
        public EmailNotFoundException(string message) : base(message)
        {
        }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message)
        {
        }
    }
}
//We can use more error handling exception

//  In the case of this project no feutures to search or update by email
// this an example how to handele error message