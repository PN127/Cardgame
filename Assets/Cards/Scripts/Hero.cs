using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField, Min (1)]
    private int _health;

    public void ChangeHealth(int value)
    {
        _health += value;
    
        if (_health < 0)
        {
            UnityEditor.EditorApplication.isPaused = true;
        }
    }

    

}
