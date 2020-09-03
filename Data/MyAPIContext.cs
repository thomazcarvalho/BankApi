using Microsoft.EntityFrameworkCore;
using BankApi.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApi.Data
{
    // DbContext está herdando o context do entity framework
    public class MyAPIContext : DbContext
    {
        public MyAPIContext(DbContextOptions<MyAPIContext> option) : base (option)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Entry> Entries { get; set; }
    }
}
