using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using SpaceBattle.Lib;
using System.Collections.Concurrent;
using Xunit.Sdk;
using Hwdtech.Ioc;
using Hwdtech;

namespace SpaceBattle.Lib.Test;
public class ServerThreadSoftStopTest
{
    public ServerThreadSoftStopTest(){

    }


    [Fact]
    public void ThreadStartPositive()
    {   
        var thread = new MyThread(ReceiverAdapter q);
        var ssc = new SoftStopCommand(thread);

        var emptyCommand = new Mock<ICommand>();
        emptyCommand.Setup(m = m.Execute()).Callback(()=>{});
        var ec = emptyCommand.Object;

        var mre = new ManualResetEvent(false);

        var mreResetCommand = new Mock<ICommand>();
        mreResetCommand.Setup(m => m.Execute()).Callback(()=>(mre.Set()));
        var mrcRC = mreResetCommand.Object;

        var q = new  BlockingCollection<IReceiver>(100);  

        q.Add(ec);
        q.Add(ec);
        q.Add(ec);
        q.Add(ssc);
        q.Add(ec);
        q.Add(ec);
        q.Add(ec);
        

        Assert.Equal(7, q.Count);
        Assert.False(q.isEmpty());

           
        // Act
        thread.Start();
    
        // Post
        Assert.True(q.IsEmpty());
        thread.Stop();
    }   
}