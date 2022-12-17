using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.IO;
using System.Globalization;
using System.Security.Permissions;
using System.Collections.ObjectModel;

namespace CaseStudy
{
    internal class ShopApp
    {
        IWebDriver driver;
        string url;

        Dictionary<string, string> valueToUrl = new Dictionary<string, string>()
        {
            // CPU
            { "i5", "intel-core-i5" },
            { "i7", "intel-core-i7" },
            { "Ryzen 5", "intel-core-i5" },
            { "Ryzen 7", "intel-core-i5" },
            // RAM
            { "8GB", "8000000000" },
            { "16GB", "16000000000" },
            { "32GB", "32000000000" },
            // SIZE
            { "13", "0.3302-0.35052" },
            { "14", "0.35306-0.37846" },
            { "15", "0.381-0.40386" },
            { "16", "0.4064-0.430784" },
            { "17", "0.4318-0.45466" }
        };
        List<string> filters;
        string sizes, processors, rams;

        List<Laptop> results = new List<Laptop>();

    public void Search()
        {
            url = "https://www.coolblue.be/nl/aanbieding/producttype:laptops";

            Console.WriteLine("Searching for Windows laptops in sale on Coolblue...");
            Console.WriteLine("Enter one of the options between parentheses (simply hit enter if you dont want a filter applied). Seperate multiple filters with a comma.");


            // SIZE
            Console.Write("Screen size (13, 14, 15, 16, 17)\t");
            sizes = Console.ReadLine();
            if (!string.IsNullOrEmpty(sizes))
            {
                url += "/schermdiagonaal:";
            }
            filters = new List<string>();
            foreach (string item in sizes.Split(','))
            {
                if (valueToUrl.ContainsKey(item))
                {
                    filters.Add(valueToUrl[item]);
                }
            }
            url += string.Join(",", filters.ToArray());


            // CPU
            Console.Write("Processor (i5, i7, Ryzen 5, Ryzen 7)\t");
            processors = Console.ReadLine();
            if (!string.IsNullOrEmpty(processors))
            {
                url += "/processor:";
            }

            filters = new List<string>();
            foreach (string item in processors.Split(','))
            {
                if (valueToUrl.ContainsKey(item))
                {
                    filters.Add(valueToUrl[item]);
                }
            }
            url += string.Join(",", filters.ToArray());


            // RAM
            Console.Write("RAM memory (8GB, 16GB, 32GB)\t\t");
            rams = Console.ReadLine();
            if (!string.IsNullOrEmpty(rams))
            {
                url += "/intern-werkgeheugen-ram:";
            }

            filters = new List<string>();
            foreach (string item in rams.Split(','))
            {
                if (valueToUrl.ContainsKey(item))
                {
                    filters.Add(valueToUrl[item]);
                }
            }
            url += string.Join(",", filters.ToArray());

            url += "/alleen-op-voorraad:ja?sorteren=laagste-prijs";

            driver = new ChromeDriver();
            driver.Url = url;
        }

        public void GetTopFive()
        {
            List<IWebElement> topFive = new List<IWebElement>();
            int counter = 1;

            int resultAmount = (driver.FindElements(By.XPath("//*[@id=\"product-results\"]/div[1]/div/div/div[contains(concat(' ', normalize-space(@class), ' '), \"product-card\")]"))).Count;
            Console.WriteLine(resultAmount.ToString());

            // Select the first 5, a page only contains 36 div's with the given xpath, therefore we also want to stop when all
            // div's have been checked but there weren't 5 laptops found (this means there were less than 5 results)
            while (topFive.Count < 5 && topFive.Count != resultAmount)
            {
                string xpath = "//*[@id=\"product-results\"]/div[1]/div[" + counter + "]";
                IWebElement laptop = WaitForElement(xpath);

                // CHECK IF ELEMENT ACTUALLY IS A SHOP ITEM (MIGHT ALSO BE A MESSAGE IN BETWEEN SHOP ITEMS)
                if (laptop.FindElements(By.XPath(".//div[contains(concat(' ', normalize-space(@class), ' '), \"product-card\")]")).Count >= 1)
                {
                    topFive.Add(laptop);
                }
                counter++;
            }

            foreach (IWebElement laptop in topFive)
            {
                string name = laptop.FindElement(By.XPath(".//*[@class=\"product-card__title\"]//a")).GetAttribute("innerText");
                string url = laptop.FindElement(By.XPath(".//*[@class=\"product-card__title\"]//a")).GetAttribute("href");
                string formerPrice = laptop.FindElement(By.XPath(".//*[@class=\"sales-price__former\"]")).Text;
                string currentPrice = laptop.FindElement(By.XPath(".//strong[\"sales-price__current js-sales-price-current\"]")).Text;
                List<string> highlights = new List<string>();
                foreach (IWebElement item in laptop.FindElements(By.XPath(".//div[@class=\"product-card__highlights\"]//span")))
                {
                    if (!item.GetAttribute("class").Contains("dynamic-highlight__separator"))
                    {
                        highlights.Add(item.Text);
                    }
                }

                results.Add(new Laptop(name, url, formerPrice, currentPrice, highlights));
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

        public void WriteOutput()
        {
            if (!Directory.Exists("outputResults/ShopApp"))
            {
                Directory.CreateDirectory("outputResults/ShopApp");
            }

            WriteCSV();
            WriteJSON();
        }

        public void WriteCSV()
        {
            string separator = ";";
            StringBuilder stringcsv = new StringBuilder();
            string[] headings = { "Name", "Former Price", "Current Price", "Highlights", "Url" };
            stringcsv.AppendLine(string.Join(separator, headings));

            foreach (Laptop laptop in results)
            {
                String[] newLine = { laptop.name, laptop.formerPrice, laptop.currentPrice, "[" + String.Join(",", laptop.highlights) + "]", laptop.url };
                stringcsv.AppendLine(string.Join(separator, newLine));
            }

            File.WriteAllText("./outputResults/ShopApp/results.csv", stringcsv.ToString());
        }

        public void WriteJSON()
        {
            var obj = new
            {
                url = url,
                filters = new dynamic[] {
                    new { size = sizes.Split(',') },
                    new { processors = processors.Split(',') },
                    new { ram = rams.Split(',') }
                },
                result = results
            };

            string stringjson = JsonConvert.SerializeObject(obj, Formatting.Indented);

            File.WriteAllText("./outputResults/ShopApp/results.json", stringjson);
        }

        public void Close()
        {
            driver.Close();
        }
    }

    class Laptop
    {
        public string name, url, formerPrice, currentPrice;
        public List<string> highlights;

        public Laptop(string name, string url, string formerPrice, string currentPrice, List<string> highlights)
        {
            this.name = name;
            this.url = url;
            this.formerPrice = formerPrice;
            this.currentPrice = currentPrice;
            this.highlights = highlights;
        }
    } 
}
