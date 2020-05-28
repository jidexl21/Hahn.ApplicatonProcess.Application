using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace Hahn.ApplicationProcess.May2020.Domain.Utility
{
    [Serializable]
    public class Country : ICountry
    {
        public string Name { get; set; }
    }
}
