//**************************************************************************  
//Copyright (C) 2019 Huawei Technologies Co., Ltd. All rights reserved.
//This program is free software; you can redistribute it and/or modify
//it under the terms of the MIT license.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//MIT license for more detail.
//*************************************************************************  
﻿using System;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using FusionDirectorPlugin.Api;
using FusionDirectorPlugin.Dal.Model;

namespace FusionDirectorPlugin.Model
{
    public class EventService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodePoolService"/> class.
        /// </summary>
        /// <param name="fusionDirector">The fusion director.</param>
        public EventService(FusionDirector fusionDirector) : base(fusionDirector)
        {
        }

        /// <summary>获取服务信息.</summary>
        /// <returns>OK</returns>
        /// <exception cref="Exception">A server side error occurred.</exception>
        public async Task<EventServiceRsp> GetEventServiceinformationAsync()
        {
            var url = $"{this.BaseUrl}/redfish/v1/EventService";
            try
            {
                var response = await httpClient.GetAsync(url);
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                this.ProcessResponse(url, response, responseData);
                return JsonConvert.DeserializeObject<EventServiceRsp>(responseData);
            }
            catch (Exception e)
            {
                throw ProcessException(url, e);
            }
        }

        /// <summary>查询事件订阅集合资源.</summary>
        /// <returns>OK</returns>
        /// <exception cref="Exception">A server side error occurred.</exception>
        public async Task<SubscriptionsRsp> GetEventServiceSubscriptionsinformationAsync()
        {
            var url = $"{this.BaseUrl}/redfish/v1/EventService/Subscriptions";
            try
            {
                var response = await httpClient.GetAsync(url);
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                this.ProcessResponse(url, response, responseData);
                return JsonConvert.DeserializeObject<SubscriptionsRsp>(responseData);
            }
            catch (Exception e)
            {
                throw ProcessException(url, e);
            }
        }

