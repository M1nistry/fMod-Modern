using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using fMod.JSON;
using FirstFloor.ModernUI.Windows.Controls;
using Newtonsoft.Json;

namespace fMod
{
    public class ModLoader : IContentLoader
    {
        /// <summary>
        /// Gets a collection of mod categories
        /// </summary>
        /// <returns></returns>
        public async Task<LinkCollection> GetCategories()
        {

            var webClient = new HttpClient();
            var result = await webClient.GetStringAsync("http://api.factoriomods.com/categories");
            var categories = JsonConvert.DeserializeObject<Category[]>(result);

            return new LinkCollection(from c in categories
                select new Link
                {
                    DisplayName = c.Title,
                    Source = new Uri($"http://api.factoriomods.com/mods?category={c.Name}")
                });
        }

        /// <summary>
        /// Asynchronously loads content from specified uri.
        /// </summary>
        /// <param name="uri">The content uri.</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The loaded content.</returns>
        public async Task<object> LoadContentAsync(Uri uri, CancellationToken ct)
        {
            // assuming uri is a valid image uri
            var client = new HttpClient();
            var result = await client.GetStringAsync(uri);
            var mods = JsonConvert.DeserializeObject<Mod[]>(result);
            return mods.Aggregate(string.Empty, (current, mod) => current + (mod.title + Environment.NewLine));
        }
    }
}
