namespace bot
{
    public class Solver
    {
        public BotCommand GetCommand(State state, Countdown countdown) //R D L U 
        {
            return new Move(V.Zero) { Message = "Nothing to do..." };
        }
    }
}   