using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ControllerFA : MonoBehaviourPun
{
    Player _localPlayer;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        _localPlayer = PhotonNetwork.LocalPlayer;

        StartCoroutine(SendPackages());
    }

    IEnumerator SendPackages()
    {
        while (true)
        {

            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");

            if (h != 0 || v != 0)
            {
                var dir = new Vector3(h, v, 0);
                MyServer.Instance.RequestMove(_localPlayer, dir);
                if (h < 0 || v < 0)
                    MyServer.Instance.RequestAnim(_localPlayer, true, -1);
                else if (h > 0 || v > 0)
                    MyServer.Instance.RequestAnim(_localPlayer, true, 1);
            }
            else
            {
                MyServer.Instance.RequestAnim(_localPlayer, false, 0);
            }


            yield return new WaitForSeconds(1 / MyServer.Instance.PackagesPerSecond);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MyServer.Instance.RequestBomb(_localPlayer);

        }
    }
}

