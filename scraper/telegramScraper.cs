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

            // Extract messages using XPath selectors
            var messageDivs = doc.DocumentNode.SelectNodes("//div[@class='tgme_widget_message_wrap js-widget_message_wrap']//div[@class='tgme_widget_message text_not_supported_wrap js-widget_message']");

            if (messageDivs != null)
            {
                foreach (var msgDiv in messageDivs)
                {
                    // Extract ID 
                    var dataPostAttr = msgDiv.Attributes["data-post"].Value;
                    UInt32 messageId = dataPostAttr != null ? UInt32.Parse(Regex.Match(dataPostAttr, @"\d+$").Value) : 0;

                    var messageTextDiv = msgDiv.SelectSingleNode(".//div[@class='tgme_widget_message_bubble']//div[@class='tgme_widget_message_text js-message_text']");
                    string messageText = messageTextDiv.InnerText;

                    if (!string.IsNullOrEmpty(messageText))
                    {
                        latestMessages.Add(new tgmessage { id = messageId, text = messageText });
                    }
                }
                return latestMessages;

            }
            else
            {
                throw new Exception("Target divs not found on the page!");
            }

        }


        public tgmessage GetMessageById(UInt32 messIageId)
        {

            var web = new HtmlWeb();
            var url = $"https://t.me/{_chanelId}/{messIageId}";
            var doc = web.Load(url);

            // Extract messages using XPath selectors
            var singleMessageText = doc.DocumentNode.SelectSingleNode("//meta[@name='twitter:description']").Attributes["content"].Value;

            if (singleMessageText != null)
            {
                return new tgmessage { id = messIageId, text = singleMessageText };
            }
            else
            {
                throw new Exception("Message does not exist on the page!");
            }
        }
    }
}