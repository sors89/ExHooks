using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace ExHooks.Mods
{
    internal class NonVanillaEventUpdate : IHook
    {
        public void Register()
            => IL.Terraria.Main.UpdateInvasion += ILHook_UpdateInvasion_NonVanillaEventUpdate;

        private static void ILHook_UpdateInvasion_NonVanillaEventUpdate(ILContext ctx)
        {
            var csr = new ILCursor(ctx);
            csr.GotoNext(i => i.OpCode == OpCodes.Ret);

            csr.Emit(OpCodes.Br_S, csr.Body.Instructions[csr.Index]);
            csr.EmitDelegate(ExHooks.Events.Invasion.InvokeModdedEventUpdate);

            csr.Body.Instructions.Where(i => i.OpCode == OpCodes.Ble).First().Operand = csr.Previous;
        }

        public void Deregister()
            => IL.Terraria.Main.UpdateInvasion -= ILHook_UpdateInvasion_NonVanillaEventUpdate;
    }
}
