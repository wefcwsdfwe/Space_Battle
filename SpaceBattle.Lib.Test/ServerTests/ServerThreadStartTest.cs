using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using SpaceBattle.Lib;
using System.Collections.Concurrent;
using Xunit.Sdk;

namespace SpaceBattle.Lib.Test;
public class ServerThreadStartTest
{
    public MoveCommandPositive()
    {
        
        // pre
        var movable = new Mock<IMovable>();
        movable.SetupGet(m => m.Position).Returns(new int[] {12, 5}.Verifiable());
        movable.SetupGet(m => m.Velocity).Returns(new int[] {12, 5}.Verifiable());
        var mc = new MoveCommand(movable.Object);
    
        //act
        mc.Execute();

        //post
        movable.VerifySet(m => m.Position = new Int[] {7, 8}, Times.Once);
        movable.VerifyAll();
    }

    [Fact]

    public void ThreadStartPositive()
    {
        var emptyCommand = new Mock<ICommand>();
        emptyCommand.Setup(m = m.Execute()).Callback(()=>{})
        var ec = emptyCommand.Object;

        var mre = new ManualResetEvent(false);

        var mreResetCommand = new Mock<ICommand>();
        mreResetCommand.Setup(m => m.Execute()).Callback(()=>(mre.Set()));
        var mrcRC = mreResetCommand.Object;

        var q = new  BlockingCollection<IReceiver>(100);  
        q.Add(ec);
        q.Add(ec);
        q.Add(ec);
        q.Add(mreRC);

        var thread = new MyThread(q);
           
        // Act
        thread.Start();
        
    
        Assert.True(q.IsEmpty());
    }
        // thread.Stop();   
}