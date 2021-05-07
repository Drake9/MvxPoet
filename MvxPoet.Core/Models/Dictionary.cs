using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MvxPoet.Core.Models
{
    class Dictionary
    {
        private string _filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\odm1.txt";

        public List<string> KnownWords { get; set; }

        public Dictionary()
        {
            KnownWords = new List<string>();
        }

        public bool LoadKnownWords()
        {
            try
            {
                var content = File.ReadAllText(_filePath);
                var lines = content.Split('\n');

                foreach (string line in lines)
                {
                    int lastCut = 0;

                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == ',')
                        {
                            KnownWords.Add(line.Substring(lastCut, i - lastCut));

                            lastCut += 2;
                        }
                    }
                }

                return true;

            } catch
            {
                return false;
            }
        }

        public string SuggestRhymes(string word, Rhyme rhyme)
        {
            string result = "";

            foreach(string knownword in KnownWords)
            {
                if (VerseModel.DoesItRhyme(word, knownword) == rhyme)
                    result += knownword;
            }

            return result;
        }
    }
}
