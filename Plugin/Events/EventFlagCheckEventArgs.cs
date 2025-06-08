using Terraria;

namespace ExHooks.Events
{
    public static partial class Invasion
    {
        public class EventFlagCheckEventArgs : EventArgs
        {
            public Player? Player { get; set; }
            public bool AnyOngoingEvent { get; set; }
            public bool Handled { get; set; }
        }

        /// <summary>
        /// Called each time when the game tries to detect any event going on.<br/>
        /// Allows plugin to modify the event checking procedure.
        /// </summary>
        public static event EventHandler<EventFlagCheckEventArgs>? EventFlagCheck;

        internal static bool InvokeEventFlagCheck(Player player, ref bool anyevent)
        {
            var args = new EventFlagCheckEventArgs
            {
                Player = player,
                AnyOngoingEvent = anyevent
            };
            EventFlagCheck?.Invoke(null, args);
            anyevent = args.AnyOngoingEvent;

            return args.Handled;
        }
    }
}
