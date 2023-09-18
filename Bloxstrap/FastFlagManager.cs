using System.Windows.Forms;

using Windows.Win32;
using Windows.Win32.Graphics.Gdi;

namespace Bloxstrap
{
    public class FastFlagManager : JsonManager<Dictionary<string, object>>
    {
        public override string FileLocation => Path.Combine(Paths.Modifications, "ClientSettings\\ClientAppSettings.json");

        public static IReadOnlyDictionary<string, string> PresetFlags = new Dictionary<string, string>
        {
            { "Network.Log", "FLogNetwork" },
            
            { "HTTP.Log", "DFLogHttpTraceLight" },

            { "HTTP.Proxy.Enable", "DFFlagDebugEnableHttpProxy" },
            { "HTTP.Proxy.Address.1", "DFStringDebugPlayerHttpProxyUrl" },
            { "HTTP.Proxy.Address.2", "DFStringHttpCurlProxyHostAndPort" },
            { "HTTP.Proxy.Address.3", "DFStringHttpCurlProxyHostAndPortForExternalUrl" },

            { "Rendering.Framerate", "DFIntTaskSchedulerTargetFps" },
            { "Rendering.ManualFullscreen", "FFlagHandleAltEnterFullscreenManually" },

            { "Rendering.Textures.New", "FStringPartTexturePackTable2022" },
            { "Rendering.Textures.Old", "FStringPartTexturePackTablePre2022" },
            { "Rendering.Textures.Quality.Enabled", "DFFlagTextureQualityOverrideEnabled" },
            { "Rendering.Textures.Quality.Value", "DFIntTextureQualityOverride"},

            { "Rendering.MSAA", "FIntDebugForceMSAASamples"},

            { "Rendering.DisableScaling", "DFFlagDisableDPIScale" },

            { "Rendering.Mode.D3D11", "FFlagDebugGraphicsPreferD3D11" },
            { "Rendering.Mode.D3D10", "FFlagDebugGraphicsPreferD3D11FL10" },
            { "Rendering.Mode.Vulkan", "FFlagDebugGraphicsPreferVulkan" },
            { "Rendering.Mode.Vulkan.Fix", "FFlagRenderVulkanFixMinimizeWindow" },
            { "Rendering.Mode.OpenGL", "FFlagDebugGraphicsPreferOpenGL" },

            { "Rendering.Lighting.Voxel", "DFFlagDebugRenderForceTechnologyVoxel" },
            { "Rendering.Lighting.ShadowMap", "FFlagDebugForceFutureIsBrightPhase2" },
            { "Rendering.Lighting.Future", "FFlagDebugForceFutureIsBrightPhase3" },

            { "UI.Hide", "DFIntCanHideGuiGroupId" },
            { "UI.FlagState", "FStringDebugShowFlagState" },

            { "UI.Menu.GraphicsSlider", "FFlagFixGraphicsQuality" },
            
            { "UI.Menu.Style.DisableV2", "FFlagDisableNewIGMinDUA" },
            { "UI.Menu.Style.EnableV4", "FFlagEnableInGameMenuControls" },

            { "UI.Menu.Style.ABTest.1", "FFlagEnableMenuControlsABTest" },
            { "UI.Menu.Style.ABTest.2", "FFlagEnableMenuModernizationABTest" },
            { "UI.Menu.Style.ABTest.3", "FFlagEnableMenuModernizationABTest2" },
            { "UI.Menu.Style.ABTest.4", "FFlagEnableV3MenuABTest3" },
            { "UI.Menu.Accessibility.EnableAccessibility", "FFlagEnableAccessibilitySettingsAPIV2" },
            { "UI.Menu.Accessibility.EnableEffectsInCoreScripts", "FFlagEnableAccessibilitySettingsEffectsInCoreScripts" },
            { "UI.Menu.Accessibility.EnableEffectsInChat", "FFlagEnableAccessibilitySettingsEffectsInExperienceChat" },
            { "UI.Menu.Accessibility.EnableSettings", "FFlagEnableAccessibilitySettingsInExperienceMenu2" }
        };

        // only one missing here is Metal because lol
        public static IReadOnlyDictionary<string, string> RenderingModes => new Dictionary<string, string>
        {
            { "Automatic", "None" },
            { "Vulkan", "Vulkan" },
            { "Direct3D 11", "D3D11" },
            { "Direct3D 10", "D3D10" },
            { "OpenGL", "OpenGL" }
        };

        public static IReadOnlyDictionary<string, string> TextureQuality => new Dictionary<string, string>
        {
            { "Automatic", "None" },
            { "Very Low", "0" },
            { "Low", "1" },
            { "Medium", "2"},
            { "High", "3" }
        };

        public static IReadOnlyDictionary<string, string> LightingModes => new Dictionary<string, string>
        {
            { "Chosen by game", "None" },
            { "Voxel (Phase 1)", "Voxel" },
            { "ShadowMap (Phase 2)", "ShadowMap" },
            { "Future (Phase 3)", "Future" }
        };

