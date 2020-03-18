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
// Last Modified By : panwei
// Last Modified On : 12-25-2018
// ***********************************************************************
// <copyright file="SingletonProvider.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace CommonUtil
{
    /// <summary>
    /// The singleton provider.
    /// </summary>
    /// <typeparam name="T">T
    /// </typeparam>
    public class SingletonProvider<T>
        where T : new()
    {
        /// <summary>
        /// The sync object.
        /// </summary>
        private static readonly object SyncObject = new object();

        /// <summary>
        /// The _singleton.
        /// </summary>
        private static T singleton;


        /// <summary>
        /// Prevents a default instance of the <see cref="SingletonProvider{T}"/> class from being created.
        /// </summary>
        private SingletonProvider()
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (singleton == null)
                {
                    lock (SyncObject)
                    {
                        singleton = new T();
                    }
                }
                return singleton;
            }
        }
    }
}