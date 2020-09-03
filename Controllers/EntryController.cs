using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankApi.Data;
using BankApi.Domains;

namespace BankApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        /*
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime EntryDate { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public EntryType Type { get; set; }
         */

        protected readonly MyAPIContext Db;

        // faz com que o banco de dados funcione como uma variável
        public EntryController(MyAPIContext context)
        {
            Db = context;
        }


        // POST --> gravar entradas nas contas
        [HttpPost("{accountId}/entry")]
        public async Task<ActionResult> Post([FromRoute] int accountId, [FromBody] Entry entry)
        {
            try
            {
                var account = await Db.Accounts.Where(account => account.AccountId == accountId).FirstOrDefaultAsync();

                if (account == null)
                {
                    return BadRequest("A conta informada não existe: Id " + entry.AccountId);
                }

                if (entry.Value <= 0)
                {
                    return BadRequest("Valor informado deve ser maior do que zero.");
                }

                if (String.IsNullOrEmpty(entry.Description))
                {
                    return BadRequest("A descrição deve ser informada.");
                }

                if ((int)entry.Type != 0 && (int)entry.Type != 1)
                {
                    return BadRequest("O tipo deve ser igual a 0 (Débito) ou 1 (Crédito).");
                }

                entry.EntryDateTime = DateTime.Now;
                entry.AccountId = accountId;

                if (entry.Type == EntryType.Credit)
                {
                    account.Balance += entry.Value;
                }
                else if (entry.Type == EntryType.Debit)
                {
                    account.Balance -= entry.Value;
                }

                await Db.Entries.AddAsync(entry);
                Db.Accounts.Update(account);
                await Db.SaveChangesAsync();

                return Ok(entry);
            }
            catch
            {
                return Problem("Ocorreu um erro no servidor. Contate  suporte.");
            }
        }

    }
}
