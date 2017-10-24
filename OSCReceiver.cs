using UnityEngine;
using System.Collections;

public class OSCReceiver : MonoBehaviour
{
    public string RemoteIP = "127.0.0.1";
    public int SendToPort = 57131;
    public int ListenerPort = 57130;
    public Transform controller;
    private Osc handler;
    // Use this for initialization
    void Start()
    {
        //Initializes on start up to listen for messages
        //make sure this game object has both UDPPackIO and OSC script attached
        UDPPacketIO udp = GetComponent("UDPPacketIO") as UDPPacketIO;
        udp.init(RemoteIP, SendToPort, ListenerPort);
        handler = GetComponent("Osc") as Osc;
        handler.init(udp);

        handler.SetAddressHandler("/test1", Example1);
        handler.SetAddressHandler("/test2", Example2);
    }

    //these fucntions are called when messages are received
    public void Example1(OscMessage oscMessage)
    {
        //How to access values: 
        //oscMessage.Values[0], oscMessage.Values[1], etc
        Debug.Log("Called Example One > " + Osc.OscMessageToString(oscMessage));
    }

    //these fucntions are called when messages are received
    public void Example2(OscMessage oscMessage)
    {
        //How to access values: 
        //oscMessage.Values[0], oscMessage.Values[1], etc
        Debug.Log("Called Example Two > " + Osc.OscMessageToString(oscMessage));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

            //handler.Send(Osc.StringToOscMessage("/surfaces/Quad1/red 0.1"));
            //handler.Send(Osc.StringToOscMessage("/surfaces/Quad/green 0.5"));
            //handler.Send(Osc.StringToOscMessage("/surfaces/Quad1/green 0.1"));
            //handler.Send(Osc.StringToOscMessage("/surfaces/Quad1/blue 0.5"));
            //handler.Send(Osc.StringToOscMessage("/surfaces/Quad1/color 0.5,0.5,0.5,0.5"));
            //handler.Send("/surfaces/Quad 1/red 0.5");

            OscMessage message = new OscMessage();
            message.Address = "/surfaces/Quad 1/color";
            //message.Values.Add(0.5f);
            message.Values.Add(new Color(255,125,0,255));
  
            
            handler.Send(message);
            print("A key was pressed");
        }
    }
}
