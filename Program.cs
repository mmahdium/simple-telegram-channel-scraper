﻿using CsvHelper;
using System;
using System.Globalization;
using System.IO;

namespace SimpleWebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            string channelId = "Nabz_e_shear"; // Just for example (Must be a public channel)
            var scraper = new scraper.telegramScraper(channelId);
            var latestMessages = scraper.GetLatestMessages();

            using (var writer = new StreamWriter("../../../export.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(latestMessages);
            }
        }
    }
}
