using DogshitSols.Features.ContactDamage.HarmonyPatches;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace DogshitSols.Features.ContactDamage {
    internal static class ContactDamageFeature {

        private static GameObject? _contactDamageReceiver;
        private static FieldInfo _damageDealerParriableOwnerField = AccessTools.Field(typeof(DamageDealer), "_parriableOwner");
        private static FieldInfo _triggerDetectorAllValidEffectTypeField = AccessTools.Field(typeof(TriggerDetector), "_allValidEffectType");

        internal static void Initialize() {
            MonsterBasePatches.OnCheckInitPostfix += HandleMonsterInit;
            PlayerPatches.OnAwakePostfix += HandlePlayerAwake;
        }

        private static void HandlePlayerAwake(Player obj) {
            var health = obj.health;

            var damageReceiver = health.transform.Find("DamageReceiver");
            _contactDamageReceiver = GameObject.Instantiate(damageReceiver.gameObject, health.transform);
            _contactDamageReceiver.transform.localPosition = Vector3.zero;
            _contactDamageReceiver.name = "ContactDamageReceiver";
            var contactDamageEffectReceiver = _contactDamageReceiver.GetComponent<EffectReceiver>();
            contactDamageEffectReceiver.effectType = EffectType.Universe2;

            var healthCollider = health.GetComponent<Collider2D>();
            EffectReceiver.EffectReceiverCache.RemoveAll(healthCollider);

            AutoAttributeManager.AutoReferenceAllChildren(health.gameObject);

            obj.fsm.Changed += HandlePlayerStateChanged;
        }

        private static void HandlePlayerStateChanged(PlayerStateType obj) {
            if(_contactDamageReceiver == null) {
                return;
            }

            switch(obj) {
                case PlayerStateType.FooAttack:
                    _contactDamageReceiver.SetActive(false);
                    break;
                case PlayerStateType.FooAttackCharge:
                    _contactDamageReceiver.SetActive(false);
                    break;
                case PlayerStateType.FooExplode:
                    _contactDamageReceiver.SetActive(false);
                    break;
                default:
                    _contactDamageReceiver.SetActive(true);
                    break;
            }
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
            //touchDamageDealer.parriable = false;
            touchDamageDealer.facingDetectionRule = FacingDetectionRule.IgnoreFacing;
            _damageDealerParriableOwnerField.SetValue(touchDamageDealer, null);

            var touchDamageEffectDealer = touchDamageTransform.GetComponentInChildren<EffectDealer>(true);
            var touchDamageTriggerDetector = touchDamageTransform.GetComponentInChildren<TriggerDetector>(true);
            touchDamageEffectDealer.type = EffectType.Universe2;
            _triggerDetectorAllValidEffectTypeField.SetValue(touchDamageTriggerDetector, EffectType.Universe2);
        }

        internal static void Release() {
            MonsterBasePatches.OnCheckInitPostfix -= HandleMonsterInit;
            PlayerPatches.OnAwakePostfix -= HandlePlayerAwake;

            if(Player.i != null) {
                Player.i.fsm.Changed -= HandlePlayerStateChanged;
            }
        }
    }
}
