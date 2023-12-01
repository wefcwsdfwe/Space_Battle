using Hwdtech;
namespace SpaceBattle.Lib;

public class GetGameTimeLimitStrategy: IStrategy
{   private int cvant;
    public GetGameTimeLimitStrategy(int cvant)
    {
        this.cvant = cvant;
    }

    public object StartStrategy(params object[] args)
    {
        return this.cvant;

    }

}
