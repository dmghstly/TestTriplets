using System;
using System.Collections.Generic;

namespace TestTriplet
{
    // Данный класс обрабатывает текст для удобной работы с поиском триплетов
    // После ищет сами триплеты
    public class TextHandler
    {
        // Символы, по которым будет производится split
        private static char[] charsToSplit = { '!', '-', '\"', '\'', ':', ';', ',', '.', '?',
                                       '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                                       ' ', '\t', '\n', '\r', '*'};

        // Данный метод преобразует текст, полученный из файла
        // В отдельный массив слов
        // Для этого текст разделяется по знакам пунктуации, цифрам, знакам табуляции
        // Подразумевается также, что нет слов, составленных из спец. символов
        public static string[] MakeWordsList(string text)
        {
            string[] list = text.Split(charsToSplit, StringSplitOptions.RemoveEmptyEntries);
            return list;
        }

        // Поиск триплетов
        // Поиск производится только среди слов от длины равной 3
        // ToLower() необходим для того, чтобы буквы в заглавном виде 
        // воспринимались также как и обычные
        public static Dictionary<string, int> FindTriplets(string[] words)
        {
            Dictionary<string, int> triplets = new Dictionary<string, int>();
            foreach(string word in words)
            {
                string help = word.ToLower();
                if (help.Length == 3)
                {
                    if (triplets.ContainsKey(help))
                        triplets[help] += 1;
                    else
                        triplets.Add(help, 1);
                }
                else if (help.Length > 3)
                {
                    for (int i = 0; i < help.Length - 2; i++)
                    {
                        string curr = help.Substring(i, 3);
                        if (triplets.ContainsKey(curr))
                            triplets[curr] += 1;
                        else
                            triplets.Add(curr, 1);
                    }
                }
            }
            return triplets;
        }
    }
}
