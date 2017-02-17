using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace FlHubberService
{
	/// <summary>_
	/// Description of IRESTService.
	/// </summary>
	[ServiceContract(Name = "FlHubber")]
	public interface IRESTService
	{    	
    	[OperationContract]
    	[WebGet(UriTemplate= "/ConfReturn/{json}", BodyStyle = WebMessageBodyStyle.Bare)]
    	string returnConfigurationEvent(string json);
    	
    	[OperationContract]
    	[WebGet(UriTemplate= "/Meteo/{meteo}", BodyStyle = WebMessageBodyStyle.Bare)]
    	string returnMeteoEvent(string meteo);
	}
}
