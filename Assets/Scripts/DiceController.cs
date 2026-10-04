using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    [SerializeField] private Dice dicePrefab;
    [SerializeField] private int diceCount = 2;
    [SerializeField] private RollButton rollButton;
    [SerializeField] private Counter score;

    private int throwForce = 3;
    private int upwardBias = 2;
    private int torqueForce = 4;

    private readonly List<Dice> dice = new ();
    private bool isRolling;

    private void Awake()
    {
        for (var i = 0; i < diceCount; i++)
        {
            var angle = i * Mathf.PI * 2 / diceCount;
            var offset = new Vector3(Mathf.Cos(angle), 1, Mathf.Sin(angle)) * 2;

            var d = Instantiate(dicePrefab, transform.position + offset, Random.rotation, transform);
            dice.Add(d);
        }

        rollButton.AddListener(StartRoll);
    }

    private void OnDestroy()
    {
        rollButton.RemoveListener(StartRoll);
    }

    private void StartRoll()
    {
        if (isRolling) return;
        StartCoroutine(RollRoutine());
    }

    private IEnumerator RollRoutine()
    {
        isRolling = true;
        rollButton.SetInteractable(false);

        foreach (var d in dice)
        {
            d.ResetToStart();

            var force = new Vector3(
                Random.Range(-1, 1),
                upwardBias,
                Random.Range(-1, 1)).normalized * throwForce;

            d.Throw(force, Random.insideUnitSphere * torqueForce);
        }

        yield return new WaitUntil(AllDiceStopped);

        var sum = dice.Sum(d => d.LastValue);
        score.SetScore(sum);

        isRolling = false;
        rollButton.SetInteractable(true);
    }

    private bool AllDiceStopped()
    {
        return dice.All(d => !d.IsRolling);
    }
}
