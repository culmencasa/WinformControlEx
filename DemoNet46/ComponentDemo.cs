using FormExCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoNet46
{
    public partial class ComponentDemo : NonFrameForm
    {
        public ComponentDemo()
        {
            InitializeComponent();

            this.Load += ComponentDemo_Load;
        }

        private void ComponentDemo_Load(object sender, EventArgs e)
        {
            CustomNavGroup group1 = new CustomNavGroup();
            group1.GroupText = "导航组1";
            group1.GroupIndent = 12;
            group1.AddGroupItem("链接1", "", null, imageList1.Images[0], OpenLink);
            group1.AddGroupItem("链接2", "", null, imageList1.Images[0], OpenLink);
            group1.AddGroupItem("链接3", "", null, imageList1.Images[0], OpenLink);
            CustomNavGroup group2 = new CustomNavGroup();
            group2.GroupText = "导航组2";
            group2.GroupIndent = 12;
            group2.AddGroupItem("双行链接1", "详细说明", null, imageList1.Images[1], OpenLink);
            group2.AddGroupItem("双行链接2", "2023-05-08", null, imageList1.Images[1], OpenLink);
            group2.AddGroupItem("双行链接3", "1000s", null, imageList1.Images[1], OpenLink);
            group2.AddGroupItem("双行链接4", "", null, imageList1.Images[1], OpenLink);
            this.customNavSideBar1.Groups.Add(group1);
            customNavSideBar1.Groups.Add(group2);


        }

        private void OpenLink(object sender , EventArgs e)
        { 
        
        }

        private void btnShowException_Click(object sender, EventArgs e)
        {
            try
            {
                throw new IndexOutOfRangeException($"Invalid index: 0");
            }
            catch (Exception ex)
            {
                ExceptionMessageBox box = new ExceptionMessageBox(ex);
                box.Show();
            }
        }

        private void btnWorkShade_Click(object sender, EventArgs ee)
        {
            WorkShade ws = new WorkShade(3000);
            ws.Attach(this.Parent as Form);
            ws.Show();
        }

        private void btnBackgroundWorkShade_Click(object sender, EventArgs arg)
        {

            BackgroundWorkShade bws = BackgroundWorkShade.Quicker;
            bws.Attach(this.GetTopLevelForm());
            bws.Show();
            bws.Setup(() =>
            {
                return new BackgroundWorkShade.PredictionInfo()
                {
                    HowLongWillItTake = 1000,
                    CompletedPercent = 10,
                    WhatsNext = "正在活动指关节"
                };
            }, (worker, e) => {
                Thread.Sleep(1000);
            });


            bws.Setup(() =>
            {
                return new BackgroundWorkShade.PredictionInfo()
                {
                    HowLongWillItTake = 3000,
                    CompletedPercent = 60,
                    WhatsNext = "正在练习装死"
                };
            }, (worker, e) => {
                Thread.Sleep(3000);
            });

            bws.Setup(() =>
            {
                return new BackgroundWorkShade.PredictionInfo()
                {
                    HowLongWillItTake = 2000,
                    CompletedPercent = 100,
                    WhatsNext = "正在练习邪恶的大笑"
                };
            }, (worker, e) => {
                Thread.Sleep(2000);
            });
        }

        private void btnRoundForm_Click(object sender, EventArgs e)
        {
            OcnForm form = new OcnForm();
            form.Show();
        }

        private void btnShowInfoTip_Click(object sender, EventArgs e)
        {
            InfoTip.SetToolTip(btnShowInfoTip, 
                "空山新雨后， 天气晚来秋。 " + Environment.NewLine +
                "明月松间照， 清泉石上流。 " + Environment.NewLine +
                "竹喧归浣女， 莲动下渔舟。 " + Environment.NewLine +  
                "随意春芳歇， 王孙自可留。", "王维《山居秋暝》", InfoTip.Position.Right, 8000);
        }
        private void btnCustomMessage_Click(object sender, EventArgs e)
        {
            CustomMessageBox.ShowInformation("", "This is a Information");
            CustomMessageBox.ShowConfirm("", "This is a Question");
            CustomMessageBox.ShowError("", "This is an Error");
            CustomMessageBox.ShowWarning("", "This is a Warning");
        }

        private void btnProgressGo_Click(object sender, EventArgs e)
        {

            circularProgressBar1.Value = 0;
            Thread t = new Thread(() =>
            {
                int i = 0;
                do
                {
                    if (!this.IsHandleCreated || this.IsDisposed)
                    {
                        break;
                    }

                    this.Invoke((Action)delegate
                    {
                        circularProgressBar1.Value++;
                        circularProgressBar1.TextLine2 = circularProgressBar1.Value.ToString();
                    });
                    i++;
                    Thread.Sleep(1);
                } while (i < 100);

            });
            t.IsBackground = true;
            t.Start();
        }

    }
}
