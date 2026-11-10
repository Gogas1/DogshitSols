using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace DogshitSols.Features.ContactDamage.HarmonyPatches {
    [HarmonyPatch(typeof(Player))]    
    
    internal static class PlayerPatches {

        internal static event Action<Player>? OnAwakePostfix;

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void Awake_Postfix(Player __instance) {
            try {
                OnAwakePostfix?.Invoke(__instance);
            } catch(Exception ex) {
                Log.Exception(ex);
            }
        }
    }
}
