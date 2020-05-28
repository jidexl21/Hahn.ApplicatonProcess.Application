using Hahn.ApplicationProcess.May2020.Core;
using Microsoft.EntityFrameworkCore;

namespace Hahn.ApplicationProcess.May2020.Data
{
    public class ApplicantContext : DbContext
    {
        public ApplicantContext(DbContextOptions options) :base(options)
        {

        }

        public DbSet<Applicant> Applicants { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("InMemoryProvider");
        }                                                                                                                                               
    }
}
