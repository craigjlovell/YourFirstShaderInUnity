using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionList : MonoBehaviour
{
    public UnityEvent onChanged;
    Action[] _actions = null;
    public Action[] actions
    {
        get 
        { 
            if(_actions == null)
                _actions = GetComponents<Action>();
            return _actions; 
        }
    }

    [ContextMenu("Delete First")]
    void DeleteFirst()
    {
        List<Action> actions = new List<Action>(_actions);
        actions.RemoveAt(0);
        _actions = actions.ToArray();
        onChanged.Invoke();
    }
}
