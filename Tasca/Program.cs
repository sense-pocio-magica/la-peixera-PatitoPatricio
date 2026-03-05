using System;
using System.Collections.Generic;

namespace PeixeraVirtual
{
    public enum Especie
    {
        PeixNormal,
        Tauro,
        Pop,
        Tortuga
    }

    public enum Sexe
    {
        Mascle,
        Femella,
        Cap // per Pops
    }

    public enum Direccio
    {
        Amunt,
        Avall,
        Esquerra,
        Dreta
    }
  
    public class Program
    {
        public static void Main(string[] args)
        {
            Simulador sim = new Simulador();
            sim.Inicialitza();
            sim.Executa(100);
            sim.MostraResultatsFinalsDetallats();

            Console.WriteLine("\nPrem una tecla per sortir...");
            Console.ReadKey();
        }
    }
}