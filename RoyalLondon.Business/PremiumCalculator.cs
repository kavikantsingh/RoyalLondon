using RoyalLondon.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalLondon.Business
{
    /// <summary>
    /// This class use to calculate premium based on premuim amount,credit charge and months.
    /// </summary>
    public class PremiumCalculator : IPremiumCalculator
    {
        /// <summary>
        /// This method calculate premium for each customer and returning list to generate txt file.
        /// </summary>
        /// <param name="lstCustomer"></param>
        /// <returns></returns>
        public List<Customer> CustomerPremiumCalculator(List<Customer> lstCustomer, string logPath)
        {
            foreach (Customer objCustomer in lstCustomer)
            {
                try
                {
                    decimal totalPremium = 0, totalCreditCharge = 0, averagePremiumAmount = 0;

                    //Calcuate total credit charge amount
                    totalCreditCharge = ((objCustomer.PremiumAmount * objCustomer.CreditCharge) / 100);

                    //Calculate Total premium (premium amount plus credit charge)
                    totalPremium = objCustomer.PremiumAmount + totalCreditCharge;
                    objCustomer.TotalPremiumAmount = CommonUtility.GetDecimalWithTwoPrecision(totalPremium);

                    //Calculate average monthly premium ampunt
                    objCustomer.AverageMonthlyPremiumAmount = objCustomer.TotalPremiumAmount / objCustomer.TotalMonth;

                    //Calculate initial month premium amount
                    decimal initialMonthPremiumAmount = 0;
                    int getPrecisionLength = CommonUtility.GetPrecisionLength(objCustomer.AverageMonthlyPremiumAmount);
                    averagePremiumAmount = CommonUtility.GetDecimalWithTwoPrecision(objCustomer.AverageMonthlyPremiumAmount);

                    if (getPrecisionLength > 2)
                    {
                        initialMonthPremiumAmount = (objCustomer.TotalPremiumAmount) - (averagePremiumAmount * (objCustomer.TotalMonth - 1));
                    }
                    objCustomer.InitialMonthlyPaymentAmount = CommonUtility.GetDecimalWithTwoPrecision(initialMonthPremiumAmount);

                    //Calculate other month premium amount
                    objCustomer.OtherMonthlyPaymentsAmount = (objCustomer.TotalPremiumAmount - objCustomer.InitialMonthlyPaymentAmount) / (objCustomer.TotalMonth - 1);
                    objCustomer.OtherMonthlyPaymentsAmount = CommonUtility.GetDecimalWithTwoPrecision(objCustomer.OtherMonthlyPaymentsAmount);
                }
                catch (Exception ex)
                {
                    CommonUtility.LogWrite(logPath,ex.Message.ToString());
                }
            }

            return lstCustomer;
        }
       
    }
}
