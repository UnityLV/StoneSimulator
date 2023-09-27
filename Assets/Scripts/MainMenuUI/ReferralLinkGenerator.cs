using MongoDBCustom;

namespace MainMenuUI
{
    public static class ReferralLinkGenerator
    {
        private static string DeviceId => DeviceInfo.GetDeviceId();
        private const string EmptyLink = "https://play.google.com/store/apps/details?id=com.StoneCompany.StoneSimulator&referrer={0}";
        public static string GetLink()
        {
            return string.Format(EmptyLink, DeviceId);
        }
    }
}