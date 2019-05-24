﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace SmaPriSample.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            //ここから追加
            var editText1 = FindViewById<TextInputEditText>(Resource.Id.textInputEditText1);
            var editText2 = FindViewById<TextInputEditText>(Resource.Id.textInputEditText2);
            var editText3 = FindViewById<TextInputEditText>(Resource.Id.textInputEditText3);
            var editText4 = FindViewById<TextInputEditText>(Resource.Id.textInputEditText4);
            var button = FindViewById<Button>(Resource.Id.button1);



            //buttonのラムダ式の中にサンプルのJavaコードを移植
            button.Click += async (sender, e) =>
            {
                var parameters = new Dictionary<string, string>()
                {
                    //{ "__format_archive_url", "file:///sdcard/Sample.spfmtz" }, //ファイルを読み込むにはPermissionの設定が必須
                    { "Text", editText1.Text },
                    { "Price", editText2.Text },
                    { "Barcode", editText3.Text },
                    { "(発行枚数)", editText4.Text },
                };

                using (var client = new HttpClient())
                {
                    //一気にURLをビルド、UTF-8エンコードして結果を取得できる。
                    //<remarks>C# 今更ですが、HttpClientを使う - Qiita https://qiita.com/rawr/items/f78a3830d894042f891b</remarks>
                    var response = await client.GetAsync($"http://localhost:8080/Format/Print?{await new FormUrlEncodedContent(parameters).ReadAsStringAsync()}");

                    System.Diagnostics.Debug.WriteLine(response);
                }
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

