using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            const byte MAX_DIGITS = 3;
            Random rnd = new Random();
            byte[] secretNumber = new byte[MAX_DIGITS];
            byte[] userGuess = new byte[MAX_DIGITS];
            byte triesCount = 0;
            bool found = false;
            bool lost = false;
            bool retry = false;

            InitConsole();

            do
            {
                secretNumber = GenSecretNumber(rnd, MAX_DIGITS);
                Reset(ref triesCount, ref found, ref lost);
                
                do
                {
                    triesCount++;
                    userGuess = GetUserGuess(MAX_DIGITS);
                    lost = CheckTries(triesCount);
                    found = CheckUserGuess(userGuess, secretNumber);

                    if (found)
                    {
                        Console.WriteLine($"YOU FOUND THE SECRET NUMBER ({triesCount}) {(triesCount == 1 ? "try" : "tries")}\n");
                    }
                    else if (lost)
                    {
                        Console.Write($"MAX TRIES ({triesCount}) REACHED, THE SECRET NUMBER WAS: \t");
                        DisplayNumber(secretNumber);
                    }

                    if (found || lost)
                    {
                        retry = GetUserChoice();
                    }
                } while (!found && !lost);

            } while (retry);
        }

        static void InitConsole()
        {
            Console.Title = "FIND THE SECRET NUMBER";
            Console.Clear();
            Console.BufferWidth = 100;
            Console.BufferHeight = 30;
            Console.WindowWidth = 100;
            Console.WindowHeight = 30;
        }

        static void Reset(ref byte triesCount, ref bool found, ref bool lost)
        {
            triesCount = 0;
            found = false;
            lost = false;
            ConsoleColor previousTextColor = Console.ForegroundColor;

            Console.Clear();
            Console.WriteLine("**********************************");
            Console.WriteLine("***** FIND THE SECRET NUMBER *****");
            Console.WriteLine("**********************************\n");

            Console.WriteLine("Try to find the secret number:\nThe secret number is composed with 3 unique digits.\n");
            Console.Write("If a digit is highlighted in ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("GREEN ");
            Console.ForegroundColor = previousTextColor;
            Console.WriteLine("=> The secret number has that digit in the exact same position.");
            Console.Write("If a digit is highlighted in ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("CYAN ");
            Console.ForegroundColor = previousTextColor;
            Console.WriteLine("=> The secret number has that digit but in a diffrent position.");
            Console.Write("If a digit is highlighted in ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("GRAY ");
            Console.ForegroundColor = previousTextColor;
            Console.WriteLine("=> The secret number does not have that digit.");
            Console.WriteLine();
        }

        static byte[] GenSecretNumber(Random rnd, byte maxDigits)
        {
            byte[] secretNumber = new byte[maxDigits];
            for (int i = 0; i < maxDigits; i++)
            {
                bool isUnique = false;
                do
                {
                    byte n = (byte)rnd.Next(0, 10);
                    if (Array.IndexOf(secretNumber, n) < 0)
                    {
                        isUnique = true;
                        secretNumber[i] = (n == 0 && i == 0) ? (byte)(n + 1) : n;
                    }

                } while (!isUnique);
            }
            return secretNumber;
        }

        static byte[] GetUserGuess(byte maxDigits)
        {
            bool isValidInput = false;
            byte[] userGuess = new byte[maxDigits];
            do
            {
                Console.Write($"Guess a number of ({maxDigits}) unique digits:\t");
                string input = Console.ReadLine();
                if (int.TryParse(input, out _) && input.Length == maxDigits)
                {
                    userGuess = StringToNumber(input);
                    isValidInput = IsNumberWithUniqueDigits(userGuess);
                } 
                if(!isValidInput)
                {
                    Console.WriteLine("Invalid guess.");
                }
            } while (!isValidInput);
            return userGuess;
        }

        static byte[] StringToNumber(string str)
        {
            byte[] number = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                number[i] = byte.Parse(str[i].ToString());
            }
            return number;
        }

        static bool IsNumberWithUniqueDigits(byte[] number)
        {
            bool hasUniqueDigits = true;
            for (int i = 0; i < number.Length; i++)
            {
                if(Array.IndexOf(number, number[i]) != Array.LastIndexOf(number, number[i]))
                {
                    hasUniqueDigits = false;
                    break;
                }
            }
            return hasUniqueDigits;
        }

        static bool CheckUserGuess(byte[] userGuess, byte[] secretNumber)
        {
            byte exactMatch = 0;
            ConsoleColor previousTextColor = Console.ForegroundColor;

            for (int i = 0; i < userGuess.Length; i++)
            {
                byte digit = userGuess[i];
                if (Array.IndexOf(userGuess, digit) == Array.IndexOf(secretNumber, digit))
                {
                    exactMatch++;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(digit + " ");
                }
                else if (Array.IndexOf(secretNumber, digit) >= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(digit + " ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(digit + " ");
                }
            }
            Console.ForegroundColor = previousTextColor;
            Console.WriteLine();

            return exactMatch == userGuess.Length;
        }

        static bool CheckTries(byte tries)
        {
            return tries >= byte.MaxValue;
        }

        static bool GetUserChoice()
        {
            bool isInvalidChoice = true;
            bool retry = false;
            do
            {
                Console.WriteLine("Press [0] to Quit, [1] to Retry: \t");
                string userInput = Console.ReadLine();
                if (userInput == "1" || userInput == "0")
                {
                    isInvalidChoice = false;
                    retry = userInput == "1" ? true : false;
                }
                else
                {
                    Console.WriteLine("Invalid Choice...");
                }
            } while (isInvalidChoice);

            return retry;
        }

        static void DisplayNumber(byte[] number)
        {
            for (int i = 0; i < number.Length; i++)
            {
                Console.Write(number[i] + " ");
            }
            Console.WriteLine();
        }
    }
}
