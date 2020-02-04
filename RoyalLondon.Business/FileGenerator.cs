using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalLondon.Business
{
    /// <summary>
    /// 
    /// </summary>
    public class FileGenerator : IFileGenerator
    {
        /// <summary>
        /// This method use to generate text file for renewal invitation letter
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="lstCustomer"></param>
        /// <returns></returns>
        public bool GenerateCustomerRenewalLetter(string filepath, List<Customer> lstCustomer)
        {
            bool isCompleted = false;
            try
            {
                foreach (Customer objCustomer in lstCustomer)
                {
                    string fileLocation = filepath + "/" + objCustomer.CustomerID + "_" + objCustomer.FirstName + "_" + objCustomer.Surname + ".txt";

                    StringBuilder sb = new StringBuilder();

                    sb = CreateTemplateForTEXTFile(sb, objCustomer);
                    using (var sw = new StreamWriter(fileLocation, false))
                    {
                        sw.Write(sb.ToString());
                    }
                }
                isCompleted = true;
            }
            catch (Exception ex)
            {
                isCompleted = false;
                throw ex;
            }
            return isCompleted;
        }

        /// <summary>
        /// This is private method of this class, Replacing dynamic values and create txt file template
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="objCustomer"></param>
        /// <returns></returns>
        private static StringBuilder CreateTemplateForTEXTFile(StringBuilder sb, Customer objCustomer)
        {
            sb.AppendLine("Date: " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            sb.AppendLine(string.Format("FAO: {0} {1}", objCustomer.Title, objCustomer.FirstName + " " + objCustomer.Surname));
            sb.AppendLine("RE: Your Renewal");
            sb.AppendLine(string.Format("Dear {0} {1}", objCustomer.Title, objCustomer.Surname));
            sb.AppendLine("We hereby invite you to renew your Insurance Policy, subject to the following terms.");
            sb.AppendLine(string.Format("Your chosen insurance product is {0}.", objCustomer.ProductName));

            sb.AppendLine(string.Format("The amount payable to you in the event of a valid claim will be £{0}.", objCustomer.PayoutAmount));
            sb.AppendLine(string.Format("Your annual premium will be £{0}.", objCustomer.PremiumAmount));
            sb.AppendLine(string.Format("If you choose to pay by Direct Debit, we will add a credit charge of £{0}, bringing the total to", objCustomer.CreditCharge));
            sb.AppendLine(string.Format("£{0}. This is payable by an initial payment of £{1}, followed by 11 payments of £{1} each.", objCustomer.TotalPremiumAmount, objCustomer.InitialMonthlyPaymentAmount, objCustomer.OtherMonthlyPaymentsAmount));

            sb.AppendLine("Please get in touch with us to arrange your renewal by visiting https://www.regallutoncodingtest.co.uk/renew");
            sb.AppendLine(" or calling us on 01625 123456.");
            sb.AppendLine("Kind Regards");
            sb.AppendLine("Regal Luton");

            return sb;
        }
    }
}
