using Plugin.Settings;
using Plugin.Settings.Abstractions;

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

        #endregion


        public static bool GiftAccepted
        {
            get { return AppSettings.GetValueOrDefault<bool>(GiftAcceptedKey, GiftAcceptedDefault); }
            set { AppSettings.AddOrUpdateValue<bool>(GiftAcceptedKey, value); }
        }

        public static bool GiftRedeemed
        {
            get { return AppSettings.GetValueOrDefault<bool>(GiftRedeemedKey, GiftRedeemedDefault); }
            set { AppSettings.AddOrUpdateValue<bool>(GiftRedeemedKey, value); }
        }

        public static bool IntroComplete
        {
            get { return AppSettings.GetValueOrDefault<bool>(IntroCompleteKey, IntroCompleteDefault); }
            set { AppSettings.AddOrUpdateValue<bool>(IntroCompleteKey, value); }
        }

    }
}