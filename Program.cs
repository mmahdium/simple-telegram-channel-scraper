using CsvHelper;
using System;
using System.Globalization;
using System.IO;

namespace SimpleWebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            string channelId = "thezoomit";

            var scraper = new scraper.telegramScraper(channelId);

            // Scrape a single message by its ID and save it to a text file
            File.WriteAllText(Environment.CurrentDirectory + "/WriteLines.txt", scraper.GetMessageById(76198).text);


            // Scrape the latest messages and save them to a CSV file
            var latestMessages = scraper.GetLatestMessages();
            using (var writer = new StreamWriter(Environment.CurrentDirectory + "/export.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(latestMessages);
            }

        }
    }
}
