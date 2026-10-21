using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    
    private Label scoreLabel;

    private IntegerField diceCountField;
    private IntegerField drawValueField;
    private IntegerField failValueField;
    private IntegerField victoryValueField;

    private Button rollButton;

    private UnityEvent onRollRequested = new ();
    private UnityEvent<Dictionary<string, int>> onFieldValuesEmitted = new ();

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        root = uiDocument.rootVisualElement;
        
        scoreLabel = root.Q<Label>("score");

        diceCountField = root.Q<TemplateContainer>("diceCountField").Q<IntegerField>();
        drawValueField = root.Q<TemplateContainer>("drawValueField").Q<IntegerField>();
        failValueField = root.Q<TemplateContainer>("failValueField").Q<IntegerField>();
        victoryValueField = root.Q<TemplateContainer>("victoryValueField").Q<IntegerField>();
        
        rollButton = root.Q<Button>("rollButton");
        rollButton.clicked += HandleClick;
    }

    private void OnDisable()
    {
        if (rollButton != null)
            rollButton.clicked -= HandleClick;
    }

    public void AddRollListener(UnityAction action) => onRollRequested.AddListener(action);
    public void RemoveRollListener(UnityAction action) => onRollRequested.RemoveListener(action);

    public void AddFieldValuesListener(UnityAction<Dictionary<string, int>> action) =>
        onFieldValuesEmitted.AddListener(action);
    public void RemoveFieldValuesListener(UnityAction<Dictionary<string, int>> action) =>
        onFieldValuesEmitted.RemoveListener(action);

    public void SetScore(int score)
    {
        scoreLabel.text = $"Score: {score}";
    }

    public void SetControlsInteractable(bool value)
    {
        diceCountField.SetEnabled(value);
        drawValueField.SetEnabled(value);
        failValueField.SetEnabled(value);
        victoryValueField.SetEnabled(value);
        rollButton.SetEnabled(value);
    }

    private void HandleClick()
    {
        if (rollButton is not { enabledSelf: true }) return;
        EmitFieldValues();
        EmitRollEvent();
    }
    
    private void EmitRollEvent()
    {
        onRollRequested.Invoke();
    }

    private void EmitFieldValues()
    {
        var values = new Dictionary<string, int>
            { 
                { "diceCount", diceCountField?.value ?? 0 },
                { "drawValue", drawValueField?.value ?? 0 },
                { "failValue", failValueField?.value ?? 0 },
                { "successValue", victoryValueField?.value ?? 0 }
            };
        
        onFieldValuesEmitted.Invoke(values);
    }
}
