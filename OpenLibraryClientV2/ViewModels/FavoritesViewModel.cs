using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using OpenLibraryClientV2.Data;

namespace OpenLibraryClientV2.ViewModels
{
    class FavoritesViewModel : NotificationBase
    {
        public ICommand BackCommand
        {
            get;
            set;
        }

        private ObservableCollection<BookViewModel> _books = new ObservableCollection<BookViewModel>();
        public ObservableCollection<BookViewModel> Books
        {
            get { return _books; }
            set { SetProperty(ref _books, value); }
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


        FavoriteManager favManager = new FavoriteManager();

        public FavoritesViewModel()
        {
            BackCommand = new Tools.RelayCommand((arg) =>
            { Tools.NavigationController.GetInstance().TryGoBack(); });

            GetBooks();
        }

        public async Task GetBooks()
        {
            List<string> names = await favManager.GetFavoritesList();

            foreach (var name in names)
            {
                Books.Add(new BookViewModel(await favManager.GetBook(name)));
            }
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
    }
}
