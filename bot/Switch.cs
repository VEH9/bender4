namespace bot
{

    public class Switch
    {
        public readonly Point location;
        public readonly Point magneticField;
        public int fieldStatus;

        public Switch(Point location, Point magneticField, int status)
        {
            this.location = location;
            this.magneticField = magneticField;
            this.fieldStatus = status;
        }

        public Switch(Switch @switch)
        {
            this.location = @switch.location;
            this.magneticField = @switch.magneticField;
            this.fieldStatus = @switch.fieldStatus;
        }
    }
}