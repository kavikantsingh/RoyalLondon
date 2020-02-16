using RoyalLondon.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalLondon.Business
{
    /// <summary>
    /// Validate CSV file class
    /// </summary>
    public class CSVFileValidator : ICSVFileValidator
    {
        /// <summary>
        /// This method implemented for validate CSV file
        /// </summary>
        /// <param name="csvFileData"></param>
        /// <returns></returns>
        public ValidateResult ValidateCSVFileData(string csvFileData,int totalMonths,decimal creditCharge)
        {
            ValidateResult objValidateResult = new ValidateResult();
            objValidateResult.lstError = new List<Error>();
            objValidateResult.lstCustomer = new List<Customer>();
            bool headerVerified = false;
            csvFileData = csvFileData.Replace("\r", "");
            //Execute a loop over the rows.
            foreach (string row in csvFileData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(row))
                {
                    //skip header row 
                    if (row.Split(',').Length == 7)
                    {
                        if (row.Split(',')[0].ToLower().Trim() == "id" && row.Split(',')[1].ToLower().Trim() == "title" &&
                            row.Split(',')[2].ToLower().Trim() == "firstname" && row.Split(',')[3].ToLower().Trim() == "surname" && row.Split(',')[4].ToLower().Trim() == "productname"
                            && row.Split(',')[5].ToLower().Trim() == "payoutamount" && row.Split(',')[6].ToLower().Trim() == "annualpremium")
                        {
                            headerVerified = true;

                        }
                        else if (headerVerified)
                        {
                            if (CommonUtility.IsNumber(row.Split(',')[0]) && CommonUtility.IsDecimal(row.Split(',')[5]) && CommonUtility.IsDecimal(row.Split(',')[6]))
                            {
                                objValidateResult.lstCustomer.Add(new Customer()
                                {
                                    CreditCharge = creditCharge,
                                    TotalMonth = totalMonths,
                                    CustomerID = CommonUtility.GetIntegerValue(row.Split(',')[0]),
                                    Title = row.Split(',')[1],
                                    FirstName = row.Split(',')[2],
                                    Surname = row.Split(',')[3],
                                    ProductName = row.Split(',')[4],
                                    PayoutAmount = CommonUtility.GetDecimalValue(row.Split(',')[5]),
                                    PremiumAmount = CommonUtility.GetDecimalValue(row.Split(',')[6]),
                                });
                            }
                            else
                            {
                                string errorMessage = string.Empty;
                                if (CommonUtility.IsNumber(row.Split(',')[0]) == false)
                                {
                                    errorMessage = string.Format("Customer ID is {0}", CommonUtility.IsNumber(row.Split(',')[0]) == true ? " correct" : " not correct, please verify it. It should be greater than zero integer only.");
                                }
                                if (CommonUtility.IsDecimal(row.Split(',')[5]) == false)
                                {
                                    errorMessage += string.Format("PayoutAmount is {0},", CommonUtility.IsDecimal(row.Split(',')[5]) == true ? "correct" : "not correct, it should be non zero decimal only.");
                                }
                                if (CommonUtility.IsDecimal(row.Split(',')[6]) == false)
                                {
                                    errorMessage += string.Format("AnnualPremium is {0}", CommonUtility.IsDecimal(row.Split(',')[6]) == true ? "correct" : "not correct, it should be non zero decimal only.");
                                }
                                objValidateResult.lstError.Add(new Error
                                {
                                    ErrorFieldName = "Customer ID :" + (row.Split(',')[0]),
                                    ErrorMessage = errorMessage

                                });
                            }

                        }
                        else
                        {
                            objValidateResult.lstError.Add(new Error
                            {
                                ErrorFieldName = "Uploaded CSV File Columns Name",

                                ErrorMessage = "Your CSV file should be similar as sample given on the page. The columns name should be same."
                            });
                            break;
                        }

                    }
                    else
                    {
                        objValidateResult.lstError.Add(new Error
                        {
                            ErrorFieldName = "Uploaded CSV File",

                            ErrorMessage = "Your CSV file should be similar as sample given on the page. The columns name should be same."
                        });
                        break;
                    }
                }
            }

            return objValidateResult;
        }
    }
}
