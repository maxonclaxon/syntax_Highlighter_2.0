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
            public bool inFunc;
            public int level;
            void checkType()
            {
                col = wordColor.Wcolor(text);
            }
            
            public word(string text, bool infunc,int level)
            {
                this.text = text;
                checkType();
                inFunc = infunc;
                this.level = level;
            }
            public word(string text, ConsoleColor col)
            {
                this.text = text;
                this.col = col;
            }
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            List<word> words= new List<word>();
            string pathInput = @"C:\Users\qwerty\Desktop\kurs\code.txt";
            string pathOutput = @"C:\Users\qwerty\Desktop\kurs\newtext.txt";
            bool inFunction = false;
            int lineLvl = 0;
            void splitWords(string line)
            {
                string[] lineWords = line.Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries);
                bool awaitString = false;
                bool awaitComment = false;
                List<word> stringWords = new List<word>();
                foreach(var word in lineWords)
                {
                    word newWord;
                    #region Замена цвета на красный, если находимся в строке
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
                    #endregion
                    #region Замена цвета на зеленый, если находимся в комментарии
                    if (awaitComment == false && word.StartsWith("/*"))
                    {
                        awaitComment = true;
                        words.Add(new word(word, ConsoleColor.Green));
                        continue;
                    }
                    if(awaitComment==true && word.Contains("*/"))
                    {
                        awaitComment = false;
                        words.Add(new word(word, ConsoleColor.Green));
                        continue;
                    }
                    if (awaitComment == true)
                    {
                        words.Add(new word(word, ConsoleColor.Green));
                        continue;
                    }
                    #endregion
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
                        words.Add(new word(word.Substring(0, word.IndexOf("=")),inFunction, lineLvl));
                        words.Add(new word("=",inFunction, lineLvl));
                        words.Add(new word(word.Substring(word.IndexOf("=") + 1, word.Length - word.IndexOf("=") - 1),inFunction, lineLvl));
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
                        words.Add(new word(newW.Substring(0, newW.IndexOf("+")),inFunction, lineLvl));
                        words.Add(new word("+",inFunction, lineLvl));
                        newW = newW.Remove(0, newW.IndexOf("+")+1);
                        if (newW.Contains("+")) splitWords(newW);
                        else words.Add(new word(newW.Substring(newW.IndexOf("+") + 1, newW.Length - newW.IndexOf("+") - 1),inFunction, lineLvl));

                        continue;

                    }
                    if (word.Contains(";") && word.Length > 1)
                    {
                        words.Add(new word(word.Substring(0, word.IndexOf(";")),inFunction, lineLvl));
                        words.Add(new word(word.Substring(word.IndexOf(";", 1)),inFunction, lineLvl));
                        continue;
                    }
                    
                     newWord = new word(word,inFunction, lineLvl);
                    words.Add(newWord);
                }
                words.Add(new word("\r\n",inFunction, lineLvl));
            }

            var lines = File.ReadLines(pathInput);
            foreach (var line in lines)
            {
                if (line.Contains("}"))
                {
                    inFunction = false;
                    lineLvl -= 2;
                }
                if (line.Contains("{"))
                {
                    inFunction = true;
                    lineLvl += 2;
                }
                splitWords(line);
                
            }
            StreamWriter myfile = new StreamWriter(pathOutput);
            foreach (word word in words)
            {
                Console.ForegroundColor = word.col;
                Console.Write(word.text);
                if (word.text.Contains("\r\n"))
                {
                    for(int i = 0; i < word.level; i++)
                    {
                        Console.Write(' ');
                    }
                }
                else Console.Write(' ');
                Console.ForegroundColor = ConsoleColor.White;
                
                myfile.Write(word.text);
            }
            myfile.Close();
            int j = 0;
            
        }
    }
}
