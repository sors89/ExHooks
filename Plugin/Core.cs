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
        public override Version Version => new Version(1, 1, 2);

        internal static HashSet<IHook>? hookInstances;
        public Core(Main game) : base(game)
        {
            Order = -137;
            hookInstances = Assembly.GetExecutingAssembly().GetTypes()
                        .Where(x => typeof(IHook).IsAssignableFrom(x) && x != typeof(IHook))
                        .Select(Activator.CreateInstance).OfType<IHook>()
                        .ToHashSet();
        }

        public override void Initialize()
        {
            foreach (var hookins in hookInstances!)
            {
                hookins.Register();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var hookins in hookInstances!)
                {
                    hookins.Deregister();
                }
            }
            base.Dispose(disposing);
        }
    }
}
