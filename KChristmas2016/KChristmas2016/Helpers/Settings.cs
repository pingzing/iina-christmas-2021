using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace KChristmas2016.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

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
            get { return AppSettings.GetValueOrDefault<bool>(GiftAcceptedKey, GiftAcceptedDefault); }
            set { AppSettings.AddOrUpdateValue<bool>(GiftAcceptedKey, value); }
        }

        [Obsolete("No longer used! Just using GiftAccepted now.")]
        public static bool GiftRedeemed
        {
            get { return false; }
            set { AppSettings.AddOrUpdateValue<bool>(GiftRedeemedKey, value); }
        }

        public static bool IntroComplete
        {
            get { return AppSettings.GetValueOrDefault<bool>(IntroCompleteKey, IntroCompleteDefault); }
            set { AppSettings.AddOrUpdateValue<bool>(IntroCompleteKey, value); }
        }

        public static string GiftHints
        {
            get { return AppSettings.GetValueOrDefault<string>(GiftHintsKey, GiftHintsDefault); }
            set { AppSettings.AddOrUpdateValue<string>(GiftHintsKey, value); }
        }

        public static Version LastSeenVersion
        {
            get
            {
                string lastSeenVersion = AppSettings.GetValueOrDefault(LastSeenVersionKey, (string)null);
                if (lastSeenVersion == null)
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
                AppSettings.AddOrUpdateValue<string>(LastSeenVersionKey, versionAsString);
            }
        }

    }
}