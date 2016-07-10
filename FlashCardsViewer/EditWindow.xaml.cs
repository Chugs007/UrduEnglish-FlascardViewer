using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public delegate void ApplyChange(string text1, string text2);
        public event ApplyChange ApplyChangeEvent;

        public EditWindow(FlashCardSet kvp)
        {
            InitializeComponent();
            this.txtBoxUrduWord.Text = kvp.Value.UrduPhrase;
            this.txtBoxEnglishWord.Text = kvp.Value.EnglishPhrase;
        }

        private void Button_Apply(object sender, RoutedEventArgs e)
        {
            if (ApplyChangeEvent != null)
                ApplyChangeEvent(this.txtBoxUrduWord.Text,this.txtBoxEnglishWord.Text);
            this.Close();
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
