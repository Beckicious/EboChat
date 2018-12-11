using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EboChat
{
    /// <summary>
    /// Interaction logic for ChatLine.xaml
    /// </summary>
    public partial class ChatLine : UserControl
    {
        ChatMessage cm;

        public ChatLine(ChatMessage msg)
        {
            InitializeComponent();
            cm = msg;
            txtDate.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
            txtMessage.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
            txtDate.Text = String.Format("{0:HH:mm:ss}", DateTime.Now);
            txtMessage.Text = cm.Text;
        }

        private void ctrChatLine_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var size = this.ActualWidth;
            if (size < 10)
            {
                size = 10;
            }
            txtMessage.Width = size;
        }
    }
}
