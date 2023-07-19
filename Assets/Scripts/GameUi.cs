using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameUi : MonoBehaviour
{
    [SerializeField] private Animator menuAnimator; 
    [SerializeField] private TMP_InputField addressInput;
    public GameObject[] screem;

    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private TMP_Text connectionStatusText;

    public static GameUi inst{set; get;}

    public Server server;
    public Client client; 



    private void Awake() {
        inst = this;
        menuAnimator = GetComponent<Animator>();
    }


    public void OnConnect(){
        networkManager.Connect();
    }
    public void OnLocalGameButton(){
        menuAnimator.SetTrigger("InGameMenu");
        server.Init(8007);
        client.Init("127.0.0.1", 8007);
    }

    public void OnOnlineGameButton(){
        menuAnimator.SetTrigger("OnlineMenu");
    }

    public void OnOnlineHostButton(){
        server.Init(8007);
        client.Init("127.0.0.1", 8007);
        menuAnimator.SetTrigger("HostMenu");
    }

    public void OnOnlineConnectButton(){
        client.Init(addressInput.text , 8007);
        // menuAnimator.SetTrigger("InGameMenus");
    }

    public void OnOnlineBackButton(){
        menuAnimator.SetTrigger("StartMenu");
    }

    public void OnHostBackButton(){
        server.ShutDown();
        client.ShutDown();
        menuAnimator.SetTrigger("OnlineMenu");
    }

    public void SetConnectionStatus(string status){
        connectionStatusText.text = status;
    }

    public void DisabelAllScreem()
    {
        for (int i = 0; i < screem.Count(); i++)
        {
            screem[i].SetActive(false);
        }
    }
}
