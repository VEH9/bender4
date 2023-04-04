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
            var solver = new Solver();
            var init = reader.ReadInit();
            reader.FlushToStdErr();
            var first = true;
            State prevState = null;
            string prevComand = null;
            var result = new StringBuilder();
            while (true)
            {
                var timer = new Countdown(first ? 500 : 50); //TODO fix timeouts
                var state = first ? reader.ReadState(init) : reader.ReadState(init, prevState, prevComand);
                var command = solver.GetCommand(state, timer);
                result.Append(command);
                prevState = state;
                prevComand = command.ToString();
                
                //Console.Error.WriteLine(timer);
                first = false;
            }
        }
    }
}