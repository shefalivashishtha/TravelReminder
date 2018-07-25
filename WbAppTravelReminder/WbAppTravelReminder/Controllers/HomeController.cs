using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.Data;
using WbAppTravelReminder.Models;
using System.Globalization;

namespace WbAppTravelReminder.Controllers
{
    public class HomeController : Controller
    {
        // Method will take to reminder page on redirection from API
        public ActionResult Index()
        {
            ClsReminder objReminder = new ClsReminder();

            return View("Reminder", objReminder);
        }
        
        // Landing Page of the website
        public ActionResult Home()
        {
            Session["AccessToken"] = null;
            return View();
        }
        
        // Search and fetch list of hotels basis city sent as query parameter
        public ActionResult Hotels(string sTrvlDestination)
        {
            Session["AccessToken"] = null;
            ClsReminder objReminder = new ClsReminder();
            objReminder.ListHotels = new List<ClsHotels>();
            if(sTrvlDestination != null && sTrvlDestination != "")
            {
                objReminder.Location = sTrvlDestination;
                objReminder.ListHotels = ClsReminder.GetHotels(sTrvlDestination);
            }
            return View(objReminder);
        }

        // Fetch list of hotels basis location sent as post method
        [HttpPost]
        public ActionResult GetHotels(ClsReminder objReminder)
        {
            objReminder.ListHotels = new List<ClsHotels>();
            objReminder.ListHotels = ClsReminder.GetHotels(objReminder.Location);
            
            return View("Hotels",objReminder);

        }

        // Displays API documnent
        public FileResult GetApiDocumentation()
        {
            try
            {
                var path = Server.MapPath("~/Content/TravelReminderAPI.pdf");
                return File(path, "application/pdf");
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Register and generate API Key
        public ActionResult ApiServices()
        {
            Session["AccessToken"] = null;
            ClsReminder objRemi = new ClsReminder();
            objRemi.ApiKey = "";
            return View("Api",objRemi);
        }

        // Google authentication, access token set here and further sent over to reminder
        [HttpPost]
        public ActionResult GoogleLogin(string token,string rtoken, string email, string name, string gender, string lastname, string location)
        {
            Session["AccessToken"] = token;
            ClsReminder objReminder = new ClsReminder();
            objReminder.UserName = name;
            return View("Reminder", objReminder);
        }

        // Reminder created basis the inputs sent as post method parameter by calling API
        [HttpPost]
        public ActionResult CreateReminder(ClsReminder objReminder)
        {
            try
            {
                if (objReminder.TravelDate != null)
                {
                    if (objReminder.TravelDate.Year == 0001 || objReminder.TravelDate.Year == 2001)
                    {
                        string temp = Request.Form.Get("TravelDate");
                        DateTime dt = DateTime.ParseExact(temp, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
                        objReminder.TravelDate = dt;
                    }
                }
                else
                {
                    string temp = Request.Form.Get("TravelDate");
                    DateTime dt = DateTime.ParseExact(temp, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
                    objReminder.TravelDate = dt;
                }
            }
            catch
            {
                string temp = Request.Form.Get("TravelDate");
                DateTime dt = DateTime.ParseExact(temp, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
                objReminder.TravelDate = dt;
            }

            objReminder.AccessToken = Convert.ToString(Session["AccessToken"]).Trim();
          bool bcheck = ClsReminder.SetReminder(objReminder);
            Session["AccessToken"] = null;
            if (bcheck)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Reminder Added in your calendar'); window.location='http://travelreminder.azurewebsites.net/home/Hotels?sTrvlDestination=" + objReminder.Location + "'; </script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('We could not process the request. Please try after some Time.'); window.location='http://travelreminder.azurewebsites.net/home/Home'; </script>");
            }
        }
        
        // Displays Input page for creating reminder
        public ActionResult Reminder()
        {
            if (Convert.ToString(Session["AccessToken"]) != "" && Convert.ToString(Session["AccessToken"]) != null)
            {
                ClsReminder objReminder = new ClsReminder();

                return View(objReminder);
            }
            else
            {
                return View("Home");
            }
        }

        // Post method for generating API key by calling API
        [HttpPost]
        public ActionResult GenerateLicenseKey(ClsReminder objParamReminder)
        {
            try
            {
                bool bCheck;
                string key_first = objParamReminder.ContactNum.Substring(6, 2);
                string key_mid = objParamReminder.EmailId.Substring(0, objParamReminder.EmailId.IndexOf('@'));
                string key_last = objParamReminder.ContactNum.Substring(0, 3);
                objParamReminder.ApiKey = key_first + "_" + key_mid + "-" + key_last;
                bCheck = ClsReminder.AuthorizeUser(objParamReminder.ApiKey);
                if (bCheck)
                {
                    return View("Api", objParamReminder);
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Reminder Added in your calendar'); window.location='http://travelreminder.azurewebsites.net/home/ApiServices'; </script>");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
    }
}