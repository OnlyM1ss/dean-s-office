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
    public class DisciplineViewModel : BindableBase, IEditableObject
    {
        public DisciplineViewModel(Discipline model = null) => Model = model ?? new Discipline();
        private Discipline _model { get; set; }

        public Discipline Model
        {
            get => _model;
            set
            {
                if (_model != value)
                {
                    _model = value;
                    Task.Run((async () => await RefreshDisciplineAsync()));

                    // Raise the PropertyChanged event for all properties.
                    OnPropertyChanged(string.Empty);
                }
            }
        }
        public string Name
        {
            get => Model.Name;
            set
            {
                if (value != Model.Name)
                {
                    Model.Name = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        public int AcademyHours
        {
            get => Model.AcademyHours;
            set
            {
                if (value != Model.AcademyHours)
                {
                    Model.AcademyHours = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AcademyHours));
                }
            }
        }
        public bool IsModified { get; set; }
        public ObservableCollection<Discipline> Disciplines { get; } = new ObservableCollection<Discipline>();

        private Discipline _selectedDiscipline;
        /// <summary>
        /// Gets or sets the currently selected discipline.
        /// </summary>
        public Discipline SelectedDiscipline
        {
            get => _selectedDiscipline;
            set => Set(ref _selectedDiscipline, value);
        }

        private bool _isLoading;

        /// <summary>
        /// Gets or sets a value that indicates whether to show a progress bar. 
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private bool _isNewCustomer;

        /// <summary>
        /// Gets or sets a value that indicates whether this is a new customer.
        /// </summary>
        public bool IsNewCustomer
        {
            get => _isNewCustomer;
            set => Set(ref _isNewCustomer, value);
        }

        private bool _isInEdit = false;

        /// <summary>
        /// Called when a bound DataGrid control causes the customer to enter edit mode.
        /// </summary>
        public bool IsInEdit
        {
            get => _isInEdit;
            set => Set(ref _isInEdit, value);
        }

        /// <summary>
        /// Saves customer data that has been edited.
        /// </summary>
        public async Task SaveAsync()
        {
            IsInEdit = false;
            IsModified = false;
            if (IsNewCustomer)
            {
                IsNewCustomer = false;
                App.ViewModel.Disciplines.Add(this);
            }

            await App.Repository.Disciplines.UpsertAsync(Model);
        }

        /// <summary>
        /// Raised when the user cancels the changes they've made to the customer data.
        /// </summary>
        public event EventHandler AddNewCustomerCanceled;

        /// <summary>
        /// Cancels any in progress edits.
        /// </summary>
        public async Task CancelEditsAsync()
        {
            if (IsNewCustomer)
            {
                AddNewCustomerCanceled?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                await RevertChangesAsync();
            }
        }

        /// <summary>
        /// Discards any edits that have been made, restoring the original values.
        /// </summary>
        public async Task RevertChangesAsync()
        {
            IsInEdit = false;
            if (IsModified)
            {
                await RefreshDisciplineAsync();
                IsModified = false;
            }
        }

        /// <summary>
        /// Enables edit mode.
        /// </summary>
        public void StartEdit() => IsInEdit = true;
        /// <summary>
        /// Reloads all of the customer data.
        /// </summary>
        public async Task RefreshDisciplineAsync()
        {
            RefreshDiscipline();
            //Model = await App.Repository.Disciplines.GetAsync(Model.Id);
        }

        /// <summary>
        /// Resets the customer detail fields to the current values.
        /// </summary>
        public void RefreshDiscipline() => Task.Run(LoadDisciplineAsync);

        /// <summary>
        /// Loads the discipline data for the customer.
        /// </summary>
        public async Task LoadDisciplineAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                IsLoading = true;
            });

            var disciplines = await App.Repository.Disciplines.GetAsync(Model.Id);

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Disciplines.Clear();
                foreach (var discipline in Disciplines)
                {
                    Disciplines.Add(discipline);
                }

                IsLoading = false;
            });
        }

        /// <summary>
        /// Called when a bound DataGrid control causes the customer to enter edit mode.
        /// </summary>
        public void BeginEdit()
        {
            // Not used.
        }

        /// <summary>
        /// Called when a bound DataGrid control cancels the edits that have been made to a customer.
        /// </summary>
        public async void CancelEdit() => await CancelEditsAsync();

        /// <summary>
        /// Called when a bound DataGrid control commits the edits that have been made to a customer.
        /// </summary>
        public async void EndEdit() => await SaveAsync();
    }
}
