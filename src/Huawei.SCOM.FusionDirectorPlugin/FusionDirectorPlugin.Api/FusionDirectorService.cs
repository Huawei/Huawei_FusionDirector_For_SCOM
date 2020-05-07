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
// <copyright file="FusionDirectorService.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Net;
using System.Threading.Tasks;
using FusionDirectorPlugin.Dal.Model;
using FusionDirectorPlugin.Model;
using Newtonsoft.Json;
using System.Security.Authentication;

namespace FusionDirectorPlugin.Api
{
    /// <summary>
    /// Class FusionDirectorService.
    /// </summary>
    /// <seealso cref="FusionDirectorPlugin.Api.BaseService" />
    public class FusionDirectorService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FusionDirectorService"/> class.
        /// </summary>
        /// <param name="fusionDirector">The fusion director.</param>
        public FusionDirectorService(FusionDirector fusionDirector) : base(fusionDirector)
        {
        }

        /// <summary>
        /// Tests the link fd.
        /// </summary>
        /// <returns>ApiResult.</returns>
        /// <exception cref="Exception">ex</exception>
        public ApiResult TestLinkFd()
        {
            var url = this.BaseUrl + "/redfish/v1/SessionService/Sessions";
            var ret = new ApiResult("-1", "Connect failed!");
            try
            {
                var response = httpClient.GetAsync(url).Result;
                var responseData = response.Content.ReadAsStringAsync().Result;
                this.ProcessResponse(url, response, responseData);
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    ret = new ApiResult("0", "Connect successful!");
                }
                else
                {
                    ret.Msg = this.GetErrorMessage(response.StatusCode);
                }
            }

            catch (Exception ex)
            {
                var e = ProcessException(url, ex);
                if (e is AuthenticationException)
                {
                    ret.Msg = "Certificate authentication failed. Upload a correct certificate. " + e.Message;
                }
                else
                {
                    ret.Msg = e.Message;
                }
            }
            return ret;
        }

        /// <summary>
        /// Tests the link fd.
        /// </summary>
        /// <returns>ApiResult.</returns>
        /// <exception cref="Exception">ex</exception>
        public ApplianceVersion GetApplianceVersion()
        {
            var url = this.BaseUrl + "/redfish/v1/rich/Appliance/Version";
            try
            {
                var response = httpClient.GetAsync(url).Result;
                var responseData = response.Content.ReadAsStringAsync().Result;
                this.ProcessResponse(url, response, responseData);
                return JsonConvert.DeserializeObject<ApplianceVersion>(responseData);
            }
            catch (Exception e)
            {
                throw ProcessException(url, e);
            }
        }
    }
}
