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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CommonUtil
{
    public class NullToEmptyStringResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return type.GetProperties()
                .Select(p =>
                {
                    var jp = base.CreateProperty(p, memberSerialization);
                    jp.ValueProvider = new NullToEmptyStringValueProvider(p);
                    return jp;
                }).ToList();
        }
    }

    public class NullToEmptyStringValueProvider : IValueProvider
    {
        private readonly PropertyInfo memberInfo;
        public NullToEmptyStringValueProvider(PropertyInfo memberInfo)
        {
            this.memberInfo = memberInfo;
        }

        public object GetValue(object target)
        {
            object result = memberInfo.GetValue(target);
            if (memberInfo.PropertyType == typeof(string) && result == null)
            {
                result = "";
            }
            return result;
        }

        public void SetValue(object target, object value)
        {
            if (memberInfo.PropertyType == typeof(string) && value == null)
            {
                value = "";
            }
            memberInfo.SetValue(target, value);
        }
    }
}
