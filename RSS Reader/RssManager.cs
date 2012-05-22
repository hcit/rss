using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections.ObjectModel;
using WindowsFormsApplication1;

namespace WindowsFormsApplication1
{
    public class RssManager : IDisposable

    {
        private string _url;
        private string _feedTitle;
        private string _feedDescription;
        private Collection<Rss.Items> _rssItems = new Collection<Rss.Items>();
        private bool _IsDisposed;
        
        #region Constructors

        public RssManager()
        {
            _url = string.Empty;
        }

        public RssManager(string feedUrl)
        {
            _url = feedUrl;
        }
        
        #endregion

        #region Properties

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public Collection<Rss.Items> RssItems
        {
            get { return _rssItems; }
        }

        public string FeedTitle
        {
            get { return _feedTitle; }
        }

        public string FeedDescription
        {
            get { return _feedDescription; }
        }

        #endregion

        public Collection<Rss.Items> GetFeed()
        {
            //check to see if the FeedURL is empty
            if (String.IsNullOrEmpty(Url))
            {
                //throw an exception if not provided
                throw new ArgumentException("You must provide a feed URL");
            }
            //start the parsing process
            using (XmlReader reader = XmlReader.Create(Url))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(reader);
                //parse the items of the feed
                ParseDocElements(xmlDoc.SelectSingleNode("//channel"), "title", ref _feedTitle);
                ParseDocElements(xmlDoc.SelectSingleNode("//channel"), "description", ref _feedDescription);
                ParseRssItems(xmlDoc);
                //return the feed items
                return _rssItems;
            }
        }

        /// <summary>
        /// Parses the xml document in order to retrieve the RSS items.
        /// </summary>
        private void ParseRssItems(XmlDocument xmlDoc)
        {
            _rssItems.Clear();
            XmlNodeList nodes = xmlDoc.SelectNodes("rss/channel/item");

            foreach (XmlNode node in nodes)
            {
                Rss.Items item = new Rss.Items();
                ParseDocElements(node, "title", ref item.Title);
                ParseDocElements(node, "description", ref item.Description);
                ParseDocElements(node, "link", ref item.Link);

                string date = null;
                ParseDocElements(node, "pubDate", ref date);
                DateTime.TryParse(date, out item.Date);

                _rssItems.Add(item);
            }
        }
        /// <summary>
        /// Parses the XmlNode with the specified XPath query 
        /// and assigns the value to the property parameter.
        /// </summary>
        private void ParseDocElements(XmlNode parent, string xPath, ref string property)
        {
            XmlNode node = parent.SelectSingleNode(xPath);
            if (node != null)
                property = node.InnerText;
            else
                property = "Unresolvable";
        }
        #region IDisposable Members

        /// <summary>
        /// Performs the disposal.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (disposing && !_IsDisposed)
            {
                _rssItems.Clear();
                _url = null;
                _feedTitle = null;
                _feedDescription = null;
            }

            _IsDisposed = true;
        }

        /// <summary>
        /// Releases the object to the garbage collector
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
