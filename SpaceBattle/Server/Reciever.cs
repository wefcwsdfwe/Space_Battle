using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class Reciever : IReceiver
{
    BlockingCollection<ICommand> queue;

    public Reciever(BlockingCollection<ICommand> queue)
    {
        this.queue = queue;
    }
    public bool IsEmpty()
    {
        return queue.Count == 0;
    }
    public ICommand Receive()
    {
        return queue.Take();
    }
}