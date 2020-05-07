using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.Monitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionDirectorPlugin.Core
{
    public class MonitoringDeviceObject
    {
        public string DeviceId { get; set; }

        public MonitoringObject Device { get; set; }

        private ManagementPackClass MpClazz;

        public MonitoringDeviceObject(string deviceId, ManagementPackClass mpClazz, MonitoringObject Device)
        {
            this.DeviceId = deviceId;
            this.Device = Device;
            this.MpClazz = mpClazz;
        }

        public MonitoringDeviceObject Reload()
        {
            MonitoringDeviceObject monitoringDeviceObject = BaseConnector.GetDeviceByObjectId(this.MpClazz, this.DeviceId);
            this.Device = monitoringDeviceObject.Device;
            return this;
        }

    }
}
