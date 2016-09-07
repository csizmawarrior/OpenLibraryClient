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

        public string LikeButtonName
        {
            get
            {
                //todo: Добавить логику
                return "Like";
            }
        }

        BookDetailsViewModel _book;

        public BookDetailsViewModel(BookViewModel bookViewModel)
        {
            _book = new BookDetailsViewModel(bookViewModel);
            LikeUnlikeCommand = new Tools.RelayCommand((arg) =>
            { LikeUnlike(); });
        }

        private void LikeUnlike()
        {

        }
    }
}
