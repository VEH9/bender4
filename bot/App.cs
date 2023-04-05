﻿using System;
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
            /*var first = true;
            State prevState = null;
            string prevComand = null;
            var result = new StringBuilder();
            while (true)
            {
                var state = first ? reader.ReadState(init) : reader.ReadState(init, prevState, prevComand);
                var command = solver.GetCommand(state, timer);
                result.Append(command);
                prevState = state;
                prevComand = command.ToString();
                
                //Console.Error.WriteLine(timer);
                first = false;
            }*/
            //var timer = new Countdown(1000);
            Console.WriteLine(solver.GetSolution(init));
            
            
        }
    }
}