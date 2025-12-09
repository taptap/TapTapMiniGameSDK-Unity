# Tap多人联机SDK API变更文档

**版本对比：** commit 1dc251f → 当前最新版本（HEAD）
**生成时间：** 2025-12-05
**主要变更：** 术语统一（"对战"→"帧同步"），API命名优化，数据结构调整

---

## 一、核心API变更（TapExt.cs）

### 1.1 帧同步管理API

| 变更类型 | 旧API（1dc251f） | 新API（最新版本） | 说明 |
|---------|-----------------|------------------|------|
| **方法重命名** | `StartBattle(StartBattleOption)` | `StartFrameSync(StartFrameSyncOption)` | 开始帧同步，参数类型同步更名 |
| **方法重命名** | `SendInput(SendInputOption)` | `SendFrameInput(SendFrameInputOption)` | 发送帧同步输入，参数类型同步更名 |
| **方法重命名** | `StopBattle(StopBattleOption)` | `StopFrameSync(StopFrameSyncOption)` | 停止帧同步，参数类型同步更名 |

**影响范围：** 所有使用帧同步功能的代码需要更新方法名和参数类型

---

### 1.2 房间管理API

| API名称 | 旧版本 | 新版本 | 变更说明 |
|--------|-------|--------|---------|
| `KickRoomPlayer` | 注释：仅限房主调用，且**房间未开战**时才能使用 | 注释：仅限房主调用，且**帧同步未开始**时才能使用 | 仅注释调整，功能不变 |

---

### 1.3 术语统一

| 位置 | 旧术语 | 新术语 |
|-----|--------|--------|
| 类注释 | Tap多人**对战**客户端 | Tap多人**联机**客户端 |
| 方法注释 | 初始化多人**对战**SDK | 初始化多人**联机**SDK |
| 方法注释 | 连接多人**对战**服务 | 连接多人**联机**服务 |
| 方法注释 | 开始**对战** | 开始**帧同步** |
| 方法注释 | 停止**对战** | 停止**帧同步** |

---

## 二、参数类型变更（BattleOption.cs）

### 2.1 帧同步相关选项类重命名

| 旧类名 | 新类名 | 用途 |
|-------|--------|------|
| `StartBattleOption` | `StartFrameSyncOption` | 开始帧同步的参数 |
| `SendInputOption` | `SendFrameInputOption` | 发送帧同步输入的参数 |
| `StopBattleOption` | `StopFrameSyncOption` | 停止帧同步的参数 |

**字段内容：** 三个类的字段（success/fail/complete回调）均未改变，仅类名变更

---

### 2.2 SendCustomMessageData 结构变更 ⚠️

**重要变更：** 自定义消息支持指定接收者

| 字段名 | 旧版本 | 新版本 | 变更说明 |
|-------|--------|--------|---------|
| `msg` | `string` 消息内容（最大2048字节） | `string` 消息内容（最大2048字节） | **无变化** |
| `type` | `int` 消息接收者类型：<br>0=房间内所有玩家<br>1=队伍内所有玩家 | `int` 消息接收者类型：<br>0=房间内所有玩家（**不包括发送者**）<br>1=**发送给指定玩家** | **含义变更** |
| `receivers` | ❌ 不存在 | ✅ `string[]` 接收方玩家ID列表<br>（当type==1时有效，最多20个ID） | **新增字段** |

**迁移建议：**
- 如果之前使用 `type=1` 发送给队伍，需要改为手动指定队伍玩家ID列表
- `type=0` 现在不包括发送者本人，如需包含需要额外处理

---

### 2.3 SendCustomMessageOption 结构简化

| 字段名 | 旧版本 | 新版本 | 变更说明 |
|-------|--------|--------|---------|
| `data` | `SendCustomMessageData` | `SendCustomMessageData` | **无变化**，但data内部结构有变化（见上节） |
| `msg` | `string` **已废弃字段** | ❌ **已移除** | 直接删除，必须使用 `data.msg` |
| `type` | `int` **已废弃字段** | ❌ **已移除** | 直接删除，必须使用 `data.type` |

