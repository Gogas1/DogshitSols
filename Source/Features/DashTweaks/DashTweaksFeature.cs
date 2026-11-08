using DogshitSols.Features.DashTweaks.HarmonyPatches;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DogshitSols.Features.DashTweaks {
    internal static class DashTweaksFeature {

        private static GameObject? _playerTrapsDamageReceiver = null;

        private static GameObject? PlayerTrapsDamageReceiver {
            get {
                if (Player.i == null) {
                    _playerTrapsDamageReceiver = null;
                    return _playerTrapsDamageReceiver;
                }

                if (_playerTrapsDamageReceiver == null) {
                    var playerHealth = Player.i.GetHealth;
                    var trapsDamageReceiverTransform = playerHealth.transform.Find("TrapDamageReceiver");

                    if (trapsDamageReceiverTransform == null) {
                        PlayerTrapsDamageReceiver = null;
                        return _playerTrapsDamageReceiver;
                    }

                    _playerTrapsDamageReceiver = trapsDamageReceiverTransform.gameObject;
                    return _playerTrapsDamageReceiver;
                }

                return _playerTrapsDamageReceiver;
            }

            set {
                _playerTrapsDamageReceiver = value;
            }
        }

        internal static void Initialize() {
            PlayerRollStatePatches.OnStateEnterPostfix += HandleRollStateEnter;
            PlayerRollStatePatches.OnStateExitPostfix += HandleRollStateExit;
        }

        private static void HandleRollStateExit(PlayerRollState obj) {
            if(SceneManager.GetActiveScene().name != "A4_S5_DaoTrapHouse_Final") {
                PlayerTrapsDamageReceiver?.SetActive(true);
            }
        }

        private static void HandleRollStateEnter(PlayerRollState obj) {
            if (SceneManager.GetActiveScene().name != "A4_S5_DaoTrapHouse_Final") {
                PlayerTrapsDamageReceiver?.SetActive(false);
            }
        }
    }
}
