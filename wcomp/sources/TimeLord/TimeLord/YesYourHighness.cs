using System;
using System.Collections.Generic;
using BasicBeans;
using Newtonsoft.Json;
using WComp.Beans;

namespace WComp.Beans
{
	[Bean(Category="FlHubber")]
	public class YesYourHighness
	{
		private static List<string> currentDevices = new List<string>();
		private static Dictionary<string, BasicBeans.Timer> timers = new Dictionary<string, WComp.BasicBeans.Timer>();
		private static Dictionary<string, int> usersTicks = new Dictionary<string, int>();
		private static Dictionary<string, int> usersMaxTicks = new Dictionary<string, int>();
	
		public void setConfig(string jsonString) {
			if (jsonString.Length == 0) {
				ifYouWantToLaunchTheUselessEvent("FAIL: length == 0");
				return;
			}
			try {
				Dictionary<string, object> config = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
				if ((null != config) && (config.ContainsKey("id")) && (config.ContainsKey("timeBeforeAlert"))) {
					if (null == config["timeBeforeAlert"]) {
						ifYouWantToLaunchTheUselessEvent("FAIL: null == config[timeBeforeAlert]");
					} else {
						int timeBeforeAlert = JsonConvert.DeserializeObject<int>(config["timeBeforeAlert"].ToString());
						string id = config["id"].ToString();
						if (usersMaxTicks.ContainsKey(id)) {
							usersMaxTicks.Remove(id);
						}
						usersMaxTicks.Add(id, timeBeforeAlert);
					}
				} else {
					ifYouWantToLaunchTheUselessEvent("FAIL: else");
				}
			} catch {
				ifYouWantToLaunchTheUselessEvent("FAIL catch");
			}
		}
		
		private void portsInAction(List<string> portsIn) {
			foreach (string id in portsIn) {
				stopTimer(id);
			}
		}
		
		private void portsOutAction(List<string> portsOut) {
			foreach (string id in portsOut) {
				currentDevices.Remove(id);
				launchTimer(id);
				bonjourConfig(id);
				//maxTicksReached(id);
			}
		}
		
		public void mymethodstring(string json) {
			try {
				Dictionary<string, object> config = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
				if ((null != config) && (config.ContainsKey("id")) && (config.ContainsKey("timeBeforeAlert"))) {
					if (null == config["timeBeforeAlert"]) {
						ifYouWantToLaunchTheUselessEvent("timeBeforeAlert NULL");
					} else {
						ifYouWantToLaunchTheUselessEvent(config["timeBeforeAlert"].ToString());
					}
				} else {
					ifYouWantToLaunchTheUselessEvent("else params");
				}
			} catch {
				ifYouWantToLaunchTheUselessEvent("FAIL");
			}
		}
		
		public void mymethod() {
			for (int i = 0; i < 5; i++) {
				launchTimer("abac " + i);
				updateCurrentPorts("yoga fire " + i);
				bonjourConfig("bi chneck " + i);
			}
		}
		
		public void CurrentPorts(string jsonString) {
			if (jsonString.Length == 0) {
				return;
			}
			
			List<string> tmpDevices = new List<string>();
			try {
				List<Dictionary<string, string>> portsInfo = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonString);
				if (null != portsInfo) {
					foreach (Dictionary<string, string> port in portsInfo) {
						if ((port.ContainsKey("idVendor")) && (port.ContainsKey("idProduct")) && (port.ContainsKey("iSerialNumber"))) {
							string id = port["idVendor"] + port["idProduct"] + port["iSerialNumber"];
							tmpDevices.Add(id);
						}
					}
				}
			} catch {}
			
			List<string> portsIn = new List<string>();
			List<string> portsOut = new List<string>();
			
			foreach (string id in currentDevices) {
				if (!tmpDevices.Contains(id)) {
					portsOut.Add(id);
				}
			}
			
			foreach (string id in tmpDevices) {
				if (!currentDevices.Contains(id)) {
					portsIn.Add(id);
					currentDevices.Add(id);
				}
			}
			
			portsOutAction(portsOut);
			portsInAction(portsIn);
			
			string portsString = "";
			string jsonPorts = "[";
			if (currentDevices.Count > 0) {
				for (int i = 0; i < currentDevices.Count - 1; i++) {
					portsString += currentDevices[i] + ",";
					jsonPorts += currentDevices[i] + ",";
				}
				portsString += currentDevices[currentDevices.Count - 1];
				jsonPorts += currentDevices[currentDevices.Count - 1];
			}
			jsonPorts += "]";
			
			updateCurrentPorts(jsonString);
		}
		
		private void launchTimer(string id) {
			if (timers.ContainsKey(id)) {
				timers.Remove(id);
			}
			timers.Add(id, new WComp.BasicBeans.Timer());
			timers[id].Period = 1000;
			timers[id].TimerTick += () => incTicks(id);
			timers[id].Start();
			if (usersTicks.ContainsKey(id)) {
				usersTicks.Remove(id);
			}
			usersTicks.Add(id, 0);
			if (!usersMaxTicks.ContainsKey(id)) {
				usersMaxTicks.Add(id, 0);
			}
		}
		
		private void stopTimer(string id) {
			if (timers.ContainsKey(id)) {
				timers[id].Stop();
				timers[id] = null;
				timers.Remove(id);
			}
		}
		
		private void incTicks(string id) {
			if (usersTicks.ContainsKey(id)) {
				usersTicks[id] += 1;
				if ((usersMaxTicks.ContainsKey(id)) && (usersMaxTicks[id] == usersTicks[id])) {
					maxTicksReached(id);
				}
			}
		}
		
		public delegate void StringValueEventHandler(string val);
		public delegate void StringCurrentPorts(string val);
		public delegate void AnotherConfigs(string util);
		public delegate void AUselessStringEvent(string use);
		
		public event StringCurrentPorts CurrentPorts_Return;
		public event StringValueEventHandler MaxReached;
		public event AnotherConfigs HelloConfig;
		public event AUselessStringEvent HeyMyEvent;
		
		private void updateCurrentPorts(string ids) {
			if (CurrentPorts_Return != null) {
				CurrentPorts_Return(ids);
			}
		}
		
		private void maxTicksReached(string id) {
			if (MaxReached != null) {
				MaxReached(id);
			}
		}
		
		private void bonjourConfig(string id) {
			if (HelloConfig != null) {
				HelloConfig(id);
			}
		}
		
		private void ifYouWantToLaunchTheUselessEvent(string mystr) {
			if (HeyMyEvent != null) {
				HeyMyEvent(mystr);
			}
		}
	}
}
