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
// <copyright file="Constants.cs" company="mike">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace FusionDirectorPlugin.ViewLib.Model
{
    /// <summary>
    /// Class Constants.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// The e sight connect status.
        /// </summary>
        public class FdConnectionStatus
        {
            /// <summary>
            /// The lates t_ statu s_ disconnect.
            /// </summary>
            public const string DISCONNECT = "DISCONNECT";

            /// <summary>
            /// The lates t_ statu s_ failed.
            /// </summary>
            public const string FAILED = "Offline";

            /// <summary>
            /// The lates t_ statu s_ none.
            /// </summary>
            public const string NONE = "Ready";

            /// <summary>
            /// The lates t_ statu s_ online.
            /// </summary>
            public const string ONLINE = "Online";
        }

        /// <summary>
        /// Class FdSubscriptionStatus.
        /// </summary>
        public class FdSubscriptionStatus
        {
            /// <summary>
            /// The not subscribed
            /// </summary>
            public const string NotSubscribed = "Not subscribed";

            /// <summary>
            /// The failed
            /// </summary>
            public const string Failed = "Error";

            /// <summary>
            /// The success
            /// </summary>
            public const string Success = "Success";
        }
    }
}