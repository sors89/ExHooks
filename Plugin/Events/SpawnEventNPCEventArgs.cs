using Terraria;

namespace ExHooks.Events
{
    public static partial class Invasion
    {
        public class SpawnEventNPCEventArgs : EventArgs
        {
            public Player? Player { get; set; }
            public int TileX { get; set; }
            public int TileY { get; set; }
            public bool Handled { get; set; }
        }

        /// <summary>
        /// Called each time an event npc is about to be spawned.<br/>
        /// Allows plugin to modify enemy list of the event.
        /// </summary>
        public static event EventHandler<SpawnEventNPCEventArgs>? SpawnEventNPC;

        internal static bool InvokeSpawnEventNPC(Player player, int tilex, int tiley)
        {
            var args = new SpawnEventNPCEventArgs
            {
                Player = player,
                TileX = tilex,
                TileY = tiley
            };
            SpawnEventNPC?.Invoke(null, args);

            return args.Handled;
        }
    }
}
