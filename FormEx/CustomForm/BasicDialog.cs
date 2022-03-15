using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    public class BasicDialog : Form
    {
        public BasicDialog()
        {
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }

        public new DialogResult ShowDialog()
        {
            Form ownerForm = FormManager.TryGetLatestActiveForm();
            if (ownerForm != null)
            {
                return this.ShowDialog(ownerForm); 
            }
            else
            {
                return base.ShowDialog();
            }
        }

        public DialogResult ShowDialog(Form ownerForm)
        {
            this.StartPosition = FormStartPosition.CenterParent;
            if (ownerForm.Icon != null && ownerForm.ShowIcon)
            {
                this.Icon = ownerForm.Icon;
            }
            return base.ShowDialog(ownerForm);
        }

        public void Close(DialogResult dialogResult)
        {
            if (!Modal)
            {
                DialogResult = dialogResult;
                base.Close(); 
            }
            else
            {
                base.Close();
            }
        }
    }
}
