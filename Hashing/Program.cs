using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

// Created: Daniel Swain
// 
// My solution to the Diploma 2016 semester 2 assignment where we needed to use a hashing/cipher to hash/encrypt data and then deencrypt it
// This is a simple console application to enable the user to enter their strings and add them to a datastructure after hashing/encryption.
namespace Hashing
{
    class Program
    {
        // Welcome text and program options.
        static string welcomeMessage = "Welcome to my Solution to the Hash/Encryption Assignment, implemented in C#. Please choose the operation you want to complete.\n";
        static string[] options = { "Set the secret key", "Hash/Encrypt a string", "Print the hashed/encrypted strings", "Print the decrypted/unhashed strings", "Exit the program" };
        static bool repeat = true;
        static List<string> hashedStrings = new List<string>();
        static string secretKey = "";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Greet the user and display the program options for them until the user specifically chooses to exit the program.
            Console.WriteLine(welcomeMessage);
            // Loop the options forever until the user chooses to exit.
            while (repeat)
            {
                // Print the options for the user.
                printOptions();
                // Get and handle the input from the user.
                getInput();
            }
        }

        // Print the options for the user to pick from.
        static void printOptions()
        {
            int count = 1;
            foreach (string option in options)
            {
                Console.WriteLine("{0}: {1}", count, option);
                count++;
            }
        }

        // Get the input and handle the actions for the program.
        static void getInput()
        {
            // Print out helper text to prompt the user for the input.
            Console.Write("\nChoice: ");
            string inputString = Console.ReadLine();
            // Print out a blank line for formatting.
            Console.WriteLine();

            // Parse the inputString to an int and handle the input.
            switch (inputString)
            {
                case "1":
                    // User wishes to set the secret key
                    setSecretKey();
                    break;

                case "2":
                    // User wishes to hash/encrypt a string.
                    hashString();
                    break;

                case "3":
                    // User wishes to view the hashed/encrypted strings.
                    printStrings(true);
                    break;

                case "4":
                    // User wishes to view the unhashed/decrypted strings.
                    printStrings(false);
                    break;

                case "5":
                    // User wishes to exit the program, show the choice and then exit the program by setting repeat to false.
                    exitProgram();
                    break;

                default:
                    // Unable to identify the users choice.
                    Console.WriteLine("Please enter a number representing the option you wish to complete.");
                    break;
            }
        }

        // Set the secret key that will used in the XOR method.
        static void setSecretKey()
        {
            // Ask the user to enter in a string for the XOR method.
            Console.WriteLine("Please enter a random string of numbers (i.e. 123456789) that will be used to encrypt/hash your data.\nNote, don't change this after you have added data as you won't be able to decrypt/unhash your data again.\n");
            Console.Write("Secret Key: ");
            secretKey = Console.ReadLine();
            Console.WriteLine();
            // Clear any previously hashed strings.
            hashedStrings.Clear();
        }

        // Hash/encrypt the string or string list the user enters.
        static void hashString()
        {
            // Check if the secret key is empty, if so then warn and return early.
            if (secretKey == "")
            {
                Console.WriteLine("You need a secret key before you can hash/encrypt your string. Please choose option 1 and try again.\n");
                return;
            }

            // Encrypt/hash the string the user enters.
            Console.WriteLine("Please enter the words you want encrypted/hashed. You can enter a list of words by space separating them (i.e. enter \"John Charlie Michael\" without quotation), or just enter one word.\n");
            Console.Write("Words to hash/encrypt: ");
            // Get the words and split them into a list.
            string words = Console.ReadLine();
            List<string> wordList = new List<string> (words.Split());

            // Hash/Encrypt these words using the XOR method below.
            foreach(string word in wordList)
            {
                hashedStrings.Add(xor_encrypt(word, secretKey));
            }

            // Show the user the strings they entered as a list.
            Console.WriteLine("\nThe array you entered was: [" + String.Join(", ", wordList) + "]");

            // Let them know how to view the encrypted strings.
            Console.WriteLine("\nTo view the hashed/encrypted string select option 3, or select 4 to view the decrypted/unhashed strings.\n");
        }

        // Print the hashed strings either hashed or unhashed/decrypted depending on what the user chose.
        static void printStrings(bool hashed)
        {
            if (hashedStrings.Count <= 0)
            {
                Console.WriteLine("You haven't hashed/encrypted any strings yet, please choose options 1 or 2 and try again.\n");
                return;
            }

            if (hashed)
            {
                // Print the hashed values.
                Console.WriteLine("The hashed/encrypted strings are:\n(note they might not display very well in the console if you used letters in your secret key):\n");
                Console.WriteLine("[ " + String.Join(", ", hashedStrings) + " ]\n");
            }
            else
            {
                // Print the unhashed values. These are retreived by running xor_encrypt again on the hashed strings.
                Console.WriteLine("The unhashed/decrypted strings are:\n");
                List<string> unhashedStrings = new List<string>();
                foreach(string hashedString in hashedStrings)
                {
                    unhashedStrings.Add(xor_encrypt(hashedString, secretKey));
                }
                Console.WriteLine("[" + String.Join(", ", unhashedStrings) + "]\n");
            }

        }

        // The user has chosen to exit the program, handle this action in this method.
        static void exitProgram()
        {
            Console.WriteLine("Exiting...");
            // Use a delay to allow the application to notify the user.
            Thread.Sleep(1000);
            // Set the repeat variable to false so the while loop running the program will exit.
            repeat = false;
        }

        // Encrypt a string using the secret using the XOR method.
        // This can be run on the encrypted word using the same secret to decrypt to the original string.
        private static string xor_encrypt(string inputString, string secret)
        {
            // String builder used to build the encrypted string.
            var result = new StringBuilder();

            for (int c = 0; c < inputString.Length; c++)
            {
                // Get the next character from the string as we'll encrypt/hash it.
                char character = inputString[c];
                // Convert it into an unsigned int.
                uint charCode = character;
                // Get the next key position from our secret. Using modulus to ensure we wrap around the secret.
                int keyPosition = c % secret.Length;
                // Get the character from that key position in our secret string.
                char keyChar = secret[keyPosition];
                // Convert that to an unsigned int.
                uint keyCode = keyChar;
                // Use the XOR operator between the inputString charCode and secret keyCode.
                uint combinedCode = charCode ^ keyCode;
                // Convert the result back to a character
                char combinedChar = (char) combinedCode;
                // Add to our final string builder.
                result.Append(combinedChar);
            }
            // Return the result as a string.
            return result.ToString();
        }
    }
}
