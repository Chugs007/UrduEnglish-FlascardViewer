//-------------------------------------------------------------------
// 2015-16 All rights reserved. 
// Created by:	Omar Chughtai
//-------------------------------------------------------------------

using Microsoft.VisualBasic.FileIO;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;


namespace FlashCardsViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region private variables
        private const string flashCardNumber = "FlashCard #"; //represents a flashcard by what number it is
        private const string flashCardNotSelected = "No Flash Card Selected!!";
        private bool urduSide = true; //denotes whether the urdu side is showing or english side is
        private int handle = 0; //represents position inside collection
        private MediaPlayer mp;
        private bool isPlaying = false;
        private ObservableCollection<KeyValuePair> dict; //collection of key values
        private object currentItem;
        private ObservableCollection<KeyValuePair> dictCopy;
        private string defaultFilePath = @"C:\Users\" + Environment.UserName + @"\Desktop\urdu_to_english.csv";
        private QuizWindow qw;
        
        #endregion
        

        public MainWindow()
        {
            SplashScreen sc = new SplashScreen(@"Resources\pakistan_splash2.jpg");
            sc.Show(false);
            sc.Close(TimeSpan.FromSeconds(3));
            System.Threading.Thread.Sleep(3000);
            sc = null;
            InitializeComponent();
            mp = new MediaPlayer();
            dict = new ObservableCollection<KeyValuePair>();
            dictCopy = new ObservableCollection<KeyValuePair>();
            listBoxFlashcards.DataContext = dict;
            if (string.IsNullOrEmpty(Properties.Settings.Default.filePath))
                Properties.Settings.Default.filePath = defaultFilePath;
            Properties.Settings.Default.Save();                     
        }

        ~MainWindow()
        {         
            using (StreamWriter sw = new StreamWriter(Properties.Settings.Default.filePath))          
            {
                sw.WriteLine("Urdu, English");
                foreach (KeyValuePair kvp in dict)
                {
                    sw.WriteLine(kvp.Value.UrduPhrase + ", " + kvp.Value.EnglishPhrase);
                }
            }
           
        }

        private void GetFlashCardsFromFile()
        {
            TextFieldParser parser;
            try
            {
                parser = new TextFieldParser(Properties.Settings.Default.filePath);
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.ReadFields(); //skips first line
                while (!parser.EndOfData)
                {
                    string[] phrases = parser.ReadFields();
                    FlashCard fc = new FlashCard();
                    fc.UrduPhrase = phrases[0];
                    fc.EnglishPhrase = phrases[1];
                    dict.Add(new KeyValuePair() { Key = flashCardNumber + ++handle, Value = fc });
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }
     
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (!File.Exists(Properties.Settings.Default.filePath))
            {
                using (StreamWriter sw = new StreamWriter(Properties.Settings.Default.filePath))
                {
                    sw.WriteLine("Urdu, English");
                    sw.WriteLine("ab" + ", " + "now");
                    sw.WriteLine("in" + ", " + "those");
                    sw.WriteLine("aisa" + ", " + "such");
                    sw.WriteLine("amir" + ", " + "rich");
                    sw.WriteLine("aur" + ", " + "and");
                    sw.WriteLine("mera" + ", " + "my");
                    sw.WriteLine("muhjhe" + ", " + "me");
                    sw.WriteLine("eik" + ", " + "one");
                    sw.WriteLine("nam" + ", " + "name");
                    sw.WriteLine("batie" + ", " + "sit down");
                    sw.WriteLine("kahan" + ", " + "where");
                    sw.WriteLine("hamara" + ", " + "our");
                    sw.WriteLine("ham" + ", " + "we");
                    sw.WriteLine("voh" + ", " + "he/she/it");
                    sw.WriteLine("yih" + ", " + "he/she/it");
                    sw.WriteLine("hal" + ", " + "condition");
                    sw.WriteLine("hai" + ", " + "is");
                    sw.WriteLine("larka" + ", " + "boy");
                    sw.WriteLine("larkee" + ", " + "girl");
                    sw.WriteLine("bachay" + ", " + "children");
                    sw.WriteLine("bacha" + ", " + "child");
                    sw.WriteLine("tum" + ", " + "you(informal)");
                    sw.WriteLine("ap" + ", " + "you(formal)");
                    sw.WriteLine("kitna" + ", " + "how many");
                    sw.WriteLine("sar" + ", " + "head");
                    sw.WriteLine("sayhat" + ", " + "health");
                    sw.WriteLine("kab" + ", " + "when");
                    sw.WriteLine("koi" + ", " + "someone");
                    sw.WriteLine("sunna" + ", " + "hear");
                    sw.WriteLine("suna" + ", " + "heard");
                    sw.WriteLine("batana" + ", " + "tell");
                    sw.WriteLine("naachna" + ", " + "dance");
                    sw.WriteLine("bemar" + ", " + "sick");
                    sw.WriteLine("kuch" + ", " + "some/something");
                }             
            }

            GetFlashCardsFromFile();
        }

        private void listBoxFlashcards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem ==null)
            {
                e.Handled = true;
                return;
            }
            urduSide = true;
            string lbi = listBoxFlashcards.SelectedItem.ToString();            
            this.txtBlockCardData.Text = dict.Single(x => x.Key == lbi).Value.UrduPhrase;
            this.txtBlockCardData.Visibility = Visibility.Visible;
            this.flashCardBorder.Visibility = Visibility.Visible;
            currentItem = listBoxFlashcards.SelectedItem;       
        }

        private void Button_ShowFlashCard(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(flashCardNotSelected);
                e.Handled = true;            
                return;
            }
            string lbi = listBoxFlashcards.SelectedItem.ToString();
            this.txtBlockCardData.Text = dict.Single(x => x.Key == lbi).Value.UrduPhrase.ToString();
            this.txtBlockCardData.Visibility = Visibility.Visible;
            this.flashCardBorder.Visibility = Visibility.Visible;

           

        }

        private void Button_FlipFlashCard(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(flashCardNotSelected);
                e.Handled = true;
                return;
            }
            string lbi = listBoxFlashcards.SelectedItem.ToString();

            if (urduSide)
            {
                this.txtBlockCardData.Text = dict.Single(x => x.Key == lbi).Value.EnglishPhrase.ToString();
                urduSide = false;
            }
            else
            {
                this.txtBlockCardData.Text = dict.Single(x => x.Key == lbi).Value.UrduPhrase.ToString();
                urduSide = true;
            }
            this.listBoxFlashcards.Focus();
      
        }

        private void Button_AddFlashCard(object sender, RoutedEventArgs e)
        {
            AddWindow ad = new AddWindow();
            ad.Show();
            
            ad.AddFlashCardEvent += ad_AddFlashCardEvent;
        }

        void ad_AddFlashCardEvent(string urduWord, string englishWord)
        {
            FlashCard fc = new FlashCard();
            fc.UrduPhrase = urduWord;
            fc.EnglishPhrase = englishWord;
            dict.Add(new KeyValuePair() { Key = flashCardNumber + ++handle, Value = fc });
               
        }

        private void Button_DeleteFlascard(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem==null)
            {
                System.Windows.Forms.MessageBox.Show(flashCardNotSelected);
                e.Handled = true;
                return;
            }
            string key = listBoxFlashcards.SelectedItem.ToString();
            dict.Remove(dict.First(x => x.Key == key));

            
            KeyValuePair[] dictCopy=new KeyValuePair[dict.Count()];
            dict.CopyTo(dictCopy,0);
            dict.Clear();
            handle = 0;
            foreach(KeyValuePair kvp in dictCopy)
            {
                dict.Add(new KeyValuePair() { Key = flashCardNumber + ++handle, Value = new FlashCard() { UrduPhrase = kvp.Value.UrduPhrase, EnglishPhrase = kvp.Value.EnglishPhrase } });
                
            }
            this.txtBlockCardData.Visibility = Visibility.Hidden;
            this.flashCardBorder.Visibility = Visibility.Hidden;           
        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(flashCardNotSelected);
                e.Handled = true;
                return;
            }
            KeyValuePair kvp = listBoxFlashcards.SelectedItem as KeyValuePair;                        
            EditWindow ew = new EditWindow(kvp);
            ew.Show();

            ew.ApplyChangeEvent += ew_ApplyChangeEvent;
        }

        void ew_ApplyChangeEvent(string text1, string text2)
        {
            string key = listBoxFlashcards.SelectedItem.ToString();
            dict.Single(x => x.Key == key).Value.UrduPhrase = text1;
            dict.Single(x => x.Key == key).Value.EnglishPhrase = text2;            
        }

        private void Button_Speak(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(flashCardNotSelected);
                e.Handled = true;
                return;
            }

            SpeechSynthesizer speech = new SpeechSynthesizer();
            KeyValuePair kvp = listBoxFlashcards.SelectedItem as KeyValuePair;

            if (urduSide)
                speech.Speak(kvp.Value.UrduPhrase);
            else
                speech.Speak(kvp.Value.EnglishPhrase);
        }      

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double oldHeight;
            double oldWidth;          
            oldHeight=(double)e.PreviousSize.Height;
            oldWidth = (double)e.PreviousSize.Width;
            MainWindow mw = sender as MainWindow;
            if (mw.WindowState==WindowState.Maximized)
            {
                txtBlockCardData.FontSize = 130;
            }
            if (oldWidth < e.NewSize.Width || oldHeight < e.NewSize.Height)
            {
                if (txtBlockCardData.FontSize == 130)
                    return;
                txtBlockCardData.FontSize += 1;
            }
            if (oldHeight > e.NewSize.Height || oldWidth > e.NewSize.Width)
            {
                if (txtBlockCardData.FontSize == 78)
                    return;
                txtBlockCardData.FontSize -= 1;
            }
        }

        private void AnthemButton_Click(object sender, RoutedEventArgs e)
        {
            if (mp.Source ==null)
            {
                mp.Open(new Uri(@"Resources\National-Anthem-Pakistan.mp3", UriKind.Relative));
                mp.Play();
                isPlaying = true;
                AnthemButton.Content = "Pause National Anthem";
            }
            else
            {
                if (isPlaying)
                {
                    mp.Pause();
                    isPlaying = false;
                    AnthemButton.Content = "Play National Anthem";
                }
                else
                {
                    mp.Play();
                    isPlaying = true;
                    AnthemButton.Content = "Pause National Anthem";
                }
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(flashCardNotSelected);
                e.Handled = true;
                return;
            }

            KeyValuePair item = listBoxFlashcards.SelectedItem as KeyValuePair;
            int currentPosition = dict.IndexOf(item);
            if (currentPosition == dict.Count()-1)
            {
                listBoxFlashcards.SelectedItem = dict.ElementAt(0);
            }
            else
            {
                listBoxFlashcards.SelectedItem = dict.ElementAt(currentPosition + 1);
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(flashCardNotSelected);
                e.Handled = true;
                return;
            }
            KeyValuePair item = listBoxFlashcards.SelectedItem as KeyValuePair;
            int currentPosition = dict.IndexOf(item);
            if (currentPosition == 0)
            {
                listBoxFlashcards.SelectedItem = dict.ElementAt(dict.Count()-1);
            }
            else
            {
                listBoxFlashcards.SelectedItem = dict.ElementAt(currentPosition -1);
            }
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            if (dict != null)
            {
                 int randomnumber=0;
                 if (dict.Count() > 0)
                 {
                     randomnumber = r.Next(dict.Count());
                     listBoxFlashcards.SelectedItem = dict.ElementAt(randomnumber);
                 }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No flashcards available!!");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (dict != null)
            {
                
           DialogResult result=System.Windows.Forms.MessageBox.Show("Do you want to save this list?", "Save List",MessageBoxButtons.YesNo );
                
            if (result==System.Windows.Forms.DialogResult.Yes)
            {
                foreach (KeyValuePair k in dict)
                {
                    dictCopy.Add(k);
                }
            }
                dict.Clear();               
            }            
        }

        private void RetrieveButton_Click(object sender, RoutedEventArgs e)
        {
            if (dictCopy != null)
               foreach(KeyValuePair x in dictCopy)
               {
                   dict.Add(x);
               }
            else
            {
                System.Windows.Forms.MessageBox.Show("No list stored!");
            }
        }

       private void FirstButton_Click(object sender, RoutedEventArgs e)
       {
            if (dict !=null && dict.Count() > 0)
            {
                listBoxFlashcards.SelectedItem = dict.ElementAt(0);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No flashcards available!");
            }
       }

       private void LastButton_Click(object sender, RoutedEventArgs e)
       {
            if (dict != null && dict.Count() > 0)
            {
                listBoxFlashcards.SelectedItem = dict.ElementAt(dict.Count() - 1);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No flashcards available!");
            }
       }

       private void TakeQuizButton_Click(object sender, RoutedEventArgs e)
       {
         
           if (dict != null)
           {
               if (dict.Count() >= 10)
               {
                   Dictionary<string, string> randomFlashCards = DrawRandomCards();
                   qw = new QuizWindow();
                   qw.Show();
                   qw.GenerateQuestions(randomFlashCards);
                   qw.GenerateRandomCardsEvent += qw_GenerateRandomCardsEvent;
               }
               else
               {
                   System.Windows.Forms.MessageBox.Show("Need at least 10 flashcards to take quiz!!");
               }
           }          
       }

       void qw_GenerateRandomCardsEvent()
       {
          Dictionary<string,string> randomCards= DrawRandomCards();
          qw.GenerateQuestions(randomCards);
       }
        
        private Dictionary<string,string> DrawRandomCards()
        {
            Random r = new Random();
            int[] randomNumbers = new int[10];
            for (int i = 0; i < 10; i++)
            {
                int temp = r.Next(dict.Count());
                while (randomNumbers.Contains(temp))
                {
                    temp = r.Next(dict.Count());
                }
                randomNumbers[i] = temp;
            }

            Dictionary<string, string> randomFlashCards = new Dictionary<string, string>();
            for (int i = 0; i < 10; i++)
            {
                FlashCard fs = dict.ElementAt(randomNumbers[i]).Value;
                randomFlashCards.Add(fs.UrduPhrase, fs.EnglishPhrase);
            }

            return randomFlashCards;
        }
    }
}
