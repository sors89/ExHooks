
namespace ExHooks.Events
{
    public static partial class World
    {
        public class CheckDaytimeEventRequirementsEventArgs : EventArgs
        {
            public bool Handled { get; set; }
        }

        /// <summary>
        /// Called each time a new day started (at 4:30 am).<br/>
        /// Allows plugin to intercept event checking.
        /// </summary>
        public static event EventHandler<CheckDaytimeEventRequirementsEventArgs>? CheckDaytimeEventRequirements;

        internal static bool InvokeCheckDaytimeEventRequirements()
        {
            var args = new CheckDaytimeEventRequirementsEventArgs();
            CheckDaytimeEventRequirements?.Invoke(null, args);

            return args.Handled;
        }
    }
}
