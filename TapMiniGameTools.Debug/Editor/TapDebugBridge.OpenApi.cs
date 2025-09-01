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
    public static void TapAchievement_Setup(AchievementManagerOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "TapAchievement_Setup", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到TapAchievement_Setup回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 注册成就监听器桥接
    /// </summary>
    public static void TapAchievement_RegisterCallBack(AchievementListener listener)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(listener);
        string messageData = JsonMapper.ToJson(new { type = "TapAchievement_RegisterCallBack", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到TapAchievement_RegisterCallBack回复: {response.ToJson()}");
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
    public static void TapAchievement_UnRegisterCallBack(AchievementListener listener)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(listener);
        string messageData = JsonMapper.ToJson(new { type = "TapAchievement_UnRegisterCallBack", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到TapAchievement_UnRegisterCallBack回复: {response.ToJson()}");
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
    public static void TapAchievement_Unlock(string achievementId)
    {
        string messageData = JsonMapper.ToJson(new { type = "TapAchievement_Unlock", param = new { achievementId = achievementId } });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到TapAchievement_Unlock回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 增加成就步数桥接
    /// </summary>
    public static void TapAchievement_Increment(string achievementId, int steps)
    {
        string messageData = JsonMapper.ToJson(new { type = "TapAchievement_Increment", param = new { achievementId = achievementId, steps = steps } });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到TapAchievement_Increment回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 显示所有成就桥接
    /// </summary>
    public static void TapAchievement_ShowAchievements()
    {
        string messageData = JsonMapper.ToJson(new { type = "TapAchievement_ShowAchievements" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到TapAchievement_ShowAchievements回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 设置Toast启用状态桥接
    /// </summary>
    public static void TapAchievement_SetToastEnable(bool enabled)
    {
        string messageData = JsonMapper.ToJson(new { type = "TapAchievement_SetToastEnable", param = new { enabled = enabled } });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到TapAchievement_SetToastEnable回复: {response.ToJson()}");
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
    public static void Tap_GetLeaderBoardManager()
    {
        string messageData = JsonMapper.ToJson(new { type = "Tap_GetLeaderBoardManager" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到Tap_GetLeaderBoardManager回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 打开排行榜桥接
    /// </summary>
    public static void Tap_OpenLeaderboard(OpenLeaderboardOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "Tap_OpenLeaderboard", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到Tap_OpenLeaderboard回复: {response.ToJson()}");
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
    public static void Tap_SubmitScore(SubmitScoreOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "Tap_SubmitScore", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到Tap_SubmitScore回复: {response.ToJson()}");
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
    public static void Tap_LoadLeaderboardScores(LoadLeaderboardScoresOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "Tap_LoadLeaderboardScores", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到Tap_LoadLeaderboardScores回复: {response.ToJson()}");
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
    public static void Tap_LoadCurrentPlayerLeaderboardScore(LoadCurrentPlayerLeaderboardScoreOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "Tap_LoadCurrentPlayerLeaderboardScore", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到Tap_LoadCurrentPlayerLeaderboardScore回复: {response.ToJson()}");
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
    public static void Tap_LoadPlayerCenteredScores(LoadPlayerCenteredScoresOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "Tap_LoadPlayerCenteredScores", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到Tap_LoadPlayerCenteredScores回复: {response.ToJson()}");
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

    #region 云存档
    /// <summary>
    /// 创建云存档管理器桥接
    /// </summary>
    public static void TapCloudSave_Setup()
    {
        string messageData = JsonMapper.ToJson(new { type = "TapCloudSave_Setup" });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到TapCloudSave_Setup回复: {response.ToJson()}");
        });
    }

    /// <summary>
    /// 创建存档桥接
    /// </summary>
    public static void TapCloudSave_CreateArchive(CreateArchiveOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "TapCloudSave_CreateArchive", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到TapCloudSave_CreateArchive回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<CloudArchiveOperationResponse>());
                    break;
                case "fail":
                    var failResult = response.GetResult<CloudSaveFailureResponse>();
                    option.fail?.Invoke(failResult.message.errno, failResult.message.errMsg);
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 更新存档桥接
    /// </summary>
    public static void TapCloudSave_UpdateArchive(UpdateArchiveOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "TapCloudSave_UpdateArchive", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到TapCloudSave_UpdateArchive回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<CloudArchiveOperationResponse>());
                    break;
                case "fail":
                    var failResult = response.GetResult<CloudSaveFailureResponse>();
                    option.fail?.Invoke(failResult.message.errno, failResult.message.errMsg);
                    break;
                case "complete":
                    option.complete?.Invoke(response.GetResult<TapCallbackResult>());
                    break;
            }
        });
    }

    /// <summary>
    /// 获取存档列表桥接
    /// </summary>
    public static void TapCloudSave_GetArchiveList(GetArchiveListOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "TapCloudSave_GetArchiveList", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到TapCloudSave_GetArchiveList回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<CloudArchiveListResponse>());
                    break;
                case "fail":
                    var failResult = response.GetResult<CloudSaveFailureResponse>();
                    option.fail?.Invoke(failResult.message.errno, failResult.message.errMsg);
                    break;
            }
        });
    }

    /// <summary>
    /// 获取存档数据桥接
    /// </summary>
    public static void TapCloudSave_GetArchiveData(GetArchiveDataOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "TapCloudSave_GetArchiveData", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到TapCloudSave_GetArchiveData回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<CloudArchiveDataResponse>());
                    break;
                case "fail":
                    var failResult = response.GetResult<CloudSaveFailureResponse>();
                    option.fail?.Invoke(failResult.message.errno, failResult.message.errMsg);
                    break;
            }
        });
    }

    /// <summary>
    /// 获取存档封面桥接
    /// </summary>
    public static void TapCloudSave_GetArchiveCover(GetArchiveCoverOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "TapCloudSave_GetArchiveCover", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到TapCloudSave_GetArchiveCover回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<CloudArchiveCoverResponse>());
                    break;
                case "fail":
                    var failResult = response.GetResult<CloudSaveFailureResponse>();
                    option.fail?.Invoke(failResult.message.errno, failResult.message.errMsg);
                    break;
            }
        });
    }

    /// <summary>
    /// 删除存档桥接
    /// </summary>
    public static void TapCloudSave_DeleteArchive(DeleteArchiveOption option)
    {
        var serializableParam = TapSDKApiUtil.CreateSerializableObject(option);
        string messageData = JsonMapper.ToJson(new { type = "TapCloudSave_DeleteArchive", param = serializableParam });
        NetworkServerModule.Instance.SendMessage(messageData, (clientId, response) =>
        {
            Debug.Log($"[测试] 收到TapCloudSave_DeleteArchive回复: {response.ToJson()}");
            switch (response.status)
            {
                case "success":
                    option.success?.Invoke(response.GetResult<CloudArchiveOperationResponse>());
                    break;
                case "fail":
                    var failResult = response.GetResult<CloudSaveFailureResponse>();
                    option.fail?.Invoke(failResult.message.errno, failResult.message.errMsg);
                    break;
            }
        });
    }
    #endregion

} 
#endif 