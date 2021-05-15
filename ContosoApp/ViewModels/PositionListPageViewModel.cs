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
    public class PositionListPageViewModel : BindableBase
    {
        /// <summary>
        /// Initializes a new instance of the OrderListPageViewModel class.
        /// </summary>
        public PositionListPageViewModel() => IsLoading = false;

        /// <summary>
        /// Gets the unfiltered collection of all orders. 
        /// </summary>
        private List<Position> MasterPositionList { get; } = new List<Position>();

        /// <summary>
        /// Gets the orders to display.
        /// </summary>
        public ObservableCollection<Position> Positions { get; private set; } = new ObservableCollection<Position>();

        private bool _isLoading;

        /// <summary>
        /// Gets or sets a value that specifies whether orders are being loaded.
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private Position _selectedPosition;

        /// <summary>
        /// Gets or sets the selected order.
        /// </summary>
        public Position SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                if (Set(ref _selectedPosition, value))
                {
                    // Clear out the existing customer.
                    SelectedPosition = null;
                    if (_selectedPosition != null)
                    {
                        Task.Run(() => LoadCustomer(_selectedPosition.Id));
                    }
                    OnPropertyChanged(nameof(SelectedPositionGrandTotalFormatted));
                }
            }
        }
        public string PositionName { get; set; }
        /// <summary>
        /// Gets a formatted version of the selected order's grand total value.
        /// </summary>
        public string SelectedPositionGrandTotalFormatted;

        /// <summary>
        /// Loads the specified customer and sets the
        /// SelectedCustomer property.
        /// </summary>
        /// <param name="customerId">The customer to load.</param>
        private async void LoadCustomer(Guid PositionId)
        {
            var Position = await App.Repository.Positions.GetAsync(PositionId);
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                SelectedPosition = Position;
            });
        }

        /// <summary>
        /// Retrieves orders from the data source.
        /// </summary>
        public async void LoadPositions()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                IsLoading = true;
                Positions.Clear();
                MasterPositionList.Clear();
            });
            var positions = await Task.Run(App.Repository.Positions.GetAsync);

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                foreach (var position in Positions)
                {
                    Positions.Add(position);
                    MasterPositionList.Add(position);
                }

                IsLoading = false;
            });
        }

        /// <summary>
        /// Submits a query to the data source.
        /// </summary>
        public async void QueryPositions(string query)
        {
            IsLoading = true;
            Positions.Clear();
            if (!string.IsNullOrEmpty(query))
            {
                var results = await App.Repository.Positions.GetAsync(query);
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    //TODO: change type in repos 
                    //foreach (Position Position in results)
                    //{
                    //    Positions.Add(Position);
                    //}
                    IsLoading = false;
                });
            }
        }

        /// <summary>
        /// Deletes the specified order from the database.
        /// </summary>
        public async Task DeletePosition(Position Position) =>
            await App.Repository.Positions.DeleteAsync(Position.Id);

        /// <summary>
        /// Stores the order suggestions.
        /// </summary>
        public ObservableCollection<Position> PositionSuggestions { get; } = new ObservableCollection<Position>();

        /// <summary>
        /// Queries the database and updates the list of new order suggestions.
        /// </summary>
        public void UpdateOrderSuggestions(string queryText)
        {
            PositionSuggestions.Clear();
            if (!string.IsNullOrEmpty(queryText))
            {
                string[] parameters = queryText.Split(new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries);

                var resultList = MasterPositionList
                    .Where(Position => parameters
                        .Any(parameter =>
                            Position.Name.StartsWith(parameter) ||
                            Position.Rating.Equals(int.Parse(parameter))));

                foreach (Position Position in resultList)
                {
                    PositionSuggestions.Add(Position);
                }
            }
        }
    }
}
