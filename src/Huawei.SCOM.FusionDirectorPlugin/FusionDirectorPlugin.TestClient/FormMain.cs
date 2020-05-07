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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FusionDirectorPlugin.Model;
using Newtonsoft.Json;
using FusionDirectorPlugin.Core;
using FusionDirectorPlugin.Core.Model;
using FusionDirectorPlugin.Core.Models;
using FusionDirectorPlugin.Model.Event;
using FusionDirectorPlugin.Service;
using System.Net.Http;
using FusionDirectorPlugin.Dal;
using FusionDirectorPlugin.Api;
using System.Net;
using System.Text.RegularExpressions;
using CommonUtil;

namespace FusionDirectorPlugin.TestClient
{
    public partial class FormMain : Form
    {
        private FusionDirectorPluginService service;

        private readonly TaskFactory taskFactory;

        private CancellationTokenSource cts = new CancellationTokenSource();
        private NodePoolService nodePoolService;
        private EnclosureService enclosureService;
        private MetricsService metricsService;

        public string FusionDirectorIp => "192.168.8.12";

        public FormMain()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new NullToEmptyStringResolver(),
                TypeNameHandling = TypeNameHandling.All,
            };
            ServicePointManager.DefaultConnectionLimit = 3;
            InitializeComponent();
            var scheduler = new LimitedConcurrencyLevelTaskScheduler(1);
            taskFactory = new TaskFactory(cts.Token, TaskCreationOptions.HideScheduler, TaskContinuationOptions.HideScheduler, scheduler);
            //taskFactory = new TaskFactory(scheduler);
            var fusionDirector = FusionDirectorDal.Instance.GetList().First();
            nodePoolService = new NodePoolService(fusionDirector);
            enclosureService = new EnclosureService(fusionDirector);
            metricsService = new MetricsService(fusionDirector);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            service.Debug();
        }

        private void btnInsertEnclosure_Click(object sender, EventArgs e)
        {
            #region MyRegion
            var t = @"{
	'@odata.id': '/redfish/v1/rich/Enclosures/0242f28b-6471-5ae6-a274-9330a8221b34',
	'Name': 'E9000',
	'Type': 'E9000',
	'CreatedAt': '2018-12-12T23:44:04+08:00',
	'UpdatedAt': '2018-12-12T23:49:09+08:00',
	'Description': '',
	'Hostname': '172.180.10.31',
	'Port': 443,
	'Protocol': 'https',
	'ChassisID': '934506',
	'ChassisType': 'Other',
	'SerialNumber': 'EEM0fdFo7x2cS7WHGy4Gs1WpgW5FGJQ4',
	'PartNumber': '8gCTa3r4lSuyFtJqVKPy39',
	'BoardFru': {
		'PartNumber': '03021RMJ',
		'SerialNumber': 'kjFa6bUZ7D4844ZdHhb97YUw01FlHy',
		'ManufacturingDate': '2014/06/29 Sun 23:00:00',
		'Manufacturer': 'Huawei',
		'ProductName': 'IT11BKPA'
	},
	'UIDState': 'Off',
	'State': 'Ready',
	'StateReason': 'AutoChange',
	'Health': 'Critical',
	'LCDVersion': '(J7)00',
	'EnclosureProfileUsageState': 'Inconsistent',
	'EnclosureProfileURL': '/redfish/v1/rich/EnclosureProfiles/67a0e4d0-c784-48ee-b3f4-bfb60844a066',
	'EnclosureSetting': {
		'PSUSetting': {
			'Mode': null,
			'EnableSleep': false
		},
		'PowerCapping': {
			'Enable': true,
			'LimitInWatts': 12000,
			'PowerLimitMode': 'Manual',
			'PowerLimitThreshold': 55,
			'MinimumValue': 2684,
			'RecommendMinimumValue': 3032,
			'MaximumValue': 5000,
			'PowerLimitActivated': false,
			'ServerPowerLimit': [{
					'Slot': 4,
					'Value': 900
				},
				{
					'Slot': 5,
					'Value': 210
				}
			]
		},
		'FanSetting': {
			'FanSpeedAdjustmentMode': 'Automatic',
			'FanSmartCoolingMode': 'EnergySaving'
		},
		'ManagementNetworkSetting': {
			'Mode': 'Manual',
			'BMCIPPoolURL': null,
			'ServiceIPPoolURL': '/redfish/v1/rich/IDPoolService/IPv4Pools/d04cc0d8-722e-4ec3-aa2c-a437d3e24d55'
		}
	},
	'EnclosureHardwareCapabilityURL': '/redfish/v1/rich/EnclosureHardwareCapabilities/E9000',
	'Slot': {
		'ServerSlot': [{
				'Index': 1,
				'State': 'Absent',
				'ProductName': '',
				'PhysicalUUID': '',
				'SerialNumber': '',
				'ResourceURL': ''
			},
			{
				'Index': 2,
				'State': 'Enabled',
				'ProductName': 'CH242 V5',
				'PhysicalUUID': '08070920-2017-A9FE-B211-D21D042E9B34',
				'SerialNumber': '210305613210FC012425',
				'Height': 2,
				'Width': 2,
				'ResourceURL': '/redfish/v1/rich/Nodes/96c4b8d2-c68b-48cd-bafc-faa3a3dfd3cf'
			}
		],
		'SwitchSlot': [{
				'Index': 1,
				'State': 'Absent',
				'ProductName': '',
				'PhysicalUUID': '',
				'SerialNumber': '',
				'ResourceURL': ''
			},
			{
				'Index': 2,
				'State': 'Enabled',
				'ProductName': 'CX920',
				'PhysicalUUID': '26A70746-1DD2-11B2-8D09-001823E5F68B',
				'SerialNumber': '210305723610H2000106',
				'ResourceURL': '/redfish/v1/rich/SwitchNodes/02dc6ca8-0c8d-4794-8a6e-8561ca092e59'
			}
		],
		'ManagerSlot': [{
				'Index': 1,
				'Name': 'HMM1',
				'State': 'Enabled',
				'ProductName': 'MM920',
				'SerialNumber': '',
				'PhysicalUUID': 'B4F2ACA6-C4B8-82B9-B211-D21D8856E632',
				'FirmwareVersion': '3.28',
				'CPLDVersion': '0.04',
				'Health': 'OK',
				'StaticIPv4Address': {
					'Address': '172.180.10.32',
					'SubnetMask': '255.255.0.0',
					'Gateway': '172.180.0.1',
					'AddressOrigin': 'Static'
				},
				'FloatIPv4Address': {
					'Address': '172.180.10.31',
					'SubnetMask': '255.255.0.0',
					'Gateway': '172.180.0.1',
					'AddressOrigin': 'Static'
				},
				'ApplianceIPv4Address': {
					'Address': '172.180.2.200',
					'SubnetMask': '255.255.0.0',
					'Gateway': '172.180.0.1',
					'AddressOrigin': 'Static'
				},
				'ApplianceFirmwareVersion': '',
				'ApplianceBIOSVersion': ''
			},
			{
				'Index': 2,
				'Name': 'HMM2',
				'State': 'StandbySpare',
				'ProductName': 'MM920',
				'SerialNumber': '',
				'PhysicalUUID': 'B4F2ACC4-C4B8-BB6A-B211-D21DECA8BA2F',
				'FirmwareVersion': '3.28',
				'CPLDVersion': '0.04',
				'Health': 'OK',
				'StaticIPv4Address': {
					'Address': '172.180.10.30',
					'SubnetMask': '255.255.0.0',
					'Gateway': '172.180.0.1',
					'AddressOrigin': 'Static'
				},
				'ApplianceIPv4Address': {
					'Address': '172.180.2.149',
					'SubnetMask': '255.255.0.0',
					'Gateway': '172.180.0.1',
					'AddressOrigin': 'Static'
				},
				'ApplianceFirmwareVersion': '',
				'ApplianceBIOSVersion': ''
			}
		],
		'FanSlot': [{
				'Index': 1,
				'Name': 'Fan1',
				'State': 'Enabled',
				'PcbVersion': 'D',
				'SoftwareVersion': '200',
				'Health': 'OK'
			},
			{
				'Index': 2,
				'Name': 'Fan2',
				'State': 'Enabled',
				'PcbVersion': 'D',
				'SoftwareVersion': '200',
				'Health': ''
			},
			{
				'Index': 7,
				'Name': 'Fan7',
				'State': 'Absent',
				'PcbVersion': '',
				'SoftwareVersion': '',
				'Health': 'OK'
			}
		],
		'PowerSlot': [{
				'Index': 1,
				'Name': 'PS1',
				'State': 'Enabled',
				'PowerSupplyType': 'DC',
				'FirmwareVersion': '105',
				'HardwareVersion': 'B',
				'SleepStatus': '',
				'Health': 'OK',
				'SerialNumber': '2102310LHNLUH3000848',
				'PowerCapacityWatts': '2500'
			},
			{
				'Index': 6,
				'Name': 'PS6',
				'State': 'Absent',
				'PowerSupplyType': '',
				'FirmwareVersion': '',
				'HardwareVersion': '',
				'SleepStatus': '',
				'Health': 'OK',
				'SerialNumber': '',
				'PowerCapacityWatts': ''
			}
		]
	}
}";
            #endregion

            var enclosure = JsonConvert.DeserializeObject<Enclosure>(t);
            enclosure.MakeDetails("192.168.1.1");
            EnclosureConnector.Instance.Sync(enclosure);
        }

        private void btnGenCode_Click(object sender, EventArgs e)
        {
            new FormGen().Show();
        }

        private void btnInsertServerEvent_Click(object sender, EventArgs e)
        {
            var t = @"{
                 'data': {
                  'alarmid': '2C00000D',
                  'alarmname': 'alarmname',
                  'resourceid': 'resourceid',
                  'resourceidname': 'resourceidname',
                  'moc': 'AtlasDirector',
                  'sn': '78998',
                  'category': '1',
                  'severity': '2',
                  'occurtime': '2012-02-03 00:00:00',
                  'cleartime': 'cleartime',
                  'cleartype': 'cleartype',
                  'isclear': '0',
                  'additional': 'CloudService=Server,Service=Server,NativeMeDn=9.88.19.13',
                  'cause': 'cause.',
                  'deviceid': '405758cf-6dae-4be6-876c-c40c204499ee',
                  'eventcategory': 'BMC',
                  'eventsubject': 'eventsubject',
                  'eventdescriptionargs': ['Disk7'],
                  'possiblecause': 'possiblecause',
                  'suggstion': 'suggstion.',
                  'effect': 'effect',
                  'status': 'Cleared',
                 }
              }";

            var pushData = JsonConvert.DeserializeObject<PushData>(t);
            var alarmData = new EventData(pushData.Data, "192.168.129.30");
            ServerConnector.Instance.InsertEvent(alarmData);
            //service.TestUpdateTask(alarmData.AlarmData);
        }

        private void btnInsertEnclosureEvent_Click(object sender, EventArgs e)
        {
            var t = @"{
                 'data': {
                  'alarmid': '2C00000D',
                  'alarmname': 'alarmname',
                  'resourceid': 'resourceid',
                  'resourceidname': 'resourceidname',
                  'moc': 'AtlasDirector',
                  'sn': '1122',
                  'category': '1',
                  'severity': '2',
                  'occurtime': '2012-02-03 00:00:00',
                  'cleartime': 'cleartime',
                  'cleartype': 'cleartype',
                  'isclear': '0',
                  'additional': 'CloudService=Server,Service=Server,NativeMeDn=9.88.19.13',
                  'cause': 'cause.',
                  'deviceid': '0242f28b-6471-5ae6-a274-9330a8221b34',
                  'eventcategory': 'Enclosure',
                  'eventsubject': 'eventsubject',
                  'eventdescriptionargs': ['Disk7'],
                  'possiblecause': 'possiblecause',
                  'suggstion': 'suggstion.',
                  'effect': 'effect'
                 }
              }";
            var pushData = JsonConvert.DeserializeObject<PushData>(t);
            var alarmData = new EventData(pushData.Data, "192.168.8.12");
            EnclosureConnector.Instance.InsertEvent(alarmData);
        }

        private void btnInsertSwitchEvent_Click(object sender, EventArgs e)
        {
            var t = @"{
                 'data': {
                  'alarmid': '2C00000D',
                  'alarmname': 'alarmname',
                  'resourceid': 'resourceid',
                  'resourceidname': 'resourceidname',
                  'moc': 'AtlasDirector',
                  'sn': '456',
                  'category': '1',
                  'severity': '2',
                  'occurtime': '2012-02-03 00:00:00',
                  'cleartime': 'cleartime',
                  'cleartype': 'cleartype',
                  'isclear': '0',
                  'additional': 'CloudService=Server,Service=Server,NativeMeDn=9.88.19.13',
                  'cause': 'cause.',
                  'deviceid': '02dc6ca8-0c8d-4794-8a6e-8561ca092e59',
                  'eventcategory': 'Switch',
                  'eventsubject': 'eventsubject',
                  'eventdescriptionargs': ['Disk7'],
                  'possiblecause': 'possiblecause',
                  'suggstion': 'suggstion.',
                  'effect': 'effect'
                 }
              }";
            var pushData = JsonConvert.DeserializeObject<PushData>(t);
            var alarmData = new EventData(pushData.Data, "192.168.8.12");
            service.TestUpdateTask(alarmData.AlarmData);
            //EnclosureConnector.Instance.InsertSwitchEvent(alarmData);
        }

        private void btnInsertServer_Click(object sender, EventArgs e)
        {
            #region MyRegion
            var t = @" {
  'UUID': '9DF94E9F-60FA-A642-E811-938B6AF01081',
  '@odata.context': '/redfish/v1/rich/$metadata#NodePoolService.NodePoolService',
  '@odata.id': '/redfish/v1/rich/Nodes/{serverGuid}',
  '@odata.type': '#NodePoolService.v1_2_0.NodeInfo',
  'DeviceID': '{serverGuid}',
  'Id': '{serverGuid}',
  'Name': 'Computer System',
  'Model': 'CH242 V5',
  'Manufacturer': 'Huawei',
  'SerialNumber': '210305774210J7000006',
  'Slot': '',
  'Alias': '',
  'Tag': '',
  'AssetTag': '',
  'Group': null,
  'IPv4Address': {
    'Address': '192.168.129.120',
    'AddressOrigin': '',
    'GateWay': '',
    'SubnetMask': '',
    'PrefixLength': null
  },
  'IPAddress': '192.168.129.120',
  'BiosVersion': '0.95',
  'PowerState': 'On',
  'ServerState': 'OnLine',
  'Processors': [{
    'DeviceID': 'c8b7cc03-6ced-4001-b952-1a24f0820280',
    'Name': 'CPU1',
    'ProcessorArchitecture': 'x86',
    'InstructionSet': 'x86-64',
    'Manufacturer': 'Intel(R) Corporation',
    'Model': 'Intel(R) Xeon(R) Gold 5115 CPU @ 2.40GHz',
    'MaxSpeedMHz': 4000.0,
    'Socket': 0.0,
    'TotalCores': 10.0,
    'TotalThreads': 20.0,
    'Oem': {
      'Huawei': {
        'L1CacheKiB': 640,
        'L2CacheKiB': 10240,
        'L3CacheKiB': 14080,
        'Position': 'mainboard',
        'Temperature': 42,
        'FrequencyMHz': 2400,
        'PartNumber': '41020675'
      }
    },
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }, {
    'UnionId': '192.168.129.30-1c296d11-7ae3-484f-921d-3f430a1e2352',
    'FusionDirectorIp': '192.168.129.30',
    'DeviceID': '1c296d11-7ae3-484f-921d-3f430a1e2352',
    'Name': 'CPU2',
    'ProcessorArchitecture': 'x86',
    'InstructionSet': 'x86-64',
    'Manufacturer': 'Intel(R) Corporation',
    'Model': 'Intel(R) Xeon(R) Gold 5115 CPU @ 2.40GHz',
    'MaxSpeedMHz': 4000.0,
    'Socket': 1.0,
    'TotalCores': 10.0,
    'TotalThreads': 20.0,
    'Oem': {
      'Huawei': {
        'L1CacheKiB': 640,
        'L2CacheKiB': 10240,
        'L3CacheKiB': 14080,
        'Position': 'mainboard',
        'Temperature': 38,
        'FrequencyMHz': 2400,
        'PartNumber': '41020675'
      }
    },
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }, {
    'UnionId': '192.168.129.30-301b2758-9999-4c3f-b055-4a4c3f1c23aa',
    'FusionDirectorIp': '192.168.129.30',
    'DeviceID': '301b2758-9999-4c3f-b055-4a4c3f1c23aa',
    'Name': 'CPU3',
    'ProcessorArchitecture': 'x86',
    'InstructionSet': 'x86-64',
    'Manufacturer': 'Intel(R) Corporation',
    'Model': 'Intel(R) Xeon(R) Gold 5115 CPU @ 2.40GHz',
    'MaxSpeedMHz': 4000.0,
    'Socket': 2.0,
    'TotalCores': 10.0,
    'TotalThreads': 20.0,
    'Oem': {
      'Huawei': {
        'L1CacheKiB': 640,
        'L2CacheKiB': 10240,
        'L3CacheKiB': 14080,
        'Position': 'mainboard',
        'Temperature': 40,
        'FrequencyMHz': 2400,
        'PartNumber': '41020675'
      }
    },
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }, {
    'UnionId': '192.168.129.30-bada5611-03e0-4ebf-8807-40b2852388d7',
    'FusionDirectorIp': '192.168.129.30',
    'DeviceID': 'bada5611-03e0-4ebf-8807-40b2852388d7',
    'Name': 'CPU4',
    'ProcessorArchitecture': 'x86',
    'InstructionSet': 'x86-64',
    'Manufacturer': 'Intel(R) Corporation',
    'Model': 'Intel(R) Xeon(R) Gold 5115 CPU @ 2.40GHz',
    'MaxSpeedMHz': 4000.0,
    'Socket': 3.0,
    'TotalCores': 10.0,
    'TotalThreads': 20.0,
    'Oem': {
      'Huawei': {
        'L1CacheKiB': 640,
        'L2CacheKiB': 10240,
        'L3CacheKiB': 14080,
        'Position': 'mainboard',
        'Temperature': 40,
        'FrequencyMHz': 2400,
        'PartNumber': '41020675'
      }
    },
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }],
  'Memorys': [{
    'DeviceID': '56986c5f-8f99-4528-80b7-f3506f653607',
    'Name': 'mainboardDIMM000',
    'CapacityGiB': 8.0,
    'Manufacturer': 'Micron',
    'OperatingSpeedMhz': 2400.0,
    'SerialNumber': '1CB72EDF',
    'MemoryDeviceType': 'DDR4',
    'DataWidthBits': 72.0,
    'Slot': 0.0,
    'Oem': {
      'Huawei': {
        'Technology': 'Synchronous| Registered (Buffered)',
        'Position': 'mainboard',
        'MinVoltageMillivolt': 1200
      }
    },
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }, {
    'UnionId': '192.168.129.30-17d3fd39-209b-48ad-961c-e09e201b206b',
    'FusionDirectorIp': '192.168.129.30',
    'DeviceID': '17d3fd39-209b-48ad-961c-e09e201b206b',
    'Name': 'mainboardDIMM100',
    'CapacityGiB': 8.0,
    'Manufacturer': 'Micron',
    'OperatingSpeedMhz': 2400.0,
    'SerialNumber': '1CB73264',
    'MemoryDeviceType': 'DDR4',
    'DataWidthBits': 72.0,
    'Slot': 0.0,
    'Oem': {
      'Huawei': {
        'Technology': 'Synchronous| Registered (Buffered)',
        'Position': 'mainboard',
        'MinVoltageMillivolt': 1200
      }
    },
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }, {
    'UnionId': '192.168.129.30-d80b75b8-f130-4804-a67e-959630866efa',
    'FusionDirectorIp': '192.168.129.30',
    'DeviceID': 'd80b75b8-f130-4804-a67e-959630866efa',
    'Name': 'mainboardDIMM200',
    'CapacityGiB': 8.0,
    'Manufacturer': 'Micron',
    'OperatingSpeedMhz': 2400.0,
    'SerialNumber': '1CB74051',
    'MemoryDeviceType': 'DDR4',
    'DataWidthBits': 72.0,
    'Slot': 0.0,
    'Oem': {
      'Huawei': {
        'Technology': 'Synchronous| Registered (Buffered)',
        'Position': 'mainboard',
        'MinVoltageMillivolt': 1200
      }
    },
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }, {
    'UnionId': '192.168.129.30-e586de70-a567-49bf-92b6-5952ac9d24a0',
    'FusionDirectorIp': '192.168.129.30',
    'DeviceID': 'e586de70-a567-49bf-92b6-5952ac9d24a0',
    'Name': 'mainboardDIMM300',
    'CapacityGiB': 8.0,
    'Manufacturer': 'Micron',
    'OperatingSpeedMhz': 2400.0,
    'SerialNumber': '1CB72EC7',
    'MemoryDeviceType': 'DDR4',
    'DataWidthBits': 72.0,
    'Slot': 0.0,
    'Oem': {
      'Huawei': {
        'Technology': 'Synchronous| Registered (Buffered)',
        'Position': 'mainboard',
        'MinVoltageMillivolt': 1200
      }
    },
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }],
  'NetworkAdapters': [{
    'Id': 'mainboardLOM',
    '@odata.id': '/redfish/v1/rich/Nodes/{serverGuid}/NetworkAdapter/370f2fba-4b3b-405a-ab84-9dad879111bb',
    'DeviceID': '370f2fba-4b3b-405a-ab84-9dad879111bb',
    'Name': 'mainboardLOM',
    'DeviceLocator': 'LOM',
    'DriverName': '',
    'DriverVersion': '',
    'Manufacturer': 'Intel',
    'Model': 'X722',
    'CardName': 'LOM',
    'CardManufacturer': 'Huawei',
    'CardModel': '2*10GE',
    'Position': 'mainboard',
    'NetworkPort': {
      '@odata.id': '/redfish/v1/rich/Nodes/{serverGuid}/NetworkAdapter/370f2fba-4b3b-405a-ab84-9dad879111bb/NetworkPort',
      'DeviceID': null,
      'Id': null,
      'Name': null
    },
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }, {
    'UnionId': '192.168.129.30-f5241c52-75de-44ef-805e-c2e272a7bdf7',
    'FusionDirectorIp': '192.168.129.30',
    'Id': 'mainboardMEZZ1',
    '@odata.id': '/redfish/v1/rich/Nodes/{serverGuid}/NetworkAdapter/f5241c52-75de-44ef-805e-c2e272a7bdf7',
    'DeviceID': 'f5241c52-75de-44ef-805e-c2e272a7bdf7',
    'Name': 'mainboardMEZZ1',
    'DeviceLocator': 'MEZZ1',
    'DriverName': '',
    'DriverVersion': '',
    'Manufacturer': 'Broadcom',
    'Model': 'XE201',
    'CardName': 'MZ220',
    'CardManufacturer': 'Huawei',
    'CardModel': '2*16G FC Port Mezzanine Card',
    'Position': 'mainboard',
    'NetworkPort': {
      '@odata.id': '/redfish/v1/rich/Nodes/{serverGuid}/NetworkAdapter/f5241c52-75de-44ef-805e-c2e272a7bdf7/NetworkPort',
      'DeviceID': null,
      'Id': null,
      'Name': null
    },
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }, {
    'UnionId': '192.168.129.30-d04492e0-1ec2-4725-9ec4-c01e067ba41b',
    'FusionDirectorIp': '192.168.129.30',
    'Id': 'mainboardMEZZ2',
    '@odata.id': '/redfish/v1/rich/Nodes/{serverGuid}/NetworkAdapter/d04492e0-1ec2-4725-9ec4-c01e067ba41b',
    'DeviceID': 'd04492e0-1ec2-4725-9ec4-c01e067ba41b',
    'Name': 'mainboardMEZZ2',
    'DeviceLocator': 'MEZZ2',
    'DriverName': '',
    'DriverVersion': '',
    'Manufacturer': 'Mellanox',
    'Model': 'CX4',
    'CardName': 'MZ620',
    'CardManufacturer': 'Huawei',
    'CardModel': '2*100G IB Port HCA Mezzanine Card',
    'Position': 'mainboard',
    'NetworkPort': {
      '@odata.id': '/redfish/v1/rich/Nodes/{serverGuid}/NetworkAdapter/d04492e0-1ec2-4725-9ec4-c01e067ba41b/NetworkPort',
      'DeviceID': null,
      'Id': null,
      'Name': null
    },
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }, {
    'UnionId': '192.168.129.30-fea66688-5254-499b-baf4-27a033ec7eab',
    'FusionDirectorIp': '192.168.129.30',
    'Id': 'mainboardMEZZ3',
    '@odata.id': '/redfish/v1/rich/Nodes/{serverGuid}/NetworkAdapter/fea66688-5254-499b-baf4-27a033ec7eab',
    'DeviceID': 'fea66688-5254-499b-baf4-27a033ec7eab',
    'Name': 'mainboardMEZZ3',
    'DeviceLocator': 'MEZZ3',
    'DriverName': '',
    'DriverVersion': '',
    'Manufacturer': 'Cavium',
    'Model': 'BCM57810S',
    'CardName': 'MZ520',
    'CardManufacturer': 'Huawei',
    'CardModel': '2*10GE Port CNA Mezzanine Card,PCIE 2.0 X8',
    'Position': 'mainboard',
    'NetworkPort': {
      '@odata.id': '/redfish/v1/rich/Nodes/{serverGuid}/NetworkAdapter/fea66688-5254-499b-baf4-27a033ec7eab/NetworkPort',
      'DeviceID': null,
      'Id': null,
      'Name': null
    },
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }, {
    'UnionId': '192.168.129.30-bdf837fa-db04-48ff-a3be-391aefa0b2a8',
    'FusionDirectorIp': '192.168.129.30',
    'Id': 'mainboardMEZZ4',
    '@odata.id': '/redfish/v1/rich/Nodes/{serverGuid}/NetworkAdapter/bdf837fa-db04-48ff-a3be-391aefa0b2a8',
    'DeviceID': 'bdf837fa-db04-48ff-a3be-391aefa0b2a8',
    'Name': 'mainboardMEZZ4',
    'DeviceLocator': 'MEZZ4',
    'DriverName': '',
    'DriverVersion': '',
    'Manufacturer': 'Mellanox',
    'Model': 'CX4',
    'CardName': 'MZ620',
    'CardManufacturer': 'Huawei',
    'CardModel': '2*100G IB Port HCA Mezzanine Card',
    'Position': 'mainboard',
    'NetworkPort': {
      '@odata.id': '/redfish/v1/rich/Nodes/{serverGuid}/NetworkAdapter/bdf837fa-db04-48ff-a3be-391aefa0b2a8/NetworkPort',
      'DeviceID': null,
      'Id': null,
      'Name': null
    },
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }],
  'RaidCards': [{
    '@odata.context': '/redfish/v1/rich/$metadata#NodePoolService.NodePoolService',
    '@odata.id': '/redfish/v1/rich/Nodes/{serverGuid}/Storage/RaidCard/5d88cbc6-6a0c-45ab-bedc-39398fdd87c8',
    '@odata.type': '#NodePoolService.v1_2_0.RaidCardInfo',
    'DeviceID': '5d88cbc6-6a0c-45ab-bedc-39398fdd87c8',
    'Name': 'RAIDStorage0',
    'StorageControllers': [{
      'UnionId': '192.168.129.30-5d88cbc6-6a0c-45ab-bedc-39398fdd87c8-RAIDCard1Controller',
      'DeviceId': '5d88cbc6-6a0c-45ab-bedc-39398fdd87c8-RAIDCard1Controller',
      'FusionDirectorIp': '192.168.129.30',
      'Description': 'RAID Controller',
      'FirmwareVersion': '5.010.00-0839',
      'Manufacturer': '',
      'MemberId': '0',
      'Model': 'SAS3508',
      'Name': 'RAID Card1 Controller',
      'Oem': {
        'Huawei': {
          'CapacitanceStatus': {
            'Health': '',
            'State': 'Absent'
          },
          'ConfigurationVersion': '4.1610.00-0149',
          'MemorySizeMiB': 2048,
          'SASAddress': '5505dac310072000',
          'SupportedRAIDLevels': ['RAID0', 'RAID1', 'RAID5', 'RAID6', 'RAID10', 'RAID50', 'RAID60']
        }
      },
      'SpeedGbps': 12.0,
      'SupportedDeviceProtocols': ['SAS'],
      'Status': {
        'State': 'Enabled',
        'Health': 'OK'
      },
      'EnableState': 'Enabled',
      'Health': 'OK'
    }],
    'StorageControllers@odata.count': 1.0,
    'Volume': {
      '@odata.id': '/redfish/v1/rich/Nodes/{serverGuid}/Storage/RaidCard/5d88cbc6-6a0c-45ab-bedc-39398fdd87c8/Volume',
      'DeviceID': null,
      'Id': null,
      'Name': null
    },
    'Health': 'OK'
  }],
  'Drives': [{
    '@odata.context': '/redfish/v1/rich/$metadata#NodePoolService.NodePoolService',
    '@odata.id': '/redfish/v1/rich/Nodes/{serverGuid}/Storage/Drive/efff388c-0a90-4f1d-872f-725b7b9a0b66',
    '@odata.type': '#NodePoolService.v1_2_0.DriveInfo',
    'CapableSpeedGbs': 12.0,
    'CapacityGiB': 278.0,
    'DeviceID': 'efff388c-0a90-4f1d-872f-725b7b9a0b66',
    'IndicatorLED': 'Off',
    'Manufacturer': 'TOSHIBA',
    'MediaType': 'HDD',
    'Model': 'AL14SEB030N',
    'Name': 'Disk0',
    'Oem': {
      'Huawei': {
        'FirmwareStatus': 'UnconfiguredGood',
        'SASAddress': ['50000398b86988be', '0000000000000000'],
        'TemperatureCelsius': 30
      },
      'SASSmartInformation': {
        'SequenceNumberOfLastPredFailEvent': 0
      }
    },
    'Protocol': 'SAS',
    'Revision': '0807',
    'SerialNumber': '68N0A03ZF4WD',
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }, {
    'UnionId': '192.168.129.30-d4fdf8b0-e723-4095-9a72-0e5b9c8bd07a',
    'FusionDirectorIp': '192.168.129.30',
    '@odata.context': '/redfish/v1/rich/$metadata#NodePoolService.NodePoolService',
    '@odata.id': '/redfish/v1/rich/Nodes/{serverGuid}/Storage/Drive/d4fdf8b0-e723-4095-9a72-0e5b9c8bd07a',
    '@odata.type': '#NodePoolService.v1_2_0.DriveInfo',
    'CapableSpeedGbs': 12.0,
    'CapacityGiB': 278.0,
    'DeviceID': 'd4fdf8b0-e723-4095-9a72-0e5b9c8bd07a',
    'IndicatorLED': 'Off',
    'Manufacturer': 'TOSHIBA',
    'MediaType': 'HDD',
    'Model': 'AL14SEB030N',
    'Name': 'Disk4',
    'Oem': {
      'Huawei': {
        'FirmwareStatus': 'UnconfiguredGood',
        'SASAddress': ['50000398b8698c66', '0000000000000000'],
        'TemperatureCelsius': 29
      },
      'SASSmartInformation': {
        'SequenceNumberOfLastPredFailEvent': 0
      }
    },
    'Protocol': 'SAS',
    'Revision': '0807',
    'SerialNumber': '68N0A04AF4WD',
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }, {
    'UnionId': '192.168.129.30-94eadd0f-cfbe-4784-8347-e1f8918d19f1',
    'FusionDirectorIp': '192.168.129.30',
    '@odata.context': '/redfish/v1/rich/$metadata#NodePoolService.NodePoolService',
    '@odata.id': '/redfish/v1/rich/Nodes/{serverGuid}/Storage/Drive/94eadd0f-cfbe-4784-8347-e1f8918d19f1',
    '@odata.type': '#NodePoolService.v1_2_0.DriveInfo',
    'CapableSpeedGbs': 12.0,
    'CapacityGiB': 278.0,
    'DeviceID': '94eadd0f-cfbe-4784-8347-e1f8918d19f1',
    'IndicatorLED': 'Off',
    'Manufacturer': 'TOSHIBA',
    'MediaType': 'HDD',
    'Model': 'AL14SEB030N',
    'Name': 'Disk2',
    'Oem': {
      'Huawei': {
        'FirmwareStatus': 'UnconfiguredGood',
        'SASAddress': ['50000398b8698c0e', '0000000000000000'],
        'TemperatureCelsius': 30
      },
      'SASSmartInformation': {
        'SequenceNumberOfLastPredFailEvent': 0
      }
    },
    'Protocol': 'SAS',
    'Revision': '0807',
    'SerialNumber': '68N0A047F4WD',
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }],
  'PCIeCards': [{
    '@odata.context': null,
    '@odata.id': '/redfish/v1/rich/Nodes/4bdfd39e-95e0-46b8-b118-f434e648cd2d/PCIe/d0c21839-a138-4848-9eff-90b73344af5a',
    '@odata.type': null,
    'DeviceID': 'd0c21839-a138-4848-9eff-90b73344af5a',
    'Name': 'PCIeCard5',
    'Description': 'MegaRAID SAS-3 3108 [Invader]',
    'Manufacturer': 'LSI Logic / Symbios Logic',
    'Model': '',
    'ModelType': '',
    'SerialNumber': '',
    'FirmwareVersion': '',
    'Oem': {
      'Huawei': {
        'DeviceLocator': 'PCIe Card 5 (RAID)',
        'Position': 'PCIe Riser 2',
        'Power': 0,
        'FunctionType': 'RAID Card'
      }
    },
    'PCIeFunction': {
      '@odata.id': '/redfish/v1/rich/Nodes/4bdfd39e-95e0-46b8-b118-f434e648cd2d/PCIe/d0c21839-a138-4848-9eff-90b73344af5a/Function',
      'DeviceID': null,
      'Id': null,
      'Name': null
    },
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }],
  'Powers': [{
    'DeviceId': 'd31ed9f0-58e8-4b34-b310-8938aeac4a74-PS1',
    'MemberId': '0',
    'Name': 'PS1',
    'PowerSupplyType': 'AC',
    'PowerCapacityWatts': 1500.0,
    'Model': 'PS-2152-2H',
    'FirmwareVersion': 'DC:108 PFC:107',
    'SerialNumber': '2102131336CSJ3005736',
    'PartNumber': '02131336',
    'Manufacturer': 'LITEON',
    'Oem': {
      'Huawei': {
        'Protocol': 'PSU',
        'DeviceLocator': 'PS1',
        'Position': 'chassis',
        'ActiveStandby': 'Active'
      }
    },
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }, {
    'DeviceId': 'd31ed9f0-58e8-4b34-b310-8938aeac4a74-PS2',
    'MemberId': '1',
    'Name': 'PS2',
    'PowerSupplyType': '',
    'PowerCapacityWatts': 1500.0,
    'Model': 'PS-2152-2H',
    'FirmwareVersion': 'DC:108 PFC:107',
    'SerialNumber': '2102131336CSJ3001326',
    'PartNumber': '02131336',
    'Manufacturer': 'LITEON',
    'Oem': {
      'Huawei': {
        'Protocol': 'PSU',
        'DeviceLocator': 'PS2',
        'Position': 'chassis',
        'ActiveStandby': 'Active'
      }
    },
    'Status': {
      'State': 'Enabled',
      'Health': 'Critical'
    },
    'EnableState': 'Enabled',
    'Health': 'Critical'
  }],
  'Fans': [{
    'DeviceId': 'd31ed9f0-58e8-4b34-b310-8938aeac4a74-FanModule1Front',
    'MemberId': '0',
    'Name': 'Fan Module1 Front',
    'Reading': 10920.0,
    'ReadingUnits': 'RPM',
    'PartNumber': '02311VSF',
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }, {
    'DeviceId': 'd31ed9f0-58e8-4b34-b310-8938aeac4a74-FanModule2Front',
    'MemberId': '1',
    'Name': 'Fan Module2 Front',
    'Reading': 10920.0,
    'ReadingUnits': 'RPM',
    'PartNumber': '02311VSF',
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }, {
    'DeviceId': 'd31ed9f0-58e8-4b34-b310-8938aeac4a74-FanModule3Front',
    'MemberId': '2',
    'Name': 'Fan Module3 Front',
    'Reading': 10920.0,
    'ReadingUnits': 'RPM',
    'PartNumber': '02311VSF',
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }, {
    'DeviceId': 'd31ed9f0-58e8-4b34-b310-8938aeac4a74-FanModule4Front',
    'MemberId': '3',
    'Name': 'Fan Module4 Front',
    'Reading': 10800.0,
    'ReadingUnits': 'RPM',
    'PartNumber': '02311VSF',
    'Status': {
      'State': 'Enabled',
      'Health': 'OK'
    },
    'EnableState': 'Enabled',
    'Health': 'OK'
  }],
  'Status': {
    'State': 'Enabled',
    'Health': 'OK'
  },
  'EnableState': 'Enabled',
  'Health': 'OK'
}
";
            #endregion

            var fusionDiretorIp = "192.168.8.12";
            int i = 0;
            while (i < 1000)
            {
                i++;
                var serverId = Guid.NewGuid().ToString();
                var server = JsonConvert.DeserializeObject<Server>(t.Replace("{serverGuid}", serverId));
                server.IPv4Address.Address = i.ToString();
                server.MakeDetails(fusionDiretorIp);

                var task = taskFactory.StartNew(() =>
                {
                    Console.WriteLine("-- " + DateTime.Now.ToString("HH:mm:ss fff") + " " + Thread.CurrentThread.ManagedThreadId);
                    ServerConnector.Instance.Sync(server).Wait();
                    Console.WriteLine(i.ToString() + "++ " + DateTime.Now.ToString("HH:mm:ss fff") + " " + Thread.CurrentThread.ManagedThreadId);

                }, cts.Token);
            }

        }

        private async Task DealServerPerformance(ServerSummary server)
        {
            try
            {
                var unionId = $"{FusionDirectorIp}-{server.Id}";
                var realTimeData = await this.metricsService.GetServerRealTimePerformanceAsync(server.Id);
                var objectName = $"{this.FusionDirectorIp}_{server.IPv4Address.Address}";
                ServerConnector.Instance.InsertPerformanceData(unionId, objectName, realTimeData);
            }
            catch (Exception ex)
            {
                OnPollingError($"DealServerPerformance Error:{server.Id}", ex);
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            service = new FusionDirectorPluginService();
        }

        private void btnInsertHistoryEvent_Click(object sender, EventArgs e)
        {
            #region MyRegion

            var t = @" {
      '@odata.context': '/redfish/v1/rich/$metadata#EventService.Events',
      '@odata.id': '/redfish/v1/rich/EventService',
      '@odata.type': '#EventService.v1_2_0.Events',
      'Members': [{
          //机框
          'BlockingStatus': false,
          'ClearTime': '',
          'ClearType': '',
          'ConfirmStatus': 'Confirmed',
          'DeviceID': '0242f28b-6471-5ae6-a274-9330a8221b34',
          'DeviceSn': '2102311TYBN0J3000293',
          'DeviceType': '2288H V5',
          'EventCategory': 'Enclosure',
          'EventDescription': 'iBMC security log has reached 90% space capacity.',
          'EventDescriptionArgs': 'null',
          'EventID': '0x1A00001D',
          'EventName': 'Security logs are about to overflow',
          'EventSource': '192.168.129.9',
          'EventSubject': 'Enclosure',
          'EventType': 'event',
          'FirstOccurTime': '2019-01-18T12:36:27+08:00',
          'LastOccurTime': '2019-01-18T12:36:27+08:00',
          'Link': {
            '@odata.id': '/redfish/v1/rich/EventService/Events/36'
          },
          'OccurCounts': '1',
          'Parts': '',
          'ReadFlag': 'Unread',
          'ScopeUris': [],
          'SerialNumber': 1122,
          'Severity': 'Warning',
          'Status': 'Uncleared'
        },
        { //BMC
          'BlockingStatus': false,
          'ClearTime': '',
          'ClearType': '',
          'ConfirmStatus': 'Unconfirmed',
          'DeviceID': '65e77ebb-1083-4bae-9885-10523b814d4a',
          'DeviceSn': '2102311TYBN0J3000293',
          'DeviceType': '2288H V5',
          'EventCategory': 'BMC',
          'EventDescription': 'The AC/DC input of PSU 2 is lost or out-of-range.',
          'EventDescriptionArgs': '[\'2\']',
          'EventID': '0x0300000D',
          'EventName': 'Server Input Lost Critical Alarm',
          'EventSource': '192.168.129.9',
          'EventSubject': 'PS2',
          'EventType': 'alert',
          'FirstOccurTime': '2019-01-10T07:34:22+08:00',
          'LastOccurTime': '2019-01-10T07:34:22+08:00',
          'Link': {
            '@odata.id': '/redfish/v1/rich/EventService/Events/2'
          },
          'OccurCounts': '1',
          'Parts': '',
          'ReadFlag': 'Unread',
          'ScopeUris': [],
          'SerialNumber': 113,
          'Severity': 'Critical',
          'Status': 'Cleared'
        },
        {
          //switch
          'BlockingStatus': false,
          'ClearTime': '',
          'ClearType': '',
          'ConfirmStatus': 'Unconfirmed',
          'DeviceID': '02dc6ca8-0c8d-4794-8a6e-8561ca092e59',
          'DeviceSn': '210305774210J7000006',
          'DeviceType': 'CH242 V5',
          'EventCategory': 'Switch',
          'EventDescription': 'switch EventDescription',
          'EventDescriptionArgs': '[\'Disk6\']',
          'EventID': '0x02000007',
          'EventName': 'Hard switch Fault Warning Alarm',
          'EventSource': '192.168.129.120',
          'EventSubject': 'Disk6',
          'EventType': 'alert',
          'FirstOccurTime': '2019-01-11T20:01:19+08:00',
          'LastOccurTime': '2019-01-11T20:01:19+08:00',
          'Link': {
            '@odata.id': '/redfish/v1/rich/EventService/Events/1'
          },
          'OccurCounts': '1',
          'Parts': '',
          'ReadFlag': 'Unread',
          'ScopeUris': [],
          'SerialNumber': 133,
          'Severity': 'Warning',
          'Status': 'Uncleared'
        }
      ],
      'Members@odata.count': 3
    }";

            #endregion

            var data = JsonConvert.DeserializeObject<EventList>(t);
            data.Members.ForEach(eventSummary =>
            {
                var alarm = new AlarmData(eventSummary);
                var eventData = new EventData(alarm, "192.168.8.12");
                switch (alarm.EventCategory)
                {
                    case "BMC":
                        ServerConnector.Instance.InsertEvent(eventData);
                        break;
                    case "Enclosure":
                        EnclosureConnector.Instance.InsertEvent(eventData);
                        break;
                    default:
                        break;
                }
            });
        }

        private void btnInsertServerPerformanceData_Click(object sender, EventArgs e)
        {
            //ServerConnector.Instance.CheckUnclosedAlert("192.168.129.103", new List<string>() { "666", "667" });
            //ServerConnector.Instance.InsertPerformanceDataTest("192.168.8.12-3f009400-e8a2-43eb-b327-af536f226359");
            //EnclosureConnector.Instance.InsertPerformanceDataTest("192.168.1.1-0242f28b-6471-5ae6-a274-9330a8221b34");
        }

        private void btnInsertAppliance_Click(object sender, EventArgs e)
        {
            var appliance = new Appliance
            {
                HostName = "HostName",
                IPAddress = "IPAddress",
                SoftwareVersion = "SoftwareVersion",
                EnclosureCollection = new EnclosureCollection { ResourceName = "EnclosureCollection", Health = Health.Critical },
                ServerCollection = new ServerCollection { ResourceName = "ServerCollection", Health = Health.Critical },
                EventCollection = new EventCollection { ResourceName = "EventCollection", Health = Health.Critical },
                FusionDirectorCollection = new FusionDirectorCollection { ResourceName = "FusionDirectorCollection", Health = Health.Critical },
                PerformanceCollection = new PerformanceCollection { ResourceName = "PerformanceCollection", Health = Health.Critical }
            };
            ApplianceConnector.Instance.Sync(appliance, false);
        }


        private void CreateOrCloseFusionDirectorAlarm()
        {
            var dic = new Dictionary<string, string>();
            dic.Add("192.168.8.12", $"OK");
            dic.Add("192.168.8.13", $"OK");
            dic.Add("192.168.8.14", $"OK");
            dic.Add("192.168.8.15", $"Fusion Director Server(192.168.8.14) connect faild");
            FusionDirectorCollection currentState = new FusionDirectorCollection
            {
                ResourceName = "FusionDirectorCollection",
                Health = Health.Critical,
                ErrorMsg = JsonConvert.SerializeObject(dic)
            };

            var existAlarmData = ApplianceConnector.Instance.GetUnclosedAlert();
            var fdErrors = JsonConvert.DeserializeObject<Dictionary<string, string>>(currentState.ErrorMsg);
            foreach (var fdError in fdErrors)
            {
                var fdIp = fdError.Key;
                var error = fdError.Value;
                var isHaveCurrentAlarm = fdError.Value != "OK";//本次是否有告警
                bool isHaveOldAlarm = existAlarmData.Any(x => x.CustomField1 == EnumAlarmType.FdConnectError.ToString() && x.CustomField4.Contains(fdIp));
                if (isHaveCurrentAlarm)//本次有告警，则插入或更新告警
                {
                    var alarm = new ApplianceAlarm
                    {
                        OptType = "1",
                        AlarmName = "FusionDirector Connect Error",
                        AlarmType = EnumAlarmType.FdConnectError,
                        PossibleCause = $"({error})",
                        Suggstion = "check whether the FusionDirector server is shut down or whether the network is abnormal.",
                        Additional = error
                    };
                    var applianceEvent = new ApplianceEvent(alarm);
                    ApplianceConnector.Instance.InsertEvent(applianceEvent, fdIp);
                }
                else
                {
                    //本次没有告警 且有旧的告警，则关闭
                    if (isHaveOldAlarm)
                    {
                        var alarm = new ApplianceAlarm
                        {
                            OptType = "2",
                            AlarmType = EnumAlarmType.FdConnectError,
                        };
                        var applianceEvent = new ApplianceEvent(alarm);
                        ApplianceConnector.Instance.InsertEvent(applianceEvent, fdIp);
                    }
                }
            }

        }

        private void btnStopSync_Click(object sender, EventArgs e)
        {
            cts.Cancel(false);
        }

        /// <summary>
        /// 模拟轮询时同步服务器
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private async void btnMockSync_Click(object sender, EventArgs e)
        {
            await MockSyncServer();
            //MockSyncEnclosure();
            //CreateOrCloseFusionDirectorAlarm();
        }
        public ServerList t()
        {
            var t = "{'@odata.context':'/redfish/v1/rich/$metadata#NodePoolService.NodePoolService','@odata.id':'/redfish/v1/rich/Nodes','@odata.type':'#NodePoolService.v1_2_0.NodeList','Members':[{'DeviceID':'174c9dea-b72e-446b-a02e-6dbed5448fd8','SerialNumber':'0e4d0f0ca','UUID':'00000000-0000-0000-0000-000000005001','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.1'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/174c9dea-b72e-446b-a02e-6dbed5448fd8','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'c9fbe341-8aca-46b3-ae0f-edddff34d30f','SerialNumber':'0cb5fdc3c','UUID':'00000000-0000-0000-0000-000000005002','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.2'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/c9fbe341-8aca-46b3-ae0f-edddff34d30f','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'f9ac313d-440c-4a01-b0ed-95343f4d5706','SerialNumber':'09b999404','UUID':'00000000-0000-0000-0000-000000005003','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.3'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/f9ac313d-440c-4a01-b0ed-95343f4d5706','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'52a568f2-2191-463b-b6e6-1bb7d75fdd83','SerialNumber':'06a6a5473','UUID':'00000000-0000-0000-0000-000000005004','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.4'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/52a568f2-2191-463b-b6e6-1bb7d75fdd83','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'ec43f27d-f83a-4ac2-89ad-8fe170a891b0','SerialNumber':'061be5751','UUID':'00000000-0000-0000-0000-000000005005','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.5'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/ec43f27d-f83a-4ac2-89ad-8fe170a891b0','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'a3c8166c-38a1-4ea7-975c-34cc6bb036ab','SerialNumber':'079a2d3b8','UUID':'00000000-0000-0000-0000-000000005006','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.6'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/a3c8166c-38a1-4ea7-975c-34cc6bb036ab','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'558c8529-6afb-4e9d-944c-842ee156ac99','SerialNumber':'0dd0a2d0a','UUID':'00000000-0000-0000-0000-000000005007','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.7'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/558c8529-6afb-4e9d-944c-842ee156ac99','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'c17a2efd-67ac-44c0-91e2-6d1466e1b30d','SerialNumber':'05a27b257','UUID':'00000000-0000-0000-0000-000000005008','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.8'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/c17a2efd-67ac-44c0-91e2-6d1466e1b30d','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'b2747f3f-4f88-426d-a4b9-f12be56adffd','SerialNumber':'0db0022c5','UUID':'00000000-0000-0000-0000-000000005009','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.9'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/b2747f3f-4f88-426d-a4b9-f12be56adffd','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'bb66517d-6f96-429e-ad4d-5a8089c5aee2','SerialNumber':'066ec6159','UUID':'00000000-0000-0000-0000-000000005010','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.10'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/bb66517d-6f96-429e-ad4d-5a8089c5aee2','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'70fa0cec-2666-4c30-a4d8-aebde0918bd1','SerialNumber':'0602a1c47','UUID':'00000000-0000-0000-0000-000000005011','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.11'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/70fa0cec-2666-4c30-a4d8-aebde0918bd1','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'e2afe84f-f8f0-461a-bacd-1e66de552fcb','SerialNumber':'0f80e2da7','UUID':'00000000-0000-0000-0000-000000005012','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.12'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/e2afe84f-f8f0-461a-bacd-1e66de552fcb','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'3de8c449-8a18-4e44-a008-de409b680013','SerialNumber':'06dd095ae','UUID':'00000000-0000-0000-0000-000000005013','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.13'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/3de8c449-8a18-4e44-a008-de409b680013','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'f80bc776-5322-4403-8caa-efb6cf452051','SerialNumber':'08e85af83','UUID':'00000000-0000-0000-0000-000000005014','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.14'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/f80bc776-5322-4403-8caa-efb6cf452051','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'7310baf7-f98a-4fab-b5c5-f5e04b07e258','SerialNumber':'05cfd9ae8','UUID':'00000000-0000-0000-0000-000000005015','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.15'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/7310baf7-f98a-4fab-b5c5-f5e04b07e258','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'8339a9ab-2663-4787-a4c5-1c23e4acb3e4','SerialNumber':'0f40e2cc9','UUID':'00000000-0000-0000-0000-000000005016','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.16'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/8339a9ab-2663-4787-a4c5-1c23e4acb3e4','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'d7662b51-cb88-4ab9-b60d-b94e567f4431','SerialNumber':'056b6c866','UUID':'00000000-0000-0000-0000-000000005017','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.17'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/d7662b51-cb88-4ab9-b60d-b94e567f4431','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'937eb0d1-ef8a-4195-aa1a-5b80c0a50688','SerialNumber':'073b291aa','UUID':'00000000-0000-0000-0000-000000005018','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.18'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/937eb0d1-ef8a-4195-aa1a-5b80c0a50688','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'6900731d-6492-4fb2-afd9-311b5a738463','SerialNumber':'001518b77','UUID':'00000000-0000-0000-0000-000000005019','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.19'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/6900731d-6492-4fb2-afd9-311b5a738463','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'bba23f5b-96dc-4ec4-9f39-383334a29a04','SerialNumber':'03dfef8ac','UUID':'00000000-0000-0000-0000-000000005020','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.20'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/bba23f5b-96dc-4ec4-9f39-383334a29a04','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'8d5c59c0-0cdb-4a96-89e0-f1a104e306b2','SerialNumber':'0a82cb7d8','UUID':'00000000-0000-0000-0000-000000005021','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.21'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/8d5c59c0-0cdb-4a96-89e0-f1a104e306b2','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'98d7ec2d-208c-4c8f-8aae-3ccc565c470c','SerialNumber':'09e1f6898','UUID':'00000000-0000-0000-0000-000000005022','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.22'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/98d7ec2d-208c-4c8f-8aae-3ccc565c470c','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'3958114b-ed6d-46db-838b-8204b54c4031','SerialNumber':'0210586c2','UUID':'00000000-0000-0000-0000-000000005023','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.23'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/3958114b-ed6d-46db-838b-8204b54c4031','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'8684cfcf-cc33-4d99-9aa9-5c61b9a49436','SerialNumber':'0e8c71a25','UUID':'00000000-0000-0000-0000-000000005024','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.24'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/8684cfcf-cc33-4d99-9aa9-5c61b9a49436','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'8a28a20d-393f-49c4-899d-568273a62502','SerialNumber':'0eacdac28','UUID':'00000000-0000-0000-0000-000000005025','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.25'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/8a28a20d-393f-49c4-899d-568273a62502','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'59ec6cf2-f8ce-4901-8b08-4671049f8546','SerialNumber':'0c5d0c510','UUID':'00000000-0000-0000-0000-000000005026','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.26'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/59ec6cf2-f8ce-4901-8b08-4671049f8546','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'8ea0e468-b6ca-4e93-8b1e-b88f745b5fdd','SerialNumber':'030b21379','UUID':'00000000-0000-0000-0000-000000005027','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.27'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/8ea0e468-b6ca-4e93-8b1e-b88f745b5fdd','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'ee01c5c8-635e-41c9-9c55-6a9f0749c464','SerialNumber':'051a7f4e2','UUID':'00000000-0000-0000-0000-000000005028','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.28'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/ee01c5c8-635e-41c9-9c55-6a9f0749c464','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'8b5bbea1-41ff-4888-9a33-1e70aad90681','SerialNumber':'0f929c810','UUID':'00000000-0000-0000-0000-000000005029','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.29'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/8b5bbea1-41ff-4888-9a33-1e70aad90681','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'fa5d1d6f-4e1e-427d-87d3-cd43f37f4057','SerialNumber':'03bc85cdd','UUID':'00000000-0000-0000-0000-000000005030','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.30'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/fa5d1d6f-4e1e-427d-87d3-cd43f37f4057','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'2899a48e-dd7b-4d0f-929c-413c0617ea23','SerialNumber':'01e163cfa','UUID':'00000000-0000-0000-0000-000000005031','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.31'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/2899a48e-dd7b-4d0f-929c-413c0617ea23','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'f7b11d4b-aceb-49e1-b075-ec0182735b8c','SerialNumber':'04a1ce957','UUID':'00000000-0000-0000-0000-000000005032','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.32'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/f7b11d4b-aceb-49e1-b075-ec0182735b8c','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'643f37bc-e44f-49d4-be32-bf9218f738c9','SerialNumber':'0afddebfd','UUID':'00000000-0000-0000-0000-000000005033','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.33'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/643f37bc-e44f-49d4-be32-bf9218f738c9','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'08cf4b6b-1238-44e3-b89c-40026b0d6693','SerialNumber':'08470db5b','UUID':'00000000-0000-0000-0000-000000005034','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.34'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/08cf4b6b-1238-44e3-b89c-40026b0d6693','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'65b963e9-57dd-49b5-b38c-c3d531fcc701','SerialNumber':'0c47a5c59','UUID':'00000000-0000-0000-0000-000000005035','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.35'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/65b963e9-57dd-49b5-b38c-c3d531fcc701','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'ebb7cfec-8b8c-464d-b678-b1dac1dc97ca','SerialNumber':'03e1a27f4','UUID':'00000000-0000-0000-0000-000000005036','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.36'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/ebb7cfec-8b8c-464d-b678-b1dac1dc97ca','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'95f89f94-3efd-453f-88e6-be89c2810848','SerialNumber':'0faa4f0d2','UUID':'00000000-0000-0000-0000-000000005037','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.37'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/95f89f94-3efd-453f-88e6-be89c2810848','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'e4a6b4a6-788d-4118-b076-beefd0c059cf','SerialNumber':'0aff019ce','UUID':'00000000-0000-0000-0000-000000005038','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.38'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/e4a6b4a6-788d-4118-b076-beefd0c059cf','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'9053327e-8e78-4604-a217-d9e92f4d5ac1','SerialNumber':'04d9873c0','UUID':'00000000-0000-0000-0000-000000005039','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.39'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/9053327e-8e78-4604-a217-d9e92f4d5ac1','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'77b42c82-2524-4b92-9423-4455a27e77e7','SerialNumber':'0479ca42c','UUID':'00000000-0000-0000-0000-000000005040','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.40'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/77b42c82-2524-4b92-9423-4455a27e77e7','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'7440e474-1985-4baa-a22e-e8d814208b52','SerialNumber':'0972eff3e','UUID':'00000000-0000-0000-0000-000000005041','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.41'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/7440e474-1985-4baa-a22e-e8d814208b52','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'fcbb1708-7180-45bf-acf4-e6e105f4a363','SerialNumber':'00c5087dc','UUID':'00000000-0000-0000-0000-000000005042','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.42'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/fcbb1708-7180-45bf-acf4-e6e105f4a363','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'930945b4-e2e8-478b-9905-76eff8dda4eb','SerialNumber':'08134ea74','UUID':'00000000-0000-0000-0000-000000005043','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.43'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/930945b4-e2e8-478b-9905-76eff8dda4eb','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'ae6f8ffa-92b4-4127-a499-1e8cdf146573','SerialNumber':'07a7c9485','UUID':'00000000-0000-0000-0000-000000005044','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.44'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/ae6f8ffa-92b4-4127-a499-1e8cdf146573','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'612c17b7-ee0f-41d2-b307-a460333f12ef','SerialNumber':'02b41134f','UUID':'00000000-0000-0000-0000-000000005045','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.45'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/612c17b7-ee0f-41d2-b307-a460333f12ef','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'14d17884-4bb8-4f97-8266-ca166b3f5c8e','SerialNumber':'0e0f957fc','UUID':'00000000-0000-0000-0000-000000005046','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.46'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/14d17884-4bb8-4f97-8266-ca166b3f5c8e','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'3308941b-6569-4ec2-815e-229b242eb2f4','SerialNumber':'09d1d5786','UUID':'00000000-0000-0000-0000-000000005047','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.47'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/3308941b-6569-4ec2-815e-229b242eb2f4','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'4b573bff-ed32-446a-aad1-28a88ec70068','SerialNumber':'0d24239fd','UUID':'00000000-0000-0000-0000-000000005048','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.48'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/4b573bff-ed32-446a-aad1-28a88ec70068','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'6428b701-cdf5-4190-8eec-9fdefd47aae5','SerialNumber':'0fd247cd0','UUID':'00000000-0000-0000-0000-000000005049','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.49'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/6428b701-cdf5-4190-8eec-9fdefd47aae5','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'9fd25bf0-4312-493e-9c12-e3bec33e480f','SerialNumber':'00fbb413f','UUID':'00000000-0000-0000-0000-000000005050','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.50'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/9fd25bf0-4312-493e-9c12-e3bec33e480f','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'110fdf75-15a0-413d-9d92-6ed92b8dd5b0','SerialNumber':'08e9ef644','UUID':'00000000-0000-0000-0000-000000005051','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.51'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/110fdf75-15a0-413d-9d92-6ed92b8dd5b0','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'9af73518-9f75-4370-9706-cf3cea2668dc','SerialNumber':'0dddfdba1','UUID':'00000000-0000-0000-0000-000000005052','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.52'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/9af73518-9f75-4370-9706-cf3cea2668dc','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'e4fff35d-f41f-4fa3-95bb-35c333205448','SerialNumber':'07400f647','UUID':'00000000-0000-0000-0000-000000005053','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.53'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/e4fff35d-f41f-4fa3-95bb-35c333205448','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'e4b1ee95-f7c4-4f9d-8bd5-3258e01d3469','SerialNumber':'0d52dab67','UUID':'00000000-0000-0000-0000-000000005054','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.54'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/e4b1ee95-f7c4-4f9d-8bd5-3258e01d3469','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'c53f088e-83d7-4e67-ba20-1bc7d6cb49c2','SerialNumber':'08e856d67','UUID':'00000000-0000-0000-0000-000000005055','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.55'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/c53f088e-83d7-4e67-ba20-1bc7d6cb49c2','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'810c5dfc-f72f-4448-86f3-5864471f0e0a','SerialNumber':'04f2d5e7d','UUID':'00000000-0000-0000-0000-000000005056','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.56'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/810c5dfc-f72f-4448-86f3-5864471f0e0a','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'4b3eb8d5-be20-4b85-bdfa-3aeb258353f3','SerialNumber':'0798e9872','UUID':'00000000-0000-0000-0000-000000005057','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.57'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/4b3eb8d5-be20-4b85-bdfa-3aeb258353f3','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'cb1b8f19-1864-499b-a62f-a84c9cf9e9f6','SerialNumber':'0310cfadf','UUID':'00000000-0000-0000-0000-000000005058','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.58'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/cb1b8f19-1864-499b-a62f-a84c9cf9e9f6','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'a1ea30ee-7580-45ab-9a60-ddc13c3d93e5','SerialNumber':'01e26127a','UUID':'00000000-0000-0000-0000-000000005059','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.59'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/a1ea30ee-7580-45ab-9a60-ddc13c3d93e5','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'d19ef7b0-26e9-4d7a-993c-98016a4fca1f','SerialNumber':'01f106187','UUID':'00000000-0000-0000-0000-000000005060','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.60'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/d19ef7b0-26e9-4d7a-993c-98016a4fca1f','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'fdaa3407-73ea-462d-a3d0-99c14ad710c0','SerialNumber':'0698230a6','UUID':'00000000-0000-0000-0000-000000005061','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.61'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/fdaa3407-73ea-462d-a3d0-99c14ad710c0','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'178d223a-052d-40c5-ac56-36ae248021c3','SerialNumber':'0071a8c93','UUID':'00000000-0000-0000-0000-000000005062','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.62'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/178d223a-052d-40c5-ac56-36ae248021c3','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'381d1ce8-92c3-4430-9d18-f28648cb5fcf','SerialNumber':'050691e54','UUID':'00000000-0000-0000-0000-000000005063','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.63'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/381d1ce8-92c3-4430-9d18-f28648cb5fcf','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'842a9103-74cf-4411-86f5-fdd9541c7ace','SerialNumber':'0fd997100','UUID':'00000000-0000-0000-0000-000000005064','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.64'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/842a9103-74cf-4411-86f5-fdd9541c7ace','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'c2096a55-f75d-47b1-9512-faaacec3e065','SerialNumber':'013e3bf6d','UUID':'00000000-0000-0000-0000-000000005065','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.65'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/c2096a55-f75d-47b1-9512-faaacec3e065','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'288eba76-b9cf-44ec-9c22-39b688ea32de','SerialNumber':'039d66491','UUID':'00000000-0000-0000-0000-000000005066','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.66'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/288eba76-b9cf-44ec-9c22-39b688ea32de','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'3d53ca80-836c-48d4-a599-f738035149b4','SerialNumber':'04660fc6a','UUID':'00000000-0000-0000-0000-000000005067','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.67'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/3d53ca80-836c-48d4-a599-f738035149b4','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'34cdc22d-3da2-44fb-87e6-e785f5e46123','SerialNumber':'0024a770f','UUID':'00000000-0000-0000-0000-000000005068','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.68'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/34cdc22d-3da2-44fb-87e6-e785f5e46123','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'8daf9b2c-1125-407c-af36-eb606f2c3380','SerialNumber':'0c6c2166a','UUID':'00000000-0000-0000-0000-000000005069','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.69'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/8daf9b2c-1125-407c-af36-eb606f2c3380','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'1f88d056-5b09-41b3-9cb3-70a88fbd2f09','SerialNumber':'071d2aa05','UUID':'00000000-0000-0000-0000-000000005070','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.70'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/1f88d056-5b09-41b3-9cb3-70a88fbd2f09','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'ebb5589e-fa96-451d-bdb1-ded10e838aa7','SerialNumber':'0049d574f','UUID':'00000000-0000-0000-0000-000000005071','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.71'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/ebb5589e-fa96-451d-bdb1-ded10e838aa7','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'15ee84d1-56ee-4a12-afff-947ee176a537','SerialNumber':'07cf67495','UUID':'00000000-0000-0000-0000-000000005072','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.72'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/15ee84d1-56ee-4a12-afff-947ee176a537','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'88cfedfb-6ecd-4d6a-bf04-6e494e403068','SerialNumber':'051acee6e','UUID':'00000000-0000-0000-0000-000000005073','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.73'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/88cfedfb-6ecd-4d6a-bf04-6e494e403068','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'262161bb-54f7-4453-bedb-5c3f5856d0ae','SerialNumber':'032dfd511','UUID':'00000000-0000-0000-0000-000000005074','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.74'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/262161bb-54f7-4453-bedb-5c3f5856d0ae','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'c80ed7fb-31c2-4d62-aa41-cbe686ecb87c','SerialNumber':'04d012a2b','UUID':'00000000-0000-0000-0000-000000005075','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.75'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/c80ed7fb-31c2-4d62-aa41-cbe686ecb87c','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'1a48a4eb-4ff5-4ac1-b517-772b4a8cae0c','SerialNumber':'0fbcf13bd','UUID':'00000000-0000-0000-0000-000000005076','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.76'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/1a48a4eb-4ff5-4ac1-b517-772b4a8cae0c','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'167f2e66-f430-4260-9374-7e6fb9fd0ffa','SerialNumber':'0141bfc8d','UUID':'00000000-0000-0000-0000-000000005077','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.77'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/167f2e66-f430-4260-9374-7e6fb9fd0ffa','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'dff1bb5b-e397-4629-8f73-37ee5c504e3c','SerialNumber':'052a23ffa','UUID':'00000000-0000-0000-0000-000000005078','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.78'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/dff1bb5b-e397-4629-8f73-37ee5c504e3c','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'421d9447-4388-47ec-82c5-e4c000ada24f','SerialNumber':'0605b32ee','UUID':'00000000-0000-0000-0000-000000005079','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.79'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/421d9447-4388-47ec-82c5-e4c000ada24f','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'cf39953f-6764-4359-8526-db5e7991d70a','SerialNumber':'064b23fb9','UUID':'00000000-0000-0000-0000-000000005080','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.80'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/cf39953f-6764-4359-8526-db5e7991d70a','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'e4400580-5518-4c44-9c6f-87e5abf9b85e','SerialNumber':'061d361b1','UUID':'00000000-0000-0000-0000-000000005081','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.81'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/e4400580-5518-4c44-9c6f-87e5abf9b85e','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'4be8956c-bd99-4d6a-8b7a-b92400f51186','SerialNumber':'046a4c161','UUID':'00000000-0000-0000-0000-000000005082','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.82'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/4be8956c-bd99-4d6a-8b7a-b92400f51186','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'efd2b3cc-eaca-440a-ac7a-ee1479ad1fc0','SerialNumber':'0a9accdc4','UUID':'00000000-0000-0000-0000-000000005083','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.83'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/efd2b3cc-eaca-440a-ac7a-ee1479ad1fc0','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'297a8966-9a13-4d35-af0f-f7b4b3b6f657','SerialNumber':'0b2890589','UUID':'00000000-0000-0000-0000-000000005084','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.84'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/297a8966-9a13-4d35-af0f-f7b4b3b6f657','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'7e67d817-51e8-4d70-9ce9-2542eee3dae4','SerialNumber':'08380565e','UUID':'00000000-0000-0000-0000-000000005085','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.85'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/7e67d817-51e8-4d70-9ce9-2542eee3dae4','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'9ffbe60c-6316-4bd4-98ab-060b04220bfa','SerialNumber':'0de86a0c7','UUID':'00000000-0000-0000-0000-000000005086','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.86'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/9ffbe60c-6316-4bd4-98ab-060b04220bfa','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'325acd35-5c23-4ad4-8c3f-206b2aa855d5','SerialNumber':'0e907a569','UUID':'00000000-0000-0000-0000-000000005087','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.87'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/325acd35-5c23-4ad4-8c3f-206b2aa855d5','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'6f4f9040-f791-4549-b0f2-08ed67ac3df2','SerialNumber':'0785360e0','UUID':'00000000-0000-0000-0000-000000005088','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.88'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/6f4f9040-f791-4549-b0f2-08ed67ac3df2','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'f54763c7-daf5-459c-bcec-e12c291cb191','SerialNumber':'0baebbc94','UUID':'00000000-0000-0000-0000-000000005089','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.89'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/f54763c7-daf5-459c-bcec-e12c291cb191','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'b526c42e-2b57-4a01-852a-f15445500841','SerialNumber':'06e0f55fa','UUID':'00000000-0000-0000-0000-000000005090','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.90'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/b526c42e-2b57-4a01-852a-f15445500841','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'c4749e18-ed7c-413d-bde6-630cf50cc9f7','SerialNumber':'00dad0ef6','UUID':'00000000-0000-0000-0000-000000005091','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.91'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/c4749e18-ed7c-413d-bde6-630cf50cc9f7','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'71fb3830-4b1e-45ad-9ff5-21850feab72e','SerialNumber':'05215df66','UUID':'00000000-0000-0000-0000-000000005092','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.92'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/71fb3830-4b1e-45ad-9ff5-21850feab72e','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'32289461-853f-45f9-a03a-a57067c0967f','SerialNumber':'01bff9256','UUID':'00000000-0000-0000-0000-000000005093','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.93'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/32289461-853f-45f9-a03a-a57067c0967f','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'e073a644-5920-4c72-97b6-f0453bc42e28','SerialNumber':'0d5be3bc5','UUID':'00000000-0000-0000-0000-000000005094','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.94'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/e073a644-5920-4c72-97b6-f0453bc42e28','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'109df83c-da49-47b9-9c40-81f98a2a5d30','SerialNumber':'08bfd7fb1','UUID':'00000000-0000-0000-0000-000000005095','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.95'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/109df83c-da49-47b9-9c40-81f98a2a5d30','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'8a86dc74-8f9b-4532-b1a6-f8250b0fddac','SerialNumber':'09db46b3c','UUID':'00000000-0000-0000-0000-000000005096','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.96'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/8a86dc74-8f9b-4532-b1a6-f8250b0fddac','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'0f8a3f16-bb1c-44b9-8028-513feb6a9016','SerialNumber':'09736abbb','UUID':'00000000-0000-0000-0000-000000005097','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.97'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/0f8a3f16-bb1c-44b9-8028-513feb6a9016','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'0afb8075-0f95-4902-8e9c-16600a39152b','SerialNumber':'04fa2126b','UUID':'00000000-0000-0000-0000-000000005098','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.98'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/0afb8075-0f95-4902-8e9c-16600a39152b','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'7bca5ec3-ad45-4a87-88ca-ede5b4f135e4','SerialNumber':'09faf8291','UUID':'00000000-0000-0000-0000-000000005099','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.99'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/7bca5ec3-ad45-4a87-88ca-ede5b4f135e4','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'95e5f73a-f0be-463b-9143-81abd62e8820','SerialNumber':'0426ee84c','UUID':'00000000-0000-0000-0000-000000005100','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.100'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/95e5f73a-f0be-463b-9143-81abd62e8820','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'5d87c36e-0335-4697-b794-663ddab76f9d','SerialNumber':'0c2cf82c7','UUID':'00000000-0000-0000-0000-000000005101','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.101'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/5d87c36e-0335-4697-b794-663ddab76f9d','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'4c406081-64d1-4a43-8a86-5515439eb6d4','SerialNumber':'0895ab6de','UUID':'00000000-0000-0000-0000-000000005102','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.102'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/4c406081-64d1-4a43-8a86-5515439eb6d4','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'0cd7f7df-24e8-4271-bc97-6b0302ee54bb','SerialNumber':'06c596ce1','UUID':'00000000-0000-0000-0000-000000005103','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.103'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/0cd7f7df-24e8-4271-bc97-6b0302ee54bb','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'bf668c8f-f338-498e-9a54-3f6f59565b34','SerialNumber':'0687b1ad9','UUID':'00000000-0000-0000-0000-000000005104','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.104'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/bf668c8f-f338-498e-9a54-3f6f59565b34','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'d69f2ccf-3097-4194-8ae1-994271efcbc9','SerialNumber':'04bae3d89','UUID':'00000000-0000-0000-0000-000000005105','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.105'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/d69f2ccf-3097-4194-8ae1-994271efcbc9','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'ba3f7da6-314b-46b6-b795-ee9857d55c91','SerialNumber':'0750e5320','UUID':'00000000-0000-0000-0000-000000005106','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.106'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/ba3f7da6-314b-46b6-b795-ee9857d55c91','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'f90ba261-d517-430a-af29-a88a0db9f4e4','SerialNumber':'0342619cd','UUID':'00000000-0000-0000-0000-000000005107','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.107'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/f90ba261-d517-430a-af29-a88a0db9f4e4','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'c1f5e96e-dcef-4dd3-a4d3-8b42b7cc885f','SerialNumber':'0219e80b8','UUID':'00000000-0000-0000-0000-000000005108','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.108'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/c1f5e96e-dcef-4dd3-a4d3-8b42b7cc885f','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'eeca06cf-8b62-4289-b765-09325b80489d','SerialNumber':'0b8b3455b','UUID':'00000000-0000-0000-0000-000000005109','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.109'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/eeca06cf-8b62-4289-b765-09325b80489d','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'ec3a008c-5940-4529-af37-a32fe3d86a87','SerialNumber':'040162073','UUID':'00000000-0000-0000-0000-000000005110','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.110'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/ec3a008c-5940-4529-af37-a32fe3d86a87','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'38e1810e-ed4c-4cc2-9bb6-7402252e809a','SerialNumber':'0fb662d80','UUID':'00000000-0000-0000-0000-000000005111','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.111'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/38e1810e-ed4c-4cc2-9bb6-7402252e809a','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'579c6e71-ec29-4caa-bf15-04bee700bf53','SerialNumber':'07f9298a3','UUID':'00000000-0000-0000-0000-000000005112','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.112'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/579c6e71-ec29-4caa-bf15-04bee700bf53','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'f8a87696-6fc8-43d1-9179-1653c1b59196','SerialNumber':'0bda9cf9d','UUID':'00000000-0000-0000-0000-000000005113','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.113'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/f8a87696-6fc8-43d1-9179-1653c1b59196','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'bc5ecc11-5b37-4e7d-a178-f9c21ca2c467','SerialNumber':'0de2dd3d0','UUID':'00000000-0000-0000-0000-000000005114','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.114'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/bc5ecc11-5b37-4e7d-a178-f9c21ca2c467','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'4d2cffbe-e95f-4109-be38-2b9c586f46d2','SerialNumber':'0d6763f00','UUID':'00000000-0000-0000-0000-000000005115','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.115'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/4d2cffbe-e95f-4109-be38-2b9c586f46d2','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'b92ed198-1334-43d5-b95d-d09dca748f60','SerialNumber':'00061bb86','UUID':'00000000-0000-0000-0000-000000005116','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.116'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/b92ed198-1334-43d5-b95d-d09dca748f60','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'664bdceb-ab8c-496c-94c8-3384b5630168','SerialNumber':'08f27d73e','UUID':'00000000-0000-0000-0000-000000005117','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.117'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/664bdceb-ab8c-496c-94c8-3384b5630168','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'33a1b42e-fa03-46d7-87bc-f952469aacf1','SerialNumber':'091c00a5e','UUID':'00000000-0000-0000-0000-000000005118','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.118'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/33a1b42e-fa03-46d7-87bc-f952469aacf1','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'e0ac544d-fa31-4941-a589-7afd165a57dc','SerialNumber':'0a9255263','UUID':'00000000-0000-0000-0000-000000005119','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.119'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/e0ac544d-fa31-4941-a589-7afd165a57dc','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'6932e96a-101a-415f-9b18-221f84fc20fe','SerialNumber':'090632a62','UUID':'00000000-0000-0000-0000-000000005120','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.120'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/6932e96a-101a-415f-9b18-221f84fc20fe','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'1a3e4119-c2d8-49e3-84ab-230cf99b9f37','SerialNumber':'00931aef6','UUID':'00000000-0000-0000-0000-000000005121','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.121'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/1a3e4119-c2d8-49e3-84ab-230cf99b9f37','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'bc1adc95-8a19-4d41-84c3-33ff350cb6f1','SerialNumber':'0f5e31dfa','UUID':'00000000-0000-0000-0000-000000005122','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.122'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/bc1adc95-8a19-4d41-84c3-33ff350cb6f1','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'1475977c-cd4e-4c30-9d83-5aaf7b01a435','SerialNumber':'03267dcd7','UUID':'00000000-0000-0000-0000-000000005123','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.123'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/1475977c-cd4e-4c30-9d83-5aaf7b01a435','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'f739d653-b5a1-4599-986c-4ad35a5ee2ae','SerialNumber':'0332426cb','UUID':'00000000-0000-0000-0000-000000005124','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.124'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/f739d653-b5a1-4599-986c-4ad35a5ee2ae','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'2d4f3881-9082-47f2-84ed-1bb9aad5a19b','SerialNumber':'0d28ebea2','UUID':'00000000-0000-0000-0000-000000005125','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.125'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/2d4f3881-9082-47f2-84ed-1bb9aad5a19b','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'3ae32ab5-e041-4ad5-b946-2692c40a1c12','SerialNumber':'03eeb0a40','UUID':'00000000-0000-0000-0000-000000005126','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.126'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/3ae32ab5-e041-4ad5-b946-2692c40a1c12','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'8bdf8a81-c182-4e74-8909-8400e8758d1c','SerialNumber':'092da0d32','UUID':'00000000-0000-0000-0000-000000005127','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.127'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/8bdf8a81-c182-4e74-8909-8400e8758d1c','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'d818d3c0-b726-40e5-8067-bab5bfe94ec9','SerialNumber':'02c2a983b','UUID':'00000000-0000-0000-0000-000000005128','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.128'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/d818d3c0-b726-40e5-8067-bab5bfe94ec9','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'f283be2a-9c7b-4ef0-881a-7931afbe1834','SerialNumber':'0c7b4a5ae','UUID':'00000000-0000-0000-0000-000000005129','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.129'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/f283be2a-9c7b-4ef0-881a-7931afbe1834','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'329ae4f8-fcda-4830-83da-017242128d5b','SerialNumber':'020f0e8b6','UUID':'00000000-0000-0000-0000-000000005130','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.130'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/329ae4f8-fcda-4830-83da-017242128d5b','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'30829ed0-63ab-4b13-ae06-8cc2dbfdb886','SerialNumber':'01dc3dd74','UUID':'00000000-0000-0000-0000-000000005131','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.131'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/30829ed0-63ab-4b13-ae06-8cc2dbfdb886','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'f401fba7-5121-49ea-a48b-a279db5f94b6','SerialNumber':'094ef002b','UUID':'00000000-0000-0000-0000-000000005132','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.132'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/f401fba7-5121-49ea-a48b-a279db5f94b6','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'7a26ec13-fff1-48cb-a79b-8e79fd94531d','SerialNumber':'00d0f4b21','UUID':'00000000-0000-0000-0000-000000005133','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.133'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/7a26ec13-fff1-48cb-a79b-8e79fd94531d','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'117afd96-31a3-46b8-ba73-a31ab2626101','SerialNumber':'0b7359b27','UUID':'00000000-0000-0000-0000-000000005134','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.134'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/117afd96-31a3-46b8-ba73-a31ab2626101','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'f583ab06-99d3-48f6-8aa4-4040f78ef2cb','SerialNumber':'0141370c3','UUID':'00000000-0000-0000-0000-000000005135','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.135'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/f583ab06-99d3-48f6-8aa4-4040f78ef2cb','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'e8ba3b85-38ac-48b7-a61f-c0678533d96c','SerialNumber':'01da1aa58','UUID':'00000000-0000-0000-0000-000000005136','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.136'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/e8ba3b85-38ac-48b7-a61f-c0678533d96c','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'0ea71515-6db3-41b8-a103-e28b993eaf64','SerialNumber':'065b33a51','UUID':'00000000-0000-0000-0000-000000005137','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.137'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/0ea71515-6db3-41b8-a103-e28b993eaf64','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'6304ed69-bd54-43df-baa4-3b47bf48c277','SerialNumber':'0d952de09','UUID':'00000000-0000-0000-0000-000000005138','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.138'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/6304ed69-bd54-43df-baa4-3b47bf48c277','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'0758b43e-53bc-43a2-820d-7d45bb4ed490','SerialNumber':'06471426d','UUID':'00000000-0000-0000-0000-000000005139','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.139'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/0758b43e-53bc-43a2-820d-7d45bb4ed490','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'daeffefc-6b22-4ff9-b45e-9551e024fb10','SerialNumber':'0e4713876','UUID':'00000000-0000-0000-0000-000000005140','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.140'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/daeffefc-6b22-4ff9-b45e-9551e024fb10','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'dc6896e6-a914-4e48-85a3-7185f6ad2fbc','SerialNumber':'068016e69','UUID':'00000000-0000-0000-0000-000000005141','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.141'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/dc6896e6-a914-4e48-85a3-7185f6ad2fbc','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'f36eaba1-8d9c-494a-a4bf-2d94524a5813','SerialNumber':'0b728c553','UUID':'00000000-0000-0000-0000-000000005142','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.142'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/f36eaba1-8d9c-494a-a4bf-2d94524a5813','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'b136ad6c-53ca-460d-8813-0303661eff0d','SerialNumber':'008e75961','UUID':'00000000-0000-0000-0000-000000005143','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.143'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/b136ad6c-53ca-460d-8813-0303661eff0d','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'0fa75321-88b0-4417-b486-457b4ab5b2b7','SerialNumber':'01732f0be','UUID':'00000000-0000-0000-0000-000000005144','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.144'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/0fa75321-88b0-4417-b486-457b4ab5b2b7','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'3e8509c4-c81f-4444-884a-30631f19fa1f','SerialNumber':'0b9c414ce','UUID':'00000000-0000-0000-0000-000000005145','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.145'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/3e8509c4-c81f-4444-884a-30631f19fa1f','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'899ae965-eb42-43d7-b3af-276996f51a8f','SerialNumber':'0cac6c01f','UUID':'00000000-0000-0000-0000-000000005146','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.146'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/899ae965-eb42-43d7-b3af-276996f51a8f','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'0f915140-5327-4d7a-8a99-e158aa3bcf64','SerialNumber':'07d800859','UUID':'00000000-0000-0000-0000-000000005147','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.147'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/0f915140-5327-4d7a-8a99-e158aa3bcf64','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'23841ce8-ed59-412a-82ce-46036265b150','SerialNumber':'00a7e86be','UUID':'00000000-0000-0000-0000-000000005148','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.148'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/23841ce8-ed59-412a-82ce-46036265b150','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'7451a98b-496a-4493-8a7d-1a0cb14c388d','SerialNumber':'0d7976294','UUID':'00000000-0000-0000-0000-000000005149','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.149'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/7451a98b-496a-4493-8a7d-1a0cb14c388d','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'6cf78af9-395c-46ee-b106-5b19238ecef6','SerialNumber':'08315fe0f','UUID':'00000000-0000-0000-0000-000000005150','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.150'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/6cf78af9-395c-46ee-b106-5b19238ecef6','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'ba19e34c-44cf-4dde-a120-561ec1c0b82e','SerialNumber':'04d1127da','UUID':'00000000-0000-0000-0000-000000005151','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.151'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/ba19e34c-44cf-4dde-a120-561ec1c0b82e','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'2b0a7ff6-50dc-40c7-8879-e9c3476c6a56','SerialNumber':'0dc27a77c','UUID':'00000000-0000-0000-0000-000000005152','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.152'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/2b0a7ff6-50dc-40c7-8879-e9c3476c6a56','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'35802751-db4d-4ca2-ab32-2a84cf2fb02c','SerialNumber':'0103cc66c','UUID':'00000000-0000-0000-0000-000000005153','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.153'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/35802751-db4d-4ca2-ab32-2a84cf2fb02c','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'cfa57b3a-38be-47f1-8b7a-7458e18c2c27','SerialNumber':'0e39c149e','UUID':'00000000-0000-0000-0000-000000005154','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.154'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/cfa57b3a-38be-47f1-8b7a-7458e18c2c27','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'5f59ad50-fec0-447b-8ae8-f1d615365d7e','SerialNumber':'04e3b4f7a','UUID':'00000000-0000-0000-0000-000000005155','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.155'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/5f59ad50-fec0-447b-8ae8-f1d615365d7e','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'b7bdcb3b-dcd6-498f-add4-40f416e7e918','SerialNumber':'01731463f','UUID':'00000000-0000-0000-0000-000000005156','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.156'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/b7bdcb3b-dcd6-498f-add4-40f416e7e918','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'2747d930-3a6e-452f-8b2c-d3763d82b294','SerialNumber':'0b32a9f1e','UUID':'00000000-0000-0000-0000-000000005157','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.157'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/2747d930-3a6e-452f-8b2c-d3763d82b294','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'dea09a11-b923-4e29-a56d-7ab94aeae6b5','SerialNumber':'07ef75dd3','UUID':'00000000-0000-0000-0000-000000005158','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.158'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/dea09a11-b923-4e29-a56d-7ab94aeae6b5','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'4322858c-591a-4bff-9adf-33c100a27895','SerialNumber':'00f2085cd','UUID':'00000000-0000-0000-0000-000000005159','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.159'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/4322858c-591a-4bff-9adf-33c100a27895','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'9307db4e-61c5-4532-a7e5-48ad148d915a','SerialNumber':'00916b0a1','UUID':'00000000-0000-0000-0000-000000005160','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.160'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/9307db4e-61c5-4532-a7e5-48ad148d915a','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'494412d8-fb42-4602-9bab-408c598e16bf','SerialNumber':'06af8f2c0','UUID':'00000000-0000-0000-0000-000000005161','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.161'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/494412d8-fb42-4602-9bab-408c598e16bf','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'41b3d4ed-26bd-45a9-a3db-1066429d1da8','SerialNumber':'000bba914','UUID':'00000000-0000-0000-0000-000000005162','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.162'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/41b3d4ed-26bd-45a9-a3db-1066429d1da8','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'9c1e3a5b-5b15-4e2d-998c-3c05026c6bd7','SerialNumber':'0789c2ef4','UUID':'00000000-0000-0000-0000-000000005163','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.163'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/9c1e3a5b-5b15-4e2d-998c-3c05026c6bd7','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'9e544851-d694-450c-b90d-050bc9ddd6e4','SerialNumber':'053a16a03','UUID':'00000000-0000-0000-0000-000000005164','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.164'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/9e544851-d694-450c-b90d-050bc9ddd6e4','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'06339ede-ca77-4423-b607-7ebe9c01d784','SerialNumber':'0b7cff766','UUID':'00000000-0000-0000-0000-000000005165','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.165'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/06339ede-ca77-4423-b607-7ebe9c01d784','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'ed12c461-e00c-423b-8458-d5ca35a0ba03','SerialNumber':'06c9ce2ca','UUID':'00000000-0000-0000-0000-000000005166','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.166'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/ed12c461-e00c-423b-8458-d5ca35a0ba03','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'e2291a81-3f18-4d78-98c7-9a88ceeffcc0','SerialNumber':'04211381d','UUID':'00000000-0000-0000-0000-000000005167','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.167'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/e2291a81-3f18-4d78-98c7-9a88ceeffcc0','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'2478092c-ae2c-40c1-8276-ba725e3dd2d9','SerialNumber':'07a13ffe4','UUID':'00000000-0000-0000-0000-000000005168','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.168'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/2478092c-ae2c-40c1-8276-ba725e3dd2d9','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'435e340d-a0f6-4179-908c-b70ba3184960','SerialNumber':'086366cae','UUID':'00000000-0000-0000-0000-000000005169','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.169'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/435e340d-a0f6-4179-908c-b70ba3184960','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'1f57ca14-9b75-4655-a399-b83fec8dc3ff','SerialNumber':'01ab9ddf1','UUID':'00000000-0000-0000-0000-000000005170','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.170'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/1f57ca14-9b75-4655-a399-b83fec8dc3ff','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'e078d127-c918-48a0-bba1-5eae43c9a257','SerialNumber':'093e429c0','UUID':'00000000-0000-0000-0000-000000005171','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.171'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/e078d127-c918-48a0-bba1-5eae43c9a257','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'61e2a460-dbc1-4b36-a2e0-80d4db189d76','SerialNumber':'04a1bc6d5','UUID':'00000000-0000-0000-0000-000000005172','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.172'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/61e2a460-dbc1-4b36-a2e0-80d4db189d76','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'ef8d174f-b553-4c10-8828-91c22ae51fae','SerialNumber':'0dfcb5070','UUID':'00000000-0000-0000-0000-000000005173','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.173'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/ef8d174f-b553-4c10-8828-91c22ae51fae','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'f73bb8ae-9cb1-41eb-b043-89d8931661bd','SerialNumber':'0678b5a57','UUID':'00000000-0000-0000-0000-000000005174','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.174'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/f73bb8ae-9cb1-41eb-b043-89d8931661bd','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'43e2e850-39df-41db-bd5c-8e75c367ca1f','SerialNumber':'033eb4b76','UUID':'00000000-0000-0000-0000-000000005175','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.175'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/43e2e850-39df-41db-bd5c-8e75c367ca1f','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'1271155a-4dfa-4335-820c-a4da2ef646e4','SerialNumber':'041718f07','UUID':'00000000-0000-0000-0000-000000005176','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.176'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/1271155a-4dfa-4335-820c-a4da2ef646e4','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'d2cc0791-beea-4af7-8a1e-8e579c00c573','SerialNumber':'07cde8e05','UUID':'00000000-0000-0000-0000-000000005177','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.177'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/d2cc0791-beea-4af7-8a1e-8e579c00c573','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'c71dfcc6-3b86-4f24-92b0-70d373b7b130','SerialNumber':'027b938cf','UUID':'00000000-0000-0000-0000-000000005178','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.178'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/c71dfcc6-3b86-4f24-92b0-70d373b7b130','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'622c88ef-f969-4ff7-ad1e-fec8bba9045f','SerialNumber':'082db09f5','UUID':'00000000-0000-0000-0000-000000005179','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.179'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/622c88ef-f969-4ff7-ad1e-fec8bba9045f','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'b88fd247-ee9c-4871-afe2-d802436240de','SerialNumber':'05edffeb5','UUID':'00000000-0000-0000-0000-000000005180','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.180'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/b88fd247-ee9c-4871-afe2-d802436240de','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'a20984a7-d3d2-471d-9f4f-e6681b39663c','SerialNumber':'01b317fdd','UUID':'00000000-0000-0000-0000-000000005181','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.181'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/a20984a7-d3d2-471d-9f4f-e6681b39663c','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'4bc9f560-70df-4895-ad50-75bf440f9a6e','SerialNumber':'022d21e3e','UUID':'00000000-0000-0000-0000-000000005182','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.182'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/4bc9f560-70df-4895-ad50-75bf440f9a6e','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'f61491c8-6a88-4fb8-89b8-a175091a792a','SerialNumber':'09c2ed0ff','UUID':'00000000-0000-0000-0000-000000005183','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.183'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/f61491c8-6a88-4fb8-89b8-a175091a792a','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'4ef44339-960b-42cf-a955-776fce57c578','SerialNumber':'0b0b9e8ff','UUID':'00000000-0000-0000-0000-000000005184','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.184'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/4ef44339-960b-42cf-a955-776fce57c578','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'bba53d81-376a-413a-a686-154445a90d25','SerialNumber':'0bcd5e389','UUID':'00000000-0000-0000-0000-000000005185','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.185'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/bba53d81-376a-413a-a686-154445a90d25','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'fbf20876-96ff-4fc7-9948-95e6610ea5f4','SerialNumber':'0e82c7cd7','UUID':'00000000-0000-0000-0000-000000005186','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.186'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/fbf20876-96ff-4fc7-9948-95e6610ea5f4','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'beada369-757d-4315-b215-128aa60b8613','SerialNumber':'08f0033bb','UUID':'00000000-0000-0000-0000-000000005187','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.187'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/beada369-757d-4315-b215-128aa60b8613','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'9d13fa73-6db8-4d4b-936a-b453ab1ada88','SerialNumber':'020fc4afa','UUID':'00000000-0000-0000-0000-000000005188','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.188'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/9d13fa73-6db8-4d4b-936a-b453ab1ada88','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'c78c5d9d-e74a-42fd-af37-c829a54a89e9','SerialNumber':'0489e22bb','UUID':'00000000-0000-0000-0000-000000005189','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.189'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/c78c5d9d-e74a-42fd-af37-c829a54a89e9','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'a695da3f-ee50-4b26-866b-438072b2108f','SerialNumber':'03589b519','UUID':'00000000-0000-0000-0000-000000005190','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.190'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/a695da3f-ee50-4b26-866b-438072b2108f','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'576a8935-7584-4541-8e79-80ea2c15a939','SerialNumber':'098820296','UUID':'00000000-0000-0000-0000-000000005191','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.191'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/576a8935-7584-4541-8e79-80ea2c15a939','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'85bb958e-bf58-4091-ab91-cf127c789ddd','SerialNumber':'07dd267c4','UUID':'00000000-0000-0000-0000-000000005192','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.192'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/85bb958e-bf58-4091-ab91-cf127c789ddd','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'77180bd5-4831-451d-b714-4db44c6f0bdf','SerialNumber':'01d8c0512','UUID':'00000000-0000-0000-0000-000000005193','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.193'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/77180bd5-4831-451d-b714-4db44c6f0bdf','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'39c96caa-9b54-4f65-ae4f-5520bebf1d45','SerialNumber':'0b00cfe9d','UUID':'00000000-0000-0000-0000-000000005194','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.194'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/39c96caa-9b54-4f65-ae4f-5520bebf1d45','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'debe13d4-90d7-448e-804b-643869a42c5f','SerialNumber':'024775370','UUID':'00000000-0000-0000-0000-000000005195','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.195'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/debe13d4-90d7-448e-804b-643869a42c5f','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'dfb92ff9-233e-4a5f-8f3b-799670d708fb','SerialNumber':'02c86c331','UUID':'00000000-0000-0000-0000-000000005196','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.196'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/dfb92ff9-233e-4a5f-8f3b-799670d708fb','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'46a9edb4-d0c9-499b-b01a-d87b8c4130dd','SerialNumber':'009e87b9f','UUID':'00000000-0000-0000-0000-000000005197','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.197'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/46a9edb4-d0c9-499b-b01a-d87b8c4130dd','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'5024fee4-c4c2-47a6-8397-34647172f9e0','SerialNumber':'0df3304db','UUID':'00000000-0000-0000-0000-000000005198','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.198'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/5024fee4-c4c2-47a6-8397-34647172f9e0','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'cb29b64c-60bf-488b-839f-047c7a478396','SerialNumber':'0d6ef936e','UUID':'00000000-0000-0000-0000-000000005199','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.199'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/cb29b64c-60bf-488b-839f-047c7a478396','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]},{'DeviceID':'2e2f0346-05cd-4f84-8e6a-7db46c96f8ae','SerialNumber':'0768204e0','UUID':'00000000-0000-0000-0000-000000005200','Name':'Computer System','Model':'RH2288H V3','Group':null,'PowerState':'On','IPv4Address':{'Address':'172.171.190.200'},'ProcessorSummary':{'Count':2},'StorageSummary':{'TotalSystemStorageGiB':5580,'Count':0},'MemorySummary':{'TotalSystemMemoryGiB':384},'Profile':{'Id':'','Name':'','State':'Unbinding'},'Status':{'Health':'Critical','State':'Enabled'},'@odata.id':'/redfish/v1/rich/Nodes/2e2f0346-05cd-4f84-8e6a-7db46c96f8ae','Alias':'','Tag':'','ServerState':'OnLine','ScopeURIs':[]}],'Members@odata.count':200}";
            return JsonConvert.DeserializeObject<ServerList>(t);
        }
        public async Task MockSyncServer()
        {
            //var result = await nodePoolService.GetServerCollectionAsync(null, null, null, null, null);
            var result = t();
            //var serverList = new List<ServerSummary>();
            //for (int i = 0; i < 1; i++)
            //{
            //    serverList.Add(result.Members.FirstOrDefault());
            //}
            foreach (var x in result.Members)
            {
                Console.WriteLine("-- " + DateTime.Now.ToString("HH:mm:ss fff") + " " + Thread.CurrentThread.ManagedThreadId);
                await this.QueryServerDetails(x).ContinueWith(y =>
                 {
                     //y.Result.ODataId = Guid.NewGuid().ToString();
                     y.Result.Status.Health = Health.Critical;
                     var manager = QueryManager(x.Id).Result;
                     y.Result.BMCVersion = manager.FirmwareVersion;
                     y.Result.HostName = manager.EthernetInterfaces.FirstOrDefault()?.HostName ?? string.Empty;//如果为null ,取空
                     Console.WriteLine("++ " + DateTime.Now.ToString("HH:mm:ss fff") + " " + Thread.CurrentThread.ManagedThreadId);
                     ServerConnector.Instance.Sync(y.Result).Wait();
                     Console.WriteLine("** " + DateTime.Now.ToString("HH:mm:ss fff") + " " + Thread.CurrentThread.ManagedThreadId);
                 });
                if (x.State.ToUpper() == "ONLINE")
                {
                    DealServerPerformance(x).Wait();
                }
            }
        }

        public async Task MockSyncEnclosure()
        {
            var result = await this.enclosureService.GetEnclosureCollectionAsync(null, null, null, null);
            var enclosureList = new List<EnclosureSummary>();
            for (int i = 0; i < 10; i++)
            {
                enclosureList.Add(result.Members.First());
            }
            foreach (var x in enclosureList)
            {
                var task = taskFactory.StartNew(() =>
                {
                    Console.WriteLine("-- " + DateTime.Now.ToString("HH:mm:ss fff") + " " + Thread.CurrentThread.ManagedThreadId);
                    this.QueryEnclosureDetails(x).ContinueWith(y =>
                    {


                        Console.WriteLine("++ " + DateTime.Now.ToString("HH:mm:ss fff") + " " + Thread.CurrentThread.ManagedThreadId);
                        EnclosureConnector.Instance.Sync(y.Result).Wait();
                        Console.WriteLine("** " + DateTime.Now.ToString("HH:mm:ss fff") + " " + Thread.CurrentThread.ManagedThreadId);
                    });
                }, cts.Token);
            }
        }
        private async Task<ServerManagerCollection> QueryManager(string serverId)
        {
            try
            {
                var manager = await this.nodePoolService.GetServerManagerCollectionAsync(serverId);
                return manager;
            }
            catch (Exception ex)
            {
                OnPollingError($"QueryBMCVersion Error:[ServerId:{serverId}]", ex);
                //throw;
                return null;
            }
        }
        #region Enclosure

        private async Task<Enclosure> QueryEnclosureDetails(EnclosureSummary enclosureSummary)
        {
            var enclosureId = enclosureSummary.UUID;
            try
            {
                return await QueryEnclosureDetailsById(enclosureId);
            }
            catch (Exception ex)
            {
                OnPollingError($"QueryEnclosureDetial Error:{enclosureId}", ex);
                var defaultEnclosure = new Enclosure(enclosureSummary);
                defaultEnclosure.MakeDetails(this.FusionDirectorIp);
                return defaultEnclosure;
            }
        }

        private void OnPollingError(string v, Exception ex)
        {
        }

        /// <summary>
        /// Queries the Enclosure detial.
        /// </summary>
        /// <returns>Task&lt;Enclosure&gt;.</returns>
        private async Task<Enclosure> QueryEnclosureDetailsById(string deviceId)
        {
            var enclosure = await this.enclosureService.GetEnclosureAsync(deviceId);
            enclosure.MakeDetails(this.FusionDirectorIp);
            return enclosure;
        }

        #endregion

        #region Server

        /// <summary>
        /// Queries the Server detial.
        /// </summary>
        /// <param name="serverSummary">The server summary.</param>
        /// <returns>Task&lt;Server&gt;.</returns>
        private async Task<Server> QueryServerDetails(ServerSummary serverSummary)
        {
            var serverId = serverSummary.Id;
            try
            {
                return await QueryServerDetailsById(serverId);
            }
            catch (Exception)
            {
                var defaultServer = new Server(serverSummary);
                defaultServer.MakeDetails(this.FusionDirectorIp);
                return defaultServer;
            }
        }

        private async Task<Server> QueryServerDetailsById(string serverId)
        {
            var server = await this.nodePoolService.GetServerInfoAsync(serverId);
            Console.WriteLine(" 1" + " " + Thread.CurrentThread.ManagedThreadId + DateTime.Now.ToString("HH:mm:ss fff"));
            server.MakeDetails(this.FusionDirectorIp);
            return server;
        }

        #endregion

        private async void btnInsertServerSummary_Click(object sender, EventArgs e)
        {
            var template = @"{
		'DeviceID': '174c9dea-b72e-446b-a02e-6dbed5448fd8',
		'SerialNumber': '0e4d0f0ca',
		'UUID': '00000000-0000-0000-0000-000000005001',
		'Name': 'Computer System',
		'Model': 'RH2288H V3',
		'Group': null,
		'PowerState': 'On',
		'IPv4Address': {
			'Address': '172.171.190.1'
		},
		'ProcessorSummary': {
			'Count': 2
		},
		'StorageSummary': {
			'TotalSystemStorageGiB': 5580,
			'Count': 0
		},
		'MemorySummary': {
			'TotalSystemMemoryGiB': 384
		},
		'Profile': {
			'Id': '',
			'Name': '',
			'State': 'Unbinding'
		},
		'Status': {
			'Health': 'Critical',
			'State': 'Enabled'
		},
		'@odata.id': '/redfish/v1/rich/Nodes/174c9dea-b72e-446b-a02e-6dbed5448fd8',
		'Alias': '',
		'Tag': '',
		'ServerState': 'OnLine',
		'ScopeURIs': []
	}";
            var serverSummary = JsonConvert.DeserializeObject<ServerSummary>(template);
            var defaultServer = new Server(serverSummary);
            defaultServer.MakeDetails(this.FusionDirectorIp);
            var manager = await QueryManager(defaultServer.Id);
            defaultServer.BMCVersion = manager.FirmwareVersion;
            defaultServer.HostName = manager.EthernetInterfaces.FirstOrDefault()?.HostName ?? string.Empty;//如果为null ,取空
            ServerConnector.Instance.Sync(defaultServer).Wait();


        }
    }
}
