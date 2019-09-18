using Microsoft.AspNetCore.Identity;
using System;

namespace Identity.Core.Models
{
    //Classe que representa o usuario identity na aplicação
    //passando o tipo Guid para o usuario
    public class ApplicationUser : IdentityUser<Guid>
    {
        
    }
}
