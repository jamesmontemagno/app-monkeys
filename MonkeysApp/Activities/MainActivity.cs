using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;

using MonkeysApp.Fragments;
using Android.Support.Design.Widget;
using MonkeysApp;
using Android.Content;
using System;
using Android.Support.V7.App;
using Android.Gms.Ads;

namespace MonkeysApp.Activities
{
    [Activity(Label = "Monkeys", Name="com.refractored.monkeysapp.MainActivity", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/ic_launcher")]
    #region Intent Filters
    [IntentFilter(new []{ Intent.ActionView },
        Categories = new []
        {
            Android.Content.Intent.CategoryDefault,
            Android.Content.Intent.CategoryBrowsable
        },
        DataScheme = "http",
        DataHost = "www.monkeysapp.com")]
    [IntentFilter(new []{ Intent.ActionView },
        Categories = new []
        {
            Android.Content.Intent.CategoryDefault,
            Android.Content.Intent.CategoryBrowsable
        },
        DataScheme = "http",
        DataHost = "www.monkeysapp.com",
        DataPathPrefix ="/Home/Detail/")]
    [IntentFilter(new []{ Intent.ActionView },
        Categories = new []
        {
            Android.Content.Intent.CategoryDefault,
            Android.Content.Intent.CategoryBrowsable
        },
        DataScheme = "http",
        DataHost = "monkeysapp.com")]
    [IntentFilter(new []{ Intent.ActionView },
        Categories = new []
        {
            Android.Content.Intent.CategoryDefault,
            Android.Content.Intent.CategoryBrowsable
        },
        DataScheme = "http",
        DataHost = "monkeysapp.com",
        DataPathPrefix ="/Home/Detail/")]
    #endregion
    public class MainActivity : BaseActivity
    {
        DrawerLayout drawerLayout;
        NavigationView navigationView;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.activity_main;
            }
        }

       

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);


            var id = Resources.GetString(Resource.String.admob_id);
            if(!string.IsNullOrWhiteSpace(id))
                MobileAds.Initialize(ApplicationContext, id);

          

            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);

                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_home:
                        ListItemClicked(0);
                        break;
                    case Resource.Id.nav_profile:
                        ListItemClicked(2);
                        break;
                }

				

                drawerLayout.CloseDrawers();
            };

           



            //if first time you will want to go ahead and click first item.
            if (savedInstanceState == null)
            {
                ListItemClicked(0);
            }

            OnNewIntent(Intent);


        }

        public override void OnBackPressed()
        {

            if (drawerLayout.IsDrawerOpen((int)GravityFlags.Start))
            {
                drawerLayout.CloseDrawer((int)GravityFlags.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }



        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            var action = intent.Action;
            var data = intent.DataString;
            if (Intent.ActionView != action || string.IsNullOrWhiteSpace(data))
                return;

            //only if deep linking
            if (!data.Contains("/Home/Detail/"))
                return;

            var monkeyId = data.Substring(data.LastIndexOf("/", StringComparison.Ordinal) + 1).Replace("%20", " ");

            if (!string.IsNullOrWhiteSpace(monkeyId))
            {
                var i = new Intent(this, typeof(DetailsActivity));
                i.PutExtra("Name", monkeyId);
                StartActivity(i);
            }
        }



        private void ListItemClicked(int position)
        {
            Android.Support.V4.App.Fragment fragment = null;
            switch (position)
            {
                case 0:
                    fragment = new BrowseFragment();
                    Title = "Monkeys";
                    break;
                case 2:
                    fragment = new ProfileFragment();
                    Title = "James Montemagno";
                    break;
            }

            SupportFragmentManager.BeginTransaction()
				.Replace(Resource.Id.content_frame, fragment)
				.Commit();
        }

	

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}