---

## 三、事件处理器接口变更（ITapBattleEventHandler.cs）

### 3.1 事件方法重命名

| 旧方法名 | 新方法名 | 参数类型变更 | 说明 |
|---------|---------|-------------|------|
| `OnBattleStop` | `OnFrameSyncStop` | `BattleStopInfo` → `FrameSyncStopInfo` | 帧同步停止通知 |
| `OnBattleFrame` | `OnFrameInput` | `string frameData`（**无变化**） | 帧同步数据通知 |
| `OnBattleStart` | `OnFrameSyncStart` | `BattleStartInfo` → `FrameSyncStartInfo` | 帧同步开始通知 |

**影响范围：** 所有实现 `ITapBattleEventHandler` 接口的类需要更新方法名

---

## 四、数据模型变更（TapBattleModels.cs）

### 4.1 RoomConfig 匹配参数结构变更 ⚠️

**重要变更：** 匹配参数从固定结构改为灵活的键值对

| 字段名 | 旧版本 | 新版本 | 变更说明 |
|-------|--------|--------|---------|
| `matchParams` | `MatchParams` 类型<br>固定字段：<br>- `level`: string<br>- `score`: string | `Dictionary<string, string>` 类型<br>支持任意键值对 | **类型变更**<br>更灵活，支持自定义匹配参数 |

**迁移示例：**
```csharp
// 旧版本
var config = new RoomConfig {
    matchParams = new MatchParams {
        level = "10",
        score = "1000"
    }
};

// 新版本
var config = new RoomConfig {
    matchParams = new Dictionary<string, string> {
        { "level", "10" },
        { "score", "1000" },
        { "rank", "gold" }  // 支持任意自定义参数
    }
};
```

---

### 4.2 MatchParams 类移除

| 类名 | 旧版本 | 新版本 | 变更说明 |
|-----|--------|--------|---------|
| `MatchParams` | ✅ 存在<br>字段：level, score | ❌ **已完全移除** | 被 `Dictionary<string, string>` 替代 |

---

### 4.3 PlayerConfig 结构简化

| 字段名 | 旧版本 | 新版本 | 变更说明 |
|-------|--------|--------|---------|
| `customProperties` | `string` 自定义玩家属性 | `string` 自定义玩家属性 | **无变化** |
| `matchParams` | ✅ 存在（已移动到RoomConfig） | ❌ **已移除** | 匹配参数现在统一在RoomConfig中配置 |

---

### 4.4 GetRoomListRequest 新增 ✨

**新增类：** 获取房间列表请求参数（对齐JS层）

| 字段名 | 类型 | 说明 | 默认值 |
|-------|------|------|--------|
| `roomType` | `string` | 房间类型（可选，不填则拉取全部类型） | - |
| `offset` | `int` | 偏移量（可选，第一次请求时为0） | 0 |
| `limit` | `int` | 请求获取的房间数量（可选，最大100） | 20 |

**使用示例：**
```csharp
// 新版本支持分页查询
var option = new GetRoomListOption {
    data = new GetRoomListRequest {
        roomType = "casual",
        offset = 0,
        limit = 50
    },
    success = (response) => { /* 处理房间列表 */ }
};
TapBattleClient.GetRoomList(option);
```

---

### 4.5 GetRoomListOption 结构变更

| 字段名 | 旧版本 | 新版本 | 变更说明 |
|-------|--------|--------|---------|
| `data` | ❌ 不存在 | ✅ `GetRoomListRequest` 类型 | **新增字段**，支持分页和类型过滤 |
| `success` | `Action<GetRoomListSuccessResponse>` | `Action<GetRoomListSuccessResponse>` | **无变化** |
| `fail` | `Action<GetRoomListFailResponse>` | `Action<GetRoomListFailResponse>` | **无变化** |

---

## 五、事件监听器数据结构变更（TapBattleListenerOption.cs）

### 5.1 帧同步事件信息类重命名

