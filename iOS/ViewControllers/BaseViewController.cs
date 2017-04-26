using System;
using System.Threading.Tasks;
using UIKit;
namespace FirebaseXamarin.iOS
{
    public class BaseViewController : UIViewController
    {
        public BaseViewController()
        {

        }

        public BaseViewController(IntPtr handle) : base(handle)
        {

        }

        // Displays a UIAlertView and returns the index of the button pressed.
        protected static Task<int> ShowAlert(string title, string message, params string[] buttons)
        {
            var tcs = new TaskCompletionSource<int>();
            var alert = new UIAlertView
            {
                Title = title,
                Message = message
            };
            foreach (var button in buttons)
                alert.AddButton(button);
            alert.Clicked += (s, e) => tcs.TrySetResult((int)e.ButtonIndex);
            alert.Show();
            return tcs.Task;
        }


        #region Loading Indicaor
        protected void showLoading(String title)
        {
            var bounds = UIScreen.MainScreen.Bounds;
            GPLoadingIndicator.showLoading(bounds, title);
        }

        protected void hideLoading()
        {
            GPLoadingIndicator.Hide();
        }
        #endregion

    }
}
