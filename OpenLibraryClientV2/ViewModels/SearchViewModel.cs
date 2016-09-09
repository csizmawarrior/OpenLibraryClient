using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using OpenLibraryClientV2.Models;
using OpenLibraryClientV2.Data;
using System.Windows.Input;

namespace OpenLibraryClientV2.ViewModels
{
    class SearchViewModel : NotificationBase
    {
        private SearchModel searchModel;

        // Для контроля текущих книг
        private ObservableCollection<BookViewModel> _books = new ObservableCollection<BookViewModel>();
        public ObservableCollection<BookViewModel> Books
        {
            get { return _books; }
            set { SetProperty(ref _books, value); }
        }

        public String SearchQuery
        {
            get { return searchModel.SearchQuery; }
            set { SetProperty(searchModel.SearchQuery, value, () => searchModel.SearchQuery = value);  }
        }

        private BookViewModel _selectedItem;
        public BookViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                
                BookListItemClicked(value);
            }
        }

        public ICommand PerformSearchCommand
        {
            get;
            set;
        }

        public ICommand TurnToFavoritesCommand
        {
            get;
            set;
        }

        public SearchViewModel()
        {
            searchModel = new SearchModel();;

            PerformSearchCommand = new Tools.RelayCommand((arg) =>
            { PerformSearch(); });

            TurnToFavoritesCommand = new Tools.RelayCommand((arg) =>
            { OpenFavoritesPage(); });
        }

        void Book_OnNotifyPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {

        }

        private void OpenFavoritesPage()
        {
            Tools.NavigationController.GetInstance().Navigate(typeof(Views.Favorites), new FavoritesViewModel());
        }

        private void BookListItemClicked(BookViewModel model)
        {
            if (model == null)
            {
                return;
            }

            Tools.NavigationController.GetInstance().Navigate(
                typeof(Views.BookDetailsView), 
                new BookDetailsViewModel(model)
            );
        }

        private async void PerformSearch()
        {
            OpenLibraryAPI.SearchResponse response = await searchModel.PerformSearch();

            Books.Clear();
            foreach (var book in response.books)
            {
                var b = new BookViewModel(book);

                b.PropertyChanged += Book_OnNotifyPropertyChanged;
                searchModel.Books.Add(b);
                Books.Add(b);
            }
        }
    }
}
