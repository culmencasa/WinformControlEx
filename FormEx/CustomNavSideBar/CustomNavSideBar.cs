using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using System.Xml.Serialization;

namespace System.Windows.Forms
{

    [Browsable(false)]
    //[Designer(typeof(MyControlDesigner))]
    public class CustomNavSideBar : CustomPanel
    {
        public class GroupCollection : List<CustomNavGroup>
        {
            public event Action<CustomNavGroup> Added;
            public event Action<CustomNavGroup> Removed;

            public new void Add(CustomNavGroup item)
            {
                base.Add(item);

                Added?.Invoke(item);
            }

            public new void Remove(CustomNavGroup item)
            {
                base.Remove(item);

                Removed?.Invoke(item);
            }
        }

        private GroupCollection _groups = new GroupCollection();

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GroupCollection Groups
        {
            get 
            {
                return _groups;
            }
            set 
            {
                _groups = value;
                this.Controls.Clear();
                this.Controls.AddRange(_groups.ToArray());
            }
        }

        public CustomNavSideBar()
        {
            _groups.Added += OnGroupAdded;
            _groups.Removed += OnGroupRemoved;
        }

        private void OnGroupAdded(CustomNavGroup item)
        {
            if (!Controls.Contains(item))
            {
                this.Controls.Add(item);
            }
        }
        private void OnGroupRemoved(CustomNavGroup item)
        {
            if (Controls.Contains(item))
            {
                this.Controls.Remove(item);
            }
        }


        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            var control = e.Control;
            control.Dock = DockStyle.Top;
            control.BringToFront();
            
        }

    }


    //public class MyControlDesigner : ControlDesigner
    //{
    //    public override DesignerVerbCollection Verbs
    //    {
    //        get
    //        {
    //            var verbs = new DesignerVerbCollection();
    //            verbs.Add(new DesignerVerb("Add Group", OnDoSomething));
    //            return verbs;
    //        }
    //    }

    //    private void OnDoSomething(object sender, EventArgs e)
    //    {
    //        var form = new NavigationSideBarDesigner();
    //        form.ShowDialog();


    //    } 
    //}
}
