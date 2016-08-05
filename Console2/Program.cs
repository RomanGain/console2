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
            //TcpListener listner = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000));
            //listner.Start();
            //while (true)
            //{
            //    try
            //    {
            //        Console.WriteLine("SERVER");
            //        TcpClient client = listner.AcceptTcpClient();

            //        StreamReader sr = new StreamReader(client.GetStream());
            //        Console.WriteLine("Client : " + sr.ReadLine());

            //        //StreamWriter sw = new StreamWriter(client.GetStream());
            //        //sw.AutoFlush = true;
            //        Console.WriteLine("Server : Hey!");
            //        //sw.WriteLine("Привет");

            //        //Console.WriteLine("Client : " + sr.ReadLine());
            //        //Console.WriteLine("Server : End");
            //        //sw.WriteLine("Пока");

            //        client.Close();
            //    }
            //    catch(System.IO.IOException io)
            //    {
            //        Console.WriteLine("IOException:\n" + io.ToString());
            //    }
            //}

            // Устанавливаем для сокета локальную конечную точку
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            // Создаем сокет Tcp/Ip
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                // Начинаем слушать соединения
                while (true)
                {
                    Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);

                    // Программа приостанавливается, ожидая входящее соединение
                    Socket handler = sListener.Accept();
                    string data = null;

                    // Мы дождались клиента, пытающегося с нами соединиться

                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);

                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    // Показываем данные на консоли
                    Console.Write("Полученный текст: " + data + "\n\n");

                    // Отправляем ответ клиенту\
                    string reply = "Спасибо за запрос в " + data.Length.ToString()
                            + " символов";
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);

                    if (data.IndexOf("<TheEnd>") > -1)
                    {
                        Console.WriteLine("Сервер завершил соединение с клиентом.");
                        break;
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }



        }
    }
}
