using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace ExHooks.Mods
{
    internal class CheckNighttimeEventRequirements : IHook
    {
        public void Register()
            => IL.Terraria.Main.UpdateTime_StartNight += ILHook_UpdateTime_StartNight_CheckNighttimeEventRequirements;

        private static void ILHook_UpdateTime_StartNight_CheckNighttimeEventRequirements(ILContext ctx)
        {
            var csr = new ILCursor(ctx);
            csr.GotoNext(MoveType.After,
                         i => i.OpCode == OpCodes.Ldarg_0,
                         i => i.OpCode == OpCodes.Ldind_U1,
                         i => i.OpCode == OpCodes.Brtrue);

            var targetLabel = csr.Previous.Operand;
            csr.EmitDelegate(ExHooks.Events.World.InvokeCheckNighttimeEventRequirements);
            csr.Emit(OpCodes.Brtrue, targetLabel);
        }

        public void Deregister()
            => IL.Terraria.Main.UpdateTime_StartDay -= ILHook_UpdateTime_StartNight_CheckNighttimeEventRequirements;
    }
}
