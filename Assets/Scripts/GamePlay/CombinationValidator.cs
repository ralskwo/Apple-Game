using System.Collections.Generic;
using UnityEngine;

public class CombinationValidator : MonoBehaviour
{
    void Awake()
    {
        ServiceLocator.Register<CombinationValidator>(this);
    }

    public bool CheckCombination(List<Apple> apples)
    {
        int sum = 0;
        foreach (var apple in apples)
        {
            sum += apple.Value;
        }
        return sum == 10;
    }
}
