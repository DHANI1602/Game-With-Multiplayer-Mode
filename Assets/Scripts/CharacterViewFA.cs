using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharacterViewFA : MonoBehaviourPun
{
    Animator _anim;
    SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }
}
