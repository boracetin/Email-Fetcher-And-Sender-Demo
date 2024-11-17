using EmailFetcherDemo.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailFetcherDemo.Models
{
    public class MailServer: BaseModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int Port { get; set; }
        public EnumMailServerType ServerType { get; set; }
        public required string Server { get; set; }


    }
}
