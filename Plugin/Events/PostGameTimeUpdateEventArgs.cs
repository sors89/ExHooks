
namespace ExHooks.Events
{
    public static partial class Game
    {
        public class PostGameTimeUpdateEventArgs : EventArgs { }

        /// <summary>
        /// Called on every game ticks to update the current time in the world.
        /// </summary>
        public static event EventHandler<PostGameTimeUpdateEventArgs>? PostGameTimeUpdate;

        internal static void InvokePostGameTimeUpdate()
        {
            var args = new PostGameTimeUpdateEventArgs();
            PostGameTimeUpdate?.Invoke(null, args);
        }
    }
}
