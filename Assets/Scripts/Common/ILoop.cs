namespace Omok
{
    public interface ILoop
    {
        void Add(IUpdatable updatable);
        void Remove(IUpdatable updatable);
    }
}
