using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HarmonyPatch("OnStateEnter")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> OnStateEnter_Transpiler(IEnumerable<CodeInstruction> instructions) {
            bool found = false;

            var instructionsList = instructions.ToList();

            for (int i = 0; i < instructionsList.Count; i++) {
                var instruction = instructionsList[i];
                var targetAheadInstruction = instructionsList.ElementAtOrDefault(i + 4);


                if(!found && targetAheadInstruction != null && targetAheadInstruction.Calls(AccessTools.Method(typeof(Health), "SetReceiversActivate"))) {
                    i = i + 4;
                    found = true;
                    continue;
                }

                yield return instruction;
            }
        }
    }
}