        public static IReadOnlyDictionary<string, string?> MSAAModes => new Dictionary<string, string?>
        {
            // dont set to higher than 8 or freddy fazebear will kill you
            { "Automatic", null },
            { "1x MSAA", "1" },
            { "2x MSAA", "2" },
            { "4x MSAA", "4" }
        };

        // this is one hell of a dictionary definition lmao
        // since these all set the same flags, wouldn't making this use bitwise operators be better?
        public static IReadOnlyDictionary<string, Dictionary<string, string?>> IGMenuVersions => new Dictionary<string, Dictionary<string, string?>>
        {
            {
                "Default",
                new Dictionary<string, string?>
                {
                    { "DisableV2", null },
                    { "EnableV4", null },
                    { "ABTest", null }
                }
            },

            {
                "Version 1 (2015)",
                new Dictionary<string, string?>
                {
                    { "DisableV2", "True" },
                    { "EnableV4", "False" },
                    { "ABTest", "False" }
                }
            },

            {
                "Version 2 (2020)",
                new Dictionary<string, string?>
                {
                    { "DisableV2", "False" },
                    { "EnableV4", "False" },
                    { "ABTest", "False" }
                }
            },

            {
                "Version 4 (2023)",
                new Dictionary<string, string?>
                {
                    { "DisableV2", "True" },
                    { "EnableV4", "True" },
                    { "ABTest", "False" }
                }
            }
        };

        // all fflags are stored as strings
        // to delete a flag, set the value as null
        public void SetValue(string key, object? value)
        {
            const string LOG_IDENT = "FastFlagManager::SetValue";

            if (value is null)
            {
                if (Prop.ContainsKey(key))
                    App.Logger.WriteLine(LOG_IDENT, $"Deletion of '{key}' is pending");

                Prop.Remove(key);
            }
            else
            {
                if (Prop.ContainsKey(key))
                {
                    if (key == Prop[key].ToString())
                        return;

                    App.Logger.WriteLine(LOG_IDENT, $"Changing of '{key}' from '{Prop[key]}' to '{value}' is pending");
                }
                else
                {
                    App.Logger.WriteLine(LOG_IDENT, $"Setting of '{key}' to '{value}' is pending");
                }

                Prop[key] = value.ToString()!;
            }
        }

        // this returns null if the fflag doesn't exist
        public string? GetValue(string key)
        {
            // check if we have an updated change for it pushed first
            if (Prop.TryGetValue(key, out object? value) && value is not null)
                return value.ToString();
            
            return null;
        }

        public void SetPreset(string prefix, object? value)
        {
            foreach (var pair in PresetFlags.Where(x => x.Key.StartsWith(prefix)))
                SetValue(pair.Value, value);
        }

        public void SetPresetEnum(string prefix, string target, object? value)
        {
            foreach (var pair in PresetFlags.Where(x => x.Key.StartsWith(prefix)))
            {
                if (pair.Key.StartsWith($"{prefix}.{target}"))
                    SetValue(pair.Value, value);
                else
                    SetValue(pair.Value, null);
            }
        }

        public string? GetPreset(string name) => GetValue(PresetFlags[name]);

        public string GetPresetEnum(IReadOnlyDictionary<string, string> mapping, string prefix, string value)
        {
            foreach (var pair in mapping)
            {
                if (pair.Value == "None")
                    continue;

                if (GetPreset($"{prefix}.{pair.Value}") == value)
                    return pair.Key;
            }

            return mapping.First().Key;
        }

        public void CheckManualFullscreenPreset()
        {
            if (GetPreset("Rendering.Mode.Vulkan") == "True" || GetPreset("Rendering.Mode.OpenGL") == "True")
                SetPreset("Rendering.ManualFullscreen", null);
            else
                SetPreset("Rendering.ManualFullscreen", "False");
        }

        public override void Save()
        {
            // convert all flag values to strings before saving

            foreach (var pair in Prop)
                Prop[pair.Key] = pair.Value.ToString()!;

            base.Save();
        }

        public override void Load()
        {
            base.Load();

            CheckManualFullscreenPreset();

            // TODO - remove when activity tracking has been revamped
            if (GetPreset("Network.Log") != "7")
                SetPreset("Network.Log", "7");


            if (GetPreset("Rendering.Framerate") is not null)
                return;

            // set it to be the framerate of the primary display by default

            var screen = Screen.AllScreens.Where(x => x.Primary).Single();
            var devmode = new DEVMODEW();

            PInvoke.EnumDisplaySettings(screen.DeviceName, ENUM_DISPLAY_SETTINGS_MODE.ENUM_CURRENT_SETTINGS, ref devmode);

            uint framerate = devmode.dmDisplayFrequency;

            if (framerate <= 100)
                framerate *= 2;

            SetPreset("Rendering.Framerate", framerate);
        }
    }
}
