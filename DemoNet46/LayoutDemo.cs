using DemoNet46.Pages;
using FormExCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace DemoNet46
{
    public partial class LayoutDemo : NonFrameForm
    {
        public LayoutDemo()
        {
            InitializeComponent();
            Load += LayoutDemo_Load;
        }

        private void LayoutDemo_Load(object sender, EventArgs e)
        {
            OpenTabPage<HomePage>();
        }

        private void btnHome_SingleClick(object sender, MouseEventArgs e)
        {
            if (IsTabPageOpen<HomePage>())
            {
                return;
            }
            //CloseCurrentPage();
            OpenTabPage<HomePage>();
        }

        private void btnFavorite_SingleClick(object sender, MouseEventArgs e)
        {
            if (IsTabPageOpen<FavoritePage>())
            {
                return;
            }
            //CloseCurrentPage();
            OpenTabPage<FavoritePage>();
        }

        private void btnHelp_SingleClick(object sender, MouseEventArgs e)
        {

        }

        private void btnSwitchSidebar_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                int maxWidth = pnlSidebar.MaximumSize.Width;
                int minWidth = pnlSidebar.MinimumSize.Width;
                Thread t1 = new Thread(() => {

                    if (Conv.NS(pnlSidebar.Tag) == "Expand")
                    {
                        while (pnlSidebar.Width < maxWidth)
                        {
                            if (pnlSidebar.Width + 20 >= maxWidth)
                            {
                                pnlSidebar.Invoke((Action)delegate
                                { 
                                    pnlSidebar.Width = maxWidth;
                                    pnlSidebar.Tag = "Collapse";
                                });

                                return;
                            }
                            else
                            {
                                pnlSidebar.Invoke((Action)delegate
                                {
                                    pnlSidebar.Width += 20;
                                    foreach (Control control in pnlSidebar.Controls)
                                    {
                                        control.Refresh();
                                    }
                                });
                            }
                        }
                    }
                    else
                    {

                        while (pnlSidebar.Width > minWidth)
                        {
                            if (pnlSidebar.Width - 20 <= minWidth)
                            {
                                pnlSidebar.Invoke((Action)delegate
                                {
                                    pnlSidebar.Width = minWidth;
                                    pnlSidebar.Tag = "Expand";
                                });
                                return;
                            }
                            else
                            {
                                pnlSidebar.Invoke((Action)delegate
                                {
                                    pnlSidebar.Width -= 20;
                                });
                            }
                        }
                    }

                });

                t1.Start();
            }
        }





        public bool IsTabPageOpen<T>() where T : UserControl
        {
            var container = ContentSwitcher;
            if (container.SelectedTab != null)
            {
                return container.SelectedTab.Controls.OfType<T>().Any();
            }

            return false;
        }


        public void CloseCurrentPage()
        {
            var container = this.ContentSwitcher;
            if (container.SelectedTab != null)
            {
                var childControls = container.SelectedTab.Controls.OfType<UserControl>();
                foreach (var item in childControls)
                {
                    container.SelectedTab.Controls.Remove(item);
                    item.Dispose();
                }
            }
        }

        /// <summary>
        /// 创新或打开一个TabPage
        /// </summary>
        /// <typeparam name="T">控件的类型，一个用户控件占用一个TabPage</typeparam>
        /// <param name="tabPageKey">指定要打开的标签栏Key</param>
        public T OpenTabPage<T>(string? tabPageKey = null, object args = null) where T : UserControl, new()
        {
            T contentControl = default(T);
            var container = this.ContentSwitcher;

            int countOfT = 0;
            if (tabPageKey == null)
            {
                tabPageKey = $"uc_{typeof(T).Name}_{countOfT + 1}";// 新TabPage的Key, 同时也是控件的Name
            }

            countOfT = container.TabPages
                    .Cast<TabPage>()
                    .SelectMany(page => page.Controls.OfType<T>().Where(control => control.Name == tabPageKey))
                    .Count();


            // 如果不存在，则创建一个
            if (countOfT == 0)
            {
                if (args == null)
                {
                    contentControl = new T();
                }
                else
                {
                    contentControl = (T)Activator.CreateInstance(typeof(T), args);
                }

                contentControl.Name = tabPageKey; 
                contentControl.Dock = DockStyle.Fill; 

                contentControl.Disposed += (sender, e) =>
                {
                    var contentControl = sender as Form;
                    if (contentControl != null && contentControl.Name != null)
                    {
                        var tabPageKey = contentControl.Name;
                        if (container.TabPages.ContainsKey(tabPageKey))
                        {
                            container.TabPages.RemoveByKey(tabPageKey);
                        }
                    }
                };

                container.TabPages.Insert(0, tabPageKey, contentControl.Text);
                var targetPage = container.TabPages[0];
                targetPage.Controls.Add(contentControl);
                container.SelectTab(targetPage);
                contentControl.Show();
            }
            else
            {
                var targetPage = container.TabPages[tabPageKey];
                contentControl = targetPage.Controls.OfType<T>().First();
                container.SelectTab(targetPage);
            }

            return contentControl;
        }


    }
}
