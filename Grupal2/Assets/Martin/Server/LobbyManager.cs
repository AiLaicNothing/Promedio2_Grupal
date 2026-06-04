using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Multiplayer;
using System.Threading.Tasks;
using TMPro;
using System.Collections.Generic;


public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Transform roomContainer;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private TMP_InputField inputField;
    private List<GameObject> spawnedRoom = new();

    async void Start()
    {
        await InitUnity();
    }


    async Task InitUnity()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRoom()
    {
        string name = inputField.text;

        SessionOptions option = new SessionOptions().WithRelayNetwork();

        option.MaxPlayers = 4;
        option.IsPrivate = false;
        option.Name = name;

        IHostSession session = await MultiplayerService.Instance.CreateSessionAsync(option);

        Unity.Netcode.NetworkManager.Singleton.StartHost();

        Refresh();
    }

    public async void JoinRoom(string id)
    {
        await MultiplayerService.Instance.JoinSessionByIdAsync(id);

        Unity.Netcode.NetworkManager.Singleton.StartClient();
    }

    public async void Refresh()
    {

        Clear();

        QuerySessionsResults results = await MultiplayerService.Instance.QuerySessionsAsync(new QuerySessionsOptions());

        foreach (ISessionInfo sesssion in results.Sessions)
        {
            GameObject room = Instantiate(buttonPrefab, roomContainer);
            TMP_Text text = room.GetComponentInChildren<TMP_Text>();

            text.text = sesssion.Name;

            string id = sesssion.Id;

            room.GetComponent<Button>().onClick.AddListener(() =>
            { 
                JoinRoom(id);
            });

            spawnedRoom.Add(room);
        }
    }

    void Clear()
    {
        foreach (GameObject room in spawnedRoom)
        {
            Destroy(room);
        }

        spawnedRoom.Clear();
    }

}
