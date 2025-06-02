// ������ �÷��̾��Ʈ�� �޾ƿ��� �ܰ� - ������ �����Ҷ� ���ٵ�

// 0529 ������ ������ �÷��̾��Ʈ �����̾ȵ�, �� ������ �� ���� ��� ���� �۾�x

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
    private FindMenuRoomListManager m_findMenuRoomListManager;
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
        m_findMenuRoomListManager = UICanvas.GetComponent<FindMenuRoomListManager>();
        m_playerListManager = UICanvas.GetComponent<PlayerListUIManager>();

        m_networkService = NetworkManager.Service;
    }


    private void Start()
    {
        InitializeAtStart?.Invoke();

        m_networkService.UpdateRoomList += UpdateFindMenuRoomList;
        m_networkService.JoinedPlayerList += JoinedPlayerList;
    }
    public void StartInGame(int sceneNum)
    {
        /*m_networkManager.SaveInfo();
        SaveNetworkInfo.Instance.SaveInfo(m_networkManager.m_PlayerName, m_networkManager.m_CurrentRoomName, m_networkManager.m_CurrentJoinPlayers);*/
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
        string roomName = m_lobbyMenuUIManager.GetInputFieldRoomName();
        string playerName = m_lobbyMenuUIManager.GetInputFieldPlayerName();

        // 2. ������ ��Ʈ��ũ �� ����
        m_networkService.CreateRoom(roomName, playerName);

        // 3. �� ���� �Ǿ����� üũ. �ȵǾ��� �� 3���������� �ȸ�������� �α�
        float time = 0;
        while (!m_networkService.CheckCreateRoom())
        {
            time += Time.deltaTime;
            if(time >= 3)
            {
                Debug.LogError("CreateRoomFailed");
            }
            m_networkService.CreateRoom(roomName, playerName);
        }

        // 4 �� �޴�UI�� �̵�
        GoMenuUI(GoMenuUIType.RoomMenuUI);

        // 5. �� �޴� ���� �� ����
        // 5.1 �� �޴��� �� �̸� ���� ������ �̸� �޾ƿ� �� ������ ���� ó���� �ڵ� �Ϸ�
        // 5.2 �÷��̾� ����Ʈ ������ ��Ʈ��ũ �Ŵ����� JoinedPlayerList�� ���� �ڵ� ����

        // 5.3 �ΰ��ӽ�ŸƮ ��ư Ȱ��ȭ
        m_lobbyMenuUIManager.ActivateStartInGameBtn(true);  //StartBtn�� ���常 ��������
    }

    // �� ����



    // �� ������

    #endregion ======================================================================= /Button Ŭ��

    // FindMenu �� ����Ʈ ������Ʈ
    public void UpdateFindMenuRoomList(List<RoomInfo> roomList)
    {
        m_findMenuRoomListManager.UpdateFindRoomMenuButtonList(roomList);
    }

    // ���� �� ������� �� + ���� ������ ��
    public void JoinedPlayerList(Player[] players)
    {
        m_playerListManager.CreatePlayerListItem(players);
    }

    // ���� �� ��Ȳ
    public void OnJoinRoom(string roomName)
    {
    }

    public void JoinLobby()
    {
        // TODO : �÷��̾� ����Ʈ�� ���� �ʱ�ȭ�ʿ�
        m_playerListManager.AllRemove();
        m_networkService.JoinLobby();
    }


}