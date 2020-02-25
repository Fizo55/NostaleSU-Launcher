using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using TextBox = System.Windows.Controls.TextBox;

namespace WowSuite.Utils
{
    public class TextSearchFilter
    {
        public TextSearchFilter(ICollectionView filteredView, TextBox searchCharBox)
        {
            string filterText = "";
            filteredView.Filter = delegate(object obj)
            {
                if (String.IsNullOrEmpty(filterText))
                    return true;
                string str = obj as string;
                if (String.IsNullOrEmpty(str))
                    return false;
                int index = str.IndexOf(filterText, 0, StringComparison.InvariantCultureIgnoreCase);
                return index > -1;
            };

            searchCharBox.TextChanged += delegate
            {
                filterText = searchCharBox.Text;
                filteredView.Refresh();
            };
        }
    }
}