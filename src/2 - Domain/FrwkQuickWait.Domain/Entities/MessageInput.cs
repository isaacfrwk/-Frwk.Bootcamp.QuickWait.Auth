using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrwkQuickWait.Domain.Entities
{
    public class MessageInput
    {
        public string Queue { get; set; }
        public string Method { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public MessageInput(string queue, string method, string content)
        {
            Queue = queue;
            Method = method;
            Content = content;
        }
    }
}
