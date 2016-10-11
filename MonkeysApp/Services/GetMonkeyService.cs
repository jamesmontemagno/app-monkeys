using Android.App;
using Android.Graphics.Drawables;
using Android.Service.QuickSettings;
using Android.Views;
using Java.Lang;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace MonkeysApp.Services
{
    [Service(Name = "com.refractored.monkeysapp.GetMonkeyService",
             Permission = Android.Manifest.Permission.BindQuickSettingsTile,
             Label = "@string/tile_name",
             Icon = "@drawable/ic_tile_default")]
    [IntentFilter(new[] { ActionQsTile })]
    public class GetMonkeyService : TileService
    {
        //TileService Lifecyle events : Passive Mode

        //First time tile is added to quick settings
        public override void OnTileAdded()
        {
            base.OnTileAdded();
        }

        //Called each time tile is visible
        public override void OnStartListening()
        {
            base.OnStartListening();

            //Tile associated with the service
            var tile = QsTile;

            //Update label, icon, description, and state

            tile.Icon = Icon.CreateWithResource(this, Resource.Drawable.ic_tile_default);
            tile.Label = GetString(Resource.String.tile_name);
            //tile.State = TileState.Active;
            tile.UpdateTile();

        }

        //Called when tile is no longer visible
        public override void OnStopListening()
        {
            base.OnStopListening();
        }

        //Called when tile is removed by the user
        public override void OnTileRemoved()
        {
            base.OnTileRemoved();
        }


        public override void OnClick()
        {
            base.OnClick();

            //Options:
            //1: Kick off background service
            //2: Open dialog (user needs more context)
            //3: Start activity/collapse

            //Remember: User can access tile if phone is locked
            //Check:
            if (IsLocked)
            {
                //Open Dialog is unavailable here

                //Option 1: Kick of background service


                //Option 2: Prompt user to unlock
                UnlockAndRun(new Runnable(() =>
                {
                    //Show Dialog when 
                    ShowDialog(MonkeyDialog);
                }));
            }
            else
            {
                //Do anything here.
                //Show Dialog
                ShowDialog(MonkeyDialog);
            }

            //Additionally, update tile if needed here.
        }

        Dialog MonkeyDialog
        {
            get
            {
                var alert = new AlertDialog.Builder(this, Resource.Style.DialogTheme);
                var inflater = (LayoutInflater)GetSystemService(LayoutInflaterService);
                var view = inflater.Inflate(Resource.Layout.alert_monkey, null);
                alert.SetView(view);
                alert.SetTitle("Monkey");

                alert.SetPositiveButton("OK", (senderAlert, args) =>
                { });
                return alert.Create();
            }
        }
    }
}
