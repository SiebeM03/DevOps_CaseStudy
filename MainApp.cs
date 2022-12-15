using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

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
            switch (choice.KeyChar)
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
            // driver.Close();
        }

        void YoutubeApp()
        {
            Console.Clear();

            YoutubeApp youtubeApp = new YoutubeApp();
            youtubeApp.Search();
            // youtubeApp.AcceptCookies();
            // youtubeApp.SetFilters();
            youtubeApp.GetTopFive();
            youtubeApp.Close();
        }

        void JobApp()
        {
            Console.Clear();

            JobApp jobApp = new JobApp();
            jobApp.Search();
            jobApp.Sort();
            jobApp.GetTopFive();
            jobApp.Close();
        }

        void ShopApp()
        {
            driver = new ChromeDriver();
            driver.Url = "https://www.coolblue.be/nl";
            Console.WriteLine("SHOP");
        }
    }
}
