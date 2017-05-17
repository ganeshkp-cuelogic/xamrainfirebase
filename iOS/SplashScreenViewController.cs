using Foundation;
using System;
using UIKit;

namespace FirebaseXamarin.iOS
{
	public partial class SplashScreenViewController : BaseViewController
	{
		public SplashScreenViewController(IntPtr handle) : base(handle)
		{

		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			NSTimer.CreateScheduledTimer(TimeSpan.FromSeconds(3.0), (NSTimer timer) =>
			{
				AppDelegate.applicationDelegate().chooseWhereToGo();
			});
		}
	}
}