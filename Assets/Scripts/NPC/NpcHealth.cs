using System.Collections;
using UnityEngine;

public class NpcHealth : MonoBehaviour
{
    [SerializeField] private HealthConfig healthConfig;
    [SerializeField] private EnergyConfig energyConfig;
    [SerializeField] private SatietyConfig satietyConfig;
    [SerializeField] private PhysicalStatsConfig physicalStatsConfig;

    private int hp;
    private int energy;
    private int satiety;
    private int hydration;
    private int velocity;

    private bool isAlive = true;
    
    public int Hp
    {
        get => hp;
        set => hp = Mathf.Clamp(value, 0, healthConfig.maxHp);
    }

    public int Energy
    {
        get => energy;
        set
        {
            energy = Mathf.Clamp(value, 0, energyConfig.maxEnergy);
            if (energy < energyConfig.lowEnergyThreshold)
            {
                StartCoroutine(DecreaseVelocity());
            }
        }
    }

    public int Satiety
    {
        get => satiety;
        set
        {
            satiety = Mathf.Clamp(value, 0, satietyConfig.maxSatiety);
            if (satiety < satietyConfig.lowSatietyThreshold)
            {
                StartCoroutine(TakeHungerDamage());
            }
        }
    }

    public int Hydration
    {
        get => hydration;
        set
        {
            hydration = Mathf.Clamp(value, 0, satietyConfig.maxHydration);
            if (hydration < satietyConfig.lowHydrationThreshold)
            {
                StartCoroutine(TakeThirstDamage());
            }
        }
    }

    public int Velocity
    {
        get => velocity;
        set => velocity = Mathf.Clamp(value, 0, physicalStatsConfig.maxVelocity);
    }
    
    private void Awake()
    {
        Hp = healthConfig.maxHp;
        Energy = energyConfig.maxEnergy;
        Satiety = satietyConfig.maxSatiety;
        Hydration = satietyConfig.maxHydration;
        Velocity = physicalStatsConfig.maxVelocity;
        
        StartCoroutine(Live());
    }
    
    public void Heal(int value) => Hp += value;

    public void Feed(int value) => Satiety += value;
    
    public void Drink(int value) => Hydration += value;
    
    public void IncreaseEnergy(int value) => Energy += value;
    
    public void IncreaseVelocity(int value) => Velocity += value;

    private IEnumerator Live()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(1);
            DecreaseSatiety(satietyConfig.hungerPerSecond);
            DecreaseHydration(satietyConfig.thirstPerSecond);
            DecreaseEnergy(energyConfig.fatiguePerSecond);
        }
    }

    private void TakeDamage(int value)
    {
        Hp -= value;
    }

    private IEnumerator TakeHungerDamage()
    {
        while (Satiety < satietyConfig.lowSatietyThreshold)
        {
            TakeDamage(satietyConfig.hungerDamage);
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator TakeThirstDamage()
    {
        while (Hydration < satietyConfig.lowHydrationThreshold)
        {
            TakeDamage(satietyConfig.thirstDamage);
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator DecreaseVelocity()
    {
        while (Energy < energyConfig.lowEnergyThreshold)
        {
            Velocity -= energyConfig.fatigueDamage;
            yield return new WaitForSeconds(1);
        }
    }

    private void DecreaseSatiety(int value)
    {
        Satiety -= value;
    }

    private void DecreaseHydration(int value)
    {
        Hydration -= value;
    }

    private void DecreaseEnergy(int value)
    {
        Energy -= value;
    }
}
