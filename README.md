# MonkeysApp-AppIndexing

This is a Sample application for Android built with Xamarin that showcases [App Indexing](https://developers.google.com/app-indexing/introduction) part of Google Play Services and Google Search. The app can be found on [Google Play](https://play.google.com/store/apps/details?id=com.refractored.monkeysapp) and the companion website can be found: http://monkeysapp.com/

### Getting Started
Before you can get started implementing App Indexing it is important that you have your **website live and that a version of your app is live on Google Play**. 

## Support HTTP URLs
Before we even integrate the App Indexing SDK we can supply a specific activity with new intent filters to handle specific data schemes. This is the fist step in associating a HTTP Url such as http://monkeysapp.com/Home/Detail/Baboon to navigate into the app's details activity instead of navigating to the website.

### Add Intent Filters
Intent filters are part of the app's manifest file and specify the type of intents that the activity would like to receive. These are common when an application would like other apps to directly start a specific activity in your app such as sharing text or photos. Filters in this case will tell App Indexing what types of URL data shemes your app can handle such as http and https. Filters are added as attributes to the Activity:

```csharp
[Activity(Name = "com.refractored.monkeysapp.DetailsActivity", Label = "Details", ParentActivity = typeof(MainActivity))]
[IntentFilter(new []{ Intent.ActionView },
        Categories = new []
        {
            Android.Content.Intent.CategoryDefault,
            Android.Content.Intent.CategoryBrowsable
        },
        DataScheme = "http",
        DataHost = "*.monkeysapp.com",
        DataPathPrefix = "/Home/Detail/")]
public class DetailsActivity : AppCompatActivity
{
    //...
}
```

If you need to support both http and https urls be sure to add another IntentFilter with the https DataScheme such as:

```csharp
[IntentFilter(new []{ Intent.ActionView },
    Categories = new []
    {
        Android.Content.Intent.CategoryDefault,
        Android.Content.Intent.CategoryBrowsable
    },
    DataScheme = "https",
    DataHost = "*.monkeysapp.com",
    DataPathPrefix = "/Home/Detail/")]
```

In this case I am specifying multiple data schemes, hosts, and prefixes to handle both http and https URLs. 
 
Here is a breakdown for http://monkeysapp.com/Home/Detail/Baboon
* Scheme: http
* Host: *.monkeysapp.com (this is enables both http://monkeysapp.com and http://www.monkeysapp.com or any subdomain)
* Path Prefix: This is what follows the main url and will lead to the monkeys identifier of Baboon. I could also have left off the / if my website used ?id=Baboon that I could parse in code.

### Handle Intent Filters
Now, when your app is started additional with this intent filter your app will recieve additional information that can be parsed to display information that was deep linked. This information will be part of the Intent's DataString. In this example we will find the Id of the monkey by finding the last "/" in the URL that is passed in. 

```csharp
Monkey monkey;
protected override void OnCreate(Android.OS.Bundle savedInstanceState)
{
    base.OnCreate(savedInstanceState);
    
    //Attempt to parse out monkey.
    OnNewIntent(Intent);
    
    SetContentView(Resource.Layout.activity_detail);
    if(monkey == null)
    {
      //Navigated from in app
    }
    else
    {
      //Navigated to from Intent Filter
    }
    //Updated UI with Monkey information
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
```

### Declare App & Website Association
This is probably the trickiest part of the process as you must have both your App setup in Google Play and also your Website registerd on Google Search. For this to work you must be the verified owner of your app in Google Play and it must be the same account that is managing the app in the Search Console. 

* From Search Console: Add and verify your app to Search Console (see [Search Console for Apps](https://support.google.com/webmasters/answer/6178088?hl=en&rd=1) for instructions). Go to [Associate a Website](https://www.google.com/webmasters/tools/app-associate-site), choose your verified app from the list, and enter the URL of the site or sites you want to associate with the app.
* From the Developer Console: Request to verify your website. This sends a message to your webmaster to associate your app to your site. See [App Indexing on Google Search](https://support.google.com/googleplay/android-developer/answer/6041489) on the Developer Console Help Center for details.

## Add App Indexing API
Now, it is time to setup App Indexing in your app. The API will enables you to provide specific information to App Indexing to help search results such as the type of actiivty (static content or video playback), title, description, and more. 

### Add Google Play services (GPS)
First, add the [App Indexing NuGet](https://www.nuget.org/packages/Xamarin.GooglePlayServices.AppIndexing) to your Xamarin.Android application.

Open your AndroidManifest.xml file and add the GPS meta-data for version number:
```xml
<application>
  ...
  <meta-data android:name="com.google.android.gms.version" android:value="@integer/google_play_services_version" />
  ...
</application>
```

### Bring in App Indexing namespaces

In the Activity that you wish to add the App Indexing APIs to add the following using statements:

```csharp
using Android.Gms.AppIndexing;
using Android.Gms.Common.Apis;
using Android.Runtime;
using IndexingAction = Android.Gms.AppIndexing.Action;
```

### Add App Indexing API calls
It is now time to setup the information that we want to supply to App Indexing that will enable serach results to show pertinent information. In this instance the information is dynamic based on the Monkey that we are viewing:

```csharp
public class DetailsActivity : AppCompatActivity
{
    Monkey monkey;
    GoogleApiClient client;
    string url, title, description, schemaType;
    protected override void OnCreate(Android.OS.Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        OnNewIntent(Intent);
        
        SetContentView(Resource.Layout.activity_detail);
        //Setup GoogleApiClient and get information
        client = new GoogleApiClient.Builder(this).AddApi(AppIndex.API).Build();
        url = $"http://monkeysapp.com/Home/Detail/{monkey.Name.Replace(" ", "%20")}";
        title = monkey.Name;
        description = monkey.Details;
        schemaType = "http://schema.org/Article";
    }
    
    // Generate the IndexingAction on the demand
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
}
```

### Indicate app activity
When the activity is create the new Indexing Action is created and can be sent to the App Indexing API.
```csharp
protected override async void OnStart()
{
    base.OnStart();
    client.Connect();
    await AppIndex.AppIndexApi.StartAsync(client, AppIndexAction);
}

protected override async void OnStop()
{
    await AppIndex.AppIndexApi.EndAsync(client, AppIndexAction);
    client.Disconnect();
    base.OnStop();
}
```

## Test Implementation
Test that your links open your app by using the Android Debug Bridge, where {DEEP-LINK} represents the URI declared in your app manifest. See Supported Deep Link Methods.
```
adb shell am start -a android.intent.action.VIEW -d "{DEEP-LINK}" com.example.android
```

Find more information on testing at: https://developers.google.com/app-indexing/android/test
