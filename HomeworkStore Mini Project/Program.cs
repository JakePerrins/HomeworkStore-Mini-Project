using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        static List<Homework> LoadHomeworks(string FileName)
        {
            List<Homework> HomeworkList = new List<Homework>();

            FileStream MyFile;

            try 
            { 
                MyFile = new FileStream("ProductData.bin", FileMode.Open); 
            }
            catch
            {
                MyFile = new FileStream("ProductData.bin", FileMode.Create);
                MyFile.Close();
            }
            // Checking if PoductData exists and if not creating it.

            MyFile = new FileStream(FileName, FileMode.Open);

            BinaryReader MyFileRead = new BinaryReader(MyFile); // Opening a Binary Reader

            while (MyFile.Position < MyFile.Length) // Loop until end of file
            {
                Homework homework = new Homework();

                homework.Subject = MyFileRead.ReadString();
                homework.Description = MyFileRead.ReadString();
                homework.DueDate = Convert.ToDateTime(MyFileRead.ReadString());
                homework.Completed = MyFileRead.ReadBoolean();


                HomeworkList.Add(homework); // Adds product to list
            }

            MyFileRead.Close();
            MyFile.Close();

            return HomeworkList;
        }
    }
}
