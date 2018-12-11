using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EboChat
{
    class Connection
    {
        public event EventHandler<string> OnStatusChange;
        public event EventHandler<ChatMessage> OnChatMessageReceived;

        TcpListener incoming;
        TcpClient outgoing;

        Thread incomingThread;

        public Connection()
        { 
            UpdateStatus("Started");
        }

        private void UpdateStatus(string status)
        {
            if (OnStatusChange != null)
            {
                OnStatusChange(this, status);
            }
        }

        public string GetOwnIpAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                return "Keine Netzwerk Verbindung";

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return $"Meine IP Adresse: {ip.ToString()}";
                }
            }
            return "IP Adresse unbekannt";
        }

        public void Connect()
        {
            try
            {
                incomingThread = new Thread(Server);
                incomingThread.Start();
            }
            catch
            {
                incomingThread?.Abort();
            }
        }

        private void Server()
        {
            while(true)
            {
                try
                {
                    UpdateStatus("Connecting...");
                    incoming = new TcpListener(IPAddress.Parse("127.0.0.1"), 13000);

                    // Start listening for client requests.
                    incoming.Start();

                    // Buffer for reading data
                    Byte[] bytes = new Byte[256];
                    String data = null;

                    TcpClient client = null;

                    // Enter the listening loop.
                    while (true)
                    {
                        try
                        {
                            // Perform a blocking call to accept requests.
                            // You could also user server.AcceptSocket() here.
                            client = incoming.AcceptTcpClient();
                            UpdateStatus("Connected");

                            data = null;

                            // Get a stream object for reading and writing
                            NetworkStream stream = client.GetStream();

                            int i;

                            // Loop to receive all the data sent by the client.
                            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                            {
                                // Translate data bytes to a ASCII string.
                                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                                OnChatMessageReceived?.Invoke(this, new ChatMessage(DateTime.Now, $"<< {data}"));
                            }
                        }
                        catch (Exception e)
                        {
                            OnChatMessageReceived?.Invoke(this, new ChatMessage(DateTime.Now, e.Message));
                        }
                        finally
                        {
                            // Shutdown and end connection
                            client?.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    OnChatMessageReceived?.Invoke(this, new ChatMessage(DateTime.Now, e.Message));
                }
                finally
                {
                    // Stop listening for new clients.
                    incoming.Stop();
                }
            }
        }
    }
}