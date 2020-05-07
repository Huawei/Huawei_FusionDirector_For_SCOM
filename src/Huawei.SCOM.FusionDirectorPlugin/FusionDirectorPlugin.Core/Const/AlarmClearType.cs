//**************************************************************************  
//Copyright (C) 2019 Huawei Technologies Co., Ltd. All rights reserved.
//This program is free software; you can redistribute it and/or modify
//it under the terms of the MIT license.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//MIT license for more detail.

namespace Huawei.SCOM.ESightPlugin.Const
{
    /// <summary>
    /// Class ConstMgr.
    /// </summary>
    public partial class Constants
    {
        /// <summary>
        /// The esight event log sources
        /// </summary>
        public class AlarmClearType
        {
            public const string NormalClear = "Normal Cleared.";

            public static string GetClearReason(string clearType)
            {
                switch(clearType)
                {
                    case "NormalClear":
                        return NormalClear;
                    default:
                        return clearType;
                }
            }
        }
    }
}