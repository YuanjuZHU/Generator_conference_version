using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using Assets.Yuanju.Interfaces_and_classes.generator_components;


public class UDP_Connection : MonoBehaviour
{
    private UdpClient udpServer;
    private Thread t;
    private static IPEndPoint remoteEP_Send;
    private static IPEndPoint remoteEP_Send2;
    private IPEndPoint remoteEP_Receive;

    private GameObject generator;

    private SteamGenerator steamGenerator;

    private bool coroutineIsStarted = false;

    private float nextActionTime = 5.0f;
    public float period = 0.1f;

    public static void ReceiveCallback(IAsyncResult ar) {
        UdpClient u = ((UdpState)(ar.AsyncState)).u;
        IPEndPoint e = ((UdpState)(ar.AsyncState)).e;

        byte[] receiveBytes = u.EndReceive(ar, ref e);
        string receiveString = Encoding.ASCII.GetString(receiveBytes);

        Debug.Log($"Received: {receiveString}");
        

        //messageReceived = true;
    }

    public struct UdpState {
        public UdpClient u;
        public IPEndPoint e;
    }


    void Start() {

        generator = GameObject.Find("Generator V3");
        steamGenerator = new SteamGenerator(generator);

        remoteEP_Send = new IPEndPoint(IPAddress.Parse("150.145.8.144"), 26000); //Local host: 127.0.0.1
        remoteEP_Send2 = new IPEndPoint(IPAddress.Parse("150.145.8.144"), 27000); //Local host: 127.0.0.1




        //t = new Thread(() => {
        //    while(true) {
        //        //this.receiveData();
        //        //udpServer.BeginReceive(new AsyncCallback(ReceiveCallback), s);
        //        receiveData();
        //    }
        //});
        //t.Start();
        //t.IsBackground = true;

        //senderUdpClient();
        //StartCoroutine(receiveData());


    }



    void Update() {


        //Debug.Log("Start");
        //if(Input.GetKeyDown(KeyCode.Return))
        //{
        //    senderUdpClient();
        //}
        //receiveData();
        //if(Input.GetKeyDown("f")) {
        //}
        var powerHandle = GameObject.Find("Leva linea elettrica");

        var switchComponetn = powerHandle.GetComponent<Switch>();
        //if(switchComponetn.Status == 1 && !coroutineIsStarted) {
        //    //receiveData();
        //    StartCoroutine(receiveData());
        //    coroutineIsStarted = true;
        //}


        //StartCoroutine(receiveData());
        if(Input.GetKeyDown(KeyCode.Return)) {
            receiveData();
        }

        //if(Time.time > nextActionTime) {
        //    nextActionTime = Time.time + period;
        //    receiveData();
        //}
    }

    private void OnApplicationQuit() {
        udpServer.Close();
        //t.Abort();
    }

    private void receiveData()
    {
        remoteEP_Receive = new IPEndPoint(IPAddress.Parse("150.145.8.168"), 26001);

        udpServer = new UdpClient(26001);
        byte[] data = udpServer.Receive(ref remoteEP_Receive);
        udpServer.Close();

        //var text = "";
        //foreach(var b in data) {
        //    text = text + " " + b;

        //}
        //Debug.Log("Received data byte dimension: " + data.Length);
        //Debug.Log("Received data byte: " + text);

        if(data.Length > 0)
        {
            var str = Encoding.UTF8.GetString(data);
            Debug.Log("Received data: " + str);

            var compList = steamGenerator.DeserializeGeneratorStatus(str);
            steamGenerator.SetComponentsStatus(compList);
        }
        //yield return new WaitForSeconds(5.0f);
        //StartCoroutine(receiveData());

    }

    public static void senderUdpClient(string jsonFile)
    {
        UdpClient senderClient = new UdpClient();
        senderClient.Connect(remoteEP_Send);

        UdpClient senderClient2 = new UdpClient();
        senderClient2.Connect(remoteEP_Send2);
        Debug.Log("JSON inviato " + jsonFile);

        byte[] bytes = toBytes(jsonFile);
        byte[] bytes2 = toBytes(bytes.Length.ToString());

        Debug.Log("Dati inviati " + bytes.Length);
        var text = "";
        foreach(var b in bytes) {
            text = text + " " + b;

        }
        Debug.Log(text);

        senderClient.Send(bytes, bytes.Length);
        senderClient2.Send(bytes2, bytes2.Length);


        //string sendString = "0.4";
        //Debug.Log("Sent data " + sendString);
        //byte[] bytes = toBytes(sendString);

        //Thread t = new Thread(() => {
        //    while(true) {
        //        Debug.Log("Dati inviati");
        //        senderClient.Send(bytes, bytes.Length);
        //        Thread.Sleep(1000);
        //    }
        //});
        //t.Start();
    }

    static byte[] toBytes(string text) {
            return Encoding.UTF8.GetBytes(text);
        }
    }
