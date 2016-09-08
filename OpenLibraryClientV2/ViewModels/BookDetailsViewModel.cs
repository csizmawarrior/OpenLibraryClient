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
        public ICommand LikeUnlikeBookCommand
        {
            get;
            set;
        }

        public ICommand BackCommand
        {
            get;
            set;
        }

        private bool _isFav;
        private string _likeButtonName;
        public string LikeButtonName
        {
            get { return _likeButtonName; }
            set { SetProperty(ref _likeButtonName, value); }
        }

        public string Title
        {
            get { return _book.Title; }
        }

        BookDetailsModel _book;

        public BookDetailsViewModel(BookViewModel bookViewModel)
        {
            _book = new BookDetailsModel(bookViewModel);
            LikeUnlikeBookCommand = new Tools.RelayCommand((arg) =>
            { LikeUnlike(); });

            BackCommand = new Tools.RelayCommand((arg) =>
            { Tools.NavigationController.GetInstance().TryGoBack(); });

            CheckIsFav();
        }

        private async Task CheckIsFav()
        {
            _isFav = await _book.IsFavorite();
            UpdateLikeButtonName();
        }

        private async void LikeUnlike()
        {
            if (_isFav)
            {
                _isFav = false;
                await _book.RemoveFromFavorites();
            }
            else
            {
                _isFav = true;
                await _book.AddToFavorites();
            }

            UpdateLikeButtonName();
        }

        private void UpdateLikeButtonName()
        {
            if (_isFav)
            {
                LikeButtonName = "Remove From Favs";
            }
            else
            {
                LikeButtonName = "Add To Favs";
            }
        }
    }
}
