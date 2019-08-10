using System;
using System.Collections.Generic;

namespace MessingAround
{
    class Program
    {
        //TODO: add winning condition
        //TODO: reset bodypartshash upon new game
        static void Main(string[] args)
        {
            bool quit = false;
            string wordToGuess;
            Dictionary<string, int> wordToGuessHash = new Dictionary<string, int>();
            string guessedLetter;
            string response;
            int bodyPartCount = 0;

            while (!quit)
            {
                bool continueGame = true;
                Console.WriteLine("Let's play some Hangman!");
                Console.WriteLine("Please enter the word for the other person to guess: ");
                wordToGuess = Console.ReadLine().ToLower();
                int wordLength = wordToGuess.Length;
                char[] wordToGuessArray = new char[wordLength];
                InitializeCharArray(wordToGuessArray);
                wordToGuessHash = AddWordToHash(wordToGuess, wordToGuessHash);
                while (continueGame)
                {
                    Console.WriteLine("\nWhich letter would you like to guess?");
                    guessedLetter = Console.ReadLine().ToLower();
                    if (guessedLetter == "guessed")
                    {
                        GuessedLetters();
                    } else if (guessedLetter == "available")
                    {
                        AvailableLetters();
                    }
                    else if (guessedLetter == "correct")
                    {
                        CorrectLetters(wordToGuessArray);
                    }
                    else if (guessedLetter == "bodypartsleft")
                    {
                        BodyPartsRemaining(BodyParts, BodyPartsHash);
                    }
                    else if (guessedLetter == "hangman")
                    {
                        CurrentHangman(BodyParts, BodyPartsHash);
                    }
                    else if (IsLetterAvailable(guessedLetter))
                    {
                        MarkLetterAsGuessed(guessedLetter);
                        if (IsLetterInWord(wordToGuess, guessedLetter.ToCharArray()[0]))
                        {
                            Console.WriteLine($"\nNice guess! {guessedLetter} is in the word.");
                            wordToGuessArray[wordToGuessHash[guessedLetter]] = guessedLetter.ToCharArray()[0];
                            wordLength--;
                            if (wordLength == 0)
                            {
                                Console.WriteLine($"\nCONGRATS! You won the game - the word was {wordToGuess}.");
                                continueGame = false;
                            }
                        } else
                        {
                            Console.WriteLine($"\nSorry, {guessedLetter} is NOT in the word.");
                            BodyPartsHash[BodyParts[bodyPartCount]] = true;
                            bodyPartCount++;
                            if (bodyPartCount > 6)
                            {
                                continueGame = false;
                                Console.WriteLine("\nGame Over (T_T) You've killed yet another innocent stick figure.");
                            }
                        }
                    };
                }
                Console.WriteLine("\nType \"exit\" to quit. Press any other key to play again.");
                response = Console.ReadLine();
                if (response == "exit") { quit = true; }
            }

        }

        static Dictionary<string, bool> BodyPartsHash = new Dictionary<string, bool>()
        {
            { "head", false },
            { "neck", false },
            { "torso", false },
            { "right arm", false },
            { "left arm", false },
            { "right leg", false },
            { "left leg", false }
        };

        static string[] BodyParts = new string[7]
        {
            "head",
            "neck",
            "torso",
            "right arm",
            "left arm",
            "right leg",
            "left leg"
        };

        static string[] Alphabet = new string[26] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
                                                    "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

        static Dictionary<string, bool> AvailableLettersMap = new Dictionary<string, bool>()
        {
            { "a", true }, { "b", true }, { "c", true }, { "d", true }, { "e", true }, { "f", true }, { "g", true },
            { "h", true }, { "i", true }, { "j", true }, { "k", true }, { "l", true }, { "m", true }, { "n", true },
            { "o", true }, { "p", true }, { "q", true }, { "r", true }, { "s", true }, { "t", true }, { "u", true },
            { "v", true }, { "w", true }, { "x", true }, { "y", true }, { "z", true }
        };

        static bool IsLetterAvailable(string letter)
        {
            if (!AvailableLettersMap.ContainsKey(letter.ToLower()))
            {
                Console.WriteLine($"\n{letter} isn't a valid letter ya doofus!");
                return false;
            }
            else if (!AvailableLettersMap.GetValueOrDefault(letter))
            {
                Console.WriteLine($"\n{letter} has already been guessed!");
                return false;
            }
            else
            {
                return true;
            }
        }

        static void MarkLetterAsGuessed(string letter)
        {
            if (AvailableLettersMap.ContainsKey(letter))
            {
                AvailableLettersMap[letter] = false;
            }
        }

        static void GuessedLetters()
        {
            string guessedLetters = "";
            foreach (string letter in Alphabet)
            {
                if (!AvailableLettersMap[letter])
                {
                    guessedLetters += $"{letter}, ";
                }
            }
            Console.WriteLine($"\nGuessed letters are: {guessedLetters}");
        }

        static void AvailableLetters()
        {
            string availableLetters = "";
            foreach (string letter in Alphabet)
            {
                if (AvailableLettersMap[letter])
                {
                    availableLetters += $"{letter}, ";
                }
            }
            Console.WriteLine($"\nAvailable letters are: {availableLetters}");
        }

        static void BodyPartsRemaining(string[] bodyParts, Dictionary<string, bool> bodyHash)
        {
            string bodyPartsRemaining = "";
            foreach (string bodyPart in bodyParts)
            {
                if (!bodyHash[bodyPart])
                {
                    bodyPartsRemaining += $"{bodyPart}, ";
                }
            }
            Console.WriteLine($"\nBody parts remaining are: {bodyPartsRemaining}");
        }

        static void CurrentHangman(string[] bodyParts, Dictionary<string, bool> bodyHash)
        {
            string currentHangman = "";
            foreach (string bodyPart in bodyParts)
            {
                if (bodyHash[bodyPart])
                {
                    currentHangman += $"{bodyPart}, ";
                }
            }
            if (currentHangman == "")
            {
                Console.WriteLine("\nLooking good so far! No body parts used yet.");
            } else
            {
                Console.WriteLine($"\nCurrent Hangman: {currentHangman}");
            }
            
        }

        static void CorrectLetters(char[] wordArray)
        {
            string correctLetters = "";
            foreach (char c in wordArray)
            {
                correctLetters += $"{c} ";
            }
            Console.WriteLine($"\nCorrect letters guessed so far: {correctLetters}");
        }

        static bool IsLetterInWord(string word, char letter)
        {
            foreach (char c in word)
            {
                if (c == letter)
                {
                    return true;
                }
            }
            return false;
        }

        static Dictionary<string, int> AddWordToHash(string lettersInWord, Dictionary<string, int> wordHash)
        {
            int i = 0;
            foreach (char c in lettersInWord)
            {
                wordHash.Add(c.ToString(), i);
                i++;
            }

            return wordHash;
        }

        static char[] InitializeCharArray(char[] charArray)
        {
            int i = 0;
            string empty = "_";
            foreach (char c in charArray)
            {
                charArray[i] = empty.ToCharArray()[0];
                i++;
            }

            return charArray;
        }
    }
}
