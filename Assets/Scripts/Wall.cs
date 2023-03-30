using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Wall : MonoBehaviourPun
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine) return;
        if (collision.gameObject.layer == 11)
        {

            photonView.RPC("DestroyThis", RpcTarget.All);
        }
     
    }
    [PunRPC]
    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }

}
