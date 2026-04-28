using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;
    public int moneyRequired;
    public int woodRequired;
    public int meatRequired;

    public bool IsCompleted(int wood, int money, int meat)
    {
        return wood >= woodRequired && money >= moneyRequired && meat >= meatRequired;
    }

}
