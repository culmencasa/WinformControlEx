using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace System.Drawing
{

	///<summary>
	///This class is implemented so that from any other class it can be called, and by passing the correct parameters
	///from any object, it will animate the object passed. Parameters required are:
	///(sender[do not modify], timer1[do not modify and must exist], X Destination, Y Destination, Animation Type, Duration) or as noted below.
	///From the remote class add this to the object's eventHandler:
	///TweenLibrary.startTweenEvent(sender, timer1, 300, randint(), "easeinoutcubic", 20); :-mm
	///</summary>
	public class TweenLibrary {			
		private  int counter = 0;
		private  int timeStart;
		private  int timeDest;
		private  string animType;
		
		private  float t;
		private  float d;
		private  float b;
		private  float c;

		private  int[] Arr_startPos	= new int[]{0,0};
		private  int[] Arr_destPos	= new int[]{0,0};

		private  System.Windows.Forms.Timer	objTimer;
		private  System.Windows.Forms.Control objHolder;

		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Timer timer1;

        public event Action JobDone;

		///<summary>
		///this method kicks off the process
		///</summary>
		public  void startTweenEvent(object _objHolder, int _destXpos, int _destYpos, string _animType, int _timeInterval){
				
				//inits the parameters for the tween process
				counter		= 0;
				timeStart	= counter;
				timeDest	= _timeInterval;
				animType	= _animType;

				this.components			= new System.ComponentModel.Container();
				this.timer1				= new System.Windows.Forms.Timer(this.components);
				this.timer1.Interval	= 1;
				this.timer1.Tick		+= new System.EventHandler(this.timer1_Tick);
				
				//Manages the object passed in to be tweened. 
				//I create a new instance of a control and then force the object to convert to 
				//a control. Doing it this way, the method accepts ANY control, 
				//rather than hard-coding "Button" or some other specific control.
				objHolder	= new System.Windows.Forms.Control();
				objHolder	= (Control) _objHolder;
				objTimer	= this.timer1;
				
				//initializes the object's position in the pos Arrays
				Arr_startPos[0]	= objHolder.Location.X;
				Arr_startPos[1]	= objHolder.Location.Y;
				Arr_destPos[0]	= _destXpos;
				Arr_destPos[1]	= _destYpos;

				//resets the timer and finally starts it
				objTimer.Stop();
				objTimer.Enabled = false;
				objTimer.Enabled = true;
		}

		///<summary>
		///This is the method that gets called every tick interval
		///</summary>
		public  void timer1_Tick(object sender, System.EventArgs e) {
			if(objHolder.Location.X == Arr_destPos[0] && objHolder.Location.Y == Arr_destPos[1]) {
				objTimer.Stop();
				objTimer.Enabled = false;
                if (JobDone != null)
                {
                    JobDone();                
                }
			}else{
				objHolder.Location = new System.Drawing.Point(tween(0), tween(1));
				counter++;
			}
		}

		///<summary>
		///This method returns a value from the tween formula.
		///</summary>
		private  int tween(int prop){
			t = (float)counter - timeStart;
			b = (float)Arr_startPos[prop];
			c = (float)Arr_destPos[prop] - Arr_startPos[prop];
			d = (float)timeDest - timeStart;
            
			return getFormula(animType, t, b, d, c);
		}

		///<summary>
		///this method selects which formula to pick and then returns a number for the tween position of the pictureBox
		///</summary>
		private  int	getFormula(string animType, float t, float b, float d, float c){
			//adjust formula to selected algoritm from combobox
			switch (animType) {
				case "linear":
					// simple linear tweening - no easing 
					return (int)(c*t/d+b);

				case "easeinquad":
					// quadratic (t^2) easing in - accelerating from zero velocity
					return (int)(c*(t/=d)*t + b);
						
				case "easeoutquad":
					// quadratic (t^2) easing out - decelerating to zero velocity
					return (int)(-c*(t=t/d)*(t-2)+b);
						
				case "easeinoutquad":
					// quadratic easing in/out - acceleration until halfway, then deceleration
					if ((t/=d/2)<1) return (int)(c/2*t*t+b); else return (int)(-c/2*((--t)*(t-2)-1)+b);
						
				case "easeincubic":
					// cubic easing in - accelerating from zero velocity
					return (int)(c*(t/=d)*t*t + b);

				case "easeoutcubic":
					// cubic easing in - accelerating from zero velocity
					return (int)(c*((t=t/d-1)*t*t + 1) + b);
						
				case "easeinoutcubic":
					// cubic easing in - accelerating from zero velocity
					if ((t/=d/2) < 1)return (int)(c/2*t*t*t+b);else return (int)(c/2*((t-=2)*t*t + 2)+b);

				case "easeinquart":
					// quartic easing in - accelerating from zero velocity
					return (int)(c*(t/=d)*t*t*t + b);

				case "easeinexpo":
					// exponential (2^t) easing in - accelerating from zero velocity
					if (t==0) return (int)b; else return (int)(c*Math.Pow(2,(10*(t/d-1)))+b);
						
				case "easeoutexpo":
					// exponential (2^t) easing out - decelerating to zero velocity
					if (t==d) return (int)(b+c); else return (int)(c * (-Math.Pow(2,-10*t/d)+1)+b);

				default:
					return 0;
			}
		}
	}
}