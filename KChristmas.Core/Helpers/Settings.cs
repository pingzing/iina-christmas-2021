using Xamarin.Essentials;
using System;

namespace KChristmas.Core.Helpers
{
    public static class Settings
    {
        #region Setting Constants

        private const string GiftAcceptedKey = "GiftAcceptedKey";
        private static readonly bool GiftAcceptedDefault = false;

        private const string LastSeenVersionKey = "LastSeenVersion";
        private static readonly Version LastSeenVersionDefault = new Version(1, 0, 0);

        private const string PinkieSeenCountKey = nameof(PinkieSeenCountKey);
        private static readonly int PinkieSeenCountDefault = 0;

        #endregion

        public static bool GiftAccepted
        {
            get { return Preferences.Get(GiftAcceptedKey, GiftAcceptedDefault); }
            set { Preferences.Set(GiftAcceptedKey, value); }
        }

        /// <summary>
        /// A json array of strings.
        /// </summary>
        public static string GiftHintsV2
        {
            get => Preferences.Get(nameof(GiftHintsV2), null);
            set => Preferences.Set(nameof(GiftHintsV2), value);
        }

        public static int PinkieSeenCount
        {
            get => Preferences.Get(PinkieSeenCountKey, PinkieSeenCountDefault);
            set => Preferences.Set(PinkieSeenCountKey, value);
        }

        public static Version LastSeenVersion
        {
            get
            {
                string lastSeenVersion = Preferences.Get(LastSeenVersionKey, (string)null);
                if (String.IsNullOrWhiteSpace(lastSeenVersion))
                {
                    return LastSeenVersionDefault;
                }
                else
                {
                    return new Version(lastSeenVersion);
                }
            }
            set
            {
                string versionAsString = value.ToString(3);
                Preferences.Set(LastSeenVersionKey, versionAsString);
            }
        }

    }
}