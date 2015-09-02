﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY.Pages;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        private ApplicationDataContainer appSetting;

        public SettingPage()
        {
            appSetting = ApplicationData.Current.LocalSettings; //本地存储

            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;//注册重写后退按钮事件
            //UmengSDK.UmengAnalytics.TrackPageStart("SettingPage");
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)//重写后退按钮，如果要对所有页面使用，可以放在App.Xaml.cs的APP初始化函数中重写。
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                e.Handled = true;
            }
        }
        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //UmengSDK.UmengAnalytics.TrackPageEnd("SettingPage");
            //HardwareButtons.BackPressed -= HardwareButtons_BackPressed;//注册重写后退按钮事件
        }

        private async void importKB2calendarButton_Click(object sender, RoutedEventArgs e)
        {
            var dig = new MessageDialog("订阅课表为实验室功能，我们无法保证此功能100%可用与数据100%正确性，我们期待您的反馈。\n\n是否继续尝试？", "警告");
            var btnOk = new UICommand("是");
            dig.Commands.Add(btnOk);
            var btnCancel = new UICommand("否");
            dig.Commands.Add(btnCancel);
            var result = await dig.ShowAsync();
            if (null != result && result.Label == "是")
            {
                Frame.Navigate(typeof(ImportKB2CalendarPage));
            }
            else if (null != result && result.Label == "否")
            {
            }
        }

        private void AboutAppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }

        private async void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            var dig = new MessageDialog("若应用无法使用，请尝试清除数据，清除数据后会自动退出应用。\n\n是否继续？", "警告");
            var btnOk = new UICommand("是");
            dig.Commands.Add(btnOk);
            var btnCancel = new UICommand("否");
            dig.Commands.Add(btnCancel);
            var result = await dig.ShowAsync();
            if (null != result && result.Label == "是")
            {
                appSetting.Values.Clear();
                IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
                IStorageFile storageFileWR = await applicationFolder.CreateFileAsync("kb", CreationCollisionOption.OpenIfExists);
                try
                {
                    await storageFileWR.DeleteAsync();
                }
                catch (Exception)
                {
                    Debug.WriteLine("设置 -> 重置应用异常");
                }
                Application.Current.Exit();
            }
            else if (null != result && result.Label == "否")
            {
            }
        }
    }
}
