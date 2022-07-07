using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CheatBase : MonoBehaviour
{
    public string CheatCodeString;
    public bool IsActive { get; private set; }
    public virtual bool CanDeactivate => true;

    private string _currentSequence = "";
    private float CleanupTime;


    private void OnGUI()
    {
        Event e = Event.current;

        if (e.isKey && e.type == EventType.KeyDown)
        {
            Cleanup(true);
            if (TryAddKey(e.keyCode) && CheckMatch()) 
                SwitchActivity();
        }
        else
            Cleanup(false);
    }


    private bool TryAddKey(KeyCode key)
    {
        var s = key.ToString();

        if (s.Length == 1 && s[0] >= 'A' && s[0] <= 'Z')
        {
            _currentSequence += key.ToString();
            return true;
        }
        return false;
    }

    private bool CheckMatch()
    {
        return _currentSequence.Contains(CheatCodeString.ToUpper());
    }

    private void Cleanup(bool keyPressed)
    {
        if (keyPressed)
        {
            CleanupTime = Time.unscaledTime;
        }
        else if (Time.unscaledTime - CleanupTime > 4)
        {
            CleanupTime = Time.unscaledTime;
            _currentSequence = "";
        }
    }


    public void SwitchActivity()
    {
        _currentSequence = "";

        if (IsActive && CanDeactivate)
            DeactivateCheat();
        else
            ActivateCheat();
    }

    protected virtual void DeactivateCheat() => IsActive = false;

    protected virtual void ActivateCheat() => IsActive = true;
}
