//**************************************************************************  
//Copyright (C) 2019 Huawei Technologies Co., Ltd. All rights reserved.
//This program is free software; you can redistribute it and/or modify
//it under the terms of the MIT license.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//MIT license for more detail.
//*************************************************************************  
﻿namespace FusionDirectorPlugin.TestClient
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.btnInsertEnclosure = new System.Windows.Forms.Button();
            this.btnGenCode = new System.Windows.Forms.Button();
            this.btnInsertServer = new System.Windows.Forms.Button();
            this.btnInsertServerEvent = new System.Windows.Forms.Button();
            this.btnInsertEnclosureEvent = new System.Windows.Forms.Button();
            this.btnInsertSwitchEvent = new System.Windows.Forms.Button();
            this.btnInsertHistoryEvent = new System.Windows.Forms.Button();
            this.btnInsertServerPerformanceData = new System.Windows.Forms.Button();
            this.btnInsertAppliance = new System.Windows.Forms.Button();
            this.btnStopSync = new System.Windows.Forms.Button();
            this.btnMockSync = new System.Windows.Forms.Button();
            this.btnInsertServerSummary = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start Service";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnInsertEnclosure
            // 
            this.btnInsertEnclosure.Location = new System.Drawing.Point(13, 58);
            this.btnInsertEnclosure.Name = "btnInsertEnclosure";
            this.btnInsertEnclosure.Size = new System.Drawing.Size(107, 23);
            this.btnInsertEnclosure.TabIndex = 1;
            this.btnInsertEnclosure.Text = "InsertEnclosure";
            this.btnInsertEnclosure.UseVisualStyleBackColor = true;
            this.btnInsertEnclosure.Click += new System.EventHandler(this.btnInsertEnclosure_Click);
            // 
            // btnGenCode
            // 
            this.btnGenCode.Location = new System.Drawing.Point(144, 24);
            this.btnGenCode.Name = "btnGenCode";
            this.btnGenCode.Size = new System.Drawing.Size(103, 23);
            this.btnGenCode.TabIndex = 2;
            this.btnGenCode.Text = "GenCode";
            this.btnGenCode.UseVisualStyleBackColor = true;
            this.btnGenCode.Click += new System.EventHandler(this.btnGenCode_Click);
            // 
            // btnInsertServer
            // 
            this.btnInsertServer.Location = new System.Drawing.Point(13, 92);
            this.btnInsertServer.Name = "btnInsertServer";
            this.btnInsertServer.Size = new System.Drawing.Size(107, 23);
            this.btnInsertServer.TabIndex = 3;
            this.btnInsertServer.Text = "InsertServer";
            this.btnInsertServer.UseVisualStyleBackColor = true;
            this.btnInsertServer.Click += new System.EventHandler(this.btnInsertServer_Click);
            // 
            // btnInsertServerEvent
            // 
            this.btnInsertServerEvent.Location = new System.Drawing.Point(12, 126);
            this.btnInsertServerEvent.Name = "btnInsertServerEvent";
            this.btnInsertServerEvent.Size = new System.Drawing.Size(138, 23);
            this.btnInsertServerEvent.TabIndex = 3;
            this.btnInsertServerEvent.Text = "InsertServerEvent";
            this.btnInsertServerEvent.UseVisualStyleBackColor = true;
            this.btnInsertServerEvent.Click += new System.EventHandler(this.btnInsertServerEvent_Click);
            // 
            // btnInsertEnclosureEvent
            // 
            this.btnInsertEnclosureEvent.Location = new System.Drawing.Point(12, 160);
            this.btnInsertEnclosureEvent.Name = "btnInsertEnclosureEvent";
            this.btnInsertEnclosureEvent.Size = new System.Drawing.Size(138, 23);
            this.btnInsertEnclosureEvent.TabIndex = 3;
            this.btnInsertEnclosureEvent.Text = "InsertEnclosureEvent";
            this.btnInsertEnclosureEvent.UseVisualStyleBackColor = true;
            this.btnInsertEnclosureEvent.Click += new System.EventHandler(this.btnInsertEnclosureEvent_Click);
            // 
            // btnInsertSwitchEvent
            // 
            this.btnInsertSwitchEvent.Location = new System.Drawing.Point(13, 194);
            this.btnInsertSwitchEvent.Name = "btnInsertSwitchEvent";
            this.btnInsertSwitchEvent.Size = new System.Drawing.Size(138, 23);
            this.btnInsertSwitchEvent.TabIndex = 4;
            this.btnInsertSwitchEvent.Text = "InsertSwitchEvent";
            this.btnInsertSwitchEvent.UseVisualStyleBackColor = true;
            this.btnInsertSwitchEvent.Click += new System.EventHandler(this.btnInsertSwitchEvent_Click);
            // 
            // btnInsertHistoryEvent
            // 
            this.btnInsertHistoryEvent.Location = new System.Drawing.Point(12, 228);
            this.btnInsertHistoryEvent.Name = "btnInsertHistoryEvent";
            this.btnInsertHistoryEvent.Size = new System.Drawing.Size(138, 23);
            this.btnInsertHistoryEvent.TabIndex = 5;
            this.btnInsertHistoryEvent.Text = "InsertHistoryEvent";
            this.btnInsertHistoryEvent.UseVisualStyleBackColor = true;
            this.btnInsertHistoryEvent.Click += new System.EventHandler(this.btnInsertHistoryEvent_Click);
            // 
            // btnInsertServerPerformanceData
            // 
            this.btnInsertServerPerformanceData.Location = new System.Drawing.Point(12, 262);
            this.btnInsertServerPerformanceData.Name = "btnInsertServerPerformanceData";
            this.btnInsertServerPerformanceData.Size = new System.Drawing.Size(183, 23);
            this.btnInsertServerPerformanceData.TabIndex = 16;
            this.btnInsertServerPerformanceData.Text = "InsertServerPerformanceData";
            this.btnInsertServerPerformanceData.UseVisualStyleBackColor = true;
            this.btnInsertServerPerformanceData.Click += new System.EventHandler(this.btnInsertServerPerformanceData_Click);
            // 
            // btnInsertAppliance
            // 
            this.btnInsertAppliance.Location = new System.Drawing.Point(181, 92);
            this.btnInsertAppliance.Name = "btnInsertAppliance";
            this.btnInsertAppliance.Size = new System.Drawing.Size(107, 23);
            this.btnInsertAppliance.TabIndex = 1;
            this.btnInsertAppliance.Text = "InsertAppliance";
            this.btnInsertAppliance.UseVisualStyleBackColor = true;
            this.btnInsertAppliance.Click += new System.EventHandler(this.btnInsertAppliance_Click);
            // 
            // btnStopSync
            // 
            this.btnStopSync.Location = new System.Drawing.Point(213, 223);
            this.btnStopSync.Name = "btnStopSync";
            this.btnStopSync.Size = new System.Drawing.Size(75, 23);
            this.btnStopSync.TabIndex = 17;
            this.btnStopSync.Text = "停止同步";
            this.btnStopSync.UseVisualStyleBackColor = true;
            this.btnStopSync.Click += new System.EventHandler(this.btnStopSync_Click);
            // 
            // btnMockSync
            // 
            this.btnMockSync.Location = new System.Drawing.Point(194, 194);
            this.btnMockSync.Name = "btnMockSync";
            this.btnMockSync.Size = new System.Drawing.Size(109, 23);
            this.btnMockSync.TabIndex = 18;
            this.btnMockSync.Text = "模拟同步";
            this.btnMockSync.UseVisualStyleBackColor = true;
            this.btnMockSync.Click += new System.EventHandler(this.btnMockSync_Click);
            // 
            // btnInsertServerSummary
            // 
            this.btnInsertServerSummary.Location = new System.Drawing.Point(181, 125);
            this.btnInsertServerSummary.Name = "btnInsertServerSummary";
            this.btnInsertServerSummary.Size = new System.Drawing.Size(122, 23);
            this.btnInsertServerSummary.TabIndex = 19;
            this.btnInsertServerSummary.Text = "InsertServerSummary";
            this.btnInsertServerSummary.UseVisualStyleBackColor = true;
            this.btnInsertServerSummary.Click += new System.EventHandler(this.btnInsertServerSummary_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 323);
            this.Controls.Add(this.btnInsertServerSummary);
            this.Controls.Add(this.btnMockSync);
            this.Controls.Add(this.btnStopSync);
            this.Controls.Add(this.btnInsertServerPerformanceData);
            this.Controls.Add(this.btnInsertHistoryEvent);
            this.Controls.Add(this.btnInsertSwitchEvent);
            this.Controls.Add(this.btnInsertEnclosureEvent);
            this.Controls.Add(this.btnInsertServerEvent);
            this.Controls.Add(this.btnInsertServer);
            this.Controls.Add(this.btnGenCode);
            this.Controls.Add(this.btnInsertAppliance);
            this.Controls.Add(this.btnInsertEnclosure);
            this.Controls.Add(this.button1);
            this.Name = "FormMain";
            this.Text = "Test";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnInsertEnclosure;
        private System.Windows.Forms.Button btnGenCode;
        private System.Windows.Forms.Button btnInsertServer;
        private System.Windows.Forms.Button btnInsertServerEvent;
        private System.Windows.Forms.Button btnInsertEnclosureEvent;
        private System.Windows.Forms.Button btnInsertSwitchEvent;
        private System.Windows.Forms.Button btnInsertHistoryEvent;
        private System.Windows.Forms.Button btnInsertServerPerformanceData;
        private System.Windows.Forms.Button btnInsertAppliance;
        private System.Windows.Forms.Button btnStopSync;
        private System.Windows.Forms.Button btnMockSync;
        private System.Windows.Forms.Button btnInsertServerSummary;
    }
}

