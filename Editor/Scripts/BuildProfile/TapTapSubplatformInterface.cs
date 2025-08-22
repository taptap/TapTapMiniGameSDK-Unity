#if TUANJIE_1_5_OR_NEWER
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Profile;
using UnityEngine;
using UnityEngine.Rendering;
using minihost.editor;

namespace TapTapMiniGame
{
    [InitializeOnLoad]
    public static class RegisterTapTapMiniGameSubplatformInterface
    {
        static RegisterTapTapMiniGameSubplatformInterface()
        {
            MiniGameSubplatformManager.RegisterSubplatform(new TapTapMiniGameSubplatformInterface());
        }
    }
    public class TapTapMiniGameSubplatformInterface : MiniGameSubplatformInterface
    {
        public override string GetSubplatformName()
        {
            return "TapTap:TapTap";
        }
        
        public override MiniGameSettings GetSubplatformSettings()
        {
            return new TapTapMiniGameSettings(new TapTapMiniGameSettingsEditor());
        }
        
        public override string GetSubplatformLink()
        {
            return "https://developer.taptap.cn/minigameapidoc/dev/engine/unity-adaptation/unity-webGL/";
        }

        public override string GetSubplatformTooltip()
        {
            return "点击查看TapTap小游戏文档";
        }

        private static bool MiniGameHostBuildPreprocess(BuildProfile buildProfile)
        {
            // Check GFX API and Color Space
            if (buildProfile != null)
            {
                PlayerSettings playerSettings = buildProfile.playerSettings;
                TapTapMiniGameSettings settings = buildProfile.miniGameSettings as TapTapMiniGameSettings;

                // Global PlayerSettings
                ColorSpace colorSpace = PlayerSettings.colorSpace;
                GraphicsDeviceType[] apis = PlayerSettings.GetGraphicsAPIs(buildProfile.buildTarget);
                bool isAutomatic = PlayerSettings.GetUseDefaultGraphicsAPIs(buildProfile.buildTarget);

                if (playerSettings != null)
                {
                    // BuildProfile PlayerSettings Override
                    colorSpace = PlayerSettings.GetColorSpace_Internal(playerSettings);
                    apis = PlayerSettings.GetGraphicsAPIs_Internal(playerSettings, buildProfile.buildTarget);
                    isAutomatic = PlayerSettings.GetUseDefaultGraphicsAPIs_Internal(playerSettings, buildProfile.buildTarget);
                }

                // Dont allow automatic
                if (isAutomatic && colorSpace == ColorSpace.Linear && settings != null)
                {
                    settings.CompileOptions.Webgl2 = true;
                }

                if (apis.Length == 1 && settings != null)
                {
                    bool isWebGL1 = apis.Contains(GraphicsDeviceType.OpenGLES2);
                    if (isWebGL1 && colorSpace == ColorSpace.Linear)
                    {
                        Debug.LogError("WebGL1 does not support Linear color space. Please switch to Gamma color space.");
                        return false;
                    }

                    settings.CompileOptions.Webgl2 = !isWebGL1;
                }
                else
                {
                    Debug.LogError("Please choose between WebGL1 and WebGL2");
                    return false;
                }

                return true;
            }
            else
            {
                throw new InvalidOperationException("Build profile has not been initialized.");
            }
        }
        

        public override BuildMiniGameError Build(BuildProfile profile)
        {
            return BuildMiniGameError.Unknown;
        }

        public override BuildMiniGameError Build(BuildProfile profile, BuildOptions options)
        {
            bool preprocessSuccess = MiniGameHostBuildPreprocess(profile);
            if (!preprocessSuccess)
            {
                return BuildMiniGameError.PlayerBuildFailed;
            }

            TapTapMiniGameSettings settings = profile.miniGameSettings as TapTapMiniGameSettings;
            if (settings is not null && settings.PreprocessBuild(profile))
            {
                // Check is support wasm split
                ScriptingImplementation backend = PlayerSettings.GetScriptingBackend(BuildPipeline.GetBuildTargetGroup(profile.buildTarget));
                bool isSupportWasmSplit = backend == ScriptingImplementation.IL2CPP;
                if (profile.playerSettings != null)
                {
                    isSupportWasmSplit = PlayerSettings.GetScriptingBackend_Internal(profile.playerSettings, BuildPipeline.GetBuildTargetGroup(profile.buildTarget)) == ScriptingImplementation.IL2CPP;
                }
                
                TJEditorScriptObject config = settings.ToTJEditorScriptObject();
                config.buildOptions = options;
                TapTapConvertCore.DoExport(config, isSupportWasmSplit);
            }
            else
            {
                Debug.Log("tapTapMiniGameSettingsEditor is null");
            }
            return BuildMiniGameError.Succeeded;
        }
    }
}
#endif