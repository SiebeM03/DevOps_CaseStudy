using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseStudy
{
    internal class JobApp
    {
        IWebDriver driver;
        string url, search;
        IWebElement searchResultElement;
            
        public void Search()
        {
            Console.WriteLine("What job keyword do you want to search for?");
            search = Console.ReadLine();
            url = "https://www.ictjob.be/en/search-it-jobs?keywords=" + search.Replace(" ", "+");

            driver = new ChromeDriver();
            driver.Url = url;
        }

        public void Sort()
        {
            searchResultElement = WaitForElement("//*[@id=\"search-result\"]");
            driver.FindElement(By.XPath("//*[@id=\"search-result\"]//a[@id=\"sort-by-date\"]")).Click();
            
            // Prevent Selenium from instantly checking the opacity (it is our trigger to check if the page loaded, if opacity = 1 it means the page is loaded, while it's loading the opacity = 0.5)
            System.Threading.Thread.Sleep(1000);
            while (driver.FindElement(By.XPath("//*[@id=\"search-result\"]//div[@id=\"search-result-body\"]")).GetCssValue("opacity").Equals("0.5"))
            {
                System.Threading.Thread.Sleep(1);
            }
            // Sorted and done loading
        }

        public void GetTopFive()
        {
            List<IWebElement> topFive = new List<IWebElement>();
            for (int i = 0; i < 6; i++)
            {
                string xpath = "//*[@id=\"search-result\"]//ul/li[" + (i + 1) + "]";
                IWebElement job = WaitForElement(xpath);
                if (job.GetAttribute("class").Equals("search-item  clearfix"))
                {
                    topFive.Add(job);
                }
            }

            // TODO MAKE INTO 1 FOR LOOP DEPENDING ON CSV AND JSON USAGE
            foreach (IWebElement job in topFive)
            {
                string title = job.FindElement(By.ClassName("job-title")).Text;
                string link = job.FindElement(By.ClassName("job-title")).GetAttribute("href");
                string company = job.FindElement(By.ClassName("job-company")).Text;
                string location = job.FindElement(By.ClassName("job-location")).Text;
                string keywords = job.FindElement(By.ClassName("job-keywords")).Text;

                Console.WriteLine(location);
                Console.WriteLine(keywords);
            }
        }

        private IWebElement WaitForElement(string regex)
        {
            for (int attempts = 0; attempts < 30; attempts++)
            {
                try
                {
                    return driver.FindElement(By.XPath(regex));
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }
                System.Threading.Thread.Sleep(200);
            }
            return null;
        }

        public void WriteCSV()
        {

        }

        public void WriteJSON()
        {

        }

        public void Close()
        {
            driver.Close();
        }
    }
}
