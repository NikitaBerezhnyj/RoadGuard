namespace RoadGuard.Utils
{
    public class TimeUtils
    {
        public static int MinutesToMilliseconds(int minutes)
        {
            return minutes * 60 * 1000;
        }
    }
}