#nullable enable
using System;
using UnityEngine.Scripting;

namespace TapTapMiniGame
{
    /// <summary>
    /// 道具直购支付请求参数。
    /// </summary>
    [Preserve]
    public class RequestPaymentGameItemOption
    {
        /// <summary>
        /// 支付原串，String 格式 JSON，由业务服务端下发。
        /// </summary>
        public string signData = "";

        /// <summary>
        /// 支付签名，用于验证请求合法性，应由业务服务端生成。
        /// </summary>
        public string paySig = "";

        /// <summary>
        /// 用户态签名，用于验证用户身份合法性，应由业务服务端生成。
        /// </summary>
        public string signature = "";

        /// <summary>
        /// 接口调用成功的回调函数。
        /// </summary>
        public Action<PaymentGameItemResult>? success;

        /// <summary>
        /// 接口调用失败的回调函数。
        /// </summary>
        public Action<PaymentGameItemResult>? fail;

        /// <summary>
        /// 接口调用结束的回调函数，成功或失败都会调用。
        /// </summary>
        public Action<PaymentGameItemResult>? complete;
    }
}
