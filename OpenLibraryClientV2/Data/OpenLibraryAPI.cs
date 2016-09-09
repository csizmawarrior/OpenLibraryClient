using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Foundation;
using Windows.Storage;
using Windows.Web;
using Windows.Data.Json;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage.Streams;

namespace OpenLibraryClientV2.Data
{
    public class Book
    {
        public String Title { get; set; }

        public Uri SmallImageUri { get; set; }

        public Uri LargeImageUri { get; set; }

        public string Key { get; set; }

        public List<string> FirstSentences { get; set; }

        public List<string> Authors { get; set; }

        public List<string> Subjects { get; set; }

        public async static Task WriteToFolder(Book b, StorageFolder folder)
        {
            // todo: Fix for internet defects + optimize async functions

            JsonObject obj = new JsonObject();
            StorageFile imageFile = await folder.CreateFileAsync("image.jpg", CreationCollisionOption.OpenIfExists);
            
            if (b.LargeImageUri != new Uri("ms-appx:/Assets/avatar_book-sm.png"))
            {
                HttpClient client = new HttpClient();

                HttpResponseMessage response = new HttpResponseMessage();

                response = await client.GetAsync(b.LargeImageUri);
                response.EnsureSuccessStatusCode();

                // todo: Monkey Coding Style. Fix this shit.
                if (response.Headers.ContainsKey("Location"))
                {
                    response = await client.GetAsync(new Uri(response.Headers["Location"]));
                    response.EnsureSuccessStatusCode();
                }

                var responseBody = await response.Content.ReadAsBufferAsync();
                await FileIO.WriteBufferAsync(imageFile, responseBody);

                obj["cover_local"] = JsonValue.CreateStringValue(imageFile.Path);
            }
            else
                obj["cover_local"] = JsonValue.CreateStringValue("ms-appx:/Assets/avatar_book-sm.png");
           
            obj["key"] = JsonValue.CreateStringValue(b.Key);
            obj["title_suggest"] = JsonValue.CreateStringValue(b.Title);

            // Forming Authors
            {
                JsonArray authors = new JsonArray();
                foreach (var author in b.Authors)
                {
                    authors.Add(JsonValue.CreateStringValue(author));
                }

                obj["author_name"] = authors;
            }

            // Forming Subjects
            {
                JsonArray subjects = new JsonArray();
                foreach (var subject in b.Subjects)
                {
                    subjects.Add(JsonValue.CreateStringValue(subject));
                }

                obj["subject"] = subjects;
            }

            // Sentence
            {
                JsonArray firstArray = new JsonArray();
                foreach (var sentence in b.FirstSentences)
                {
                    firstArray.Add(JsonValue.CreateStringValue(sentence));
                }

                obj["first_sentence"] = firstArray;
            }


            StorageFile file = await folder.CreateFileAsync("data.json", CreationCollisionOption.OpenIfExists);
            
            await FileIO.WriteTextAsync(file, obj.ToString());
        }

        public async static Task<Book> ReadFromFolder(StorageFolder folder)
        {
            StorageFile file = await folder.GetFileAsync("data.json");

            string data = await FileIO.ReadTextAsync(file);

            Book b = Book.FromJsonObject(JsonObject.Parse(data));

            return b;
        }

        public static Book FromJsonObject(JsonObject jsonObject)
        {
            Book b = new Data.Book();

            b.Key = jsonObject.GetNamedString("key");
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

            b.FirstSentences = new List<string>();
            if (jsonObject.ContainsKey("first_sentence"))
            {
                JsonArray array = jsonObject.GetNamedArray("first_sentence");
                foreach (var el in array)
                {
                    b.FirstSentences.Add(el.GetString());
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
                // For local loading
                if (jsonObject.ContainsKey("cover_local"))
                {
                    b.SmallImageUri = new Uri(jsonObject.GetNamedString("cover_local"));
                    b.LargeImageUri = new Uri(jsonObject.GetNamedString("cover_local"));
                }
                else
                {
                    b.SmallImageUri = new Uri("ms-appx:/Assets/avatar_book-sm.png");
                    b.LargeImageUri = new Uri("ms-appx:/Assets/avatar_book-sm.png");
                }
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
