using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ATBM_DOAN01
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Login());
        }

        
    }

    public class Hash
    {
        public static string getHashSha256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }
    }

    public class checkInput
    {
        public static bool hasSpecialCharacter(string input)
        {
            return !Regex.IsMatch(input, "^[a-zA-Z0-9_]*$");
        }
    }
}