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
            string pathInput = @"code.txt";
            string pathOutput = @"newtext.txt";
            bool inFunction = false;
            int lineLvl = 0;
            bool awaitComment = false;
            void splitWords(string line)
            {
                string[] lineWords = line.Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries);
                bool awaitString = false;
                bool awaitWord = false;
                List<word> stringWords = new List<word>();
                foreach(var word in lineWords)
                {
                    word newWord;
                    #region Замена цвета на красный, если находимся в строке
                    if (!awaitString  && word.Contains('"'))
                    {
                        awaitString = true;
                        int indexOf2Quote=-1;
                        int indexOfQuote = word.IndexOf('"');
                        if (indexOfQuote > -1)
                        {
                            indexOf2Quote = word.IndexOf('"', indexOfQuote+1);
                        }
                        words.Add(new word(word.Substring(0, indexOfQuote), ConsoleColor.White));
                        string newW = word.Substring(indexOfQuote);
                        if (indexOf2Quote > -1)
                        {
                            indexOf2Quote = newW.IndexOf('"',1);
                            words.Add(new word(newW.Substring(0, indexOf2Quote + 1), ConsoleColor.DarkRed));
                            string newW2 = newW.Substring(indexOf2Quote+1);
                            words.Add(new word(newW2, ConsoleColor.White));
                            continue;
                        }
                        else
                        {
                            words.Add(new word(newW, ConsoleColor.DarkRed));
                            continue;
                        }
                    }
                    if (awaitString)
                    {
                        if (word.IndexOf('"') == -1)
                        {
                            words.Add(new word(word, ConsoleColor.DarkRed));
                            continue;
                        }
                        else
                        {
                            words.Add(new word(word.Substring(0, word.IndexOf('"') + 1), ConsoleColor.DarkRed));
                            string newW = word.Substring(word.IndexOf('"')+1);
                            words.Add(new word(newW, ConsoleColor.White));
                            awaitString = false;
                            continue;
                        }
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
                    

                    if (word.Contains(";") && word.Length > 1)
                    {
                        words.Add(new word(word.Substring(0, word.IndexOf(";")), inFunction, lineLvl));
                        words.Add(new word(word.Substring(word.IndexOf(";", 1)), inFunction, lineLvl));
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
                string newLine = line;
                if (newLine.Contains(')'))
                {
                    if (newLine.IndexOf(')') + 1 != newLine.Length && newLine[newLine.IndexOf(')') + 1] != ' ')
                    {
                        newLine=newLine.Insert(newLine.IndexOf(')') + 1, " ");
                    }
                    if (newLine.IndexOf(')') != 0 && newLine[newLine.IndexOf(')') - 1] != ' ')
                    {
                        newLine=newLine.Insert(newLine.IndexOf(')') - 1, " ");
                    }
                }
                if (newLine.Contains('('))
                {
                    if (newLine.IndexOf('(') + 1 != newLine.Length && newLine[newLine.IndexOf('(') + 1] != ' ')
                    {
                        newLine=newLine.Insert(newLine.IndexOf('(') + 1, " ");
                    }
                    if (newLine.IndexOf('(') != 0 && newLine[newLine.IndexOf('(') - 1] != ' ')
                    {
                        newLine=newLine.Insert(newLine.IndexOf('(') , " ");
                    }
                }
                if (newLine.Contains('='))
                {
                    if (newLine.IndexOf('=') + 1 != newLine.Length && newLine[newLine.IndexOf('=') + 1] != ' ')
                    {
                        newLine=newLine.Insert(newLine.IndexOf('=') + 1, " ");
                    }
                    if (newLine.IndexOf('=') != 0 && newLine[newLine.IndexOf('=') - 1] != ' ')
                    {
                        newLine=newLine.Insert(newLine.IndexOf('=') , " ");
                    }
                }
                splitWords(newLine);
                
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
