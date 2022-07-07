using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMaster : MonoBehaviour
{
    private List<EntityState> _currentState = new List<EntityState>();

    public List<EntityState> States = new List<EntityState>();
    public bool AutoStates = true;


    private void Start()
    {
        if (AutoStates)
            States.AddRange(GetComponentsInChildren<EntityState>());
    }

    private void ChangeCurrentState(EntityState[] newStates)
    {
        _currentState.ForEach(s => s.DeactivateState());
        _currentState.Clear();
        _currentState.AddRange(newStates);
        _currentState.ForEach(s => s.ActivateState());

        Debug.Log(_currentState.Select(s => s.ToString()).Aggregate((a, acc) => acc + a).ToString());
    }

    private bool CanChangeState(EntityState[] to)
    {
        if (to.Length == 0) return false;
        int newPriority = to.Min(s => s.Priority);
        if (to.Any(s => s.Priority != newPriority)) return false;

        if (_currentState.Count == 0) return true;
        int currPriority = _currentState.First().Priority;

        return newPriority > currPriority || _currentState.Any(s => !s.IsAvailable);
    }

    private void Update()
    {
        if (States.Count == 0) return;

        int priority = States.Where(s => s.IsAvailable).Max(s => s.Priority);

        EntityState[] targetState = States
            .Where(s => s.IsAvailable && s.Priority == priority)
            .ToArray();

        if (CanChangeState(targetState))
            ChangeCurrentState(targetState);
    }
}