﻿using Hwdtech;

namespace SpaceBattle.Lib
{
    public class CheckCollision : ICommand
    {
        IUObject UO1 { get; }
        IUObject UO2 { get; }
        public CheckCollision(IUObject UnicObj1, IUObject UnicObj2)
        {
            UO1 = UnicObj1;
            UO2 = UnicObj2;
        }

        public void Execute()
        {
            var vec = IoC.Resolve<Vector>("Calculate.Delta", UO1, UO2);
            bool collis = IoC.Resolve<bool>("CollisionDecisionTree", vec);
            if (collis)
            {
                throw new Exception();
            }
        }
    }
}
