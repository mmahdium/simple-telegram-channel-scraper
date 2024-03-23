using HtmlAgilityPack;
using CsvHelper;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace scraper
{
    public class telegramScraper
    {
        private readonly string _chanelId;

        public telegramScraper(string chanelId)
        {
            _chanelId = chanelId;
        }

        public List<tgmessage> GetLatestMessages()
        {
            var latestMessages = new List<tgmessage>();

            var web = new HtmlWeb();
            
                var url = $"https://t.me/s/{_chanelId}";
                var doc = web.Load(url);

                // Extract messages using reliable XPath selectors
                var messageDivs = doc.DocumentNode.SelectNodes("//div[@class='tgme_widget_message_wrap js-widget_message_wrap']//div[@class='tgme_widget_message text_not_supported_wrap js-widget_message']");

            if (messageDivs != null)
                {
                    foreach (var msgDiv in messageDivs)
                    {
                        // Extract ID using a more robust approach
                        var dataPostAttr = msgDiv.Attributes["data-post"].Value;
                        int messageId = dataPostAttr != null ? int.Parse(Regex.Match(dataPostAttr, @"\d+$").Value) : 0;

                        var messageTextDiv = msgDiv.SelectSingleNode(".//div[@class='tgme_widget_message_bubble']//div[@class='tgme_widget_message_text js-message_text']");
                    string messageText = messageTextDiv.InnerText;

                        if (!string.IsNullOrEmpty(messageText))
                        {
                            latestMessages.Add(new tgmessage { id = messageId, text = messageText });
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Target divs not found on the page!");
                }
            

            return latestMessages;
        }
    }
}