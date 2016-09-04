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
            SearchQuery = "Hello world";
            Books = new List<Book>();
        }

        public void PerformSearch()
        {
            // Place search here
        }
    }
}
