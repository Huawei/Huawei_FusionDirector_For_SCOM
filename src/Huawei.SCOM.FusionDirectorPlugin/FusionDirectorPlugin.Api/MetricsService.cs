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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionDirectorPlugin.Dal.Model;
using FusionDirectorPlugin.Model;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Api
{
    public class MetricsService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnclosureService" /> class.
        /// </summary>
        /// <param name="fusionDirector">The fusion director.</param>
        public MetricsService(FusionDirector fusionDirector) : base(fusionDirector)
        {
        }

        /// <summary>
        /// 返回指定设备的实时性能数据。
        /// </summary>
        /// <param name="serverID">机框的ID。</param>
        /// <returns>成功。</returns>
        public async Task<ServerRealTimePerformance> GetServerRealTimePerformanceAsync(string serverID)
        {
            var url = $"{BaseUrl}/redfish/v1/rich/Statistics/{serverID}/RealTime";
            try
            {
                var response = await httpClient.GetAsync(url);
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                this.ProcessResponse(url, response, responseData);
                return JsonConvert.DeserializeObject<ServerRealTimePerformance>(responseData);
            }
            catch (Exception e)
            {
                throw ProcessException(url, e);
            }
        }
    }
}
