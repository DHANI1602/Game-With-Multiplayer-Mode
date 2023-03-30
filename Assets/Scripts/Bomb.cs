using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bomb : MonoBehaviourPun
{
    public float timer, currentTime;
    public GameObject fire;
    public int firePower;
    CharacterFA _owner;

    private void Start()
    {
        firePower++;
    }
    void Update()
    {
        if (!photonView.IsMine) return;

        if(currentTime < timer)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            Boom();
            PhotonNetwork.Destroy(gameObject);
        }
     
    }

    public Bomb SetOwner(CharacterFA owner)
    {
        _owner = owner;
        return this;
    }
    void Boom()
    {
        PhotonNetwork.Instantiate(fire.name, transform.position, transform.rotation)
                                         .GetComponent<Fire>()
                                        .SetOwner(_owner);

        for (int x = 1; x < firePower; x++)
        {
            PhotonNetwork.Instantiate(fire.name, transform.position + new Vector3(x, 0), Quaternion.identity)
                                        .GetComponent<Fire>()
                                        .SetOwner(_owner);
            PhotonNetwork.Instantiate(fire.name, transform.position - new Vector3(x, 0), Quaternion.identity)
                                        .GetComponent<Fire>()
                                        .SetOwner(_owner);
        }
        for (int y = 1; y < firePower; y++)
        {
            PhotonNetwork.Instantiate(fire.name, transform.position + new Vector3(0, y), Quaternion.identity)
                                         .GetComponent<Fire>()
                                        .SetOwner(_owner);
            PhotonNetwork.Instantiate(fire.name, transform.position - new Vector3(0, y), Quaternion.identity)
                                         .GetComponent<Fire>()
                                        .SetOwner(_owner);
        }
    }

}
