using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WbAppTravelReminder;
using WbAppTravelReminder.Controllers;
using WbAppTravelReminder.Models;

namespace WbAppTravelReminder.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        // Validate Redirection
        [TestMethod]
        public void Index()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.AreEqual("Reminder",result.ViewName);
        }

        // Validate Redirection
        [TestMethod]
        public void Home()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Home() as ViewResult;
            Assert.AreEqual("Home",result.ViewName);
        }

        // Validate Hotel method without any parameter
        // expected result true
        [TestMethod]
        public void Hotels()
        {
            var controller = new HomeController();
            var result = controller.Hotels("") as ViewResult;
            ClsReminder objReminder = (ClsReminder)result.ViewData.Model;
            Assert.AreEqual(0, objReminder.ListHotels.Count);
        }

        // validate Hotel method after auto redirection
        // expected result false
        [TestMethod]
        public void GetHotels_AutoRedirect()
        {
            var controller = new HomeController();
            var result = controller.Hotels("Dublin") as ViewResult;
            ClsReminder objReminder = (ClsReminder)result.ViewData.Model;
            Assert.AreNotEqual(0, objReminder.ListHotels.Count);
        }

        // Validate GetHotel with all relevant parameter
        // expected result true
        [TestMethod]
        public void GetHotels_SearchwithParam()
        {
            var controller = new HomeController();
            ClsReminder objParam = new ClsReminder();
            objParam.Location = "Dublin";
            var result = controller.GetHotels(objParam) as ViewResult;
            ClsReminder objReminder = (ClsReminder)result.ViewData.Model;
            Assert.AreNotEqual(0, objReminder.ListHotels.Count);
        }

        // VAlidate GetHotels method without any parameter
        // expected result true 
        [TestMethod]
        public void GetHotels_SearchwithoutParam()
        {
            var controller = new HomeController();
            ClsReminder objParam = new ClsReminder();
            var result = controller.GetHotels(objParam) as ViewResult;
            ClsReminder objReminder = (ClsReminder)result.ViewData.Model;
            Assert.AreEqual(0, objReminder.ListHotels.Count);
        }

        // Validate Create Reminder Method with all the parameters except the access token
        // expected result false
        [TestMethod]
        public void CreateReminder()
        {
            
            HomeController controller = new HomeController();

            Models.ClsReminder objReminder = new Models.ClsReminder();
            objReminder.AccessToken = "";
            objReminder.BoradingPointPostalCode = "456010";
            objReminder.SourcePostalCode = "456001";
            objReminder.Location = "Ujjain";
            objReminder.TravelDate = new DateTime(2018, 07, 18, 9, 0,0);
            objReminder.ModeOfTravel = "Walking";
            
            ContentResult result = controller.CreateReminder(objReminder) as ContentResult;

            
            Assert.AreEqual(false, result.Content.Contains("Added"));
        }

        // Validate Return view result of ApiServices Controller
        // expcted output Api View
        [TestMethod]
        public void ApiServices()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.ApiServices() as ViewResult;

            // Assert
            Assert.AreEqual("Api",result);
        }

        // Validate Generation of License key with parameters
        // expected output Api view result
        [TestMethod]
        public void GenerateLicenseKey_withParam()
        {
            
            ClsReminder objReminder = new Models.ClsReminder();
            objReminder.EmailId = "abcdefgh@xyz.com";
            objReminder.ContactNum = "0099887766";
            string apikey = "addsd-sdfsdf_sdf";
            bool result = ClsReminder.AuthorizeUser(apikey);

            // Assert
            Assert.AreEqual(true, result);
        }

       
    }
}
