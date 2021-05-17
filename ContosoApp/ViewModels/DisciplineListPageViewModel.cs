//  ---------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//
//  The MIT License (MIT)
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  ---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Contoso.Models;
using Microsoft.Toolkit.Uwp.Helpers;

namespace Contoso.App.ViewModels
{
    /// <summary>
    /// Encapsulates data for the OrderListPage. The page UI
    /// binds to the properties defined here.
    /// </summary>
    public class DisciplineListPageViewModel : BindableBase
    {
        /// <summary>
        /// Initializes a new instance of the OrderListPageViewModel class.
        /// </summary>
        public DisciplineListPageViewModel() => IsLoading = false;

        /// <summary>
        /// Gets the unfiltered collection of all disciplines. 
        /// </summary>
        private List<Discipline> MasterDisciplineList { get; } = new List<Discipline>();

        /// <summary>
        /// Gets the discipline to display.
        /// </summary>
        public ObservableCollection<Discipline> Disciplines { get; private set; } = new ObservableCollection<Discipline>();

        private bool _isLoading;

        /// <summary>
        /// Gets or sets a value that specifies whether discipline are being loaded.
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private Discipline _selectedDiscipline;

        /// <summary>
        /// Gets or sets the selected discipline.
        /// </summary>
        public Discipline SelectedDiscipline
        {
            get => _selectedDiscipline;
            set
            {
                if (Set(ref _selectedDiscipline, value))
                {
                    // Clear out the existing discipline.
                    //SelectedDiscipline = null;
                    if (_selectedDiscipline != null)
                    {
                        Task.Run(() => LoadDiscipline(_selectedDiscipline.Id));
                    }
                    OnPropertyChanged(nameof(SelectedDisciplineGrandTotalFormatted));
                }
            }
        }
        public string DisciplineName { get; set; }
        /// <summary>
        /// Gets a formatted version of the selected discipline's grand total value.
        /// </summary>
        public string SelectedDisciplineGrandTotalFormatted;

        /// <summary>
        /// Loads the specified customer and sets the
        /// SelectedCustomer property.
        /// </summary>
        /// <param name="customerId">The customer to load.</param>
        private async void LoadDiscipline(Guid disciplineId)
        {
            var discipline = await App.Repository.Disciplines.GetAsync(disciplineId);
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                SelectedDiscipline = discipline;
            });
        }

        /// <summary>
        /// Retrieves disciplines from the data source.
        /// </summary>
        public async void LoadDisciplines()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                IsLoading = true;
                Disciplines.Clear();
                MasterDisciplineList.Clear();
            });
            var disciplines = await Task.Run(App.Repository.Disciplines.GetAsync);

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                foreach (var discipline in disciplines)
                {
                    Disciplines.Add(discipline);
                    MasterDisciplineList.Add(discipline);
                }

                IsLoading = false;
            });
        }

        /// <summary>
        /// Submits a query to the data source.
        /// </summary>
        public async void QueryDisciplines(string query)
        {
            IsLoading = true;
            Disciplines.Clear();
            if (!string.IsNullOrEmpty(query))
            {
                //var results = await App.Repository.Disciplines.GetAsync(query);
                //await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                //{
                //    TODO: change type in repos
                //    foreach (Discipline discipline in results)
                //    {
                //        Disciplines.Add(discipline);
                //    }
                //    IsLoading = false;
                //});
            }
        }

        /// <summary>
        /// Deletes the specified discipline from the database.
        /// </summary>
        public async Task DeleteDiscipline(Discipline discipline) =>
            await App.Repository.Disciplines.DeleteAsync(discipline.Id);

        /// <summary>
        /// Stores the discipline suggestions.
        /// </summary>
        public ObservableCollection<Discipline> DisciplineSuggestions { get; } = new ObservableCollection<Discipline>();

        /// <summary>
        /// Queries the database and updates the list of new discipline suggestions.
        /// </summary>
        public void UpdateOrderSuggestions(string queryText)
        {
            DisciplineSuggestions.Clear();
            if (!string.IsNullOrEmpty(queryText))
            {
                string[] parameters = queryText.Split(new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries);

                var resultList = MasterDisciplineList
                    .Where(discipline => parameters
                        .Any(parameter =>
                            discipline.Name.StartsWith(parameter)));

                foreach (Discipline discipline in resultList)
                {
                    DisciplineSuggestions.Add(discipline);
                }
            }
        }
    }
}