﻿using Cartheur.Ideal.Mooc.Existence;
using Cartheur.Ideal.Mooc.Interfaces;

namespace Cartheur.Ideal.Mooc
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello, Agent World!");
            // Change this line to instantiate another existence.
            IExistence existence = new Existence010(); // <-- 020 integrated

            //Existence existence = new Existence030();
            //Existence existence = new Existence031();
            //Existence existence = new Existence032();
            //Existence existence = new Existence040();
            //Existence existence = new Existence050();
            //Existence existence = new Existence051();

            // Change this line to adjust the number of cycles of the loop.
            for (int i = 0; i < 200; i++)
            {
                string stepTrace = existence.Step();
                Console.WriteLine(i + ": " + stepTrace);
            }
        }
    }
}
