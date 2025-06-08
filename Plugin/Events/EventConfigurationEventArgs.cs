
namespace ExHooks.Events
{
    public static partial class Invasion
    {
        public class EventConfigurationEventArgs : EventArgs
        {
            public int EventID;
            public bool Handled;
        }

        /// <summary>
        /// Called each time before an event's properties are configured.<br/>
        /// Allows plugin to modify event properties.
        /// </summary>
        public static event EventHandler<EventConfigurationEventArgs>? EventConfiguration;

        internal static bool InvokeEventConfiguration(int eventId)
        {
            var args = new EventConfigurationEventArgs
            {
                EventID = eventId
            };
            EventConfiguration?.Invoke(null, args);

            return args.Handled;
        }
    }
}