| 旧类名 | 新类名 | 字段变更 |
|-------|--------|---------|
| `BattleStopInfo` | `FrameSyncStopInfo` | 见下方详细说明 |
| `BattleStartInfo` | `FrameSyncStartInfo` | 见下方详细说明 |

---

### 5.2 FrameSyncStopInfo 字段变更 ⚠️

**重要变更：** battleId 类型从 string 改为 int

| 字段名 | 旧版本 | 新版本 | 变更说明 |
|-------|--------|--------|---------|
| `roomId` | `string` | `string` | **无变化** |
| `battleId` | `string` 对战ID | `int` 帧同步会话ID | **类型变更** string→int |
| `reason` | `int` 结束原因<br>0=房主主动结束<br>1=超时结束(30分钟) | `int` 结束原因<br>0=房主主动结束<br>1=超时结束(30分钟) | **无变化** |

---

### 5.3 FrameSyncStartInfo 字段变更 ⚠️

**重要变更：** battleId 类型从 string 改为 int

| 字段名 | 旧版本 | 新版本 | 变更说明 |
|-------|--------|--------|---------|
| `roomId` | `string` | `string` | **无变化** |
| `battleId` | `string` 对战ID | `int` 帧同步会话ID | **类型变更** string→int |
| `startTime` | `int` | `int` | **无变化** |
| `seed` | `int` 随机数种子 | `int` 随机数种子 | **无变化** |

**迁移建议：**
```csharp
// 旧版本
public void OnBattleStart(BattleStartInfo info) {
    string battleId = info.battleId;  // string类型
}

// 新版本
public void OnFrameSyncStart(FrameSyncStartInfo info) {
    int battleId = info.battleId;  // int类型
}
```

---

### 5.4 RoomPropertiesChangeInfo 字段类型变更 ⚠️

**重要变更：** 自定义属性从字典改为JSON字符串

| 字段名 | 旧版本 | 新版本 | 变更说明 |
|-------|--------|--------|---------|
| `id` | `string` | `string` | **无变化** |
| `name` | `string` | `string` | **无变化** |
| `customProperties` | `Dictionary<string, object>` | `string` JSON字符串 | **类型变更**<br>需要手动序列化/反序列化 |

**迁移示例：**
```csharp
// 旧版本
public void OnRoomPropertiesChange(RoomPropertiesChangeInfo info) {
    var value = info.customProperties["key"];  // 直接访问
}

// 新版本
public void OnRoomPropertiesChange(RoomPropertiesChangeInfo info) {
    var dict = JsonUtility.FromJson<Dictionary<string, object>>(info.customProperties);
    var value = dict["key"];  // 需要先反序列化
}
```

---

### 5.5 PlayerCustomPropertiesChangeInfo 字段类型变更 ⚠️

**重要变更：** 玩家属性从字典改为JSON字符串

| 字段名 | 旧版本 | 新版本 | 变更说明 |
|-------|--------|--------|---------|
| `playerId` | `string` | `string` | **无变化** |
| `properties` | `Dictionary<string, object>` | `string` JSON字符串 | **类型变更**<br>需要手动序列化/反序列化 |

---

### 5.6 CustomMessageInfo 结构变更 ⚠️

**重要变更：** 简化消息结构，移除type字段

| 字段名 | 旧版本 | 新版本 | 变更说明 |
|-------|--------|--------|---------|
| `playerId` | `string` 消息发送者玩家ID | `string` 消息发送者玩家ID | **无变化** |
| `message` | `object` 消息内容 | ❌ **已移除** | 改为 `msg` 字段 |
| `msg` | ❌ 不存在 | ✅ `string` 消息内容（UTF-8字符串） | **新增字段**，替代原message |
| `type` | `int` 消息类型 | ❌ **已移除** | 不再需要，消息类型在发送端指定 |

**迁移建议：**
```csharp
// 旧版本
public void OnCustomMessage(CustomMessageInfo info) {
    object message = info.message;
    int type = info.type;
    // 需要根据type判断如何处理message
}

// 新版本
public void OnCustomMessage(CustomMessageInfo info) {
    string msg = info.msg;
    // 直接使用字符串消息
}
```

