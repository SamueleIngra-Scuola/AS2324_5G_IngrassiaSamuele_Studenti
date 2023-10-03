using AS2324_5G_IngrassiaSamuele_Studenti;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

class Program
{

    // index per direct access 
    // student ID = key
    // block number = value

    static void Main()
    {

        //build dictionary
        StudentDatabase db = new StudentDatabase("studenti.dat");


        // Cerca uno studente via student ID
        Console.Write("Please insert Student ID: ");
        if (int.TryParse(Console.ReadLine(), out int studentId))
        {
            if (db.GetStudentIndex().TryGetValue(studentId, out int blockNumber))
            {
                db.NumberOfBlocks = 7;

                int recSize = 80;

                db.ResizeStudentFile(recSize);

                int blockPosition = db.FindBlockNumber(studentId);

                if (blockPosition > 0 && blockPosition <= db.NumberOfBlocks)
                {
                    long position = db.FindPosition(studentId, blockPosition);

                    Console.WriteLine(position);

                    string studentInfo = db.ReadStudentInfoAtPosition(position);

                    Console.WriteLine($"Student {studentId} was found in block {blockPosition}");
                    Console.WriteLine($"Info: {studentInfo}");
                }

            }

            else
                Console.WriteLine($"Student {studentId} not found");
        }
        else
            Console.WriteLine("Invalid input, please insert a valid student id");
    }

}