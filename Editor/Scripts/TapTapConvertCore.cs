using System.IO;
using System.Text;
using LitJson;
using JsonWriter = LitJson.JsonWriter;
using minihost.editor;
using UnityEditor;

namespace TapTapMiniGame
{
    public class TapTapConvertCore
    {
        private static TJEditorScriptObject config;
        private static bool isSupportWasmSplit = false;

        public static void DoExport(TJEditorScriptObject config, bool isSupportWasmSplit)
        {
            TapTapUtil.Init();
            
            if (string.IsNullOrEmpty(config.ProjectConf.bgImageSrc))
            {
                config.ProjectConf.bgImageSrc = TapTapUtil.tapTapBgImageSrc;
            }
            
            config.ProjectConf.isCoverviewCustomized = config.ProjectConf.bgImageSrc != TapTapUtil.tapTapBgImageSrc;

            TapTapConvertCore.config = config;
            TapTapConvertCore.isSupportWasmSplit = isSupportWasmSplit;
            var selection = Selection.objects;
            TJConvertCore.RegisterPostProcessHandler(PostProcess);
            TJConvertCore.DoExport(config);
            TJConvertCore.UnregisterPostProcessHandler(PostProcess);
            Selection.objects = selection;
        }

        public static void PostProcess()
        {
            var filePath = Path.Combine(config.ProjectConf.DST, TJConvertCore.miniGameDir, "game.json");

            string content = File.ReadAllText(filePath, Encoding.UTF8);
            JsonData gameJson = JsonMapper.ToObject(content);

            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(TapTapConvertCore).Assembly);
            gameJson["convertToolVersion"] = packageInfo.version;

            gameJson["isWasmSplitSupport"] = isSupportWasmSplit;
            WriteJsonToFile(filePath, gameJson);
            ZipGame("game_wasm_split.zip");

            // ignore webgl.wasm.symbols.unityweb and udpate isWasmSplitSupport
            gameJson["isWasmSplitSupport"] = false;
            WriteJsonToFile(filePath, gameJson);
            ZipGame("game.zip", "webgl.wasm.symbols.unityweb");
        }

        private static void WriteJsonToFile(string filePath, JsonData gameJson)
        {
            JsonWriter writer = new JsonWriter();
            writer.IndentValue = 2;
            writer.PrettyPrint = true;
            gameJson.ToJson(writer);
            File.WriteAllText(filePath, writer.TextWriter.ToString());
        }

        static void ZipGame(string fileName, string ignoreFile = "")
        {
            var zipped = Path.Combine(config.ProjectConf.DST, fileName);
            UnityUtil.ZipGame(zipped, Path.Combine(config.ProjectConf.DST, TJConvertCore.miniGameDir), ignoreFile);
        }
    }
}