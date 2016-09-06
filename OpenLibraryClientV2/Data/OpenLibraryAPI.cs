using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web;
using Windows.Data.Json;

namespace OpenLibraryClientV2.Data
{
    public class Book
    {
        public String Title { get; set; }

        public Uri SmallImageUri { get; set; }

        public Uri LargeImageUri { get; set; }

        public List<string> FirstSentence { get; set; }

        public List<string> Authors { get; set; }

        public List<string> Subjects { get; set; }

        public static Book FromJsonObject(JsonObject jsonObject)
        {
            Book b = new Data.Book();

            b.Title = jsonObject.GetNamedString("title_suggest");
            b.Authors = new List<string>();

            if (jsonObject.ContainsKey("author_name"))
            {
                JsonArray array = jsonObject.GetNamedArray("author_name");
                foreach (var el in array)
                {
                    b.Authors.Add(el.GetString());
                }
            }

            b.FirstSentence = new List<string>();
            if (jsonObject.ContainsKey("first_sentence"))
            {
                JsonArray array = jsonObject.GetNamedArray("first_sentence");
                foreach (var el in array)
                {
                    b.FirstSentence.Add(el.GetString());
                }
            }

            b.Subjects = new List<string>();
            if (jsonObject.ContainsKey("subject"))
            {
                JsonArray array = jsonObject.GetNamedArray("subject");
                foreach (var el in array)
                {
                    b.Subjects.Add(el.GetString());
                }
            }

            if (jsonObject.ContainsKey("cover_i"))
            {
                b.SmallImageUri = new Uri("http://covers.openlibrary.org/b/id/" + ((int)jsonObject.GetNamedNumber("cover_i")).ToString() + "-S.jpg");
                b.LargeImageUri = new Uri("http://covers.openlibrary.org/b/id/" + ((int)jsonObject.GetNamedNumber("cover_i")).ToString() + "-L.jpg");
            }
            else
            {
                b.SmallImageUri = new Uri("ms-appx:/Assets/avatar_book-sm.png");
                b.LargeImageUri = new Uri("ms-appx:/Assets/avatar_book-sm.png");
            }

            return b;
        }
    }

    public class OpenLibraryAPI
    {
        public enum SearchField
        {
            Author,
            Title,
            Query
        }

        private HttpClient m_client = new HttpClient();

        public struct SearchResponse
        {
            public List<Book> books;
            public int start;
            public int numFound;
        }

        public async Task<SearchResponse> PerformSearch(string query, SearchField field, int page = 1)
        {
            UriBuilder uriBuilder = new UriBuilder("http://openlibrary.org/search.json");

            var kvp = new List<KeyValuePair<string, string>>();

            switch (field)
            {
                case SearchField.Author:
                    kvp.Add(new KeyValuePair<string, string>("author", query));
                    break;
                case SearchField.Title:
                    kvp.Add(new KeyValuePair<string, string>("title", query));
                    break;
                case SearchField.Query:
                    kvp.Add(new KeyValuePair<string, string>("q", query));
                    break;
            }

            kvp.Add(new KeyValuePair<string, string>("page", page.ToString()));

            using (var content = new HttpFormUrlEncodedContent(kvp.ToArray()))
            {
                uriBuilder.Query = content.ReadAsStringAsync().GetResults();
            }
            

            HttpResponseMessage response = new HttpResponseMessage();
            string responseBody = "";
            
            response = await m_client.GetAsync(uriBuilder.Uri);
            response.EnsureSuccessStatusCode();
            responseBody = await response.Content.ReadAsStringAsync();

            SearchResponse searchResponse = new SearchResponse();

            searchResponse.books = new List<Book>();

            JsonObject root = JsonObject.Parse(responseBody).GetObject();
            searchResponse.numFound = (int)root.GetNamedNumber("numFound");
            searchResponse.start = (int)root.GetNamedNumber("start");

            JsonArray docs = root.GetNamedArray("docs");
            for (int i = 0; i < docs.Count; ++i)
            {
                searchResponse.books.Add(Book.FromJsonObject(docs[i].GetObject()));
            }

            return searchResponse;
        }
    }
}
