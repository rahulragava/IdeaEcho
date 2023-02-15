﻿using Windows.UI.Xaml.Controls;
using SocialMediaApplication.Models.BusinessModels;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SocialMediaApplication.Presenter.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UserAlreadyLoggedIn();
        }

        public void UserAlreadyLoggedIn()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values["user"] == null)
            {
                this.Frame.Navigate(typeof(SignUpPage));
            }
            else
            {
                //work page of the app
                //this.Frame.Navigate(typeof());
            }
        }


        //to signin/singnup/logout
        //public void Dummy()
        //{
        //    UserBObj userBObj = new UserBObj();
        //    var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        //    localSettings.Values["user"] = userBObj;


        //    localSettings.Values.Remove("user");
        //}
    }
}
