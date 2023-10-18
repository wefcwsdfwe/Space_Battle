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

public class ReceiverAdapter(): IReceiver {
    BlockingCollection<ICommand> q;
    ReceiverAdapter (BlockingCollection<ICommand> q) {
        this.q = q;
    }
    ICommand Receive() {
        return q.Take();
    }
    bool IsEmpty() {
        return thread.q.IsEmpty();       
    }
}