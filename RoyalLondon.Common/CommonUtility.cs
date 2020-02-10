using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalLondon.Common
{
    public static class CommonUtility
    {
        public static decimal GetDecimalValue(string value)
        {
            return Convert.ToDecimal(value);
        }

        public static int GetIntegerValue(string value)
        {
            return Convert.ToInt32(value);
        }

        public static int GetPrecisionLength(decimal value)
        {
            string number = String.Format("{0:0.00}", value); // Convert to string 
            int length = 0;
            if(value!=0)
             length = number.Substring(number.IndexOf(".")).Length;

            return length;
        }

        public static decimal GetDecimalWithTwoPrecision(decimal value)
        {
            
            value = SignificantTruncate(value, 2);
            return value;
        }
        public static decimal SignificantTruncate(decimal num, int significantDigits)
        {
            decimal y = (decimal)Math.Pow(10, significantDigits);
            return Math.Truncate(num * y) / y;
        }
        public static bool IsNumber(string number)
        {
            return int.TryParse(number, out int n);
        }

        public static bool IsDecimal(string number)
        {
           bool decimalData= decimal.TryParse(number, out decimal n);

            if(n==0)
            {
                return false;
            }
            return decimalData;
        }

        public static void LogWrite(string logPath, string errorMessage)
        {
            // Create a writer and open the file:
            StreamWriter log;
            logPath = logPath + "logfile.txt";
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
