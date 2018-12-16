using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace KChristmas.Core.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants
        private const string IntroCompleteKey = "IntroCompleteKey";
        private static readonly bool IntroCompleteDefault = false;

        private const string GiftAcceptedKey = "GiftAcceptedKey";
        private static readonly bool GiftAcceptedDefault = false;

        private const string GiftRedeemedKey = "GiftRedeemedKey";
        private static readonly bool GiftRedeemedDefault = false;

        private const string GiftHintsKey = "GiftHintsKeys";
        private static readonly string GiftHintsDefault = null;

        private const string LastSeenVersionKey = "LastSeenVersion";
        private static readonly Version LastSeenVersionDefault = new Version(1, 0, 0);

        #endregion

        public static bool GiftAccepted
        {
            get { return AppSettings.GetValueOrDefault(GiftAcceptedKey, GiftAcceptedDefault); }
            set { AppSettings.AddOrUpdateValue(GiftAcceptedKey, value); }
        }

        [Obsolete("No longer used! Just using GiftAccepted now.")]
        public static bool GiftRedeemed
        {
            get { return false; }
            set { AppSettings.AddOrUpdateValue(GiftRedeemedKey, value); }
        }

        [Obsolete("No longer used in v4.")]
        public static bool IntroComplete
        {
            get { return AppSettings.GetValueOrDefault(IntroCompleteKey, IntroCompleteDefault); }
            set { AppSettings.AddOrUpdateValue(IntroCompleteKey, value); }
        }

        public static string GiftHints
        {
            get { return AppSettings.GetValueOrDefault(GiftHintsKey, GiftHintsDefault); }
            set { AppSettings.AddOrUpdateValue(GiftHintsKey, value); }
        }

        public static Version LastSeenVersion
        {
            get
            {
                string lastSeenVersion = AppSettings.GetValueOrDefault(LastSeenVersionKey, (string)null);
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
                AppSettings.AddOrUpdateValue(LastSeenVersionKey, versionAsString);
            }
        }

    }
}