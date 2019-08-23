using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeviceSubscription.Web
{
    public interface IMailSender
    {
        string SendEmail(string Email, string Message);
    }
}
