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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using FusionDirectorPlugin.Core.Models;
using FusionDirectorPlugin.Model;
// ReSharper disable StyleCop.SA1600
// ReSharper disable StyleCop.SA1300

namespace FusionDirectorPlugin.TestClient
{
    public partial class FormGen : Form
    {
        public FormGen()
        {
            this.InitializeComponent();
        }

        #region Common

        private void GenClass(string className, string folder, PropertyInfo[] propertys)
        {
            var fields = GetFields(className, propertys);
            var result = this.GenClassMpx(className, fields).Replace("'", "\"");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            doc.Save($"{folder}/{className}.mpx");
        }

        private void GenGroup(string className, string folder)
        {
            var template =
                $@"<ManagementPackFragment SchemaVersion='2.0' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                  <TypeDefinitions>
                    <EntityTypes>
                      <ClassTypes>
                        <ClassType ID='{
                        className
                    }Group' Base='FVL!FusionDirector.BaseGroup' Accessibility='Internal' Abstract='false' Hosted='true' Singleton='false'> </ClassType>
                      </ClassTypes>
                      <RelationshipTypes>
                        <RelationshipType ID='{
                        className
                    }Group.Relationship' Accessibility='Public' Base='System!System.Hosting'>
                          <Source ID='Source' Type='{className.Substring(0, className.LastIndexOf("."))}' />
                          <Target ID='Target' Type='{className}Group' />
                        </RelationshipType>
                      </RelationshipTypes>
                    </EntityTypes>
                  </TypeDefinitions>
                  <Presentation>
                    <ImageReferences>
                      <ImageReference ElementID='{className}Group' ImageID='FVL!{className}.Diagram'/>
                      <ImageReference ElementID='{className}Group' ImageID='FVL!{className}.Small'/>
                    </ImageReferences>
                  </Presentation>
                  <LanguagePacks>
                    <LanguagePack ID='ENU' IsDefault='true'>
                      <DisplayStrings>
                        <DisplayString ElementID='{className}Group'>
                          <Name>{GetName(className)} Group</Name>
                          <Description>{className.Replace("Slot", "").Replace(".", " ")}Group</Description>
                        </DisplayString>
                      </DisplayStrings>
                    </LanguagePack>
                  </LanguagePacks>
                </ManagementPackFragment>";

