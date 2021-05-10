using System.Collections.Generic;
using MvxPoet.Core.Properties;
using Newtonsoft.Json;

namespace MvxPoet.Core.Models
{
    class Dictionary
    {
        public Dictionary<string, string> RhymesDictionary { get; set; }

        public Dictionary()
        {
            RhymesDictionary = new Dictionary<string, string>();
        }

        public bool LoadDictionaryFromFile()
        {
            try
            {
                string json = Resources.rhymes_dictionary;

                RhymesDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string SuggestRhymes(string givenWord)
        {
            foreach (KeyValuePair<string, string> entry in RhymesDictionary)
            {

                if (VerseModel.DoesItRhyme(givenWord, entry.Key) != Rhyme.NONE)
                {
                    return entry.Value;
                }
            }

            return "Sorry. No rhymes found.";
        }
    }
}
