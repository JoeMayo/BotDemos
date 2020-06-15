using System;
using System.Collections.Generic;
using System.Linq;

namespace PigLatinBot
{
    public class PigLatin : IPigLatin
    {
        readonly char[] alphas = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
        readonly char[] vowels = "AEIOUaeiou".ToCharArray();
        const string VowelSuffix = "yay";
        const string ConsonantSuffix = "ay";

        public string FromEnglish(string text)
        {
            string[] rawWords = text.Split(' ');

            List<string> cleanedWords =
                (from word in rawWords
                 let wordChars = word.ToLower().ToCharArray()
                 let cleanChars =
                    (from ch in wordChars
                     where alphas.Contains(ch)
                     select ch)
                    .ToArray()
                 select new string(cleanChars))
                .ToList();

            List<string> cookedWords =
                (from word in cleanedWords
                 select ConvertWord(word))
                .ToList();

            return string.Join(" ", cookedWords);
        }

        string ConvertWord(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            char[] textChars = text.ToCharArray();
            char firstChar = textChars[0];

            if (vowels.Contains(firstChar))
            {
                return text + VowelSuffix;
            }
            else
            {
                int newPrefixSize = textChars.Length > 0 ? textChars.Length - 1 : 0;
                var newPrefix = new char[newPrefixSize];
                Array.Copy(textChars, 1, newPrefix, 0, newPrefixSize);

                return new string(newPrefix) + firstChar + ConsonantSuffix;
            }
        }
    }
}
