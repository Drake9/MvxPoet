using System.Collections.ObjectModel;
using MvvmCross.ViewModels;
using MvvmCross.Commands;
using MvxPoet.Core.Models;
using System.Collections.Generic;

namespace MvxPoet.Core.ViewModels
{
    public class WritePoemViewModel : MvxViewModel
    {
        public WritePoemViewModel()
        {
            SuggestRhymesCommand = new MvxCommand(SuggestRhymes);

            if (!_dictionary.LoadDictionaryFromFile())
            {
                _canUseDictionary = false;
                _rhymeSuggestions = "Nie udało się załadowac słownika. Przepraszamy.";
            }
        }

        private Dictionary _dictionary = new Dictionary();

        private bool _canUseDictionary = true;
        private string _title;
        private string _text;
        private string _givenWord;
        private string _rhymeSuggestions;

        private ObservableCollection<string> _linesEndings = new ObservableCollection<string>();
        private ObservableCollection<int?> _numbersOfSyllables = new ObservableCollection<int?>();
        private ObservableCollection<char> _rhymes = new ObservableCollection<char>();

        public IMvxCommand SuggestRhymesCommand { get; set; }       

        public string Title
        {
            get { return _title; }
            set { 
                SetProperty(ref _title, value);
                RaisePropertyChanged(() => CanWriteToFile);
            }
        }

        public string Text
        {
            get { return _text; }
            set { 
                SetProperty(ref _text, value);
                RaisePropertyChanged(() => TextLenght);

                ClearTables();
                SetLinesEndingsAndNumbersOfSyllables();
                SetRhymes();

                RaisePropertyChanged(() => LinesEndings);
                RaisePropertyChanged(() => NumbersOfSyllables);
                RaisePropertyChanged(() => Rhymes);
                RaisePropertyChanged(() => CanWriteToFile);
            }
        }

        public int TextLenght => Text.Length;

        public bool CanWriteToFile => Title?.Length > 0 && Text?.Length > 0;
       
        public ObservableCollection<string> LinesEndings
        {
            get { return _linesEndings; }
        }

        private void SetLinesEndingsAndNumbersOfSyllables()
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                var lines = Text.Split('\n');

                for (int i = 0; i < lines.Length; i++)
                {
                    _linesEndings.Add(VerseModel.GetLastSyllables(lines[i]));
                    _numbersOfSyllables.Add(VerseModel.NumberOfSyllables(lines[i]));
                }
            }
        }

        private void ClearTables()
        {
            _linesEndings.Clear();
            _numbersOfSyllables.Clear();
            _rhymes.Clear();
        }      

        public ObservableCollection<int?> NumbersOfSyllables
        {
            get { return _numbersOfSyllables; }
        }

        public ObservableCollection<char> Rhymes
        {
            get { return _rhymes; }
        }

        private void SetRhymes()
        {
            if (LinesEndings.Count == 0)
                return;

            char mark = 'A';

            int index = 0;

            List<int> observedLines = new List<int>();

            while (index < LinesEndings.Count)
            {
                if (_linesEndings[index] == "")
                {
                    observedLines.Clear();
                    _rhymes.Add(' ');
                    mark = 'A';
                    index++;
                    continue;
                }

                _rhymes.Add('?');

                foreach (int observedIndex in observedLines)
                {
                    if (VerseModel.DoesItRhyme(_linesEndings[index], _linesEndings[observedIndex]) == Rhyme.FEMININE)
                    {
                        if (_rhymes[observedIndex] != '?')
                        {
                            _rhymes[index] = _rhymes[observedIndex];
                        }
                        else
                        {
                            _rhymes[observedIndex] = mark;
                            _rhymes[index] = mark;
                            mark += (char)1;
                        }
                    }
                    else if (VerseModel.DoesItRhyme(_linesEndings[index], _linesEndings[observedIndex]) == Rhyme.MASCULINE)
                    {
                        if (_rhymes[observedIndex] != '?')
                        {
                            _rhymes[index] = _rhymes[observedIndex];
                        }
                        else
                        {
                            _rhymes[observedIndex] = char.ToLower(mark);
                            _rhymes[index] = char.ToLower(mark);
                            mark += (char)1;
                        }
                    }
                }

                observedLines.Add(index);
                index++;
            }
        }

        public string GivenWord
        {
            get { return _givenWord; }
            set
            {
                SetProperty(ref _givenWord, value);
                RaisePropertyChanged(() => CanSuggestRhymes);
            }
        }

        public string RhymeSuggestions
        {
            get { return _rhymeSuggestions; }
        }

        public bool CanSuggestRhymes => (!string.IsNullOrWhiteSpace(_givenWord) && GivenWord.Length > 2 && _canUseDictionary);

        private void SuggestRhymes()
        {
            _rhymeSuggestions = _dictionary.SuggestRhymes(_givenWord);
            RaisePropertyChanged(() => RhymeSuggestions);
        }
    }
}
