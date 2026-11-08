using DogshitSols.Features.ContactDamage.HarmonyPatches;
using System;
using System.Collections.Generic;
using System.Text;

namespace DogshitSols.Features.ContactDamage {
    internal static class ContactDamageFeature {
        
        
        internal static void Initialize() {
            MonsterBasePatches.OnCheckInitPostfix += HandleMonsterInit;
        }

        private static void HandleMonsterInit(MonsterBase obj) {
            var logicRootTransform = obj?.monsterCore?.logicRoot ?? null;

            if (logicRootTransform == null) return;

            var touchDamageTransform = logicRootTransform.transform.Find("TouchDamage");

            if(touchDamageTransform == null) {
                return;
            }

            touchDamageTransform.gameObject.SetActive(true);
            var touchDamageDealer = touchDamageTransform.GetComponentInChildren<DamageDealer>(true);

            if(touchDamageDealer == null) {                
                return;
            }

            touchDamageDealer.effectDealer.IsClearHitOwnerWhenExit = true;
        }
    }
}
