using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Console2
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener listner = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000));
            listner.Start();
            while (true)
            {
                try
                {
                    Console.WriteLine("SERVER");
                    TcpClient client = listner.AcceptTcpClient();

                    StreamReader sr = new StreamReader(client.GetStream());
                    Console.WriteLine("Client : " + sr.ReadLine());

                    //StreamWriter sw = new StreamWriter(client.GetStream());
                    //sw.AutoFlush = true;
                    Console.WriteLine("Server : Hey!");
                    //sw.WriteLine("Привет");

                    //Console.WriteLine("Client : " + sr.ReadLine());
                    //Console.WriteLine("Server : End");
                    //sw.WriteLine("Пока");

                    client.Close();
                }
                catch(System.IO.IOException io)
                {
                    Console.WriteLine("IOException:\n" + io.ToString());
                }
            }
        }
    }
}
