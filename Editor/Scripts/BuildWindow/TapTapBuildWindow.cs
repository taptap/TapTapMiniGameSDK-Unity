using minihost.editor;
using UnityEditor;
using UnityEngine;

namespace TapTapMiniGame
{
    public class TapTapBuildWindow : EditorWindow
    {
        [MenuItem("TapTap小游戏/构建")]
        static void ShowWindow()
        {
            var win = GetWindow(typeof(TapTapBuildWindow), false, "构建TapTap小游戏");
            win.minSize = new Vector2(750, 400);
            win.position = new Rect(100, 100, 750, 400);
            win.Show();
        }

        private void OnGUI()
        {
            TapTapBuildWindowHelper.OnSettingsGUI(true);
            OnBuildButtonGUI();
        }

        public void OnDisable()
        {
            TapTapBuildWindowHelper.OnDisable();
        }

        private void OnBuildButtonGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var toBuild = GUILayout.Button(new GUIContent("生成TapTap小游戏"), GUILayout.Width(140), GUILayout.Height(25));
            GUILayout.Space(20);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            if (toBuild)
            {
                ScriptingImplementation backend = PlayerSettings.GetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup);
                bool isSupportWasmSplit = backend == ScriptingImplementation.IL2CPP;

                TJEditorScriptObject config = TapTapUtil.GetEditorConf();
                config.buildOptions = UnityUtil.GetBuildOptions(config);
                TapTapBuildWindowHelper.UpdateWebGL2();
                TapTapConvertCore.DoExport(config, isSupportWasmSplit);
                GUIUtility.ExitGUI();
            }
        }
    }
}