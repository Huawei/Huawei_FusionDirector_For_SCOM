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
// Created          : 05-05-2019
//
// Last Modified By : mike
// Last Modified On : 05-05-2019
// ***********************************************************************
// <copyright file="FdClient.cs" company="mike">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Authentication;
using System.Text;
using FusionDirectorPlugin.ViewLib.Model;
using FusionDirectorPlugin.ViewLib.Utils;
using System.Threading.Tasks;
using System.Net.Security;

namespace FusionDirectorPlugin.ViewLib.Client
{
    /// <summary>
    /// Class FdClient.
    /// </summary>
    internal class FdClient
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FdClient"/> class.
        /// </summary>
        /// <param name="fusionDirector">The e sight.</param>
        public FdClient(FdAppliance fusionDirector)
        {
            this.Appliance = fusionDirector;
            this.httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            this.httpClient.DefaultRequestHeaders.Add("Authorization", this.BaseAuthStr);

            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                return sslPolicyErrors == SslPolicyErrors.None || sslPolicyErrors == SslPolicyErrors.RemoteCertificateNameMismatch;
            };
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the e sight.
        /// </summary>
        /// <value>The e sight.</value>
        public FdAppliance Appliance { get; set; }


        /// <summary>
        /// Gets the base authentication string.
        /// </summary>
        /// <value>The base authentication string.</value>
        public string BaseAuthStr => "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(this.Appliance.LoginAccount + ":" + this.Appliance.LoginPd));

        /// <summary>
        /// Gets or sets the HTTP client.
        /// </summary>
        /// <value>The HTTP client.</value>
        public HttpClient httpClient { get; set; }

        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        public string BaseUrl => $"https://{this.Appliance.HostIP}:{this.Appliance.Port}";


        #endregion

        #region 测试连接

        /// <summary>
        /// Reads as json data contract.
        /// </summary>
        /// <typeparam name="T">t</typeparam>
        /// <param name="jsonString">The json string.</param>
        /// <returns>T.</returns>
        public T ReadAsJsonDataContract<T>(string jsonString)
        {
            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            return (T)((object)dataContractJsonSerializer.ReadObject(stream));
        }

        /// <summary>
        /// Gets the appliance version.
        /// </summary>
        /// <returns>ApiResult.</returns>
        /// <exception cref="Exception">ex</exception>
        public async Task<ApplianceVersion> GetApplianceVersion()
        {
            var url = this.BaseUrl + "/redfish/v1/rich/Appliance/Version";
            try
            {
                var response = await this.httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseData = response.Content.ReadAsStringAsync().Result;
                return this.ReadAsJsonDataContract<ApplianceVersion>(responseData);
            }
            catch (Exception e)
            {
                throw this.ProcessException(url, e);
            }
        }

        /// <summary>
        /// Tests the connection.
        /// </summary>
        /// <returns>Result.</returns>
        public async Task<Result> TestCredential()
        {
            var url = this.BaseUrl + "/redfish/v1/SessionService/Sessions";
            var ret = Result.Failed(-1, "Connect failed!");
            try
            {
                var response = await this.httpClient.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    ret = Result.Done("Connect successful!");
                }
                else
                {
                    ret.Message = this.GetErrorMessage(response.StatusCode);
                }
            }
            catch  (TaskCanceledException ex)
            {
                ret.Message = "Connect timeout";
            }
            catch (Exception ex)
            {
                var e = this.ProcessException(url, ex);
                ret.Message = e is AuthenticationException ? "Certificate valid falid.Please upload the correct certificate." : e.Message;
            }
            return ret;
        }

        /// <summary>
        /// 删除事件订阅资源.
        /// </summary>
        /// <param name="id">资源 ID.</param>
        /// <returns>OK</returns>
        /// <exception cref="ArgumentNullException">id</exception>
        /// <exception cref="Exception">A server side error occurred.</exception>
        public async Task<Result> DeleteGivenSubscriptions(string id)
        {
            var url = $"{this.BaseUrl}/{"redfish/v1/EventService/Subscriptions/{id}"}";
            var ret = Result.Failed(-1, "Unsuccessful unsubscribe!");
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("id");
                }
                url = url.Replace("{id}", Uri.EscapeDataString(id));
                var response = await this.httpClient.DeleteAsync(url);
                var responseData = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(responseData);
                }
                ret = Result.Done("Successful unsubscribe!");
            }
            catch (Exception ex)
            {
                var e = this.ProcessException(url, ex);
                ret.Message = e is AuthenticationException ? "Certificate valid falid.Please upload the correct certificate." : e.Message;
            }
            return ret;
        }

        #endregion
        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>System.String.</returns>
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
                    return "The number of created sessions reaches the upper limit.";
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
        /// <param name="ex">The ex.</param>
        /// <returns>Exception.</returns>
        public Exception ProcessException(string url, Exception ex)
        {
            var innerEx = this.GetInnerException(ex);
            LogHelper.Error(innerEx, url);
            return innerEx;
        }

        /// <summary>
        /// Gets the inner exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>Exception.</returns>
        private Exception GetInnerException(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return this.GetInnerException(ex.InnerException);
            }
            return ex;
        }
    }
}
