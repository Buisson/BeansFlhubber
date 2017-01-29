/*
 * Crée par SharpDevelop.
 * Utilisateur: Meulon
 * Date: 25/01/2017
 * Heure: 19:04
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using WComp.Beans;
using Newtonsoft.Json;

namespace WComp.Beans
{

	[Bean(Category="Flhubber")]
	public class Proxy
	{

		private string port = "8081"; 
		private string ip = "10.0.0.3";
		//private string getVal = "";
		
		public string Port {
			get { return port; }
			set {
				port = value;
			}
		}
		
		public string IP {
			get { return ip; }
			set {
				ip = value;
			}
		}
		
		public string get(string url){
			string tmp = Url.GET(url);
			getVal(tmp);
			return tmp;
		}
		
		public string getConfig(string id) {
			string tmp = Url.GET(ip + ":" + port + "/user?id=" + id);
			getVal(tmp);
			return tmp;
		}
		
		public string sendMail(string id){
			string tmp = Url.GET(ip + ":" + port + "/sendMail?id=" + id);
			getVal(tmp);
			return tmp;
		}
		
		public string newDevice(string ids){
			string tmp = Url.GET(ip + ":" + port + "/create?ids=" + ids);
			getVal(tmp);
			return tmp;
		}
		
		public delegate string getBodyOfGETHandler(string url);
		
		public event getBodyOfGETHandler getVal;
		
		private string getBodyOfGET(string url){
			string tmp = Url.GET(url);
			getVal(tmp);
			return tmp;
		}
		
		


	}
}
