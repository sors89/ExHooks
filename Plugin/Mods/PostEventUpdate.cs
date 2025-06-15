using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace ExHooks.Mods
{
    internal class PostEventUpdate : IHook
    {
        public void Register()
            => IL.Terraria.Main.UpdateInvasion += ILHook_UpdateInvasion_PostEventUpdate;

        private static void ILHook_UpdateInvasion_PostEventUpdate(ILContext ctx)
        {
            var csr = new ILCursor(ctx);
            csr.GotoNext(i => i.OpCode == OpCodes.Ret);

            csr.EmitDelegate(ExHooks.Events.Invasion.InvokePostEventUpdate);

            var retLabels = csr.Body.Instructions
                .Where(i => i.Operand is ILLabel ilLabel && ilLabel.Target == csr.Body.Instructions.Last())
                .ToList();

            for (int i = 0; i < retLabels.Count; i++)
            {
                retLabels[i].Operand = csr.Previous;
            }
        }

        public void Deregister()
            => IL.Terraria.Main.UpdateInvasion -= ILHook_UpdateInvasion_PostEventUpdate;
    }
}
