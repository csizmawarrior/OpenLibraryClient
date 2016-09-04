using System;
using OpenLibraryClientV2.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OpenLibraryClientV2.ViewModels
{
    class BookViewModel : NotificationBase<Book>
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        public String Title
        {
            get { return This.Title; }
            set { SetProperty(This.Title, value, () => This.Title = value); }
        }

        public BookViewModel(Book book = null) :
            base(book)
        {

        }
    }
}
