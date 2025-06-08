
namespace ExHooks.Events
{
    public static partial class Invasion
    {
        public class EventPreparationEventArgs : EventArgs { }

        /// <summary>
        /// Called each time a new event is about to start, after Main.invasionType has been reset to 0.<br/>
        /// Allows plugin to run custom code before the event started.
        /// </summary>
        public static event EventHandler<EventPreparationEventArgs>? EventPreparation;

        internal static void InvokeEventPreparation()
        {
            var args = new EventPreparationEventArgs();
            EventPreparation?.Invoke(null, args);
        }
    }
}
