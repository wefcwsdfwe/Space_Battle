using Hwdtech.Ioc;
namespace SpaceBattle.Lib.Test;
public class GameQueuePushTests
{
    public GameQueuePushTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }
    [Fact]
    public void GameQueuePushCommandTest()
    {
        var icom = new Mock<ICommand>();
        var queue = new Mock<IQueue<ICommand>>();
        bool pushed = false;
        queue.Setup(m=>m.Push(icom.Object)).Callback(()=>{pushed=true;});

        var cmd = new GameQueuePushCommand(queue.Object, icom.Object);
        cmd.Execute();
        Assert.True(pushed);
    }
    [Fact]
    public void GameQueuePushCommandStrategyTest()
    {
        var icom = new Mock<ICommand>();
        var queue = new Mock<IQueue<ICommand>>();
        bool pushed = false;
        queue.Setup(m=>m.Push(icom.Object)).Callback(()=>{pushed=true;});
        var getQueue_Strat = new Mock<IStrategy>();
        getQueue_Strat.Setup(m=>m.StartStrategy("Game123")).Returns(queue.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameQueue",(object[]args)=>getQueue_Strat.Object.StartStrategy(args)).Execute();

        var strat = new GameQueuePushCommandStrategy();
        var cmd =(ICommand) strat.StartStrategy("Game123", icom.Object);
        cmd.Execute();
        Assert.True(pushed);
    }
}
