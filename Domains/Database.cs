using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApi.Domains
{
    public class Database
    {
        // uma classe estática não precisa ser instanciada em nenhum momento
        // é a unica classe no sistema com um valor fixo e não poderá existir outras
        // aqui estamos criando uma lista utilizando a classe Account como base
        public static List<Account> Accounts = new List<Account>();
    }
}
