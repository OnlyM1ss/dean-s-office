using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
    public sealed partial class DisciplineDetailPage : Page
    {
        public DisciplineDetailPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Used to bind the UI to the data.
        /// </summary>
        public DisciplineViewModel ViewModel { get; set; }

        /// <summary>
        /// Navigate to the previous page when the user cancels the creation of a new discipline record.
        /// </summary>
        private void AddNewDisciplineCanceled(object sender, EventArgs e) => Frame.GoBack();

        /// <summary>
        /// Displays the selected discipline data.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                ViewModel = new DisciplineViewModel
                {
                    IsNewDiscipline = true,
                    IsInEdit = true
                };
            }
            else
            {
                ViewModel = App.ViewModel.Disciplines.Where(
                    discipline => discipline.Model.Id == (Guid)e.Parameter).First();
            }

            ViewModel.AddNewDisciplineCanceled += AddNewDisciplineCanceled;
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Check whether there are unsaved changes and warn the user.
        /// </summary>
        protected async override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (ViewModel.IsModified)
            {
                // Cancel the navigation immediately, otherwise it will continue at the await call. 
                e.Cancel = true;

                void resumeNavigation()
                {
                    if (e.NavigationMode == NavigationMode.Back)
                    {
                        Frame.GoBack();
                    }
                    else
                    {
                        Frame.Navigate(e.SourcePageType, e.Parameter, e.NavigationTransitionInfo);
                    }
                }

                var saveDialog = new SaveChangesDialog() { Title = $"Save changes?" };
                await saveDialog.ShowAsync();
                SaveChangesDialogResult result = saveDialog.Result;

                switch (result)
                {
                    case SaveChangesDialogResult.Save:
                        await ViewModel.SaveAsync();
                        resumeNavigation();
                        break;
                    case SaveChangesDialogResult.DontSave:
                        await ViewModel.RevertChangesAsync();
                        resumeNavigation();
                        break;
                    case SaveChangesDialogResult.Cancel:
                        break;
                }
            }

            base.OnNavigatingFrom(e);
        }

        /// <summary>
        /// Disconnects the AddNewDisciplineCanceled event handler from the ViewModel 
        /// when the parent frame navigates to a different page.
        /// </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.AddNewDisciplineCanceled -= AddNewDisciplineCanceled;
            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Initializes the AutoSuggestBox portion of the search box.
        /// </summary>
        private void DisciplineSearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is UserControls.CollapsibleSearchBox searchBox)
            {
                searchBox.AutoSuggestBox.QuerySubmitted += DisciplineSearchBox_QuerySubmitted;
                searchBox.AutoSuggestBox.TextChanged += DisciplineSearchBox_TextChanged;
                searchBox.AutoSuggestBox.PlaceholderText = "Search disciplines...";
            }
        }

        /// <summary>
        /// Queries the database for a discipline result matching the search text entered.
        /// </summary>
        private async void DisciplineSearchBox_TextChanged(AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args)
        {
            // We only want to get results when it was a user typing,
            // otherwise we assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                // If no search query is entered, refresh the complete list.
                if (string.IsNullOrEmpty(sender.Text))
                {
                    sender.ItemsSource = null;
                }
                else
                {
                    sender.ItemsSource = await App.Repository.Disciplines.GetAsync(sender.Text);
                }
            }
        }

        /// <summary>
        /// Search by discipline name, email, or phone number, then display results.
        /// </summary>
        private void DisciplineSearchBox_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion is Discipline discipline)
            {
                Frame.Navigate(typeof(DisciplineDetailPage), discipline.Id);
            }
        }

        /// <summary>
        /// Adjust the command bar button label positions for optimimum viewing.
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

            // Disable dynamic overflow on this page. There are only a few commands, and it causes
            // layout problems when save and cancel commands are shown and hidden.
            (sender as CommandBar).IsDynamicOverflowEnabled = false;
        }

        /// <summary>
        /// Navigates to the discipline page for the discipline.
        /// </summary>
        private void ViewDisciplineButton_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(DisciplineDetailPage), ((sender as FrameworkElement).DataContext as Discipline).Id,
                new DrillInNavigationTransitionInfo());

        /// <summary>
        /// Adds a new discipline for the discipline.
        /// </summary>
        private void AddDiscipline_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(DisciplineDetailPage), ViewModel.Model.Id);

        /// <summary>
        /// Sorts the data in the DataGrid.
        /// </summary>
        private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e) =>
            (sender as DataGrid).Sort(e.Column, ViewModel.Disciplines.Sort);
    }
}
