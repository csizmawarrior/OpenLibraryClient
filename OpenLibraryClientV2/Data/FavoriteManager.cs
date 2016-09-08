using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace OpenLibraryClientV2.Data
{
    class FavoriteManager
    {
        StorageFolder folder = ApplicationData.Current.LocalFolder;
        StorageFolder favFolder;

        public FavoriteManager()
        {
            
        }

        public async Task Init()
        {
            favFolder = await folder.CreateFolderAsync("fav");
        }

        public async Task<List<string>> GetFavoritesList()
        {
            if (favFolder == null)
            {
                await Init();
            }

            List<string> favs = new List<string>();

            IReadOnlyList<StorageFolder> folderList = await favFolder.GetFoldersAsync();
            
            foreach (var f in folderList )
            {
                favs.Add(f.Name);
            }

            return favs;
        }

        public async Task<bool> IsFavorute(Book book)
        {
            List<string> favs = await GetFavoritesList();

            string bookPath = ComputeMD5(book.Key);
            foreach (var favName in favs)
            {
                if (bookPath == favName)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task AddFavorite(Book book)
        {
            if (!await IsFavorute(book))
            {
                string bookPath = ComputeMD5(book.Key);

                StorageFolder f = await favFolder.CreateFolderAsync(bookPath);

                await Book.WriteToFolder(book, f);
            }
        }

        public async Task RemoveFromFavorites(Book book)
        {
            if (await IsFavorute(book))
            {
                string bookPath = ComputeMD5(book.Key);

                StorageFolder f = await favFolder.GetFolderAsync(bookPath);

                await f.DeleteAsync();
            }
        }

        public static string ComputeMD5(string str)
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
        }
    }
}
