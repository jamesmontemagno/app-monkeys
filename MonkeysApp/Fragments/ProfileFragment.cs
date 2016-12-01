using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using MonkeysApp.Activities;
using Fragment = Android.Support.V4.App.Fragment;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;
using UniversalImageLoader.Core;
using Plugin.Share;
using Android.Support.V7.Widget;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Crashes;

namespace MonkeysApp.Fragments
{
    public class ProfileFragment : Fragment
    {
		private ImageLoader imageLoader;
        public ProfileFragment()
        {
            this.RetainInstance = true;
        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_profile, null);
            imageLoader = ImageLoader.Instance;
            view.FindViewById<TextView>(Resource.Id.profile_name).Text = "James Montemagno";
			imageLoader.DisplayImage("https://s.gravatar.com/avatar/7d1f32b86a6076963e7beab73dddf7ca?s=250", view.FindViewById<ImageView>(Resource.Id.profile_image));

            view.FindViewById<ImageView>(Resource.Id.profile_image).Click += (sender, args) =>
                {
                    CrossShare.Current.OpenBrowser("http://github.com/jamesmontemagno");
                };


            view.FindViewById<CardView>(Resource.Id.copyright).Click += (sender, args) =>
                {
                   

                    CrossShare.Current.OpenBrowser("http://www.wikipedia.org");
                };
            view.FindViewById<CardView>(Resource.Id.copyright).LongClickable = true;

            view.FindViewById<CardView>(Resource.Id.copyright).LongClick += (sender, args) =>
            {
                Crashes.GenerateTestCrash();
            };

                view.FindViewById<TextView>(Resource.Id.website).Click += (sender, args) =>
                {
                    CrossShare.Current.OpenBrowser("http://motzcod.es");
                };

            view.FindViewById<TextView>(Resource.Id.twitter).Click += (sender, args) =>
                {
                    CrossShare.Current.OpenBrowser("http://m.twitter.com/jamesmontemagno");
                };
            return view;
        }

        /*var pendingIntent = Android.App.TaskStackBuilder.Create(Activity)
                                     .AddNextIntentWithParentStack(friendActivity)
                                     .GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);*/
    }
}