using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Mabo.Data.Collections
{
    public class Collection {
#pragma warning disable CS0618

        private string fileName;

        public Collection(string Name) {
            fileName = Name;
        }

        public async Task<T> InsertAsync<T>(T Item) {
            string content = await LoadAsync();
            List<T> items = await JsonConvert.DeserializeObjectAsync<List<T>>(content);
            items = items == null ? new List<T>() : items;

            (Item as CollectionItem).Id = Guid.NewGuid().ToString();
            items.Add(Item);

            content = await JsonConvert.SerializeObjectAsync(items);
            await SaveAsync(content);

            return Item;
        }

        public async Task<List<T>> SelectAsync<T>() {
            string content = await LoadAsync();
            List<T> items = await JsonConvert.DeserializeObjectAsync<List<T>>(content);
            items = items == null ? new List<T>() : items;

            return items;
        }

        public async Task<bool> DeleteAsync<T>(object Item) {
            string content = await LoadAsync();
            List<T> items = await JsonConvert.DeserializeObjectAsync<List<T>>(content);

            foreach (T item in items) {
                if ((item as CollectionItem).Id == (Item as CollectionItem).Id) {
                    items.Remove(item);
                    content = await JsonConvert.SerializeObjectAsync(items);
                    await SaveAsync(content);

                    return true;
                }
            }

            return false;
        }

        public async Task<bool> UpdateAsync<T>(object Item) {
            string content = await LoadAsync();
            List<T> items = await JsonConvert.DeserializeObjectAsync<List<T>>(content);

            int itemIndex = items.FindIndex(i => (i as CollectionItem).Id == (Item as CollectionItem).Id);
            if (itemIndex > -1) {
                items[itemIndex] = (T)Item;
                content = await JsonConvert.SerializeObjectAsync(items);
                await SaveAsync(content);

                return true;
            }

            return false;
        }

        private async Task<StorageFile> GetCollectionFile() {
            return await ApplicationData.Current.LocalFolder.CreateFileAsync("MABO.Data.Collections." + fileName + ".json", CreationCollisionOption.OpenIfExists);
        }

        private async Task<string> LoadAsync() {
            return await FileIO.ReadTextAsync(await GetCollectionFile(), UnicodeEncoding.Utf8);
        }

        private async Task<bool> SaveAsync(string Content) {
            try {
                await FileIO.WriteTextAsync(await GetCollectionFile(), Content, UnicodeEncoding.Utf8);
                return true;
            } catch {
                return false;
            }
        }

#pragma warning restore CS0618
    }

    public class CollectionItem {
        public string Id { get; set; }
    }
}
