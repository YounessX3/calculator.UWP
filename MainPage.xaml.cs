using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AppPerfect
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            UpdateHistoryPlaceholder();
        }

        // Public method to safely add history entries
        public void AddToHistory(string entry)
        {
            HistoryList.Items.Insert(0, entry);
            UpdateHistoryPlaceholder();
        }

        private void ClearHistory_Click(object sender, RoutedEventArgs e)
        {
            HistoryList.Items.Clear();
            UpdateHistoryPlaceholder();
        }


        private void UpdateHistoryPlaceholder()
        {
            if (HistoryList.Items.Count == 0)
            {
                HistoryList.Visibility = Visibility.Collapsed;
                HistoryPlaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                HistoryList.Visibility = Visibility.Visible;
                HistoryPlaceholder.Visibility = Visibility.Collapsed;
            }
        }


        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateHistoryPlaceholder();
            HistoryFlyout.ShowAt(HistoryButton);
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            var item = GetNavigationViewItem("basic");
            if (item != null)
            {
                NavView.SelectedItem = item;
                NavView_Navigate("basic");
            }

            ContentFrame.Navigated += On_Navigated;
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
                return;
            }

            var item = sender.MenuItems.OfType<NavigationViewItem>()
                .FirstOrDefault(x => x.Content?.ToString() == args.InvokedItem?.ToString());

            if (item != null)
            {
                var tag = item.Tag?.ToString();
                if (!string.IsNullOrEmpty(tag))
                {
                    NavView_Navigate(tag);
                }
            }
        }

        private NavigationViewItem GetNavigationViewItem(string tag)
        {
            return NavView.MenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(i => i.Tag?.ToString() == tag);
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            if (ContentFrame.SourcePageType == typeof(SettingsPage))
            {
                NavView.SelectedItem = NavView.SettingsItem as NavigationViewItem;
            }
            else if (_pageContents.TryGetValue(ContentFrame.SourcePageType, out string tag))
            {
                var item = GetNavigationViewItem(tag);
                if (item != null)
                {
                    NavView.SelectedItem = item;
                }
            }
        }

        private void NavView_Navigate(string tag)
        {
            var targetType = _pageContents
                .Where(c => c.Value == tag)
                .Select(c => c.Key)
                .FirstOrDefault();

            if (targetType != null)
            {
                ContentFrame.Navigate(targetType);
            }
        }

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }

        private static readonly IReadOnlyDictionary<Type, string> _pageContents = new Dictionary<Type, string>
        {
            { typeof(BasicCalculatorPage), "basic" },
            { typeof(ScientificCalculatorPage), "scientific" }
        };
    }
}
