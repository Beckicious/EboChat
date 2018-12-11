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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EboChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Connection c;

        private const int maxNumberOfChatMessages = 10000;
        private const int numberOfChatMessagesToRemove = 1000;

        public MainWindow()
        {
            c = new Connection();
            c.OnStatusChange += UpdateStatus;
            c.OnChatMessageReceived += OnChatMessageReceived;

            InitializeComponent();

            txtOwnIpAddress.Content = c.GetOwnIpAddress();
            txtOtherIpAddress.Text = Properties.Settings.Default.lastIpAddress;
        }

        private void butConnect_Click(object sender, RoutedEventArgs e)
        {
            butConnect.IsEnabled = false;
            txtOtherIpAddress.IsEnabled = false;
            butDisconnect.IsEnabled = false;
            c.Connect();
        }

        private void butDisconnect_Click(object sender, RoutedEventArgs e)
        {
            butConnect.IsEnabled = false;
            txtOtherIpAddress.IsEnabled = false;
            butDisconnect.IsEnabled = false;
        }

        private void OnChatMessageReceived(object sender, ChatMessage e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (spChatLines.Children.Count > maxNumberOfChatMessages - 1)
                {
                    spChatLines.Children.RemoveRange(0, numberOfChatMessagesToRemove);
                }

                if (svChatLines.VerticalOffset == svChatLines.ScrollableHeight)
                {
                    spChatLines.Children.Add(new ChatLine(e));
                    svChatLines.ScrollToBottom();
                }
                else
                {
                    spChatLines.Children.Add(new ChatLine(e));
                }
            });
        }

        private void UpdateStatus(object sender, string e)
        {
            this.Dispatcher.Invoke(() =>
            {
                txtStatus.Content = e;
            });
        }

        private void TxtStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            const int height = 20;

            if (txtCreator.Height > 0)
            {
                //shrink
                Storyboard sb = new Storyboard();
                sb.FillBehavior = FillBehavior.Stop;
                DoubleAnimation anim = new DoubleAnimation();
                anim.From = txtCreator.ActualHeight;
                anim.To = 0;
                anim.Duration = TimeSpan.FromSeconds(0.5);
                anim.FillBehavior = FillBehavior.HoldEnd;
                Storyboard.SetTarget(anim, this.txtCreator);
                Storyboard.SetTargetProperty(anim, new PropertyPath("Height"));
                sb.Children.Add(anim);
                sb.Completed += (ss, ee) =>
                {
                    this.txtCreator.Height = 0;
                };
                sb.Begin();
            }
            else
            {
                //grow
                Storyboard sb = new Storyboard();
                sb.FillBehavior = FillBehavior.Stop;
                DoubleAnimation anim = new DoubleAnimation();
                anim.From = txtCreator.ActualHeight;
                anim.To = height;
                anim.Duration = TimeSpan.FromSeconds(0.5);
                anim.FillBehavior = FillBehavior.HoldEnd;
                Storyboard.SetTarget(anim, this.txtCreator);
                Storyboard.SetTargetProperty(anim, new PropertyPath("Height"));
                sb.Children.Add(anim);
                sb.Completed += (ss, ee) =>
                {
                    this.txtCreator.Height = height;
                };
                sb.Begin();
            }
        }   
    }
}
