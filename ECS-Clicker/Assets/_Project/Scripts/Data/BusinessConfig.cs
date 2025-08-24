using UnityEngine;

[System.Serializable]
public struct UpgradeConfig
{
    public double Price;
    public float IncomeMultiplierPercent;
}


[CreateAssetMenu(fileName = "NewBusinessConfig", menuName = "Clicker/Business Config")]
public class BusinessConfig : ScriptableObject
{

    public float IncomeDelay;
    public double BaseCost;
    public double BaseIncome;
    public string BusinessId;
    public int InitialLevel;
    public UpgradeConfig Upgrade1;
    public UpgradeConfig Upgrade2;

}