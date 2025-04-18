﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailFetcherDemo.Models
{
    public class Mails : BaseModel
    {
        public required string MessageId { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public required string To { get; set; }
        public string? Cc { get; set; }
        public string? Bcc { get; set; }
        public string? InReplyToMessageId { get; set; }
        public string? Referances { get; set; }


    }
}
