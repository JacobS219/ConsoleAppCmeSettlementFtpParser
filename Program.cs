using System;
using System.Text;
using System.IO;
using System.Net;

namespace TextParser1
{
    class Program
    {
        static void Main(string[] args)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ftp.cmegroup.com/settle/stlcomex");
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string contents = reader.ReadToEnd();

                            string[] splitContent = contents.Split('\n');

                            begin:
                            string data = null;
                            int i = 0;
                            int location = 0;

                            Console.WriteLine("Enter Symbol");
                            string input = Console.ReadLine().ToUpper();
                            if (input[0] == ('/'))
                                input = input.Substring(1);
                            switch (input)
                            {
                                case "GC":
                                    input = "GC Gold";
                                    break;
                                case "SI":
                                    input = "SI Silver";
                                    break;
                                case "HG":
                                    input = "HG Copper";
                                    break;
                            }
                            Console.WriteLine("Thank you...");

                            foreach (string item in splitContent)
                            {
                                i++;

                                //Console.WriteLine(i, Console.BufferHeight = Int16.MaxValue - 1);
                                if (item.StartsWith("        F"))
                                {
                                    data = item;
                                    Console.WriteLine(data);
                                }

                                if (item.StartsWith(input))
                                {
                                    data = item;
                                    location = i;
                                    Console.WriteLine(data);
                                }

                                if (item.StartsWith("MTH/"))
                                {
                                    data = item;
                                    Console.WriteLine(data);
                                }

                                if (item.StartsWith("STRIKE"))
                                {
                                    data = item;
                                    Console.WriteLine(data);
                                }
                            }

                            for (int n = 0; n <= 10; n++)
                            {
                                Console.WriteLine(splitContent[location + n]);
                            }

                            Console.WriteLine("DONE");
                            goto begin;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                   WebResponse errorResponse = ex.Response;
                        using (Stream responseStream = errorResponse.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                            String errorText = reader.ReadToEnd();
                        }
                   throw;
            }
        }
    }
}
