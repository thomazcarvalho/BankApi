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
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // criamos a variável Db que representa o contexto da API
        protected readonly MyAPIContext Db;

        // faremos uma injeção de dependencia no construtor do controlador: contexto do tipo MyAPI
        // atrelamos esse contexto à variável Db
        // faz com que o banco de dados funcione como uma variável
        public AccountController (MyAPIContext context)
        {
            Db = context;
        }

        // GET --> pegar os dados das contas
        [HttpGet]
        public async Task<ActionResult<List<Account>>> Get() // uma task realiza tarefas de maneira assíncrona
        {
            // aqui é feito basicamente um select * no banco na tabela accounts
            var accounts = await Db.Accounts.ToListAsync();
            
            // accounts.ForEach(item => item.Entries = new List<Entry>(Db.Entries.Where(entry => entry.AccountId == item.AccountId).ToList()));
            foreach (var account in accounts)
            {
                var entries = await Db.Entries.Where(acc => acc.AccountId == account.AccountId).ToListAsync();
                account.Entries = entries;
            }

            return Ok(accounts);
        }

        // POST --> gravar os dados nas contas
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Account account)
        {
            try
            {
                // neste ponto estamos verificando se o banco já exite (limitando a apenas um banco), pois o entity framework não deixa que o usuário defina o Id
                // o Id é gerado automaticamente, sendo desconsiderado se passado via JSON
                var accountExists = await Db.Accounts.Where(acc => acc.Bank == account.Bank).FirstOrDefaultAsync();
                
                if (accountExists != null)
                {
                    return BadRequest("O banco já existe.");
                }

                await Db.Accounts.AddAsync(account);
                await Db.SaveChangesAsync();

                return Ok(account);
            }
            catch (Exception error)
            {
                return Problem("Ocorreu um erro no servidor: " + error);
            }
        }

        // PUT --> faz update em dados das contas
        [HttpPut("{accountId}")]
        public async Task<ActionResult> Put([FromRoute] int accountId, [FromBody] Account account)
        {
            try
            {
                // (acc => acc == account.Id) é uma sintaxe lambda onde:
                // foram filtradas as contas do banco de dados (acc) onde o Id (acc.Id) é igual ao Id informado no body (account.Id)
                // FirstorDefault pega apenas o primeiro valor na lista. Se não encontra aloca um NULL
                // ToList pega todos os valores que respondem a essa função lambda (fetch)
                var modifiedAccount = await Db.Accounts.Where(acc => acc.AccountId == accountId).FirstOrDefaultAsync();

                if (modifiedAccount != null)
                {
                    if (modifiedAccount.Balance != account.Balance)
                    {
                        return BadRequest("O saldo da conta não pode ser modificado.");
                    }

                    modifiedAccount.Description = account.Description;
                    modifiedAccount.Bank = account.Bank;

                    account.AccountId = modifiedAccount.AccountId;

                    Db.Accounts.Update(modifiedAccount);
                    await Db.SaveChangesAsync();

                    return Ok(account);
                }

                return BadRequest("A conta informada não foi encontrada: Id " + account.AccountId);
            }
            catch
            {
                return StatusCode(500, "Ocorreu um erro no servidor. Contate o suporte.");
            }
        }

        // DELETE --> deleta dados das contas
        [HttpDelete("{accountId}")]
        public async Task<ActionResult> Delete([FromRoute] int accountId)
        {
            var deletedAccount = await Db.Accounts.Where(acc => acc.AccountId == accountId).FirstOrDefaultAsync();

            if (deletedAccount != null)
            {
                Db.Accounts.Remove(deletedAccount);
                await Db.SaveChangesAsync();
                return Ok();
            }

            return NotFound("A conta informada não foi encontrada: Id " + accountId);
        }

    }
}


/*
    Esse arquivo serve para controlar as rotas da nossa API
 */