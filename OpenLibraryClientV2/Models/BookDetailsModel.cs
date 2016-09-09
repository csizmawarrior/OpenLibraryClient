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

        public string Title
        {
            get { return _data.Title; }
        }

        public Uri ImageUrl
        {
            get { return _data.LargeImageUri; }
        }

        public string Subjects
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < _data.Subjects.Count; ++i)
                {
                    builder.Append(_data.Subjects[i]);
                    if (i < _data.Subjects.Count - 1)
                    {
                        builder.Append(", ");
                    }
                }

                return builder.ToString();
            }
        }

        public string Excerpts
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < _data.FirstSentences.Count; ++i)
                {
                    builder.Append(_data.FirstSentences[i]);
                    if (i < _data.FirstSentences.Count - 1)
                    {
                        builder.Append('\n');
                    }
                }

                return builder.ToString();
            }
        }

        public string Authors
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < _data.Authors.Count; ++i)
                {
                    builder.Append(_data.Authors[i]);
                    if (i < _data.Authors.Count - 1)
                    {
                        builder.Append(", ");
                    }
                }

                return builder.ToString();
            }
        }

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
