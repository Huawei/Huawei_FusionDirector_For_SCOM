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
// Created          : 12-27-2018
//
// Last Modified By : panwei
// Last Modified On : 12-27-2018
// ***********************************************************************
// <copyright file="EnclosureService.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Text;
using System.Threading.Tasks;
using FusionDirectorPlugin.Dal.Model;
using FusionDirectorPlugin.Model;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Api
{
#pragma warning disable

    /// <summary>
    /// Class MyClass.
    /// </summary>
    public class EnclosureService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnclosureService" /> class.
        /// </summary>
        /// <param name="fusionDirector">The fusion director.</param>
        public EnclosureService(FusionDirector fusionDirector) : base(fusionDirector)
        {
        }

        /// <summary>
        /// 返回机框的集合。
        /// </summary>
        /// <param name="top">$top 查询参数指明返回的集合中应该包含的个数。最小值为1。未提供该选项则返回所有数据。</param>
        /// <param name="skip">$skip 查询参数指明在查询的集合中有多少个需要从头部跳过不被包含在返回中。</param>
        /// <param name="filter">查询中使用的过滤器，例如 $filter=Name eq 'MyName' 。</param>
        /// <param name="orderby">$orderby 查询参数允许客户端指明返回的元素应该依据元素的某属性按升（asc）序或降序（desc）排列。例如 $orderby=Name asc 。当没有指明升序还是降序这按升序排列。</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>成功。</returns>
        public async Task<EnclosureList> GetEnclosureCollectionAsync(long? top, long? skip, string filter, string orderby)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl).Append("/redfish/v1/rich/Enclosures?");
            if (top != null)
            {
                urlBuilder.Append($"$top={top}").Append("&");
            }
            if (skip != null)
            {
                urlBuilder.Append($"$skip={skip}").Append("&");
            }
            if (filter != null)
            {
                urlBuilder.Append($"$filter={filter}").Append("&");
            }
            if (orderby != null)
            {
                urlBuilder.Append($"$orderby={orderby}").Append("&");
            }
            urlBuilder.Length--;
            var url = urlBuilder.ToString();
            try
            {
                var response = await httpClient.GetAsync(url);
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                this.ProcessResponse(url, response, responseData, "Info");
                return JsonConvert.DeserializeObject<EnclosureList>(responseData);
            }
            catch (Exception e)
            {
                throw ProcessException(url, e);
            }
        }

        /// <summary>
        /// 查询指定机框。
        /// </summary>
        /// <param name="id">机框的ID。</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>成功。</returns>
        public async Task<Enclosure> GetEnclosureAsync(string id)
        {
            var url = $"{BaseUrl}/redfish/v1/rich/Enclosures/{id}";
            try
            {
                var response = await httpClient.GetAsync(url);
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                this.ProcessResponse(url, response, responseData);
                return JsonConvert.DeserializeObject<Enclosure>(responseData);
            }
            catch (Exception e)
            {
                throw ProcessException(url, e);
               
            }
        }

    }
}