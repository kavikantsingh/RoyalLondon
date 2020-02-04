using RoyalLondon.Business;
using RoyalLondon.Common;
using RoyalLondon.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoyalLondon.Controllers
{
    public class CustomerController : Controller
    {
        private IPremiumCalculator iPremiumCalculator;
        private IFileGenerator iFileGenerator;
        private ICSVFileValidator iCSVFileValidator;

        /// <summary>
        ///use the Unity.Mvc5 DependencyResolver to 
        ///resolve for _iPremiumCalculator,iCSVFileValidator
        ///and _iFileGenerator components
        /// </summary>
        /// <param name="_iPremiumCalculator"></param>
        /// <param name="_iFileGenerator"></param>
        /// <param name="_iCSVFileValidator"></param>
        public CustomerController(IPremiumCalculator _iPremiumCalculator, IFileGenerator _iFileGenerator, ICSVFileValidator _iCSVFileValidator)
        {
            iPremiumCalculator = _iPremiumCalculator;
            iFileGenerator = _iFileGenerator;
            iCSVFileValidator = _iCSVFileValidator;
        }

        /// <summary>
        ///Get Customer Index Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Server != null)
                ViewBag.SampleFilePath = Server.MapPath("~/Uploads/") + "Customer.csv";
            return View(new List<CustomerModel>());
        }

        /// <summary>
        /// This methoid use to read CSV 
        /// file and create txt file.
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            List<CustomerModel> lstCustomerModel = new List<CustomerModel>();
            List<Customer> lstCustomer = new List<Customer>();
            string filePath = string.Empty;
            if (postedFile != null)
            {
                #region Import CSV file and save inside project to read text data

                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                //Read the contents of CSV file.
                string csvData = System.IO.File.ReadAllText(filePath);

                #endregion

                #region This method use to validate CSV data

                List<Error> lstError = iCSVFileValidator.ValidateCSVFileData(csvData);
                if (lstError != null && lstError.Count > 0)
                {
                    List<ErrorModel> lstErrorModel = new List<ErrorModel>();
                    foreach (Error objError in lstError)
                    {
                        lstErrorModel.Add(new ErrorModel()
                        {
                            ErrorFieldName = objError.ErrorFieldName,
                            ErrorMessage = objError.ErrorMessage
                        });
                    }
                    ViewBag.ErrorList = lstErrorModel;
                    return View(lstCustomerModel);
                }
                #endregion

                #region After validation read CSV data and calculate prenium to generate renewal letter 

                //Execute a loop over the rows.
                foreach (string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        //skip header row 
                        if (row.Split(',')[0] != "ID")
                        {
                            lstCustomerModel.Add(new CustomerModel
                            {
                                CustomerID = CommonUtility.GetIntegerValue(row.Split(',')[0]),
                                Title = row.Split(',')[1],
                                FirstName = row.Split(',')[2],
                                Surname = row.Split(',')[3],
                                ProductName = row.Split(',')[4],
                                PayoutAmount = CommonUtility.GetDecimalValue(row.Split(',')[5]),
                                PremiumAmount = CommonUtility.GetDecimalValue(row.Split(',')[6]),

                            });
                        }
                    }
                }

                //Total premium month and credit charge amount is configurable, so later we can change it from web.config file 
                int totalPremiumMonths = CommonUtility.GetIntegerValue(ConfigurationManager.AppSettings["TotalPremiumMonth"]);
                decimal creditCharge = CommonUtility.GetIntegerValue(ConfigurationManager.AppSettings["CreditCharge"]);

                foreach (CustomerModel objCustomerModel in lstCustomerModel)
                {
                    lstCustomer.Add(new Customer()
                    {
                        CustomerID = objCustomerModel.CustomerID,
                        FirstName = objCustomerModel.FirstName,
                        Surname = objCustomerModel.Surname,
                        ProductName = objCustomerModel.ProductName,
                        Title = objCustomerModel.Title,
                        PayoutAmount = objCustomerModel.PayoutAmount,
                        CreditCharge = creditCharge,
                        PremiumAmount = objCustomerModel.PremiumAmount,
                        TotalMonth = totalPremiumMonths,
                    });

                }

                string targetPath = Server.MapPath("~/OutPutFile/");

                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                lstCustomer = iPremiumCalculator.CustomerPremiumCalculator(lstCustomer);
                bool IsSuccess = iFileGenerator.GenerateCustomerRenewalLetter(targetPath, lstCustomer);

                if (IsSuccess)
                {
                    lstCustomerModel.ForEach(x => x.FilePath = x.CustomerID + "_" + x.FirstName + "_" + x.Surname + ".txt");
                }

                #endregion
            }

            return View(lstCustomerModel);
        }
    }
}