---

### 5.7 TapOnlineBattleListenerOption 回调字段更新

| 回调字段名 | 旧类型 | 新类型 | 变更说明 |
|-----------|--------|--------|---------|
| `onBattleStop` | `Action<BattleStopInfo>` | `Action<FrameSyncStopInfo>` | 参数类型更名 |
| `onBattleFrame` | `Action<string>` | `Action<string>` | **无变化** |
| `onBattleStart` | `Action<BattleStartInfo>` | `Action<FrameSyncStartInfo>` | 参数类型更名 |

---

## 六、错误码模块变更（TapBattleErrorCodes.cs）

### 6.1 注释术语统一

| 位置 | 旧术语 | 新术语 | 变更说明 |
|-----|--------|--------|---------|
| 类注释 | 多人**对战**错误码常量 | 多人**联机**错误码常量 | 仅注释变更 |
| 字典注释 | 多人**对战**错误码字典 | 多人**联机**错误码字典 | 仅注释变更 |

**错误码常量和描述内容均未改变**

---

## 七、完整迁移检查清单

### ✅ 必须修改的代码

1. **[ ] 帧同步API调用**
   - `StartBattle` → `StartFrameSync`
   - `SendInput` → `SendFrameInput`
   - `StopBattle` → `StopFrameSync`

2. **[ ] 参数类型更新**
   - `StartBattleOption` → `StartFrameSyncOption`
   - `SendInputOption` → `SendFrameInputOption`
   - `StopBattleOption` → `StopFrameSyncOption`

3. **[ ] 事件处理器方法**
   - `OnBattleStop` → `OnFrameSyncStop`
   - `OnBattleFrame` → `OnFrameInput`
   - `OnBattleStart` → `OnFrameSyncStart`

4. **[ ] 事件参数类型**
   - `BattleStopInfo` → `FrameSyncStopInfo`
   - `BattleStartInfo` → `FrameSyncStartInfo`

5. **[ ] battleId类型处理**
   - `string battleId` → `int battleId`（在FrameSyncStartInfo和FrameSyncStopInfo中）

6. **[ ] 匹配参数结构**
   - `MatchParams` → `Dictionary<string, string>`
   - 移除PlayerConfig中的matchParams字段

### ⚠️ 需要特别注意的变更

1. **[ ] SendCustomMessageData.type含义变更**
   - 旧：0=所有玩家, 1=队伍玩家
   - 新：0=所有玩家（不含自己）, 1=指定玩家（配合receivers字段）

2. **[ ] 自定义属性类型变更**
   - `RoomPropertiesChangeInfo.customProperties`: Dictionary → string
   - `PlayerCustomPropertiesChangeInfo.properties`: Dictionary → string
   - 需要手动序列化/反序列化JSON

3. **[ ] CustomMessageInfo结构变更**
   - `message: object` → `msg: string`
   - 移除`type`字段

### 📝 可选功能迁移

1. **[ ] 获取房间列表支持分页**
   - 使用新增的`GetRoomListRequest`配置offset、limit、roomType

---

## 八、向后兼容性说明

### 🔴 不兼容变更（需要强制修改）

1. 帧同步API方法名和类型名变更
2. battleId类型从string改为int
3. MatchParams类移除
4. SendCustomMessageOption中msg/type字段移除

### 🟡 功能变更（需要评估影响）

1. SendCustomMessageData.type语义变更
2. 自定义属性从字典改为JSON字符串
3. CustomMessageInfo结构简化

### 🟢 仅注释变更（无需修改代码）

1. "对战"术语统一改为"帧同步"或"联机"
2. 错误码注释调整

---

## 九、联系与反馈

如有疑问或发现文档错误，请联系：
- **负责人：** 梁栋
- **问题反馈：** 提交至项目Issue

**文档版本：** v1.0
**生成脚本：** Claude Code
**最后更新：** 2025-12-05
