using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;


public class LobbyNetworkManager : MonoBehaviourPunCallbacks
{
    /*[SerializeField] LobbySceneManager m_lobbySceneManager;

    [Header("[Get RoomMenu]")]
    [SerializeField] Button m_startBtn;

    int m_gameVersion = 1;
    public bool m_isRoomCreated { get; private set; } = false; // �� ���� ����, �� ���� �� �ʱ�ȭ �ϱ�

    public string m_PlayerName { get; private set; }
    public string m_CurrentRoomName { get; private set; }
    public Player[] m_CurrentJoinPlayers { get; private set; }

    private void Awake()
    {
        m_lobbySceneManager = GetComponent<LobbySceneManager>();
        InitializeAtStart();
    }

    private void Start()
    {
        //LobbySceneManager.Instance.InitializeAtStart += InitializeAtStart;
    }

    public void InitializeAtStart()
    {
        // ���� ���� ����
        PhotonNetwork.GameVersion = m_gameVersion.ToString();
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // ������ ����Ǿ��� �� ȣ��Ǵ� �޼���
    *//*public override void OnConnectedToMaster()
    {
        // ��Ʈ������ ó���ϱ��
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true; // ������ Ŭ���̾�Ʈ�� ���� �����ϸ� �ٸ� Ŭ���̾�Ʈ�鵵 �ڵ����� ����ȭ��
    }*//*

    // ������ ������ �������� �� ȣ��Ǵ� �޼���
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Disconnected from Server: " + cause.ToString());
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnRoomCreate(string roomName, string playerName)
    {
        PhotonNetwork.CreateRoom(roomName);
        PhotonNetwork.NickName = playerName;
        
        m_isRoomCreated = true;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to create room: " + message);
        m_isRoomCreated = false;
    }

    public bool CheckRoomCreated(string name)
    {
        return m_isRoomCreated; // �� ���� ���� ���� ��ȯ
    }

    // �� ���� Ȥ�� JoinLobby �� ȣ��
    public override void OnJoinedRoom()
    {
        // ���� �濡 ������ �ִ� ���� ���� ��� �÷��̾��
        // �����ÿ��� �̹� ����Ʈ�� �� �ֱ⿡ �ٽ� �������� ����
        Player[] players = PhotonNetwork.PlayerList;
        
        if(players.Length != 0 || players != null)
        {
            foreach (Player player in players)
            {
                m_lobbySceneManager.OnJoinedRoom(player.NickName);
            }
        }

        // �� �̸� �޾ƿ���
        m_startBtn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void OnJoinRoom(string roomName, string playerName)
    {
        PhotonNetwork.JoinRoom(roomName);
        PhotonNetwork.NickName = playerName;
    }

    // �κ񿡼� �� ����� ������Ʈ�Ǿ��� �� ȣ��Ǵ� �޼���
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            // ���ŵ� ��������
            m_lobbySceneManager.OnRoomListUpdate(roomInfo.RemovedFromList, roomInfo.Name);
        }
    }

    // �÷��̾ �濡 �������� �� ���游 ȣ��
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // ���� �÷��̾����� ���濡�� ����
        m_lobbySceneManager.OnPlayerEnterted(newPlayer.NickName);
    }

    public void OnRoomLeave()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.JoinLobby();
        }
    }

    // ������ ���� ������ �� ȣ��
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        m_lobbySceneManager.OnPlayerLeftRoom(otherPlayer.NickName);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        m_startBtn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void SaveInfo()
    {
        m_PlayerName = PhotonNetwork.NickName;
        m_CurrentRoomName = PhotonNetwork.CurrentRoom.Name;
        m_CurrentJoinPlayers = PhotonNetwork.PlayerList;
    }*/
}
