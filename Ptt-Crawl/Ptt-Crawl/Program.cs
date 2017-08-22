using HtmlAgilityPack;
using Newtonsoft.Json;
using Ptt_Crawl.Function;
using Ptt_Crawl.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ptt_Crawl
{
    class Program
    {
        static string hostUrl = "https://www.ptt.cc";

        static void Main(string[] args)
        {
            try
            {
                string subject = args[0];
                int index = Int32.Parse(args[1]);
                int indexEnd = index;
                bool isSaveToSameFile = false;

                try { indexEnd = Int32.Parse(args[2]); } catch (Exception ex) { }
                try { isSaveToSameFile = bool.Parse(args[3]); } catch (Exception ex) { }
                DomoreJob(subject, index, indexEnd, isSaveToSameFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.ReadLine();

            }
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
            //sw.Reset();//碼表歸零
            //sw.Start();//碼表開始計時
            //           /**************/
            //           /**************/
            //           /***目標程式***/
            //           /**************/
            //           /**************/

            //DomoreJob("Agriculture", 1, 103, true);
            //sw.Stop();//碼錶停止
            //          //印出所花費的總豪秒數
            //string result1 = sw.Elapsed.TotalMilliseconds.ToString();
            //Console.WriteLine(result1);
            //Console.ReadLine();
        }

        private static void DomoreJob(string subject, int index, int indexEnd, bool isSaveToSameFile = false)
        {
            string pathString = Path.Combine(Environment.CurrentDirectory, "PttData");
            string subPathString = Path.Combine(pathString, subject);

            if (!Directory.Exists(pathString)) Directory.CreateDirectory(pathString);
            if (!Directory.Exists(subPathString)) Directory.CreateDirectory(subPathString);

            List<Task> Tasks = new List<Task>();
            //var t = Task.Run(() => ShowThreadInfo("Task"));
            //t.Wait();
            for (int i = index; i <= indexEnd; ++i)
            {
                //int threadId;
                //// Create an instance of the test class.
                //AsyncDemo ad = new AsyncDemo();

                //// Create the delegate.
                //AsyncMethodAddDataCaller caller = new AsyncMethodAddDataCaller(ad.AddData);

                //caller.BeginInvoke($"{hostUrl}/bbs/{subject}/index{i}.html"
                //     , subPathString, isSaveToSameFile, null, null);


                // string returnValue = caller.EndInvoke( result);
                // Console.WriteLine("The call executed on thread {0}, with return value \"{0}\".", returnValue);
                //var t = Task.Run(() => AddData($"{hostUrl}/bbs/{subject}/index{i}.html", subPathString, isSaveToSameFile));
                //{


                HtmlWeb webClient = new HtmlWeb();
                try
                {
                    HtmlDocument doc = webClient.Load($"{hostUrl}/bbs/{subject}/index{i}.html");
                    HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//*[@id='main-container']/div[2]/div/div[3]/a");
                    foreach (HtmlNode node in nodes)
                    {
                        string link = node.Attributes.FirstOrDefault().Value;
                        HtmlDocument docInner = webClient.Load($"{hostUrl}/{link}");
                        HtmlNodeCollection nodesInner = docInner.DocumentNode.SelectNodes("//*[@id=\"main-content\"]/text()[1]");
                        ResponseModel remodel = new ResponseModel();
                        remodel = Method.WriteToModel(remodel, docInner, link);
                        Console.WriteLine(remodel.article_id);
                        if (isSaveToSameFile) Method.WriteData(Path.Combine(subPathString + ".json"), JsonConvert.SerializeObject(remodel));
                        else Method.WriteData(Path.Combine(subPathString, remodel.article_id + ".json"), JsonConvert.SerializeObject(remodel));
                    }
                }
                catch (Exception ex) { }
                //}
            }

           // Task.WaitAll(Tasks.ToArray());
        }

        private  static void AddData(string Url,string subPathString, bool isSaveToSameFile)
        {
            try
            {
                HtmlWeb webClient = new HtmlWeb();
                HtmlDocument doc = webClient.Load(Url);
                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//*[@id='main-container']/div[2]/div/div[3]/a");
                foreach (HtmlNode node in nodes)
                {
                    string link = node.Attributes.FirstOrDefault().Value;
                    HtmlDocument docInner = webClient.Load($"{hostUrl}/{link}");
                    HtmlNodeCollection nodesInner = docInner.DocumentNode.SelectNodes("//*[@id=\"main-content\"]/text()[1]");
                    ResponseModel remodel = new ResponseModel();
                    remodel = Method.WriteToModel(remodel, docInner, link);
                    Console.WriteLine(remodel.article_id);
                    if (isSaveToSameFile) Method.WriteData(Path.Combine(subPathString + ".json"), JsonConvert.SerializeObject(remodel));
                    else Method.WriteData(Path.Combine(subPathString, remodel.article_id + ".json"), JsonConvert.SerializeObject(remodel));
                }
            }
            catch (Exception ex) { }
        }

    }
}
