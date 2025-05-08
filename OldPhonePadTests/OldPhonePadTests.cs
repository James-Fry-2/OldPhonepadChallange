

using Microsoft.VisualStudio.TestPlatform.TestHost;
using OldPhonePadChallenge;

namespace OldPhonePadTests
{
    [TestClass]
    public class OldPhonePadTestsUnit
    {
        [TestMethod]
        public void TestSplitInputWithSpaces()
        {
            string input = "222 2 222#";
            List<Tuple<char, int>> actualOutput = OldPhonePadProgram.SplitInput(input);
            string actualOutputConverted = ConvertTupleListToString(actualOutput);;
            string expectedOutput = "(2,3), (2,1), (2,3)";
            Assert.AreEqual(expectedOutput, actualOutputConverted);
        }

        [TestMethod]
        public void TestSplitInputWithBackspace()
        {
            string input = "444*222#";
            List<Tuple<char, int>> actualOutput = OldPhonePadProgram.SplitInput(input);
            string actualOutputConverted = ConvertTupleListToString(actualOutput);;
            string expectedOutput = "(4,3), (*,1), (2,3)";
            Assert.AreEqual(expectedOutput, actualOutputConverted);
        }

        [TestMethod]
        public void TestSplitInputWithMultipleBackspaces()
        {
            string input = "777***666#";
            List<Tuple<char, int>> actualOutput = OldPhonePadProgram.SplitInput(input);
            string actualOutputConverted = ConvertTupleListToString(actualOutput);;
            string expectedOutput = "(7,3), (*,3), (6,3)";
            Assert.AreEqual(expectedOutput, actualOutputConverted);
        }

        [TestMethod]
        public void TestSplitInputWithMixedCharacters()
        {
            string input = "123 456 789 0#";
            List<Tuple<char, int>> actualOutput = OldPhonePadProgram.SplitInput(input);
            string actualOutputConverted = ConvertTupleListToString(actualOutput);;
            string expectedOutput = "(1,1), (2,1), (3,1), (4,1), (5,1), (6,1), (7,1), (8,1), (9,1), (0,1)";
            Assert.AreEqual(expectedOutput, actualOutputConverted);
        }

        [TestMethod]
        public void TestSplitInputWithSameDigitAfterSpace()
        {
            string input = "555 555#";
            List<Tuple<char, int>> actualOutput = OldPhonePadProgram.SplitInput(input);
            string actualOutputConverted = ConvertTupleListToString(actualOutput);;
            string expectedOutput = "(5,3), (5,3)";
            Assert.AreEqual(expectedOutput, actualOutputConverted);
        }

        [TestMethod]
        public void TestSplitInputWithOnlySpecialCharacters()
        {
            string input = "**3 #";
            List<Tuple<char, int>> actualOutput = OldPhonePadProgram.SplitInput(input);
            string actualOutputConverted = ConvertTupleListToString(actualOutput);;
            string expectedOutput = "(*,2), (3,1)";
            Assert.AreEqual(expectedOutput, actualOutputConverted);
        }


        [TestMethod]
        public void TestSplitInputWithManyBackSpace()
        {
            string input = "8 88777444666*664******#";
            List<Tuple<char, int>> actualOutput = OldPhonePadProgram.SplitInput(input);
            string actualOutputConverted = ConvertTupleListToString(actualOutput);;
            string expectedOutput = "(8,1), (8,2), (7,3), (4,3), (6,3), (*,1), (6,2), (4,1), (*,6)";
            Assert.AreEqual(expectedOutput, actualOutputConverted);
        }


        // Method to convert a List<Tuple<char, int>> to a string representation
        private string ConvertTupleListToString(List<Tuple<char, int>> tupleList)
        {
            return string.Join(", ", tupleList.Select(t => $"({t.Item1},{t.Item2})"));
        }
        [TestMethod]
        public void TestApplyingBackspaceBasic()
        {
            // Create a list with one digit followed by one backspace
            var input = new List<Tuple<char, int>>
                            {
                                Tuple.Create('2', 1),
                                Tuple.Create('*', 1)
                            };  

            List<Tuple<char, int>> actualOutput = input.ApplyBackspaces();
            string actualOutputConverted = ConvertTupleListToString(actualOutput);
            string expectedOutputConverted = ""; // Empty result as '2' is removed by backspace

            Assert.AreEqual(expectedOutputConverted, actualOutputConverted);
        }
        [TestMethod]
        public void TestApplyingBackspaceMultipleBackspaces()
        {
            var input = new List<Tuple<char, int>>
            {
                Tuple.Create('5', 3), 
                Tuple.Create('7', 2),  
                Tuple.Create('*', 2)   
            };

            List<Tuple<char, int>> actualOutput = input.ApplyBackspaces();
            string actualOutputConverted = ConvertTupleListToString(actualOutput);
            string expectedOutputConverted = ""; // Expect empty

            Assert.AreEqual(expectedOutputConverted, actualOutputConverted);
        }
        public void TestApplyingBackspaceTooManyBackspaces()
        {
            var input = new List<Tuple<char, int>>
            {
                Tuple.Create('5', 3),
                Tuple.Create('7', 2),
                Tuple.Create('7', 2),
                Tuple.Create('*', 4)
            };

            List<Tuple<char, int>> actualOutput = input.ApplyBackspaces();
            string actualOutputConverted = ConvertTupleListToString(actualOutput);
            string expectedOutputConverted = ""; // Expect empty

            Assert.AreEqual(expectedOutputConverted, actualOutputConverted);
        }

