using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace ExHooks.Mods
{
    internal class CheckDaytimeEventRequirements : IHook
    {
        public void Register()
            => IL.Terraria.Main.UpdateTime_StartDay += ILHook_UpdateTime_StartDay_CheckDaytimeEventRequirements;

        private static void ILHook_UpdateTime_StartDay_CheckDaytimeEventRequirements(ILContext ctx)
        {
            var csr = new ILCursor(ctx);
            csr.GotoNext(MoveType.After,
                         i => i.OpCode == OpCodes.Ldarg_0,
                         i => i.OpCode == OpCodes.Ldind_U1,
                         i => i.OpCode == OpCodes.Brtrue);

            var targetLabel = csr.Previous.Operand;
            csr.EmitDelegate(ExHooks.Events.Invasion.InvokeCheckDaytimeEventRequirements);
            csr.Emit(OpCodes.Brtrue, targetLabel);
        }

        public void Deregister()
            => IL.Terraria.Main.UpdateTime_StartDay -= ILHook_UpdateTime_StartDay_CheckDaytimeEventRequirements;
    }
}
