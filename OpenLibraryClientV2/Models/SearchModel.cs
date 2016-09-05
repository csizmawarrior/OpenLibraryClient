using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenLibraryClientV2.Data;

namespace OpenLibraryClientV2.Models
{
    public class SearchModel
    {
        public List<Book> Books
        {
            get;
            set;
        }

        public String SearchQuery
        {
            get;
            set;
        }

        private OpenLibraryAPI m_api;

        public SearchModel()
        {
            m_api = new OpenLibraryAPI();
            Books = new List<Book>();
        }

        public async Task<OpenLibraryAPI.SearchResponse> PerformSearch()
        {
            return await m_api.PerformSearch(SearchQuery, OpenLibraryAPI.SearchField.Query);
        }
    }
}
