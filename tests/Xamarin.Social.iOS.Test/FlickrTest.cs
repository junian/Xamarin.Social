
using System;
using NUnit.Framework;
using MonoTouch.UIKit;
using MonoTouch.Accounts;
using Xamarin.Social.Services;
using System.Linq;

namespace Xamarin.Social.iOS.Test
{
	[TestFixture]
	public class FlickrTest
	{
		FlickrService CreateService ()
		{
			return new FlickrService () {
				ConsumerKey = "cd876d2995de61c8f57efb44520461e2",
				ConsumerSecret = "34bf6099d244db6a",
			};
		}

		[Test]
		public void Manual_Authenticate ()
		{
			var service = CreateService ();
			var vc = service.GetAuthenticateUI (account => {
				Console.WriteLine ("AUTHENTICATE RESULT = " + account);
				AppDelegate.Shared.RootViewController.DismissModalViewControllerAnimated (true);
			});
			AppDelegate.Shared.RootViewController.PresentViewController (vc, true, null);
		}
		
		[Test]
		public void Manual_ShareImageTextLinks ()
		{
			var service = CreateService ();
			
			var item = new Item {
				Text = "Hello, World",
			};
			item.Images.Add ("Images/what_does_that_mean_trollcat.jpg");
			item.Links.Add (new Uri ("http://xamarin.com"));
			item.Links.Add (new Uri ("https://twitter.com/xamarinhq"));
			
			var vc = service.GetShareUI (item, result => {
				Console.WriteLine ("SHARE RESULT = " + result);
				item.Dispose ();
				AppDelegate.Shared.RootViewController.DismissModalViewControllerAnimated (true);
			});
			AppDelegate.Shared.RootViewController.PresentViewController (vc, true, null);
		}

		[Test]
		public void PeopleGetPhotos ()
		{
			var service = CreateService ();

			var account = service.GetAccountsAsync ().Result.First ();

			var req = service.CreateRequest ("GET", new Uri ("http://www.flickr.com/services/rest"), account);
			req.Parameters["user_id"] = "me";
			req.Parameters["method"] = "flickr.people.getPhotos";

			var content = req.GetResponseAsync ().Result.GetResponseText ();

			Assert.That (content.Contains ("stat=\"ok\""));
		}
	}
}
