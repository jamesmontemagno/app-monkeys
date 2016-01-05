using Android.App;
using Android.Widget;
using Android.OS;
using Plugin.Share;

namespace IndexingTest
{
    [Activity(Label = "Indexing Test", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);
            
            button.Click += delegate
            {
                    CrossShare.Current.OpenBrowser("http://www.monkeysapp.com/Home/Detail/Baboon");
            };
            
            FindViewById<Button>(Resource.Id.myButton2).Click += delegate
                {
                    CrossShare.Current.OpenBrowser("http://monkeysapp.com/Home/Detail/Mandrill");
                };

            FindViewById<Button>(Resource.Id.myButton3).Click += delegate
                {
                    CrossShare.Current.OpenBrowser("http://monkeysapp.com");
                };

            FindViewById<Button>(Resource.Id.myButton4).Click += delegate
                {
                    CrossShare.Current.OpenBrowser("http://www.monkeysapp.com");
                };
        }
    }
}


