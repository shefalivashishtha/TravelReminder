using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Google.Apis.Calendar.v3.Data;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.IO;
using System.Data;

namespace WbAppTravelReminder.Models
{
    public class ClsReminder
    {
        #region Constructor

        public ClsReminder()
        {
        }

        #endregion

        #region Reminder Properties

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }


        private string _accessToken;
        public string AccessToken
        {
            get { return _accessToken; }
            set { _accessToken = value; }
        }


        private string _location;
        [Required(ErrorMessage = "Mandatory Field")]
        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }


        private DateTime _travelDate;
        [Required(ErrorMessage = "Mandatory Field")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = @"{0:MM\/dd\/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime TravelDate
        {
            get { return _travelDate; }
            set { _travelDate = value; }
        }


        private string _sourcePostalCode;
        [Required(ErrorMessage = "Mandatory Field")]
        public string SourcePostalCode
        {
            get { return _sourcePostalCode; }
            set { _sourcePostalCode = value; }
        }

        private string _boardingPointPostalCode;
        [Required( ErrorMessage ="Mandatory Field")]
        public string BoradingPointPostalCode
        {
            get { return _boardingPointPostalCode; }
            set { _boardingPointPostalCode = value; }
        }

        private string _modeOfTravel;
        [Required]
        public string ModeOfTravel
        {
            get { return _modeOfTravel; }
            set { _modeOfTravel = value; }
        }

        
        private List<string> _lstTravelModes;
        public List<string> LstTravelModes
        {
            get
            {
                _lstTravelModes = new List<string>();
                _lstTravelModes.Add("Walking");
                _lstTravelModes.Add("Bicycling");
                _lstTravelModes.Add("Driving");
                return _lstTravelModes;
            }
        }

        #endregion

        #region API Key Properties

        private string _EmailId;
        [Required(ErrorMessage = "Mandatory Field")]
        public string EmailId
        {
            get { return _EmailId; }
            set { _EmailId = value; }
        }

        private string _contactNum;
        [Required(ErrorMessage = "Mandatory Field")]
        public string ContactNum
        {
            get { return _contactNum; }
            set { _contactNum = value; }
        }

        private string _apiKey;

        public string ApiKey
        {
            get { return _apiKey; }
            set { _apiKey = value; }
        }

        private List<ClsHotels> _lstHotels;

        public List<ClsHotels> ListHotels
        {
            get { return _lstHotels; }
            set { _lstHotels = value; }
        }


        #endregion

        #region Functions
        
        public static bool SetReminder(ClsReminder objParamReminder)
        {
            try
            {
                string sResult = "";
                objParamReminder.ApiKey = "abc-1";
                HttpResponseMessage response =  CallApiPostMethod(objParamReminder, "SetReminder");
                if (response.IsSuccessStatusCode)
                {
                    sResult = response.Content.ReadAsStringAsync().Result;
                }

                if (sResult != "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static bool AuthorizeUser(string sLicenseKey)
        {
            try
            {
                
                HttpResponseMessage response = CallApiGetMethod(sLicenseKey, "GetApiKey", "sLicenseKey");
                if(response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        #endregion

        #region APi Calls

        protected static HttpResponseMessage CallApiPostMethod(ClsReminder objParamReminder,string sMethodName)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://apitravelreminder.azurewebsites.net/"); //new Uri("http://localhost:49998/");// 
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(objParamReminder), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("api/Values/"+ sMethodName, content).Result;
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected static HttpResponseMessage CallApiGetMethod(string sParam, string sMethodName,string uriParameterName)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://apitravelreminder.azurewebsites.net/"); //new Uri("http://localhost:49998/"); 
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("api/Values/" + sMethodName+"?"+uriParameterName+"="+ sParam).Result;
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<ClsHotels> GetHotels(string sParam)
        {
            try
            {
                
                string credentials = String.Format("{0}:{1}", "user", "password");
                byte[] bytes = Encoding.ASCII.GetBytes(credentials);
                string base64 = Convert.ToBase64String(bytes);
                string authorization = String.Concat("Basic ", base64);

                List<ClsHotels> _lstHotel = new List<ClsHotels>();

                string wbUrl = "http://hotelnci.azurewebsites.net/rest/UserService/users/" + sParam;
                HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp(wbUrl);
                //request.Credentials = new NetworkCredential("user", "password");
                request.Headers.Set(HttpRequestHeader.Authorization, authorization);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                DataSet dsResponse = new DataSet();
                dsResponse.ReadXml(response.GetResponseStream());
                if (dsResponse != null)
                {
                    if (dsResponse.Tables.Count > 0)
                    {
                        DataTable dt_Hotels = dsResponse.Tables["interface"];
                        if (dt_Hotels != null)
                        {
                            for (int i = 0; i < dt_Hotels.Rows.Count; i++)
                            {
                                ClsHotels tmpHotel = new ClsHotels();
                                tmpHotel.Address = Convert.ToString(dt_Hotels.Rows[i]["Address"]);
                                tmpHotel.Name = Convert.ToString(dt_Hotels.Rows[i]["Name"]);
                                tmpHotel.City = Convert.ToString(dt_Hotels.Rows[i]["City"]);
                                tmpHotel.Price = Convert.ToDouble(dt_Hotels.Rows[i]["Price"]);
                                tmpHotel.Rating = Convert.ToInt32(dt_Hotels.Rows[i]["Rating"]);
                                tmpHotel.Id = Convert.ToInt32(dt_Hotels.Rows[i]["Id"]);
                                _lstHotel.Add(tmpHotel);
                            }
                        }

                    }
                }

                return _lstHotel;

            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion
    }

   public class ClsHotels
    {
        #region Properties

        public string Address { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public double Price { get; set; }
        public int Rating { get; set; }


        #endregion
    }

}