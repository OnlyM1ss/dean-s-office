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
    public class TeacherListPageViewModel : BindableBase
    {
        /// <summary>
        /// Initializes a new instance of the OrderListPageViewModel class.
        /// </summary>
        public TeacherListPageViewModel() => IsLoading = false;

        /// <summary>
        /// Gets the unfiltered collection of all orders. 
        /// </summary>
        private List<Teacher> MasterTeacherList { get; } = new List<Teacher>();

        /// <summary>
        /// Gets the orders to display.
        /// </summary>
        public ObservableCollection<Teacher> Teachers { get; private set; } = new ObservableCollection<Teacher>();

        private bool _isLoading;

        /// <summary>
        /// Gets or sets a value that specifies whether orders are being loaded.
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private Teacher _selectedTeacher;

        /// <summary>
        /// Gets or sets the selected order.
        /// </summary>
        public Teacher SelectedTeacher
        {
            get => _selectedTeacher;
            set
            {
                if (Set(ref _selectedTeacher, value))
                {
                    // Clear out the existing customer.
                    SelectedTeacher = null;
                    if (_selectedTeacher != null)
                    {
                        Task.Run(() => LoadCustomer(_selectedTeacher.Id));
                    }
                    OnPropertyChanged(nameof(SelectedTeacherGrandTotalFormatted));
                }
            }
        }
        public string TeacherName { get; set; }
        /// <summary>
        /// Gets a formatted version of the selected order's grand total value.
        /// </summary>
        public string SelectedTeacherGrandTotalFormatted;

        /// <summary>
        /// Loads the specified customer and sets the
        /// SelectedCustomer property.
        /// </summary>
        /// <param name="customerId">The customer to load.</param>
        private async void LoadCustomer(Guid teacherId)
        {
            var teacher = await App.Repository.Teachers.GetAsync(teacherId);
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                SelectedTeacher = teacher;
            });
        }

        /// <summary>
        /// Retrieves orders from the data source.
        /// </summary>
        public async void LoadTeachers()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                IsLoading = true;
                Teachers.Clear();
                MasterTeacherList.Clear();
            });
            var teachers = await Task.Run(App.Repository.Teachers.GetAsync);

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                foreach (var teacher in teachers)
                {
                    Teachers.Add(teacher);
                    MasterTeacherList.Add(teacher);
                }

                IsLoading = false;
            });
        }

        /// <summary>
        /// Submits a query to the data source.
        /// </summary>
        public async void QueryTeachers(string query)
        {
            IsLoading = true;
            Teachers.Clear();
            if (!string.IsNullOrEmpty(query))
            {
                var results = await App.Repository.Teachers.GetAsync(query);
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    //TODO: change type in repos 
                    //foreach (Teacher teacher in results)
                    //{
                    //    Teachers.Add(teacher);
                    //}
                    IsLoading = false;
                });
            }
        }

        /// <summary>
        /// Deletes the specified order from the database.
        /// </summary>
        public async Task DeleteTeacher(Teacher teacher) =>
            await App.Repository.Teachers.DeleteAsync(teacher.Id);

        /// <summary>
        /// Stores the order suggestions.
        /// </summary>
        public ObservableCollection<Teacher> TeacherSuggestions { get; } = new ObservableCollection<Teacher>();

        /// <summary>
        /// Queries the database and updates the list of new order suggestions.
        /// </summary>
        public void UpdateOrderSuggestions(string queryText)
        {
            TeacherSuggestions.Clear();
            if (!string.IsNullOrEmpty(queryText))
            {
                string[] parameters = queryText.Split(new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries);

                var resultList = MasterTeacherList
                    .Where(teacher => parameters
                        .Any(parameter =>
                            teacher.FirstName.StartsWith(parameter) ||
                            teacher.LastName.StartsWith(parameter)));

                foreach (Teacher teacher in resultList)
                {
                    TeacherSuggestions.Add(teacher);
                }
            }
        }
    }
}
