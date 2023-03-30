using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Fire : MonoBehaviourPun
{
    CharacterFA _owner;
    float timer = 0.85f;
    float currentTime;


    private void Update()
    {
        if(currentTime < timer)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            photonView.RPC("DestroyThis", RpcTarget.All);
        }
    }
    public Fire SetOwner(CharacterFA owner)
    {
        _owner = owner;
        return this;
    }
    [PunRPC]
    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
