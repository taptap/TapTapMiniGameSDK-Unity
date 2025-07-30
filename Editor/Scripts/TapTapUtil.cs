using System.IO;
using minihost.editor;
using UnityEditor;
using UnityEngine;

namespace TapTapMiniGame
{
    public class TapTapUtil
    {
        public static string tapTapBgImagePath = "Assets/TapTapMiniGame/Runtime/minigame-default/images";
        public static string tapTapBgImageSrc = Path.Combine(tapTapBgImagePath, "background.png");

        private static TJEditorScriptObject config = null;

        public static void Init(bool isBuildProfile = false)
        {
            bool needRefresh = false;
            string sdkRootPath = Path.Combine(Application.dataPath, "TapTapMiniGame");
            string editorPath = Path.Combine(sdkRootPath, "Editor");
            string pkgPath = Path.Combine("Packages", "com.taptap.minigame");
            string sourceImageSrc = Path.Combine(pkgPath, "Editor", "images", "background.png");

            CreateDirectoryIfNotExists(sdkRootPath);
            CreateDirectoryIfNotExists(editorPath);

            if (!isBuildProfile)
            {
                if (!File.Exists(Path.Combine(editorPath, "MiniGameConfig.asset")))
                {
                    File.Copy(
                        Path.Combine(pkgPath, "Editor", "MiniGameConfig.asset"),
                        Path.Combine(editorPath, "MiniGameConfig.asset"), true);
                    needRefresh = true;
                }

                TJEditorScriptObject config = UnityUtil.GetEditorConf("taptap", "Assets/TapTapMiniGame/Editor/MiniGameConfig.asset");

                if (config.ProjectConf.bgImageSrc ==
                    "Assets/TJ-WASM-SDK-V2/Runtime/minigame-default/images/background.jpg")
                {
                    config.ProjectConf.bgImageSrc = tapTapBgImageSrc;
                }
            }
            
            if (!File.Exists(tapTapBgImageSrc))
            {
                CreateDirectoryIfNotExists(tapTapBgImagePath);
                if (File.Exists(sourceImageSrc))
                {
                    File.Copy(sourceImageSrc, tapTapBgImageSrc, true);
                }

                needRefresh = true;
            }

            if (needRefresh)
            {
                AssetDatabase.Refresh();
            }
        }

        public static void CreateDirectoryIfNotExists(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static TJEditorScriptObject GetEditorConf(bool force = false)
        {
            Init();
            if (force || config == null)
            {
                config = UnityUtil.GetEditorConf("taptap", "Assets/TapTapMiniGame/Editor/MiniGameConfig.asset", false);
            }

            return config;
        }
    }
}