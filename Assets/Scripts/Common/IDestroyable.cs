using System;

namespace Omok
{
    public interface IDestroyable
    {
        event EventHandler<DestroyEventArgs> DestroyEvent;
        void Destroy();
    }
}
