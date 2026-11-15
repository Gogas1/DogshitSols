using NineSolsAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DogshitSols.Features.ContactDamage {
    internal class EnemyInvincibilityWatcher : MonoBehaviour {
        internal GameObject TouchDamageGameObject = null!;
        internal MonsterBase Monster = null!;
        private GameObject? _correntDamageReceiver = null;        

        private HashSet<string> LadyENames = new HashSet<string>() {
            "StealthGameMonster_Boss_ButterFly Variant",
            "StealthGameMonster_Boss_ButterFly Variant (1)",
            "StealthGameMonster_Boss_ButterFly Variant (2)",
            "StealthGameMonster_Boss_ButterFly Variant (3)",
            "StealthGameMonster_Boss_ButterFly Variant (4)",
        };

        private void RememberDamageReceiver() {
            if(Monster == null) {
                return;
            }

            if(LadyENames.Contains(Monster.name)) {
                var canHurtObject = Monster.ChildAnimator.gameObject.transform.Find("CanHurt");
                _correntDamageReceiver = canHurtObject != null ? canHurtObject.gameObject : null;
                return;
            }

            _correntDamageReceiver = Monster.damageReceivers.FirstOrDefault(dr => dr != null && dr.gameObject != null && dr.gameObject.activeInHierarchy)?.gameObject;
        }

        private void Update() {
            if(_correntDamageReceiver == null) {

                if(TouchDamageGameObject.activeInHierarchy) {
                    TouchDamageGameObject.SetActive(false);
                }

                RememberDamageReceiver();
                return;
            }

            if (_correntDamageReceiver.activeInHierarchy && !TouchDamageGameObject.activeInHierarchy) {
                TouchDamageGameObject.SetActive(true);
            }

            if(!_correntDamageReceiver.activeInHierarchy && TouchDamageGameObject.activeInHierarchy) {
                TouchDamageGameObject.SetActive(false);
            }
        }
    }
}
