using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace ExHooks.Mods
{
    internal class EventFlagCheck : IHook
    {
        public void Register()
            => IL.Terraria.NPC.SpawnNPC += ILHook_SpawnNPC_EventFlagCheck;

        private static void ILHook_SpawnNPC_EventFlagCheck(ILContext ctx)
        {
            var csr = new ILCursor(ctx);
            csr.GotoNext(MoveType.After,
                         i => i.MatchCall(typeof(Terraria.Main), "get_IsItAHappyWindyDay"),
                         i => i.MatchStloc(25));

            csr.Emit(OpCodes.Ldsfld, typeof(Terraria.Main).GetField("player"));
            csr.Emit(OpCodes.Ldloc, 13);
            csr.Emit(OpCodes.Ldelem_Ref);
            csr.Emit(OpCodes.Ldloca, 16);
            csr.EmitDelegate(ExHooks.Events.Invasion.InvokeEventFlagCheck);
            csr.Emit(OpCodes.Brtrue, csr.Body.Instructions[csr.Index + 4].Operand);
        }

        public void Deregister()
            => IL.Terraria.NPC.SpawnNPC -= ILHook_SpawnNPC_EventFlagCheck;
    }
}
