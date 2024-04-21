﻿using CollapseLauncher.Helper.LauncherApiLoader.Sophon;
using CollapseLauncher.Helper.Metadata;
using Hi3Helper;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using static Hi3Helper.Shared.Region.LauncherConfig;

namespace CollapseLauncher.Pages
{
    public sealed partial class HomePage : Page
    {
        string GameDirPath { get => CurrentGameProperty._GameVersion.GameDirPath; }

#nullable enable
        private LauncherGameNewsData? GameNewsData { get => LauncherMetadataHelper.CurrentMetadataConfig?.GameLauncherApi?.LauncherGameNews?.Content; }
        public bool IsPostPanelAvailable => (GameNewsData?.NewsPost?.Count ?? 0) > 0;
        public bool IsCarouselPanelAvailable => (GameNewsData?.NewsCarousel?.Count ?? 0) > 0;

        public Visibility IsPostEventPanelVisible => (GameNewsData?.NewsPostTypeActivity?.Count ?? 0) == 0 ? Visibility.Collapsed : Visibility.Visible;
        public Visibility IsPostEventPanelEmpty => (GameNewsData?.NewsPostTypeActivity?.Count ?? 0) != 0 ? Visibility.Collapsed : Visibility.Visible;
        public Visibility IsPostNoticePanelVisible => (GameNewsData?.NewsPostTypeAnnouncement?.Count ?? 0) == 0 ? Visibility.Collapsed : Visibility.Visible;
        public Visibility IsPostNoticePanelEmpty => (GameNewsData?.NewsPostTypeAnnouncement?.Count ?? 0) != 0 ? Visibility.Collapsed : Visibility.Visible;
        public Visibility IsPostInfoPanelVisible => (GameNewsData?.NewsPostTypeInfo?.Count ?? 0) == 0 ? Visibility.Collapsed : Visibility.Visible;
        public Visibility IsPostInfoPanelEmpty => (GameNewsData?.NewsPostTypeInfo?.Count ?? 0) != 0 ? Visibility.Collapsed : Visibility.Visible;
        public int PostEmptyMascotTextWidth => Locale.Lang._HomePage.PostPanel_NoNews.Length > 30 ? 200 : 100;
#nullable restore

        public bool IsEventsPanelShow
        {
            get
            {
                bool ret = GetAppConfigValue("ShowEventsPanel").ToBoolNullable() ?? true;
                return ret;
            }
            set
            {
                SetAndSaveConfigValue("ShowEventsPanel", value);
                ToggleEventsPanel(value);
            }
        }

        public bool IsSocMedPanelShow
        {
            get
            {
                bool ret = GetAppConfigValue("ShowSocialMediaPanel").ToBoolNullable() ?? true;
                return ret;
            }
            set
            {
                SetAndSaveConfigValue("ShowSocialMediaPanel", value);
                ToggleSocmedPanelPanel(value);
            }
        }

        public string NoNewsSplashMascot
        {
            get
            {
                GameNameType gameType = CurrentGameProperty._GamePreset.GameType;
                return gameType switch
                {
                    GameNameType.Honkai => "ms-appx:///Assets/Images/GameMascot/AiShocked.png",
                    GameNameType.StarRail => "ms-appx:///Assets/Images/GameMascot/PomPomWhat.png",
                    GameNameType.Zenless => "ms-appx:///Assets/Images/GameMascot/BangbooShocked.png",
                    _ => "ms-appx:///Assets/Images/GameMascot/PaimonWhat.png"
                };
            }
        }

        public void ToggleEventsPanel(bool hide) => HideImageCarousel(!hide);
        public void ToggleSocmedPanelPanel(bool hide) => HideSocialMediaPanel(!hide);
    }

    public class NullVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string input) => (bool)value ? Visibility.Visible : Visibility.Collapsed;
        public object ConvertBack(object value, Type targetType, object parameter, string input) => new NotImplementedException();
    }
}
