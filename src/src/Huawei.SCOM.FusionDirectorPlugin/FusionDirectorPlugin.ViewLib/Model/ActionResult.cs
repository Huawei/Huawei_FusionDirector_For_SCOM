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
// Assembly         : FusionDirectorPlugin.ViewLib
// Author           : mike
// Created          : 05-09-2019
//
// Last Modified By : mike
// Last Modified On : 05-09-2019
// ***********************************************************************
// <copyright file="ActionResult.cs" company="mike">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace FusionDirectorPlugin.ViewLib.Model
{
    /// <summary>
    /// Class Result.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// The default failire code
        /// </summary>
        public const int DEFAULT_FAILIRE_CODE = 100;

        /// <summary>
        /// 错误代码
        /// </summary>
        /// <value>The error code.</value>
        public int Code { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        /// <value>The MSG.</value>
        public string Message { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        /// <value>The MSG.</value>
        public Exception Cause { get; set; }

        /// <summary>
        /// 成功 失败
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success => this.Code == 0;

        /// <summary>
        /// Dones this instance.
        /// </summary>
        /// <returns>Result.</returns>
        public static Result Done()
        {
            Result result = new Result
            {
                Code = 0,
                Message = string.Empty,
            };
            return result;
        }

        /// <summary>
        /// Dones the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Result.</returns>
        public static Result Done(string message)
        {
            Result result = new Result
            {
                Code = 0,
                Message = message,
            };
            return result;
        }


        /// <summary>
        /// Faileds the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <returns>Result.</returns>
        public static Result Failed(int code, string message)
        {
            Result result = new Result
            {
                Message = message,
                Code = code
            };
            return result;
        }

        /// <summary>
        /// Faileds the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="cause">The cause.</param>
        /// <returns>Result.</returns>
        public static Result Failed(int code, string message, Exception cause)
        {
            Result result = new Result
            {
                Message = message,
                Code = code,
                Cause = cause,
            };
            return result;
        }
    }

    /// <summary>
    /// Class Result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="FusionDirectorPlugin.ViewLib.Model.Result" />
    public class Result<T> : Result
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public T Data { get; set; }

        /// <summary>
        /// Dones the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Result.</returns>
        public static Result<T> Done(T data)
        {
            Result<T> result = new Result<T>
            {
                Code = 0,
                Message = string.Empty,
                Data = data
            };
            return result;
        }

        /// <summary>
        /// Faileds the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="cause">The cause.</param>
        /// <returns>Result&lt;T&gt;.</returns>
        public static Result<T> Failed(string message, Exception cause)
        {
            Result<T> result = new Result<T>
            {
                Message = message,
                Code = DEFAULT_FAILIRE_CODE,
                Cause = cause,
                Data = default(T)
            };
            return result;
        }

        /// <summary>
        /// Faileds the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="cause">The cause.</param>
        /// <returns>Result&lt;T&gt;.</returns>
        public static new Result<T> Failed(int code, string message, Exception cause)
        {
            Result<T> result = new Result<T>
            {
                Message = message,
                Code = code,
                Cause = cause,
                Data = default(T)
            };
            return result;
        }
    }
}