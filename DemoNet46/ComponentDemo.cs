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
    }
}
