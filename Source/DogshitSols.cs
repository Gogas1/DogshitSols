using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using DogshitSols.Features.ContactDamage;
using DogshitSols.Features.DashTweaks;
using HarmonyLib;
using NineSolsAPI;
using NineSolsAPI.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DogshitSols;

[BepInDependency(NineSolsAPICore.PluginGUID)]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class DogshitSols : BaseUnityPlugin {

    private Harmony harmony = null!;
    internal ConfigurationManager ConfigManager = null!;
    internal static DogshitSols Instance { get; private set; } = null!;

    private void Awake() {
        Log.Init(Logger);
        RCGLifeCycle.DontDestroyForever(gameObject);
        hideFlags = HideFlags.HideAndDontSave;
        
        Instance = this;
        this.ConfigManager = new ConfigurationManager(Config);

        harmony = Harmony.CreateAndPatchAll(typeof(DogshitSols).Assembly);

        InitializeFeatures();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void InitializeFeatures() {
        ContactDamageFeature.Initialize();
        DashTweaksFeature.Initialize();
    }

    private void OnDestroy() {
        // Make sure to clean up resources here to support hot reloading
        DashTweaksFeature.Release();
        ContactDamageFeature.Release();
        harmony.UnpatchSelf();
    }
}