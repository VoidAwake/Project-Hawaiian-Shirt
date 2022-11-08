using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Codice.CM.Client.Differences;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class TreasureUtil
{
    public enum PointLevel
    {
        Minimum,
        Small,
        Medium,
        Large,
        ExtraLarge,
        Supersized
    }

    public static async UniTask BeginDetonatorTimer(int milliseconds) => await UniTask.Delay(milliseconds);

    public static ICollection<int> GetDetonatedItemsData(int currentPoints)
    {
        ICollection<int> data = new List<int>();
        KeyValuePair<int, int> lossData = CalculatePointLoss(currentPoints);

        for (int i = 1; i < lossData.Key + 1; i++)
            data.Add(lossData.Value / lossData.Key);

        return data;
    }


    //This is calculation could definitely be done with more flexibility such as allowing for a minimum point level amount, different treasure amounts being given when detonated etc. However
    //for now this should be okay. Playtesting would definitely be needed to adjust these values
    public static KeyValuePair<int,int> CalculatePointLoss(int currentPoints)
    {
        PointLevel pointLevel = CalculatePointLevel(currentPoints);
        int loss = 0;
        
        switch (pointLevel)
        {
            case PointLevel.Minimum:
                loss =  currentPoints - currentPoints * 20 / 100;
                return new KeyValuePair<int, int>(1, loss);
            case PointLevel.Small:
                loss =  currentPoints - currentPoints * 25 / 100;
                return new KeyValuePair<int, int>(2, loss);
            case PointLevel.Medium:
                loss = currentPoints - currentPoints * 30 / 100;
                return new KeyValuePair<int, int>(2, loss);
            case PointLevel.Large:
                loss = currentPoints - currentPoints * 35 / 100;
                return new KeyValuePair<int, int>(3, loss);
            case PointLevel.ExtraLarge:
                loss = currentPoints - currentPoints * 40 / 100;
                return new KeyValuePair<int, int>(4, loss);
            case PointLevel.Supersized:
                loss = currentPoints - currentPoints * 45 / 100;
                return new KeyValuePair<int, int>(4, loss);
            default:
                loss = currentPoints;
                return new KeyValuePair<int, int>(1, loss);
        }
        
        // < 50 20% treasure amount : 1 
        //50   25%  treasure amount: 1
        //150  30%  treasure amount: 2
        //200  35%  treasure amount: 2
        //250  40%  treasure amount: 3
        //300  45%  treasure amount: 4
    }


    private static PointLevel CalculatePointLevel(int points)
    {
        return points switch
        {
            > 50 and < 149 => PointLevel.Small,
            > 150 and < 199 => PointLevel.Medium,
            > 200 and < 249 => PointLevel.Large,
            > 250 and < 299 => PointLevel.ExtraLarge,
            > 300 => PointLevel.Supersized,
            _ => PointLevel.Minimum // _ represents default
        };
    }
}