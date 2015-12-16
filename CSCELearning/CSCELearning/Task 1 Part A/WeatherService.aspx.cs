using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Xml;
using System.Net;
using System.IO;

namespace CSCELearning.Task_1_Part_A
{
    public partial class WeatherService : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            XmlDocument wsResponseXmlDoc = new XmlDocument();

            //http://api.worldweatheronline.com/free/v1/weather.ashx?q=china&format=xml&num_of_days=5&key=ffeecea954dc52242b2949d42bafa
            //id=jipx(spacetime0)
            UriBuilder url = new UriBuilder();
            url.Scheme = "http";// Same as "http://"

            String APIKey = System.Configuration.ConfigurationManager.AppSettings["APIKey"];

            url.Host = "api.worldweatheronline.com";
            url.Path = "free/v2/weather.ashx";
            url.Query = "q=singapore&format=xml&num_of_days=5&key=" + APIKey;
            
            // URI caching and check
            String cachedURI = File.ReadAllText(Server.MapPath("cachedURI.txt"));

            if (String.Compare(cachedURI, url.ToString()) == 0)
            {
                // If URIs are the same
                // Display cached XML
                wsResponseXmlDoc = new XmlDocument();
                wsResponseXmlDoc.Load(Server.MapPath("WeatherServiceCache.xml"));

                //display the XML response 
                String xmlString = wsResponseXmlDoc.InnerXml;
                Response.ContentType = "text/xml";
                Response.Write(xmlString);

            }
            else
            {

                // Get response
                // If succesful, update cached URI and cached XML
                // If unsuccessful, don't update anything and display error message

                //Make a HTTP request to the global weather web service
                wsResponseXmlDoc = MakeRequest(url.ToString());

                // Check for errors
                int retries = 0;

                // Recall 3 times if unsuccessful
                while (wsResponseXmlDoc == null && retries != 3)
                {

                    wsResponseXmlDoc = MakeRequest(url.ToString());
                    retries++;
                    
                }

                // Use defaut error xml document if error 
                if (wsResponseXmlDoc == null)
                {
                    // If error
                    wsResponseXmlDoc = new XmlDocument();
                    wsResponseXmlDoc.Load(Server.MapPath("WeatherService.xml"));

                    //display the XML response 
                    String xmlString = wsResponseXmlDoc.InnerXml;
                    Response.ContentType = "text/xml";
                    Response.Write(xmlString);

                }
                else
                {
                    // If no error
                    //display the XML response 
                    String xmlString = wsResponseXmlDoc.InnerXml;
                    Response.ContentType = "text/xml";
                    Response.Write(xmlString);

                    // Cache URI and XML

                    // Write URI to file
                    File.WriteAllText(Server.MapPath("cachedURI.txt"), url.ToString());

                    // Save XML file in cache
                    wsResponseXmlDoc.Save(Server.MapPath("WeatherServiceCache.xml"));


                }

            }
        }

        public static XmlDocument MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                // Set timeout to 5 seconds
                request.Timeout = 5 * 1000;
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return (xmlDoc);
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}
