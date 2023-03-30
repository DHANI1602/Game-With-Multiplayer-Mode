using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MyServer : MonoBehaviourPun
{
    public static MyServer Instance;
    bool canPlay = false;
    Player _server;
    public int ID;
    public CharacterFA[] characterPrefabs;
    float timer = 10;
    float currentTime;
    bool activeGame = false;


    Dictionary<Player, CharacterFA> _dicModels = new Dictionary<Player, CharacterFA>();
    Dictionary<Player, CharacterViewFA> _dicViews = new Dictionary<Player, CharacterViewFA>();
    public int PackagesPerSecond { get; private set; }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        if (Instance == null)
        {
            if (photonView.IsMine)
            {
                photonView.RPC("SetServer", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer, 1);
            }
        }
    }
    private void Update()
    {
        if (PhotonNetwork.PlayerList.Length > 4)
        {

            activeGame = true;


        }


        if (activeGame)
        {

            if (PhotonNetwork.PlayerList.Length < 3)
            {
                photonView.RPC("PlayerVictory", RpcTarget.Others);
                Debug.Log(PhotonNetwork.PlayerList.Length);
            }
        }

    }

    [PunRPC]
    void SetServer(Player serverPlayer, int sceneIndex = 1)
    {
        if (Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        _server = serverPlayer;

        PackagesPerSecond = 60;

        PhotonNetwork.LoadLevel(sceneIndex);

        var playerLocal = PhotonNetwork.LocalPlayer;

        if (playerLocal != _server)
        {
            photonView.RPC("AddPlayer", _server, playerLocal);
        }
    }

    [PunRPC]
    void AddPlayer(Player player)
    {
        StartCoroutine(WaitForLevel(player));
    }

    IEnumerator WaitForLevel(Player player)
    {
        while (PhotonNetwork.LevelLoadingProgress > 0.9f)
        {
            yield return new WaitForEndOfFrame();
        }
        if (ID == 0)
        {
            CharacterFA Character = PhotonNetwork.Instantiate(characterPrefabs[0].name, new Vector3(-7.452718f, 4.154998f, 0), Quaternion.identity)
                                       .GetComponent<CharacterFA>().SetInitialParameters(player);
            _dicModels.Add(player, Character);
            _dicViews.Add(player, Character.GetComponent<CharacterViewFA>());


        }
        else if (ID == 1)
        {
            CharacterFA Character1 = PhotonNetwork.Instantiate(characterPrefabs[1].name, new Vector3(7.41f, 3.26f, 0), Quaternion.identity)
                                       .GetComponent<CharacterFA>().SetInitialParameters(player);
            _dicModels.Add(player, Character1);
            _dicViews.Add(player, Character1.GetComponent<CharacterViewFA>());

        }
        else if (ID == 2)
        {
            CharacterFA Character2 = PhotonNetwork.Instantiate(characterPrefabs[2].name, new Vector3(7.409997f, -3.508111f, 0), Quaternion.identity)
                                       .GetComponent<CharacterFA>().SetInitialParameters(player);
            _dicModels.Add(player, Character2);
            _dicViews.Add(player, Character2.GetComponent<CharacterViewFA>());
        }
        else
        {
            CharacterFA Character3 = PhotonNetwork.Instantiate(characterPrefabs[3].name, new Vector3(-7.452718f, -3.44f, 0), Quaternion.identity)
                                       .GetComponent<CharacterFA>().SetInitialParameters(player);
            _dicModels.Add(player, Character3);
            _dicViews.Add(player, Character3.GetComponent<CharacterViewFA>());

        }
        ID++;
    }

    public void RequestMove(Player player, Vector3 dir)
    {
        if (activeGame)
        {

            photonView.RPC("Move", _server, player, dir);
        }
    }

    public void RequestBomb(Player player)
    {
        if (activeGame)
        {

            photonView.RPC("DropBomb", _server, player);
        }
    }

    public void RequestAnim(Player player, bool set, int Menor)
    {
        photonView.RPC("PlayAnim", _server, player, set, Menor);
    }

    public void PlayerDisconnect(Player player)
    {
        PhotonNetwork.Destroy(_dicModels[player].gameObject);
        _dicModels.Remove(player);
        _dicViews.Remove(player);
    }


    [PunRPC]
    void Move(Player player, Vector3 dir)
    {
        if (_dicModels.ContainsKey(player))
        {
            _dicModels[player].Move(dir, 2);

        }
    }

    [PunRPC]
    void DropBomb(Player player)
    {
        if (_dicModels.ContainsKey(player))
        {
            _dicModels[player].DropBomb();
        }
    }

    [PunRPC]
    void PlayAnim(Player player, bool moving, int dir)
    {
        if (_dicModels.ContainsKey(player))
        {
            _dicModels[player].SetAnim(moving, dir);
        }
    }
    [PunRPC]
    void PlayerVictory()
    {
        SceneManager.LoadScene("Victory");
        PhotonNetwork.Disconnect();
    }
}
