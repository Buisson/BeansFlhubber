/*
 * Crée par SharpDevelop.
 * Utilisateur: Meulon
 * Date: 25/01/2017
 * Heure: 19:04
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using System.Collections.Generic;
using WComp.Beans;
using Newtonsoft.Json;

namespace WComp.Beans
{

	[Bean(Category="FlHhubber")]
	public class Proxy {
		private string port = "8081"; 
		private string ip = "http://127.0.0.1";
		
		public string Port {
			get { return port; }
			set { port = value; }
		}
		public string IP {
			get { return ip; }
			set { ip = value; }
		}
		
		public string get(string url){
			string tmp = getBodyOfGET(url);
			return tmp;
		}
		
		private string getIpInfos() {
			return Url.GET("http://ip-api.com/json");
		}
		
		private bool hasToLaunchMeteo(string conf) {
			if (conf.Equals("meteo")) {
				return true;
			} else if ((conf.Equals("blue")) ||
			           (conf.Equals("orange")) || 
			           (conf.Equals("all")) || 
			           (conf.Equals("none"))) {
				getLedsToChange(conf.ToUpper());
				return false;
			}
			return false;
		}
		
		private string getConfigMeteo() {
			string url = ip + ":" + port + "/getConfigMeteo";
			return Url.GET(url).ToLower();
		}
		
		public void setFlHubberServiceIp(string flhubber_ip) {
			string url = ip + ":" + port + "/setIpWcomp?ip=" + flhubber_ip;
			try {
				Url.GET(url);
			} catch {
				launchFail(url);
			}
		}
		
		public string getWeather() {
			string ip_json;
			try {
				ip_json = getIpInfos();
			} catch {
				launchFail("http://ip-api.com/json");
				return "";
			}
			
			Dictionary<string, object> ip_dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(ip_json);
			
			string base_url = "http://api.openweathermap.org/data/2.5/weather?";
			string api_key = "3ba28e4a597f0802521b732234a7f2c0";
			string lat = ip_dic["lat"].ToString();
			lat = lat.Replace(',','.');
			string lon = ip_dic["lon"].ToString();
			lon = lon.Replace(',','.');
			string url = base_url + "lat=" + lat + "&lon=" + lon + "&appid=" + api_key;
			getMyWheather(url);
			return url;
		}
		
		public string getConfig(string id) {
			string tmp = ip + ":" + port + "/user?id=" + id;
			helloConfconf(tmp);
			return tmp;
		}
		
		public string sendSms(string id) {
			string tmp = ip + ":" + port + "/sendSms?id=" + id;
			return getBodyOfGET(tmp);
		}
		
		public string sendMail(string id){
			string tmp = ip + ":" + port + "/sendMail?id=" + id;
			return getBodyOfGET(tmp);
		}
		
		public string newDevice(string ids){
			string tmp = ip + ":" + port + "/create?ids=" + ids;
			return getBodyOfGET(tmp);
		}
		
		public delegate string leRetourDeMaConfig(string cafa);
		public event leRetourDeMaConfig bonjourMaConfig;
		
		public delegate string getBodyOfGETHandler(string url);
		public event getBodyOfGETHandler getVal;
		public event getBodyOfGETHandler getWeatherEvent;
		public event getBodyOfGETHandler getLedsColor;
		public event getBodyOfGETHandler failMessage;
		
		private string helloConfconf(string url) {
			string tmp = "";
			if (bonjourMaConfig != null) {
				try {
					tmp = Url.GET(url);
					bonjourMaConfig(tmp);
					return tmp;
				} catch {
					launchFail(url);
					return "";
				}
			}
			return "";
		}
		
		private string getBodyOfGET(string url){
			string tmp = "";
			if (getVal != null) {
				try {
					tmp = Url.GET(url);
					getVal(tmp);
				} catch {
					launchFail(url);
				}
			}
			return tmp;
		}
		
		private void launchFail(string url) {
			if (failMessage != null) {
				string tmp = "FAIL retreiving from url: " + url;
				failMessage(tmp);
			}
		}
		
		private void getMyWheather(string url) {
			if (getWeatherEvent != null) {
				try {
					string tmp = Url.GET(url);
					getWeatherEvent(tmp);
				} catch {
					launchFail(url);
				}
			}
		}
		
		private void getLedsToChange (string conf) {
			if (getLedsColor != null) {
				getLedsColor(conf);
			}
		}
	}
}
