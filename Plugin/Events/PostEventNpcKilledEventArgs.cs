using Terraria;

namespace ExHooks.Events
{
    public static partial class Invasion
    {
        public class PostEventNpcKilledEventArgs : EventArgs
        {
            public NPC? NPC { get; set; }
        }

        /// <summary>
        /// Called each time an npc is killed, regardless of whether the npc is part of an event.<br/>
        /// Works similarly to <see cref="OTAPI.Hooks.NPC.Killed"/>, but this hook is always invoked after the vanilla invasion size has been updated.<br/>
        /// So this hook functions identically to OTAPI's NpcKilled hook when the npc is not part of an event.
        /// The only difference is that the npc will no longer be active when this hook is called.
        /// </summary>
        public static event EventHandler<PostEventNpcKilledEventArgs>? PostEventNpcKilled;

        internal static void InvokePostEventNpcKilled(NPC npc)
        {
            var args = new PostEventNpcKilledEventArgs
            {
                NPC = npc
            };
            PostEventNpcKilled?.Invoke(null, args);
        }
    }
}
