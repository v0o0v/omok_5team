namespace Omok
{
    public interface ITime
    {
        long GetCurrentTimeMilliseconds();
        long GetCurrentTime();
        float GetDeltaTime();
        long GetToday();
    }
}
