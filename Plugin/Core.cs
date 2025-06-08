using System.Reflection;
using Terraria;
using TerrariaApi.Server;

namespace ExHooks
{
    [ApiVersion(2, 1)]
    public class Core : TerrariaPlugin
    {
        public override string Author => "sors89";
        public override string Description => "Extra hooks that might be useful for TShock modding!!!";
        public override string Name => "ExHooks";
        public override Version Version => new Version(1, 0, 0);

        internal static HashSet<object?>? hookInstances;
        public Core(Main game) : base(game)
        {
            Order = -137;
            hookInstances = Assembly.GetExecutingAssembly().GetTypes()
                        .Where(x => typeof(IHook).IsAssignableFrom(x) && x != typeof(IHook))
                        .Select(x => Activator.CreateInstance(x))
                        .ToHashSet();
        }

        public override void Initialize()
        {
            if (hookInstances == null)
            {
                return;
            }

            foreach (var hookins in hookInstances)
            {
                hookins?.GetType().GetMethod("Register")!.Invoke(hookins, null);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (hookInstances == null)
                {
                    return;
                }

                foreach (var hookins in hookInstances)
                {
                    hookins?.GetType().GetMethod("Deregister")!.Invoke(hookins, null);
                }
            }
            base.Dispose(disposing);
        }
    }
}
