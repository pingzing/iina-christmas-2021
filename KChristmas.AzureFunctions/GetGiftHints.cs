using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KChristmas.AzureFunctions
{
    public static class GetGiftHints
    {
        private const string GiftHints = "It's smaller than a breadbox.|Careful! It could be fragile!|"
+ "Focus, and shake again.|Better not tell you now.|Prediction hazy, try again.|You hear...chittering?|418 I'M A TEAPOT|"
+ "Sounds blurry.|The world shakes around you, but the box remains motionless.|C̖̲̩̰̼ͫ͐͐͌ͭͥ̓̋̎T̷̫͙̤͗̈̐ͦ̾̓́̈́Ḣ̢͖̘͉͚͚͈̊ͤͥ͆̎̽̀́͘U̖̟̫̳̮̥ͮ̾̈̃͘͢L̢̬̞͖̰̯̲̘ͩ͂͜U̷̲͚̞̤̗̬͍͓͉̿̑̒̊ͯ̆̈́ ̴̬͍̥͉ͦ͝F̢̖͈ͥͤ͐ͯ̋̇ͅ'̸̛̗͕̹͈̝̩̰͇ͤͥ̃ͬ͒͂̊̓Ť̈͆͆ͨͧ̚̕҉̯̣̣̝͍̤͖̬Ȟ̢͚͎̟̓̇͋̍̅ͥ̀Ã̶͎͔̪ͧ̑̑G̸̛̦̠͕̻̼͓͆̽̋̿̂̐͑̚N̨ͪ͌҉͉̳͖͙͚̝͇̱ I̡̹ͨ̊̋̀ͨͭ͜Ḁ̵̥̘̓̾̉̄͐ ̛̻̘̫̻ͭ̀̓̎͜Iͧ͏̫͇̬̲͚͓͙̘̬A̧̓ͩ̈́̇̍͏̘͚̖̳̺̦̳̕ͅ|"
+ "You gently tilt the box on its side and you hear a quiet 'thunk'.|The box rattles. It keeps rattling even after you stop shaking.|"
+ "It doesn't seem very heavy...|Have you tried just asking the box?|The box purrs. At least, you think it's the box.|"
+ "'PLEASURE TO MEET YOU TOO,' the box booms.|You hear a great crash, followed by ominous silence.|"
+ "My junk may or may not be in this box.|Maybe this IS the present?|Smells like Christmas.|Loading...|It's ticking...|"
+ "After you shake it, the box remains suspended in midair.|You hear a satisfied sigh.|"
+ "What if it was an Etch-A-Sketch with a picture?|Not going to be much left at this rate.|"
+ "The box seems happy with all this attention.|Seems abstract.|Seems exciting.|Seems clear.|Seems good.|"
+ "IT'S A WITCH!|The box seems to be pining for the fjords...|Who's a good box?|Pie iesu domine, dona eis requiem.|"
+ "Sounds expensive.|Sounds cheap.|Sounds fragile.|Seems dizzy.|Seems confused.|The box begins to wonder if it's going into withdrawal.|"
+ "BOX ANGRY! BOX SMASH!|You shake the box! But you're still hungry...|Hello to the family!|"
+ "It's so close you can almost taste it!|Not much longer now...|The box is ready for its big debut!|"
+ "The box would like you to know that it's ready for its close-up.|You hear something fall, as though from a great distance.|"
+ "You hear snoring, and what sounds like something mumbling 'Come back later...'|The box ponders a different shape for next year. Maybe a circle?|"
+ "OH YEAH|EEEEEEEEEEEELLLS|The box contemplates pushing your face into the mud. How rude.|˙xoq ǝɥʇ ǝʞɐɥs no⅄|The box shakes back.|"
+ "The box requests that it be stirred next time.|The name's Box. Gift Box.|Shake shake shake. Shakeshakeshake! Shake your booty!|"
+ "Alas, poor boxio. We knew him well.|You hear a voice faintly shout 'We don't want any!'|The box screams.|The box goes flying into the sky.|"
+ "WELP|What's...in the box? Could it be....a rrrrrrubber biscuit?|SIX FOOT, SEVEN FOOT, EIGHT FOOT, BUNCH!|BEEP BOOP INVALID PASSCODE|"
+ "It giggles.|The box moans suggestively.|The box gasps.|You hear what sounds like hooves clopping on stone.|ALL HAIL THE SUN TRIUMPHANT|Praise the sun!|"
+ "The moon refused to yield.|But it failed!|*starving walrus noises*";

        [FunctionName("GetGiftHints")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Returning GiftHints at {DateTime.UtcNow}");

            return new OkObjectResult(GiftHints);
        }
    }
}
