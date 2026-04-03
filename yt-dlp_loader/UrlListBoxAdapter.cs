using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace yt_dlp_loader
{
    internal class UrlListBoxAdapter
    {
        private readonly ListBox listBox;

        public UrlListBoxAdapter(ListBox listBox)
        {
            this.listBox = listBox;
        }

        public void AddIfValid(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || url.Length <= 10)
            {
                return;
            }

            if (!listBox.Items.Contains(url))
            {
                listBox.Items.Add(url);
            }
        }

        public List<string> GetAll()
        {
            return listBox.Items.Cast<string>().ToList();
        }

        public bool HasItems()
        {
            return listBox.Items.Count > 0;
        }

        public string GetFirstOrEmpty()
        {
            return listBox.Items.Count == 0
                ? string.Empty
                : listBox.Items[0]?.ToString() ?? string.Empty;
        }

        public void Clear()
        {
            listBox.Items.Clear();
        }

        public void RemoveFirst()
        {
            if (listBox.Items.Count == 0)
            {
                return;
            }

            listBox.Items.RemoveAt(0);
        }
    }
}
