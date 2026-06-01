#nullable enable
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 道具直购支付回调结果。
    /// </summary>
    [Preserve]
    public class PaymentGameItemResult
    {
        /// <summary>
        /// 错误码，0 表示成功。
        /// </summary>
        public int errCode;

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg = "";
    }
}
