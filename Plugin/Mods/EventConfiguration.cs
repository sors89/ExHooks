using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace ExHooks.Mods
{
    internal class EventConfiguration : IHook
    {
        public void Register()
            => IL.Terraria.Main.StartInvasion += ILHook_StartInvasion_EventConfiguration;

        private static void ILHook_StartInvasion_EventConfiguration(ILContext ctx)
        {
            var csr = new ILCursor(ctx);
            csr.GotoNext(MoveType.After, i => i.OpCode == OpCodes.Brtrue);

            var target = csr.Previous.Operand;
            csr.Emit(OpCodes.Ldarg_0);
            csr.EmitDelegate(ExHooks.Events.Invasion.InvokeEventConfiguration);
            csr.Emit(OpCodes.Brtrue, target);
        }

        public void Deregister()
            => IL.Terraria.Main.StartInvasion -= ILHook_StartInvasion_EventConfiguration;
    }
}
