using KChristmas.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KChristmas.Core
{
    public class NetworkService
    {
        private HttpClient _httpClient;

#if DEBUG
        private readonly string _baseUrl;
#else
        private readonly string _baseUrl = "https://kc2016.azurewebsites.net/";
#endif

        private readonly string GetGiftHintsUrl = "api/GetGiftHints?code=Z8RSw1TEMQtGP36KqEIax5bj/XRSvJQBAalBmteLYhWwrk9ldqSFhA==";
        private readonly string GetPinkieEventsUrl = "api/GetPinkieEvents?code=nUU8YEaQ6Dw4BgoqFAPFjM1NfTFd1hImXbd/EezinkbL9jbsq0ZBdQ==";

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
