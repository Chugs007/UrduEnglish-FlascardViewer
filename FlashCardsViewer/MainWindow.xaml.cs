//-------------------------------------------------------------------
// 2016-17 All rights reserved. 
// Created by:	Omar Chughtai
//-------------------------------------------------------------------

using Microsoft.VisualBasic.FileIO;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace FlashCardsViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region private variables
        private const string FLASHCARDNUMBER = "FlashCard #"; //represents a flashcard by what number it is
        private const string FLASHCARDNOTSELECTED = "No Flash Card Selected!!";
        private const string ADDCARD = "Add Card";
        private const string DELETECARD = "Delete Card";
        private const string EDITCARD = "Edit Card";
        private const string FLIPCARD = "Flip Card";
        private const string HEARPHRASE = "Hear Phrase";
        private const string NEXTCARD = "Next Card";
        private const string PREVIOUSCARD = "Previous Card";
        private const string RANDOMCARD = "Random Card";
        private const string FIRSTCARD = "First Card";
        private const string LASTCARD = "Last Card";
        private const string TAKEQUIZ = "Take Quiz";
        private const string SAVELIST = "Save List";
        private const string SEARCH = "Search";
        private bool urduSide = true; //denotes whether the urdu side is showing or english side is
        private int handle = 0; //represents position inside collection            
        private ObservableCollection<FlashCardSet> flashCardCollection; //collection of key values         
        private string defaultFilePath = "urdu_to_english.csv";
        private QuizWindow qw;
        private SearchWindow sw;
        private AddWindow ad;          
        #endregion
        

        public MainWindow()
        {
            SplashScreen sc = new SplashScreen(@"Resources\pakistan_splash2.jpg");            
            sc.Show(false);
            sc.Close(TimeSpan.FromSeconds(3));
            System.Threading.Thread.Sleep(3000);
            sc = null;
            InitializeComponent();            
            flashCardCollection = new ObservableCollection<FlashCardSet>();            
            listBoxFlashcards.DataContext = flashCardCollection;
            if (string.IsNullOrEmpty(Properties.Settings.Default.filePath))
                Properties.Settings.Default.filePath = defaultFilePath;
            Properties.Settings.Default.Save();
            this.KeyDown += MainWindow_KeyDown;         
        }

        void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                Button_AddFlashCard(sender, e);
            }
            if (e.Key == Key.D)
            {
                Button_DeleteFlascard(sender, e);
            }
            if (e.Key == Key.E)
            {
                Button_Edit(sender, e);
            }
            if (e.Key == Key.O)
            {
                flipcard();       
            }
            if (e.Key ==Key.N)
            {
                NextButton_Click(sender, e);
            }
            if (e.Key == Key.P)
            {
                PreviousButton_Click(sender, e);
            }
            if (e.Key == Key.H)
            {
                Button_Speak(sender, e);
            }
            if (e.Key == Key.R)
            {
                RandomButton_Click(sender, e);
            }
            if (e.Key == Key.T)
            {
                FirstButton_Click(sender, e);
            }
            if (e.Key == Key.B)
            {
                LastButton_Click(sender, e);
            }
            if (e.Key == Key.S)
            {
                SearchButton_Click(sender, e);
            }
            if (e.Key ==Key.Q)
            {
                TakeQuizButton_Click(sender, e);
            }
            if (e.Key == Key.V)
            {
                SaveListButton_Click(sender, e);
            }
            
        }   

        /// <summary>
        /// Writes each flashcard english and urdu value from collection into specified file.
        /// </summary>
        private void WriteListToCSVFile()
        {
            using (StreamWriter sw = new StreamWriter(Properties.Settings.Default.filePath))
            {
                sw.WriteLine("Urdu Word, English Word");
                foreach (FlashCardSet kvp in flashCardCollection)
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
                    flashCardCollection.Add(new FlashCardSet() { Key = FLASHCARDNUMBER + ++handle, Value = fc });
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }
     
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //If no file exsits, then create new file stored in default file path location with some default urdu/english words.
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
            FlashCardSet flashCardItem = (FlashCardSet)listBoxFlashcards.SelectedItem; 
            this.txtBlockCardData.Text = urduSide ? flashCardCollection.Single(x => x.Key == flashCardItem.Key).Value.UrduPhrase : flashCardCollection.Single(x => x.Key == flashCardItem.Key).Value.EnglishPhrase;           
            this.txtBlockCardData.Visibility = Visibility.Visible;
            this.flashCardBorder.Visibility = Visibility.Visible;        
        }

        private void Button_ShowFlashCard(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(FLASHCARDNOTSELECTED);
                e.Handled = true;            
                return;
            }            
            FlashCardSet flashCardItem = (FlashCardSet)listBoxFlashcards.SelectedItem; 
            this.txtBlockCardData.Text = flashCardCollection.Single(x => x.Key == flashCardItem.Key).Value.UrduPhrase.ToString();
            this.txtBlockCardData.Visibility = Visibility.Visible;
            this.flashCardBorder.Visibility = Visibility.Visible;           
        }

        private void Button_FlipFlashCard(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(FLASHCARDNOTSELECTED);
                e.Handled = true;
                return;
            }
            flipcard();
        }

        private void flipcard()
        {                   
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(FLASHCARDNOTSELECTED);               
                return;
            }
            FlashCardSet flashCardItem = (FlashCardSet)listBoxFlashcards.SelectedItem;
            if (urduSide)
            {
                this.txtBlockCardData.Text = flashCardCollection.Single(x => x.Key == flashCardItem.Key).Value.EnglishPhrase.ToString();
                urduSide = false;
            }
            else
            {
                this.txtBlockCardData.Text = flashCardCollection.Single(x => x.Key == flashCardItem.Key).Value.UrduPhrase.ToString();
                urduSide = true;
            }
            this.listBoxFlashcards.Focus();
        }

        private void Button_AddFlashCard(object sender, RoutedEventArgs e)
        {
            ad = new AddWindow();
            ad.Show();
            
            ad.AddFlashCardEvent += ad_AddFlashCardEvent;
        }

        void ad_AddFlashCardEvent(string urduWord, string englishWord)
        {
            if (flashCardCollection.Select(x=>x.Value.UrduPhrase.ToLower()).Contains(urduWord))
            {
                System.Windows.MessageBox.Show(urduWord + " already exists in flash card set!");        
                return;
            }
            FlashCard fc = new FlashCard();
            fc.UrduPhrase = urduWord;
            fc.EnglishPhrase = englishWord;
            flashCardCollection.Add(new FlashCardSet() { Key = FLASHCARDNUMBER + ++handle, Value = fc });
            ad.Close();   
        }

        private void Button_DeleteFlascard(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem==null)
            {
                System.Windows.Forms.MessageBox.Show(FLASHCARDNOTSELECTED);
                e.Handled = true;
                return;
            }            
            MessageBoxResult mbr=  System.Windows.MessageBox.Show("Are you sure you want to delete this item?","Delete Button",MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                FlashCardSet flashCardItem = (FlashCardSet)listBoxFlashcards.SelectedItem;
                flashCardCollection.Remove(flashCardCollection.First(x => x.Key == flashCardItem.Key));


                FlashCardSet[] dictCopy = new FlashCardSet[flashCardCollection.Count()];
                flashCardCollection.CopyTo(dictCopy, 0);
                flashCardCollection.Clear();
                handle = 0;
                foreach (FlashCardSet kvp in dictCopy)
                {
                    flashCardCollection.Add(new FlashCardSet() { Key = FLASHCARDNUMBER + ++handle, Value = new FlashCard() { UrduPhrase = kvp.Value.UrduPhrase, EnglishPhrase = kvp.Value.EnglishPhrase } });

                }
                this.txtBlockCardData.Visibility = Visibility.Hidden;
                this.flashCardBorder.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(FLASHCARDNOTSELECTED);
                e.Handled = true;
                return;
            }
            FlashCardSet kvp = listBoxFlashcards.SelectedItem as FlashCardSet;                        
            EditWindow ew = new EditWindow(kvp);
            ew.Show();

            ew.ApplyChangeEvent += ew_ApplyChangeEvent;
        }

        void ew_ApplyChangeEvent(string text1, string text2)
        {           
            FlashCardSet flashCardItem = (FlashCardSet)listBoxFlashcards.SelectedItem; 
            flashCardCollection.Single(x => x.Key == flashCardItem.Key).Value.UrduPhrase = text1;
            flashCardCollection.Single(x => x.Key == flashCardItem.Key).Value.EnglishPhrase = text2;
            this.txtBlockCardData.Text = urduSide ? flashCardCollection.Single(x => x.Key == flashCardItem.Key).Value.UrduPhrase : flashCardCollection.Single(x => x.Key == flashCardItem.Key).Value.EnglishPhrase;
        }

        private void Button_Speak(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(FLASHCARDNOTSELECTED);
                e.Handled = true;
                return;
            }

            SpeechSynthesizer speech = new SpeechSynthesizer();
            
            FlashCardSet kvp = listBoxFlashcards.SelectedItem as FlashCardSet;

            if (urduSide)
                speech.Speak(kvp.Value.UrduPhrase);
            else
                speech.Speak(kvp.Value.EnglishPhrase);
        }      

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double multiplier;
            double oldHeight;
            double oldWidth;
            oldHeight = (double)e.PreviousSize.Height;
            oldWidth = (double)e.PreviousSize.Width;
            if (oldWidth == 0)
                return;

            if (oldWidth < e.NewSize.Width || oldHeight < e.NewSize.Height)
            {

                multiplier = FindMultiplier(oldHeight, oldWidth, e.NewSize.Height, e.NewSize.Width);
                txtBlockCardData.FontSize *= multiplier;

                if (txtBlockCardData.FontSize >= 140)
                {
                    txtBlockCardData.FontSize = 140;
                    return;
                }

            }
            else if (oldHeight > e.NewSize.Height || oldWidth > e.NewSize.Width)
            {

                multiplier = FindMultiplier(oldHeight, oldWidth, e.NewSize.Height, e.NewSize.Width);
                txtBlockCardData.FontSize *= multiplier;

                if (txtBlockCardData.FontSize <= 78)
                {
                    txtBlockCardData.FontSize = 78;
                    return;
                }
            }
        }

        private double FindMultiplier(double oldHeight, double oldWidth, double newHeight, double newWidth)
        {

            double oldArea = oldHeight * oldWidth;
            double newArea = newHeight * newWidth;

            double multiplier = newArea / oldArea;

            return multiplier;

        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(FLASHCARDNOTSELECTED);
                e.Handled = true;
                return;
            }

            FlashCardSet item = listBoxFlashcards.SelectedItem as FlashCardSet;
            int currentPosition = flashCardCollection.IndexOf(item);
            if (currentPosition == flashCardCollection.Count()-1)
            {
                listBoxFlashcards.SelectedItem = flashCardCollection.ElementAt(0);
            }
            else
            {
                listBoxFlashcards.SelectedItem = flashCardCollection.ElementAt(currentPosition + 1);
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(FLASHCARDNOTSELECTED);
                e.Handled = true;
                return;
            }
            FlashCardSet item = listBoxFlashcards.SelectedItem as FlashCardSet;
            int currentPosition = flashCardCollection.IndexOf(item);
            if (currentPosition == 0)
            {
                listBoxFlashcards.SelectedItem = flashCardCollection.ElementAt(flashCardCollection.Count()-1);
            }
            else
            {
                listBoxFlashcards.SelectedItem = flashCardCollection.ElementAt(currentPosition -1);
            }
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            if (flashCardCollection != null)
            {
                 int randomnumber=0;
                 if (flashCardCollection.Count() > 0)
                 {
                     randomnumber = r.Next(flashCardCollection.Count());
                     listBoxFlashcards.SelectedItem = flashCardCollection.ElementAt(randomnumber);
                 }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No flashcards available!!");
            }
        }

       
       private void FirstButton_Click(object sender, RoutedEventArgs e)
       {
            if (flashCardCollection !=null && flashCardCollection.Count() > 0)
            {
                listBoxFlashcards.SelectedItem = flashCardCollection.ElementAt(0);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No flashcards available!");
            }
       }

       private void LastButton_Click(object sender, RoutedEventArgs e)
       {
            if (flashCardCollection != null && flashCardCollection.Count() > 0)
            {
                listBoxFlashcards.SelectedItem = flashCardCollection.ElementAt(flashCardCollection.Count() - 1);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No flashcards available!");
            }
       }

       private void TakeQuizButton_Click(object sender, RoutedEventArgs e)
       {
         
           if (flashCardCollection != null)
           {
               if (flashCardCollection.Count() >= 10)
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
                int temp = r.Next(flashCardCollection.Count());
                while (randomNumbers.Contains(temp))
                {
                    temp = r.Next(flashCardCollection.Count());
                }
                randomNumbers[i] = temp;
            }

            Dictionary<string, string> randomFlashCards = new Dictionary<string, string>();
            for (int i = 0; i < 10; i++)
            {
                FlashCard fs = flashCardCollection.ElementAt(randomNumbers[i]).Value;
                randomFlashCards.Add(fs.UrduPhrase, fs.EnglishPhrase);
            }

            return randomFlashCards;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {        
            sw = new SearchWindow();            
            sw.Show();
            sw.searchforcardevent += sw_searchforcardevent;
        }

        void sw_searchforcardevent(string englishphrase)
        {
            bool containsPhrase=flashCardCollection.Select(x => x.Value.EnglishPhrase.ToLower()).Contains(englishphrase.ToLower());
            if (containsPhrase)
            {
                urduSide = false;
                listBoxFlashcards.SelectedItem = flashCardCollection.Single(x => x.Value.EnglishPhrase.ToLower() == englishphrase.ToLower());
                sw.Close();
            }     
            else
            {
                System.Windows.MessageBox.Show("word/phrase not found!");
            }
        }

        private void SaveListButton_Click(object sender, RoutedEventArgs e)
        {
            WriteListToCSVFile();
            System.Windows.MessageBox.Show("List Saved!");
        }

        private void buttonMouseEnter(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button b = sender as System.Windows.Controls.Button;
            string buttonname = b.Content.ToString();
            switch(buttonname)
            {
                case ADDCARD:
                    b.ToolTip = "Shortcut key -> A";
                    break;
                case DELETECARD:
                    b.ToolTip = "Shortcut key -> D";
                    break;
                case EDITCARD:
                    b.ToolTip = "Shortcut key -> E";
                    break;
                case FLIPCARD:
                    b.ToolTip = "Shortcut key -> O";
                    break;
                case HEARPHRASE:
                    b.ToolTip = "Shortcut key -> H";
                    break;
                case NEXTCARD:
                    b.ToolTip = "Shortcut key -> N";
                    break;
                case PREVIOUSCARD:
                    b.ToolTip = "Shortcut key -> P";
                    break;
                case RANDOMCARD:
                    b.ToolTip = "Shortcut key -> R";
                    break;
                case FIRSTCARD:
                    b.ToolTip = "Shortcut key -> T";
                    break;
                case LASTCARD:
                    b.ToolTip = "Shortcuty key -> B";
                    break;
                case TAKEQUIZ:
                    b.ToolTip = "Shortcut key -> Q";
                    break;
                case SAVELIST:
                    b.ToolTip = "Shortcuty key -> V";
                    break;
                case SEARCH:
                    b.ToolTip = "Shortcut key -> S";
                    break;
                default:
                    break;  

            }
        }
    }

    /// <summary>
    /// Object represents a key representing Flashcard Number, and value representing Flashcard object(contains urdu and english phrase strings).
    /// </summary>
    public class FlashCardSet
    {
        public string Key
        {
            get;
            set;
        }

        public FlashCard Value
        {
            get;
            set;
        }
    }
}
