# Assignment DevOps & Security

[Selenium](https://www.selenium.dev/documentation/) is the most popular web browser automation project. It is supported by many programming languages, including C#, and emulates user interaction with a browser. This allows one to [automate many user tests](https://www.lambdatest.com/selenium), but there are many other possibilities. 
One of those things is web scraping. It's up to you to get to know Selenium and in this role build it into a Console-based web scraping tool. A useful [tutorial](https://www.lambdatest.com/blog/scraping-dynamic-web-pages/).

The intention is to have at least 3 scraping options:
- Scraping the basic data (link, title of the video, uploader and number of views) of the 5 most recently uploaded Youtube videos based on a search term that the user of the scraping tool can enter.
- Scraping the data on the [jobsite](https://www.ictjob.be/). Hereby the user of the scraping tool must be able to enter a search term, after which the data is retrieved of the 5 most recently uploaded jobs that have been entered under that search term: Title, company, location, keywords and link to the details page.
- 1 self-selected site & data, but always based on a term entered by the user of the scraping tool. 
Scraping data from [Coolblue](https://www.coolblue.be), this will ask the user for input such as what screen size, CPU or RAM size to filter on. Afterwards it will return the name, link, prices and keywords of the 5 cheapest laptops that are on sale.

Furthermore, the following must be taken into account:
- The data is written via to a .CSV file
- The data is written to a .json-file

Creative and technical additions you make are also mentioned and highlighted, as this results in a higher score.