using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hahn.ApplicationProcess.Application;
using Hahn.ApplicationProcess.May2020.Core;
using Hahn.ApplicationProcess.May2020.Domain;
using Hahn.ApplicationProcess.May2020.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hahn.ApplicationProcess.May2020.Web.Controllers
{
    /// <summary>
    /// Applicant  endpoints
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService applicantService; 
        /// <summary>
        /// 
        /// </summary>
        public ApplicantController(IApplicantService applicantService)
        {
            this.applicantService = applicantService;
        }
        /// <summary>
        ///  Returns list of Applicants
        /// </summary>
        /// <returns></returns>
        // GET: api/Applicant
        [HttpGet]
        public IEnumerable<Applicant> Get()
        {
            return applicantService.GetAll();
        }

        /// <summary>
        /// Gets an Applicant by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Applicant/5
        [HttpGet("{id}")]
        public Applicant Get(int id)
        {
            return applicantService.Find(id);
        }

        /// <summary>
        /// Create a new Applicant
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     
        ///     {
        ///     "name": "Ferdinand",
        ///     "familyName": "Magellan",
        ///     "address": "Villa Real, Sabrosa",
        ///     "countryOfOrigin": "Portugal",
        ///     "emailAddress": "not@yet.made",
        ///     "age": 34,
        ///     "hired": false
        ///     }
        /// </remarks>
        /// <param name="value"></param>
        // POST: api/Applicant
        [HttpPost]
        public ActionResult<Applicant> Post([FromBody] Applicant value)
        {
            return Created("",applicantService.Save(value));
        }

        /// <summary>
        /// Update an applicant
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     
        ///     {
        ///     "id": 1,
        ///     "name": "Ferdinand",
        ///     "familyName": "Magellan",
        ///     "address": "Villa Real, Sabrosa",
        ///     "countryOfOrigin": "Portugal",
        ///     "emailAddress": "newemail@sabrosa.com",
        ///     "age": 34,
        ///     "hired": false
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="value"></param>
        // PUT: api/Applicant/5
        [HttpPut("{id}")]
        public ActionResult<Applicant> Put(int id, [FromBody] Applicant value)
        {
            var result = applicantService.Update(value);
            return Ok(result);
        }
        /// <summary>
        /// Delete an applicant
        /// </summary>
        /// <param name="id"></param>
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var item = applicantService.Find(id);
            bool result = false;
            if (item != null) { result = applicantService.Delete(item); };
            return Ok(result); 
        }
    }
}