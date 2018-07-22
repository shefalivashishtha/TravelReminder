using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WbAppTravelReminder;
using WbAppTravelReminder.Controllers;

namespace WbAppTravelReminder.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Home()
        {

        }

        [TestMethod]
        public void Hotels()
        {

        }

        [TestMethod]
        public void GetHotels()
        {

        }

        [TestMethod]
        public void CreateReminder()
        {
            // Arrange
            HomeController controller = new HomeController();

            Models.ClsReminder objReminder = new Models.ClsReminder();

            // Act
            ViewResult result = controller.CreateReminder(objReminder) as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void ApiServices()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.ApiServices() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Reminder()
        {

        }
    }
}
