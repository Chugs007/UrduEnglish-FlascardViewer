using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlashCardsViewer
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {

        public delegate void searchforcard(string englishphrase);
        public event searchforcard searchforcardevent;

        public SearchWindow()
        {
            InitializeComponent();
            this.KeyDown += SearchWindow_KeyDown;
        }

        void SearchWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchTextBox.Text))
            {
                MessageBox.Show("No text entered. Please enter a english word/phrase.");
                return;
            }
            searchforcardevent?.Invoke(searchTextBox.Text);
        }


    }
}
