using System;
using CoreGraphics;
using Foundation;
using UIKit;
namespace FirebaseXamarin.iOS.Views
{
	public class GPTextView : UITextView
	{
		/// <summary>
		/// Gets or sets the placeholder to show prior to editing - doesn't exist on UITextView by default
		/// </summary>
		public string Placeholder { get; set; }

		public GPTextView()
		{
			InitializeTextView();
		}

		[Export("initWithCoder:")]
		public GPTextView(NSCoder coder)
		{
			InitializeTextView();
		}

		public GPTextView(CGRect frame)
			: base(frame)
		{
			InitializeTextView();
		}

		public GPTextView(IntPtr handle)
			: base(handle)
		{
			InitializeTextView();
		}

		public void InitializeTextView()
		{
			Placeholder = "Enter Message";
			Text = Placeholder;

			ShouldBeginEditing = t =>
			{
				if (Text == Placeholder)
					Text = string.Empty;

				return true;
			};

			ShouldEndEditing = t =>
			{
				if (string.IsNullOrEmpty(Text))
					Text = Placeholder;

				return true;
			};
		}

		public void callMethod()
		{

		}
	}
}
