using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using System.Xml.Serialization;

namespace HomeworkStore_Mini_Project
{
    internal class Program
    {

        public struct Homework
        {
            public string Subject;
            public string Description;
            public DateTime DueDate;
            public bool Completed;
        }

        static void Main(string[] args)
        {
            /*------------------------------------------------------------------*/
            /* Variables                                                        */
            /*------------------------------------------------------------------*/

            string title = ("===================================================\n" +
                            "Homework \n"                                           +
                            "===================================================\n");

            string menu = ("1. View Homework\n"     +
                           "2. Add Homework\n"      +
                           "3. Complete Homework\n" +
                           "4. Exit\n\n"              );

            List<Homework> HomeworkList = new List<Homework>();

            HomeworkList = LoadHomeworks("HomeworkData.bin");

            /*------------------------------------------------------------------*/

            /*------------------------------------------------------------------*/
            /* Main Program                                                     */
            /*------------------------------------------------------------------*/

            int menuChoice = 0;

            while (menuChoice != 4)
            {

                Console.Clear();
                menuChoice = (int)DeclareInput(title + menu, "System.Int32", "Enter a Number: ");

                menuChoice = RangeCheck(menuChoice, 1, 4);

                switch (menuChoice)
                {
                    case 1: ClearLines(6);
                        DisplayHomeworks(HomeworkList, true);

                        Console.ReadKey();
                        break;

                    case 2: ClearLines(6);


                        Homework newHomework = new Homework(); // Creating a new homework to be made by the user

                        newHomework.Subject = (string)DeclareInput("Enter Subject: ", "System.String", "");
                        ClearLines(1);
                        newHomework.Description = (string)DeclareInput("Enter Description: ", "System.String", "");
                        ClearLines(1);

                        newHomework.DueDate = (DateTime)DeclareInput("Enter Due Date (dd/mm/yyyy): ", "System.DateTime", "Enter as dd/mm/yyyy: ");

                        while (newHomework.DueDate.CompareTo(DateTime.Today) < 0)
                        {
                            ClearLines(1);
                            newHomework.DueDate = (DateTime)DeclareInput("Enter Due Date in the Future (dd/mm/yyyy): ", "System.DateTime", "Enter as dd/mm/yyyy: ");
                        }
                        
                        newHomework.Completed = false;

                        ClearLines(1);

                        if (newHomework.DueDate.CompareTo(DateTime.Today) < 0) { HomeworkList = HomeworkList.Append(newHomework).ToList(); }

                        else { HomeworkList = HomeworkList.Prepend(newHomework).ToList(); }

                        break;

                    case 3: ClearLines(6);

                        if (HomeworkList.Count == 0)
                        {
                            Console.Write("There are no homeworks stored. Press any key to continue: ");
                            Console.ReadKey();
                            break;
                        }

                        DisplayHomeworks(HomeworkList, true);

                        int choice = (int)DeclareInput("Which Homework Would You like to mark as completed? ", "System.Int32", "Enter a Number");

                        choice = RangeCheck(choice, 1, HomeworkList.Count);

                        Homework replaceHomework = new Homework();

                        replaceHomework.Subject = HomeworkList[choice - 1].Subject;
                        replaceHomework.Description = HomeworkList[choice - 1].Description;
                        replaceHomework.DueDate = HomeworkList[choice - 1].DueDate;
                        replaceHomework.Completed = true;

                        HomeworkList[choice-1] = replaceHomework;

                        Console.ReadKey();

                        break;

                    case 4: break; /* Exit */
                }

            }

            SaveHomeworks(HomeworkList, "HomeworkData.bin");

            /*------------------------------------------------------------------*/
        }

        /* File Interactions */

        static void SaveHomeworks(List<Homework> HomeworkList, string FileName)
        {
            FileStream MyFile = new FileStream(FileName, FileMode.Truncate);

            BinaryWriter MyFileWrite = new BinaryWriter(MyFile); // Opens a binary reader after deleting contents of the file

            for (int homeworkCount = 0; homeworkCount < HomeworkList.Count; homeworkCount++) // Loops through homework list
            {
                Homework homework = HomeworkList[homeworkCount];

                MyFileWrite.Write(homework.Subject);
                MyFileWrite.Write(homework.Description);
                MyFileWrite.Write(Convert.ToString(homework.DueDate));
                MyFileWrite.Write(homework.Completed);

            } // Adds HomeworkList to file

            MyFileWrite.Close();
            MyFile.Close();
        }

