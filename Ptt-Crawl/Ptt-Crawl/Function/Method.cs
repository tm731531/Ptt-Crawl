using HtmlAgilityPack;
using Ptt_Crawl.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptt_Crawl.Function
{
   public class Method
    {
        public static ResponseModel WriteToModel(ResponseModel remodel, HtmlDocument docInner, string link)
        {
            int message_conut_all = 0;
            int message_conut_boo = 0;
            int message_conut_neutral = 0;
            int message_conut_push = 0;
            remodel.content = GetDataBySingleNodeXPath(docInner, "//*[@id=\"main-content\"]/text()[1]").InnerText;
            remodel.article_id = link.Split('/')[3].ToUpper().Replace(".HTML", "");
            remodel.article_title = GetDataBySingleNodeXPath(docInner, "//*[@id=\"main-content\"]/div[3]/span[2]").InnerText;
            remodel.author = GetDataBySingleNodeXPath(docInner, "//*[@id=\"main-content\"]/div[1]/span[2]").InnerText;
            remodel.board = GetDataBySingleNodeXPath(docInner, "//*[@id=\"main-content\"]/div[2]/span[2]").InnerText;
            remodel.date = GetDataBySingleNodeXPath(docInner, "//*[@id=\"main-content\"]/div[4]/span[2]").InnerText;
            remodel.ip = GetDataBySingleNodeXPath(docInner, "//*[@id=\"main-content\"]/text()[2]").InnerText;
            remodel.messages = new List<Message>();
            int start = 5;
            foreach (var nodesInners in GetDataByMutiNodeXPath(docInner, "//*[@id=\"main-content\"]/div[@class='push']"))
            {
                try
                {
                    var tag = GetDataBySingleNodeXPath(docInner, $"//*[@id=\"main-content\"]/div[{start}]/span[1]").InnerText;
                    remodel.messages.Add(
                        new Message()
                        {
                            push_content = GetDataBySingleNodeXPath(docInner, $"//*[@id=\"main-content\"]/div[{start}]/span[3]").InnerText,
                            push_ipdatetime = GetDataBySingleNodeXPath(docInner, $"//*[@id=\"main-content\"]/div[{start}]/span[4]").InnerText,
                            push_tag = tag,
                            push_userid = GetDataBySingleNodeXPath(docInner, $"//*[@id=\"main-content\"]/div[{start}]/span[2]").InnerText
                        });
                    if (tag == "→") { message_conut_neutral++; }
                    else if (tag == "推") { message_conut_push++; }
                    else if (tag == "噓") { message_conut_boo++; }
                    start++;
                    message_conut_all++;
                }
                catch (Exception ex) {
                    var q =  ex.ToString(); }
            }
            remodel.message_conut = new Message_Conut();
            remodel.message_conut.all = message_conut_all;
            remodel.message_conut.boo = message_conut_boo;
            remodel.message_conut.count = message_conut_push - message_conut_boo;
            remodel.message_conut.neutral = message_conut_neutral;
            remodel.message_conut.push = message_conut_push;
            return remodel;

        }

        public static HtmlNode GetDataBySingleNodeXPath(HtmlDocument docInner, string xpath)
        {
            return docInner.DocumentNode.SelectSingleNode(xpath);
        }

        public static HtmlNodeCollection GetDataByMutiNodeXPath(HtmlDocument docInner, string xpath)
        {
            return docInner.DocumentNode.SelectNodes(xpath);
        }

        public static void WriteData(string filePath, string content)
        {
            if (!File.Exists(filePath))
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(content);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(content);
                }
            }
        }
    }
}
