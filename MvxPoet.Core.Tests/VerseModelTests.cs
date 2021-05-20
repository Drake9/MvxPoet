using NUnit.Framework;
using MvxPoet.Core.Models;

namespace TestProject1.Tests
{
    class VerseModelTests
    {
        [Test]
        [TestCase("a ja na to", 0, true)]
        [TestCase("a ja na to", 1, false)]
        [TestCase("a ja na to", 2, false)]
        [TestCase("a ja na to", 3, true)]
        [TestCase("a ja na to", 4, false)]
        [TestCase("a ja na to", 5, false)]
        [TestCase("a ja na to", 6, true)]
        [TestCase("a ja na to", 7, false)]
        [TestCase("a ja na to", 8, false)]
        [TestCase("a ja na to", 9, true)]
        [TestCase("jak na lato", 0, false)]
        [TestCase("jak na lato", 1, true)]
        [TestCase("jak na lato", 2, false)]
        [TestCase("jak na lato", 3, false)]
        [TestCase("jak na lato", 4, false)]
        [TestCase("jak na lato", 5, true)]
        [TestCase("jak na lato", 6, false)]
        [TestCase("jak na lato", 7, false)]
        [TestCase("jak na lato", 8, true)]
        [TestCase("jak na lato", 9, false)]
        [TestCase("jak na lato", 10, true)]
        public void IsSyllableCore_WhenCalled_ReturnCorrectResult(string line, int index, bool result)
        {
            Assert.That(VerseModel.IsSyllableCore(line, index) == result);
        }

        [Test]
        [TestCase("kot", "płot", Rhyme.MASCULINE)]
        [TestCase("kot", "kat", Rhyme.NONE)]
        [TestCase("lotokot", "czarny kot", Rhyme.NONE)]
        [TestCase("panie", "sanie", Rhyme.FEMININE)]
        [TestCase("popatrzyła", "mogiła", Rhyme.FEMININE)]
        [TestCase("jeden", "dwa", Rhyme.NONE)]
        [TestCase("byście", "wyście", Rhyme.FEMININE)]
        [TestCase("pomalowalibyście", "skanalizowalibyście", Rhyme.FEMININE)]
        [TestCase("a ja na to", "jak na lato", Rhyme.FEMININE)]
        public void DoesItRhyme_WhenCalled_ReturnCorrectresult(string word1, string word2, Rhyme result)
        {
            Assert.That(VerseModel.DoesItRhyme(word1, word2) == result);
        }
    }
}
