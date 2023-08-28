namespace HospitalSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Address victorAddress = new Address("123 Victoria Ave", "Sydney", "NSW");
            Patient victorPatient = new Patient("Victor", "Zottmann", "v@z.com", "123456789", victorAddress);
            victorPatient.ListDetails();
        }
    }
}