
using System;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Client : MonoBehaviour
{
    public static Client instance {set; get;}

    private void Awake() {
        instance  = this;
    }

    public NetworkDriver driver;
    private NetworkConnection connection ; 

    private bool isActive = false;

    public Action connectionDropped; 

    public void Init(string ip, ushort port){
        driver = NetworkDriver.Create();
        NetworkEndPoint endPoint = NetworkEndPoint.Parse(ip, port); 

        connection = driver.Connect(endPoint);

        Debug.Log("Attemping to connect to server on " + endPoint.Address);

        isActive = true; 

        RegisterToEvent();
    }



    public void ShutDown(){
        if(isActive){
            UnRegisterToEvent();
            driver.Dispose();
            isActive = false;
            connection = default(NetworkConnection);
        }
    }

    private void OnDestroy() {
        ShutDown();
    }

    public void Update(){
        if(!isActive){
            return ;
        }

        driver.ScheduleUpdate().Complete();
        CheckAlive();
        
        UpdateMessagePump();
    }

    private void CheckAlive()
    {
        if(!connection.IsCreated && isActive){
            Debug.Log("Something went wrong, lost connection to server ");
            connectionDropped?.Invoke();
            ShutDown();
        }
    }

    private void UpdateMessagePump()
    {
        DataStreamReader stream;
        NetworkEvent.Type cmd; 
        while((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty){
            if(cmd == NetworkEvent.Type.Connect){
                // var s = new NetWelcome();
                // s.AssignedTeam = 5;
                SendToServer(new NetWelcome());
                Debug.Log("We're connected");
            }else if(cmd == NetworkEvent.Type.Data){
                NetUtility.OnData(stream, default(NetworkConnection));
            }else if(cmd == NetworkEvent.Type.Disconnect){
                Debug.Log("Client got disconnected from server");
                connection = default(NetworkConnection);
                connectionDropped?.Invoke();
                ShutDown();
            }
        }
    }

    private void SendToServer(NetMessage msg)
    {
        DataStreamWriter writer; 
        driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }

    private void RegisterToEvent () {
        NetUtility.C_KEEP_ALIVE += OnKeepAlive;
    }

    private void UnRegisterToEvent(){
        NetUtility.C_KEEP_ALIVE -= OnKeepAlive;
    }

    private void OnKeepAlive(NetMessage msg){        
        SendToServer(msg);
    }
}

