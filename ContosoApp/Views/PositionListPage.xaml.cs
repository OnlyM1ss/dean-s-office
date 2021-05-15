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
    public sealed partial class PositionListPage : Page
    {
        public PositionListPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// We use this object to bind the UI to our data. 
        /// </summary>
        public PositionListPageViewModel ViewModel { get; } = new PositionListPageViewModel();
        /// <summary>
        /// Retrieve the list of Positions when the user navigates to the page. 
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ViewModel.Positions.Count < 1)
            {
                ViewModel.LoadPositions();
            }
        }

        /// <summary>
        /// Opens the order in the order details page for editing. 
        /// </summary>
        private void EditButton_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(PositionDetailPage), ViewModel.SelectedPosition.Id);

        /// <summary>
        /// Deletes the currently selected order.
        /// </summary>
        private async void DeletePosition_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var deletedePositionr = ViewModel.SelectedPosition;
                await ViewModel.DeletePosition(deletedePositionr);
            }
            catch (/*PositionDeletionException ex*/ Exception ex)
            {
                var dialog = new ContentDialog()
                {
                    Title = "Unable to delete position",
                    Content = $"There was an error when we tried to delete " +
                        $"invoice #{ViewModel.SelectedPosition.Id}:\n{ex.Message}",
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
            if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            {
                (sender as CommandBar).DefaultLabelPosition = CommandBarDefaultLabelPosition.Bottom;
            }
            else
            {
                (sender as CommandBar).DefaultLabelPosition = CommandBarDefaultLabelPosition.Right;
            }
        }

        /// <summary>
        /// Initializes the AutoSuggestBox portion of the search box.
        /// </summary>
        private void PositionSearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is UserControls.CollapsibleSearchBox searchBox)
            {
                searchBox.AutoSuggestBox.QuerySubmitted += PositionSearch_QuerySubmitted;
                searchBox.AutoSuggestBox.TextChanged += PositionSearch_TextChanged;
                searchBox.AutoSuggestBox.PlaceholderText = "Search positions...";
                searchBox.AutoSuggestBox.ItemTemplate = (DataTemplate)Resources["SearchSuggestionItemTemplate"];
                searchBox.AutoSuggestBox.ItemContainerStyle = (Style)Resources["SearchSuggestionItemStyle"];
            }
        }
        /// <summary>
        /// Searchs the list of orders.
        /// </summary>
        private void PositionSearch_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args) =>
            ViewModel.QueryPositions(args.QueryText);

        /// <summary>
        /// Updates the suggestions for the AutoSuggestBox as the user types. 
        /// </summary>
        private void PositionSearch_TextChanged(AutoSuggestBox sender,
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
        /// Searchs the list of position.
        /// </summary>
        private void OrderSearch_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args) =>
                ViewModel.QueryPositions(args.QueryText);

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
        /// Navigates to the Position detail page when the user
        /// double-clicks an Position. 
        /// </summary>
        private void DataGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) =>
            Frame.Navigate(typeof(PositionDetailPage), ViewModel.SelectedPosition.Id);

        // Navigates to the details page for the selected customer when the user presses SPACE.
        private void DataGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Space)
            {
                Frame.Navigate(typeof(PositionDetailPage), ViewModel.SelectedPosition.Id);
            }
        }

        /// <summary>
        /// Selects the tapped position. 
        /// </summary>
        private void DataGrid_RightTapped(object sender, RightTappedRoutedEventArgs e) =>
            ViewModel.SelectedPosition = (e.OriginalSource as FrameworkElement).DataContext as Position;

        /// <summary>
        /// Navigates to the position details page.
        /// </summary>
        private void MenuFlyoutViewDetails_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(PositionDetailPage), ViewModel.SelectedPosition.Id, new DrillInNavigationTransitionInfo());

        /// <summary>
        /// Sorts the data in the DataGrid.
        /// </summary>
        private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e) =>
            (sender as DataGrid).Sort(e.Column, ViewModel.Positions.Sort);
    }
}
