using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

using MonkeysApp.Adapters;
using MonkeysApp.Models;
using UniversalImageLoader.Core;
using Android.Support.Design.Widget;
using Android.Support.V7.App;


using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using MonkeysApp.Helpers;

using Android.Gms.AppIndexing;
using Android.Gms.Common.Apis;
using Android.Runtime;
using IndexingAction = Android.Gms.AppIndexing.Action;
using Android.Content.PM;

namespace MonkeysApp.Activities
{
    
    [Activity(Name = "com.refractored.monkeysapp.DetailsActivity", Label = "Details", LaunchMode = LaunchMode.SingleTop, ParentActivity = typeof(MainActivity))]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "com.refactored.monkeysapp.MainActivity")]
    public class DetailsActivity : AppCompatActivity
    {
        List<Monkey> friends;
        ImageLoader imageLoader;

        Monkey monkey;
        GoogleApiClient client;
        string url;
        string title;
        string description;

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            SetContentView(Resource.Layout.activity_detail);

            friends = Util.GenerateFriends();
            imageLoader = ImageLoader.Instance;

            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            if (monkey == null)
            { 
                var t = Intent.GetStringExtra("Name");
                monkey = friends.First(m => m.Name == t);
            }

            ShowMonkey();


            client = new GoogleApiClient.Builder(this).AddApi(AppIndex.API).Build();
            url = $"http://monkeysapp.com/Home/Detail/{monkey.Name.Replace(" ", "%20")}";
            title = monkey.Name;
            description = monkey.Details;
        }

        void ShowMonkey()
        {
            if(monkey == null)
                return;

            Xamarin.Insights.Track("Details", "monkey", monkey.Name);
            
            var collapsingToolbar = FindViewById<CollapsingToolbarLayout>(Resource.Id.collapsing_toolbar);
            collapsingToolbar.SetTitle(monkey.Name);

            imageLoader.DisplayImage(monkey.Image, FindViewById<ImageView>(Resource.Id.friend_image));


            var detailsTextView = FindViewById<TextView>(Resource.Id.details);
            detailsTextView.Text = monkey.Details;

          
        }

        public IndexingAction AppIndexAction
        {
            get
            {
                var item = new Thing.Builder()
                .SetName(title)
                .SetDescription(description)
                .SetUrl(Android.Net.Uri.Parse(url))
                .Build();

                var thing = new IndexingAction.Builder(IndexingAction.TypeView)
                    .SetObject(item)
                    .SetActionStatus(IndexingAction.StatusTypeCompleted)
                    .Build();

                return thing.JavaCast<IndexingAction>();
            }
        }

       

        protected override async void OnStart()
        {
            base.OnStart();
            try
            {
                client.Connect();
                await AppIndex.AppIndexApi.StartAsync(client, AppIndexAction);
            }
            catch(Exception ex)
            {
            }
        }

        protected override async void OnStop()
        {
            
            base.OnStop();
            try
            {
                await AppIndex.AppIndexApi.EndAsync(client, AppIndexAction);
                client.Disconnect();
            }
            catch(Exception ex)
            {
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:

                    NavUtils.NavigateUpFromSameTask(this);
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}