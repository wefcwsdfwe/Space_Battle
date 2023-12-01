using Hwdtech.Ioc;
namespace SpaceBattle.Lib.Test;
public class InterpretationCommandTests
{
    public InterpretationCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }
    [Fact]
    public void InterpretationCommandTest()
    {
        var getGameObj_Strat = new Mock<IStrategy>();
        var obj = new Mock<IUObject>();
        getGameObj_Strat.Setup(m=>m.StartStrategy("Item548")).Returns(obj.Object);
        var Move_Strat = new Mock<IStrategy>();
        var icom = new Mock<ICommand>();
        bool command_got = false;
        Move_Strat.Setup(m=>m.StartStrategy(obj.Object)).Returns(icom.Object).Callback(()=>{command_got = true;});
        var getQueue_Strat = new Mock<IStrategy>();
        var queue = new Mock<IQueue<ICommand>>();
        bool pushed = false;
        queue.Setup(m=>m.Push(It.IsAny<ICommand>())).Callback(()=>{pushed = true;});
        getQueue_Strat.Setup(m=>m.StartStrategy("Game123")).Returns(queue.Object);
        var uobjectsetproperty = new Mock<ICommand>();
        var uobjectsetpropertystrategy = new Mock<IStrategy>();
        uobjectsetpropertystrategy.Setup(m => m.StartStrategy(It.IsAny<object[]>())).Returns(uobjectsetproperty.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameQueue.PushCommand", (object[]args)=>new GameQueuePushCommandStrategy().StartStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ConstructCommand", (object[]args)=>new ConstructCommandStrategy().StartStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameObject", (object[]args)=>getGameObj_Strat.Object.StartStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "UObjectsetProperty", (object[]args)=>uobjectsetpropertystrategy.Object.StartStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "MoveCommand", (object[]args)=>Move_Strat.Object.StartStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameQueue",(object[]args)=>getQueue_Strat.Object.StartStrategy(args)).Execute();

        var m = new Mock<IMessage>();
        var prop = new Dictionary<string,object>(){{"Velocity",2}};
        m.SetupGet(m=>m.CommandName).Returns("Move");
        m.SetupGet(m=>m.GameId).Returns("Game123");
        m.SetupGet(m=>m.GameItemId).Returns("Item548");
        m.SetupGet(m=>m.CommandParams).Returns(prop);
        var cmd = new InterpretationCommand(m.Object);
        cmd.Execute();
        Assert.True(pushed);
        Assert.True(command_got);
    }
}
