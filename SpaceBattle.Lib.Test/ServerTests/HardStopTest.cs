using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using SpaceBattle.Lib;
using System.Collections.Concurrent;
using Xunit.Sdk;
using Hwdtech.Ioc;
using Hwdtech;

namespace SpaceBattle.Lib.Test;
public class ServerThreadHardStopTest
{   
    public ServerThreadHardStopTest() {
        
    }

    [Fact]
    public void ThreadStartPositive()
    {
        var activete = new ReceiverAdapter();
        var q = new BlockingCollection<ICommand>(100); 
        var thread = new MyThread(q);        
        var hsc = new HardStopCommand(thread); 
        
        var emptyCommand = new Mock<ICommand>();
        emptyCommand.Setup(m => m.Execute()).Callback(()=>{});
        var ec = emptyCommand.Object;

        var mre = new ManualResetEvent(false);

        var mreResetCommand = new Mock<ICommand>();
        mreResetCommand.Setup(m => m.Execute()).Callback(()=>(mre.Set()));
        var mrcRC = mreResetCommand.Object;

        // public HardStopCommand hsc;
        q.Add(ec);
        q.Add(ec);
        q.Add(ec);
        q.Add(hsc);
        q.Add(ec);
        q.Add(ec);
        


        Assert.Equal(6, q.Count);
        Assert.False(q.IsEmpty());

        
        // Act
        thread.Start();

        // Post
        Assert.False(q.IsEmpty());
        
        thread.Stop();
    }   
}