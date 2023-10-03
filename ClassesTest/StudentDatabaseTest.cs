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
            if (db.FileName == "test.dat")
                Assert.Pass();
            else
                Assert.Fail();
        }

        public void BuildDB()
        {
            StudentDatabase db = new StudentDatabase("test.dat");
            if (db.FileName == "test.dat")
                Assert.Pass();
            else
                Assert.Fail();
        }
    }
}