using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils.UI;

namespace Ocean
{
    public partial class OcnMessageBox : CustomForm
    {
        public OcnMessageBox()
        {
            InitializeComponent();

            ControlBorder cb = new ControlBorder(btnClose);
            cb.Timing = ControlBorder.DrawBorderTiming.MouseDown;
            cb.KeepBorderOnFocus = true;
            cb.DrawBorderCondition = () =>
            {
                object keepStatus = btnClose.Tag;
                if (keepStatus == null)
                {
                    if ((this.btnClose.Bms & ImageButton.ButtonMouseStatus.Pressed) == ImageButton.ButtonMouseStatus.Pressed)
                    {
                        btnClose.Tag = true;
                        return true;
                    }
                }
                else
                {
                    if ((bool)keepStatus)
                    {
                        if ((this.btnClose.Bms & ImageButton.ButtonMouseStatus.Pressed) == ImageButton.ButtonMouseStatus.Pressed)
                        {
                            btnClose.Tag = false;
                            return true;
                        }
                    }
                    else
                    {
                        if ((this.btnClose.Bms & ImageButton.ButtonMouseStatus.Pressed) == ImageButton.ButtonMouseStatus.Pressed)
                        {
                            btnClose.Tag = true;
                            return true;
                        }
                    }
                }

                return false;
            };
            cb.Apply();

            //cb.AddEventHandler("MouseDown", new MouseEventHandler((o, e) =>
            //{
            //    cb.DrawBorder();
            //}));

            //btnClose.ShowFocusLine = true;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //if (Modal)
            //{
            //    DialogResult = DialogResult.Cancel;
            //    this.Close();
            //}
            //else
            //{
            //    this.Close();
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblContent.Text = btnClose.Bms.ToString();
        }
    }
}
