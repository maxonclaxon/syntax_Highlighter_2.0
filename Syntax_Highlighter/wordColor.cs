﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Syntax_Highlighter
{
    class wordColor
    {
        static List<string> blue = new List<string>() {"void", "char", "signed", "unsigned", "short", "int", "long", "float", "double", "bool", "string", "if", "true", "false", "struct" };
        static List<string> darkGrey = new List<string>() { "#include" };

        static public ConsoleColor Wcolor(string word)
        {
            if (word == "") return ConsoleColor.White;
            if (blue.Contains(word)) return ConsoleColor.Blue;
            if (word == "#include") return ConsoleColor.DarkGray;
            if (word[0] == '<' && word[word.Length - 1] == '>') return ConsoleColor.Yellow;
            if (word[0] == '"' && word[word.Length - 1] == '"') return ConsoleColor.DarkRed;
            if (int.TryParse(word, out int outT2)) return ConsoleColor.Green;
            if (word.Contains('.'))
            {
                List<string> subWords = new List<string>();
                int count;
                foreach(var newWord in word.Split('.'))
                {
                    subWords.Add(newWord);
                }
                if (int.TryParse(subWords[0],out int outT)&&int.TryParse(subWords[1],out int outT1))
                {
                    return ConsoleColor.Green;
                }
            }
            if (word.StartsWith("/*")&& word.EndsWith("*/")) return ConsoleColor.Green;
            return ConsoleColor.White;
        }
    }
}
