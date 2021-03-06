using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;
using Microsoft.Toolkit.Uwp.Helpers;

namespace Contoso.App.ViewModels
{
    public class UserListPageViewModel : BindableBase
    {
        /// <summary>
        /// Initializes a new instance of the OrderListPageViewModel class.
        /// </summary>
        public UserListPageViewModel() => IsLoading = false;

        /// <summary>
        /// Gets the unfiltered collection of all orders. 
        /// </summary>
        private List<User> MasterUserList { get; } = new List<User>();

        /// <summary>
        /// Gets the orders to display.
        /// </summary>
        public ObservableCollection<User> Users { get; private set; } = new ObservableCollection<User>();

        private bool _isLoading;

        /// <summary>
        /// Gets or sets a value that specifies whether orders are being loaded.
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private User _selectedUser;

        /// <summary>
        /// Gets or sets the selected order.
        /// </summary>
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (Set(ref _selectedUser, value))
                {
                    // Clear out the existing customer.
                    SelectedUser = null;
                    if (_selectedUser != null)
                    {
                        Task.Run(() => LoadCustomer(_selectedUser.Id));
                    }

                    OnPropertyChanged(nameof(SelectedUserGrandTotalFormatted));
                }
            }
        }

        public string UserName { get; set; }

        /// <summary>
        /// Gets a formatted version of the selected order's grand total value.
        /// </summary>
        public string SelectedUserGrandTotalFormatted;

        /// <summary>
        /// Loads the specified customer and sets the
        /// SelectedCustomer property.
        /// </summary>
        /// <param name="customerId">The customer to load.</param>
        private async void LoadCustomer(Guid userId)
        {
            var user = await App.Repository.Users.GetAsync(userId);
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => { SelectedUser = user; });
        }

        /// <summary>
        /// Retrieves orders from the data source.
        /// </summary>
        public async void LoadUsers()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                IsLoading = true;
                Users.Clear();
                MasterUserList.Clear();
            });
            var users = await Task.Run(App.Repository.Users.GetAsync);

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                foreach (var user in users)
                {
                    Users.Add(user);
                    MasterUserList.Add(user);
                }

                IsLoading = false;
            });
        }

        /// <summary>
        /// Submits a query to the data source.
        /// </summary>
        public async void QueryUsers(string query)
        {
            IsLoading = true;
            Users.Clear();
            if (!string.IsNullOrEmpty(query))
            {
                var results = await App.Repository.Users.GetAsync(query);
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    //TODO: change type in repos 
                    //foreach (User user in results)
                    //{
                    //    Users.Add(user);
                    //}
                    IsLoading = false;
                });
            }
        }

        /// <summary>
        /// Deletes the specified order from the database.
        /// </summary>
        public async Task DeleteUser(User user) =>
            await App.Repository.Users.DeleteAsync(user.Id);

        /// <summary>
        /// Stores the order suggestions.
        /// </summary>
        public ObservableCollection<User> UserSuggestions { get; } = new ObservableCollection<User>();

        /// <summary>
        /// Queries the database and updates the list of new order suggestions.
        /// </summary>
        public void UpdateOrderSuggestions(string queryText)
        {
            UserSuggestions.Clear();
            if (!string.IsNullOrEmpty(queryText))
            {
                string[] parameters = queryText.Split(new char[] {' '},
                    StringSplitOptions.RemoveEmptyEntries);

                var resultList = MasterUserList
                    .Where(user => parameters
                        .Any(parameter =>
                            user.FirstName.StartsWith(parameter) ||
                            user.Login.StartsWith(parameter) ||
                            user.LastName.StartsWith(parameter)));

                foreach (User user in resultList)
                {
                    UserSuggestions.Add(user);
                }
            }
        }
    }
}