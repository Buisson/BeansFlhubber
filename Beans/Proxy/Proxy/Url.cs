/*
 * Crée par SharpDevelop.
 * Utilisateur: Meulon
 * Date: 25/01/2017
 * Heure: 19:08
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WComp.Beans
{
	/// <summary>
	/// Description of Url.
	/// </summary>
	public class Url
	{
		
		// Returns JSON string 
        public static string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream,
        Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText 
                }
                throw;
            }
        }
        
        // {info:"info",meteo:"meteo",lum:10,pres:10,humTerre:10,humAir:10,temp:10,dist:10}
        
        
        public static void POST(string url, string json)
        {

            try
            {
				var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.Method = "POST";
				
				using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
				{
				
				    streamWriter.Write(json);
				    streamWriter.Flush();
				    streamWriter.Close();
				}
				
				var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
				    var result = streamReader.ReadToEnd();
				}
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream,Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText 
                }
                throw;
            }
        }
		
		
		public static void post(string url, string content){
        	POST(url, content);
		}
        

	}
}