        static List<Homework> LoadHomeworks(string FileName)
        {
            List<Homework> HomeworkList = new List<Homework>();

            FileStream MyFile;

            try 
            { 
                MyFile = new FileStream("HomeworkData.bin", FileMode.Open); 
            }
            catch
            {
                MyFile = new FileStream("HomeworkData.bin", FileMode.Create);
                MyFile.Close();
            }

            MyFile.Close();
            // Checking if HomewrokData exists and if not creating it.

            MyFile = new FileStream(FileName, FileMode.Open);

            BinaryReader MyFileRead = new BinaryReader(MyFile); // Opening a Binary Reader

            while (MyFile.Position < MyFile.Length) // Loop until end of file
            {
                Homework homework = new Homework();

                homework.Subject = MyFileRead.ReadString();
                homework.Description = MyFileRead.ReadString();
                homework.DueDate = Convert.ToDateTime(MyFileRead.ReadString());
                homework.Completed = MyFileRead.ReadBoolean();

                if (homework.DueDate.CompareTo(DateTime.Today) < 0) { HomeworkList = HomeworkList.Append(homework).ToList(); }

                else { HomeworkList = HomeworkList.Prepend(homework).ToList(); }
            }

            MyFileRead.Close();
            MyFile.Close();

            return HomeworkList;
        }

        static string DisplayHomeworks(List<Homework> HomeworkList, bool Write)
        {
            string homeworkString = "";

            for (int homeworkCount = 1; homeworkCount <= HomeworkList.Count; homeworkCount++) // Loops through homework list
            {
                Homework homework = HomeworkList[homeworkCount - 1];

                if (homework.DueDate.CompareTo(DateTime.Today) >= 0)
                {

                    Console.ForegroundColor = homework.DueDate.CompareTo(DateTime.Today.AddDays(3)) < 0 ? ConsoleColor.Red : Console.ForegroundColor;

                    Console.ForegroundColor = homework.Completed ? ConsoleColor.Green : Console.ForegroundColor;

                    string homeworkFormat = String.Format(($"{homeworkCount}. {homework.Subject}: " +
                                                           $"{(homework.Completed ? "" : "Not")} Completed: " +
                                                           $"{homework.DueDate.ToString("dd/MM/yyyy")}\n" +

                                                           $"{homework.Description}\n"), homework, homeworkCount);

                    homeworkString += homeworkFormat; // Adds to string

                    if (Write)
                    {
                        Console.WriteLine(homeworkFormat); // If write, write
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            return homeworkString;
        }

        /*-------------------------------------------------------------------------------*/

        /* Improvements to interface */

        static void ClearLines(int numLines) // Method for clearing a certain number of lines.
        {
            for (int linesCleared = 0; linesCleared < numLines; linesCleared++) // Looping for number of lines specified.
            {
                Console.SetCursorPosition(0, Console.CursorTop);                                                          // Deletes a single line.
                Console.SetCursorPosition(0, Console.CursorTop - (Console.WindowWidth >= Console.BufferWidth ? 1 : 0));
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop - (Console.WindowWidth >= Console.BufferWidth ? 1 : 0));
            }
        }

        /*-------------------------------------------------------------------------------*/

        /* Validation */

        static int RangeCheck(int num, int Min, int Max)
        {
            bool valid = true;

            do // Loops until valid
            {
                valid = true;
                if (num < Min || num > Max) // If not within range (inclusive)
                {
                    ClearLines(1);
                    Console.Write("Enter a valid number:  ");
                    num = (int)Validate(Console.ReadLine(), "System.Int32", "Enter a number: "); // Asks for a new input
                    valid = false;
                }

            } while (valid == false);

            return num;
        }

        static object Validate(object input, string targetType, string errorMessage) // Validation for a variable of any type. Parameter are input of any type, target type and error message.
        {
            Type type = Type.GetType(targetType); // Gets the target type from the given string.

            bool valid = false; // Assumes false.

            while (valid == false) // Loop while false.
            {
                try // tries to convert the input to the target type.
                {
                    input = Convert.ChangeType(input, type);
                    valid = true; // Ends loop if possible.
                }
                catch // Asks for a new input if not possible.
                {
                    ClearLines(1);
                    Console.Write(errorMessage);
                    input = Console.ReadLine();
                }
            }
            return input; // Returns input.
        }

        static object DeclareInput(string prompt, string targetType, string errorMessage) // Method for declaring an input on one line.
        {
            Type type = Type.GetType(targetType); // Gets target type to pass to validate.

            Console.Write(prompt); // Asks using prompt.

            object variable = Convert.ChangeType(Validate(Console.ReadLine(), targetType, errorMessage), type); // Validates and then changes user input to the correct type after.

            return variable; // Returning.
        }

        /*-------------------------------------------------------------------------------*/
    }
}
