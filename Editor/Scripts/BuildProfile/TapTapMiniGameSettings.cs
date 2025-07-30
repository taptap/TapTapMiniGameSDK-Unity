#if TUANJIE_1_5_OR_NEWER
using UnityEditor.Build.Profile;
using UnityEngine;
using System.Collections.Generic;
using minihost.editor;

namespace TapTapMiniGame
{
    public class TapTapMiniGameSettings : MiniGameSettings
    {
        public TapProjectConf ProjectConf;
        public SDKOptions SDKOptions;
        public CompileOptions CompileOptions;
        public List<string> PlayerPrefsKeys = new List<string>();
        public FontOptions FontOptions;

        private bool AutoUploadOnBuild = false;
        
        public TapTapMiniGameSettings(TapTapMiniGameSettingsEditor editor) : base(editor)
        {
            TapTapUtil.Init(true);
        }
        
        public TJEditorScriptObject ToTJEditorScriptObject()
        {
            var scriptObject = ScriptableObject.CreateInstance<TJEditorScriptObject>();
    
            scriptObject.ProjectConf = this.ProjectConf;
            scriptObject.SDKOptions = this.SDKOptions;
            scriptObject.CompileOptions = this.CompileOptions;
            scriptObject.PlayerPrefsKeys = new List<string>(this.PlayerPrefsKeys);
            scriptObject.FontOptions = this.FontOptions;
            scriptObject.AutoUploadOnBuild = this.AutoUploadOnBuild;

            return scriptObject;
        }

        public bool PreprocessBuild(BuildProfile buildProfile)
        {
            if (TJConvertCore.IsInstantGameAutoStreaming())
            {
                ProjectConf.CDN = UnityUtil.GetInstantGameAutoStreamingCDN();
            }
            bool result = true;
            if (!string.IsNullOrEmpty(buildProfile.buildPath))
            {
                this.ProjectConf.DST = buildProfile.buildPath;
            }
            else
            {
                Debug.LogError("Build Path is empty!");
                result = false;
            }

            return result;
        }
    }
}
#endif