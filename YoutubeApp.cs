using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            string[] headings = { "Keyword", "Title", "Uploader", "Views", "Upload Time", "Url" };
            stringcsv.AppendLine(string.Join(separator, headings));

            foreach (Video video in results)
            {
                foreach (var item in video.viewsList)
                {
                    String[] newLine = { search, video.title, video.uploader, item.Key + ": " + item.Value.ToString(), video.uploaded, video.url };
                    stringcsv.AppendLine(string.Join(separator, newLine));
                }
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
        public string title, url, uploader, uploaded;
        public Dictionary<string, int> viewsList = new Dictionary<string, int>();
        int viewsInt;

        public Video(string title, string url, string uploader, string views, string uploaded)
        {
            this.title = title;
            this.url = url;
            this.uploader = uploader;

            AddViews(DateTime.Now, views);

            this.uploaded = uploaded;
        }

        int viewsStringToInt(string str)
        {
            string viewsString = str.Split(' ')[0];
            switch (viewsString.Substring(viewsString.Length - 1))
            {
                case "o":
                    // e.g. No views
                    viewsInt = 0;
                    return viewsInt;
                case "M":
                    // e.g. 1.4M views
                    viewsString = viewsString.Remove(viewsString.Length - 1);
                    viewsInt = (int)(Convert.ToDecimal(viewsString, CultureInfo.InvariantCulture) * 1000000);
                    return viewsInt;
                case "K":
                    // e.g. 1.4K views
                    viewsString = viewsString.Remove(viewsString.Length - 1);
                    viewsInt = (int)(Convert.ToDecimal(viewsString, CultureInfo.InvariantCulture) * 1000);
                    return viewsInt;
                default:
                    // All between 0 and 1000
                    viewsInt = (int)(Convert.ToDecimal(viewsString, CultureInfo.InvariantCulture));
                    return viewsInt;
            }
        }

        public void AddViews(DateTime date, string views)
        {
            this.viewsInt = viewsStringToInt(views);
            this.viewsList.Add(date.ToString(), this.viewsInt);
        }
    }
}