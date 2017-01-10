

using System;
using System.Collections.Generic;

using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

using MonkeysApp.Activities;
using MonkeysApp.Adapters;
using MonkeysApp.Models;
using MonkeysApp.Helpers;
using Android.Gms.Ads;

namespace MonkeysApp.Fragments
{
    public class BrowseFragment : Fragment
    {

        public BrowseFragment()
        {
            RetainInstance = true;
        }
		List<Monkey> friends;
        public override View OnCreateView(LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
			base.OnCreateView(inflater, container, savedInstanceState);

          	HasOptionsMenu = true;
            var view = inflater.Inflate(Resource.Layout.fragment_browse, null);

            var grid = view.FindViewById<GridView>(Resource.Id.grid);
            friends = Util.GenerateFriends();
            grid.Adapter = new MonkeyAdapter(Activity, friends);
            grid.ItemClick += GridOnItemClick;

            var adView = view.FindViewById<AdView>(Resource.Id.adView);
            
            var adRequest = new AdRequest.Builder().Build();
            
            adView.LoadAd(adRequest);
            return view;
        }

        void GridOnItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        {
            var intent = new Intent(Activity, typeof(DetailsActivity));
            intent.PutExtra("Name", friends[itemClickEventArgs.Position].Name);
            StartActivity(intent);
        }

       
    }
}