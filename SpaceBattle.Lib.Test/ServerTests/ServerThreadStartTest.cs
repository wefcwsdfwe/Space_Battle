using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using SpaceBattle.Lib;
using System.Collections.Concurrent;
using Xunit.Sdk;
using Hwdtech.Ioc;
using Hwdtech;

namespace SpaceBattle.Lib.Test;
public class ServerThreadStartTest
{
    
   [Fact]
    public void ThreadStartPositive()
    {
        var emptyCommand = new Mock<ICommand>();
        emptyCommand.Setup(m = m.Execute()).Callback(()=>{});
        var ec = emptyCommand.Object;

        var mre = new ManualResetEvent(false);

        var mreResetCommand = new Mock<ICommand>();
        mreResetCommand.Setup(m => m.Execute()).Callback(()=>(mre.Set()));
        var mrcRC = mreResetCommand.Object;

        var q = new  BlockingCollection<IReceiver>(100);  

        // public HardStopCommand hsc;
        q.Add(ec);
        q.Add(ec);
        q.Add(ec);
        q.Add(mreRC);


        Assert.Equal(4, q.Count);
        Assert.False(q.isEmpty());

        var thread = new MyThread(q);
           
        // Act
        thread.Start();
        mre.WaitOne();
    
        // Post
        
        Assert.True(q.IsEmpty());
        thread.Stop(); 
    }  
}