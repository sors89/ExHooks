using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace ExHooks.Mods
{
    internal class PostEventNpcKilled : IHook
    {
        public void Register()
            => IL.Terraria.NPC.checkDead += ILHook_checkDead_PostEventNpcKilled;

        private static void ILHook_checkDead_PostEventNpcKilled(ILContext ctx)
        {
            var csr = new ILCursor(ctx);
            while (csr.TryGotoNext(i => i.OpCode == OpCodes.Ret)) ;

            csr.Emit(OpCodes.Ldarg_0);
            csr.EmitDelegate(ExHooks.Events.Invasion.InvokePostEventNpcKilled);

            var retLabels = csr.Body.Instructions.Reverse()
                    .Where(x => x.Operand is ILLabel ilLabel && ilLabel.Target == csr.Next)
                    .Take(4).ToList();

            for (int i = 0; i < retLabels.Count; i++)
            {
                retLabels[i].Operand = csr.Previous.Previous;
            }
        }

        public void Deregister()
            => IL.Terraria.NPC.checkDead -= ILHook_checkDead_PostEventNpcKilled;
    }
}
