
namespace ExHooks.Events
{
    public static partial class Invasion
    {
        public class ModdedEventUpdateEventArgs : EventArgs { }

        /// <summary>
        /// Called each time there's an update that's not from vanilla events.<br/>
        /// Allows plugin to do update for modded events.
        /// </summary>
        public static event EventHandler<ModdedEventUpdateEventArgs>? ModdedEventUpdate;

        internal static void InvokeModdedEventUpdate()
        {
            var args = new ModdedEventUpdateEventArgs();
            ModdedEventUpdate?.Invoke(null, args);
        }
    }
}
