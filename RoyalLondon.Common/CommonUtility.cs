using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalLondon.Common
{
    /// <summary>
    /// This is a Common Utility Class. 
    /// It's contains common functionality of Application.
    /// </summary>
    public static class CommonUtility
    {
        /// <summary>
        /// It's return decimal value and take string input.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal GetDecimalValue(string value)
        {
            return Convert.ToDecimal(value);
        }

        /// <summary>
        /// It's return int value and take string input.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetIntegerValue(string value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// It's return int value and take decimal input.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetPrecisionLength(decimal value)
        {
            string number = String.Format("{0:0.00}", value); // Convert to string 
            int length = 0;
            if (value != 0)
                length = number.Substring(number.IndexOf(".")).Length;

            return length;
        }

        /// <summary>
        /// It's return two precision decimal value and take decimal input.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal GetDecimalWithTwoPrecision(decimal value)
        {

            value = SignificantTruncate(value, 2);
            return value;
        }

        /// <summary>
        /// It's return decimal value and take decimal input and int significantdigits.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="significantDigits"></param>
        /// <returns></returns>
        public static decimal SignificantTruncate(decimal num, int significantDigits)
        {
            decimal y = (decimal)Math.Pow(10, significantDigits);
            return Math.Truncate(num * y) / y;
        }

        /// <summary>
        /// It's verify string is number or not.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsNumber(string number)
        {
            return int.TryParse(number, out int n);
        }

        /// <summary>
        /// It's verify string is decimal or not.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsDecimal(string number)
        {
            bool decimalData = decimal.TryParse(number, out decimal n);

            if (n == 0)
            {
                return false;
            }
            return decimalData;
        }

        /// <summary>
        /// This function use to log write in log txt file.
        /// </summary>
        /// <param name="logPath"></param>
        /// <param name="errorMessage"></param>
        public static void LogWrite(string logPath, string errorMessage)
        {
            // Create a writer and open the file:
            StreamWriter log;
            logPath = logPath + "log.txt";
            if (!File.Exists(logPath))
            {
                log = new StreamWriter(logPath);
            }
            else
            {
                log = File.AppendText(logPath);
            }

            // Write to the file:
            log.WriteLine(DateTime.Now);
            log.WriteLine(errorMessage);
            log.WriteLine();

            // Close the stream:
            log.Close();
        }

    }
}
