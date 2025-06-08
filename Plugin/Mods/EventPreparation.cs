using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace ExHooks.Mods
{
    internal class EventPreparation : IHook
    {
        public void Register()
            => IL.Terraria.Main.StartInvasion += ILHook_StartInvasion_EventPreparation;

        private static void ILHook_StartInvasion_EventPreparation(ILContext ctx)
        {
            var csr = new ILCursor(ctx);
            csr.GotoNext(MoveType.After, i => i.MatchStsfld(typeof(Terraria.Main), "invasionType"));

            csr.EmitDelegate(ExHooks.Events.Invasion.InvokeEventPreparation);

            csr.Body.Instructions.Where(x => x.OpCode == OpCodes.Brfalse_S).First().Operand = csr.Previous;
            csr.Body.Instructions.Where(x => x.OpCode == OpCodes.Brtrue_S).First().Operand = csr.Previous;
        }

        public void Deregister()
            => IL.Terraria.Main.StartInvasion -= ILHook_StartInvasion_EventPreparation;
    }
}
