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
    public sealed partial class TeacherListPage : Page
    {
        public TeacherListPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// We use this object to bind the UI to our data. 
        /// </summary>
        public TeacherListPageViewModel ViewModel { get; } = new TeacherListPageViewModel();
        /// <summary>
        /// Retrieve the list of Teachers when the user navigates to the page. 
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ViewModel.Teachers.Count < 1)
            {
                ViewModel.LoadTeachers();
            }
        }

        /// <summary>
        /// Opens the order in the order details page for editing. 
        /// </summary>
        private void EditButton_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(TeacherDetailPage), ViewModel.SelectedTeacher.Id);

        /// <summary>
        /// Deletes the currently selected order.
        /// </summary>
        private async void DeleteTeacher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var deletedeTeacherr = ViewModel.SelectedTeacher;
                await ViewModel.DeleteTeacher(deletedeTeacherr);
            }
            catch (/*TeacherDeletionException ex*/ Exception ex)
            {
                var dialog = new ContentDialog()
                {
                    Title = "Unable to delete teacher",
                    Content = $"There was an error when we tried to delete " +
                        $"invoice #{ViewModel.SelectedTeacher.Id}:\n{ex.Message}",
                    PrimaryButtonText = "OK"
                };
                await dialog.ShowAsync();
            }
        }
        private void ViewTeacherButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TeacherDetailPage), ((sender as FrameworkElement).DataContext as Teacher).Id,
                new DrillInNavigationTransitionInfo());
        }

        /// <summary>
        /// Workaround to support earlier versions of Windows. 
        /// </summary>
        private void CommandBar_Loaded(object sender, RoutedEventArgs e)
        {
            //if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            //{
            //    (sender as CommandBar).DefaultLabelTeacher = CommandBarDefaultLabelTeacher.Bottom;
            //}
            //else
            //{
            //    (sender as CommandBar).DefaultLabelTeacher = CommandBarDefaultLabelTeacher.Right;
            //}
        }

        /// <summary>
        /// Initializes the AutoSuggestBox portion of the search box.
        /// </summary>
        private void TeacherSearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is UserControls.CollapsibleSearchBox searchBox)
            {
                searchBox.AutoSuggestBox.QuerySubmitted += TeacherSearch_QuerySubmitted;
                searchBox.AutoSuggestBox.TextChanged += TeacherSearch_TextChanged;
                searchBox.AutoSuggestBox.PlaceholderText = "Search teachers...";
                searchBox.AutoSuggestBox.ItemTemplate = (DataTemplate)Resources["SearchSuggestionItemTemplate"];
                searchBox.AutoSuggestBox.ItemContainerStyle = (Style)Resources["SearchSuggestionItemStyle"];
            }
        }
        /// <summary>
        /// Searchs the list of orders.
        /// </summary>
        private void TeacherSearch_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args) =>
            ViewModel.QueryTeachers(args.QueryText);

        /// <summary>
        /// Updates the suggestions for the AutoSuggestBox as the user types. 
        /// </summary>
        private void TeacherSearch_TextChanged(AutoSuggestBox sender,
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
        /// Searchs the list of teacher.
        /// </summary>
        private void OrderSearch_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args) =>
                ViewModel.QueryTeachers(args.QueryText);

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
        /// Navigates to the Teacher detail page when the user
        /// double-clicks an Teacher. 
        /// </summary>
        private void DataGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) =>
            Frame.Navigate(typeof(TeacherDetailPage), ViewModel.SelectedTeacher.Id);

        // Navigates to the details page for the selected customer when the user presses SPACE.
        private void DataGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Space)
            {
                Frame.Navigate(typeof(TeacherDetailPage), ViewModel.SelectedTeacher.Id);
            }
        }

        /// <summary>
        /// Selects the tapped teacher. 
        /// </summary>
        private void DataGrid_RightTapped(object sender, RightTappedRoutedEventArgs e) =>
            ViewModel.SelectedTeacher = (e.OriginalSource as FrameworkElement).DataContext as Teacher;

        /// <summary>
        /// Navigates to the teacher details page.
        /// </summary>
        private void MenuFlyoutViewDetails_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(TeacherDetailPage), ViewModel.SelectedTeacher.Id, new DrillInNavigationTransitionInfo());

        /// <summary>
        /// Sorts the data in the DataGrid.
        /// </summary>
        private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e) =>
            (sender as DataGrid).Sort(e.Column, ViewModel.Teachers.Sort);
    }
}
