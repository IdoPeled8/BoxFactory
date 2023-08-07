using FactoryLogic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace BoxFactory
{
    public static class JsonFileSerialize
    {
        static StorageFile Path;
        static string fileNameLibraryJson = "BoxFaxtory06.json";

        private static readonly JsonSerializerSettings _options = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public async static void SimpleWrite(SortedSet<Box> boxTree)
        {
            var libraryData = boxTree;

            // serialize JSON to a string
            string jsonLibraryData = JsonConvert.SerializeObject(libraryData);

            // write string to a file
            Path = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileNameLibraryJson, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(Path, jsonLibraryData);
        }

        public static async Task<SortedSet<Box>> SimpleRead()
        {
            Path = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileNameLibraryJson, CreationCollisionOption.OpenIfExists);

            string jsonItemList = await FileIO.ReadTextAsync(Path);

            if (string.IsNullOrEmpty(jsonItemList))
            {
                throw new FileNotFoundException();
            }


            SortedSet<Box> boxTree = JsonConvert.DeserializeObject<SortedSet<Box>>(jsonItemList);

            return boxTree;

        }
    }
}
