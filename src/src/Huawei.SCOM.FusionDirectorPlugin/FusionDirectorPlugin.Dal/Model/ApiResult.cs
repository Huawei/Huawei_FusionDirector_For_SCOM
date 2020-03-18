//**************************************************************************  
//Copyright (C) 2019 Huawei Technologies Co., Ltd. All rights reserved.
//This program is free software; you can redistribute it and/or modify
//it under the terms of the MIT license.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//MIT license for more detail.
//*************************************************************************  
﻿// ***********************************************************************
// Assembly         : FusionDirectorPlugin.Dal
// Author           : panwei
// Created          : 12-28-2018
//
// Last Modified By : panwei
// Last Modified On : 01-02-2019
// ***********************************************************************
// <copyright file="ApiResult.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Dal.Model
{
    /// <summary>
    /// Ajax请求返回结果
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResult" /> class.
        /// </summary>
        /// <param name="errCode">The err code.</param>
        /// <param name="msg">The msg.</param>
        public ApiResult(string errCode, string msg)
        {
            this.Code = errCode;
            this.Msg = msg;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResult" /> class.
        /// </summary>
        /// <param name="msg">The msg.</param>
        public ApiResult(string msg)
            : this("0", msg)
        {
        }

        /// <summary>
        /// 错误代码
        /// </summary>
        /// <value>The error code.</value>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        /// <value>The MSG.</value>
        [JsonProperty(PropertyName = "msg")]
        public string Msg { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        /// <value>The MSG.</value>
        [JsonProperty(PropertyName = "exceptionMsg")]
        public string ExceptionMsg { get; set; }

        /// <summary>
        /// 成功 失败
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        [JsonProperty(PropertyName = "success")]
        public bool Success => this.Code == "0";
    }

    /// <summary>
    /// 请求接口返回结果
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public class ApiResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResult{T}" /> class.
        /// </summary>
        /// <param name="errCode">The err code.</param>
        /// <param name="msg">The msg.</param>
        /// <param name="data">The data.</param>
        public ApiResult(string errCode, string msg, T data)
        {
            this.Code = errCode;
            this.Msg = msg;
            this.Data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResult{T}" /> class.
        /// </summary>
        /// <param name="msg">The msg.</param>
        /// <param name="data">The data.</param>
        public ApiResult(string msg, T data)
            : this("0", msg,  data)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResult{T}" /> class.
        /// </summary>
        /// <param name="errCode">The err code.</param>
        /// <param name="msg">The MSG.</param>
        public ApiResult(string errCode, string msg)
            : this(errCode, msg, default(T))
        {
        }

        /// <summary>
        /// 错误代码
        /// </summary>
        /// <value>The error code.</value>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        /// <value>The data.</value>
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        /// <value>The MSG.</value>
        [JsonProperty(PropertyName = "msg")]
        public string Msg { get; set; }

        /// <summary>
        /// 成功 失败
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        [JsonProperty(PropertyName = "success")]
        public bool Success => this.Code == "0";
    }

    /// <summary>
    /// The api list result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiListResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiListResult{T}" /> class.
        /// </summary>
        /// <param name="errCode">The err code.</param>
        /// <param name="msg">The msg.</param>
        /// <param name="data">The data.</param>
        public ApiListResult(string errCode, string msg,  IEnumerable<T> data)
        {
            this.Code = errCode;
            this.Msg = msg;
            this.Data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiListResult{T}" /> class.
        /// </summary>
        /// <param name="msg">The msg.</param>
        /// <param name="data">The data.</param>
        public ApiListResult(string msg, IEnumerable<T> data)
            : this("0", msg,  data)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiListResult{T}" /> class.
        /// </summary>
        /// <param name="errCode">The err code.</param>
        /// <param name="msg">The MSG.</param>
        public ApiListResult(string errCode, string msg)
            : this(errCode, msg, default(IEnumerable<T>))
        {
        }

        /// <summary>
        /// 成功 失败
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        [JsonProperty(PropertyName = "success")]
        public bool Success => this.Code == "0";

        /// <summary>
        /// 错误代码
        /// </summary>
        /// <value>The error code.</value>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        /// <value>The data.</value>
        [JsonProperty(PropertyName = "data")]
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        /// <value>The MSG.</value>
        [JsonProperty(PropertyName = "msg")]
        public string Msg { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        /// <value>The total number.</value>
        [JsonProperty(PropertyName = "totalNum")]
        public int TotalNum { get; set; }

        /// <summary>
        /// Gets or sets the exception MSG.
        /// </summary>
        /// <value>The exception MSG.</value>
        [JsonProperty(PropertyName = "exceptionMsg")]
        public string ExceptionMsg { get; set; }
    }
}