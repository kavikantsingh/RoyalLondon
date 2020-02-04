using RoyalLondon.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalLondon.Business
{
 public   class CSVFileValidator:ICSVFileValidator
    {
        public  List<Error> ValidateCSVFileData(string csvFileData)
        {
            List<Error> lstError = new List<Error>();
            bool headerVerified = false;
            csvFileData=csvFileData.Replace("\r", "");
            //Execute a loop over the rows.
            foreach (string row in csvFileData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(row))
                {
                    //skip header row 
                    if (row.Split(',').Length ==7)
                    {
                        if(row.Split(',')[0].ToLower().Trim()== "id" && row.Split(',')[1].ToLower().Trim() == "title" &&
                            row.Split(',')[2].ToLower().Trim() == "firstname" && row.Split(',')[3].ToLower().Trim() == "surname" && row.Split(',')[4].ToLower().Trim() == "productname" 
                            && row.Split(',')[5].ToLower().Trim() == "payoutamount"&& row.Split(',')[6].ToLower().Trim() == "annualpremium")
                        {
                            headerVerified = true;
                          
                        }
                        else if(headerVerified)
                        {
                            if(CommonUtility.IsNumber(row.Split(',')[0])&& CommonUtility.IsDecimal(row.Split(',')[5])&& CommonUtility.IsDecimal(row.Split(',')[6]))
                            {

                            }
                            else
                            {
                                
                                lstError.Add(new Error
                                {
                                    ErrorFieldName = "Customer ID :"+(row.Split(',')[0]),
                                    ErrorMessage=string.Format("Customer ID is {0}," +
                                    ". PayoutAmount is {1}," +
                                    ". AnnualPremium is {2}", CommonUtility.IsNumber(row.Split(',')[0]) == true?"OK":" not OK, please verify it, it should be greater than zero integer only", CommonUtility.IsDecimal(row.Split(',')[5]) == true?"OK":"not OK, it should be non zero decimal only", CommonUtility.IsDecimal(row.Split(',')[6]) == true ? "OK" : "not OK, it should be non zero decimal only")

                                });
                            }
                           
                        }
                        else
                        {
                            lstError.Add(new Error
                            {
                                ErrorFieldName = "Uploaded CSV File Columns Name",

                                ErrorMessage = "Your CSV file should be similar as sample given on the page. The columns and their should be same."
                            });
                            break;
                        }
                       
                    }
                    else
                    {
                        lstError.Add(new Error
                        {
                            ErrorFieldName ="Uploaded CSV File",
                           
                            ErrorMessage="Your CSV file should be similar as sample given on the page. The columns and their should be same."
                        });
                        break;
                    }
                }
            }

            return lstError;
        }
    }
}
