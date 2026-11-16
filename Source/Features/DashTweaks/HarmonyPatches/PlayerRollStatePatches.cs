using BepInEx.Configuration;
using HarmonyLib;
using MonoMod.Utils.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace DogshitSols.Features.DashTweaks.HarmonyPatches {

    [HarmonyPatch(typeof(PlayerRollState))]
    internal static class PlayerRollStatePatches {

        internal static event Action<PlayerRollState>? OnStateEnterPostfix;
        internal static event Action<PlayerRollState>? OnStateExitPostfix;

        [HarmonyPatch(nameof(PlayerRollState.OnStateEnter))]
        [HarmonyPostfix]
        private static void OnStateEnter_Postfix(PlayerRollState __instance) {
            try {
                OnStateEnterPostfix?.Invoke(__instance);
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }


        [HarmonyPatch(nameof(PlayerRollState.OnStateExit))]
        [HarmonyPostfix]
        private static void OnStateExit_Postfix(PlayerRollState __instance) {
            try {
                OnStateExitPostfix?.Invoke(__instance);
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        private static MethodInfo _dogshitSolsGetInstanceMethod = typeof(DogshitSols).GetProperty("Instance", BindingFlags.NonPublic | BindingFlags.Static)!.GetMethod!;
        private static FieldInfo _dogshitSolsConfigManagerField = typeof(DogshitSols).GetField("ConfigManager", BindingFlags.NonPublic | BindingFlags.Instance)!;
        private static FieldInfo _configurationManagerDisableDashIFramesField = typeof(ConfigurationManager).GetField("DisableDashIFrames", BindingFlags.NonPublic | BindingFlags.Instance)!;
        private static MethodInfo _configEntryGetValueMethod = typeof(ConfigEntry<bool>).GetProperty("Value", BindingFlags.Public | BindingFlags.Instance)!.GetMethod!;

        private static MethodInfo _playerIsWeakGetter = typeof(Player).GetProperty("IsWeak", BindingFlags.Public | BindingFlags.Instance)!.GetMethod!;

        [HarmonyPatch("OnStateEnter")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> OnStateEnter_Transpiler(IEnumerable<CodeInstruction> instructions) {
            var m = new CodeMatcher(instructions)
                .MatchForward(
                    false,
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldfld),
                    new CodeMatch(OpCodes.Callvirt, _playerIsWeakGetter),
                    new CodeMatch(ci => ci.Branches(out _)))
                .ThrowIfInvalid("first brtrue not found")
                .Advance(3); 

                    var target = (Label)m.Operand;

                    m.Advance(1)
                     .Insert(
                        new CodeInstruction(OpCodes.Call, _dogshitSolsGetInstanceMethod),
                        new CodeInstruction(OpCodes.Ldfld, _dogshitSolsConfigManagerField),
                        new CodeInstruction(OpCodes.Ldfld, _configurationManagerDisableDashIFramesField),
                        new CodeInstruction(OpCodes.Callvirt, _configEntryGetValueMethod),
                        new CodeInstruction(OpCodes.Brtrue, target));

                    return m.InstructionEnumeration();
        
        }
    }
}
