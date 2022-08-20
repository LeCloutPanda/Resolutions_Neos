using BaseX;
using FrooxEngine;
using HarmonyLib;
using NeosModLoader;

namespace Resolutions
{ 
    public class Patch : NeosMod
    {
        public override string Name => "Resolutions";
        public override string Author => "LeCloutPanda";
        public override string Version => "1.0.1";

        public static ModConfiguration config;
        static PhotoCaptureManager photoCaptureManager;

        public override void OnEngineInit()
        {
            config = GetConfiguration();
            config.Save(true);

            Harmony harmony = new Harmony($"dev.{Author}.{Name}");
            harmony.PatchAll();

            config.OnThisConfigurationChanged += UpdateCrap;
        }


        private void UpdateCrap(ConfigurationChangedEvent @event)
        {
            photoCaptureManager.NormalResolution.Value = config.GetValue(RESOLUTION);
            photoCaptureManager.TimerResolution.Value = config.GetValue(TiMER_RESOLUTION);
        }
        
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<int2> RESOLUTION = new ModConfigurationKey<int2>("Finger Photo Resolution", "", () => new int2(1920, 1080));
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<int2> TiMER_RESOLUTION = new ModConfigurationKey<int2>("Finger Photo Timer Resolution", "", () => new int2(2560, 1440));
        
        [HarmonyPatch(typeof(PhotoCaptureManager))]
        static class HeheMakeCustomResPhotos
        {
            [HarmonyPatch("OnAwake")]
            [HarmonyPrefix]
            static void ChangeResolution(PhotoCaptureManager __instance)
            {
                if (__instance.Slot.ActiveUserRoot.ActiveUser != __instance.LocalUser) return;

                __instance.RunInUpdates(3, () =>
                {
                    photoCaptureManager = __instance;
                    photoCaptureManager.NormalResolution.Value = config.GetValue(RESOLUTION);
                    photoCaptureManager.TimerResolution.Value = config.GetValue(TiMER_RESOLUTION);
                });
            }
        }
    }
}
