using SpaceBattle.Lib;
using System.Collections.Concurrent;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;
public class ThreadSendCommandTests
{
    public ThreadSendCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }
    [Fact]
    public void SenderTest()
    {
        var queue = new BlockingCollection<ICommand>(100);
        var reciever = new RecieverAdapter(queue);
        var sender = new Sender(queue);
        var thread = new MyThread(reciever);
        var cmd = new ActionCommand((arg) =>
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        });
        sender.Send(cmd);
        Assert.True(queue.Count == 1);
        Assert.False(reciever.isEmpty());
    }
    [Fact]
    public void ThreadSendCommandStrategyTest()
    {
        var queue = new BlockingCollection<ICommand>(100);
        var sender = new Sender(queue);
        var reciever = new RecieverAdapter(queue);
        var thread = new MyThread(reciever);
        var m_sender_tree_Strategy = new Mock<IStrategy>();
        m_sender_tree_Strategy.Setup(m => m.StartStrategy("Thread_id")).Returns(sender);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Thread.GetSender", (object[] args) => m_sender_tree_Strategy.Object.StartStrategy(args)).Execute();
        var sendcommandstrategy = new SendCommandStrategy();
        var cmd = new ThreadHardStopCommand(thread);
        var return_cmd = (ActionCommand)sendcommandstrategy.StartStrategy("Thread_id", cmd);

        Assert.True(reciever.isEmpty());

        return_cmd.Execute();

        Assert.False(reciever.isEmpty());
    }
}
