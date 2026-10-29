using UnityEngine;

[CreateAssetMenu(fileName = "EnergyConfig", menuName = "Scriptable Objects/EnergyConfig")]
public class EnergyConfig : ScriptableObject
{
    public int maxEnergy;
    public int fatiguePerSecond;
    public int lowEnergyThreshold;
    public int fatigueDamage;
}
