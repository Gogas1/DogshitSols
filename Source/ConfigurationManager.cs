using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DogshitSols {
    internal class ConfigurationManager {
        internal event Action<bool>? OnLadyEClonesDamageEnabledChanged;
        internal ConfigEntry<bool> LadyEClonesDamageEnabled = null!;
        internal ConfigEntry<bool> DisableDashIFrames = null!;

        public ConfigurationManager(ConfigFile config) {
            LadyEClonesDamageEnabled = config.Bind("General", "Lady E clones contact damage", true, "Whether Lady E clones have contact damage.");
            LadyEClonesDamageEnabled.SettingChanged += (sender, args) => OnLadyEClonesDamageEnabledChanged?.Invoke(LadyEClonesDamageEnabled.Value);

            DisableDashIFrames = config.Bind("General", "Disable dash i-frames", true, "Whether to disable dash i-frames.");
        }
    }
}
