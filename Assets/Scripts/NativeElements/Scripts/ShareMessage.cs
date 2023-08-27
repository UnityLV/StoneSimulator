using NativeAndroidElements;
namespace NativeElements.Scripts
{
    public static class ShareMessage
    {
        public static void Share(string message)
        {
            TextIntent intent = new TextIntent();
            intent.ShareText("VIP Invite", message, "shareTagStoneSim");
        }
    }
}