using Hwdtech.Ioc;
namespace SpaceBattle.Lib.Test;
public class ConstructCommandStrategyTests
{
    public ConstructCommandStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }
    [Fact]
    public void ConstructCommandStrategyTest()
    {
        var getGameObj_Strat = new Mock<IStrategy>();
        var obj = new Mock<IUObject>();
        bool prop_set=false;
        obj.Setup(m=>m.set_property("Velocity", 2)).Callback(()=>{prop_set=true;});
        bool obj_got = false;
        getGameObj_Strat.Setup(m=>m.StartStrategy("Item548")).Returns(obj.Object).Callback(()=>{obj_got = true;});
        var Move_Strat = new Mock<IStrategy>();
        var icom = new Mock<ICommand>();
        bool command_got = false;
        Move_Strat.Setup(m=>m.StartStrategy(obj.Object)).Returns(icom.Object).Callback(()=>{command_got = true;});
        var uobjectsetproperty = new Mock<ICommand>();
        var uobjectsetpropertystrategy = new Mock<IStrategy>();
        uobjectsetpropertystrategy.Setup(m => m.StartStrategy(It.IsAny<object[]>())).Returns(uobjectsetproperty.Object).Callback(()=>{prop_set = true;});

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameObject", (object[]args)=>getGameObj_Strat.Object.StartStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "UObjectsetProperty", (object[]args)=>uobjectsetpropertystrategy.Object.StartStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "MoveCommand", (object[]args)=>Move_Strat.Object.StartStrategy(args)).Execute();

        var m = new Mock<IMessage>();
        var prop = new Dictionary<string,object>(){{"Velocity",2}};
        m.SetupGet(m=>m.CommandName).Returns("Move");
        m.SetupGet(m=>m.GameId).Returns("Game123");
        m.SetupGet(m=>m.GameItemId).Returns("Item548");
        m.SetupGet(m=>m.CommandParams).Returns(prop);
        var cmd = new ConstructCommandStrategy();
        cmd.StartStrategy(m.Object);
        Assert.True(obj_got);
        Assert.True(prop_set);
        Assert.True(command_got);
    }
}
