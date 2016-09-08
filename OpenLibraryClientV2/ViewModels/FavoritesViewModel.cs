using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private List<BookViewModel> _books = new List<BookViewModel>();
        public List<BookViewModel> Books
        {
            get { return _books; }
            set { SetProperty(ref _books, value); }
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
    }
}
