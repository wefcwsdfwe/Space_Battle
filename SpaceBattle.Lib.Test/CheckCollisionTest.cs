﻿using Hwdtech;
using Moq;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test
{
    public class CheckCollisionTest
    {
        public CheckCollisionTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }
        [Fact]
        public void NoCollision_and_NoException()
        {
            var UnicObject1 = new Mock<IUObject>();
            var UnicObject2 = new Mock<IUObject>();
            UnicObject1.Setup(x => x.get_property("Velocity")).Returns(new Vector(2,1));
            UnicObject1.Setup(x => x.get_property("Coords")).Returns(new Vector(1, 1));
            UnicObject2.Setup(x => x.get_property("Velocity")).Returns(new Vector(0, 1));
            UnicObject2.Setup(x => x.get_property("Coords")).Returns(new Vector(0, 1));
            var DeltaStrategy = new StrategyDeltaCalculation();
            var GetPropertyStrategy = new StrategyGetProperty();
            var DecisionStrategy = new Mock<IStrategy>();

            DecisionStrategy.Setup(_strategy => _strategy.StartStrategy(It.IsAny<object[]>())).Returns(false).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "UObject.GetProperty", (object[] args) => GetPropertyStrategy.StartStrategy(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Calculate.Delta", (object[] args) => DeltaStrategy.StartStrategy(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CollisionDecisionTree", (object[] args) => DecisionStrategy.Object.StartStrategy(args)).Execute();
            CheckCollision collision = new CheckCollision(UnicObject1.Object, UnicObject2.Object);

            collision.Execute();
            DecisionStrategy.Verify();
        }
        [Fact]
        public void YesCollision_and_Exception()
        {
            var UnicObject1 = new Mock<IUObject>();
            var UnicObject2 = new Mock<IUObject>();
            UnicObject1.Setup(x => x.get_property("Velocity")).Returns(new Vector(2, 1));
            UnicObject1.Setup(x => x.get_property("Coords")).Returns(new Vector(1, 1));
            UnicObject2.Setup(x => x.get_property("Velocity")).Returns(new Vector(0, 1));
            UnicObject2.Setup(x => x.get_property("Coords")).Returns(new Vector(0, 1));
            var DeltaStrategy = new StrategyDeltaCalculation();
            var GetPropertyStrategy = new StrategyGetProperty();
            var DecisionStrategy = new Mock<IStrategy>();

            DecisionStrategy.Setup(_strategy => _strategy.StartStrategy(It.IsAny<object[]>())).Returns(true).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "UObject.GetProperty", (object[] args) => GetPropertyStrategy.StartStrategy(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Calculate.Delta", (object[] args) => DeltaStrategy.StartStrategy(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CollisionDecisionTree", (object[] args) => DecisionStrategy.Object.StartStrategy(args)).Execute();
            CheckCollision collision = new CheckCollision(UnicObject1.Object, UnicObject2.Object);

            Assert.Throws<Exception>(() => collision.Execute());
            DecisionStrategy.Verify();
        }

    }
}
