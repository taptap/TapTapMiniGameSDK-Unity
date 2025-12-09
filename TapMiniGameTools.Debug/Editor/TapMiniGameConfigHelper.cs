#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_2022_1_OR_NEWER && ENABLE_BUILD_PROFILE_2022
using UnityEditor.Build.Profile;
#endif

namespace TapTapMiniGame.Editor
{
    /// <summary>
    /// TapTap 小游戏配置帮助类
    /// 用于统一管理构建配置的加载和游戏信息的读取
    /// </summary>
    public static class TapMiniGameConfigHelper
    {
        /// <summary>
        /// 加载所有可用的 TapTap 构建配置
        /// </summary>
        /// <returns>构建配置列表</returns>
        public static List<BuildProfileInfo> LoadAllBuildProfiles()
        {
            var buildProfiles = new List<BuildProfileInfo>();
            
            try
            {
#if UNITY_2022_1_OR_NEWER && ENABLE_BUILD_PROFILE_2022
                // Unity 2022及以上版本，读取Build Profiles (需要定义ENABLE_BUILD_PROFILE_2022宏才启用)
                LoadBuildProfiles(buildProfiles);
#endif
                
                // 始终尝试加载传统配置作为备用
                LoadLegacyConfig(buildProfiles);
                
                if (buildProfiles.Count == 0)
                {
                    Debug.LogError("No TapTap build profiles found. Please build your game first!");
                }
                else
                {
                    Debug.Log($"Loaded {buildProfiles.Count} TapTap build profile(s)");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading build profiles: {e.Message}");
            }
            
            return buildProfiles;
        }
        
        /// <summary>
        /// 从指定的 DST 路径加载游戏信息
        /// </summary>
        /// <param name="dstPath">构建输出目录路径</param>
        /// <returns>游戏信息，如果加载失败返回 null</returns>
        public static GameInfo LoadGameInfoFromDst(string dstPath)
        {
            if (string.IsNullOrEmpty(dstPath))
            {
                Debug.LogError("DST path is empty. Build configuration is invalid!");
                return null;
            }

            string gameJsonPath = Path.Combine(dstPath, "minigame", "game.json");
            
            if (!File.Exists(gameJsonPath))
            {
                Debug.LogError($"game.json not found at: {gameJsonPath}. Please build your game first!");
                return null;
            }

            try
            {
                string jsonContent = File.ReadAllText(gameJsonPath);
                GameInfo gameInfo = JsonUtility.FromJson<GameInfo>(jsonContent);
                
                if (gameInfo == null || string.IsNullOrEmpty(gameInfo.appId))
                {
                    Debug.LogError($"Invalid game.json at: {gameJsonPath}. Missing appId field!");
                    return null;
                }
                
                Debug.Log($"Game info loaded: appId={gameInfo.appId}, productName={gameInfo.productName}");
                return gameInfo;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing game.json at {gameJsonPath}: {e.Message}");
                return null;
            }
        }
        
#if UNITY_2022_1_OR_NEWER && ENABLE_BUILD_PROFILE_2022
        /// <summary>
        /// 加载 Unity 2022+ 的 Build Profiles (需要定义ENABLE_BUILD_PROFILE_2022宏才启用)
        /// </summary>
        private static void LoadBuildProfiles(List<BuildProfileInfo> buildProfiles)
        {
            string buildProfilesPath = "Assets/Settings/Build Profiles";
            
            if (!Directory.Exists(buildProfilesPath))
            {
                Debug.Log("Build Profiles directory not found");
                return;
            }
            
            // 获取所有.asset文件
            string[] assetFiles = Directory.GetFiles(buildProfilesPath, "*.asset", SearchOption.TopDirectoryOnly);
            
            foreach (string assetFile in assetFiles)
            {
                try
                {
                    // 转换为相对路径用于AssetDatabase
                    string relativePath = assetFile.Replace(Application.dataPath, "Assets").Replace("\\", "/");
                    
                    // 使用AssetDatabase加载ScriptableObject
                    var buildProfile = AssetDatabase.LoadAssetAtPath<ScriptableObject>(relativePath);
                    
                    if (buildProfile != null && IsTapTapProfile(buildProfile))
                    {
                        string dst = ExtractDSTFromBuildProfile(buildProfile);
                        if (!string.IsNullOrEmpty(dst))
                        {
                            string profileName = Path.GetFileNameWithoutExtension(assetFile);
                            buildProfiles.Add(new BuildProfileInfo(profileName, relativePath, dst, "TapTap"));
                            Debug.Log($"Found TapTap Build Profile: {profileName} -> {dst}");
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error parsing build profile {assetFile}: {e.Message}");
                }
            }
        }
        
        /// <summary>
        /// 判断是否为 TapTap 配置文件
        /// </summary>
        private static bool IsTapTapProfile(ScriptableObject profile)
        {
            try
            {
                string profileTypeName = profile.GetType().Name;
                string profileTypeFullName = profile.GetType().FullName;
                
                // 对于DouYin配置文件，直接排除
                if (profileTypeName.Contains("DouYin", StringComparison.OrdinalIgnoreCase))
                {
                    Debug.Log($"发现DouYin配置文件，跳过: {profileTypeName}");
                    return false;
                }
                
                // 检查是否是BuildProfile或其子类，并且有miniGameSettings属性
                if (profile is BuildProfile buildProfile)
                {
                    Debug.Log($"确认是BuildProfile类型: {profileTypeName}");
                    
                    // 使用公共属性获取miniGameSettings
                    var miniGameSettings = buildProfile.miniGameSettings;
                    if (miniGameSettings == null)
                    {
                        Debug.Log($"miniGameSettings 属性值为空");
                        return false;
                    }
                    
                    // 检查MiniGameSettings的类型和命名空间
                    var settingsType = miniGameSettings.GetType();
                    string typeName = settingsType.Name;
                    string nameSpace = settingsType.Namespace;
                    
                    Debug.Log($"MiniGameSettings类型: {typeName}, 命名空间: {nameSpace}");
                    
                    // TapTap配置文件的独有标识：
                    // 1. 类型名为 TapTapMiniGameSettings
                    // 2. 命名空间为 TapTapMiniGame
                    bool isTapTapType = typeName.Equals("TapTapMiniGameSettings", StringComparison.OrdinalIgnoreCase);
                    bool isTapTapNamespace = nameSpace != null && nameSpace.Equals("TapTapMiniGame", StringComparison.OrdinalIgnoreCase);
                    
                    if (isTapTapType && isTapTapNamespace)
                    {
                        Debug.Log("通过类型和命名空间确认为TapTap配置文件");
                        return true;
                    }
                    
                    // 备用检查：检查HostName属性
                    string hostName = buildProfile.GetHostName();
                    Debug.Log($"检查到HostName: {hostName}");
                    if (!string.IsNullOrEmpty(hostName) && hostName.Equals("TapTap", StringComparison.OrdinalIgnoreCase))
                    {
                        Debug.Log("通过HostName确认为TapTap配置文件");
                        return true;
                    }
                    
                    Debug.Log($"不是TapTap配置文件: MiniGameSettings类型={typeName}, 命名空间={nameSpace}, HostName={hostName}");
                    return false;
                }
                
                Debug.Log($"不是BuildProfile类型，跳过: {profileTypeFullName}");
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error checking TapTap profile: {e.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// 从 BuildProfile 中提取 DST 路径
        /// </summary>
        private static string ExtractDSTFromBuildProfile(ScriptableObject profile)
        {
            try
            {
                // 只处理BuildProfile类型
                if (!(profile is BuildProfile buildProfile))
                {
                    Debug.Log($"配置文件类型 {profile.GetType().Name} 不是BuildProfile，跳过DST提取");
                    return null;
                }

                // 使用公共属性获取miniGameSettings
                var miniGameSettings = buildProfile.miniGameSettings;
                if (miniGameSettings == null)
                {
                    Debug.Log("miniGameSettings属性值为空");
                    return null;
                }
                
                // 通过反射获取DST路径（ProjectConf可能仍然是私有字段）
                var bindingFlags = System.Reflection.BindingFlags.Instance | 
                                  System.Reflection.BindingFlags.NonPublic | 
                                  System.Reflection.BindingFlags.Public;
                
                var projectConfField = miniGameSettings.GetType().GetField("ProjectConf", bindingFlags);
                if (projectConfField == null)
                {
                    Debug.Log("MiniGameSettings中未找到ProjectConf字段");
                    return null;
                }
                
                var projectConf = projectConfField.GetValue(miniGameSettings);
                if (projectConf == null)
                {
                    Debug.Log("ProjectConf字段值为空");
                    return null;
                }
                
                var dstField = projectConf.GetType().GetField("DST", bindingFlags);
                if (dstField == null)
                {
                    Debug.Log("ProjectConf中未找到DST字段");
                    return null;
                }
                
                string dst = (string)dstField.GetValue(projectConf);
                Debug.Log($"成功提取DST路径: {dst}");
                return dst;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error extracting DST from build profile: {e.Message}");
                return null;
            }
        }
#endif
        
        /// <summary>
        /// 加载传统的 MiniGameConfig.asset 配置
        /// </summary>
        private static void LoadLegacyConfig(List<BuildProfileInfo> buildProfiles)
        {
            try
            {
                // 加载传统MiniGameConfig.asset
                var config = AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/TapTapMiniGame/Editor/MiniGameConfig.asset");
                if (config != null)
                {
                    // 通过反射获取DST路径
                    var bindingFlags = System.Reflection.BindingFlags.Instance | 
                                      System.Reflection.BindingFlags.NonPublic | 
                                      System.Reflection.BindingFlags.Public;
                                      
                    var projectConfField = config.GetType().GetField("ProjectConf", bindingFlags);
                    if (projectConfField != null)
                    {
                        var projectConf = projectConfField.GetValue(config);
                        var dstField = projectConf.GetType().GetField("DST", bindingFlags);
                        if (dstField != null)
                        {
                            string dst = (string)dstField.GetValue(projectConf);
                            if (!string.IsNullOrEmpty(dst))
                            {
                                // 检查是否已存在相同路径的配置
                                bool exists = buildProfiles.Any(p => p.dstPath == dst);
                                if (!exists)
                                {
                                    buildProfiles.Add(new BuildProfileInfo("Legacy Config", "Assets/TapTapMiniGame/Editor/MiniGameConfig.asset", dst, "TapTap"));
                                    Debug.Log($"Found Legacy Config: {dst}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading legacy config: {e.Message}");
            }
        }
    }
    
    /// <summary>
    /// 构建配置信息
    /// </summary>
    [Serializable]
    public class BuildProfileInfo
    {
        public string profileName;
        public string profilePath;
        public string dstPath;
        public string hostName;
        
        public BuildProfileInfo(string name, string path, string dst, string host)
        {
            profileName = name;
            profilePath = path;
            dstPath = dst;
            hostName = host;
        }
    }
    
    /// <summary>
    /// 游戏信息数据结构
    /// </summary>
    [Serializable]
    public class GameInfo
    {
        public string appId;
        public string productName;
        public string companyName;
        public string productVersion;
        public string convertScriptVersion;
        public string convertToolVersion;
    }
}
#endif

