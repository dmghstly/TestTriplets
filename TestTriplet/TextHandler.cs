using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        // Словарь триплетов
        private static Dictionary<string, int> triplets;
        // Локер для синхронизации потоков, она нужна, чтобы не было проблем с доступом к словарю
        private static object locker = new object();

        // Данный метод преобразует текст, полученный из файла
        // В отдельный массив слов
        // Для этого текст разделяется по знакам пунктуации, цифрам, знакам табуляции
        // Подразумевается также, что нет слов, составленных из спец. символов
        public static string[] MakeWordsList(string text)
        {
            string[] list = text.Split(charsToSplit, StringSplitOptions.RemoveEmptyEntries);
            return list;
        }

        // Подсчёт
        private static void TripletCount(string curr)
        {
            lock (locker)
            {
                if (triplets.ContainsKey(curr))
                    triplets[curr] += 1;
                else
                    triplets.Add(curr, 1);
            }
        }

        // Отдельный поток для обработки слов
        private static void TripletFind(string help)
        {
            help = help.ToLower();
            if (help.Length == 3)
            {
                TripletCount(help);
            }
            else if (help.Length > 3)
            {
                for (int i = 0; i < help.Length - 2; i++)
                {
                    string curr = help.Substring(i, 3);
                    TripletCount(curr);
                }
            }
        }

        // Поиск триплетов
        // Поиск производится только среди слов от длины равной 3
        // ToLower() необходим для того, чтобы буквы в заглавном виде 
        // воспринимались также как и обычные
        public static Dictionary<string, int> FindTriplets(string[] words)
        {
            triplets = new Dictionary<string, int>();

            // Параллельный запуск
            ParallelLoopResult result = Parallel.ForEach(words, TripletFind);

            //foreach(string word in words)
            //{
            //    Thread thread = new Thread(TripletCount);
            //    thread.Start(word);
            //}
            return triplets;
        }
    }
}
