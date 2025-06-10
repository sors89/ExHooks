using Terraria.Localization;

namespace ExHooks.Events
{
    public static partial class Invasion
    {
        public class ModdedEventAnnouncementEventArgs : EventArgs
        {
            public LocalizedText? AnnouncementMsg { get; set; }
            public int MsgR { get; set; }
            public int MsgG { get; set; }
            public int MsgB { get; set; }
        }

        /// <summary>
        /// Called each time there's an event announcement message that's not from vanilla.<br/>
        /// Allows plugin to customize modded event announcement message.
        /// </summary>
        public static event EventHandler<ModdedEventAnnouncementEventArgs>? ModdedEventAnnouncement;

        internal static void InvokeModdedEventAnnouncement(ref LocalizedText msg, ref int r, ref int g, ref int b)
        {
            var args = new ModdedEventAnnouncementEventArgs
            {
                AnnouncementMsg = msg,
                MsgR = r,
                MsgG = g,
                MsgB = b
            };
            ModdedEventAnnouncement?.Invoke(null, args);
            msg = args.AnnouncementMsg;
            r = args.MsgR; g = args.MsgG; b = args.MsgB;
        }
    }
}
