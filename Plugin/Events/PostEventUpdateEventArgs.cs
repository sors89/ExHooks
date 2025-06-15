
namespace ExHooks.Events
{
    public static partial class Invasion
    {
        public class PostEventUpdateEventArgs : EventArgs { }

        /// <summary>
        /// Called on every game ticks to update the on-going event.
        /// Allows plugin to run custom code.
        /// </summary>
        public static event EventHandler<PostEventUpdateEventArgs>? PostEventUpdate;

        internal static void InvokePostEventUpdate()
        {
            var args = new PostEventUpdateEventArgs();
            PostEventUpdate?.Invoke(null, args);
        }
    }
}
