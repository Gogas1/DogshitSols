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

        private void RememberDamageReceiver() {
            _correntDamageReceiver = Monster.damageReceivers.FirstOrDefault(dr => dr != null && dr.gameObject != null && dr.gameObject.activeInHierarchy)?.gameObject;
        }

        private void Update() {
            if(_correntDamageReceiver == null) {

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
