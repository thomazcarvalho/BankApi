using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApi.Domains
{
    public class Account
    {
        public int AccountId { get; set; }

        public string Bank { get; set; }

        public string Description { get; set; }

        public decimal Balance { get; set; }

        public List<Entry> Entries { get; set; }

    }
}
