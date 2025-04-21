using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SkinItem : UIMonoBehaviour
{
    public event Action<SkinItem> OnClicked;

    [SerializeField] private int _skinId;
    [SerializeField] private string _conditionSphereName;
    [SerializeField] private State _currentState;
    [SerializeField] private List<VisualState> _visualStates;

    public int Id => _skinId;
    public string ConditionSphereName => _conditionSphereName;

    #region Custom types
    [Serializable]
    public struct VisualState
    {
        public State State;
        public List<GameObject> Visuals;
    }

    public enum State
    {
        LOCKED,
        UNLOCKED,
        SELECTED
    }
    #endregion

    public void SetState(State state)
    {
        _currentState = state;
        _visualStates.ForEach(vs => vs.Visuals.ForEach((go) => go.SetActive(false)));
        _visualStates.First(vs => vs.State == state).Visuals.ForEach(go => go.SetActive(true));
    }

    public void OnItemClick()
    {
        if (_currentState == State.LOCKED)
            return;

        OnClicked?.Invoke(this);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        SetState(_currentState);
    }
#endif
}
