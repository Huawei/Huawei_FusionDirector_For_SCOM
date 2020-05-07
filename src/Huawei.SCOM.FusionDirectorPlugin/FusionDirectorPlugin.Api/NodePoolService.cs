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
// Created          : 12-26-2018
//
// Last Modified By : mike
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="NodePoolService.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Threading.Tasks;
using FusionDirectorPlugin.Dal.Model;
using FusionDirectorPlugin.Model;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Api
{
    /// <summary>
    /// Class NodePoolService.
    /// </summary>
    public class NodePoolService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodePoolService"/> class.
        /// </summary>
        /// <param name="fusionDirector">The fusion director.</param>
        public NodePoolService(FusionDirector fusionDirector) : base(fusionDirector)
        {
        }

        /// <summary>
        /// 查询Node资源集合信息
        /// </summary>
        /// <param name="groupId">指定Groups资源的系统组名称。</param>
        /// <param name="bMcip">指定BMC的IP地址。</param>
        /// <param name="group">指定Groups资源的用户组名称。</param>
        /// <param name="skip">指定分页结果的偏移量。</param>
        /// <param name="top">指定每页的最大显示数量。</param>
        /// <returns>OK</returns>
        public async Task<ServerList> GetServerCollectionAsync(string groupId, string bMcip, string group, int? skip, int? top)
        {
            var url = $"{this.BaseUrl}/redfish/v1/rich/Nodes?";
            try
            {
                if (groupId != null)
                {
                    url += $"GroupID={Uri.EscapeDataString(groupId)}&";
                }

                if (bMcip != null)
                {
                    url += $"BMCIP={Uri.EscapeDataString(bMcip)}&";
                }

                if (group != null)
                {
                    url += $"Group={Uri.EscapeDataString(group)}&";
                }

                if (skip != null)
                {
                    url += $"skip={Uri.EscapeDataString(skip.Value.ToString())}&";
                }

                if (top != null)
                {
                    url += $"top={Uri.EscapeDataString(top.Value.ToString())}&";
                }
                url = url.TrimEnd('?').TrimEnd('&');
                var response = await this.httpClient.GetAsync(url);
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false); this.ProcessResponse(url, response, responseData);
                this.ProcessResponse(url, response, responseData, "Debug");
                return JsonConvert.DeserializeObject<ServerList>(responseData);
            }
            catch (Exception e)
            {
                throw ProcessException(url, e);
            }
        }

        /// <summary>
        /// 查询指定Node资源信息
        /// </summary>
        /// <param name="id">指定Node资源的设备ID</param>
        /// <returns>OK</returns>
        /// <exception cref="System.ArgumentNullException">id</exception>
        public async Task<Server> GetServerInfoAsync(string id)
        {
            var url = $"{this.BaseUrl}/{"redfish/v1/rich/Nodes/{id}"}";
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("id");
                }
                url = url.Replace("{id}", Uri.EscapeDataString(id));

                var response = await this.httpClient.GetAsync(url);
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                this.ProcessResponse(url, response, responseData);
                return JsonConvert.DeserializeObject<Server>(responseData);
            }
            catch (Exception e)
            {
                throw ProcessException(url, e);
            }
        }

        /// <summary>
        /// 查询Node物理网卡资源集合信息
        /// </summary>
        /// <param name="id">Node的设备ID。</param>
        /// <returns>OK</returns>
        /// <exception cref="System.ArgumentNullException">id</exception>
        public async Task<ServerManagerCollection> GetServerManagerCollectionAsync(string id)
        {
            var url = $"{this.BaseUrl}/{"redfish/v1/rich/Nodes/{id}/Manager"}";
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("id");
                }
                url = url.Replace("{id}", Uri.EscapeDataString(id));
                var response = await this.httpClient.GetAsync(url);
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                this.ProcessResponse(url, response, responseData);
                return JsonConvert.DeserializeObject<ServerManagerCollection>(responseData);
            }
            catch (Exception e)
            {
                throw ProcessException(url, e);
            }
        }

    }
}