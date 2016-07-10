using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlashCardsViewer
{
   public class FlashCard
    {
        public string UrduPhrase
        {
            get;
            set;
        }

        public string EnglishPhrase
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", UrduPhrase.ToString(), EnglishPhrase.ToString());
        }
    }
}
