using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace DogshitSols.Features.ContactDamage.HarmonyPatches {

    [HarmonyPatch(typeof(MonsterBase))]
    internal static class MonsterBasePatches {

        internal static event Action<MonsterBase>? OnCheckInitPostfix;


        [HarmonyPatch("CheckInit")]
        [HarmonyPostfix]
        private static void CheckInit_Postfix(MonsterBase __instance) {
            try {
                OnCheckInitPostfix?.Invoke(__instance);
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }
    }
}
