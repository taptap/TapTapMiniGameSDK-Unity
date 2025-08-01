#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using minihost.editor;
using TapTapMiniGame;

namespace TapTapMiniGame.Editor
{
    /// <summary>
    /// Tap小游戏调试客户端构建器
    /// 使用TapTap的构建系统，临时修改构建场景来构建调试客户端
    /// </summary>
    public class TapMiniGameDebugClientBuilder
    {
        #region 常量
        private const string DEBUG_SCENE_RELATIVE_PATH = "TapMiniGameTools.Debug/Client/Scenes/AppScene.unity";
        #endregion

        #region 私有变量
        private static EditorBuildSettingsScene[] originalScenes;
        private static bool isBuilding = false;
        #endregion

        #region 私有方法 - 路径获取
        /// <summary>
        /// 获取当前Package的根路径
        /// </summary>
        /// <returns>Package根路径，如果获取失败返回null</returns>
        private static string GetPackageRootPath()
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(TapMiniGameDebugClientBuilder).Assembly);
            if (packageInfo == null)
            {
                Debug.LogError("无法获取Package信息，请确保代码在Package中运行");
                return null;
            }
            
            return packageInfo.assetPath;
        }

        /// <summary>
        /// 获取调试场景的完整路径
        /// </summary>
        /// <returns>调试场景的完整路径</returns>
        private static string GetDebugScenePath()
        {
            string packageRoot = GetPackageRootPath();
            if (string.IsNullOrEmpty(packageRoot))
            {
                return null;
            }
            
            return Path.Combine(packageRoot, DEBUG_SCENE_RELATIVE_PATH).Replace("\\", "/");
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 构建Tap小游戏调试客户端
        /// </summary>
        public static void BuildDebugClient()
        {
            if (isBuilding)
            {
                Debug.LogWarning("构建正在进行中，请等待当前构建完成");
                return;
            }

            try
            {
                isBuilding = true;
                
                // 验证调试场景是否存在
                if (!ValidateDebugScene())
                {
                    return;
                }


                Debug.Log("开始构建Tap小游戏调试客户端...");

                // 备份当前构建场景配置
                BackupBuildScenes();
                
                // 设置调试场景为唯一构建场景
                SetDebugSceneAsBuildScene();
                
                // 执行TapTap构建
                ExecuteTapTapBuild();
                
                Debug.Log("Tap小游戏调试客户端构建完成! 构建输出请查看TapTap构建窗口的设置路径。");
            }
            catch (System.Exception ex)
            {
                // ExitGUIException 是Unity GUI系统的正常异常，应该重新抛出
                if (ex is UnityEngine.ExitGUIException)
                {
                    throw;
                }
                
                Debug.LogError($"构建Tap小游戏调试客户端失败: {ex.Message}");
                // EditorUtility.DisplayDialog("构建失败", $"构建过程中发生错误:\n{ex.Message}", "确定");
            }
            finally
            {
                // 恢复原始构建场景配置
                RestoreBuildScenes();
                isBuilding = false;
                EditorUtility.ClearProgressBar();
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 验证调试场景是否存在
        /// </summary>
        private static bool ValidateDebugScene()
        {
            string debugScenePath = GetDebugScenePath();
            if (string.IsNullOrEmpty(debugScenePath))
            {
                Debug.LogError("无法获取调试场景路径，请检查Package配置");
                EditorUtility.DisplayDialog("构建失败", "无法获取调试场景路径，请检查Package配置", "确定");
                return false;
            }

            if (!File.Exists(debugScenePath))
            {
                Debug.LogError($"调试场景不存在: {debugScenePath}");
                EditorUtility.DisplayDialog("构建失败", $"找不到调试场景文件:\n{debugScenePath}", "确定");
                return false;
            }
            
            Debug.Log($"找到调试场景: {debugScenePath}");
            return true;
        }

        /// <summary>
        /// 执行TapTap构建
        /// </summary>
        private static void ExecuteTapTapBuild()
        {
            Debug.Log("开始执行TapTap构建...");
            
            // 直接调用统一的构建方法，跳过验证因为已经在调用前验证过了
            // bool buildSuccess = TapTapMiniGame.TapTapBuildWindowHelper.BuildTapTapMiniGame(false);
            //
            // if (buildSuccess)
            // {
            //     Debug.Log("TapTap构建调用完成");
            // }
            // else
            // {
            //     Debug.LogError("TapTap构建失败，请查看日志了解详情");
            // }
        }

        /// <summary>
        /// 备份当前构建场景配置
        /// </summary>
        private static void BackupBuildScenes()
        {
            originalScenes = EditorBuildSettings.scenes;
            Debug.Log($"已备份 {originalScenes.Length} 个构建场景");
        }

        /// <summary>
        /// 恢复原始构建场景配置
        /// </summary>
        private static void RestoreBuildScenes()
        {
            if (originalScenes != null)
            {
                EditorBuildSettings.scenes = originalScenes;
                Debug.Log($"已恢复 {originalScenes.Length} 个构建场景");
                originalScenes = null;
            }
        }

        /// <summary>
        /// 设置调试场景为唯一构建场景
        /// </summary>
        private static void SetDebugSceneAsBuildScene()
        {
            string debugScenePath = GetDebugScenePath();
            var debugScene = new EditorBuildSettingsScene(debugScenePath, true);
            EditorBuildSettings.scenes = new[] { debugScene };
            Debug.Log($"已设置调试场景为构建场景: {debugScenePath}");
        }
        #endregion
    }
}
#endif 