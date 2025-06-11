using System;
using System.Collections.Generic;
using DUS.AssetLoad;
using DUS.Network;
using DUS.scene;
using DUS.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbySceneManager : MonoBehaviour
{
    public static LobbySceneManager Instance { get; private set; } // �̱��� �ν��Ͻ�

    [SerializeField]
    GameObject UICanvas;

    public NextSceneRequireData m_nextSceneRequireData;
    // =========================== ���
    private LobbyMenuUIManager m_lobbyMenuUIManager;
    private RoomListManager m_roomListManager;
    private PlayerListUIManager m_playerListManager;

    public event Action InitializeAtStart;
    
    private INetworkService m_networkService;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject); // �ߺ� �ν��Ͻ� ����
            return;
        }

        m_lobbyMenuUIManager = UICanvas.GetComponent<LobbyMenuUIManager>();
        m_roomListManager = UICanvas.GetComponent<RoomListManager>();
        m_playerListManager = UICanvas.GetComponent<PlayerListUIManager>();

        m_networkService = NetworkManager.Service;
    }


    private void Start()
    {
        InitializeAtStart?.Invoke();

        m_networkService.UpdateRoomList += UpdateFindMenuRoomList;
        m_networkService.JoinedPlayerList += JoinedPlayerList;
        m_networkService.EntertedPlayer += EntertedPlayer;
        m_networkService.LeftPlayer += LeftPlayer;
        m_networkService.ActivateStartInGame += ActivateStartInGame;
    }
    public void StartInGame(int sceneNum)
    {
        SceneLoadManager.Instance.LoadNextScene_Boot(SceneType.InGame, m_nextSceneRequireData);
    }

    #region ======================================================================= Button Ŭ��
    // �޴� �̵�
    public void GoMenuUI(GoMenuUIType mainMenuType)
    {
        m_lobbyMenuUIManager.GoMenuUI(mainMenuType);
    }

    // �� ����
    // TODO : ���� �ߺ� ���� ���� �ʿ�
    public void CreateRoom()
    {
        // 1. �� �̸��� �� �г��� ��������
        string _roomName = m_lobbyMenuUIManager.GetInputFieldRoomName();
        string _playerName = m_lobbyMenuUIManager.GetInputFieldPlayerName();

        // 2. ������ ��Ʈ��ũ �� ����(���� �� ���� ��� ��������� ����)
        m_networkService.CreateRoom(_roomName, _playerName);

        // 3. �� ���� �Ǿ����� üũ. �ȵǾ��� �� 3���������� �ȸ�������� �α�
        float time = 0;
        while (!m_networkService.CheckCreateRoom())
        {
            time += Time.deltaTime;
            if(time >= 3)
            {
                Debug.LogError("CreateRoomFailed");
            }
            m_networkService.CreateRoom(_roomName, _playerName);
        }

        // 4 �� �޴�UI�� �̵�
        GoMenuUI(GoMenuUIType.RoomMenuUI);

        // 5. �� �޴� ���� �� ����
        // 5.1 �� �޴��� �� �̸� ���� ������ �̸� �޾ƿ� �� ������ ���� ó���� �ڵ� �Ϸ�        
        // 5.2 �÷��̾� ����Ʈ ������ ��Ʈ��ũ �Ŵ����� JoinedPlayerList�� ���� �ڵ� ����

        // 6 FindMenu�� �� ���� ��ư ����
        m_roomListManager.CreateJoinRoomBtn(_roomName);

        // 7 �ΰ��ӽ�ŸƮ ��ư Ȱ��ȭ
        ActivateStartInGame(true);  //StartBtn�� ���常 ��������
    }

    public void ActivateStartInGame(bool isActivate)
    {
        m_lobbyMenuUIManager.ActivateStartInGameBtn(isActivate);
    }
    // �� ����
    public void OnJoinRoom(string roomName)
    {
        // 1. Join�� �� ��������, �÷��̾� �̸�
        string _roomName = roomName;
        string _playerName = m_lobbyMenuUIManager.GetInputFieldPlayerName();

        // 2. ��Ʈ��ũ ����
        m_networkService.JoinRoom(_roomName, _playerName);

        // 3. �� �̸� ����
        m_lobbyMenuUIManager.SetRoomMenuRoomName(_roomName);

        ActivateStartInGame(false);
    }

    public void JoinLobby()
    {
        m_networkService.JoinLobby();
    }

    public void LeaveRoom()
    {
        // TODO : �÷��̾� ����Ʈ�� ���� �ʱ�ȭ�ʿ�

        // 1. �÷��̾� ����Ʈ ����
        m_playerListManager.AllRemove();

        m_networkService.LeaveRoom();
    }
    #endregion ======================================================================= /Button Ŭ��

    // FindMenu �� ����Ʈ ������Ʈ
    public void UpdateFindMenuRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) m_roomListManager.RemoveRoomList(roomInfo.Name);
            else m_roomListManager.CreateJoinRoomBtn(roomInfo.Name);
        }
    }

    // CreateRoom + JoinRoom �� ȣ�� (�����׸� ����, �濡 �ִ� ����� ���������� �𸣴� ���� ���� �������� �˷�����Ѵ� EntertedPlayer)
    public void JoinedPlayerList(Player[] players)
    {
        m_playerListManager.CreatePlayerListItem(players);
    }

    // ���� �濡�ְ� ���� ���� �÷��̾� �ν�
    public void EntertedPlayer(Player entertedPlayer)
    {
        m_playerListManager.AddPlayerListItem(entertedPlayer);
    }

    // Leave�� ���� ���� ���� �����ִ� �����鿡�� ���� ���� ���� ���� 
    public void LeftPlayer(Player player)
    {
        m_playerListManager.RemovePlayer(player);
    }
}