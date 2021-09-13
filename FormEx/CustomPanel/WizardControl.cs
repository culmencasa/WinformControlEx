using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace System.Windows.Forms
{

	public class WizardControl : TabControl
	{
		#region 内嵌类

		public class WizardButtonEventArgs : EventArgs
		{
			public TabPage CurrentTabPage { get; set; }

			public TabPage TargetTabPage { get; set; }

			public int CurrentIndex { get; set; }

			public int TargetIndex { get; set; }

			public bool GoNext { get; set; }

			public bool GoPrevious { get; set; }
		}

		#endregion

		#region 构造

		public WizardControl()
		{

		}

		#endregion

		#region 事件

		public event EventHandler LastButtonClicked;

		#endregion

		#region 字段

		private Button _nextButton;
		private Button _prevButton;
		private int _lastTabIndex = -1;

		private Dictionary<TabPage, List<Delegate>> _nextCheck = new Dictionary<TabPage, List<Delegate>>();
		private Dictionary<TabPage, List<Delegate>> _prevCheck = new Dictionary<TabPage, List<Delegate>>();

		private Dictionary<TabPage, EventHandler<WizardButtonEventArgs>> _pageDisplay = new Dictionary<TabPage, EventHandler<WizardButtonEventArgs>>();

		#endregion

		#region 属性

		/// <summary>
		/// 设置与向导控件关联的下一步按钮
		/// </summary>
		[Category("Buttons")]
		public Button NextButton
		{
			get
			{
				return _nextButton;
			}
			set
			{
				_nextButton = value;
				if (value == null)
				{
					//_nextButtonLabel = "Next";
					_nextButton.Click -= NextButton_Click;
				}
				else
				{
					//_nextButtonLabel = _nextButton.Text;
					_nextButton.Click += NextButton_Click;
				}
			}
		}

		/// <summary>
		/// 设置与向导控件关联的上一步按钮
		/// </summary>
		[Category("Buttons")]
		public Button PreviousButton
		{
			get
			{
				return _prevButton;
			}
			set
			{
				_prevButton = value;
				_prevButton.Enabled = false;
				_prevButton.Click += PreviousButton_Click;
			}
		}

		[Category("Buttons")]
		public string NextButtonTextOnLastPage { get; set; } = "关闭";

		public string NextButtonText { get; set; } = "下一步";
		public new TabAlignment Alignment { get; set; } = TabAlignment.Bottom;

		/// <summary>
		/// 标题
		/// </summary>
		public override string Text
		{
			get;
			set;
		}


		#endregion

		#region 覆盖的方法

		/// <summary>
		/// 此属性对于此控件无意义。
		/// </summary>
		/// <param name="page"></param>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void SelectTab(TabPage page)
		{
		}
		/// <summary>
		/// 此属性对于此控件无意义。
		/// </summary>
		/// <param name="tabName"></param>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void SelectTab(string tabName)
		{
		}
		/// <summary>
		/// 此属性对于此控件无意义。
		/// </summary>
		/// <param name="index"></param>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void SelectTab(int index)
		{
		}

		#endregion

		#region 重写的方法

		protected override void WndProc(ref Message m)
		{
			// 隐藏标签栏
			if (m.Msg == 0x1328 && !DesignMode)
			{
				m.Result = (IntPtr)1;
			}
			else
			{
				base.WndProc(ref m);
			}
		}
		protected override void OnHandleCreated(EventArgs e)
		{
			// 默认SelectedIndex为-1，
			// 但在OnHandleCreated触发时，SelectedIndex值会变为0或者用户指定的Index
			_lastTabIndex = SelectedIndex;
			base.OnHandleCreated(e);
		}

		/// <summary>
		/// OnCreateControl仅在TabPage第一次加载时激活
		/// </summary>
		protected override void OnCreateControl()
		{
			//备注： 由于OnSelectedIndexChanged不会在启动时触发，
			//      所以第一个页面在第一次启动时的操作放在这里, 并延迟触发
			//		后续的跳转全部由NextButton和PreviousButton的点击事件控制


			// 表示首次跳到第一页
			if (_lastTabIndex == -1 && SelectedIndex == 0)
			{
				var args = new WizardButtonEventArgs()
				{
					CurrentIndex = _lastTabIndex,
					TargetIndex = SelectedIndex,
					CurrentTabPage = null,
					TargetTabPage = TabPages[SelectedIndex],
					GoNext = true,
					GoPrevious = false
				};

				EventHandler eventHandler = delegate
				{
					if (Visible && !Disposing && _lastTabIndex == -1)
					{
						// 如果配置了第一个页面，则在第一个页面加载时执行指定的操作
						DoPageShownAction(TabPages[SelectedIndex], args);

						UpdateControlStates();

						_lastTabIndex = 0;
					}


				};
				this.VisibleChanged += eventHandler;


			}

			base.OnCreateControl();
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (Visible && !Disposing)
			{

			}

		}
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
		}


		#endregion

		#region 公有方法

		/// <summary>
		/// 设置当前页面在点击下一步按钮前，要做的检查和所要执行的操作.
		/// 当canGoNext=null且beforeGoNext=null时，页面不可跳转
		/// </summary>
		/// <param name="currentPage">当前页面</param>
		/// <param name="canGoNext">用于决定“下一步”按钮能否点击</param>
		/// <param name="beforeGoNext"></param>
		public void SetNextMove(TabPage currentPage, Func<bool> canGoNext, Func<TabPage> beforeGoNext)
		{
			if (_nextCheck.ContainsKey(currentPage))
			{
				_nextCheck[currentPage][0] = canGoNext == null ? () => false : canGoNext;
				_nextCheck[currentPage][1] = beforeGoNext;
			}
			else
			{
				_nextCheck.Add(currentPage, new List<Delegate>()
				{
					canGoNext == null ? () => false : canGoNext,
					beforeGoNext
				});
			}
		}

		/// <summary>
		/// 设置当前页面在点击上一步按钮前，要做的检查和所要执行的操作
		/// 当canGoPrev=null且beforeGoPrev=null时，页面不可跳转
		/// </summary>
		/// <param name="currentPage"></param>
		/// <param name="canGoPrev">用于决定“上一步”按钮能否点击</param>
		/// <param name="beforeGoPrev"></param>
		public void SetPreviousMove(TabPage currentPage, Func<bool> canGoPrev, Func<TabPage> beforeGoPrev)
		{
			if (_prevCheck.ContainsKey(currentPage))
			{
				_prevCheck[currentPage][0] = canGoPrev == null ? () => false : canGoPrev;
				_prevCheck[currentPage][1] = beforeGoPrev;
			}
			else
			{
				_prevCheck.Add(currentPage, new List<Delegate>()
				{
					canGoPrev == null ? ()=> false : canGoPrev,
					beforeGoPrev
				});
			}
		}

		/// <summary>
		/// 设置指定的页面在显示时要执行的操作
		/// </summary>
		/// <param name="page">指定的页面</param>
		/// <param name="action">在显示时要执行的操作</param>
		public void SetPageShownAction(TabPage page, EventHandler<WizardButtonEventArgs> action)
		{
			if (_pageDisplay.ContainsKey(page))
			{
				_pageDisplay[page] = action;
			}
			else
			{
				_pageDisplay.Add(page, action);
			}
		}

		/// <summary>
		/// 更新控件的状态
		/// </summary>
		public void UpdateControlStates()
		{
			if (NextButton != null)
			{
				if (_nextCheck.ContainsKey(SelectedTab))
				{
					var funcList = _nextCheck[SelectedTab];
					var canGoNext = funcList[0] as Func<bool>;
					if (canGoNext != null)
					{
						NextButton.Enabled = canGoNext.Invoke();
					}
				}

				if (SelectedIndex == TabCount - 1 && !string.IsNullOrEmpty(NextButtonTextOnLastPage))
				{
					NextButton.Text = NextButtonTextOnLastPage;
				}
				else
				{
					NextButton.Text = NextButtonText;
				}
			}

			if (PreviousButton != null)
			{
				// 默认可以点击“上一步”
				PreviousButton.Enabled = (SelectedIndex > 0);
				if (PreviousButton.Enabled)
				{
					// 如果配置了当前页面canGoPrev规则，则检查
					if (_prevCheck.ContainsKey(SelectedTab))
					{
						var funcList = _prevCheck[SelectedTab];
						var canGoPrevious = funcList[0] as Func<bool>;
						if (canGoPrevious != null)
						{
							PreviousButton.Enabled = canGoPrevious.Invoke();
						}
					}
				}
			}


			Text = SelectedTab.Text;
		}

		public void JumpTo(TabPage page)
		{
			var args = new WizardButtonEventArgs()
			{
				CurrentIndex = SelectedIndex,
				CurrentTabPage = SelectedTab,
				TargetIndex = TabPages.IndexOf(page),
				TargetTabPage = page,
				GoNext = SelectedIndex < TabPages.IndexOf(page),
				GoPrevious = SelectedIndex > TabPages.IndexOf(page)
			};

			base.SelectTab(page);
			DoPageShownAction(page, args);

			UpdateControlStates();
		}

#if COMPILE_NET40
		public void SuspendPage(Action action)
		{
			DiableButtons();

			IAsyncResult asyncResult = action.BeginInvoke(null, null);
			action.EndInvoke(asyncResult);

			EnableButtons();
		}

#else
		public async void SuspendPage(Action action)
		{ 
			DiableButtons();

			await Task.Run(action); 

			EnableButtons();
		}
#endif

		#endregion

		#region 事件处理

		/// <summary>
		/// 点击下一步的操作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void NextButton_Click(object sender, EventArgs e)
		{
			if (SelectedIndex == TabPages.Count - 1)
			{
				OnLastButtonClicked();
				return;
			}

			if (!DoNextMove(SelectedTab))
			{
				return;
			}

			UpdateControlStates();
		}


		/// <summary>
		/// 点击上一步的操作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void PreviousButton_Click(object sender, EventArgs e)
		{
			if (SelectedIndex == 0)
			{
				UpdateControlStates();
				return;
			}

			if (!DoPreviousMove(SelectedTab))
			{
				return;
			}

			UpdateControlStates();

		}

		#endregion

		#region 内部方法

		protected virtual void OnLastButtonClicked()
		{
			LastButtonClicked?.Invoke(this, new EventArgs());
		}

		protected bool DoNextMove(TabPage currentPage)
		{
			var args = new WizardButtonEventArgs()
			{
				CurrentIndex = TabPages.IndexOf(currentPage),
				CurrentTabPage = currentPage,
			};


			if (_nextCheck.ContainsKey(currentPage))
			{
				var funcList = _nextCheck[currentPage];
				var workBeforeGoNext = funcList[1] as Func<TabPage>;
				if (workBeforeGoNext == null)
				{
					#region 如果未指定跳转页，则检查能否跳转，通过则跳到下一页
					var canGoNext = funcList[0] as Func<bool>;
					if (canGoNext())
					{
						_lastTabIndex = SelectedIndex;
						SelectedIndex++;
					}
					else
					{
						return false;
					}
					#endregion

				}
				else
				{
					#region  如果指定了跳转页, 则在执行完操作后跳转到指定页
					var nextPage = workBeforeGoNext();
					if (nextPage != null)
					{
						_lastTabIndex = SelectedIndex;
						SelectedTab = nextPage;
					}
					else
					{
						return false;
					}
					#endregion
				}
			}
			else if (SelectedIndex > 0)
			{
				// 如果没有任何限定, 则默认跳转下一页
				_lastTabIndex = SelectedIndex;
				SelectedIndex++;
			}
			else
			{
				return false;
			}

			// 确定参数
			args.TargetIndex = SelectedIndex;
			args.TargetTabPage = TabPages[SelectedIndex];
			args.GoNext = args.CurrentIndex < args.TargetIndex;
			args.GoPrevious = args.CurrentIndex > args.TargetIndex;

			DoPageShownAction(SelectedTab, args);


			return true;
		}

		protected bool DoPreviousMove(TabPage currentPage)
		{
			var args = new WizardButtonEventArgs()
			{
				CurrentIndex = TabPages.IndexOf(currentPage),
				CurrentTabPage = currentPage,
			};

			if (_prevCheck.ContainsKey(currentPage))
			{
				var funcList = _prevCheck[currentPage];
				var beforeGoPrevious = funcList[1] as Func<TabPage>;
				if (beforeGoPrevious == null)
				{
					#region 如果未指定跳转页，则检查能否跳转，通过则跳到上一页
					var canGoPrevious = funcList[0] as Func<bool>;
					if (canGoPrevious())
					{
						_lastTabIndex = SelectedIndex;
						SelectedIndex--;
					}
					else
					{
						return false;
					}
					#endregion
				}
				else
				{
					#region  如果指定了跳转页, 则在执行完操作后跳转到指定页
					var previousPage = beforeGoPrevious();
					if (previousPage != null)
					{
						_lastTabIndex = SelectedIndex;
						SelectedTab = previousPage;
					}
					else
					{
						return false;
					}
					#endregion
				}
			}
			else if (SelectedIndex > 0)
			{
				_lastTabIndex = SelectedIndex;
				SelectedIndex--;
			}
			else
			{
				return false;
			}

			// 确定参数
			args.TargetIndex = SelectedIndex;
			args.TargetTabPage = TabPages[SelectedIndex];
			args.GoNext = args.CurrentIndex < args.TargetIndex;
			args.GoPrevious = args.CurrentIndex > args.TargetIndex;

			DoPageShownAction(SelectedTab, args);

			return true;
		}
		protected void DoPageShownAction(TabPage page, WizardButtonEventArgs args)
		{
			if (_pageDisplay.ContainsKey(page))
			{
				var action = _pageDisplay[page];
				if (action != null)
				{
					action(this, args);
				}
			}

		}

		/// <summary>
		/// 手动开启按钮
		/// </summary>
		protected void EnableButtons()
		{
			this.NextButton.Enabled = true;
			this.PreviousButton.Enabled = true;

			this.UpdateControlStates();
		}

		/// <summary>
		/// 手动关闭按钮
		/// </summary>
		protected void DiableButtons()
		{
			this.NextButton.Enabled = false;
			this.PreviousButton.Enabled = false;
		}


		#endregion
	}
}
