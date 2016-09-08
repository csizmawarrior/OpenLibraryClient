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
        private FavoriteManager _favManager = new FavoriteManager();

        public BookDetailsModel(Book data)
        {
            _data = data;
        }

        public async Task<bool> IsFavorite()
        {
            return await IsFavorite(_data);
        }

        public async Task<bool> IsFavorite(Book book)
        {
            return await _favManager.IsFavorute(book);
        }

        public async Task AddToFavorites()
        {
            await _favManager.AddFavorite(_data);
        }

        public async Task RemoveFromFavorites()
        {
            await _favManager.RemoveFromFavorites(_data);
        }
    }
}
