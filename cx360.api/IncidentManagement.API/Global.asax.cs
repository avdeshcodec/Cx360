using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Net;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IncidentManagement.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            if (ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12) == false)
            {
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            }
            // For converting DateTime into Date (Meeting History.)
            JsonMediaTypeFormatter jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;            JsonSerializerSettings jSettings = new Newtonsoft.Json.JsonSerializerSettings()            {                Formatting = Formatting.Indented,                DateTimeZoneHandling = DateTimeZoneHandling.Utc            };            jSettings.Converters.Add(new MyDateTimeConvertor());            jsonFormatter.SerializerSettings = jSettings;            //============
        }
    }
    // For converting DateTime into Date.
    public class MyDateTimeConvertor : DateTimeConverterBase    {        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)        {            return DateTime.Parse(reader.Value.ToString());        }        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)        {            writer.WriteValue(((DateTime)value).ToString("MM/dd/yyyy")); //yyyy/MM/dd
        }    }    //=========
}
