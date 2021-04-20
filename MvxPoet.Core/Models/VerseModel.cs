using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MvxPoet.Core.Models
{
    public class VerseModel
    {
        public string Text { get; set; }

        public static string GetPolishLetters() {return "ąćęłńóśżźĄĆĘŁŃÓŚŻŹ"; }
        public static int NumberOfSyllables(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return 0;

            int result = 0;
            
            for(int i = 0; i < line.Length; i++)
            {
                if (IsSyllableCore(line, i))
                    result++;
            }

            return result;
        }

        private static bool IsVowel(char letter)
        {
            return letter == 'a' || letter == 'e' || letter == 'i'
                || letter == 'o' || letter == 'u' || letter == 'y'
                || letter == 'ą' || letter == 'ę' || letter == 'ó';
        }

        private static bool DoesVowelsMatch(char vowel1, char vowel2)
        {
            if (vowel1 == vowel2)
                return true;
            else if (vowel1 == 'o' && vowel2 == 'ą')
                return true;
            else if (vowel1 == 'ą' && vowel2 == 'o')
                return true;
            else if (vowel1 == 'e' && vowel2 == 'ę')
                return true;
            else if (vowel1 == 'ę' && vowel2 == 'e')
                return true;
            else if (vowel1 == 'u' && vowel2 == 'ó')
                return true;
            else if (vowel1 == 'ó' && vowel2 == 'u')
                return true;
            else if (vowel1 == 'i' && vowel2 == 'y')
                return true;
            else if (vowel1 == 'y' && vowel2 == 'i')
                return true;
            else
                return false;
        }

        private static bool IsSyllableCore(string line, int index)
        {
            line = " " + line.ToLower() + " ";
            index++;

            if (IsVowel(line[index]))
            {
                if (line[index] == 'i')
                {
                    if (!IsVowel(line[index + 1]))
                        return true;
                }

                else if (line[index] == 'u')
                {
                    if (line[index - 1] != 'a')
                        return true;
                }

                else
                    return true;
            }

            return false;
        }

        public static string GetLastSyllables(string line)
        {
            if (string.IsNullOrWhiteSpace(line) || line.Length < 3)
                return "";

            int wantedSyllablesNumber = 2;
            int sumOfSyllables = 0;
            string[] words = line.Split(' ');
            int index = words.Length - 1;

            while (index >=0 && sumOfSyllables < wantedSyllablesNumber)
            {
                words[index] = DivideWordIntoSyllables(words[index], wantedSyllablesNumber - sumOfSyllables);
                sumOfSyllables += NumberOfSyllables(words[index]);
                index--;
            }

            string result = string.Join(" ", words, index + 1, words.Length - index - 1);

            return result;

            /*
            int numberOfSyllableCores = 0;
            int numberOfWantedSyllableCores = 3;
            
            int indexOfSpace = 0;

            while (index > 0 && numberOfSyllableCores < numberOfWantedSyllableCores)
            {
                if (IsSyllableCore(line, index))
                {
                    numberOfSyllableCores++;

                    if(numberOfSyllableCores > 1 && line[index + 1] != ' ')
                        line = line.Insert(index + 1, "-");
                }
                else
                {
                    if(numberOfSyllableCores == numberOfWantedSyllableCores - 1 && line[index] == ' ')
                        indexOfSpace = index;
                }

                index--;
            }

            if (indexOfSpace == 0)
                return line.Substring(index + 3);
            else
                return line.Substring(indexOfSpace + 1);

            */
        }

        private static string DivideWordIntoSyllables(string word, int numberOfWantedSyllables = 20)
        {
            if (string.IsNullOrWhiteSpace(word))
                return "";

            if (word.Length < 3)
                return word;

            int numberOfSyllables = 0;
            int index = word.Length - 1;

            while(index >= 0)
            {
                if (IsSyllableCore(word, index))
                {
                    numberOfSyllables++;

                    if(numberOfSyllables > 1)
                    {
                        word = word.Insert(index + 1, "-");

                        if (numberOfSyllables > numberOfWantedSyllables)
                            break;
                    }
                }

                index--;
            }

            return word.Substring(index + 1);
        }

        public static Rhyme DoesItRhyme(string lineEnding1, string lineEnding2)
        {
            int core11 = 0, core12 = 0, core21 = 0, core22 = 0;

            lineEnding1 = RemoveSpecialCharacters(lineEnding1);
            lineEnding2 = RemoveSpecialCharacters(lineEnding2);

            for(int i = lineEnding1.Length - 1; i >= 0; i--)
            {
                if(IsSyllableCore(lineEnding1, i))
                {
                    if (core12 == 0)
                        core12 = i;
                    else
                        core11 = i;
                }
            }

            for (int i = lineEnding2.Length - 1; i >= 0; i--)
            {
                if (IsSyllableCore(lineEnding2, i))
                {
                    if (core22 == 0)
                        core22 = i;
                    else
                        core21 = i;
                }
            }

            if (!DoesVowelsMatch(lineEnding1[core12], lineEnding2[core22]))
                return Rhyme.NONE;

            if (lineEnding1.Substring(core12) != lineEnding2.Substring(core22) && (core12 < lineEnding1.Length - 1 && core22 < lineEnding2.Length - 1))
                return Rhyme.NONE;

            if (!DoesVowelsMatch(lineEnding1[core11], lineEnding2[core21]))
                return Rhyme.MASCULINE;

            if (lineEnding1.Substring(core11, core12 - core11) != lineEnding2.Substring(core21, core22 - core21))
                return Rhyme.MASCULINE;
            else
                return Rhyme.FEMININE;
        }

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            string polishLetters = GetPolishLetters();
            foreach (char c in str)
            {
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || polishLetters.Contains(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
