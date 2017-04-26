using System;
using CoreGraphics;
using UIKit;

namespace FirebaseXamarin.iOS
{
    public class GPLoadingIndicator : UIView
    {
        public GPLoadingIndicator()
        {

        }

        private static GPLoadingIndicator loadingIndicator;

        // control declarations
        UIActivityIndicatorView activitySpinner;
        UILabel loadingLabel;

        public static void showLoading(CGRect frame, String message)
        {
            loadingIndicator = new GPLoadingIndicator(frame, message);
            AppDelegate.applicationDelegate().Window.Add(loadingIndicator);
        }

        public GPLoadingIndicator(CGRect frame, String message) : base(frame)
        {
            // configurable bits
            BackgroundColor = UIColor.Black;
            Alpha = 0.75f;
            AutoresizingMask = UIViewAutoresizing.All;

            nfloat labelHeight = 22;
            nfloat labelWidth = Frame.Width - 20;

            // derive the center x and y
            nfloat centerX = Frame.Width / 2;
            nfloat centerY = Frame.Height / 2;

            // create the activity spinner, center it horizontall and put it 5 points above center x
            activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
            activitySpinner.Frame = new CGRect(
                centerX - (activitySpinner.Frame.Width / 2),
                centerY - activitySpinner.Frame.Height - 20,
                activitySpinner.Frame.Width,
                activitySpinner.Frame.Height);
            activitySpinner.AutoresizingMask = UIViewAutoresizing.All;
            AddSubview(activitySpinner);
            activitySpinner.StartAnimating();

            // create and configure the "Loading Data" label
            loadingLabel = new UILabel(new CGRect(
                centerX - (labelWidth / 2),
                centerY + 5,
                labelWidth,
                labelHeight
                ));
            loadingLabel.BackgroundColor = UIColor.Clear;
            loadingLabel.TextColor = UIColor.White;
            loadingLabel.Text = message + "...";
            loadingLabel.TextAlignment = UITextAlignment.Center;
            loadingLabel.AutoresizingMask = UIViewAutoresizing.All;
            AddSubview(loadingLabel);

        }

        /// <summary>
        /// Fades out the control and then removes it from the super view
        /// </summary>
        public static void Hide()
        {
            UIView.Animate(
                0.5, // durations
                () => { loadingIndicator.Alpha = 0; },
                () => { loadingIndicator.RemoveFromSuperview(); }
            );
        }
    }
}