        [TestMethod]
        public void TestApplyingBackspaceMultipleTimesLongInput()
        {
            var input = new List<Tuple<char, int>>
            {
                Tuple.Create('1', 2),  
                Tuple.Create('8', 3),  
                Tuple.Create('9', 1),  
                Tuple.Create('*', 1),   
                Tuple.Create('1', 2),  
                Tuple.Create('8', 3),  
                Tuple.Create('9', 1),  
                Tuple.Create('*', 4),
                Tuple.Create('1', 2),

            };

            List<Tuple<char, int>> actualOutput = input.ApplyBackspaces();
            string actualOutputConverted = ConvertTupleListToString(actualOutput);
            string expectedOutputConverted = "(1,2), (1,2)"; 

            Assert.AreEqual(expectedOutputConverted, actualOutputConverted);
        }

    }

    [TestClass]
    public class OldPhonePadTestsFull
    {
        [TestMethod]
        public void TestBasicInputExamples()
        {
            // Test the examples from the requirements
            Assert.AreEqual("E", OldPhonePadProgram.OldPhonePad("33#"));
            Assert.AreEqual("B", OldPhonePadProgram.OldPhonePad("227*#"));
            Assert.AreEqual("HELLO", OldPhonePadProgram.OldPhonePad("4433555 555666#"));
            Assert.AreEqual("CAB", OldPhonePadProgram.OldPhonePad("222 2 22#"));
            Assert.AreEqual("TURING", OldPhonePadProgram.OldPhonePad("8 88777444666*664#"));
            Assert.AreEqual("HELLO TURING", OldPhonePadProgram.OldPhonePad("4433555 55566608 88777444666*664#"));

        }

        [TestMethod]
        public void TestSingleKeyPress()
        {
            // Test single key presses for various keys
            Assert.AreEqual("A", OldPhonePadProgram.OldPhonePad("2#"));
            Assert.AreEqual("D", OldPhonePadProgram.OldPhonePad("3#"));
            Assert.AreEqual("G", OldPhonePadProgram.OldPhonePad("4#"));
            Assert.AreEqual("J", OldPhonePadProgram.OldPhonePad("5#"));
            Assert.AreEqual("M", OldPhonePadProgram.OldPhonePad("6#"));
            Assert.AreEqual("P", OldPhonePadProgram.OldPhonePad("7#"));
            Assert.AreEqual("T", OldPhonePadProgram.OldPhonePad("8#"));
            Assert.AreEqual("W", OldPhonePadProgram.OldPhonePad("9#"));
            Assert.AreEqual(" ", OldPhonePadProgram.OldPhonePad("0#"));
            Assert.AreEqual("&", OldPhonePadProgram.OldPhonePad("1#"));
        }


        [TestMethod]
        public void TestMultipleBackspaces()
        {
            // Test multiple consecutive backspaces
            Assert.AreEqual("T", OldPhonePadProgram.OldPhonePad("8 88777444666*664*****#")); //  empty result
            Assert.AreEqual("H", OldPhonePadProgram.OldPhonePad("4433555 555666****#")); // HELLO with ELLO deleted
            Assert.AreEqual("", OldPhonePadProgram.OldPhonePad("4433555 555666*********#")); // HELLO with more than 5 deletions
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNoKeysInput()
        {
            // This should throw an ArgumentException because noting is entered before the send key
            OldPhonePadProgram.OldPhonePad("#");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestNoPausesBetweenKey()
        {
            // This should throw an ArgumentException because noting is entered before the send key
            OldPhonePadProgram.OldPhonePad("1111111111#");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNoKeysInput2()
        {
            // This should throw an ArgumentException because nothing is entered
            OldPhonePadProgram.OldPhonePad("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIncorrectKeysInput()
        {
            // This should throw an ArgumentException because 'Z' is not a valid keypad digit
            OldPhonePadProgram.OldPhonePad("Z#");
        }

    }
    }
