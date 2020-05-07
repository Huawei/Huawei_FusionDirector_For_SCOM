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
// Assembly         : FusionDirectorPlugin.Model
// Author           : yayun
// Created          : 02-12-2019
//
// Last Modified By : yayun
// Last Modified On : 02-12-2019
// ***********************************************************************
// <copyright file="JObjectExt.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class JObjectExt.
    /// </summary>
    public static class JObjectExt
    {
        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="path">The path.</param>
        /// <returns>System.String.</returns>
        public static string GetString(this JObject obj, string path)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            var val = obj.SelectToken(path);
            if (val == null)
            {
                return string.Empty;
            }

            var t = val.GetType();
            if (t.Name == "JArray")
            {
                return string.Join(",", val.ToList().Select(x => x.ToString()));
            }
            if (t.Name == "JValue")
            {
                return val.ToString();
            }
            return val.ToString();
        }

        public static string GetString(this JObject obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
           
            //var t = obj.GetType();
            //if (t.Name == "JArray")
            //{
            //    return string.Join(",", obj.ToList().Select(x => x.ToString()));
            //}
            //if (t.Name == "JValue")
            //{
            //    return obj.ToString();
            //}
            return obj.ToString();
        }
    }
}
