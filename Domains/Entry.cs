using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApi.Domains
{
    public class Entry
    {
        public int EntryId { get; set; }

        public int AccountId { get; set; }

        public DateTime EntryDateTime { get; set; }

        public decimal Value { get; set; }

        public string Description { get; set; }

        public EntryType Type { get; set; }

    }

    // aqui estamos declarando como funcionará o Type
    public enum EntryType
    {
        Debit = 0,
        Credit = 1
    }
}
