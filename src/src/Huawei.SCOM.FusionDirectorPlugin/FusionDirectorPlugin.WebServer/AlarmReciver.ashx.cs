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
// Assembly         : FusionDirectorPlugin.WebServer
// Author           : yayun
// Created          : 01-21-2019
//
// Last Modified By : yayun
// Last Modified On : 01-21-2019
// ***********************************************************************
// <copyright file="AlarmReciver.ashx.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using FusionDirectorPlugin.LogUtil;
using FusionDirectorPlugin.Model.Event;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FusionDirectorPlugin.Model;

namespace FusionDirectorPlugin.WebServer
{
    /// <summary>
    /// AlarmReciver 的摘要说明
    /// </summary>
    /// <seealso cref="System.Web.IHttpHandler" />
    public class AlarmReciver : IHttpHandler
    {

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        /// <exception cref="System.Exception"></exception>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            context.Response.ContentType = "text/plain";
            var url = context.Request.Url;
            try
            {
                var authorization = context.Request.Headers["Authorization"];

                JsonReader reader = new JsonTextReader(new StreamReader(context.Request.InputStream));
                var obj = JObject.Load(reader);
                var pushData = obj.ToObject<PushData>();

                var alarmData = pushData.Data;
                alarmData.Status = alarmData.Category == "1" ? "Uncleared" : "Cleared";
               
                var message = new TcpMessage<AlarmData>(authorization, TcpMessageType.Alarm, alarmData);
                NotifyClient.Instance.SendMsg(message);
                context.Response.Write($"success");
            }
            catch (Exception ex)
            {
                HWLogger.NotifyRecv.Error(ex, $"Alarm Notification Error.[{url}]");
                context.Response.Write($"Alarm Notification Error: { ex }");
            }
            context.Response.End();
        }
        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
        public bool IsReusable => false;
    }
}