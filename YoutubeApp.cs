using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CaseStudy
{
    internal class YoutubeApp
    {
        IWebDriver driver;
        string url, search;

        List<Video> results = new List<Video>();

        public void Search()
        {
            Console.WriteLine("What do you want to search for on Youtube?");
            search = Console.ReadLine();
            url = "http://www.youtube.com/results?search_query=" + search.Replace(" ", "+") + "&sp=CAISAhAB";

            driver = new ChromeDriver();
            driver.Url = url;
        }

        public void GetTopFive()
        {
            for (int i = 0; i < 5; i++)
            {
                string xpath = "//*[@id='contents']/ytd-video-renderer[" + (i + 1) + "]";
                IWebElement video = driver.FindElement(By.XPath(xpath));

                string title = video.FindElement(By.XPath(".//*[@id='video-title']")).GetAttribute("title");
                string link = video.FindElement(By.XPath(".//a")).GetAttribute("href");
                string uploader = video.FindElement(By.XPath(".//*[@id='channel-name']//a")).GetAttribute("innerText");
                string views = video.FindElement(By.XPath(".//*[@id='metadata-line']/span[1]")).Text;
                string uploaded = video.FindElement(By.XPath(".//*[@id='metadata-line']/span[2]")).Text;

                results.Add(new Video(title, link, uploader, views, uploaded));
            }
        }

        public void WriteOutput()
        {
            if (!Directory.Exists("outputResults/YoutubeApp"))
            {
                Directory.CreateDirectory("outputResults/YoutubeApp");
            }

            WriteCSV();
            WriteJSON();
        }

        public void WriteCSV()
        {
            string separator = ";";
            StringBuilder stringcsv = new StringBuilder();
            string[] headings = { "Title", "Uploader", "Views", "Upload Time", "Url" };
            stringcsv.AppendLine(string.Join(separator, headings));

            foreach (Video video in results)
            {
                String[] newLine = { video.title, video.uploader, video.views, video.uploaded, video.url };
                stringcsv.AppendLine(string.Join(separator, newLine));
            }

            File.WriteAllText("./outputResults/YoutubeApp/results.csv", stringcsv.ToString());
        }

        public void WriteJSON()
        {
            var obj = new
            {
                keyword = search,
                result = results
            };

            string stringjson = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText("./outputResults/YoutubeApp/results.json", stringjson);
        }

        public void Close()
        {
            driver.Close();
        }
    }

    class Video
    {
        public string title, url, uploader, views, uploaded;

        public Video(string title, string url, string uploader, string views, string uploaded)
        {
            this.title = title;
            this.url = url;
            this.uploader = uploader;
            this.views = views;
            this.uploaded = uploaded;
        }
    }
}