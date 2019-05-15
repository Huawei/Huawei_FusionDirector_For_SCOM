using CommonUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Huawei.SCCMPlugin.Core
{
    public class HttpWhiteListHepler
    {
        public static HttpWhiteListHepler Instance
        {
            get { return SingletonProvider<HttpWhiteListHepler>.Instance; }
        }

        public HttpWhiteListHepler()
        {
            this.WhiteList = new List<UrlInfo>()
            {
                new UrlInfo(){ Url="/rich/ServerProfiles",HttpMethod="GET"},
                new UrlInfo(){ Url="rich/ServerHardwareModels",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/ServerProfiles/Actions/Device.Query",HttpMethod="GET"},
                new UrlInfo(){ Url="rich/DeployModels",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Images",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/DeployService/Actions/QueryOption.Enums",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Tasks",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Groups",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Nodes",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Enclosures",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Appliance/Version",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/NodeGroups",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/EventService/Events",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/UpgradeService/BaseLine",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/UpgradeService/Repository",HttpMethod="GET"},

                new UrlInfo(){ Url="/rich/UpgradeService/UpgradePlan",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/UpgradeService/Actions/UpgradeService.UpgradeableDeviceInfo",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/UpgradeService/DeviceVersion/RackDevices",HttpMethod="GET"},
              



                new UrlInfo(){ Url="/rich/ServerProfiles/[a-zA-Z0-9\\-]+",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/ServerHardwareModels/[a-zA-Z0-9\\-]+",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/DeployModels/[a-zA-Z0-9\\-]+",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Images/[a-zA-Z0-9\\-]+",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Tasks/[a-zA-Z0-9\\-]+",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Nodes/[a-zA-Z0-9\\-]+",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Enclosures/[a-zA-Z0-9\\-]+",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Nodes/[a-zA-Z0-9\\-]+/Processor",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Nodes/[a-zA-Z0-9\\-]+/Memory",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Nodes/[a-zA-Z0-9\\-]+/Power",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Nodes/[a-zA-Z0-9\\-]+/Thermal",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Nodes/[a-zA-Z0-9\\-]+/Storage/Drive",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Nodes/[a-zA-Z0-9\\-]+/Storage/Drive/[a-zA-Z0-9\\-]+",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Nodes/[a-zA-Z0-9\\-]+/NetworkAdapter",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Nodes/[a-zA-Z0-9\\-]+/PCIe",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Nodes/[a-zA-Z0-9\\-]+/Storage/RaidCard",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Nodes/[a-zA-Z0-9\\-]+/Storage/RaidCard/[a-zA-Z0-9\\-]+",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/SwitchNodes/[a-zA-Z0-9\\-]+",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/Nodes/[a-zA-Z0-9\\-]+/Catalogue",HttpMethod="GET"},
                new UrlInfo(){ Url="/rich/UpgradeService/UpgradePlan/[a-zA-Z0-9\\-]+",HttpMethod="GET"},




                new UrlInfo(){ Url="/rich/ServerProfiles/Actions/Device.Query",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/ServerProfiles/Actions/ServerProfile.BatchDelete",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/Nodes/Actions/Profile.Binding",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/Nodes/Actions/Profile.Unbinding",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/ServerProfiles/Actions/ServerProfile.Deploy",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/ServerProfiles",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/DeployModels/Actions/DeployModel.Query",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/DeployService/Actions/QueryOption.OsInfo",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/Images/Actions/Image.Query",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/DeployModels",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/DeployModels/Actions/DeployModel.Deploy",HttpMethod="POST"},
                new UrlInfo(){ Url="rich/Images",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/Nodes/Actions/ComputerSystem.Reset",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/Images/Actions/Image.Import",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/UpgradeService/Repository/Check",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/UpgradeService/Action/ImportFile",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/UpgradeService/BaseLine",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/UpgradeService/Actions/Package.Import",HttpMethod="POST"},

                new UrlInfo(){ Url="/rich/UpgradeService/UpgradePlan",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/UpgradeService/UpgradePlan/CheckName",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/UpgradeService/UpgradePlan/Actions/UpgradePlan.CheckGroup",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/UpgradeService/Actions/UpgradeService.UpgradeableDeviceInfo",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/UpgradeService/DeviceVersion/Actions/UpgradeService.ActiveFireware",HttpMethod="POST"},
                new UrlInfo(){ Url="/rich/UpgradeService/UpgradePlan/[a-zA-Z0-9\\-]+/Actions/UpgradePlan.Enable",HttpMethod="POST"},


                new UrlInfo(){ Url="/rich/ServerProfiles/[a-zA-Z0-9\\-]+",HttpMethod="DELETE"},
                new UrlInfo(){ Url="/rich/DeployModels/[a-zA-Z0-9\\-]+",HttpMethod="DELETE"},
                new UrlInfo(){ Url="/rich/Images/[a-zA-Z0-9\\-]+",HttpMethod="DELETE"},
                new UrlInfo(){ Url="/rich/Tasks/[a-zA-Z0-9\\-]+",HttpMethod="DELETE"},
                new UrlInfo(){ Url="/rich/UpgradeService/BaseLine/[a-zA-Z0-9\\-]+",HttpMethod="DELETE"},
                new UrlInfo(){ Url="/rich/UpgradeService/Repository/[a-zA-Z0-9\\-]+",HttpMethod="DELETE"},
                new UrlInfo(){ Url="/rich/UpgradeService/UpgradePlan/[a-zA-Z0-9\\-]+",HttpMethod="DELETE"},

                new UrlInfo(){ Url="/rich/UpgradeService/Network",HttpMethod="PATCH"},
                new UrlInfo(){ Url="/rich/ServerProfiles/[a-zA-Z0-9\\-]+",HttpMethod="PATCH"},
                new UrlInfo(){ Url="/rich/DeployModels/[a-zA-Z0-9\\-]+",HttpMethod="PATCH"},
                new UrlInfo(){ Url="/rich/Images/[a-zA-Z0-9\\-]+",HttpMethod="PATCH"},
                new UrlInfo(){ Url="/rich/UpgradeService/BaseLine/[a-zA-Z0-9\\-]+",HttpMethod="PATCH"},
                 new UrlInfo(){ Url="/rich/UpgradeService/UpgradePlan/[a-zA-Z0-9\\-]+",HttpMethod="PATCH"},
                //new UrlInfo(){ Url="",HttpMethod="POST"},
                //new UrlInfo(){ Url="",HttpMethod="POST"},
                //new UrlInfo(){ Url="",HttpMethod="POST"},
                //new UrlInfo(){ Url="",HttpMethod="POST"},
                //new UrlInfo(){ Url="",HttpMethod="POST"},
                //new UrlInfo(){ Url="",HttpMethod="POST"},
                //new UrlInfo(){ Url="",HttpMethod="POST"},
                //new UrlInfo(){ Url="",HttpMethod="POST"},
                //new UrlInfo(){ Url="",HttpMethod="POST"},
                //new UrlInfo(){ Url="",HttpMethod="POST"},
                //new UrlInfo(){ Url="",HttpMethod="POST"},
                //new UrlInfo(){ Url="",HttpMethod="POST"},
                //new UrlInfo(){ Url="",HttpMethod="POST"},
                //new UrlInfo(){ Url="",HttpMethod="POST"},

            };
        }
        public List<UrlInfo> WhiteList { get; set; }

        public bool Validate(string url, string httpMethod)
        {
            foreach (var x in WhiteList)
            {
                var regexStr = x.Url;
                if (url.Contains("?"))
                {
                    url = url.Substring(0, url.IndexOf("?"));
                }
                if (x.HttpMethod == httpMethod.ToUpper() && Regex.IsMatch(url, regexStr))
                {
                    return true ;
                }
            }
           return false;
        }

    }

    public class UrlInfo
    {
        public string Url { get; set; }

        public string HttpMethod { get; set; }

    }


}
