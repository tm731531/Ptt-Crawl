using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptt_Crawl.Models
{
    public class ResponseModel
    {
        public string article_id { get; set; }
        public string article_title { get; set; }
        public string author { get; set; }
        public string board { get; set; }
        public string content { get; set; }
        public string date { get; set; }
        public string ip { get; set; }
        public Message_Conut message_conut { get; set; }
        public List<Message> messages { get; set; }
    }

    public class Message_Conut
    {
        public int all { get; set; }
        public int boo { get; set; }
        public int count { get; set; }
        public int neutral { get; set; }
        public int push { get; set; }
    }

    public class Message
    {
        public string push_content { get; set; }
        public string push_ipdatetime { get; set; }
        public string push_tag { get; set; }
        public string push_userid { get; set; }
    }
}
