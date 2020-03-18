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
// Assembly         : FusionDirectorPlugin.Api
// Author           : panwei
// Created          : 12-28-2018
//
// Last Modified By : panwei
// Last Modified On : 12-28-2018
// ***********************************************************************
// <copyright file="BaseService.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using FusionDirectorPlugin.Dal;
using FusionDirectorPlugin.Dal.Model;
using FusionDirectorPlugin.LogUtil;
using Newtonsoft.Json;
using NLog;

namespace FusionDirectorPlugin.Api
{
    /// <summary>
    /// Class BaseService.
    /// </summary>
    public partial class BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService" /> class.
        /// </summary>
        /// <param name="fusionDirector">The fusion director.</param>
        public BaseService(FusionDirector fusionDirector)
        {
            //this.LocalCerts = CertDal.Instance.GetCerts();
            this.FusionDirector = fusionDirector;
            this.ApiLogger = new FusionDirectorLogger(fusionDirector.HostIP).Api;
            this.httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(120) };
            this.httpClient.DefaultRequestHeaders.Add("Authorization", this.BaseAuthStr);
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public Logger ApiLogger { get; set; }

        /// <summary>
        /// Gets or sets the fd.
        /// </summary>
        /// <value>The fd.</value>
        public FusionDirector FusionDirector { get; set; }

        /// <summary>
        /// Gets the base authentication string.
        /// </summary>
        /// <value>The base authentication string.</value>
        public string BaseAuthStr => "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(this.FusionDirector.LoginAccount + ":" + this.FusionDirector.LoginPd));

        /// <summary>
        /// Gets or sets the HTTP client.
        /// </summary>
        /// <value>The HTTP client.</value>
        public HttpClient httpClient { get; set; }

        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        public string BaseUrl => $"https://{this.FusionDirector.HostIP}:{this.FusionDirector.Port}";

        /// <summary>
        /// fd证书，跟服务端返回的对比
        /// </summary>
        /// <value>The local cert.</value>
        public List<X509Certificate2> LocalCerts { get; set; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="Exception">params error
        /// or
        /// username or password is incorrect
        /// or
        /// system error
        /// or
        /// user type is error
        /// or
        /// 432
        /// or
        /// user is locked
        /// or
        /// username or password is incorrect</exception>
        public string GetErrorMessage(HttpStatusCode statusCode)
        {
            var code = (int)statusCode;
            switch (code)
            {
                case 200:
                case 201:
                    return "Success.";
                case 400:
                    return "Parameter error.";
                case 430:
                    return "Incorrect username or password.";
                case 429:
                    return "System error.";
                case 431:
                    return "Incorrect user type.";
                case 432:
                    return " The number of created sessions reaches the upper limit.";
                case 445:
                    return "The user is locked.";
                case 404:
                    return "Not Found.";
                case 500:
                    return "Server Error.";
                default:
                    return "Incorrect username or password.";
            }
        }

        /// <summary>
        /// Processes the response.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="response">The response.</param>
        /// <param name="data">The data.</param>
        public void ProcessResponse(string url, HttpResponseMessage response, string data, string loglevel = "Debug")
        {
            if (loglevel.ToUpper() == "INFO")
            {
                ApiLogger.Info($"{url}\n-[{(int)response.StatusCode}][data:{data ?? string.Empty}]");
            }
            else
            {
                ApiLogger.Debug($"{url}\n-[{(int)response.StatusCode}][data:{data ?? string.Empty}]");
            }
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Processes the response.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        public void ProcessRequest(string url, object data)
        {
            ApiLogger.Debug($"{url}\n-[data:{(data == null ? string.Empty : HidePd(JsonConvert.SerializeObject(data)))}]");
        }

        /// <summary>
        /// Processes the response.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="ex">The ex.</param>
        public Exception ProcessException(string url, Exception ex)
        {
            var innerEx = GetInnerException(ex);
            ApiLogger.Error(innerEx, $"{url}");
            return innerEx;
        }

        private Exception GetInnerException(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return GetInnerException(ex.InnerException);
            }
            return ex;
        }

        /// <summary>
        /// 隐藏Json字符串中的密码
        /// </summary>
        /// <param name="jsonData">The json data.</param>
        /// <returns>System.String.</returns>
        private string HidePd(string jsonData)
        {
            var replacement = "\"${str}\":\"********\"";
            var pattern1 = "\"(?<str>([A-Za-z0-9_]*)[Password])\":\"(.*?)\"";
            string newJsonData = Regex.Replace(
                jsonData,
                pattern1,
                replacement,
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            return newJsonData;
        }
    }
}
