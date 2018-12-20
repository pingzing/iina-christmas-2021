using KChristmas.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace KChristmas.Core
{
    public class NetworkService
    {
        private HttpClient _httpClient;

        private const string GetGiftHintsUrl = "https://kc2016.azurewebsites.net/api/GetGiftHints?code=Z8RSw1TEMQtGP36KqEIax5bj/XRSvJQBAalBmteLYhWwrk9ldqSFhA==";
        private const string GetPinkieEventsUrl = "https://kc2016.azurewebsites.net/api/GetPinkieEvents?code=nUU8YEaQ6Dw4BgoqFAPFjM1NfTFd1hImXbd/EezinkbL9jbsq0ZBdQ==";

        public NetworkService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetGiftHints()
        {
            var response = await _httpClient.GetAsync(GetGiftHintsUrl);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<PinkieEvent[]> GetPinkieEvents()
        {
            var response = await _httpClient.GetAsync(GetPinkieEventsUrl);
            if (!response.IsSuccessStatusCode)
            {
                return new PinkieEvent[0];
            }

            return JsonConvert.DeserializeObject<PinkieEvent[]>(await response.Content.ReadAsStringAsync());
        }
    }
}
