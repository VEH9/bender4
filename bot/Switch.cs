namespace bot
{

    public class Switch
    {
        public readonly Point location;
        public readonly Point magneticField;

        public Switch(Point location, Point magneticField)
        {
            this.location = location;
            this.magneticField = magneticField;
        }

        public Switch(Switch @switch)
        {
            this.location = @switch.location;
            this.magneticField = @switch.magneticField;
        }
    }
}