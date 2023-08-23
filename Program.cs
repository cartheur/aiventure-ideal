using Cartheur.Ideal.Mooc.Interfaces;

namespace Cartheur.Ideal.Mooc
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello, David...Hello, Chris");
            
            IExistence existence = new Existence(); // <-- 020, 030 integrated, 031-032 integrated (check), 040 integrated (check), 050 integrated (check) null object!
            // 051, 052 todo //Existence existence = new Existence051();

            // Only passes with '2', three or more a null object fail.
            for (int i = 0; i < 2; i++)
            {
                string stepTrace = existence.Step();
                Console.WriteLine(i + ": " + stepTrace);
            }
        }
    }
}
