using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CaseStudy
{
    internal class JobApp
    {
        IWebDriver driver;
        string url, search;
        List<Job> results = new List<Job>();

        public void Search()
        {
            Console.WriteLine("What job keyword do you want to search for?");
            search = Console.ReadLine();
            url = "https://www.ictjob.lu/en/search-it-jobs?keywords=" + search.Replace(" ", "+");

            driver = new ChromeDriver();
            driver.Url = url;
        }

        public void Sort()
        {
            System.Threading.Thread.Sleep(1000);
            driver.FindElement(By.XPath("//*[@id='search-result']//a[@id='sort-by-date']")).Click();

            // Prevent Selenium from instantly checking the opacity after clicking the button (it is our trigger
            // to check if the page loaded if opacity = 1 it means the page is loaded, while it's loading the opacity = 0.5)
            System.Threading.Thread.Sleep(1000);
            while (driver.FindElement(By.XPath("//*[@id='search-result']//div[@id='search-result-body']")).GetCssValue("opacity").Equals("0.5"))
            {
                System.Threading.Thread.Sleep(1);
            }
            // Sorted and done loading
        }

        public void GetTopFive()
        {
            for (int i = 0; i < 6; i++)
            {
                string xpath = "//*[@id='search-result']//ul/li[" + (i + 1) + "]";
                IWebElement job = driver.FindElement(By.XPath(xpath));

                if (job.GetAttribute("class").Equals("search-item  clearfix"))
                {
                    string title = job.FindElement(By.ClassName("job-title")).Text;
                    string link = job.FindElement(By.ClassName("job-title")).GetAttribute("href");
                    string company = job.FindElement(By.ClassName("job-company")).Text;
                    string location = job.FindElement(By.ClassName("job-location")).Text;
                    string keywords = job.FindElement(By.ClassName("job-keywords")).Text;

                    results.Add(new Job(title, link, company, location, keywords));
                }
            }
        }

        public void WriteOutput()
        {
            if (!Directory.Exists("outputResults/JobApp"))
            {
                Directory.CreateDirectory("outputResults/JobApp");
            }

            WriteCSV();
            WriteJSON();
        }

        public void WriteCSV()
        {
            string separator = ";";
            StringBuilder stringcsv = new StringBuilder();
            string[] headings = { "Title", "Company", "Location", "Keywords", "Url" };
            stringcsv.AppendLine(string.Join(separator, headings));

            foreach (Job job in results)
            {
                String[] newLine = { job.title, job.company, job.location, job.keywords, job.url };
                stringcsv.AppendLine(string.Join(separator, newLine));
            }

            File.WriteAllText("./outputResults/JobApp/results.csv", stringcsv.ToString());
        }

        public void WriteJSON()
        {
            var obj = new
            {
                keyword = search,
                result = results
            };

            string stringjson = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText("./outputResults/JobApp/results.json", stringjson);
        }

        public void Close()
        {
            driver.Close();
        }
    }

    class Job
    {
        public string title, url, company, location, keywords;

        public Job(string title, string url, string company, string location, string keywords)
        {
            this.title = title;
            this.url = url;
            this.company = company;
            this.location = location;
            this.keywords = keywords;
        }
    }
}
