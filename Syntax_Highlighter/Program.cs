using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
namespace Syntax_Highlighter
{
    class Program
    {
        class word
        {
            public string text;
            string type;
            public ConsoleColor col;
            void checkType()
            {
                col = wordColor.Wcolor(text);
            }
            
            public word(string text)
            {
                this.text = text;
                checkType();
            }
            public word(string text, ConsoleColor col)
            {
                this.text = text;
                this.col = col;
            }
        }
        static void Main(string[] args)
        {
            
            List<word> words= new List<word>();
            Console.OutputEncoding = Encoding.ASCII;
            string pathInput = @"C:\Users\qwerty\Desktop\kurs\code.txt";
            string pathOutput = @"C:\Users\qwerty\Desktop\kurs\newtext.txt";
            
            void splitWords(string line)
            {
                string[] lineWords = line.Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries);
                bool awaitString = false;
                bool awaitComment = false;
                List<word> stringWords = new List<word>();
                foreach(var word in lineWords)
                {
                    word newWord;
                    if (awaitString == false && word.StartsWith('"'))
                    {
                        awaitString = true;
                        words.Add(new word(word, ConsoleColor.DarkRed));
                        continue;
                    }
                    
                    if (awaitString == true && word.Contains('"'))
                    {
                        awaitString = false;
                        words.Add(new word(word, ConsoleColor.DarkRed));
                        continue;
                    }
                    if (awaitString == true)
                    {
                        words.Add(new word(word, ConsoleColor.DarkRed));
                        continue;
                    }
                    if (word.Contains("=") )
                    {
                        if (word.IndexOf("=")!=word.Length-1)
                        {
                            if (word[word.IndexOf("=") + 1] != ' ') word.Insert(word.IndexOf("=") + 1, " ");
                        }
                        if (word.IndexOf("=") != 0)
                        {
                            if (word[word.IndexOf("=") - 1] != ' ') word.Insert(word.IndexOf("=") - 1, " ");
                        }
                        words.Add(new word(word.Substring(0, word.IndexOf("="))));
                        words.Add(new word("="));
                        words.Add(new word(word.Substring(word.IndexOf("=") + 1, word.Length - word.IndexOf("=") - 1)));
                        continue;

                    }
                    if (word.Contains("+") && word.Length > 2)
                    {
                        var newW = word;
                        if (newW.IndexOf("+") != 0)
                        {
                            if ( newW[newW.IndexOf("+")- 1] != ' ') newW = newW.Insert(newW.IndexOf("+"), " ");
                        }
                        if (newW.IndexOf("+") != newW.Length - 1)
                        {
                            if (newW[newW.IndexOf("+") + 1] != ' ') newW = newW.Insert(newW.IndexOf("+")+1, " ");
                        }
                        words.Add(new word(newW.Substring(0, newW.IndexOf("+"))));
                        words.Add(new word("+"));
                        newW = newW.Remove(0, newW.IndexOf("+")+1);
                        if (newW.Contains("+")) splitWords(newW);
                        else words.Add(new word(newW.Substring(newW.IndexOf("+") + 1, newW.Length - newW.IndexOf("+") - 1)));

                        continue;

                    }
                    if (word.Contains(";") && word.Length > 1)
                    {
                        words.Add(new word(word.Substring(0, word.IndexOf(";"))));
                        words.Add(new word(word.Substring(word.IndexOf(";", 1))));
                        continue;
                    }
                    
                     newWord = new word(word);
                    words.Add(newWord);
                }
                words.Add(new word("\r\n"));
            }

            var lines = File.ReadLines(pathInput);
            foreach (var line in lines)
            {
                splitWords(line);
            }
            StreamWriter myfile = new StreamWriter(pathOutput);
            foreach (word word in words)
            {
                Console.ForegroundColor = word.col;
                Console.Write(word.text);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(' ');
                myfile.Write(word.text);
            }
            int j = 0;
            
        }
    }
}
