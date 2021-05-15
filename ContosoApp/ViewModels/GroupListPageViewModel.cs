using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;
using Microsoft.Toolkit.Uwp.Helpers;

namespace Contoso.App.ViewModels
{
    public class GroupListPageViewModel : BindableBase
    {
        /// <summary>
        /// Initializes a new instance of the OrderListPageViewModel class.
        /// </summary>
        public GroupListPageViewModel() => IsLoading = false;

        /// <summary>
        /// Gets the unfiltered collection of all orders. 
        /// </summary>
        private List<Group> MasterGroupList { get; } = new List<Group>();

        /// <summary>
        /// Gets the orders to display.
        /// </summary>
        public ObservableCollection<Group> Groups { get; private set; } = new ObservableCollection<Group>();

        private bool _isLoading;

        /// <summary>
        /// Gets or sets a value that specifies whether orders are being loaded.
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private Group _selectedGroup;

        /// <summary>
        /// Gets or sets the selected order.
        /// </summary>
        public Group SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                if (Set(ref _selectedGroup, value))
                {
                    // Clear out the existing customer.
                    SelectedGroup = null;
                    if (_selectedGroup != null)
                    {
                        Task.Run(() => LoadCustomer(_selectedGroup.Id));
                    }
                    OnPropertyChanged(nameof(SelectedGroupGrandTotalFormatted));
                }
            }
        }
        public string GroupName { get; set; }
        /// <summary>
        /// Gets a formatted version of the selected order's grand total value.
        /// </summary>
        public string SelectedGroupGrandTotalFormatted;

        /// <summary>
        /// Loads the specified customer and sets the
        /// SelectedCustomer property.
        /// </summary>
        /// <param name="customerId">The customer to load.</param>
        private async void LoadCustomer(Guid GroupId)
        {
            var Group = await App.Repository.Groups.GetAsync(GroupId);
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                SelectedGroup = Group;
            });
        }

        /// <summary>
        /// Retrieves orders from the data source.
        /// </summary>
        public async void LoadGroups()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                IsLoading = true;
                Groups.Clear();
                MasterGroupList.Clear();
            });
            var groups = await Task.Run(App.Repository.Groups.GetAsync);

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                foreach (var group in Groups)
                {
                    Groups.Add(group);
                    MasterGroupList.Add(group);
                }

                IsLoading = false;
            });
        }

        /// <summary>
        /// Submits a query to the data source.
        /// </summary>
        public async void QueryGroups(string query)
        {
            IsLoading = true;
            Groups.Clear();
            if (!string.IsNullOrEmpty(query))
            {
                var results = await App.Repository.Groups.GetAsync(query);
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    //TODO: change type in repos 
                    //foreach (Group Group in results)
                    //{
                    //    Groups.Add(Group);
                    //}
                    IsLoading = false;
                });
            }
        }

        /// <summary>
        /// Deletes the specified order from the database.
        /// </summary>
        public async Task DeleteGroup(Group Group) =>
            await App.Repository.Groups.DeleteAsync(Group.Id);

        /// <summary>
        /// Stores the order suggestions.
        /// </summary>
        public ObservableCollection<Group> GroupSuggestions { get; } = new ObservableCollection<Group>();

        /// <summary>
        /// Queries the database and updates the list of new order suggestions.
        /// </summary>
        public void UpdateOrderSuggestions(string queryText)
        {
            GroupSuggestions.Clear();
            if (!string.IsNullOrEmpty(queryText))
            {
                string[] parameters = queryText.Split(new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries);

                var resultList = MasterGroupList
                    .Where(Group => parameters
                        .Any(parameter =>
                            Group.Faculty.StartsWith(parameter) ||
                            Group.Count.Equals(int.Parse(parameter))));

                foreach (Group Group in resultList)
                {
                    GroupSuggestions.Add(Group);
                }
            }
        }
    }

}
