using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class CharacterFA : MonoBehaviourPun, IPunObservable
{
    Player _owner;
    Animator _anim;
    public float speed;
    bool dropped = false;
    public Bomb bombPrefab;
    Rigidbody2D _rb;
    SpriteRenderer sprite;


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }


    public void Move(Vector3 dir, float speed)
    {
        _rb.velocity = dir * speed;


    }
    public void DropBomb()
    {
        PhotonNetwork.Instantiate(bombPrefab.name, transform.position, transform.rotation)
                 .GetComponent<Bomb>()
                 .SetOwner(this);

    }
    public void SetAnim(bool set, int Mayor)
    {


        if (set == true && Mayor > 0)
        {
            _anim.SetBool("Moving", true);
            sprite.flipX = false;
        }
        else if (set == true && Mayor < 0)
        {
            _anim.SetBool("Moving", true);
            sprite.flipX = true;
        }
        else
        {
            _anim.SetBool("Moving", false);

        }

    }

    public CharacterFA SetInitialParameters(Player localPlayer)
    {
        _owner = localPlayer;

        return this;
    }
    [PunRPC]
    public void TakeDmg(Player player)
    {
        _anim.SetTrigger("Damage");
        if (PhotonNetwork.LocalPlayer == player)
        {
            SceneManager.LoadScene("Lose");
            PhotonNetwork.Disconnect();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            if (photonView.IsMine)
                photonView.RPC("TakeDmg", RpcTarget.All, _owner);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(sprite.flipX);
        }
        else
        {
            sprite.flipX = (bool)stream.ReceiveNext();
        }
    }
}
