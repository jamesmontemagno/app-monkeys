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

namespace MonkeysApp.Activities
{
    [Activity(Name = "com.refractored.monkeysapp.DetailsActivity", Label = "Details", ParentActivity = typeof(MainActivity))]
    [IntentFilter(new []{ Intent.ActionView },
        Categories = new []
        {
            Android.Content.Intent.CategoryDefault,
            Android.Content.Intent.CategoryBrowsable
        },
        DataScheme = "http",
        DataHost = "www.monkeysapp.com",
        DataPathPrefix = "/Home/Detail/")]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "MonkeysApp.activities.HomeView")]
    public class DetailsActivity : AppCompatActivity
    {
        List<Monkey> friends;
        ImageLoader imageLoader;

        Monkey monkey;
        GoogleApiClient client;
        string url;
        string title;
        string description;
        string schemaType;

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            SetContentView(Resource.Layout.activity_detail);

            imageLoader = ImageLoader.Instance;

						
            friends = Util.GenerateFriends();
           

            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            if (monkey == null)
            { 
                var t = Intent.GetStringExtra("Name");
                monkey = friends.First(m => m.Name == t);
            }

            ShowMonkey();


            //client = new GoogleApiClient.Builder(this).AddApi(AppIndex.API).Build();
            url = $"http://monkeysapp.com/Home/Detail/{monkey.Name.Replace(" ", "%20")}";
            title = monkey.Name;
            description = monkey.Details;
            schemaType = "http://schema.org/Article";
        }

        void ShowMonkey()
        {
            var collapsingToolbar = FindViewById<CollapsingToolbarLayout>(Resource.Id.collapsing_toolbar);
            collapsingToolbar.SetTitle(monkey.Name);

            imageLoader.DisplayImage(monkey.Image, FindViewById<ImageView>(Resource.Id.friend_image));


            var detailsTextView = FindViewById<TextView>(Resource.Id.details);
            detailsTextView.Text = monkey.Details;

          
        }

        public Android.Gms.AppIndexing.Action AppIndexAction
        {
            get
            {
                var item = new Thing.Builder()
                .SetName(title)
                .SetDescription(description)
                .SetUrl(Android.Net.Uri.Parse(url))
                //.SetType(schemaType)
                .Build();

                var thing = new Android.Gms.AppIndexing.Action.Builder(Android.Gms.AppIndexing.Action.TypeView)
                    .SetObject(item)
                    .SetActionStatus(Android.Gms.AppIndexing.Action.StatusTypeCompleted)
                    .Build();

                return thing.JavaCast<Android.Gms.AppIndexing.Action>();
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            var action = intent.Action;
            var data = intent.DataString;
            if (Intent.ActionView != action || string.IsNullOrWhiteSpace(data))
                return;

            var monkeyId = data.Substring(data.LastIndexOf("/", StringComparison.Ordinal) + 1).Replace("%20", " ");

            monkey = friends.First(m => m.Name == monkeyId);

        }

        protected override async void OnStart()
        {
            base.OnStart();
            //client.Connect();
            //await AppIndex.AppIndexApi.StartAsync(client, AppIndexAction);
        }

        protected override async void OnStop()
        {
            //await AppIndex.AppIndexApi.EndAsync(client, AppIndexAction);
            //client.Disconnect();
            base.OnStop();
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