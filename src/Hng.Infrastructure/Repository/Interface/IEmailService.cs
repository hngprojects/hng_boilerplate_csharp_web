using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Infrastructure.Repository.Interface
{
    public interface IEmailService
    {
       Task SendEmailAsync(string to, string subject, string body);
    }
}
