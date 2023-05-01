using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace bot
{
    public static class App
    {
        [SuppressMessage("ReSharper", "FunctionNeverReturns")]
        private static void Main(string[] args)
        {
            var reader = new ConsoleReader();
            var init = reader.ReadInit();
            var solver = new Solver();
            reader.FlushToStdErr();
            var timer = new Countdown(900);
            Console.WriteLine(solver.GetSolution(init, timer));
        }
    }
}