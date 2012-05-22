using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        //create our FeedManager & FeedList items
        RssManager reader = new RssManager();
        Collection<Rss.Items> list;
        ListViewItem row;

        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //execute the GetRssFeeds method in out
                //FeedManager class to retrieve the feeds
                //for the specified URL
                reader.Url = txtURL.Text;
                reader.GetFeed();
                list = reader.RssItems;
                //list = reader
                //now populate out ListBox
                //loop through the count of feed items returned
                for (int i = 0; i < list.Count; i++)
                {
                    //add the title, link and public date
                    //of each feed item to the ListBox
                    row = new ListViewItem();
                    row.Text = list[i].Title;
                    row.SubItems.Add(list[i].Link);
                    row.SubItems.Add(list[i].Date.ToShortDateString());
                    lstNews.Items.Add(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
