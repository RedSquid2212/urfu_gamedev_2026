using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    [SerializeField] private Dice dicePrefab;
    [SerializeField] private UIController uiController;
    
    private int diceCount = 2;
    private int drawValue = 0;
    private int failValue = 0;
    private int successValue = 0;

    private int throwForce = 3;
    private int upwardBias = 2;
    private int torqueForce = 4;

    private readonly List<Dice> dices = new ();
    private bool isRolling;

    private void Awake()
    {
        uiController.AddFieldValuesListener(SetParams);
        uiController.AddRollListener(StartRoll);
    }

    private void OnDestroy()
    {
        uiController.RemoveRollListener(StartRoll);
        uiController.RemoveFieldValuesListener(SetParams);
    }
    
    private void SyncDiceCount()
    {
        while (dices.Count < diceCount)
        {
            var angle = dices.Count * Mathf.PI * 2 / diceCount;
            var offset = new Vector3(Mathf.Cos(angle), 1f, Mathf.Sin(angle)) * 2f;

            var d = Instantiate(dicePrefab, transform.position + offset, Random.rotation, transform);
            dices.Add(d);
        }

        while (dices.Count > diceCount)
        {
            var last = dices.Count - 1;
            Destroy(dices[last].gameObject);
            dices.RemoveAt(last);
        }
    }

    private void SetParams(Dictionary<string, int> parameters)
    {
        diceCount = Mathf.Max(1, parameters["diceCount"]);
        drawValue = parameters["drawValue"];
        failValue = parameters["failValue"];
        successValue = parameters["successValue"];
    }

    private void StartRoll()
    {
        if (isRolling) return;
        StartCoroutine(RollRoutine());
    }

    private IEnumerator RollRoutine()
    {
        isRolling = true;
        uiController.SetControlsInteractable(false);

        SyncDiceCount();

        foreach (var d in dices)
        {
            d.ResetToStart();

            var force = new Vector3(
                Random.Range(-1, 1),
                upwardBias,
                Random.Range(-1, 1)).normalized * throwForce;

            d.Throw(force, Random.insideUnitSphere * torqueForce);
        }

        yield return new WaitUntil(AllDiceStopped);

        var sum = dices.Sum(d => d.LastValue);
        uiController.SetScore(sum);

        isRolling = false;
        uiController.SetControlsInteractable(true);
    }

    private bool AllDiceStopped()
    {
        return dices.All(d => !d.IsRolling);
    }
}
