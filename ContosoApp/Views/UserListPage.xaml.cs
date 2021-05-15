using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Contoso.App.ViewModels;
using Contoso.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Contoso.App.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class UserListPage : Page
    {
        public UserListPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// We use this object to bind the UI to our data. 
        /// </summary>
        public UserListPageViewModel ViewModel { get; } = new UserListPageViewModel();
        /// <summary>
        /// Retrieve the list of Users when the user navigates to the page. 
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ViewModel.Users.Count < 1)
            {
                ViewModel.LoadUsers();
            }
        }

        /// <summary>
        /// Opens the order in the order details page for editing. 
        /// </summary>
        private void EditButton_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(UserDetailPage), ViewModel.SelectedUser.Id);

        /// <summary>
        /// Deletes the currently selected order.
        /// </summary>
        private async void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var deletedeUserr = ViewModel.SelectedUser;
                await ViewModel.DeleteUser(deletedeUserr);
            }
            catch (/*UserDeletionException ex*/ Exception ex)
            {
                var dialog = new ContentDialog()
                {
                    Title = "Unable to delete user",
                    Content = $"There was an error when we tried to delete " +
                        $"invoice #{ViewModel.SelectedUser.Id}:\n{ex.Message}",
                    PrimaryButtonText = "OK"
                };
                await dialog.ShowAsync();
            }
        }

        /// <summary>
        /// Workaround to support earlier versions of Windows. 
        /// </summary>
        private void CommandBar_Loaded(object sender, RoutedEventArgs e)
        {
            //if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            //{
            //    (sender as CommandBar).DefaultLabelUser = CommandBarDefaultLabelUser.Bottom;
            //}
            //else
            //{
            //    (sender as CommandBar).DefaultLabelUser = CommandBarDefaultLabelUser.Right;
            //}
        }
        private void ViewUserButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserDetailPage), ((sender as FrameworkElement).DataContext as User).Id,
                new DrillInNavigationTransitionInfo());
        }

        /// <summary>
        /// Initializes the AutoSuggestBox portion of the search box.
        /// </summary>
        private void UserSearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is UserControls.CollapsibleSearchBox searchBox)
            {
                searchBox.AutoSuggestBox.QuerySubmitted += UserSearch_QuerySubmitted;
                searchBox.AutoSuggestBox.TextChanged += UserSearch_TextChanged;
                searchBox.AutoSuggestBox.PlaceholderText = "Search users...";
                searchBox.AutoSuggestBox.ItemTemplate = (DataTemplate)Resources["SearchSuggestionItemTemplate"];
                searchBox.AutoSuggestBox.ItemContainerStyle = (Style)Resources["SearchSuggestionItemStyle"];
            }
        }
        /// <summary>
        /// Searchs the list of orders.
        /// </summary>
        private void UserSearch_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args) =>
            ViewModel.QueryUsers(args.QueryText);

        /// <summary>
        /// Updates the suggestions for the AutoSuggestBox as the user types. 
        /// </summary>
        private void UserSearch_TextChanged(AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args)
        {
            // We only want to get results when it was a user typing, 
            // otherwise we assume the value got filled in by TextMemberPath 
            // or the handler for SuggestionChosen
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                ViewModel.UpdateOrderSuggestions(sender.Text);
            }
        }
        /// <summary>
        /// Searchs the list of user.
        /// </summary>
        private void OrderSearch_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args) =>
                ViewModel.QueryUsers(args.QueryText);

        /// <summary>
        /// Updates the suggestions for the AutoSuggestBox as the user types. 
        /// </summary>
        private void OrderSearch_TextChanged(AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args)
        {
            // We only want to get results when it was a user typing, 
            // otherwise we assume the value got filled in by TextMemberPath 
            // or the handler for SuggestionChosen
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                ViewModel.UpdateOrderSuggestions(sender.Text);
            }
        }

        /// <summary>
        /// Navigates to the User detail page when the user
        /// double-clicks an User. 
        /// </summary>
        private void DataGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) =>
            Frame.Navigate(typeof(UserDetailPage), ViewModel.SelectedUser.Id);

        // Navigates to the details page for the selected customer when the user presses SPACE.
        private void DataGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Space)
            {
                Frame.Navigate(typeof(UserDetailPage), ViewModel.SelectedUser.Id);
            }
        }

        /// <summary>
        /// Selects the tapped user. 
        /// </summary>
        private void DataGrid_RightTapped(object sender, RightTappedRoutedEventArgs e) =>
            ViewModel.SelectedUser = (e.OriginalSource as FrameworkElement).DataContext as User;

        /// <summary>
        /// Navigates to the user details page.
        /// </summary>
        private void MenuFlyoutViewDetails_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(UserDetailPage), ViewModel.SelectedUser.Id, new DrillInNavigationTransitionInfo());

        /// <summary>
        /// Sorts the data in the DataGrid.
        /// </summary>
        private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e) =>
            (sender as DataGrid).Sort(e.Column, ViewModel.Users.Sort);
    }
}
