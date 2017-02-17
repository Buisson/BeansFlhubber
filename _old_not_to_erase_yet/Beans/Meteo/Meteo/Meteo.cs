/*
 * Crée par SharpDevelop.
 * Utilisateur: Meulon
 * Date: 05/02/2017
 * Heure: 14:59
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using System.Collections.Generic;
using WComp.Beans;
using Newtonsoft.Json;

namespace WComp.Beans
{
	/// <summary>
	/// This is a sample bean, which has an integer evented property and a method.
	/// 
	/// Notes: for beans creating threads, the IThreadCreator interface should be implemented,
	/// 	providing a cleanup method should be implemented and named `Stop()'.
	/// For proxy beans, the IProxyBean interface should  be implemented,
	/// 	providing the IsConnected property, allowing the connection status to be drawn in
	/// 	the AddIn's graphical designer.
	/// 
	/// Several classes can be defined or used by a Bean, but only the class with the
	/// [Bean] attribute will be available in WComp. Its ports will be all public methods,
	/// events and properties definied in that class.
	/// </summary>
	[Bean(Category="FlHubber")]
	public class Meteo
	{		
		BasicBeans.Timer timer = new WComp.BasicBeans.Timer();
		int maxTicks = 3600;
		int count = 0;
		
		private float kelvinToCelsius(float temp) {
			return (float)(temp - 273.15);
		}
		
		public void setLeds(string leds) {
			if (timer.Started) {
				timer.Stop();
			}
			LedChanged(leds);
		}
		
		public void getJSONweather(string meteo) {
			try {				
				launchFailEvent("1");
				Dictionary<string, object> meteoJSON = JsonConvert.DeserializeObject<Dictionary<string, object>>(meteo);
				if (!meteoJSON.ContainsKey("main")) {
					launchFailEvent("not main");
					return;
				}
				launchFailEvent("2");
				Dictionary<string, object> tmp = JsonConvert.DeserializeObject<Dictionary<string, object>>(meteoJSON["main"].ToString());
				if (!tmp.ContainsKey("temp")) {
					launchFailEvent("not temp");
					return;
				}
				launchFailEvent("3");
				if (null == tmp["temp"]) {
					launchFailEvent("temp is null");
					return;	
				}
				launchFailEvent("4");
				float temperature = 0;
				string mystr = tmp["temp"].ToString();
				launchFailEvent("5");
				temperature = float.Parse(mystr);
				launchFailEvent("6");
				if(kelvinToCelsius(temperature) < 15.0){
					LedChanged("ORANGE");
				}
				else{
					LedChanged("BLUE");
				}
			}
			catch{
				//launchFailEvent("bou..");
			}
		}
		
		public void launchMeteo() {
			launchAskForMeteoEvent();
			timer.Period = 1000;
			timer.TimerTick += () => incTicks();
			if (!timer.Started) {
				timer.Start();
			}
		}
		
		private void incTicks() {
			count += 1;
			if (count % maxTicks == 0) {
				launchAskForMeteoEvent();
			}
		}
		
		private void launchAskForMeteoEvent() {
			if (AskForMeteo != null) {
				AskForMeteo("");
			}
		}
		
		private void launchFailEvent(string mess) {
			if (FailEvent != null) {
				FailEvent(mess);
			}
		}

		public delegate void StringValueEventHandler(string val);
		public event StringValueEventHandler LedChanged;
		public event StringDelegateHandler AskForMeteo;
		public event StringDelegateHandler FailEvent;

	}
}
