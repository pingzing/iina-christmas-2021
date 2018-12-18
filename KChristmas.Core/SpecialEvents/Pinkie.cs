﻿using KChristmas.Core.Helpers;
using KChristmas.Core.XamlExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using KChristmas.Core.Extensions;

namespace KChristmas.Core.SpecialEvents
{
    public class Pinkie
    {
        private static readonly Guid _firstTimeScriptGuid = Guid.Parse("0b3a9635-c6e2-4bc4-804a-59f614925fd2");
        private static readonly List<(string newImagePath, string text, int delay)> _firstTimePinkieScript = new List<(string newImagePath, string text, int delay)>
        {
            { ("pinkie_bounce_up_3.png", "Hi!", 3000) },
            { ("pinkie_confused.png", "Hey, wait a minute...", 4000) },
            { (null, "...this isn't the Hearth's Warming Eve party!", 4000) },
            { ("pinkie_bounce_up_3.png", "Ohmigosh, I gotta get going!", 4000) },
            { (null, "I hope you're having lots and lots of fun though!", 3000) },
            { (null, "Byeeeee!", 3000) }
        };
        // Doesn't include the first time script, as it's a special case.
        private Dictionary<Guid, List<(string, string, int)>> _pinkieScripts = new Dictionary<Guid, List<(string, string, int)>>
        {
            { Guid.Parse("73d3b631-41b2-4fc3-a9e9-543c7675e6a8"), new List<(string, string, int)> {
                (null, "Oh, hi again!", 3000),
                (null, "Fancy meeting you here, hee hee!", 4000),
                (null, "I'm actually here to tell you...", 4000),
                ("pinkie_confused.png", "Um, hang on, I wrote it on my hoof...", 4000),
                (null, "Oh! Here! 'You should stop breaking the fourth wall...", 3000),
                (null, "...just to deliver present hints?'", 5000),
                ("pinkie_bounce_up_3.png", "Oh! That was a message for me! Hehe, sorry!", 4000),
                (null, "Anyway, gotta go again! Bye!", 3000)
            }},
            { Guid.Parse("bd0f3f57-1a7c-4ac5-9c8f-eea37c3a2d4e"), new List<(string, string, int)> {
                (null, "Boo!", 3000),
                ("pinkie_confused.png", "Oh wait, wrong holiday.", 4000),
                ("pinkie_bounce_up_3.png", "Anyway, I think I know what your present is!", 4000),
                ("pinkie_confused.png", "...exceeeeept I'm not supposed to tell you.", 4000),
                ("pinkie_bounce_up_3.png", "Sorry!", 2000)
            }},
        };
        private List<Guid> _seenScripts = new List<Guid>();

        private MainPage _mainPageReference;

        public Pinkie(MainPage mainPage)
        {
            _mainPageReference = mainPage;
        }

