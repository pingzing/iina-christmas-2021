using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using KChristmas.Models;

namespace KChristmas.AzureFunctions
{
    public static class GetPinkieEvents
    {
        private static readonly List<PinkieEvent> _storedPinkieEvents = new List<PinkieEvent>
        {
            new PinkieEvent(Guid.Parse("c7d8c4d5-3d75-4ee9-ac80-51a68b5ea1c9")) {
                { "pinkie_confused.png", "Soooooo...", 2000 },
                { null, "I don't get it.", 3000 },
                { null, "Shouldn't all this shaking break the present?", 3000 },
                { "pinkie_bounce_up_3.png", "Unless...", 3000 },
                { null, "It's a SHAKE-POWERED-PRESENT! AHHHH!", 4000 }
            },
        };

        [FunctionName("GetPinkieEvents")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Returning PinkieEvents at {DateTime.UtcNow}");

            string pinkieEventsJson = JsonConvert.SerializeObject(_storedPinkieEvents);

            return new OkObjectResult(pinkieEventsJson);
        }
    }
}
