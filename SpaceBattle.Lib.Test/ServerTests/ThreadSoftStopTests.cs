using SpaceBattle.Lib;
using System.Collections.Concurrent;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;
public class ThreadSoftStopTests
{
    public ThreadSoftStopTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        var m_exceptionHandler = new Mock<ICommand>();
        var m_handler = new Mock<IStrategy>();
        m_handler.Setup(m => m.StartStrategy(It.IsAny<object[]>())).Returns(m_exceptionHandler.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetExceptionHandler", (object[] args) => m_handler.Object.StartStrategy(args)).Execute();
    }
    [Fact]
    public void ThreadSoftStopInCorrectThread()
    {
        bool handler_called = false;
        var m_exceptionHandler = new Mock<ICommand>();
        m_exceptionHandler.Setup(m => m.Execute()).Callback(() => { handler_called = true; });
        var m_handler = new Mock<IStrategy>();
        m_handler.Setup(m => m.StartStrategy(It.IsAny<object[]>())).Returns(m_exceptionHandler.Object);
        var queue = new BlockingCollection<ICommand>(100);
        var reciever = new RecieverAdapter(queue);
        var sender = new Sender(queue);
        var thread = new MyThread(reciever);
        var sf = new ThreadSoftStopCommand(thread);
        var cv = new ManualResetEvent(false);
        var cv1 = new ManualResetEvent(false);
        var cmd = new ActionCommand((arg) =>
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetExceptionHandler", (object[] args) => m_handler.Object.StartStrategy(args)).Execute();
            cv.WaitOne();
            sf.Execute();
        });
        bool wasCalled = false;
        var mcmd = new Mock<ICommand>();
        mcmd.Setup(m => m.Execute()).Callback(() => { wasCalled = true; cv1.Set(); });
        thread.Execute();
        sender.Send(cmd);
        sender.Send(mcmd.Object);
        cv.Set();
        Assert.True(cv1.WaitOne(10000));
        Assert.True(wasCalled);
        Assert.False(handler_called);
    }
    [Fact]
    public void ThreadSoftStopInNotCorrectThread()
    {
        bool handler_called = false;
        var m_exceptionHandler = new Mock<ICommand>();
        m_exceptionHandler.Setup(m => m.Execute()).Callback(() => { handler_called = true; });
        var m_handler = new Mock<IStrategy>();
        m_handler.Setup(m => m.StartStrategy(It.IsAny<object[]>())).Returns(m_exceptionHandler.Object);
        var queue = new BlockingCollection<ICommand>(100);
        var queue1 = new BlockingCollection<ICommand>(100);
        var reciever = new RecieverAdapter(queue);
        var reciever1 = new RecieverAdapter(queue1);
        var sender = new Sender(queue);
        var sender1 = new Sender(queue1);
        var thread = new MyThread(reciever);
        var thread1 = new MyThread(reciever1);
        var sf = new ThreadSoftStopCommand(thread);
        var cv = new ManualResetEvent(false);
        var cv1 = new ManualResetEvent(false);
        var cmd = new ActionCommand((arg) =>
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetExceptionHandler", (object[] args) => m_handler.Object.StartStrategy(args)).Execute();
            cv.WaitOne();
            sf.Execute();
        });
        bool wasCalled = false;
        var mcmd = new Mock<ICommand>();
        mcmd.Setup(m => m.Execute()).Callback(() => { wasCalled = true; cv1.Set(); });
        thread1.Execute();
        sender1.Send(cmd);
        sender1.Send(mcmd.Object);
        cv.Set();
        Assert.True(cv1.WaitOne(10000));
        Assert.True(wasCalled);
        Assert.True(handler_called);

        var hs1 = new ThreadHardStopCommand(thread1);
        sender1.Send(hs1);
        var hs = new ThreadHardStopCommand(thread);
        sender.Send(hs);
    }
    [Fact]
    public void ThreadSoftStopStrategyTest()
    {
        var queue = new BlockingCollection<ICommand>(100);
        var sender = new Sender(queue);
        var reciever = new RecieverAdapter(queue);
        var thread = new MyThread(reciever);
        var m_thread_tree_Strategy = new Mock<IStrategy>();
        m_thread_tree_Strategy.Setup(m => m.StartStrategy("Thread_id")).Returns(thread);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThread", (object[] args) => m_thread_tree_Strategy.Object.StartStrategy(args)).Execute();
        var m_sender_tree_Strategy = new Mock<IStrategy>();
        m_sender_tree_Strategy.Setup(m => m.StartStrategy("Thread_id")).Returns(sender);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Thread.GetSender", (object[] args) => m_sender_tree_Strategy.Object.StartStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Thread.SendCommand", (object[] args) => new SendCommandStrategy().StartStrategy(args)).Execute();
        var sfts_strategy = new ThreadSoftStopCommandStrategy();
        Assert.True(reciever.isEmpty());
        var sfts = (ActionCommand)sfts_strategy.StartStrategy("Thread_id");
        sfts.Execute();
        Assert.False(reciever.isEmpty());
    }
    [Fact]
    public void ThreadSoftStopStrategyWithActionTest()
    {
        bool actioncall = false;
        var queue = new BlockingCollection<ICommand>(100);
        var sender = new Sender(queue);
        var reciever = new RecieverAdapter(queue);
        var thread = new MyThread(reciever);
        var m_thread_tree_Strategy = new Mock<IStrategy>();
        m_thread_tree_Strategy.Setup(m => m.StartStrategy("Thread_id")).Returns(thread);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThread", (object[] args) => m_thread_tree_Strategy.Object.StartStrategy(args)).Execute();
        var m_sender_tree_Strategy = new Mock<IStrategy>();
        m_sender_tree_Strategy.Setup(m => m.StartStrategy("Thread_id")).Returns(sender);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Thread.GetSender", (object[] args) => m_sender_tree_Strategy.Object.StartStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Thread.SendCommand", (object[] args) => new SendCommandStrategy().StartStrategy(args)).Execute();
        var sfts_strategy = new ThreadSoftStopCommandStrategy();
        Assert.True(reciever.isEmpty());
        var sfts = (ActionCommand)sfts_strategy.StartStrategy("Thread_id", ()=>{actioncall=true;});
        sfts.Execute();
        Assert.False(reciever.isEmpty());
        Assert.True(actioncall);
    }
}
