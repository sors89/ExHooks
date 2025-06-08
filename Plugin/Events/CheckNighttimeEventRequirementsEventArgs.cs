
namespace ExHooks.Events
{
    public static partial class World
    {
        public class CheckNighttimeEventRequirementsEventArgs : EventArgs
        {
            public bool Handled { get; set; }
        }

        /// <summary>
        /// Called each time a new night started (at 7:30 pm).<br/>
        /// Allows plugin to intercept event checking.
        /// </summary>
        public static event EventHandler<CheckNighttimeEventRequirementsEventArgs>? CheckNighttimeEventRequirements;

        internal static bool InvokeCheckNighttimeEventRequirements()
        {
            var args = new CheckNighttimeEventRequirementsEventArgs();
            CheckNighttimeEventRequirements?.Invoke(null, args);

            return args.Handled;
        }
    }
}
