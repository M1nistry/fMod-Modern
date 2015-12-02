using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using fMod.JSON;
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

            var lCollection = new LinkCollection(from c in categories
                select new Link
                {
                    DisplayName = c.Title,
                    Source = new Uri($"http://api.factoriomods.com/mods?category={c.Name}")
                });
            lCollection.Add(new Link
            {
                DisplayName = "All",
                Source = new Uri("http://api.factoriomods.com/mods")
            });
            var orderedCollection = lCollection.OrderBy(x => x.DisplayName).ToList();
            return new LinkCollection(from n in orderedCollection select n);
        }

        /// <summary>
        /// Asynchronously loads content from specified uri.
        /// </summary>
        /// <param name="uri">The content uri.</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The loaded content.</returns>
        public async Task<object> LoadContentAsync(Uri uri, CancellationToken ct)
        {
            var page = 0;
            var modList = new List<Mod>();
            var client = new HttpClient();
            while (true)
            {
                var symbol = uri.AbsoluteUri.Contains("?") ? "&" : "?";
                var result = await client.GetStringAsync(uri + $"{symbol}page={page++}");
                var mods = JsonConvert.DeserializeObject<Mod[]>(result);
                if (mods.Any())
                {
                    foreach (var mod in mods.Where(mod => !modList.Contains(mod)))
                    {
                        modList.Add(mod);
                    }
                    continue;
                }
                break;
            }
            
            var sv = new ScrollViewer
            {
                Content = modList.Aggregate(string.Empty, (current, mod) => current + (mod.title + Environment.NewLine))
            };
            return sv;
        }
    }
}
