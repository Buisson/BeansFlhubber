using System;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Web;
using WComp.Beans;

namespace FlHubberService
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, 
    ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
	[Bean(Category="FlHubber")]
	public class FlHubberService : IRESTService {
		public WebServiceHost serviceHost;

		public FlHubberService() {
			serviceHost = new WebServiceHost(this, new Uri("http://0.0.0.0:8081/FlHubber"));
		}
		
		public void myIpToServer() {
			var host = Dns.GetHostEntry(Dns.GetHostName());
			string final_ip = "";
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					final_ip = ip.ToString();
				}
			}
			launchSetTheIp(final_ip);
		}
		
		public string returnConfigurationEvent(string json) {
			launchConfEvent(json);
			return json;
		}
		
		public string returnMeteoEvent(string meteo) {
			meteo = meteo.ToLower();
			if (meteo.Equals("meteo")) {
				launchMeteoEvent(meteo);
			} else if ((meteo.Equals("blue")) ||
			           (meteo.Equals("orange")) || 
			           (meteo.Equals("all")) || 
			           (meteo.Equals("none"))) {
				launchSetColor(meteo.ToUpper());
			}
			return meteo;
		}
		
		private void launchMeteoEvent(string meteo) {
			if (Ret_Meteo != null) {
				Ret_Meteo(meteo);
			}
		}
			
		public void launchConfEvent(string json) {
			if (Ret_Config != null) {
				Ret_Config(json);
			}
		}
		
		private void launchSetTheIp(string ip) {
			if (Set_Ip != null) {
				Set_Ip(ip);
			}
		}
		
		private void launchSetColor(string color) {
			if (SetColor != null) {
				SetColor(color);
			}
		}
		
		public void startService(){
			if (serviceHost == null) {
				serviceHost = new WebServiceHost(this, new Uri("http://0.0.0.0:8081/FlHubber"));
			}
			serviceHost.Open();
			myIpToServer();
		}
		
		public void stopService(){
			serviceHost.Close();
		}
 
		public delegate void Output_Color_Signature(string val);
	
		public event Output_Color_Signature SetColor;
		public event Output_Color_Signature Ret_Config;
		public event Output_Color_Signature Ret_Meteo;
		public event Output_Color_Signature Set_Ip;
	}
	
	
}
