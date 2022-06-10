using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace TestTriplet
{
    // Автор - Макаров Дмитрий
    // Код создан на C#, консольная программа на .Net Core
    // Задача - счёт появлений триплетов в тексте
    // Текст достаётся из файла .txt, путь до файла является входным параметром
    // Сам код получения статистики по триплетам работает многопоточно
    // P. S. Как по мне такое приложение было бы лучше реализовывать при помощи WinForms,
    // так как там более удобная и понятная загрузка текстовых файлов
    // Важно!!! Подсчёт времени ведётся именно для обработки текста,
    // так как возможны заминки во время написания пути к файлу
    class MainProgram
    {
        // Путь до файла
        private static string path;
        // Текст из файла
        private static string mainText;
        // Массив слов
        private static string[] allWords;
        // Словарь триплетов
        private static Dictionary<string, int> triplets;
        // Таймер для обработки текста
        private static Stopwatch watch;

        // Проверка типа файла (.txt)
        private static bool CheckPath()
        {
            bool check = true;
            if (path[^4..] != ".txt")
                check = false;
            return check;
        }

        // Ввод пути до файла
        private static void InputPath()
        {
            // Проверка существования файла
            bool check;
            do
            {
                Console.Write("Введите путь до файла (.txt): ");
                path = Console.ReadLine();
                check = File.Exists(path);
                if (!check)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Такого файла не существует, либо доступа к нему нет!");
                    Console.ResetColor();
                }
            } while (!check);
        }

        // Функцияя для загрузки текста из файла
        private static void LoadTextFile()
        {
            bool check;
            do
            {
                // Проверка при считывании из файла
                InputPath();
                check = CheckPath();
                if (check)
                    mainText = File.ReadAllText(path);
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ошибка! Файл не является текстовым");
                    Console.ResetColor();
                }
            } while (!check);
        }

        // Функция запуска обработки
        private static void FindTripletsExecute()
        {
            // Измерение времени работы операции
            watch = new Stopwatch();
            watch.Start();

            // Получение массива всех слов из текста
            allWords = TextHandler.MakeWordsList(mainText);
            // Определение словаря
            triplets = new Dictionary<string, int>();
            // Подсчёт триплетов
            triplets = TextHandler.FindTriplets(allWords);
            // Сортировка массива (возможно она не входит в обработку текста)
            triplets = triplets.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            watch.Stop();

            // Вывод в консоль
            OutputTriplet();
        }

        // Вывод триплетов
        private static void OutputTriplet()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            for (int i = 0; i < 10; i++)
                Console.WriteLine($"Триплет: {triplets.ElementAt(i).Key} встречается: {triplets.ElementAt(i).Value}");
            // Подсчёт времмени
            TimeSpan ts = watch.Elapsed;

            string time = String.Format("{0:00}:{1:00}.{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds);
            Console.WriteLine("");
            Console.WriteLine("Обработка текста заняла: " + time + " мс.");
            Console.ResetColor();
        }

        // Главаная функция
        static void Main(string[] args)
        {
            LoadTextFile();

            // Создание нового потока
            Thread taskExecute = new Thread(FindTripletsExecute);
            taskExecute.Start();
        }
    }
}
