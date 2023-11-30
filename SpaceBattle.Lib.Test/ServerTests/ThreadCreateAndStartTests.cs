using SpaceBattle.Lib;
using System.Collections.Concurrent;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;
public class ThreadCreateAndStartTests
{
    public ThreadCreateAndStartTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }
    [Fact]
    public void ThreadCreateAndStartTest()
    {
        var queue = new BlockingCollection<ICommand>(100);
        var reciever = new RecieverAdapter(queue);
        var sender = new Sender(queue);
        var thread = new MyThread(reciever);
        var cv = new ManualResetEvent(false);
        var hs = new ThreadHardStopCommand(thread);
        var cmd = new ActionCommand((arg) =>
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            cv.Set();
            hs.Execute();
        });
        sender.Send(cmd);
        Assert.True(queue.Count == 1);
        Assert.False(reciever.isEmpty());
        thread.Execute();
        Assert.True(cv.WaitOne(10000));
        Assert.NotEqual(1, queue.Count);
        Assert.True(reciever.isEmpty());
    }
    [Fact]
    public void ThreadCreateAndStartStrategyTest()
    {
        var thread_dict = new Dictionary<string, MyThread>();
        var sender_dict = new Dictionary<string, ISender>();
        var m_get_thread_list_strategy = new Mock<IStrategy>();
        var m_get_sender_list_strategy = new Mock<IStrategy>();
        m_get_sender_list_strategy.Setup(m => m.StartStrategy()).Returns(sender_dict);
        m_get_thread_list_strategy.Setup(m => m.StartStrategy()).Returns(thread_dict);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThreadList", (object[] args) => m_get_thread_list_strategy.Object.StartStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetSenderList", (object[] args) => m_get_sender_list_strategy.Object.StartStrategy(args)).Execute();
        var cr_strategy = new ThreadCreateAndStartCommandStrategy();
        var cr = (ActionCommand)cr_strategy.StartStrategy("Thread_id");
        cr.Execute();
        Assert.True(thread_dict.ContainsKey("Thread_id"));
        var thread = thread_dict["Thread_id"];
        Assert.Equal(typeof(MyThread), thread.GetType());

        var sender = sender_dict["Thread_id"];
        var hs = new ThreadHardStopCommand(thread);
        sender.Send(hs);

    }
    [Fact]
    public void ThreadCreateAndStartStrategyWithActionTest()
    {
        bool actioncall = false;
        var thread_dict = new Dictionary<string, MyThread>();
        var sender_dict = new Dictionary<string, ISender>();
        var m_get_thread_list_strategy = new Mock<IStrategy>();
        var m_get_sender_list_strategy = new Mock<IStrategy>();
        m_get_sender_list_strategy.Setup(m => m.StartStrategy()).Returns(sender_dict);
        m_get_thread_list_strategy.Setup(m => m.StartStrategy()).Returns(thread_dict);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThreadList", (object[] args) => m_get_thread_list_strategy.Object.StartStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetSenderList", (object[] args) => m_get_sender_list_strategy.Object.StartStrategy(args)).Execute();
        var cr_strategy = new ThreadCreateAndStartCommandStrategy();
        var cr = (ActionCommand)cr_strategy.StartStrategy("Thread_id",  ()=>{actioncall=true;});
        cr.Execute();
        Assert.True(thread_dict.ContainsKey("Thread_id"));
        var thread = thread_dict["Thread_id"];
        Assert.Equal(typeof(MyThread), thread.GetType());
        Assert.True(actioncall);

        var sender = sender_dict["Thread_id"];
        var hs = new ThreadHardStopCommand(thread);
        sender.Send(hs);

    }
}
