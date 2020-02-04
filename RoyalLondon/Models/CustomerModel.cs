using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoyalLondon.Models
{
    public class CustomerModel
    {
        #region Properties 
        public int CustomerID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string ProductName { get; set; }
        public decimal PayoutAmount { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal CreditCharge { get; set; }
        public decimal TotalPremiumAmount { get; set; }
        public decimal AverageMonthlyPremiumAmount { get; set; }
        public decimal InitialMonthlyPaymentAmount { get; set; }
        public decimal OtherMonthlyPaymentsAmount { get; set; }
        public string FilePath { get; set; }
        public List<ErrorModel> lstErrorModel { get; set; }
        #endregion
    }
}