using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace appcitas.Services
{
    public static class BACWS
    {
        public static async Task<BACObject> GetGeneralByTC(string tarjeta)
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri(WebConfigurationManager.AppSettings["BACAPIBaseAddress"].ToString()) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage responseMessage = await client.GetAsync(new Uri(WebConfigurationManager.AppSettings["GetGeneralByTC"].ToString() + tarjeta));

            BACObject _BACObject = null;

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                _BACObject = JsonConvert.DeserializeObject<BACObject>(responseData);
            }

            return _BACObject;
        }

        public static async Task<BACObject> GetInfoClientByCif(string cuenta)
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri(WebConfigurationManager.AppSettings["BACAPIBaseAddress"].ToString()) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage responseMessage = await client.GetAsync(new Uri(WebConfigurationManager.AppSettings["GetInfoClientByCif"].ToString() + cuenta));

            BACObject _BACObject = null;

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                _BACObject = JsonConvert.DeserializeObject<BACObject>(responseData);
            }

            return _BACObject;
        }

        public static async Task<BACObject> GetValuesByCif(string cuenta)
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri(WebConfigurationManager.AppSettings["BACAPIBaseAddress"].ToString()) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage responseMessage = await client.GetAsync(new Uri(WebConfigurationManager.AppSettings["GetValuesByCif"].ToString() + cuenta));

            BACObject _BACObject = null;

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                _BACObject = JsonConvert.DeserializeObject<BACObject>(responseData);
            }

            return _BACObject;
        }
    }
}