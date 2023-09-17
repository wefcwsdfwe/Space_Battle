using SpaceBattle.Lib;
using System.Collections.Concurrent;


namespace SpaceBattle.Lib.Test;
public class ThreadSendCommandTests
{
    public void SenderTest()
    {
        var queue = new BlockingCollection<ICommand>(100);
        var reciever = new Reciever(queue);
        var sender = new Sender(queue);
        var thread = new MyThread(reciever);
        var cmd = new ICommand();
        
        sender.Send(cmd);

        Assert.Equal(1, queue.Count);
        Assert.False(reciever.isEmpty());
    }
}