﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.ViewModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View
{
    public sealed partial class UserListUserControl : UserControl
    {
        public GetUsersDetailViewModel GetUserDetailViewModel;
        public UserListUserControl()
        {
            GetUserDetailViewModel = new GetUsersDetailViewModel();
            this.InitializeComponent();
            Loaded += UserListControl_Loaded;
        }

        private void UserListControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GetUserDetailViewModel.UserIds.Clear();
            if (UserPollChoiceSelectionList != null)
            {
                foreach (var userPollChoiceSelection in UserPollChoiceSelectionList)
                {
                    GetUserDetailViewModel.UserIds.Add(userPollChoiceSelection.SelectedBy);
                }
            }

            if (UserIdList != null)
            {
                GetUserDetailViewModel.UserIds = UserIdList;
            }

            if (GetUserDetailViewModel.UserIds.Count > 0)
            {
                GetUserDetailViewModel.GetUsers();
                userList.Visibility = Visibility.Visible;
                NoUserFont.Visibility = Visibility.Collapsed;
            }
            else
            {
                userList.Visibility = Visibility.Collapsed;
                NoUserFont.Visibility = Visibility.Visible;
            }
        }

        public static readonly DependencyProperty UserIdListProperty = DependencyProperty.Register(
            nameof(UserIdList), typeof(List<string>), typeof(UserListUserControl), new PropertyMetadata(default(List<string>)));

        public List<string> UserIdList
        {
            get => (List<string>)GetValue(UserIdListProperty);
            set => SetValue(UserIdListProperty, value);
        }

        public static readonly DependencyProperty UserPollChoiceSelectionListProperty = DependencyProperty.Register(
            nameof(UserPollChoiceSelectionList), typeof(List<UserPollChoiceSelection>), typeof(UserListUserControl), new PropertyMetadata(default(List<UserPollChoiceSelection>)));

        public List<UserPollChoiceSelection> UserPollChoiceSelectionList
        {
            get => (List<UserPollChoiceSelection>)GetValue(UserPollChoiceSelectionListProperty);
            set => SetValue(UserPollChoiceSelectionListProperty, value);
        }
    }
}