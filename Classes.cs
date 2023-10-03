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
        public StudentDatabase(string _tempName) //Constructor with file name
        {
            _fileName = _tempName;
            BuildStudentIndex();
        }

        string _fileName;

        public string FileName { get => _fileName; set // Automatically readjust StudentIndex when FileName is set
            {
                _fileName = value;
                BuildStudentIndex();
            }
        }
        public int NumRec { get; set; }
        public int NumberOfBlocks { get; set; }

        internal Dictionary<int, int> StudentIndex = new Dictionary<int, int>();
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

        public Dictionary<int, int> GetStudentIndex()
        {
            return StudentIndex;
        }

        public void ResizeStudentFile(int newRecSize)
        {
            recSize = newRecSize;

            string tempFileName = $"temp_{_fileName}";

            try
            {
                using (StreamReader reader = new StreamReader(_fileName))
                using (StreamWriter writer = new StreamWriter(tempFileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.PadRight(newRecSize, '*');
                        writer.WriteLine(line);
                    }
                }
                File.Delete(_fileName);
                File.Move(tempFileName, _fileName);
                GetRecordsPerBlock();
                Console.WriteLine($"{_fileName} is now resized!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }

        public string ReadStudentInfoAtPosition(long position)
        {
            using (FileStream fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read))
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
