using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;

namespace Contoso.App.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public MainViewModel() => Task.Run(GetDisciplineListAsync);
        /// <summary>
        /// The collection of discipline in the list. 
        /// </summary>
        public ObservableCollection<DisciplineViewModel> Disciplines { get; }
         = new ObservableCollection<DisciplineViewModel>();

        private DisciplineViewModel _selectedDiscipline;
        /// <summary>
        /// Gets or sets the selected discipline, or null if no discipline is selected. 
        /// </summary>
        public DisciplineViewModel SelectedDiscipline
        {
            get => _selectedDiscipline;
            set => Set(ref _selectedDiscipline, value);
        }

        private bool _isLoading = false;

        /// <summary>
        /// Gets or sets a value indicating whether the discipline list is currently being updated. 
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        /// <summary>
        /// Gets the complete list of discipline from the database.
        /// </summary>
        public async Task GetDisciplineListAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => IsLoading = true);

            var disciplines = await App.Repository.Disciplines.GetAsync();
            if (disciplines == null)
            {
                return;
            }

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Disciplines.Clear();
                foreach (var discipline in disciplines)
                {
                    Disciplines.Add(new DisciplineViewModel());
                }
                IsLoading = false;
            });
        }

        /// <summary>
        /// Saves any modified customers and reloads the discipline list from the database.
        /// </summary>
        public void Sync()
        {
            Task.Run(async () =>
            {
                IsLoading = true;
                foreach (var modifiedCustomer in Disciplines
                    .Where(discipline => discipline.IsModified).Select(discipline => discipline.Model))
                {
                    await App.Repository.Disciplines.UpsertAsync(modifiedCustomer);
                }

                await GetDisciplineListAsync();
                IsLoading = false;
            });
        }
    }
}
