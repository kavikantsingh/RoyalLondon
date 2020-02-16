using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoyalLondon.Controllers;
using System.Web.Mvc;
using RoyalLondon.Business;
using System.Web;

namespace RoyalLondon.Tests.Controllers
{
    [TestClass]
    public class CustomerControllerTest
    {
        private IPremiumCalculator iPremiumCalculator;
        private IFileGenerator iFileGenerator;
        private ICSVFileValidator iCSVFileValidator;

        [TestMethod]
        public void Index()
        {
            // Arrange
            CustomerController controller = new CustomerController(iPremiumCalculator,iFileGenerator,iCSVFileValidator);

            // Act for Get method
            ViewResult result = controller.Index() as ViewResult;

            // Assert for Get method
            Assert.IsNotNull(result);

            // Act For Post Method
            HttpPostedFileBase postedFile =null;
            ViewResult resultPost = controller.Index(postedFile) as ViewResult;

            // Assert for Get method
            Assert.IsNotNull(resultPost);

        }

        [TestMethod]
        public void Index(HttpPostedFileBase postedFile)
        {
            // Arrange
            CustomerController controller = new CustomerController(iPremiumCalculator, iFileGenerator, iCSVFileValidator);

            // Act
            ViewResult result = controller.Index(postedFile) as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }
       
    }
}
