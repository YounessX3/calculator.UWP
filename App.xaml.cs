using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;

namespace AppPerfect
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Auto;

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }

            Window.Current.Activate();

            // Extend into title bar
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            ApplyTitleBarButtonColors();
        }

        public static void ApplyTitleBarButtonColors()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            var root = Window.Current.Content as FrameworkElement;
            bool isDark = root?.RequestedTheme == ElementTheme.Dark;

            Color backgroundColor = isDark ? Colors.Black : Colors.White;
            Color foregroundColor = isDark ? Colors.White : Colors.Black;
            Color hoverBackgroundColor = isDark ? Color.FromArgb(255, 30, 30, 30) : Color.FromArgb(255, 220, 220, 220);
            Color pressedBackgroundColor = isDark ? Color.FromArgb(255, 10, 10, 10) : Color.FromArgb(255, 200, 200, 200);
            Color inactiveForegroundColor = isDark ? Colors.Gray : Colors.DarkGray;

            titleBar.ButtonBackgroundColor = backgroundColor;
            titleBar.ButtonForegroundColor = foregroundColor;
            titleBar.ButtonHoverBackgroundColor = hoverBackgroundColor;
            titleBar.ButtonHoverForegroundColor = foregroundColor;
            titleBar.ButtonPressedBackgroundColor = pressedBackgroundColor;
            titleBar.ButtonPressedForegroundColor = foregroundColor;
            titleBar.ButtonInactiveBackgroundColor = backgroundColor;
            titleBar.ButtonInactiveForegroundColor = inactiveForegroundColor;
        }

        private void OnSuspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            // Handle suspension logic here
        }
    }
}
