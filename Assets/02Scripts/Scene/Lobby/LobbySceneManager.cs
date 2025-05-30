// ������ �÷��̾��Ʈ�� �޾ƿ��� �ܰ� - ������ �����Ҷ� ���ٵ�

// 0529 ������ ������ �÷��̾��Ʈ �����̾ȵ�, �� ������ �� ���� ��� ���� �۾�x

using System;
using DUS.Scene;
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

    // =========================== ���
    private LobbyMenuUIManager m_lobbyMenuUIManager;
    private FindMenuRoomListManager m_findMenuRoomListManager;
    private PlayerListUIManager m_playerListManager;
    private LobbyNetworkManager m_networkManager;

    public event Action InitializeAtStart;

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
        m_findMenuRoomListManager = UICanvas.GetComponent<FindMenuRoomListManager>();
        m_playerListManager = UICanvas.GetComponent<PlayerListUIManager>();

        m_networkManager = GetComponent<LobbyNetworkManager>();
    }

    private void Start()
    {
        InitializeAtStart?.Invoke();
    }
    public void StartInGame(int sceneNum)
    {
        m_networkManager.SaveInfo();
        SaveNetworkInfo.Instance.SaveInfo(m_networkManager.m_PlayerName, m_networkManager.m_CurrentRoomName, m_networkManager.m_CurrentJoinPlayers);
        SceneLoadManager.Instance.PushNextInfoToBootSceneAndLoadBootScene(SceneType.InGame);
    }

    // �� ��ư�� OnClick�� ����
    public void GoMenuUI(GoMenuUIType mainMenuType)
    {
        m_lobbyMenuUIManager.GoMenuUI(mainMenuType);
    }

    // �� ���� ��
    public void OnRoomCreate()
    {
        string roomName = m_lobbyMenuUIManager.GetInputFieldRoomName();
        string playerName = m_lobbyMenuUIManager.GetInputFieldPlayerName();
        m_lobbyMenuUIManager.SetInputFieldPlayerName(playerName);

        m_networkManager.OnRoomCreate(roomName, playerName);

        // �� ���� ���н�
        if (!m_networkManager.m_isRoomCreated)
        {
            return;
            //FailedRoomCrate();
        }
        
        m_playerListManager.OnRoomCreate(playerName);
        m_findMenuRoomListManager.OnRoomCreate(roomName);
    }

    // ���� �� ��Ȳ
    public void OnJoinRoom(string roomName)
    {
        string playerName = m_lobbyMenuUIManager.GetInputFieldPlayerName();
        m_lobbyMenuUIManager.SetRoomMenuNameText(roomName);
        m_networkManager.OnJoinRoom(roomName, playerName);
    }

    // �濡 ���ӵ� �÷��̾��(���� ���������� ���������δ� )
    public void OnJoinedRoom(string playerName)
    {
        m_playerListManager.OnJoinedRoom(playerName);
    }

    public void OnRoomLeave()
    {
        m_networkManager.OnRoomLeave();

        m_findMenuRoomListManager.OnRoomLeave();
        m_playerListManager.OnRoomLeave();

        m_lobbyMenuUIManager.GoMenuUI(GoMenuUIType.MainMenuUI);
    }

    public void OnPlayerLeftRoom(string leftName)
    {
        string leftPlayerName = leftName;
        m_playerListManager.OnPlayerLeftRoom(leftPlayerName);
    }

    // ���ſ� ����
    public void OnRoomListUpdate(bool isRemove, string roomName)
    {
        m_findMenuRoomListManager.OnRoomUpdateList(isRemove, roomName);
    }

    public void OnPlayerEnterted(string newplayerName)
    {
        m_playerListManager.EntertedPlayer(newplayerName);
    }

    // �÷��̾��� �г��� ���� �ʿ�
    // �г��� �ο��Ǵ� �ñ� ����(�� ���� Ŭ�� ��, ���� ��ư Ŭ�� ��)
    /*public void OnRoomCreate(string roomName)
    {
        if (roomName == "" || string.IsNullOrEmpty(roomName))
        {
            roomName = "RoomMenuUI" + UnityEngine.Random.Range(1000, 9999).ToString(); // �� �̸��� ���� ��� ���� ���� ����
        }

        LobbyMenuUIManager.Instance.OnRoomCreate(roomName);

        string playerNickName = LobbyMenuUIManager.Instance.GetPlayerNickNameTextEnterRoom();

        LobbyNetworkManager.Instance.OnRoomCreate(roomName, playerNickName);

    }


    // ������ �� ������ ��
    public void OnRoomLeave()
    {
        LobbyMenuUIManager.Instance.OnRoomLeave(PhotonNetwork.CurrentRoom.roomName);
        LobbyNetworkManager.Instance.OnRoomLeave(PhotonNetwork.CurrentRoom.roomName);
    }

    // ������ ���� ������ �� ȣ��
    public void OnPlayerLeftedRoom(string leftPlayerName)
    {
        LobbyMenuUIManager.Instance.OnPlayerListItemRemove(leftPlayerName);
    }
    // LobbyNetworkManager���� �� ���� Ȥ�� ����� ����� �޼���
    public void AddPlayerListWhenJoined(string createPlayerList)
    {
        LobbyMenuUIManager.Instance.OnPlayerListItemCreate(createPlayerList);
    }

    public void OnRoomRemove(string removeRoomName)
    {
        LobbyMenuUIManager.Instance.OnRoomRemove(removeRoomName);
    }

    public void FindRoomListCreate(string roomName)
    {
        LobbyMenuUIManager.Instance.HandleFindMenuRoomList(roomName);
    }

    public void OnJoinRoom(string roomName)
    {
        LobbyMenuUIManager.Instance.OnJoinRoom(roomName);

        string playerName = LobbyMenuUIManager.Instance.GetPlayerNickNameTextEnterRoom();
        LobbyNetworkManager.Instance.OnJoinRoom(roomName, playerName);
    }

    // LobbyNetworkManager���� ���ο� �÷��̾� ���� Ȯ�� ��
    */
}