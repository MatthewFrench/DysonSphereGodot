using System;
using Test;
using Newtonsoft.Json;

namespace Run
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Hexasphere h = new Hexasphere(30, 25, 0.95);

            Console.WriteLine("Number of Tiles: " + h.GetTiles().Count);
            Console.WriteLine("Tiles: " + JsonConvert.SerializeObject(h.toJson()));
        }
    }
}
