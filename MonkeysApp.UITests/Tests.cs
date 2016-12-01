using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using Xamarin.UITest.Android;

namespace MonkeysApp.UITests
{
    [TestFixture]
    public class Tests
    {
        AndroidApp app;

        [SetUp]
        public void BeforeEachTest()
        {
            // TODO: If the Android app being tested is included in the solution then open
            // the Unit Tests window, right click Test Apps, select Add App Project
            // and select the app projects that should be tested.
            app = ConfigureApp
                .Android
                // TODO: Update this path to point to your Android app and uncomment the
                // code if the app is not included in the solution.
                .ApkFile ("../../../MonkeysApp/bin/Debug/com.refractored.monkeysapp.apk")
                .StartApp();
        }

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }

        [Test]
        public void AppNavigates()
        {
            app.Screenshot("First screen.");
            app.Tap("Baboon");
            app.WaitForElement("details");
            app.Screenshot("Details Page Visible");

        }

        [Test]
        public void Navigate_To_About()
        {
            app.Screenshot("First screen.");
            app.Tap("Navigate up");
            app.Screenshot("Navigation Drawer Open.");
            app.Tap("About");
            app.WaitForElement("profile_image");
            app.Screenshot("About Page Visibile.");
        }
    }
}