        public async Task Run(Button giftBase, Button giftTop, AbsoluteLayout specialEventCanvas)
        {
            Rectangle baseRect = giftBase.Bounds;
            double xMid = baseRect.X + baseRect.Width / 2;
            double pinkieHeight = 94;
            double pinkieWidth = 94;

            Image pinkieImage = new Image
            {
                HeightRequest = 94,
                WidthRequest = 94,
                Source = ImageExtension.GetPlatformIndependentPath("pinkie_woundup_1.png")
            };
            double pinkieMid = pinkieImage.WidthRequest / 2;
            pinkieImage.Rotation = 270;

            AbsoluteLayout.SetLayoutBounds(pinkieImage, new Rectangle(xMid - pinkieMid, baseRect.Y - 40, pinkieWidth, pinkieHeight));
            specialEventCanvas.Children.Add(pinkieImage);

            double totalMillis = 3000.0;
            double tick = 25.0 / totalMillis;
            var boxShudderSlamOpen = new Animation
            {
                { 0.0, tick * 1, new Animation(v => { giftBase.TranslationX = v; giftTop.TranslationX = v; }, 0, 10) },
                { tick * 1, tick * 2, new Animation(v => { giftBase.TranslationX = v; giftTop.TranslationX = v; }, 10, -10) },
                { tick * 2, tick * 3, new Animation(v => { giftBase.TranslationX = v; giftTop.TranslationX = v; }, 10, -10) },
                { tick * 3, tick * 4, new Animation(v => { giftBase.TranslationX = v; giftTop.TranslationX = v; }, -10, 0) },
                { tick * 4, tick * 5, new Animation(v => { giftBase.TranslationY = v; giftTop.TranslationY = v; }, 0, 10) },
                { tick * 5, tick * 6, new Animation(v => { giftBase.TranslationY = v; giftTop.TranslationY = v; }, 10, -10) },
                { tick * 6, tick * 7, new Animation(v => { giftBase.TranslationY = v; giftTop.TranslationY = v; }, -10, 0) },

                { tick * 7, tick * 8, new Animation(v => { giftBase.TranslationX = v; giftTop.TranslationX = v; }, 10, -10) },
                { tick * 8, tick * 9, new Animation(v => { giftBase.TranslationX = v; giftTop.TranslationX = v; }, 10, -10) },
                { tick * 9, tick * 10, new Animation(v => { giftBase.TranslationX = v; giftTop.TranslationX = v; }, -10, 0) },
                { tick * 10, tick * 11, new Animation(v => { giftBase.TranslationY = v; giftTop.TranslationY = v; }, 0, 10) },
                { tick * 11, tick * 12, new Animation(v => { giftBase.TranslationY = v; giftTop.TranslationY = v; }, 10, -10) },
                { tick * 12, tick * 13, new Animation(v => { giftBase.TranslationY = v; giftTop.TranslationY = v; }, -10, 0) },

                { tick * 13, tick * 14, new Animation(v => { giftBase.TranslationX = v; giftTop.TranslationX = v; }, 10, -10) },
                { tick * 14, tick * 15, new Animation(v => { giftBase.TranslationX = v; giftTop.TranslationX = v; }, 10, -10) },
                { tick * 15, tick * 16, new Animation(v => { giftBase.TranslationX = v; giftTop.TranslationX = v; }, -10, 0) },
                { tick * 16, tick * 17, new Animation(v => { giftBase.TranslationY = v; giftTop.TranslationY = v; }, 0, 10) },
                { tick * 17, tick * 18, new Animation(v => { giftBase.TranslationY = v; giftTop.TranslationY = v; }, 10, -10) },
                { tick * 18, tick * 19, new Animation(v => { giftBase.TranslationY = v; giftTop.TranslationY = v; }, -10, 0) },

                { tick * 19, tick * 20, new Animation(v => { giftBase.TranslationX = v; giftTop.TranslationX = v; }, 10, -10) },
                { tick * 20, tick * 21, new Animation(v => { giftBase.TranslationX = v; giftTop.TranslationX = v; }, 10, -10) },
                { tick * 21, tick * 22, new Animation(v => { giftBase.TranslationX = v; giftTop.TranslationX = v; }, -10, 0) },
                { tick * 22, tick * 23, new Animation(v => { giftBase.TranslationY = v; giftTop.TranslationY = v; }, 0, 10) },
                { tick * 23, tick * 24, new Animation(v => { giftBase.TranslationY = v; giftTop.TranslationY = v; }, 10, -10) },
                { tick * 24, tick * 25, new Animation(v => { giftBase.TranslationY = v; giftTop.TranslationY = v; }, -10, 0) },

                { tick * 25, tick * 26, new Animation(v => { giftBase.TranslationX = v; giftTop.TranslationX = v; }, 10, -10) },
                { tick * 26, tick * 27, new Animation(v => { giftBase.TranslationX = v; giftTop.TranslationX = v; }, 10, -10) },
                { tick * 27, tick * 28, new Animation(v => { giftBase.TranslationX = v; giftTop.TranslationX = v; }, -10, 0) },
                { tick * 28, tick * 29, new Animation(v => { giftBase.TranslationY = v; giftTop.TranslationY = v; }, 0, 10) },
                { tick * 29, tick * 30, new Animation(v => { giftBase.TranslationY = v; giftTop.TranslationY = v; }, 10, -10) },
                { tick * 30, tick * 31, new Animation(v => { giftBase.TranslationY = v; giftTop.TranslationY = v; }, -10, 0) },

                { 0.95, 0.97, new Animation(v => {pinkieImage.Source = ImageExtension.GetPlatformIndependentPath("pinkie_woundup_2.png"); }, 0, 0) },
                { 0.95, 0.98, new Animation(v => {giftBase.TranslationY = v; }, 0, -30, Easing.SpringOut) },
                { 0.97, 1.0, new Animation(v => {pinkieImage.Source = ImageExtension.GetPlatformIndependentPath("pinkie_woundup_3.png"); }, 0, 0) },
                { 0.98, 1.0, new Animation(v => {giftBase.TranslationY = v; }, -30, 0, Easing.SpringOut) },
                { 0.95, 1.0, new Animation(v => { giftTop.TranslationY = v; }, 0, -300, Easing.CubicOut) }
            };

            boxShudderSlamOpen.Commit(giftBase, "BoxShduder", 16, (uint)totalMillis);
            await Task.Delay((int)totalMillis);

            await pinkieImage.TranslateTo(0, 50, 1000, Easing.SpringIn);

            pinkieImage.Rotation = 0;
            pinkieImage.Source = ImageExtension.GetPlatformIndependentPath("pinkie_bounce_up_3.png");
            await pinkieImage.TranslateTo(0, -20, 300, Easing.SpringOut);

            if (Settings.PinkieSeenCount == 0)
            {
                await RunPinkieScript(_firstTimePinkieScript, pinkieImage);                
            }
            else
            {
                if (_seenScripts.Count == _pinkieScripts.Count)
                {
                    _seenScripts.Clear();
                }

                var randomScript = _pinkieScripts
                    .Where(x => !_seenScripts.Any(y => x.Key == y))
                    .ToDictionary(keypair => keypair.Key, keypair => keypair.Value)
                    .RandomEntry();

                await RunPinkieScript(randomScript.Value, pinkieImage);
                _seenScripts.Add(randomScript.Key);
            }
            Settings.PinkieSeenCount += 1;

            await pinkieImage.TranslateTo(0, 40, 300, Easing.SpringIn);
            pinkieImage = null;
            specialEventCanvas.Children.Clear();
            await giftTop.TranslateTo(0, 0, 300, Easing.SpringIn);
        }

        private async Task RunPinkieScript(List<(string, string, int)> script, Image pinkieImage)
        {
            Task _; // dummy to keep the compiler quiet
            foreach (var line in script)
            {
                (string newImage, string text, int delay) = line;
                if (newImage != null)
                {
                    pinkieImage.Source = ImageExtension.GetPlatformIndependentPath(newImage);
                }
                _ = _mainPageReference.ShowFloatingText(text, Color.HotPink);
                await Task.Delay(delay);
            }
        }
    }
}
