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
    public class BookDetailsViewModel : NotificationBase
    {
        public ICommand LikeUnlikeCommand
        {
            get;
            set;
        }

        private bool _isFav;
        public string LikeButtonName
        {
            get
            {
                if (_isFav)
                    return "Like";
                else
                    return "Unlike";
            }
        }

        BookDetailsModel _book;

        public BookDetailsViewModel(BookViewModel bookViewModel)
        {
            _book = new BookDetailsModel(bookViewModel);
            LikeUnlikeCommand = new Tools.RelayCommand((arg) =>
            { LikeUnlike(); });
        }

        private void LikeUnlike()
        {

        }

        private async Task AddToFavs()
        {
            await _book.AddToFavorites();
        }
    }
}
