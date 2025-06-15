using Terraria.Localization;
using ModFramework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;

namespace ExHooks.Mods
{
    internal class NonVanillaEventAnnouncement : IHook
    {
        public void Register()
            => IL.Terraria.Main.InvasionWarning += ILHook_InvasionWarning_NonVanillaEventAnnouncement;

        private static void ILHook_InvasionWarning_NonVanillaEventAnnouncement(ILContext ctx)
        {
            var csr = new ILCursor(ctx);
            csr.GotoNext(i => i.MatchLdsfld(typeof(Terraria.Main), "invasionSize"));

            //Adds some variables that allows plugin to change the announcement message color.
            //Does not work for vanilla color tho, there's a different and easier way to do that.
            var intRef = csr.Module.ImportReference(typeof(int));
            var r = new VariableDefinition(intRef);
            var g = new VariableDefinition(intRef);
            var b = new VariableDefinition(intRef);
            csr.Body.Variables.AddRange(new List<VariableDefinition> { r, g, b });
            csr.Emit(OpCodes.Ldc_I4, 175);
            csr.Emit(OpCodes.Stloc, r);
            csr.Emit(OpCodes.Ldc_I4, 75);
            csr.Emit(OpCodes.Stloc, g);
            csr.Emit(OpCodes.Ldc_I4, 255);
            csr.Emit(OpCodes.Stloc, b);

            //Adds a check if the current vanilla invasion is not 0 because ReLogic forgot to
            csr.Emit(OpCodes.Ldsfld, typeof(Terraria.Main).GetField("invasionType"));
            csr.Emit(OpCodes.Ldc_I4_0);
            csr.Emit(OpCodes.Nop);
   
            csr.GotoNext(i => i.MatchLdsfld(typeof(Terraria.Main), "netMode"));

            var netModeLabel = csr.Body.Instructions[csr.Index];
            csr.Emit(OpCodes.Ldloc_0);
            csr.Emit(OpCodes.Callvirt, typeof(LocalizedText).GetProperty("Value")!.GetMethod);
            csr.Emit(OpCodes.Ldstr, "");
            csr.Emit(OpCodes.Call, typeof(string).GetMethod("op_Equality"));
            csr.Emit(OpCodes.Nop);
            csr.Emit(OpCodes.Ldsfld, typeof(Terraria.Main).GetField("invasionType"));
            csr.Emit(OpCodes.Ldc_I4_4);
            csr.Emit(OpCodes.Nop);
            csr.Emit(OpCodes.Ldloca, 0);
            csr.Emit(OpCodes.Ldloca, r);
            csr.Emit(OpCodes.Ldloca, g);
            csr.Emit(OpCodes.Ldloca, b);
            csr.EmitDelegate(ExHooks.Events.Invasion.InvokeModdedEventAnnouncement);

            var wrongTargetLabels = csr.Body.Instructions
                            .Where(x => x.Operand is ILLabel ilLabel && ilLabel.Target == netModeLabel)
                            .ToList();

            for (int i = 0; i < wrongTargetLabels.Count; i++)
            {
                wrongTargetLabels[i].Operand = csr.Previous.Previous(12);
            }

            csr.Previous.Previous(5).OpCode = OpCodes.Beq_S;
            csr.Previous.Previous(5).Operand = netModeLabel;
            csr.Previous.Previous(8).OpCode = OpCodes.Brfalse_S;
            csr.Previous.Previous(8).Operand = netModeLabel;

            var fixReLogic = csr.Body.Instructions.Where(x => x.OpCode == OpCodes.Nop).First();
            fixReLogic.OpCode = OpCodes.Ble_S;
            fixReLogic.Operand = csr.Previous.Previous(12);

            while (csr.TryGotoNext(i => i.MatchLdcI4(175))) ;
            csr.RemoveRange(3);
            csr.Emit(OpCodes.Ldloc, r);
            csr.Emit(OpCodes.Ldloc, g);
            csr.Emit(OpCodes.Ldloc, b);
        }

        public void Deregister()
            => IL.Terraria.Main.InvasionWarning -= ILHook_InvasionWarning_NonVanillaEventAnnouncement;
    }
}
