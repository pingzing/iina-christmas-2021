using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using IinaChristmas.Models;

namespace IinaChristmas.AzureFunctions
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
            new PinkieEvent(Guid.Parse("d09f236d-cc83-477c-b159-7d6c0a32cf9a")) {
                { null, "My head is all spinny...", 4000 },
                { null, "SHAKE HARDER! Wheeeee!", 3000 }
            },
            new PinkieEvent(Guid.Parse("d32836f2-cc0e-41e2-a611-36ddae0168cf")) {
                { null, "There's over eighty different kinds of rock!", 4000 },
                { null, "Andesite, aplite, basanite, boninite, blairmorite, dacite...", 4000 },
                { null, "...diorite, dunite, essexite, foidolite, granite, kimberlite...", 3000 },
                { null, "...invite, polite, excite, ignite, erudite, lazurite...", 2000 },
                { null, "...so like, I totally know all about rocks!", 4000 },
                { null, "Which means that I know YOU rock!", 3000 },
            },
            new PinkieEvent(Guid.Parse("5a7dffbf-9ae2-49f3-bcb7-bdc10dc36187")) {
                { null, "Hey!", 2000 },
                { "pinkie_confused.png", "What do you call...", 2000 },
                { null, "A RHINO crossed with an ELEPHANT?", 5000 },
                { "pinkie_bounce_up_3.png", "A RHINOPHANT! *snort* HEE HEE HEE!", 4000 },
            },
            new PinkieEvent(Guid.Parse("eda3de30-39e5-4fcd-9a31-61f23518e7c5")) {
                { null, "♪ It's true some days are dark and lonely... ♪", 4000 },
                { null, "♪ ...and maybe you feel sad... ♪", 3000 },
                { null, "♪ But I will be there to show you that it isn't that bad! ♪", 4000 },
                { null, "♪ There's one thing that makes me happy, ♪", 3000 },
                { null, "♪ ...and makes my whole life worthwhile... ♪", 3000 },
                { null, "♪ ...and that's when I talk to my friends and get them to SMILE! ♪", 5000 },
                { null, "Yeah!", 2000 },
            },
            new PinkieEvent(Guid.Parse("4e56c4db-4843-4a24-abf7-49ef06efc722"))
            {
                { null, "Hey!", 3000 },
                { "pinkie_confused.png", "This isn't Albuquerque!", 4000 },
                { "pinkie_bounce_up_3.png", "Well, probably, anyway.", 4000 },
                { null, "I have no idea what an Albuquerque is!", 4000 },
            },
            new PinkieEvent(Guid.Parse("ff4d57b7-0814-476e-b285-bb06733fd92c"))
            {
                { null, "So, I've been aaaaalll across Equestria.", 4000 },
                { null, "And beyond, even! To some places even *I* couldn't fathom!", 4000 },
                { null, "So believe me when I say that this present?", 4000 },
                { null, "Is totally splen-differ-riffic!", 4000 }
            },
            new PinkieEvent(Guid.Parse("b1690007-4d85-4eaf-a629-c7ad22151fb1"))
            {
                { null, "On the one hoof, I spend a lot of my time baking sweet treats.", 4000 },
                { null, "So I know what it's like to have to wait for something!", 3000 },
                { null, "But I also know that waiting is the wor-hor-hor-hoooorst!", 3000 },
                { "pinkie_confused.png", "So if you're shaking this box and totally excited about what's inside and can't wait?", 4500 },
                { "pinkie_bounce_up_3.png", "I am TOTALLY right there with you!", 3000 },
            },
            new PinkieEvent(Guid.Parse("bbad9f91-d23f-48d6-b313-1e8c229743db"))
            {
                { null, "Hey! You still kickin' that mean ol' cancer's butt?", 3000 },
                { null, "'cause I know you can do it! Show it who's boss!", 3000 },
                { null, "(It's you. You're the boss.)", 3000 },
            },
            new PinkieEvent(Guid.Parse("bdbe16e6-8766-4840-b5d9-bf1771049868"))
            {
                { "pinkie_backwards_1.png", "Oh! Wow, where is everybody?", 3000 },
                { "pinkie_backwards_1.png", "Gosh, it's *super* dark in here!", 3000 },
                { "pinkie_backwards_1.png", "...hey, wait a minute.", 2000 },
                { "pinkie_confused.png", "Huh?", 2000 },
                { "pinkie_bounce_up_3.png", "Oh! There you are!", 2000 },
                { null, "You were all backwards! Hiiii!", 3000 },
            },
            new PinkieEvent(Guid.Parse("1bdd2cf2-214e-4427-95b8-209491c43f9c"))
            {
                { null, "So I wanna show you something super coolio.", 3000 },
                { null, "I've been practicing it ALL NIGHT!", 3000 },
                { null, "It's my super-duper-looper awesome ANTI-CANCER KICK!", 3000 },
                { "pinkie_woundup_vertical.png", "POW!", 3000 },
                { "pinkie_bounce_up_3.png", "See? Whaddaya think? Pretty great, huh?", 3000 },
                { null, "I can teach it to you if you want!", 3000 },
            }
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
