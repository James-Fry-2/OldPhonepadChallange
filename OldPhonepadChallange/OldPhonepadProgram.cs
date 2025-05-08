// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OldPhonePadChallenge
{
    public static class OldPhonePadProgram
    {
        private const char PAUSE_CHAR = ' ';
        private const char BACKSPACE_CHAR = '*';
        private const char SEND_MESSAGE_CHAR = '#';


        //Define the layout of standard keyboard
        private static readonly IReadOnlyDictionary<char, IReadOnlyList<char>> keyPadToCharacterListStandard =
        new Dictionary<char, IReadOnlyList<char>>{
                ['1'] = new [] { '&', '\'', '(' },
                ['2'] = new []  { 'A', 'B', 'C' },
                ['3'] = new [] { 'D', 'E', 'F' },
                ['4'] = new [] { 'G', 'H', 'I' },
                ['5'] = new [] { 'J', 'K', 'L' },
                ['6'] = new [] { 'M', 'N', 'O' },
                ['7'] = new [] { 'P', 'Q', 'R', 'S' },
                ['8'] = new [] { 'T', 'U', 'V' },
                ['9'] = new [] { 'W', 'X', 'Y', 'Z' },
                ['0'] = new[] { ' ' }
            };

        static void Main(string[] args)
        {
            Console.WriteLine("===============================");
            Console.WriteLine("| Old Phone Pad Text Converter |");
            Console.WriteLine("===============================");
            Console.WriteLine();


            bool continueRunning = true;

            while (continueRunning)
            {
                try
                {
                    // Get user input
                    Console.WriteLine("\nEnter your keypad sequence (end with #) or type 'exit' to quit:");
                    Console.Write("> ");
                    string input = Console.ReadLine()?.Trim();

                    // Check for exit command
                    if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(input, "quit", StringComparison.OrdinalIgnoreCase))
                    {
                        continueRunning = false;
                        continue;
                    }

                    // Handle empty input gracefully
                    if (string.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("No input provided. Please try again.");
                        continue;
                    }

                    // Process the input
                    string result = OldPhonePad(input);

                    // Display the result
                    Console.WriteLine("\nConverted text:");
                    Console.WriteLine($"\"{result}\"");
                }
                catch (ArgumentException ex)
                {
                    // Handle validation errors
                    Console.WriteLine($"\nError: {ex.Message}");
                    Console.WriteLine("Please check your input and try again.");
                }
                catch (Exception ex)
                {
                    // Handle unexpected errors
                    Console.WriteLine($"\nUnexpected error: {ex.Message}");
                    Console.WriteLine("Please try again or report this issue.");
                }
            }
        }


        /// <summary>
        /// Converts old phone keypad input to text
        /// </summary>
        /// <param name="input">String of keypad buttons pressed. Must end with '#'.</param>
        /// <returns>Decoded text message.</returns>
        /// <exception cref="ArgumentException">Invalid input format.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Too many presses for a single key.</exception>
        public static string OldPhonePad(string input)
        {
            string completedOutput = string.Empty;
            var parsedInput = new List<Tuple<char, int>>();

            if (isInputValid(input))
            {
                parsedInput = SplitInput(input.CleanInput());
                parsedInput.ApplyBackspaces();
            }

            for(int i = 0; i< parsedInput.Count; i++)
            {
                char outputLetter = ReadKeyInput(parsedInput[i]);
                completedOutput += outputLetter.ToString();
            }

            return completedOutput;
        }



        /// <summary>
        /// Splits the full input into groups of button presses - i.e. 44433 3312 => [(4,3), (3,2), (3,2), (1,1), (2,1)]
        /// </summary>
        /// <param name="input">String of the keys pressed - along with spaces for pauses </param>
        /// <returns>List of tuples containing (key pressed , number of time key pressed)  </returns>
        public static List<Tuple<char, int>> SplitInput(string input)
        {
            var parsedInput = new List<Tuple<char, int>>();
            int pressCount = 1;

            
            for (int i = 0, j = 1; j < input.Length;)
            {
                char currentChar = input[i];
                char checkNextChar = input[j];

                //handle reaching end of input
                if (checkNextChar == SEND_MESSAGE_CHAR)
                {
                    parsedInput.Add(new Tuple<char, int>(currentChar, pressCount));
                    break;
                }

                //handle pauses between inputs
                else if (checkNextChar == PAUSE_CHAR)
                {
                    //add current char and reset pointers to character after the pause
                    parsedInput.Add(new Tuple<char, int>(currentChar, pressCount));
                    i = j + 1;
                    j = i + 1;
                    pressCount = 1;

                }
                //When char doesn't change increment press count
                else if (currentChar == checkNextChar)
                {
                    pressCount++;
                    j++;
                }

                //when check ahead character is different OR we reach end of input => Add to li
                else if (currentChar != checkNextChar || input.Length == j)
                {
                    parsedInput.Add(new Tuple<char, int>(currentChar, pressCount));
                    pressCount = 1;
                    i = j;
                    j++;
                }
            } 
            return parsedInput;
        }

        /// <summary>
        /// Validates user input string for old phone keypad text processing.
        /// </summary>
        /// <param name="input">The input string to validate</param>
        /// <returns>True if the input is valid</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when:
        /// - Input doesn't end with the send character ('#')
        /// - Input contains more than one send character
        /// - Input is empty or contains only the send character
        /// </exception>
        public static bool isInputValid(string input)
        {
            //validate input
            if (!input.EndsWith(SEND_MESSAGE_CHAR))
            {
                throw new ArgumentException("Invalid input: Input must end with '#'");
            }
            //Too many send keys
            else if (input.Count( x=> x == SEND_MESSAGE_CHAR) > 1)
            {
                throw new ArgumentException($"Invalid input: Only put '{SEND_MESSAGE_CHAR}' at the end of the input");
            }
            else if (input.Length <= 1)
            {
                throw new ArgumentException("Invalid input: No keys selected");
            }
            return true;
        }



        public static string CleanInput(this string input)
        {
            string cleanedInput = string.Empty;
            //remove multiple whitespace
            cleanedInput = string.Join(PAUSE_CHAR, input.Split(PAUSE_CHAR, StringSplitOptions.RemoveEmptyEntries));
            //remove leading * and pauses 
            cleanedInput = input.TrimStart(PAUSE_CHAR , BACKSPACE_CHAR);
            return cleanedInput;
        }


        /// <summary>
        /// Processes a list of key press tuples and applies backspace operations.
        /// Each backspace character ('*') removes preceding entries based on its count.
        /// </summary>
        /// <param name="parsedInput">List of tuples containing (key pressed , number of time key pressed) </param>
        /// <returns>A modified list with backspace operations applied</returns>
        public static List<Tuple<char, int>> ApplyBackspaces( this List<Tuple<char, int>> parsedInput)
        {

            for (int i = 0; i < parsedInput.Count;)
            {
                var cuurentEntry = parsedInput[i];
                if (cuurentEntry.Item1 == '*')
                {   

                    //Identify what index the deletions go back to
                    int backspacePressCount = cuurentEntry.Item2;
                    int removalIndex = Math.Max(i  - cuurentEntry.Item2, 0);
                    int deletionCount = i - removalIndex + 1;

                    //remove all entries between lower bound and *
                    for (int j = 0; j <  deletionCount; j++)
                    {
                        parsedInput.RemoveAt(removalIndex);
                        //move pointer to index of next char after deletion
                        i = removalIndex;
                    }
                }
                else
                {
                    //No deletions move onto next item
                    i++;
                }
            }

            return parsedInput;
        }



        /// <summary>
        /// Retrieves the list of characters associated with a keypad button.
        /// 
        /// </summary>
        /// <param name="keyPressed">The keypad digit (1-9) that was pressed</param>
        /// <returns>An ordered list of characters corresponding to the keypad button</returns>
        private static IReadOnlyList<char> GetKeypadCharacters(char keyPressed)
        {
            if (!keyPadToCharacterListStandard.TryGetValue(keyPressed, out var characterList)){
                throw new ArgumentException($"Unable to parse character: {keyPressed}");
            }
            else
            {
                return characterList;
            }
        }

        /// <summary>
        /// Determines the character produced by pressing a keypad button a specific number of times.
        /// Maps old phone keypad input to characters (e.g., pressing '2' three times produces 'c').
        /// </summary>
        /// <param name="keyCombo">A tuple containing the key pressed and how many times it was pressed</param>
        /// <returns>The character corresponding to the key press combination</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the number of presses exceeds the available characters for that key</exception>
        private static char ReadKeyInput(Tuple<char, int> keyCombo) {

            char keyPressed = keyCombo.Item1;
            int numberOfTimesKeyPressed = keyCombo.Item2;
            var buttonCharactersList = GetKeypadCharacters(keyPressed);
            if (buttonCharactersList.Count >= numberOfTimesKeyPressed)
            {
                return buttonCharactersList[numberOfTimesKeyPressed-1];
            }
            else 
            {
                throw new ArgumentOutOfRangeException(
                    $"Invalid key press: Key '{keyPressed}' was pressed {numberOfTimesKeyPressed} times, " +
                    $"but it only supports {buttonCharactersList.Count} character(s) " +
                    "Use a space to pause between repeated keys."
                );
            }
        }


    }
}