        /// <summary>创建事件订阅资源.</summary>
        /// <returns>Created</returns>
        /// <exception cref="Exception">A server side error occurred.</exception>
        public async Task<Uri> CreateSubscriptionAsync(CreateSubscriptionBody body)
        {
            var url = $"{this.BaseUrl}/redfish/v1/EventService/Subscriptions";
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(body));
                ProcessRequest(url, body);
                var response = await httpClient.PostAsync(url, content);

                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                this.ProcessResponse(url, response, responseData);
                var resut = JsonConvert.DeserializeObject<CommonResponse>(responseData);
                if (response.Headers.Location == null)
                {
                    throw new Exception(resut.ErrorMessage + " Resolution:" + resut.Resolution);
                }
                return response.Headers.Location;
            }
            catch (Exception e)
            {
                throw ProcessException(url, e);
            }
        }

        /// <summary>查询事件订阅资源.</summary>
        /// <returns>OK</returns>
        /// <exception cref="Exception">A server side error occurred.</exception>
        public async Task<SubscriptionInfo> GetEventServiceSubscriptionAsync(string id)
        {
            var url = $"{this.BaseUrl}/redfish/v1/EventService/Subscriptions/{id}";
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("id");
                }
                var response = await httpClient.GetAsync(url);
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                this.ProcessResponse(url, response, responseData);
                return JsonConvert.DeserializeObject<SubscriptionInfo>(responseData);
            }
            catch (Exception e)
            {
                throw ProcessException(url, e);
            }
        }

        /// <summary>
        /// 删除事件订阅资源.
        /// </summary>
        /// <param name="id">资源 ID.</param>
        /// <returns>OK</returns>
        /// <exception cref="ArgumentNullException">id</exception>
        /// <exception cref="Exception">A server side error occurred.</exception>
        public bool DeleteGivenSubscriptions(string id)
        {
            var url = $"{this.BaseUrl}/{"redfish/v1/EventService/Subscriptions/{id}"}";
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("id");
                }
                url = url.Replace("{id}", Uri.EscapeDataString(id));
                var response = httpClient.DeleteAsync(url).Result;
                var responseData = response.Content.ReadAsStringAsync().Result;
                this.ProcessResponse(url, response, responseData);
                if (!response.IsSuccessStatusCode)
                {
                    var resut = JsonConvert.DeserializeObject<CommonResponse>(responseData);
                    throw new Exception(resut.ErrorMessage + " Resolution:" + resut.Resolution);
                }
                return true;
            }
            catch (Exception e)
            {
                throw ProcessException(url, e);
            }
        }

        /// <summary>
        /// 修改事件订阅资源.
        /// </summary>
        /// <param name="id">资源 ID.</param>
        /// <param name="body">The body.</param>
        /// <returns>OK</returns>
        /// <exception cref="System.ArgumentNullException">id</exception>
        /// <exception cref="Exception">A server side error occurred.</exception>
        public async Task<CommonResponse> PatchSubscriptionsInfoAsync(string id, PatchSubscriptionsInfo body)
        {
            var url = $"{this.BaseUrl}/{"redfish/v1/EventService/Subscriptions/{id}"}";
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("id");
                }
                url = url.Replace("{id}", Uri.EscapeDataString(id));
                using (var request = new HttpRequestMessage())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(body));
                    request.Content = content;
                    request.Method = new HttpMethod("PATCH");

                    var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                    var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    this.ProcessResponse(url, response, responseData);
                    return JsonConvert.DeserializeObject<CommonResponse>(responseData);

                }
            }
            catch (Exception e)
            {
                throw ProcessException(url, e);
            }
        }

        public async Task<EventList> GetEventView(long? top, long? skip)
        {
            string filter = "(EventView='CurrentAlert')and(EventSearch='event_category:BMC','event_category:Enclosure')";
            string order = "(EventOrder='ID asc')";
            return await GetEventListCollectionAsync(top, skip, filter, order);
        }

        /// <summary>查询事件列表.</summary>
        /// <param name="top">$top 查询参数指明返回的集合中应该包含的个数。最小值为1。未提供该选项则返回所有数据。</param>
        /// <param name="skip">$skip 查询参数指明在查询的集合中有多少个需要从头部跳过不被包含在返回中。</param>
        /// <param name="filter">查询中使用的过滤器，例如 $filter=(EventView='CurrentAlert') 。</param>
        /// <param name="orderby">$orderby 查询参数允许客户端指明返回的元素应该依据元素的某属性按升（asc）序或降序（desc）排列。例如 $orderby=Name asc 。</param>
        /// <returns>OK</returns>
        /// <exception cref="Exception">A server side error occurred.</exception>
        public async Task<EventList> GetEventListCollectionAsync(long? top, long? skip, string filter, string orderby)
        {
            var url = $"{this.BaseUrl}/redfish/v1/rich/EventService/Events?";
            try
            {
                if (skip != null)
                {
                    url += $"$skip={Uri.EscapeDataString(skip.Value.ToString())}&";
                }
                if (top != null)
                {
                    url += $"$top={Uri.EscapeDataString(top.Value.ToString())}&";
                }
                if (filter != null)
                {
                    url += $"$filter={Uri.EscapeDataString(filter)}&";
                }
                if (orderby != null)
                {
                    url += $"$orderby={Uri.EscapeDataString(orderby)}&";
                }

                url = url.TrimEnd('?').TrimEnd('&');
                var response = await httpClient.GetAsync(url);
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                this.ProcessResponse(url, response, responseData);
                return JsonConvert.DeserializeObject<EventList>(responseData);
            }
            catch (Exception e)
            {
                throw ProcessException(url, e);
            }
        }

        /// <summary>查询某条事件详细信息.</summary>
        /// <param name="id">事件 ID.</param>
        /// <returns>OK</returns>
        /// <exception cref="Exception">A server side error occurred.</exception>
        public async Task<EventInfo> GetEventsInfoAsync(string id)
        {
            var url = $"{this.BaseUrl}/{"redfish/v1/rich/EventService/Events/{id}"}";
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("id");
                }
                url = url.Replace("{id}", Uri.EscapeDataString(id));
                var response = await httpClient.GetAsync(url);
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<EventInfo>(responseData);
            }
            catch (Exception e)
            {
                throw ProcessException(url, e);
            }
        }

    }
}