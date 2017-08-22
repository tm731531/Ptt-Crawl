using HtmlAgilityPack;
using Newtonsoft.Json;
using Ptt_Crawl.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ptt_Crawl.Function
{
    //static string hostUrl = "https://www.ptt.cc";

    public class AsyncDemo
    {
        // The method to be executed asynchronously.
        public string TestMethod(int callDuration, out int threadId)
        {
            Console.WriteLine("Test method begins.");
            Thread.Sleep(callDuration);
            threadId = Thread.CurrentThread.ManagedThreadId;
            return String.Format("My call time was {0}.", callDuration.ToString());
        }
        public string AddData(string Url, string subPathString, bool isSaveToSameFile)
        {
            string hostUrl = "https://www.ptt.cc";
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
            return String.Format("My call time was {0}.", Thread.CurrentThread.ManagedThreadId.ToString());
        }

    }
    // The delegate must have the same signature as the method
    // it will call asynchronously.
    public delegate string AsyncMethodCaller(int callDuration, out int threadId);
    public delegate string AsyncMethodAddDataCaller(string Url, string subPathString, bool isSaveToSameFile);

}
