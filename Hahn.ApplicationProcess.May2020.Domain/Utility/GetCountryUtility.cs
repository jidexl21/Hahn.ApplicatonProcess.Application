using Hahn.ApplicatonProcess.May2020.Core.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.May2020.Domain.Utility
{
    public class  GetCountryUtility : IGetCountryUtility
    {
        string status = "Ok";
        private List<Country> countries = new List<Country>();
        private readonly string template = "https://restcountries.eu/rest/v2";
        public GetCountryUtility(ClientSettings clientSettings)
        {
            template = clientSettings.CountryVerificationURI;
            countries = GetAllCountries(); 
        }

        private List<Country> GetAllCountries() {
            Task<List<Country>> tsk = GetAllCountriesAsync();
            tsk.Wait();
            return tsk.Result;
        }

        /// <summary>
        /// Gets Countries and fails silently if API is unavailable (countries will be populated at next call)
        /// </summary>
        /// <returns></returns>
        private async Task<List<Country>> GetAllCountriesAsync() {
            List<Country> retrievedCountries;
            try
            {
                using (HttpClient client = new HttpClient()) {
                    var response = await client.GetAsync(template);
                    var stream = await response.Content.ReadAsStreamAsync();
                    status = "Ok";
                    retrievedCountries = DeserializeFromStream<List<Country>>(stream);
                };
            }
            catch (Exception e)
            {
                status = "Unavailable";
                string msg = e.Message; 
                return countries;
            }
            return retrievedCountries;
        }

        private static T DeserializeFromStream<T>(Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<T>(jsonTextReader);
            }
        }

        public ICountry GetCountry(string suppliedName)
        {
            if (status == "Unavailable") { countries = GetAllCountries(); }
            return countries.Where(x => x.Name.ToLower() == suppliedName.ToLower()).FirstOrDefault(); 
        }

        public async Task<ICountry> GetCountryAsync(string suppliedName)
        {
            Country country;
            using (HttpClient client = new HttpClient()) {
                var response = await client.GetAsync($"{template}/name/{suppliedName}?fullText=true");
                var str = await response.Content.ReadAsStringAsync();
                country = JsonConvert.DeserializeObject<Country>(str);
            }
            return country;
        }
    }


}
