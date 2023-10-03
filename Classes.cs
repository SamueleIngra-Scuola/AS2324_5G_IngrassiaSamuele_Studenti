using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS2324_5G_IngrassiaSamuele_Studenti
{
    public class StudentDatabase
    {
        public StudentDatabase() { } //Constructor without parameters
        public StudentDatabase(string _fileName) //Constructor with file name
        {
            FileName = _fileName;
            BuildStudentIndex();
        }

        public string FileName { get => FileName; set
            {
                FileName = value;
                BuildStudentIndex();
            }
        }
        public int NumRec { get; set; }
        public int NumberOfBlocks { get; set; }
        public Dictionary<int, int> StudentIndex = new Dictionary<int, int>();
        int recSize { get; set; }
        int recordsPerBlock { get; set; }

        public long FindPosition(int studentId, int blockPosition)
        {
            int recordOffsetInBlock = studentId - (recordsPerBlock * (blockPosition - 1));

            long position = (blockPosition - 1) * recordsPerBlock + recordOffsetInBlock - 1;
            position *= (recSize + 2); //+2 Because of CR+LF
            return position;
        }

        internal void BuildStudentIndex()
        {

            using (StreamReader reader = new StreamReader(FileName))
            {
                int blockNumber = 1; // Init block number
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    int studentId = int.Parse(parts[0]);

                    // Salva student ID e block number nel dictionary
                    // Incrementa block numb per il prossimo studente
                    StudentIndex[studentId] = blockNumber++;

                }
            }
        }

        public void ResizeStudentFile(int newRecSize)
        {
            recSize = newRecSize;

            string tempFileName = $"temp_{FileName}";

            try
            {
                using (StreamReader reader = new StreamReader(FileName))
                using (StreamWriter writer = new StreamWriter(tempFileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.PadRight(newRecSize, '*');
                        writer.WriteLine(line);
                    }
                }
                File.Delete(FileName);
                File.Move(tempFileName, FileName);
                GetRecordsPerBlock();
                Console.WriteLine($"{FileName} is now resized!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }

        public string ReadStudentInfoAtPosition(long position)
        {
            using (FileStream fileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            {
                //Seek(offset, origin);
                fileStream.Seek(position, SeekOrigin.Begin);

                // Read lines until carriage return character (CR)
                string studentInfo = "";
                char currentChar;
                int k = 0;
                while (k++ < recSize)
                {
                    currentChar = (char)fileStream.ReadByte();
                    studentInfo += currentChar;
                }
                studentInfo = studentInfo.Trim('*');
                return studentInfo;
            }
        }

        public int FindBlockNumber(int studentId)
        {
            int blockNumber = (studentId - 1) / recordsPerBlock + 1;

            return blockNumber;
        }

        internal int GetRecordsPerBlock()
        {
            recordsPerBlock = (int)Math.Ceiling((double)StudentIndex.Count / NumberOfBlocks);
            return recordsPerBlock;
        }
    }
}
