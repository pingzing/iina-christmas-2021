using IinaChristmas.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace IinaChristmas.Core
{
    public class NetworkService
    {
        private HttpClient _httpClient;

#if DEBUG
        private readonly string _baseUrl;
#else
        private readonly string _baseUrl = "https://iinachristmas2021.azurewebsites.net/";
#endif

        private readonly string GetGiftHintsUrl = "api/GetGiftHints?code=XQDAARH96cErethMWfhmMO6JAMpi3wst/SNEywJ4wRKSyBBTCSctLg==";
        private readonly string GetPinkieEventsUrl = "api/GetPinkieEvents?code=IYFv4alvTa5TwGaFsyQryRW/7a5R866CLa1af72VIOeZ24bfFWc47w==";

        public NetworkService()
        {
            // Android emulators sit behind a virutal router, so they need to use a differnet IP to hit
            // the dev machine's localhost.
#if DEBUG
            _baseUrl = DeviceInfo.Platform == DevicePlatform.Android 
                ? "http://10.0.2.2:7071/"
                : "http://localhost:7071/";
#endif
            _httpClient = new HttpClient();
        }

        public async Task<string?> GetGiftHints()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}{GetGiftHintsUrl}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<PinkieEvent[]> GetPinkieEvents()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}{GetPinkieEventsUrl}");
            if (!response.IsSuccessStatusCode)
            {
                return new PinkieEvent[0];
            }

            return JsonConvert.DeserializeObject<PinkieEvent[]>(await response.Content.ReadAsStringAsync());
        }
    }
}
