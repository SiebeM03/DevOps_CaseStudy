using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CaseStudy
{
    class MainApp
    {
        static IWebDriver driver;

        static void Main(string[] args)
        {
            MainApp app = new MainApp();

            Console.WriteLine("1\tYoutubeApp");
            Console.WriteLine("2\tJobApp");
            Console.WriteLine("3\tShopApp"); 

            ConsoleKeyInfo choice = Console.ReadKey();
            switch(choice.KeyChar)
            { 
                case '1':
                    app.YoutubeApp();
                    break;
                case '2':
                    app.JobApp();
                    break;
                case '3':
                    app.ShopApp();
                    break;
            }

            Console.ReadLine();
            driver.Close();
        }

        void YoutubeApp()
        {
            driver = new ChromeDriver();
            driver.Url = "http://www.youtube.com";
            Console.WriteLine("YOUTUBE");
        }

        void JobApp()
        {
            driver = new ChromeDriver();
            driver.Url = "https://www.ictjob.be";
            Console.WriteLine("JOB");
        }

        void ShopApp()
        {
            driver = new ChromeDriver();
            driver.Url = "https://www.coolblue.be/nl";
            Console.WriteLine("SHOP");
        }
    }
}
