using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace ExHooks.Mods
{
    internal class PostGameTimeUpdate : IHook
    {
        public void Register()
            => IL.Terraria.Main.UpdateTime += ILHook_UpdateTime_PostGameTimeUpdate;

        private static void ILHook_UpdateTime_PostGameTimeUpdate(ILContext ctx)
        {
            var csr = new ILCursor(ctx);
            csr.GotoNext(MoveType.After, i => i.MatchStsfld(typeof(Terraria.Main), "time"));

            csr.EmitDelegate(ExHooks.Events.Game.InvokePostGameTimeUpdate);
        }

        public void Deregister()
            => IL.Terraria.Main.UpdateTime -= ILHook_UpdateTime_PostGameTimeUpdate;
    }
}
