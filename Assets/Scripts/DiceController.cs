using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    [SerializeField] private Dice dicePrefab;
    [SerializeField] private UIController uiController;
    
    private int diceCount = 2;
    private int drawValue = 1;
    private int failValue = 1;
    private int successValue = 1;

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

            var dice = Instantiate(dicePrefab, transform.position + offset, Random.rotation, transform);
            dices.Add(dice);
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

        foreach (var dice in dices)
        {
            dice.ResetToStart();

            var force = new Vector3(
                Random.Range(-1, 1),
                upwardBias,
                Random.Range(-1, 1)).normalized * throwForce;

            dice.Throw(force, Random.insideUnitSphere * torqueForce);
        }

        yield return new WaitUntil(AllDiceStopped);

        var sum = dices.Sum(d => d.LastValue);
        uiController.SetScore(sum);
        ComputeResult(sum);

        isRolling = false;
        uiController.SetControlsInteractable(true);
    }

    private void ComputeResult(int sum)
    {
        var resultText = sum >= successValue ?
            "Success" :
            (drawValue <= sum && sum < successValue) ?
                "Draw" : (failValue <= sum && sum < drawValue) ? "Fail" : "Epic fail";
        
        var resultColorClass = resultText switch
        {
            "Success" => "success",
            "Draw" => "draw",
            _ => "fail"
        };
        
        uiController.SetResult(resultText, resultColorClass);
    }

    private bool AllDiceStopped()
    {
        return dices.All(dice => !dice.IsRolling);
    }
}
