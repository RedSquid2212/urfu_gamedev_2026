using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class RollButton : MonoBehaviour
{
    private UnityEvent onRollRequested = new ();
    
    private Button button;
    
    private UIDocument uiDocument;
    private VisualElement root;
    
    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        root = uiDocument.rootVisualElement;
        button = root.Q<Button>("rollButton");
        
        button.clicked += HandleClick;
        root.RegisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);
        root.focusable = true;
        root.Focus();
    }

    private void OnDisable()
    {
        if (button != null)
            button.clicked -= HandleClick;

        root?.UnregisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);
    }
    
    public void AddListener(UnityAction action) => onRollRequested.AddListener(action);
    public void RemoveListener(UnityAction action) => onRollRequested.RemoveListener(action);
    
    private void OnKeyDown(KeyDownEvent evt)
    {
        if (evt.keyCode != KeyCode.Space) return;
        if (button is not { enabledSelf: true }) return;

        EmitEvent();
        evt.StopPropagation();
    }

    private void HandleClick()
    {
        if (button is not { enabledSelf: true }) return;
        EmitEvent();
    }

    private void EmitEvent()
    {
        onRollRequested.Invoke();
    }
}