            var result = template.Replace("'", "\"");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            doc.Save($"{folder}/{className}Group.mpx");
        }

        private string GenClassMpx(string className, List<string> fields)
        {
            var template =
                $@"<ManagementPackFragment SchemaVersion='2.0' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
              <TypeDefinitions>
                <EntityTypes>
                  <ClassTypes>
                    <ClassType ID='{
                        className
                    }' Base='System!System.PhysicalEntity' Accessibility='Public' Abstract='false' Hosted='true' Singleton='false'>
                     {this.GenFields(fields)}
                    </ClassType>
                  </ClassTypes>
                  <RelationshipTypes>
                    <RelationshipType ID='{
                        className
                    }.Relationship' Accessibility='Public' Base='System!System.Hosting'>
                      <Source ID='Source' Type='{className}Group' />
                      <Target ID='Target' Type='{className}' />
                    </RelationshipType>
                  </RelationshipTypes>
                </EntityTypes>
              </TypeDefinitions>
              <Presentation>
                <ImageReferences>
                  <ImageReference ElementID='{className}' ImageID='FVL!{className}.Diagram'/>
                  <ImageReference ElementID='{className}' ImageID='FVL!{className}.Small'/>
                </ImageReferences>
              </Presentation>
              <LanguagePacks>
                <LanguagePack ID='ENU' IsDefault='true'>
                  <DisplayStrings>
                   {this.GenLangs(className, fields)}
                  </DisplayStrings>
                </LanguagePack>
              </LanguagePacks>
            </ManagementPackFragment>";
            return template;
        }

        private string GenFields(List<string> fields)
        {
            var result = string.Empty;
            fields.ForEach(x =>
            {
                result +=
                    $"<Property ID='{x}' Type='string' Key='{("UnionId" == x).ToString().ToLower()}' CaseSensitive='false' MaxLength='256' MinLength='0' />";
            });
            return result;
        }

        private string GenLangs(string className, List<string> fields)
        {
            var name = GetName(className);
            var result = $@"<DisplayString ElementID='{className}'>
                      <Name>{name}</Name>
                      <Description>{className.Replace(".", " ")}</Description>
                    </DisplayString>";
            fields.ForEach(x =>
            {
                result += $@"<DisplayString ElementID='{className}' SubElementID='{x}'>
                      <Name>{x}</Name>
                      <Description>{x}</Description>
                    </DisplayString>";
            });
            return result;
        }

        private void GenAll(string className, PropertyInfo[] propertys)
        {
            this.GenClass(className, "Classes", propertys);
            this.GenGroup(className, "Groups");
            this.GenMonitor(className, "Monitors/Classes");
            this.GenGroupMonitor(className, "Monitors/Groups");
        }

        private void GenIcon(string className)
        {
            var template = $@"<ManagementPackFragment SchemaVersion='2.0' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
              <Categories>
                <Category ID='{className}.Diagram.Category' Target='{className}.Diagram' Value='System!System.Internal.ManagementPack.Images.DiagramIcon' />
                <Category ID='{className}.Small.Category' Target='{className}.Small' Value='System!System.Internal.ManagementPack.Images.u16x16Icon' />
              </Categories>
              <Resources>
                <Image ID='{className}.Diagram' FileName='{className}_80.png' Accessibility='Public' HasNullStream='false' Comment='{className.Replace(".", " ")} Image Diagram' />
                <Image ID='{className}.Small' FileName='{className}_16.png' Accessibility='Public' HasNullStream='false' Comment='{className.Replace(".", " ")} Image Small' />
              </Resources>
            </ManagementPackFragment>";
            var result = template.Replace("'", "\"");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            if (!Directory.Exists("Resources"))
            {
                Directory.CreateDirectory("Resources");
            }
            doc.Save($"Resources/{className}.Icon.mpx");
        }

        private void GenMonitor(string className, string folder)
        {
            var template = $@"<TemplateGroup>
              <Instances>
                <Instance ID='Instance1cb8bbbf42bb4bd8ba0ce5a2d4aaa2da' Type='Microsoft.SystemCenter.Authoring.CodeGeneration.Monitoring.UnitMonitor' Version='1.0.0.0'>
                  <UnitMonitorConfig xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                    <MonitorType>Windows!Microsoft.Windows.TimedScript.ThreeStateMonitorType</MonitorType>
                    <MonitorConfig>&lt;IntervalSeconds&gt;900&lt;/IntervalSeconds&gt;&lt;SyncTime /&gt;&lt;ScriptName&gt;GetStatus.vbs&lt;/ScriptName&gt;&lt;Arguments&gt;$Target/Property[Type='{className}']/Health$ $Target/Property[Type='{className}']/UnionId$ '{className}'&lt;/Arguments&gt;&lt;ScriptBody&gt;$IncludeFileContent/Scripts/GetStatus.vbs$&lt;/ScriptBody&gt;&lt;SecureInput&gt;&lt;/SecureInput&gt;&lt;TimeoutSeconds&gt;5&lt;/TimeoutSeconds&gt;&lt;ErrorExpression&gt;&lt;SimpleExpression&gt;&lt;ValueExpression&gt;&lt;XPathQuery&gt;Property[@Name='healthStatus']&lt;/XPathQuery&gt;&lt;/ValueExpression&gt;&lt;Operator&gt;Equal&lt;/Operator&gt;&lt;ValueExpression&gt;&lt;Value Type='String'&gt;Critical&lt;/Value&gt;&lt;/ValueExpression&gt;&lt;/SimpleExpression&gt;&lt;/ErrorExpression&gt;&lt;WarningExpression&gt;&lt;SimpleExpression&gt;&lt;ValueExpression&gt;&lt;XPathQuery&gt;Property[@Name='healthStatus']&lt;/XPathQuery&gt;&lt;/ValueExpression&gt;&lt;Operator&gt;Equal&lt;/Operator&gt;&lt;ValueExpression&gt;&lt;Value Type='String'&gt;Warning&lt;/Value&gt;&lt;/ValueExpression&gt;&lt;/SimpleExpression&gt;&lt;/WarningExpression&gt;&lt;SuccessExpression&gt;&lt;SimpleExpression&gt;&lt;ValueExpression&gt;&lt;XPathQuery&gt;Property[@Name='healthStatus']&lt;/XPathQuery&gt;&lt;/ValueExpression&gt;&lt;Operator&gt;Equal&lt;/Operator&gt;&lt;ValueExpression&gt;&lt;Value Type='String'&gt;OK&lt;/Value&gt;&lt;/ValueExpression&gt;&lt;/SimpleExpression&gt;&lt;/SuccessExpression&gt;</MonitorConfig>
                    <MonitorOperationalStates>
                      <OperationalState>
                        <OperationalStateId>Error</OperationalStateId>
                        <OperationalStateDisplayName>Error</OperationalStateDisplayName>
                        <MonitorTypeStateId>Error</MonitorTypeStateId>
                        <HealthState>Error</HealthState>
                      </OperationalState>
                      <OperationalState>
                        <OperationalStateId>Warning</OperationalStateId>
                        <OperationalStateDisplayName>Warning</OperationalStateDisplayName>
                        <MonitorTypeStateId>Warning</MonitorTypeStateId>
                        <HealthState>Warning</HealthState>
                      </OperationalState>
                      <OperationalState>
                        <OperationalStateId>Success</OperationalStateId>
                        <OperationalStateDisplayName>Success</OperationalStateDisplayName>
                        <MonitorTypeStateId>Success</MonitorTypeStateId>
                        <HealthState>Success</HealthState>
                      </OperationalState>
                    </MonitorOperationalStates>
                    <ParentMonitor>Health!System.Health.AvailabilityState</ParentMonitor>
                    <Accessibility>Internal</Accessibility>
                    <Id>{className}.Monitor</Id>
                    <DisplayName>{className}.Monitor</DisplayName>
                    <Description>{className}.Monitor</Description>
                    <Target>{className}</Target>
                    <Enabled>true</Enabled>
                    <Category>AvailabilityHealth</Category>
                    <Remotable>true</Remotable>
                    <Priority>Normal</Priority>
                    <AlertAutoResolve>true</AlertAutoResolve>
                    <AlertOnState>None</AlertOnState>
                    <AlertPriority>Normal</AlertPriority>
                    <AlertSeverity>MatchMonitorHealth</AlertSeverity>
                    <AlertName>{className}.Alert</AlertName>
                    <AlertDescription>AlertDescription</AlertDescription>
                    <ConfirmDelivery>false</ConfirmDelivery>
                  </UnitMonitorConfig>
                </Instance>
              </Instances>
            </TemplateGroup>";
            var result = template.Replace("'", "\"");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            doc.Save($"{folder}/{className}.Monitor.mptg");
        }

        private void GenGroupMonitor(string className, string folder)
        {
            var template = $@"<TemplateGroup>
                      <Instances>
                        <Instance ID='Instanced4c31df0abfd435982dd70d9fdf8fa56' Type='Microsoft.SystemCenter.Authoring.CodeGeneration.Monitoring.DependencyMonitor' Version='1.0.0.0'>
                          <DependencyMonitorConfig xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                            <Algorithm>
                              <Algorithm>WorstOf</Algorithm>
                            </Algorithm>
                            <MemberMonitor>Health!System.Health.AvailabilityState</MemberMonitor>
                            <MemberInMaintenance>Ignore</MemberInMaintenance>
                            <MemberUnavailable>Ignore</MemberUnavailable>
                            <RelationshipType>{className}.Relationship</RelationshipType>
                            <ParentMonitor>Health!System.Health.AvailabilityState</ParentMonitor>
                            <Accessibility>Internal</Accessibility>
                            <Id>{className}Group.Monitor</Id>
                            <DisplayName>{className}Group.Monitor</DisplayName>
                            <Description>{className}Group.Monitor</Description>
                            <Target>{className}Group</Target>
                            <Enabled>true</Enabled>
                            <Category>AvailabilityHealth</Category>
                            <Remotable>true</Remotable>
                            <Priority>Normal</Priority>
                            <AlertAutoResolve>true</AlertAutoResolve>
                            <AlertOnState>None</AlertOnState>
                            <AlertPriority>Normal</AlertPriority>
                            <AlertSeverity>MatchMonitorHealth</AlertSeverity>
                          </DependencyMonitorConfig>
                        </Instance>
                      </Instances>
                    </TemplateGroup>";
            var result = template.Replace("'", "\"");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            doc.Save($"{folder}/{className}.Monitor.mptg");
        }

        private string GetName(string className)
        {
            return className.Substring(className.LastIndexOf(".") + 1, className.Length - className.LastIndexOf(".") - 1);
        }

        private List<string> GetFields(string className, PropertyInfo[] propertys)
        {
            Func<PropertyInfo, bool> filterProperties = x => x.Name != "ODataContext" && x.Name != "ODataId" && x.Name != "ODataType" && x.Name != "ResourceURL";
            Func<PropertyInfo, bool> filterObjects = x => !x.PropertyType.FullName.Contains("FusionDirector") || (x.PropertyType.FullName.Contains("FusionDirector") && x.PropertyType.BaseType.FullName != "System.Object");
            var result = propertys
                .Where(filterProperties)
                .Where(filterObjects)
                .Where(x => !x.PropertyType.FullName.Contains("List"))
                .Where(x => x.PropertyType.FullName != "System.Object")
                .Select(x => x.Name).ToList();
            if (className != "FusionDirector.Enclosure" && className != "FusionDirector.Server")
            {
                result = result.Where(x => x != "FusionDirectorIp").ToList();
            }
            return result;
        }


        private void GenRule(string className, string folder, int severity, int mantissaNumber)
        {
            var con = $"{severity}{mantissaNumber}";
            var name = className.Split('.').Last();
            var template = $@"<TemplateGroup>
              <Instances>
                <Instance ID='Instancead122e8e2c2b461a804a5ac5069aa36f' Type='Microsoft.SystemCenter.Authoring.CodeGeneration.Monitoring.CustomRule' Version='1.0.0.0'>
                  <CustomRuleConfig xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                    <Id>FusionDirector.{name}.AlertRule.{con}</Id>
                    <DisplayName>FusionDirector {name} Alert Rule {con}</DisplayName>
                    <Description>FusionDirector {name} Alert Rule (The alarm level is '{(severity == 1 ? "warning" : "error")}' and the eventnumber has a mantissa of {mantissaNumber})</Description>
                    <Category>None</Category>
                    <ConfirmDelivery>false</ConfirmDelivery>
                    <Target>FusionDirector.{name}</Target>
                    <Enabled>true</Enabled>
                    <Remotable>true</Remotable>
                    <Priority>Normal</Priority>
                    <DiscardLevel>100</DiscardLevel>
                    <DataSources>
                      <RuleDataSourceItem>
                        <DataSourceId>FusionDirector.{name}.DS.{con}</DataSourceId>
                        <DataSourceType>SC!Microsoft.SystemCenter.SdkEventProvider</DataSourceType>
                      </RuleDataSourceItem>
                    </DataSources>
                    <ConditionDetectionConfig>&lt;Expression&gt;&lt;And&gt;&lt;Expression&gt;&lt;SimpleExpression&gt;&lt;ValueExpression&gt;&lt;XPathQuery&gt;Params/Param[3]&lt;/XPathQuery&gt;&lt;/ValueExpression&gt;&lt;Operator&gt;Equal&lt;/Operator&gt;&lt;ValueExpression&gt;&lt;Value&gt;$Target/Property[Type='FusionDirector.{name}']/UnionId$&lt;/Value&gt;&lt;/ValueExpression&gt;&lt;/SimpleExpression&gt;&lt;/Expression&gt;&lt;Expression&gt;&lt;SimpleExpression&gt;&lt;ValueExpression&gt;&lt;XPathQuery&gt;Params/Param[1]&lt;/XPathQuery&gt;&lt;/ValueExpression&gt;&lt;Operator&gt;Equal&lt;/Operator&gt;&lt;ValueExpression&gt;&lt;Value&gt;{con}&lt;/Value&gt;&lt;/ValueExpression&gt;&lt;/SimpleExpression&gt;&lt;/Expression&gt;&lt;/And&gt;&lt;/Expression&gt;</ConditionDetectionConfig>
                    <ConditionDetectionId>FusionDirector.{name}.CD.{con}</ConditionDetectionId>
                    <ConditionDetectionType>System!System.ExpressionFilter</ConditionDetectionType>
                    <WriteActions>
                      <RuleWriteActionItem>
                        <WriteActionId>FusionDirector.{name}.WA.{con}</WriteActionId>
                        <WriteActionConfig>&lt;Priority&gt;$Data/Params/Param[2]$&lt;/Priority&gt;&lt;Severity&gt;{severity}&lt;/Severity&gt;&lt;AlertMessageId&gt;$MPElement[Name='FVL!FusionDirector.{name}.AlertMessage']$&lt;/AlertMessageId&gt;&lt;AlertParameters&gt;&lt;AlertParameter1&gt;$Data/EventData/AlarmData/EventSubject$&lt;/AlertParameter1&gt;&lt;AlertParameter2&gt;$Data/EventData/AlarmData/ResourceIdName$&lt;/AlertParameter2&gt;&lt;AlertParameter3&gt;$Data/EventData/AlarmData/AlarmName$&lt;/AlertParameter3&gt;&lt;AlertParameter4&gt;$Data/EventData/AlarmData/OccurTime$&lt;/AlertParameter4&gt;&lt;AlertParameter5&gt;$Data/EventData/AlarmData/Additional$&lt;/AlertParameter5&gt;&lt;AlertParameter6&gt;$Data/EventData/AlarmData/Suggstion$&lt;/AlertParameter6&gt;&lt;/AlertParameters&gt;&lt;Suppression /&gt;&lt;!--UnionId--&gt;&lt;Custom1&gt;$Target/Property[Type='FusionDirector.{name}']/UnionId$&lt;/Custom1&gt;&lt;!--AlarmId--&gt;&lt;Custom2&gt;$Data/EventData/AlarmData/AlarmId$&lt;/Custom2&gt;&lt;!--AlarmName--&gt;&lt;Custom3&gt;$Data/EventData/AlarmData/AlarmName$&lt;/Custom3&gt;&lt;!--ResourceId--&gt;&lt;Custom4&gt;$Data/EventData/AlarmData/ResourceId$&lt;/Custom4&gt;&lt;!--Sn--&gt;&lt;Custom5&gt;$Data/EventData/AlarmData/Sn$&lt;/Custom5&gt;&lt;!--Additional--&gt;&lt;Custom6&gt;$Data/EventData/AlarmData/Additional$&lt;/Custom6&gt;&lt;!--Suggstion--&gt;&lt;Custom7&gt;$Data/EventData/AlarmData/Suggstion$&lt;/Custom7&gt;&lt;!--OccurTime--&gt;&lt;Custom8&gt;$Data/EventData/AlarmData/OccurTime$&lt;/Custom8&gt;&lt;!--PossibleCause--&gt;&lt;Custom9&gt;$Data/EventData/AlarmData/PossibleCause$&lt;/Custom9&gt;&lt;!--Effect--&gt;&lt;Custom10&gt;$Data/EventData/AlarmData/Effect$&lt;/Custom10&gt;</WriteActionConfig>
                        <WriteActionType>Health!System.Health.GenerateAlert</WriteActionType>
                      </RuleWriteActionItem>
                    </WriteActions>
                  </CustomRuleConfig>
                </Instance>
              </Instances>
            </TemplateGroup>";
            var result = template.Replace("'", "\"");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            doc.Save($"{folder}/{className}.Rule.{con}.mptg");
        }

        #endregion

        private void FormGen_Load(object sender, EventArgs e)
        {

        }

        #region Enclosure

        private void btnGenEnclosure_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Enclosure";
            PropertyInfo[] propertys = typeof(Enclosure).GetProperties();
            this.GenClass(className, "Classes", propertys);
            //this.GenGroup(className, "Groups");
            this.GenMonitor(className, "Monitors/Classes");
            this.GenGroupMonitor(className, "Monitors/Groups");
        }

        private void btnGenServerSlot_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Enclosure.ServerSlot";
            PropertyInfo[] propertys = typeof(ServerSlot).GetProperties();
            this.GenAll(className, propertys);
        }

        private void btnGenSwitchSlot_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Enclosure.SwitchSlot";
            PropertyInfo[] propertys = typeof(SwitchSlot).GetProperties();
            this.GenAll(className, propertys);
        }

        private void btnGenManagerSlot_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Enclosure.ManagerSlot";
            PropertyInfo[] propertys = typeof(ManagerSlot).GetProperties();
            this.GenAll(className, propertys);
        }

        private void btnGenFanSlot_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Enclosure.FanSlot";
            PropertyInfo[] propertys = typeof(FanSlot).GetProperties();
            this.GenAll(className, propertys);
        }

        private void btnGenPowerSlot_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Enclosure.PowerSlot";
            PropertyInfo[] propertys = typeof(PowerSlot).GetProperties();
            this.GenAll(className, propertys);
        }

        #endregion

        #region Server

        private void btnGenServer_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Server";
            PropertyInfo[] propertys = typeof(Server).GetProperties();
            this.GenClass(className, "Classes", propertys);
            //this.GenGroup(className, "Groups");
            this.GenMonitor(className, "Monitors/Classes");
            this.GenGroupMonitor(className, "Monitors/Groups");
        }

        private void btnGenProcessor_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Server.Processor";
            PropertyInfo[] propertys = typeof(Processor).GetProperties();
            this.GenAll(className, propertys);
        }

        private void btnGenMemory_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Server.Memory";
            PropertyInfo[] propertys = typeof(Memory).GetProperties();
            this.GenAll(className, propertys);
        }

        private void btnGenDrive_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Server.Drive";
            PropertyInfo[] propertys = typeof(Drive).GetProperties();
            this.GenAll(className, propertys);
        }

        private void btnGenNetworkAdapter_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Server.NetworkAdapter";
            PropertyInfo[] propertys = typeof(NetworkAdapter).GetProperties();
            this.GenAll(className, propertys);
        }

        private void btnGenRaidCard_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Server.RaidCard";
            PropertyInfo[] propertys = typeof(RaidCard).GetProperties();
            this.GenAll(className, propertys);
        }

        private void btnGenPCIeCard_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Server.PCIeCard";
            PropertyInfo[] propertys = typeof(PCIeCard).GetProperties();
            this.GenAll(className, propertys);
        }

        private void btnGenPower_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Server.Power";
            PropertyInfo[] propertys = typeof(Power).GetProperties();
            this.GenAll(className, propertys);
        }

        private void btnGenFan_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Server.Fan";
            PropertyInfo[] propertys = typeof(Fan).GetProperties();
            this.GenAll(className, propertys);
        }

        private void btnGenStorageController_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Server.RaidCard.StorageController";
            PropertyInfo[] propertys = typeof(StorageController).GetProperties();
            this.GenAll(className, propertys);
        }

        #endregion

        #region Appliance

        private void btnGenAppliance_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Appliance";
            PropertyInfo[] propertys = typeof(Appliance).GetProperties();
            this.GenClass(className, "Classes", propertys);
            //this.GenGroup(className, "Groups");
            this.GenMonitor(className, "Monitors/Classes");
        }
        private void btnGenEnclosureCollection_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Appliance.EnclosureCollection";
            //PropertyInfo[] propertys = typeof(EnclosureCollection).GetProperties();
            //this.GenClass(className, "Classes", propertys);
            this.GenMonitor(className, "Monitors/Classes");
        }

        private void btnGenServerCollection_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Appliance.ServerCollection";
            //PropertyInfo[] propertys = typeof(EnclosureCollection).GetProperties();
            //this.GenClass(className, "Classes", propertys);
            this.GenMonitor(className, "Monitors/Classes");
        }

        private void btnGenPerformanceCollection_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Appliance.PerformanceCollection";
            //PropertyInfo[] propertys = typeof(EnclosureCollection).GetProperties();
            //this.GenClass(className, "Classes", propertys);
            this.GenMonitor(className, "Monitors/Classes");
        }

        private void btnGenEventCollection_Click(object sender, EventArgs e)
        {
            var className = "FusionDirector.Appliance.EventCollection";
            //PropertyInfo[] propertys = typeof(EnclosureCollection).GetProperties();
            //this.GenClass(className, "Classes", propertys);
            this.GenMonitor(className, "Monitors/Classes");
        }
        #endregion

        private void btnGenIcon_Click(object sender, EventArgs e)
        {
            this.GenIcon("FusionDirector.Enclosure");
            this.GenIcon("FusionDirector.Enclosure.ServerSlot");
            this.GenIcon("FusionDirector.Enclosure.SwitchSlot");
            this.GenIcon("FusionDirector.Enclosure.ManagerSlot");
            this.GenIcon("FusionDirector.Enclosure.FanSlot");
            this.GenIcon("FusionDirector.Enclosure.PowerSlot");

            this.GenIcon("FusionDirector.Server");
            this.GenIcon("FusionDirector.Server.Processor");
            this.GenIcon("FusionDirector.Server.Memory");
            this.GenIcon("FusionDirector.Server.Drive");
            this.GenIcon("FusionDirector.Server.NetworkAdapter");
            this.GenIcon("FusionDirector.Server.RaidCard");
            this.GenIcon("FusionDirector.Server.PCIeCard");
            this.GenIcon("FusionDirector.Server.Power");
            this.GenIcon("FusionDirector.Server.Fan");
            this.GenIcon("FusionDirector.Server.StorageController");

            this.GenIcon("FusionDirector.Appliance");
        }

        #region Gen Add And Update
        public string GenCreate(string className, PropertyInfo[] propertys)
        {
            className = $"{GetName(className)}";
            var template = $@"private MPObject Create{className}({className} model)
            %
                var propertys = this.{className}Class.PropertyCollection;
                var obj = new MPObject(MGroup.Instance, this.{className}Class);
                {GenAddRow(className, propertys)} 
                obj[this.DisplayNameField].Value = model.ProductName;
                return obj;
            ^";
            return template.Replace("%", "{").Replace("^", "}").Replace("'", "\"");
        }

        public string GenUpdate(string className, PropertyInfo[] propertys)
        {
            className = $"{GetName(className)}";
            var template = $@"private void Update{className}({className} model, MonitoringObject existObj)
            %
                var propertys = this.{className}Class.PropertyCollection;
                {GenUpdateRow(className, propertys)} 
                existObj[this.DisplayNameField].Value = model.ProductName;
             ^";
            return template.Replace("%", "{").Replace("^", "}").Replace("'", "\"");
        }

        private string GenAddRow(string className, PropertyInfo[] propertys)
        {
            var fields = GetFields(className, propertys);
            return string.Join("\n", fields.Select(x =>
            {
                if (x.EndsWith("State"))
                {
                    return $"obj[propertys['{x}']].Value = model.{x}.ToString();";
                }
                if (x == "Health")
                {
                    return $"obj[propertys['Health']].Value = (int)model.Health;";
                }
                return $"obj[propertys['{x}']].Value = model.{x};";
            }));
        }

        private string GenUpdateRow(string className, PropertyInfo[] propertys)
        {
            var fields = GetFields(className, propertys);
            fields = fields.Where(x => x != "UnionId").ToList();
            return string.Join("\n", fields.Where(x => x != "Id").Select(x =>
            {
                if (x.EndsWith("State"))
                {
                    return $"existObj[propertys['{x}']].Value = model.{x}.ToString();";
                }
                if (x == "Health")
                {
                    return $"existObj[propertys['Health']].Value = (int)model.Health;";
                }
                return $"existObj[propertys['{x}']].Value = model.{x};";
            }));
        }

        private void btnGenEnclosureCode_Click(object sender, EventArgs e)
        {
            var result = new List<string>
            {
                "#region Create And Update",
                this.GenCreate("FusionDirector.Enclosure", typeof(Enclosure).GetProperties()),
                this.GenCreate("FusionDirector.Enclosure.ServerSlot", typeof(ServerSlot).GetProperties()),
                this.GenCreate("FusionDirector.Enclosure.SwitchSlot", typeof(SwitchSlot).GetProperties()),
                this.GenCreate("FusionDirector.Enclosure.ManagerSlot", typeof(ManagerSlot).GetProperties()),
                this.GenCreate("FusionDirector.Enclosure.FanSlot", typeof(FanSlot).GetProperties()),
                this.GenCreate("FusionDirector.Enclosure.PowerSlot", typeof(PowerSlot).GetProperties()),

                this.GenUpdate("FusionDirector.Enclosure", typeof(Enclosure).GetProperties()),
                this.GenUpdate("FusionDirector.Enclosure.ServerSlot", typeof(ServerSlot).GetProperties()),
                this.GenUpdate("FusionDirector.Enclosure.SwitchSlot", typeof(SwitchSlot).GetProperties()),
                this.GenUpdate("FusionDirector.Enclosure.ManagerSlot", typeof(ManagerSlot).GetProperties()),
                this.GenUpdate("FusionDirector.Enclosure.FanSlot", typeof(FanSlot).GetProperties()),
                this.GenUpdate("FusionDirector.Enclosure.PowerSlot", typeof(PowerSlot).GetProperties()),
                "#endregion"
            };
            var str = string.Join("\n\n", result);
        }

        private void btnGenServerCode_Click(object sender, EventArgs e)
        {
            var result = new List<string>
            {
                "#region Create And Update",
                this.GenCreate("FusionDirector.Server", typeof(Server).GetProperties()),
                this.GenCreate("FusionDirector.Server.Processor", typeof(Processor).GetProperties()),
                this.GenCreate("FusionDirector.Server.Memory", typeof(Memory).GetProperties()),
                this.GenCreate("FusionDirector.Server.Drive", typeof(Drive).GetProperties()),
                this.GenCreate("FusionDirector.Server.NetworkAdapter", typeof(NetworkAdapter).GetProperties()),
                this.GenCreate("FusionDirector.Server.PCIeCard", typeof(PCIeCard).GetProperties()),
                this.GenCreate("FusionDirector.Server.Power", typeof(Power).GetProperties()),
                this.GenCreate("FusionDirector.Server.Fan", typeof(Fan).GetProperties()),
                this.GenCreate("FusionDirector.Server.RaidCard", typeof(RaidCard).GetProperties()),
                this.GenCreate("FusionDirector.Server.StorageController", typeof(StorageController).GetProperties()),

                this.GenUpdate("FusionDirector.Server", typeof(Server).GetProperties()),
                this.GenUpdate("FusionDirector.Server.Processor", typeof(Processor).GetProperties()),
                this.GenUpdate("FusionDirector.Server.Memory", typeof(Memory).GetProperties()),
                this.GenUpdate("FusionDirector.Server.Drive", typeof(Drive).GetProperties()),
                this.GenUpdate("FusionDirector.Server.NetworkAdapter", typeof(NetworkAdapter).GetProperties()),
                this.GenUpdate("FusionDirector.Server.PCIeCard", typeof(PCIeCard).GetProperties()),
                this.GenUpdate("FusionDirector.Server.Power", typeof(Power).GetProperties()),
                this.GenUpdate("FusionDirector.Server.Fan", typeof(Fan).GetProperties()),
                this.GenUpdate("FusionDirector.Server.RaidCard", typeof(RaidCard).GetProperties()),
                this.GenUpdate("FusionDirector.Server.StorageController", typeof(StorageController).GetProperties()),
                "#endregion"
            };
            var str = string.Join("\n\n", result);
        }

        private void btnGenApplianceCode_Click(object sender, EventArgs e)
        {
            var result = new List<string>
            {
                "#region Create And Update",
                this.GenCreate("FusionDirector.Appliance", typeof(Appliance).GetProperties()),

                this.GenUpdate("FusionDirector.Appliance", typeof(Appliance).GetProperties()),
                "#endregion"
            };
            var str = string.Join("\n\n", result);
        }
        #endregion

        #region Rule
        private void btnGenEnclosureRule_Click(object sender, EventArgs e)
        {
            string className = "FusionDirector.Enclosure";
            var folder = "Enclosure Rules";
            for (int i = 0; i < 10; i++)
            {
                GenRule(className, folder, 1, i);
                GenRule(className, folder, 2, i);
            }
        }

        private void btnGenServerRule_Click(object sender, EventArgs e)
        {
            string className = "FusionDirector.Server";
            var folder = "Server Rules";
            for (int i = 0; i < 10; i++)
            {
                GenRule(className, folder, 1, i);
                GenRule(className, folder, 2, i);
            }
        }

        private void btnGenRule_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void btnGenFDCode_Click(object sender, EventArgs e)
        {
            var result = new List<string>
            {
                "#region Create And Update",
                this.GenCreate("FusionDirector.Entity", typeof(FusionDirectorPlugin.Dal.Model.FusionDirector).GetProperties()),

                this.GenUpdate("FusionDirector.Entity", typeof(FusionDirectorPlugin.Dal.Model.FusionDirector).GetProperties()),
                "#endregion"
            };
            var str = string.Join("\n\n", result);
        }

        private void btnGenFDClass_Click(object sender, EventArgs e)
        {
            PropertyInfo[] propertys = typeof(FusionDirectorPlugin.Dal.Model.FusionDirector).GetProperties();
            this.GenClass("FusionDirector.Entity", "Classes", propertys);
        }
    }
}
