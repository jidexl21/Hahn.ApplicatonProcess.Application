using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;
using Hahn.ApplicationProcess.May2020.Core;
using Hahn.ApplicationProcess.May2020.Domain.Utility;
using Newtonsoft.Json;

namespace Hahn.ApplicationProcess.May2020.Domain.Validations
{
    public class ApplicantValidator : AbstractValidator<Applicant>
    {
        private readonly IGetCountryUtility getCountryUtility;
        public ApplicantValidator(IGetCountryUtility getCountryUtility):base()
        {
            this.getCountryUtility = getCountryUtility;
            RuleFor(applicant => applicant.Name).NotEmpty().MinimumLength(5);
            RuleFor(applicant => applicant.FamilyName).NotEmpty().MinimumLength(5);
            RuleFor(applicant => applicant.Address).NotEmpty().MinimumLength(10);

            RuleFor(applicant => applicant.CountryOfOrigin).NotEmpty().Must(country =>
            {
                var c = getCountryUtility.GetCountry(country);
                if (c == null) { return false; }
                return (c.Name.ToLower() == country.ToLower()); 
            }).WithMessage("Selected country was not found");

            RuleFor(applicant => applicant.EmailAddress).Must(mail =>
            {
                if (string.IsNullOrEmpty(mail)) return false; 
                Regex rg = new Regex(@"^[\w]+@[\w]+[\.][\w]+$");
                return rg.IsMatch(mail); 
            });
            RuleFor(applicant => applicant.Age).InclusiveBetween(20, 60);
            RuleFor(applicant => applicant.Hired).NotNull();
        }

        public async Task<Country> FetchCountry(string country) {
            HttpClient client = new HttpClient();
            string template = "https://restcountries.eu/rest/v2/name/{0}?fullText=false";
            var response = await client.GetAsync(String.Format(template, country));
            if (response.StatusCode != System.Net.HttpStatusCode.OK) { return null;  }
            string stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Country>>(stringResponse);
            return result.FirstOrDefault(); 
        }
    }
}
