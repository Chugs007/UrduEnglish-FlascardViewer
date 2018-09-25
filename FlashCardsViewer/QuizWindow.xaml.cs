using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for QuizWindow.xaml
    /// </summary>
    public partial class QuizWindow : Window
    {
        public delegate void GenerateRandomCards();

        public event GenerateRandomCards GenerateRandomCardsEvent;

        private Dictionary<string, string> flashCards;
        private SolidColorBrush correctAnswerBrush = new SolidColorBrush(Colors.Transparent);
        private SolidColorBrush wrongAnswerBrush = new SolidColorBrush(Colors.Red);
        private const string DEFAULTANSWER= "textboxAnswer";
        private int numberCorrect = 0;

        public QuizWindow()
        {
            InitializeComponent();
            this.KeyDown += QuizWindow_KeyDown;
        }

        void QuizWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        public static readonly DependencyProperty ShowStatusImageProperty = DependencyProperty.Register("ShowStatusImage", typeof(bool), typeof(QuizWindow), new PropertyMetadata(false));
  
        public bool ShowStatusImage
        {
            get
            {
                return (bool)GetValue(ShowStatusImageProperty);
            }

            set
            {
                SetValue(ShowStatusImageProperty, value);
            }
        }

        public void GenerateQuestions(Dictionary<string, string> randomCards)
        {
            foreach (TextBox tb in stackPanelTextBoxes.Children)
            {
                tb.Text = string.Empty;
            }
            ShowStatusImage = false;
            flashCards = randomCards;
            textBlockQuestion1.Text = randomCards.ElementAt(0).Key;
            textBlockQuestion2.Text = randomCards.ElementAt(1).Key;
            textBlockQuestion3.Text = randomCards.ElementAt(2).Key;
            textBlockQuestion4.Text = randomCards.ElementAt(3).Key;
            textBlockQuestion5.Text = randomCards.ElementAt(4).Key;
            textBlockQuestion6.Text = randomCards.ElementAt(5).Key;
            textBlockQuestion7.Text = randomCards.ElementAt(6).Key;
            textBlockQuestion8.Text = randomCards.ElementAt(7).Key;
            textBlockQuestion9.Text = randomCards.ElementAt(8).Key;
            textBlockQuestion10.Text = randomCards.ElementAt(9).Key;
        }

        private void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            
            if (textboxAnswer1.Text != flashCards.ElementAt(0).Value)
            {                
                imageStatus1.Source = new BitmapImage(new Uri(@"/Resources/X_Icon_clip_art_small.png",UriKind.Relative));
            }
            else
            {                
                imageStatus1.Source = new BitmapImage(new Uri(@"Resources/Tick_clip_art_small.png",UriKind.Relative));
                numberCorrect++;
            }

            if (textboxAnswer2.Text != flashCards.ElementAt(1).Value)
            {                
                imageStatus2.Source = new BitmapImage(new Uri(@"Resources/X_Icon_clip_art_small.png", UriKind.Relative));
            }
            else
            {                
                imageStatus2.Source = new BitmapImage(new Uri(@"Resources/Tick_clip_art_small.png", UriKind.Relative));
                numberCorrect++;
            }

            if (textboxAnswer3.Text != flashCards.ElementAt(2).Value)
            {                
                imageStatus3.Source = new BitmapImage(new Uri(@"Resources/X_Icon_clip_art_small.png", UriKind.Relative));
            }
            else
            {                
                imageStatus3.Source = new BitmapImage(new Uri(@"Resources/Tick_clip_art_small.png", UriKind.Relative));
                numberCorrect++;
            }

            if (textboxAnswer4.Text != flashCards.ElementAt(3).Value)
            {               
                imageStatus4.Source = new BitmapImage(new Uri(@"Resources/X_Icon_clip_art_small.png", UriKind.Relative));
            }
            else
            {                
                imageStatus4.Source = new BitmapImage(new Uri(@"Resources/Tick_clip_art_small.png", UriKind.Relative));
                numberCorrect++;
            }

            if (textboxAnswer5.Text != flashCards.ElementAt(4).Value)
            {               
                imageStatus5.Source = new BitmapImage(new Uri(@"Resources/X_Icon_clip_art_small.png", UriKind.Relative));
            }
            else
            {                
                imageStatus5.Source = new BitmapImage(new Uri(@"Resources/Tick_clip_art_small.png", UriKind.Relative));
                numberCorrect++;
            }

            if (textboxAnswer6.Text != flashCards.ElementAt(5).Value)
            {                
                imageStatus6.Source = new BitmapImage(new Uri(@"Resources/X_Icon_clip_art_small.png", UriKind.Relative));
            }
            else
            {           
                imageStatus6.Source = new BitmapImage(new Uri(@"Resources/Tick_clip_art_small.png", UriKind.Relative));
                numberCorrect++;
            }

            if (textboxAnswer7.Text != flashCards.ElementAt(6).Value)
            {                
                imageStatus7.Source = new BitmapImage(new Uri(@"Resources/X_Icon_clip_art_small.png", UriKind.Relative));
            }
            else
            {                
                imageStatus7.Source = new BitmapImage(new Uri(@"Resources/Tick_clip_art_small.png", UriKind.Relative));
                numberCorrect++;
            }

            if (textboxAnswer8.Text != flashCards.ElementAt(7).Value)
            {              
                imageStatus8.Source = new BitmapImage(new Uri(@"Resources/X_Icon_clip_art_small.png", UriKind.Relative));
            }
            else
            {                
                imageStatus8.Source = new BitmapImage(new Uri(@"Resources/Tick_clip_art_small.png", UriKind.Relative));
                numberCorrect++;
            }

            if (textboxAnswer9.Text != flashCards.ElementAt(8).Value)
            {                
                imageStatus9.Source = new BitmapImage(new Uri(@"Resources/X_Icon_clip_art_small.png", UriKind.Relative));
            }
            else
            {                
                numberCorrect++;
                imageStatus9.Source = new BitmapImage(new Uri(@"Resources/Tick_clip_art_small.png", UriKind.Relative));
            }

            if (textboxAnswer10.Text != flashCards.ElementAt(9).Value)
            {                
                imageStatus10.Source = new BitmapImage(new Uri(@"Resources/X_Icon_clip_art_small.png", UriKind.Relative));
            }
            else
            {                
                imageStatus10.Source = new BitmapImage(new Uri(@"Resources/Tick_clip_art_small.png", UriKind.Relative));
                numberCorrect++;
            }

            ShowStatusImage = true;
            System.Windows.Forms.MessageBox.Show("You got " + numberCorrect + " out of 10 correct!");

            numberCorrect = 0;
        }

        private void buttonGenerateQuiz_Click(object sender, RoutedEventArgs e)
        {
            foreach (var child in stackPanelTextBoxes.Children)
            {
                TextBox tb = child as TextBox;
                tb.Background = correctAnswerBrush;
            }
            GenerateRandomCardsEvent?.Invoke();
        }

        private void ShowAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (flashCards == null)
            {
                MessageBox.Show("Must generate quiz first!");
                return;
            }
            for (int i = 0; i < flashCards.Count; i++)
            {
                TextBox tb = stackPanelTextBoxes.Children[i] as TextBox;
                tb.Text = flashCards.ElementAt(i).Value;
            }            
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            foreach (var child in stackPanelTextBoxes.Children)
            {
                TextBox tb = child as TextBox;
                tb.Text = string.Empty;
            }

            ShowStatusImage = false;
        }  
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;
            return val ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility val = (Visibility)value;

            return Visibility.Visible == val;
        }
    }
   
}