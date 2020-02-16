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
    /// <summary>
    /// Customer controller
    /// </summary>
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

                //Total premium month and credit charge amount is configurable, so later we can change it from web.config file 
                int totalPremiumMonths = CommonUtility.GetIntegerValue(ConfigurationManager.AppSettings["TotalPremiumMonth"]);
                decimal creditCharge = CommonUtility.GetIntegerValue(ConfigurationManager.AppSettings["CreditCharge"]);

                ValidateResult objValidateResult = iCSVFileValidator.ValidateCSVFileData(csvData,totalPremiumMonths,creditCharge);
                List<ErrorModel> lstErrorModel = new List<ErrorModel>();
                if (objValidateResult != null && objValidateResult.lstError.Count > 0)
                {
                    foreach (Error objError in objValidateResult.lstError)
                    {
                        lstErrorModel.Add(new ErrorModel()
                        {
                            ErrorFieldName = objError.ErrorFieldName,
                            ErrorMessage = objError.ErrorMessage
                        });
                    }
                    ViewBag.ErrorList = lstErrorModel;
                }
                else
                {
                    ViewBag.ErrorList = lstErrorModel;
                }
                #endregion

                #region After validation read CSV data and calculate prenium to generate renewal letter 

                string targetPath = Server.MapPath("~/OutPutFile/");

                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                string logPath = Server.MapPath("~/LogFile/");

                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                lstCustomer = iPremiumCalculator.CustomerPremiumCalculator(objValidateResult.lstCustomer, logPath);
                bool IsSuccess = iFileGenerator.GenerateCustomerRenewalLetter(targetPath, lstCustomer,logPath);

                if (IsSuccess)
                {
                    foreach (Customer objCustomerModel in lstCustomer)
                    {
                        lstCustomerModel.Add(new CustomerModel()
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
                    lstCustomerModel.ForEach(x => x.FilePath = x.CustomerID + "_" + x.FirstName + "_" + x.Surname + ".txt");
                }
                else
                {
                    lstCustomerModel = new List<CustomerModel>();
                }

                #endregion
            }

            return View(lstCustomerModel);
        }
    }
}