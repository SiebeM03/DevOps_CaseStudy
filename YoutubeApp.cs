using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace CaseStudy
{
    internal class YoutubeApp
    {
        IWebDriver driver;
        string url, search;

        public void Search()
        {
            Console.WriteLine("What do you want to search for on Youtube?");
            search = Console.ReadLine();
            url = "http://www.youtube.com/results?search_query=" + search.Replace(" ", "+") + "&sp=CAISAhAB";

            driver = new ChromeDriver();
            driver.Url = url;
        }

        public void AcceptCookies()
        {
            WaitForElement(".//*[@aria-label='Accept the use of cookies and other data for the purposes described']").Click();
        }

        public void SetFilters()
        {
            // FILTER BUTTON
            WaitForElement("//*[@id='filter-menu']//ytd-toggle-button-renderer//button").Click();
            // VIDEO BUTTON
            WaitForElement("//iron-collapse/div/ytd-search-filter-group-renderer[2]/ytd-search-filter-renderer[1]/a").Click();

            // FILTER BUTTON
            WaitForElement("//*[@id='filter-menu']//ytd-toggle-button-renderer//button").Click();
            // UPLOAD DATE BUTTON
            WaitForElement("//iron-collapse/div/ytd-search-filter-group-renderer[5]/ytd-search-filter-renderer[2]/a").Click();
        }

        public void GetTopFive()
        {
            List<IWebElement> topFive = new List<IWebElement>();
            for (int i = 0; i < 5; i++)
            {
                string xpath = "//*[@id=\"contents\"]/ytd-video-renderer[" + (i + 1) + "]";
                WaitForElement(xpath);
                topFive.Add(driver.FindElement(By.XPath(xpath)));
            }

            // TODO MAKE INTO 1 FOR LOOP DEPENDING ON CSV AND JSON USAGE
            foreach (IWebElement video in topFive)
            {
                string title = video.FindElement(By.XPath(".//*[@id='video-title']")).GetAttribute("title");
                string link = video.FindElement(By.XPath(".//a")).GetAttribute("href");
                string uploader = video.FindElement(By.XPath(".//*[@id='channel-name']//a")).GetAttribute("innerText");

                Console.WriteLine(title);
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
                    Console.WriteLine(e);
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