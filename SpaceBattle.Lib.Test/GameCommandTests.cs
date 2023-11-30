using SpaceBattle.Lib;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.test;
public class GameCommandTests
{   
    public GameCommandTests(){
       
    }
    [Fact]
    public void GameCommandTest(){
         new InitScopeBasedIoCImplementationCommand().Execute();
        var scope1 = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        var game_scope = IoC.Resolve<object>("Scopes.New", scope1);
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", game_scope).Execute();
        var m_handler = new Mock<IExceptionHandler>();
        var m_handler_strat = new Mock<IStrategy>();
        m_handler_strat.Setup(m => m.StartStrategy(It.IsAny<object[]>())).Returns(m_handler.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetExceptionHandler", (object[] args) => m_handler_strat.Object.StartStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameTimeLimit", (object[] args) => new GetGameTimeLimitStrategy(400).StartStrategy()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope1).Execute();
        
        var cmd = new ActionCommand((arg) =>
        {   
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameTimeLimit", (object[] args) => new GetGameTimeLimitStrategy(0).StartStrategy()).Execute();
        });
        var queue = new Queue<ICommand>(100);
        queue.Enqueue(cmd);
        queue.Enqueue(cmd);
        var receiver = new GameRecieverAdapter(queue);
        var game  = new GameCommand("GameId", game_scope, receiver);
        game.Execute();
        Assert.False(receiver.isEmpty());
    }
    [Fact]
    public void GameCommandHandleTest(){
        new InitScopeBasedIoCImplementationCommand().Execute();
        var scope1 = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        var game_scope1 = IoC.Resolve<object>("Scopes.New", scope1);
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", game_scope1).Execute();
        var m_handler = new Mock<IExceptionHandler>();
        bool handler_called = false;
        m_handler.Setup(m=>m.Handle()).Callback(()=>{handler_called = true;});
        var m_handler_strat = new Mock<IStrategy>();
        m_handler_strat.Setup(m => m.StartStrategy(It.IsAny<object[]>())).Returns(m_handler.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetExceptionHandler", (object[] args) => m_handler_strat.Object.StartStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameTimeLimit", (object[] args) => new GetGameTimeLimitStrategy(400).StartStrategy()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope1).Execute();
        
        var cmd = new Mock<ICommand>();
        var cmd1 = new ActionCommand((arg) =>
        {   
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameTimeLimit", (object[] args) => new GetGameTimeLimitStrategy(0).StartStrategy()).Execute();
        });
        cmd.Setup(m=>m.Execute()).Throws<Exception>();
        var queue = new Queue<ICommand>(100);
        queue.Enqueue(cmd.Object);
        queue.Enqueue(cmd1);
        var receiver = new GameRecieverAdapter(queue);
        var game  = new GameCommand("GameId", game_scope1, receiver);
        game.Execute();
        Assert.True(handler_called);
    }
    
}
