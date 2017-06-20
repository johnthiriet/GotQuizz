using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GotQuizz.Models;
using Newtonsoft.Json;

namespace GotQuizz.Services
{
    public class GotApiService
    {
#if DEBUG
        private static Task<string> LoadTestJsonAsync(string fileName)
        {
            return Task.Factory.StartNew(() =>
            {
                var assembly = typeof(GotApiService).GetTypeInfo().Assembly;

                var names = assembly.GetManifestResourceNames();
                var name = names.FirstOrDefault(x => x.EndsWith(fileName, StringComparison.CurrentCultureIgnoreCase));
                using (var stream = assembly.GetManifestResourceStream(name))
                using (var reader = new System.IO.StreamReader(stream))
                    return reader.ReadToEnd();
            });
        }

		public async Task<CharactersLocation[]> GetCharactersLocationAsync()
		{
            var json = await LoadTestJsonAsync("characterslocation.json");
            return JsonConvert.DeserializeObject<CharactersLocation[]>(json);			
		}
#else
        public async Task<CharactersLocation[]> GetCharactersLocationAsync()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var json = await client.GetStringAsync("https://api.got.show/api/characters/locations");
                return JsonConvert.DeserializeObject<CharactersLocation[]>(json);
            }
        }
#endif
	}
}
