using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*EL VIEW*/

public class CharViewFA : MonoBehaviour
{
    Animator _anim;
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void SetVel(float value)
    {
        if (_anim)
        {
            _anim.SetFloat("Vel", value);
        }
    }
}
