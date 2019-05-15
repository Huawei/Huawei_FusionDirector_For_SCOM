using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Views;

namespace UnitTestSampleOnUI
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        //eSight配置
        private void button1_Click(object sender, EventArgs e)
        {
            HostTabsViewFrm frm = new HostTabsViewFrm("huawei/FDConfig/index.html");
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        //服务器列表
        private void button2_Click(object sender, EventArgs e)
        {
            HostTabsViewFrm frm = new HostTabsViewFrm("huawei/server/list.html");
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        //关于
        private void button3_Click(object sender, EventArgs e)
        {
            HostTabsViewFrm frm = new HostTabsViewFrm("huawei/about/about.html");
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        //OS镜像管理
        private void button4_Click(object sender, EventArgs e)
        {
            HostTabsViewFrm frm = new HostTabsViewFrm("huawei/osImage/index.html");
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        //OS部署管理
        private void button5_Click(object sender, EventArgs e)
        {
            HostTabsViewFrm frm = new HostTabsViewFrm("huawei/osDeploy/index.html");
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        //任务管理
        private void button6_Click(object sender, EventArgs e)
        {
            HostTabsViewFrm frm = new HostTabsViewFrm("huawei/task/index.html");
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        //配置管理
        private void button7_Click(object sender, EventArgs e)
        {
            HostTabsViewFrm frm = new HostTabsViewFrm("huawei/configuration/index.html");
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        //
        private void button8_Click(object sender, EventArgs e)
        {
            HostTabsViewFrm frm = new HostTabsViewFrm("huawei/upgradePackageWarehouse/index.html");
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

       
        private void button11_Click(object sender, EventArgs e)
        {

            HostTabsViewFrm frm = new HostTabsViewFrm("huawei/deviceVersionStatus/index.html");
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {

            HostTabsViewFrm frm = new HostTabsViewFrm("huawei/upgradePlan/index.html");
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }
    }
}
