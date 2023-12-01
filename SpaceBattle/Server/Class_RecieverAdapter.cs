using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class RecieverAdapter : IReceiver
{
    BlockingCollection<ICommand> queue;

    public RecieverAdapter(BlockingCollection<ICommand> queue)
    {
        this.queue = queue;
    }
    public bool isEmpty()
    {
        return queue.Count == 0;
    }
    public ICommand Receive()
    {
        return queue.Take();
    }
}
