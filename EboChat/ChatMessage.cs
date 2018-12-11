using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EboChat
{
    public class ChatMessage
    {
        public DateTime Date { get; }
        public string Text { get; }

        public ChatMessage(DateTime date, string text)
        {
            this.Date = date;
            this.Text = text;
        }
    }
}
