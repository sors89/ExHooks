using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace ExHooks.Mods
{
    internal class SpawnEventNPC : IHook
    {
        public void Register()
            => IL.Terraria.NPC.SpawnNPC += ILHook_SpawnNPC_SpawnEventNPC;
        
        private static void ILHook_SpawnNPC_SpawnEventNPC(ILContext ctx)
        {
            var csr = new ILCursor(ctx);
            csr.GotoNext(MoveType.After,
                         i => i.MatchLdloc(16),
                         i => i.OpCode == OpCodes.Brfalse);

            var targetLabel = csr.Previous.Operand;
            csr.Emit(OpCodes.Ldsfld, typeof(Terraria.Main).GetField("player"));
            csr.Emit(OpCodes.Ldloc, 13);
            csr.Emit(OpCodes.Ldelem_Ref);
            csr.Emit(OpCodes.Ldloc, 4);
            csr.Emit(OpCodes.Ldloc, 5);
            csr.EmitDelegate(ExHooks.Events.Invasion.InvokeSpawnEventNPC);
            csr.Emit(OpCodes.Brfalse, targetLabel);
        }

        public void Deregister()
            => IL.Terraria.NPC.SpawnNPC -= ILHook_SpawnNPC_SpawnEventNPC;
    }
}
