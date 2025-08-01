#if (UNITY_WEBGL || UNITY_MINIGAME) && UNITY_EDITOR && TAP_DEBUG_ENABLE
using System;
using System.Collections;
using System.Collections.Generic;
using TapTapMiniGame;
using UnityEngine;
using LitJson;
using TapServer;

public partial class TapDebugBridge
{
    #region 开放接口
    #region 用户信息
    /// <summary>
    /// 获取用户信息桥接
    /// </summary>
    public static void GetUserInfo(GetUserInfoOption option)
    {
        string messageData = JsonMapper.ToJson(new { type = "GetUserInfo" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetUserInfo回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetUserInfoSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }
    #endregion

    #region 登录
    /// <summary>
    /// 登录接口桥接
    /// </summary>
    public static void Login(LoginOption option)
    {
        string messageData = JsonMapper.ToJson(new { type = "Login" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到Login回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<LoginSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<RequestFailCallbackErr>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 检查会话状态桥接
    /// </summary>
    public static void CheckSession(CheckSessionOption option)
    {
        string messageData = JsonMapper.ToJson(new { type = "CheckSession" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到CheckSession回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }
    #endregion

    #region 授权
    /// <summary>
    /// 请求授权桥接
    /// </summary>
    public static void Authorize(AuthorizeOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "Authorize", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到Authorize回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }
    #endregion

    #region 桌面文件夹
    /// <summary>
    /// 创建桌面小组件桥接
    /// </summary>
    public static void CreateHomeScreenWidget(CreateHomeScreenWidgetOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "CreateHomeScreenWidget", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到CreateHomeScreenWidget回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<CreateHomeScreenWidgetOptionResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 检查桌面小组件是否存在且已固定桥接
    /// </summary>
    public static void HasHomeScreenWidgetAndPinned(HasHomeScreenWidgetOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "HasHomeScreenWidgetAndPinned", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到HasHomeScreenWidgetAndPinned回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<HasHomeScreenWidgetOptionResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }
    #endregion

    #region 成就系统
    /// <summary>
    /// 设置成就管理器桥接
    /// </summary>
    public static void SetupAchievementManager(AchievementManagerOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SetupAchievementManager", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetupAchievementManager回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 注册成就监听器桥接
    /// </summary>
    public static void AchievementManager_RegisterListener(AchievementListener listener)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(listener);
        string messageData = JsonMapper.ToJson(new { type = "AchievementManager_RegisterListener", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到AchievementManager_RegisterListener回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    listener.success?.Invoke(response.GetResult<AchievementListenerSuccess>());
                    break;
                case "fail":
                    listener.fail?.Invoke(response.GetResult<AchievementListenerFail>());
                    break;
            }
        });
    }

    /// <summary>
    /// 取消注册成就监听器桥接
    /// </summary>
    public static void AchievementManager_UnregisterListener(AchievementListener listener)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(listener);
        string messageData = JsonMapper.ToJson(new { type = "AchievementManager_UnregisterListener", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到AchievementManager_UnregisterListener回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    listener.success?.Invoke(response.GetResult<AchievementListenerSuccess>());
                    break;
                case "fail":
                    listener.fail?.Invoke(response.GetResult<AchievementListenerFail>());
                    break;
            }
        });
    }

    /// <summary>
    /// 解锁成就桥接
    /// </summary>
    public static void AchievementManager_Unlock(string achievementId)
    {
        string messageData = JsonMapper.ToJson(new { type = "AchievementManager_Unlock", param = new { achievementId = achievementId } });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到AchievementManager_Unlock回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 增加成就步数桥接
    /// </summary>
    public static void AchievementManager_IncrementSteps(string achievementId, int steps)
    {
        string messageData = JsonMapper.ToJson(new { type = "AchievementManager_IncrementSteps", param = new { achievementId = achievementId, steps = steps } });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到AchievementManager_IncrementSteps回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 显示所有成就桥接
    /// </summary>
    public static void AchievementManager_ShowAllAchievements()
    {
        string messageData = JsonMapper.ToJson(new { type = "AchievementManager_ShowAllAchievements" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到AchievementManager_ShowAllAchievements回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 设置Toast启用状态桥接
    /// </summary>
    public static void AchievementManager_SetToastEnabled(bool enabled)
    {
        string messageData = JsonMapper.ToJson(new { type = "AchievementManager_SetToastEnabled", param = new { enabled = enabled } });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到AchievementManager_SetToastEnabled回复: {response.ToJson()}");
        });
    }
    #endregion

    #region 设置
    /// <summary>
    /// 打开设置页面桥接
    /// </summary>
    public static void OpenSetting(OpenSettingOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OpenSetting", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenSetting回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<OpenSettingSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取设置桥接
    /// </summary>
    public static void GetSetting(GetSettingOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetSetting", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetSetting回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetSettingSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }
    #endregion

    #region 账号信息
    // GetAccountInfoSync 是同步方法，不需要桥接
    #endregion

    #region 隐私信息授权
    /// <summary>
    /// 需要隐私授权桥接
    /// </summary>
    public static void RequirePrivacyAuthorize(RequirePrivacyAuthorizeOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "RequirePrivacyAuthorize", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到RequirePrivacyAuthorize回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 打开隐私协议桥接
    /// </summary>
    public static void OpenPrivacyContract(OpenPrivacyContractOption option)
    {
        string messageData = JsonMapper.ToJson(new { type = "OpenPrivacyContract" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenPrivacyContract回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取隐私设置桥接
    /// </summary>
    public static void GetPrivacySetting(GetPrivacySettingOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "GetPrivacySetting", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetPrivacySetting回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<GetPrivacySettingSuccessCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<GeneralCallbackResult>());
                    break;
            }
        });
    }
    #endregion

    #region 好友和分享
    /// <summary>
    /// 分享功能桥接
    /// </summary>
    public static void ShowShareboard(ShowShareboardOption option)
    {
        // 使用新的序列化方法，自动排除函数字段
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "ShowShareboard", param = serializableParam });
        
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到Shared回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }
    public static void OnShareMessage(OnShareOption option)
    {
        // 使用新的序列化方法，自动排除函数字段
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OnShareMessage", param = serializableParam });
        
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OnShareMessage回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<OnShareMessageListenerResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<OnShareMessageListenerResult>());
                    break;
            }
        });
    }
    
    /// <summary>
    /// 设置分享面板隐藏状态桥接
    /// </summary>
    public static void SetShareboardHidden(SetShareboardHiddenOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SetShareboardHidden", param = serializableParam });
        
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SetShareboardHidden回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "fail":
                    option.fail?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }
    
    /// <summary>
    /// 取消分享消息监听桥接
    /// </summary>
    public static void OffShareMessage()
    {
        string messageData = JsonMapper.ToJson(new { type = "OffShareMessage" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OffShareMessage回复: {response.ToJson()}");
        });
    }
    
    public static void OpenFriendList()
    {
        string messageData = JsonMapper.ToJson(new { type = "OpenFriendList" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenFriendList回复: {response.ToJson()}");
        });
    }
    

    #endregion
    #endregion
     #region 排行榜
    /// <summary>
    /// 获取排行榜管理器桥接
    /// </summary>
    public static void GetLeaderBoardManager()
    {
        string messageData = JsonMapper.ToJson(new { type = "GetLeaderBoardManager" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到GetLeaderBoardManager回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 打开排行榜桥接
    /// </summary>
    public static void OpenLeaderboard(OpenLeaderboardOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "OpenLeaderboard", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到OpenLeaderboard回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.callback?.onSuccess?.Invoke(response.GetResult<string>());
                    break;
                case "fail":
                    var failResult = response.GetResult<LeaderboardFailureResponse>();
                    option.callback?.onFailure?.Invoke(failResult.code, failResult.message);
                    break;
            }
        });
    }

    /// <summary>
    /// 提交分数桥接
    /// </summary>
    public static void SubmitScores(SubmitScoreOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "SubmitScores", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到SubmitScores回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.callback?.onSuccess?.Invoke(response.GetResult<SubmitScoresResponse>());
                    break;
                case "fail":
                    var failResult = response.GetResult<LeaderboardFailureResponse>();
                    option.callback?.onFailure?.Invoke(failResult.code, failResult.message);
                    break;
            }
        });
    }

    /// <summary>
    /// 加载排行榜分数桥接
    /// </summary>
    public static void LoadLeaderboardScores(LoadLeaderboardScoresOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "LoadLeaderboardScores", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到LoadLeaderboardScores回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.callback?.onSuccess?.Invoke(response.GetResult<LeaderboardScoreResponse>());
                    break;
                case "fail":
                    var failResult = response.GetResult<LeaderboardFailureResponse>();
                    option.callback?.onFailure?.Invoke(failResult.code, failResult.message);
                    break;
            }
        });
    }

    /// <summary>
    /// 加载当前玩家排行榜分数桥接
    /// </summary>
    public static void LoadCurrentPlayerLeaderboardScore(LoadCurrentPlayerLeaderboardScoreOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "LoadCurrentPlayerLeaderboardScore", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到LoadCurrentPlayerLeaderboardScore回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.callback?.onSuccess?.Invoke(response.GetResult<UserScoreResponse>());
                    break;
                case "fail":
                    var failResult = response.GetResult<LeaderboardFailureResponse>();
                    option.callback?.onFailure?.Invoke(failResult.code, failResult.message);
                    break;
            }
        });
    }

    /// <summary>
    /// 加载玩家为中心的分数桥接
    /// </summary>
    public static void LoadPlayerCenteredScores(LoadPlayerCenteredScoresOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "LoadPlayerCenteredScores", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到LoadPlayerCenteredScores回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.callback?.onSuccess?.Invoke(response.GetResult<LeaderboardScoreResponse>());
                    break;
                case "fail":
                    var failResult = response.GetResult<LeaderboardFailureResponse>();
                    option.callback?.onFailure?.Invoke(failResult.code, failResult.message);
                    break;
            }
        });
    }
    #endregion

} 
#endif 