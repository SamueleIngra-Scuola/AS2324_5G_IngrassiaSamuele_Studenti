using AS2324_5G_IngrassiaSamuele_Studenti;
namespace StudentDatabaseTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BuildDB()
        {
            StudentDatabase db = new StudentDatabase("test.dat"); 
            if (db.FileName == "test.dat") //Check if filename was correct
                Assert.Pass();
            else
                Assert.Fail();
        }

        [Test]
        public void FindPosition()
        {
            StudentDatabase db = new StudentDatabase("test.dat");

            db.NumberOfBlocks = 7;

            int recSize = 80;

            db.ResizeStudentFile(recSize);

            long position = db.FindPosition(7, 2);

            if (position == 492) //Check if info string is correct
                Assert.Pass();
            else
                Assert.Fail();
        }

        [Test]
        public void SearchStudent()
        {
            StudentDatabase db = new StudentDatabase("test.dat");
            
            db.NumberOfBlocks = 7;

            int recSize = 80;

            db.ResizeStudentFile(recSize);

            string info = db.ReadStudentInfoAtPosition(492); //Position 7

            if (info == "7,Emily,Taylor,333 Main St,Anytown,CA,12345") //Check if info string is correct
                Assert.Pass();
            else
                Assert.Fail();
        }

        [Test]
        public void FindBlockNumber()
        {
            StudentDatabase db = new StudentDatabase("test.dat");
            
            db.NumberOfBlocks = 7;

            int recSize = 80;

            db.ResizeStudentFile(recSize);

            if (db.FindBlockNumber(7) == 2) //Check if info string is correct
                Assert.Pass();
            else
                Assert.Fail();
        }

        /*public void BuildDB()
        {
            StudentDatabase db = new StudentDatabase("test.dat");
            if (db.FileName == "test.dat")
                Assert.Pass();
            else
                Assert.Fail();
        }*/
    }
}