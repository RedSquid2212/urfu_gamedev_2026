using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RollButton : MonoBehaviour
{
    private UnityEvent onRollRequested = new ();
    
    private Button button;
    
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleClick);
    }
    
    private void Update()
    {
        if (!button.interactable) return;
        if (Keyboard.current == null) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            EmitEvent();
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(HandleClick);
    }
    
    public void AddListener(UnityAction action) => onRollRequested.AddListener(action);
    public void RemoveListener(UnityAction action) => onRollRequested.RemoveListener(action);
    
    public void SetInteractable(bool value)
    {
        button.interactable = value;
    }

    private void HandleClick()
    {
        if (!button.interactable) return;
        
        EmitEvent();
    }

    private void EmitEvent()
    {
        onRollRequested.Invoke();
    }
}
