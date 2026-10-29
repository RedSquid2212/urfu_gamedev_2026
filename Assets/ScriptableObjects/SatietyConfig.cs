using UnityEngine;

[CreateAssetMenu(fileName = "SatietyConfig", menuName = "Scriptable Objects/SatietyConfig")]
public class SatietyConfig : ScriptableObject
{
    public int maxSatiety;
    public int hungerPerSecond;
    public int lowSatietyThreshold;
    public int hungerDamage;
    
    public int maxHydration;
    public int thirstPerSecond;
    public int lowHydrationThreshold;
    public int thirstDamage;
}
