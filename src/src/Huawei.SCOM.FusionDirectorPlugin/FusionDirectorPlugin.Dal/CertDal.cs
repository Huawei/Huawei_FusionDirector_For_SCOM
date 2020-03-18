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
// Assembly         : FusionDirectorPlugin.Dal
// Author           : yayun
// Created          : 01-16-2019
//
// Last Modified By : yayun
// Last Modified On : 01-18-2019
// ***********************************************************************
// <copyright file="CertDal.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using CommonUtil;

namespace FusionDirectorPlugin.Dal
{
    /// <summary>
    /// Class CertDal.
    /// </summary>
    public class CertDal
    {
        /// <summary>
        /// The file path
        /// </summary>
        /// <value>The cert folder.</value>
        /// <exception cref="System.Exception"></exception>
        private DirectoryInfo CertFolder
        {
            get
            {
                var env = Environment.GetEnvironmentVariable("FDSCOMPLUGIN");
                if (string.IsNullOrEmpty(env))
                {
                    throw new Exception($"can not find the environment variable : FDSCOMPLUGIN");
                }
                string path = Path.Combine(env, "Cert");
                if (!Directory.Exists(Path.Combine(path)))
                {
                    Directory.CreateDirectory(path);
                }
                return new DirectoryInfo(Path.Combine(path));
            }
        }

        /// <summary>
        /// 单例
        /// </summary>
        /// <value>The instance.</value>
        public static CertDal Instance => SingletonProvider<CertDal>.Instance;

        /// <summary>
        /// Reads the certs.
        /// </summary>
        /// <returns>List&lt;X509Certificate2&gt;.</returns>
        public List<X509Certificate2> GetCerts()
        {
            var result = new List<X509Certificate2>();
            var certs = CertFolder.GetFiles("*.crt");
            foreach (FileInfo file in certs)
            {
                try
                {
                    result.Add(new X509Certificate2(file.FullName));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return result;
        }

        /// <summary>
        /// Saves the cert.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>X509Certificate2.</returns>
        /// <exception cref="System.Exception">The certificate already exists.</exception>
        public X509Certificate2 SaveCert(HttpPostedFile file)
        {
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                var bytes = binaryReader.ReadBytes(file.ContentLength);
                var cert = new X509Certificate2(bytes);
                var certs = GetCerts();
                if (certs.Contains(cert))
                {
                    throw new Exception("The certificate already exists.");
                }
                var savePath = Path.Combine(CertFolder.FullName, DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName));
                file.SaveAs(savePath);
                return cert;
            }
        }
    }
}
