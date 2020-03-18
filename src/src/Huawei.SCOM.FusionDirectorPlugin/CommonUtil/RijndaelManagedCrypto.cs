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
// Assembly         : FusionDirectorPlugin.ViewLib
// Author           : mike
// Created          : 05-05-2019
//
// Last Modified By : mike
// Last Modified On : 05-05-2019
// ***********************************************************************
// <copyright file="RijndaelManagedCrypto.cs" company="mike">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CommonUtil
{
    /// <summary>
    /// Class RijndaelManagedCrypto.
    /// </summary>
    public class RijndaelManagedCrypto
    {
        /// <summary>
        /// The key
        /// </summary>
        private static readonly string Key = "668DAFB758034A97";

        /// <summary>
        /// The cs key
        /// </summary>
        private static readonly byte[] CsKey = Encoding.ASCII.GetBytes(Key);

        /// <summary>
        /// The cs initialize vector
        /// </summary>
        private static readonly byte[] CsInitVector = new byte[16];

        /// <summary>
        /// The synchronize object
        /// </summary>
        private static readonly object SyncObject = new object();

        /// <summary>
        /// The instance
        /// </summary>
        private static RijndaelManagedCrypto _instance = null;


        /// <summary>
        /// The encryptor
        /// </summary>
        private ICryptoTransform encryptor;

        /// <summary>
        /// The decryptor
        /// </summary>
        private ICryptoTransform decryptor;

        /// <summary>
        /// The ut f8 encoder
        /// </summary>
        private UTF8Encoding utf8Encoder;

        /// <summary>
        /// Initializes a new instance of the <see cref="RijndaelManagedCrypto"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="iv">The iv.</param>
        public RijndaelManagedCrypto(byte[] key, byte[] iv)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            this.encryptor = rijndaelManaged.CreateEncryptor(key, iv);
            this.decryptor = rijndaelManaged.CreateDecryptor(key, iv);
            this.utf8Encoder = new UTF8Encoding();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RijndaelManagedCrypto"/> class.
        /// </summary>
        public RijndaelManagedCrypto() : this(CsKey, CsInitVector)
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static RijndaelManagedCrypto Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new RijndaelManagedCrypto();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Encrypts the specified unencrypted.
        /// </summary>
        /// <param name="unencrypted">The unencrypted.</param>
        /// <returns>System.String.</returns>
        public string Encrypt(string unencrypted)
        {
            return Convert.ToBase64String(this.Encrypt(this.utf8Encoder.GetBytes(unencrypted)));
        }

        /// <summary>
        /// Encrypts the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] Encrypt(byte[] buffer)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, this.encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(buffer, 0, buffer.Length);
            }
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Decrypts the specified encrypted.
        /// </summary>
        /// <param name="encrypted">The encrypted.</param>
        /// <returns>System.String.</returns>
        public string Decrypt(string encrypted)
        {
            return this.utf8Encoder.GetString(this.Decrypt(Convert.FromBase64String(encrypted)));
        }

        /// <summary>
        /// Decrypts the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] Decrypt(byte[] buffer)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, this.decryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(buffer, 0, buffer.Length);
            }
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Encrypts for cs.
        /// </summary>
        /// <param name="unencrypted">The unencrypted.</param>
        /// <returns>System.String.</returns>
        public string EncryptForCs(string unencrypted)
        {
            byte[] array = new byte[8];
            RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider();
            rngcryptoServiceProvider.GetBytes(array);
            string str = string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}{6:x2}{7:x2}", array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7]);
            return this.Encrypt(str + unencrypted);
        }

        /// <summary>
        /// Decrypts from cs.
        /// </summary>
        /// <param name="encrypted">The encrypted.</param>
        /// <returns>System.String.</returns>
        public string DecryptFromCs(string encrypted)
        {
            return this.Decrypt(encrypted).Substring(16);
        }



    }
}
