using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using OpenLibraryClientV2.Models;
using OpenLibraryClientV2.Data;

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

        public SearchViewModel()
        {
            searchModel = new SearchModel();

            BookViewModel b = new BookViewModel();
            b.Title = "Hello";

            b.PropertyChanged += Book_OnNotifyPropertyChanged;

            searchModel.Books.Add(b);
            Books.Add(b);
            
        }

        void Book_OnNotifyPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {

        }
    }
}
