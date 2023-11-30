namespace SpaceBattle.Lib;
using System.Collections.Concurrent;
using Hwdtech;

public class ThreadCreateAndStartCommand : ICommand
{
    string thread_id;

    public ThreadCreateAndStartCommand(string thread_id)
    {
        this.thread_id = thread_id;
    }

    public void Execute()
    {
        BlockingCollection<ICommand> queue = new BlockingCollection<ICommand>(1000);
        var reciever = new RecieverAdapter(queue);
        MyThread thread = new MyThread(reciever);
        thread.Execute();
        var dict = IoC.Resolve<Dictionary<string, MyThread>>("GetThreadList");
        dict.Add(thread_id, thread);
        var senders = IoC.Resolve<Dictionary<string, ISender>>("GetSenderList");
        var sender = new Sender(queue);
        senders.Add(thread_id, sender);
    }
}
