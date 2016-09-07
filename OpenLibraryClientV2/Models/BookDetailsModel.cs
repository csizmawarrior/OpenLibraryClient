using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenLibraryClientV2.Data;
using OpenLibraryClientV2.ViewModels;


namespace OpenLibraryClientV2.Models
{
    class BookDetailsModel
    {
        private Book _data;

        public BookDetailsModel(Book data)
        {
            _data = data;
        }

        public bool IsFavorite(Book book)
        {

        }
    }
}
