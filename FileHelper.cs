using System;
using System.IO;
namespace fs15_12_Customer_Database
{
    public class FileHelper
    {
        public static string[] ReadAllLines(string filePath)
        {
            try
            {
                return File.ReadAllLines(filePath);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                return new string[0];
            }
        }

        public static void WriteAllLines(string filePath, string[] lines)
        {
            try
            {
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
    }
}
