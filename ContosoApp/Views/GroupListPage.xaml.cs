using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Contoso.App.ViewModels;
using Contoso.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Contoso.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GroupListPage : Page
    {
        public GroupListPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// We use this object to bind the UI to our data. 
        /// </summary>
        public GroupListPageViewModel ViewModel { get; } = new GroupListPageViewModel();
        /// <summary>
        /// Retrieve the list of Groups when the user navigates to the page. 
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ViewModel.Groups.Count < 1)
            {
                ViewModel.LoadGroups();
            }
        }

        /// <summary>
        /// Opens the order in the order details page for editing. 
        /// </summary>
        private void EditButton_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(GroupDetailPage), ViewModel.SelectedGroup.Id);

        /// <summary>
        /// Deletes the currently selected order.
        /// </summary>
        private async void DeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var deletedeGroupr = ViewModel.SelectedGroup;
                await ViewModel.DeleteGroup(deletedeGroupr);
            }
            catch (/*GroupDeletionException ex*/ Exception ex)
            {
                var dialog = new ContentDialog()
                {
                    Title = "Unable to delete group",
                    Content = $"There was an error when we tried to delete " +
                        $"invoice #{ViewModel.SelectedGroup.Id}:\n{ex.Message}",
                    PrimaryButtonText = "OK"
                };
                await dialog.ShowAsync();
            }
        }

        private void ViewGroupButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GroupDetailPage), ((sender as FrameworkElement).DataContext as Group).Id,
                new DrillInNavigationTransitionInfo());
        }
        /// <summary>
        /// Workaround to support earlier versions of Windows. 
        /// </summary>
        private void CommandBar_Loaded(object sender, RoutedEventArgs e)
        {
            //if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            //{
            //    (sender as CommandBar).DefaultLabelGroup = CommandBarDefaultLabelGroup.Bottom;
            //}
            //else
            //{
            //    (sender as CommandBar).DefaultLabelGroup = CommandBarDefaultLabelGroup.Right;
            //}
        }

        /// <summary>
        /// Initializes the AutoSuggestBox portion of the search box.
        /// </summary>
        private void GroupSearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is UserControls.CollapsibleSearchBox searchBox)
            {
                searchBox.AutoSuggestBox.QuerySubmitted += GroupSearch_QuerySubmitted;
                searchBox.AutoSuggestBox.TextChanged += GroupSearch_TextChanged;
                searchBox.AutoSuggestBox.PlaceholderText = "Search groups...";
                searchBox.AutoSuggestBox.ItemTemplate = (DataTemplate)Resources["SearchSuggestionItemTemplate"];
                searchBox.AutoSuggestBox.ItemContainerStyle = (Style)Resources["SearchSuggestionItemStyle"];
            }
        }
        /// <summary>
        /// Searchs the list of orders.
        /// </summary>
        private void GroupSearch_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args) =>
            ViewModel.QueryGroups(args.QueryText);

        /// <summary>
        /// Updates the suggestions for the AutoSuggestBox as the user types. 
        /// </summary>
        private void GroupSearch_TextChanged(AutoSuggestBox sender,
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
        /// Searchs the list of group.
        /// </summary>
        private void OrderSearch_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args) =>
                ViewModel.QueryGroups(args.QueryText);

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
        /// Navigates to the Group detail page when the user
        /// double-clicks an Group. 
        /// </summary>
        private void DataGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) =>
            Frame.Navigate(typeof(GroupDetailPage), ViewModel.SelectedGroup.Id);

        // Navigates to the details page for the selected customer when the user presses SPACE.
        private void DataGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Space)
            {
                Frame.Navigate(typeof(GroupDetailPage), ViewModel.SelectedGroup.Id);
            }
        }

        /// <summary>
        /// Selects the tapped group. 
        /// </summary>
        private void DataGrid_RightTapped(object sender, RightTappedRoutedEventArgs e) =>
            ViewModel.SelectedGroup = (e.OriginalSource as FrameworkElement).DataContext as Group;

        /// <summary>
        /// Navigates to the group details page.
        /// </summary>
        private void MenuFlyoutViewDetails_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(GroupDetailPage), ViewModel.SelectedGroup.Id, new DrillInNavigationTransitionInfo());

        /// <summary>
        /// Sorts the data in the DataGrid.
        /// </summary>
        private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e) =>
            (sender as DataGrid).Sort(e.Column, ViewModel.Groups.Sort);
    }
}
