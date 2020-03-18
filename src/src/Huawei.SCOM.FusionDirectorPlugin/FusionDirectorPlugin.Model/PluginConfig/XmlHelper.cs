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
// Assembly         : CommonUtil
// Author           : panwei
// Created          : 12-25-2018
//
// Last Modified By : mike
// Last Modified On : 12-25-2018
// ***********************************************************************
// <copyright file="XmlHelper.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FusionDirectorPlugin.PluginConfigs.Helpers
{
    /// <summary>
    /// Class XmlHelper.
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        /// <returns>System.Object.</returns>
        public static object Load(Type type, string filename)
        {
            using (FileStream fs = new FileStream(Path.Combine(filename), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void Save(object obj, string filename)
        {
            using (FileStream fs = new FileStream(Path.Combine(filename), FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
        }

        /// <summary>
        /// 使用XmlSerializer序列化对象
        /// </summary>
        /// <typeparam name="T">需要序列化的对象类型，必须声明[Serializable]特征</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="omitXmlDeclaration">true:省略XML声明;否则为false.默认false，即编写 XML 声明。</param>
        /// <returns>序列化后的字符串</returns>
        public static string SerializeToXmlStr<T>(T obj, bool omitXmlDeclaration)
        {
            var xmlSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = omitXmlDeclaration,
                Encoding = new UTF8Encoding(false)
            };
            using (var stream = new MemoryStream())
            {
                var xmlwriter = XmlWriter.Create(stream, xmlSettings);

                var xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add(string.Empty, string.Empty); //在XML序列化时去除默认命名空间xmlns:xsd和xmlns:xsi
                var xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(xmlwriter, obj, xmlSerializerNamespaces);

                return Encoding.UTF8.GetString(stream.ToArray());
            }

        }
    }
}