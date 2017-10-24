﻿using UnityEngine;
using System.Threading;
using System.Text;
using System.Collections;
using System.IO;
using System;

/// \mainpage
/// \section Overview
/// The .NET Visual C# library for the Make Controller Kit is designed 
/// to make it as simple as possible for developers to integrate the 
/// Make Controller Kit into their desktop applications, offering the 
/// transparency that makes open source software so rewarding to work with.  
/// You can communicate with the Make Controller Kit from your applications 
/// over either an Ethernet or USB connection, or both.  This library is 
/// supplied both in source form and built, as MakeControllerOsc.dll
/// This document is a reference for MakeControllerOsc.
/// 
/// \section Communication
/// Messages to and from the board conform to the OSC (Open Sound Control) protocol.  
/// OSC is an open, transport-independent standard supported by an increasing 
/// number of environments and devices. 
/// 
/// \subsection OSCmessages OSC Messages
/// OSC messages are represented by the class OscMessage, and consist of two elements:
/// - An address string for the device on the board you韗e dealing with.
/// - A list of value(s) being sent to or from that device. The list of values is optional.
/// 
/// From the perspective of OSC addresses, the Make Controller Kit is organized into a hierarchy of two or three layers:
/// - subsystems ?classes of device, such as analog inputs, servo controllers, and digital outputs.
/// - devices ?the index of a specific device within a subsystem.  
/// If there is only one device in a subsystem, the device is not included in the OSC address.
/// - properties ?different devices have different properties, such as the value of an analog input, 
/// the position of a servo motor, or the state of an LED. 
/// 
/// OSC messages always begin with a slash, and use a slash to delimit each element in the address, 
/// so an example OSC address string would look like:
/// \code /subsystem/device/property \endcode
/// 
/// The second part of an OscMessage is a list of values to be sent to the specified address. 
/// The OSC types that are used by the Make Controller Kit for these values are integers, 
/// floats, and strings.  The values in this list are simply separated by spaces, and the 
/// list can be arbitrarily long.  Most devices on the Make Controller Kit expect only one value.  
/// For example, to set the position of servo 1, you might send a message which 
/// in string form might look like:
/// \code /servo/1/position 512 \endcode
/// 
/// This addressing scheme allows interactions with the board's various subsystems 
/// and properties, and most importantly, accommodates the possibility of future or 
/// custom devices on the board that have not yet been implemented or imagined.  
/// If somebody creates, for example, a GPS extension to the board, communicating 
/// with that device from this library is the same as for any other.  More details 
/// about OSC can be found at http://www.opensoundcontrol.org.
/// 
/// \section sendingdata Sending Data
/// As previously mentioned, the Make Controller Kit can communicate over both 
/// Ethernet and USB.  Messages are sent as packets, both over USB and UDP, and 
/// corresponding structures are used ?UsbPacket and UdpPacket.  Once you韛e created 
/// a packet, you can simply call its Send() method, with the OscMessage you韉 like to send.  
/// There are helper methods to create an OscMessage from a string, or you can pass in the OscMessage itself. 
/// 
/// For example, you might set up your UsbSend() routine to look something like:
/// \code public void usbSend(string text)
/// {
///     OscMessage oscM = Osc.StringToOscMessage(text);
///     oscUsb is an Osc object, connected to a UsbPacket object 
///     oscUsb.Send(oscM);
/// } \endcode
/// If your data is already in the form of an OscMessage, you can call oscUsb.Send() directly.
/// 
/// \section readingdata Reading Data
/// The Make Controller Kit must be polled in order to read data from it.  To do this, 
/// send an OscMessage with the address of the device you韉 like to read, but omit 
/// the list of values.  When the board receives an OscMessage with no value, 
/// it interprets that as a read request, and sends back an OscMessage with the 
/// current value at the appropriate address.
/// 
/// The .NET Make Controller Kit library conveniently provides handlers that will 
/// call back a given function when an OscMessage with a given address string is received.  
/// Your implementation could look something like:
/// \code// Set the handler in the constructor for a particular address
/// MyConstructor()
/// {
///     udpPacket = new UdpPacket();
///     oscUdp = new Osc(udpPacket);
///     // A thread is started when the Osc object is created to read 
///     // incoming messages.
///     oscUdp.SetAddressHandler("/analogin/0/value", Ain0Message);
/// }
///
/// // The method you specified as the handler will be called back when a 
/// // message with a matching address string comes back from the board.
/// public void AIn0Message(OscMessage oscMessage)
/// {
///     // write the message to a console, for example
///     mct.WriteLine("AIn0 > " + Osc.OscMessageToString(oscMessage));
/// } \endcode
/// You could alternatively set a handler for all incoming messages by calling 
/// the SetAllMessageHandler() method in your setup, instead of SetAddressHandler().
/// 
/// 


//namespace MakingThings
//{
/// <summary>
/// The OscMessage class is a data structure that represents
/// an OSC address and an arbitrary number of values to be sent to that address.
/// </summary>
public class OscMessage
{
    /// <summary>
    /// The OSC address of the message as a string.
    /// </summary>
    public string Address;
    /// <summary>
    /// The list of values to be delivered to the Address.
    /// </summary>
    public ArrayList Values;
    public OscMessage()
    {
        Values = new ArrayList();
    }
}

public delegate void OscMessageHandler(OscMessage oscM);

/// <summary>
/// The Osc class provides the methods required to send, receive, and manipulate OSC messages.
/// Several of the helper methods are static since a running Osc instance is not required for 
/// their use.
/// 
/// When instanciated, the Osc class opens the PacketIO instance that's handed to it and 
/// begins to run a reader thread.  The instance is then ready to service Send OscMessage requests 
/// and to start supplying OscMessages as received back.
/// 
/// The Osc class can be called to Send either individual messages or collections of messages
/// in an Osc Bundle.  Receiving is done by delegate.  There are two ways: either submit a method
/// to receive all incoming messages or submit a method to handle only one particular address.
/// 
/// Messages can be encoded and decoded from Strings via the static methods on this class, or
/// can be hand assembled / disassembled since they're just a string (the address) and a list 
/// of other parameters in Object form. 
/// 
/// </summary>
public class Osc : MonoBehaviour
{

    /// <summary>
    /// Osc Constructor.  Starts the Reader thread and initializes some internal state.
    /// </summary>
    /// <param name="oscPacketIO">The PacketIO instance used for packet IO.</param>
    /*
    public Osc(PacketIO oscPacketIO)
    {
      // Save the PacketExchage pointer
      OscPacketIO = oscPacketIO;

      // Create the hashtable for the address lookup mechanism
      AddressTable = new Hashtable();

      ReadThread = new Thread(Read);
      ReaderRunning = true;
      ReadThread.IsBackground = true;
      ReadThread.Start();
    }
	*/


    void Start()
    {
        //do nothing, init must be called	
    }

    public void init(UDPPacketIO oscPacketIO)
    {
        OscPacketIO = oscPacketIO;

        // Create the hashtable for the address lookup mechanism
        AddressTable = new Hashtable();

        ReadThread = new Thread(Read);
        ReaderRunning = true;
        ReadThread.IsBackground = true;
        ReadThread.Start();
    }

    /// <summary>
    /// Make sure the PacketExchange is closed.
    /// </summary>
    ~Osc()
    {
        //Cancel();
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        Cancel();
    }

    public void Cancel()
    {
        ReaderRunning = false;
        ReadThread.Abort();
        if (OscPacketIO.IsOpen())
            OscPacketIO.Close();
    }

    /// <summary>
    /// Read Thread.  Loops waiting for packets.  When a packet is received, it is 
    /// dispatched to any waiting All Message Handler.  Also, the address is looked up and
    /// any matching handler is called.
    /// </summary>
    private void Read()
    {
        while (ReaderRunning)
        {
            Debug.Log("Read");
            byte[] buffer = new byte[1000];
            int length = OscPacketIO.ReceivePacket(buffer);
            if (length > 0)
            {
                ArrayList messages = Osc.PacketToOscMessages(buffer, length);
                foreach (OscMessage om in messages)
                {
                    if (AllMessageHandler != null)
                        AllMessageHandler(om);
                    OscMessageHandler h = (OscMessageHandler)Hashtable.Synchronized(AddressTable)[om.Address];
                    if (h != null)
                        h(om);
                }
            }
            else
                Thread.Sleep(500);
        }
    }

    /// <summary>
    /// Send an individual OSC message.  Internally takes the OscMessage object and 
    /// serializes it into a byte[] suitable for sending to the PacketIO.
    /// </summary>
    /// <param name="oscMessage">The OSC Message to send.</param>   
    public void Send(OscMessage oscMessage)
    {
        byte[] packet = new byte[1000];
        int length = Osc.OscMessageToPacket(oscMessage, packet, 1000);
        OscPacketIO.SendPacket(packet, length);
    }
    public void Send(string message)
    {
        //byte[] packet = new byte[1000];
        //int length = message.Length;
        byte[] packet = System.Text.Encoding.Default.GetBytes(message);
        OscPacketIO.SendPacket(packet, packet.Length);
    }

    /// <summary>
    /// Sends a list of OSC Messages.  Internally takes the OscMessage objects and 
    /// serializes them into a byte[] suitable for sending to the PacketExchange.
    /// </summary>
    /// <param name="oms">The OSC Message to send.</param>   
    public void Send(ArrayList oms)
    {
        byte[] packet = new byte[1000];
        int length = Osc.OscMessagesToPacket(oms, packet, 1000);
        OscPacketIO.SendPacket(packet, length);
    }

    /// <summary>
    /// Set the method to call back on when any message is received.
    /// The method needs to have the OscMessageHandler signature - i.e. void amh( OscMessage oscM )
    /// </summary>
    /// <param name="amh">The method to call back on.</param>   
    public void SetAllMessageHandler(OscMessageHandler amh)
    {
        AllMessageHandler = amh;
    }

    /// <summary>
    /// Set the method to call back on when a message with the specified
    /// address is received.  The method needs to have the OscMessageHandler signature - i.e. 
    /// void amh( OscMessage oscM )
    /// </summary>
    /// <param name="key">Address string to be matched</param>   
    /// <param name="ah">The method to call back on.</param>   
    public void SetAddressHandler(string key, OscMessageHandler ah)
    {
        Hashtable.Synchronized(AddressTable).Add(key, ah);
    }

    private UDPPacketIO OscPacketIO;
    Thread ReadThread;
    private bool ReaderRunning;
    private OscMessageHandler AllMessageHandler;
    Hashtable AddressTable;

    /// <summary>
    /// General static helper that returns a string suitable for printing representing the supplied 
    /// OscMessage.
    /// </summary>
    /// <param name="message">The OscMessage to be stringified.</param>
    /// <returns>The OscMessage as a string.</returns>
    public static string OscMessageToString(OscMessage message)
    {
        StringBuilder s = new StringBuilder();
        s.Append(message.Address);
        foreach (object o in message.Values)
        {
            s.Append(" ");
            s.Append(o.ToString());
        }
        return s.ToString();
    }

    /// <summary>
    /// Creates an OscMessage from a string - extracts the address and determines each of the values. 
    /// </summary>
    /// <param name="message">The string to be turned into an OscMessage</param>
    /// <returns>The OscMessage.</returns>
    public static OscMessage StringToOscMessage(string message)
    {
        OscMessage oM = new OscMessage();
        // Console.WriteLine("Splitting " + message);
        string[] ss = message.Split(new char[] { ' ' });
        IEnumerator sE = ss.GetEnumerator();
        if (sE.MoveNext())
            oM.Address = (string)sE.Current;
        while (sE.MoveNext())
        {
            string s = (string)sE.Current;
            // Console.WriteLine("  <" + s + ">");
            if (s.StartsWith("\""))
            {
                StringBuilder quoted = new StringBuilder();
                bool looped = false;
                if (s.Length > 1)
                    quoted.Append(s.Substring(1));
                else
                    looped = true;
                while (sE.MoveNext())
                {
                    string a = (string)sE.Current;
                    // Console.WriteLine("    q:<" + a + ">");
                    if (looped)
                        quoted.Append(" ");
                    if (a.EndsWith("\""))
                    {
                        quoted.Append(a.Substring(0, a.Length - 1));
                        break;
                    }
                    else
                    {
                        if (a.Length == 0)
                            quoted.Append(" ");
                        else
                            quoted.Append(a);
                    }
                    looped = true;
                }
                oM.Values.Add(quoted.ToString());
            }
            else
            {
                if (s.Length > 0)
                {
                    try
                    {
                        int i = int.Parse(s);
                        // Console.WriteLine("  i:" + i);
                        oM.Values.Add(i);
                    }
                    catch
                    {
                        try
                        {
                            float f = float.Parse(s);
                            // Console.WriteLine("  f:" + f);
                            oM.Values.Add(f);
                        }
                        catch
                        {
                            // Console.WriteLine("  s:" + s);
                            oM.Values.Add(s);
                        }
                    }

                }
            }
        }
        return oM;
    }

    /// <summary>
    /// Takes a packet (byte[]) and turns it into a list of OscMessages.
    /// </summary>
    /// <param name="packet">The packet to be parsed.</param>
    /// <param name="length">The length of the packet.</param>
    /// <returns>An ArrayList of OscMessages.</returns>
    public static ArrayList PacketToOscMessages(byte[] packet, int length)
    {
        ArrayList messages = new ArrayList();
        ExtractMessages(messages, packet, 0, length);
        return messages;
    }

    /// <summary>
    /// Puts an array of OscMessages into a packet (byte[]).
    /// </summary>
    /// <param name="messages">An ArrayList of OscMessages.</param>
    /// <param name="packet">An array of bytes to be populated with the OscMessages.</param>
    /// <param name="length">The size of the array of bytes.</param>
    /// <returns>The length of the packet</returns>
    public static int OscMessagesToPacket(ArrayList messages, byte[] packet, int length)
    {
        int index = 0;
        if (messages.Count == 1)
            index = OscMessageToPacket((OscMessage)messages[0], packet, 0, length);
        else
        {
            // Write the first bundle bit
            index = InsertString("#bundle", packet, index, length);
            // Write a null timestamp (another 8bytes)
            int c = 8;
            while ((c--) > 0)
                packet[index++]++;
            // Now, put each message preceded by it's length
            foreach (OscMessage oscM in messages)
            {
                int lengthIndex = index;
                index += 4;
                int packetStart = index;
                index = OscMessageToPacket(oscM, packet, index, length);
                int packetSize = index - packetStart;
                packet[lengthIndex++] = (byte)((packetSize >> 24) & 0xFF);
                packet[lengthIndex++] = (byte)((packetSize >> 16) & 0xFF);
                packet[lengthIndex++] = (byte)((packetSize >> 8) & 0xFF);
                packet[lengthIndex++] = (byte)((packetSize) & 0xFF);
            }
        }
        return index;
    }

    /// <summary>
    /// Creates a packet (an array of bytes) from a single OscMessage.
    /// </summary>
    /// <remarks>A convenience method, not requiring a start index.</remarks>
    /// <param name="oscM">The OscMessage to be returned as a packet.</param>
    /// <param name="packet">The packet to be populated with the OscMessage.</param>
    /// <param name="length">The usable size of the array of bytes.</param>
    /// <returns>The length of the packet</returns>
    public static int OscMessageToPacket(OscMessage oscM, byte[] packet, int length)
    {
        return OscMessageToPacket(oscM, packet, 0, length);
    }

    /// <summary>
    /// Creates an array of bytes from a single OscMessage.  Used internally.
    /// </summary>
    /// <remarks>Can specify where in the array of bytes the OscMessage should be put.</remarks>
    /// <param name="oscM">The OscMessage to be turned into an array of bytes.</param>
    /// <param name="packet">The array of bytes to be populated with the OscMessage.</param>
    /// <param name="start">The start index in the packet where the OscMessage should be put.</param>
    /// <param name="length">The length of the array of bytes.</param>
    /// <returns>The index into the packet after the last OscMessage.</returns>
    private static int OscMessageToPacket(OscMessage oscM, byte[] packet, int start, int length)
    {
        int index = start;
        index = InsertString(oscM.Address, packet, index, length);
        //if (oscM.Values.Count > 0)
        {
            StringBuilder tag = new StringBuilder();
            tag.Append(",");
            int tagIndex = index;
            index += PadSize(2 + oscM.Values.Count);

            foreach (object o in oscM.Values)
            {
                if (o is int)
                {
                    int i = (int)o;
                    tag.Append("i");
                    packet[index++] = (byte)((i >> 24) & 0xFF);
                    packet[index++] = (byte)((i >> 16) & 0xFF);
                    packet[index++] = (byte)((i >> 8) & 0xFF);
                    packet[index++] = (byte)((i) & 0xFF);
                }
                else
                {
                    if (o is float)
                    {
                        float f = (float)o;
                        tag.Append("f");
                        byte[] buffer = new byte[4];
                        MemoryStream ms = new MemoryStream(buffer);
                        BinaryWriter bw = new BinaryWriter(ms);
                        bw.Write(f);
                        packet[index++] = buffer[3];
                        packet[index++] = buffer[2];
                        packet[index++] = buffer[1];
                        packet[index++] = buffer[0];
                    }
                    else
                    {
                        if (o is string)
                        {
                            tag.Append("s");
                            index = InsertString(o.ToString(), packet, index, length);
                        }
                        else if(o is Boolean)
                        {
                            tag.Append((Boolean)o ? "T" : "F");
                        }
                        else if(o is Color)
                        {
                            tag.Append("r");
                            byte[] buffer = new byte[4];
                            packet[index++] = (Byte)((Color)o).r;
                            packet[index++] = (Byte)((Color)o).g;
                            packet[index++] = (Byte)((Color)o).b;
                            packet[index++] = (Byte)((Color)o).a;
                        }
                        else
                        {
                            tag.Append("?");
                        }
                    }
                  
                }
            }
            InsertString(tag.ToString(), packet, tagIndex, length);
        }
        return index;
    }

    /// <summary>
    /// Receive a raw packet of bytes and extract OscMessages from it.  Used internally.
    /// </summary>
    /// <remarks>The packet may contain a OSC message or a bundle of messages.</remarks>
    /// <param name="messages">An ArrayList to be populated with the OscMessages.</param>
    /// <param name="packet">The packet of bytes to be parsed.</param>
    /// <param name="start">The index of where to start looking in the packet.</param>
    /// <param name="length">The length of the packet.</param>
    /// <returns>The index after the last OscMessage read.</returns>
    private static int ExtractMessages(ArrayList messages, byte[] packet, int start, int length)
    {
        int index = start;
        switch ((char)packet[start])
        {
            case '/':
                index = ExtractMessage(messages, packet, index, length);
                break;
            case '#':
                string bundleString = ExtractString(packet, start, length);
                if (bundleString == "#bundle")
                {
                    // skip the "bundle" and the timestamp
                    index += 16;
                    while (index < length)
                    {
                        int messageSize = (packet[index++] << 24) + (packet[index++] << 16) + (packet[index++] << 8) + packet[index++];
                        /*int newIndex = */
                        ExtractMessages(messages, packet, index, length);
                        index += messageSize;
                    }
                }
                break;
        }
        return index;
    }

    /// <summary>
    /// Extracts a messages from a packet.
    /// </summary>
    /// <param name="messages">An ArrayList to be populated with the OscMessage.</param>
    /// <param name="packet">The packet of bytes to be parsed.</param>
    /// <param name="start">The index of where to start looking in the packet.</param>
    /// <param name="length">The length of the packet.</param>
    /// <returns>The index after the OscMessage is read.</returns>
    private static int ExtractMessage(ArrayList messages, byte[] packet, int start, int length)
    {
        OscMessage oscM = new OscMessage();
        oscM.Address = ExtractString(packet, start, length);
        int index = start + PadSize(oscM.Address.Length + 1);
        string typeTag = ExtractString(packet, index, length);
        index += PadSize(typeTag.Length + 1);
        //oscM.Values.Add(typeTag);
        foreach (char c in typeTag)
        {
            switch (c)
            {
                case ',':
                    break;
                case 's':
                    {
                        string s = ExtractString(packet, index, length);
                        index += PadSize(s.Length + 1);
                        oscM.Values.Add(s);
                        break;
                    }
                case 'i':
                    {
                        int i = (packet[index++] << 24) + (packet[index++] << 16) + (packet[index++] << 8) + packet[index++];
                        oscM.Values.Add(i);
                        break;
                    }
                case 'f':
                    {
                        byte[] buffer = new byte[4];
                        buffer[3] = packet[index++];
                        buffer[2] = packet[index++];
                        buffer[1] = packet[index++];
                        buffer[0] = packet[index++];
                        MemoryStream ms = new MemoryStream(buffer);
                        BinaryReader br = new BinaryReader(ms);
                        float f = br.ReadSingle();
                        oscM.Values.Add(f);
                        break;
                    }
            }
        }
        messages.Add(oscM);
        return index;
    }

    /// <summary>
    /// Removes a string from a packet.  Used internally.
    /// </summary>
    /// <param name="packet">The packet of bytes to be parsed.</param>
    /// <param name="start">The index of where to start looking in the packet.</param>
    /// <param name="length">The length of the packet.</param>
    /// <returns>The string</returns>
    private static string ExtractString(byte[] packet, int start, int length)
    {
        StringBuilder sb = new StringBuilder();
        int index = start;
        while (packet[index] != 0 && index < length)
            sb.Append((char)packet[index++]);
        return sb.ToString();
    }

    private static string Dump(byte[] packet, int start, int length)
    {
        StringBuilder sb = new StringBuilder();
        int index = start;
        while (index < length)
            sb.Append(packet[index++] + "|");
        return sb.ToString();
    }

    /// <summary>
    /// Inserts a string, correctly padded into a packet.  Used internally.
    /// </summary>
    /// <param name="string">The string to be inserted</param>
    /// <param name="packet">The packet of bytes to be parsed.</param>
    /// <param name="start">The index of where to start looking in the packet.</param>
    /// <param name="length">The length of the packet.</param>
    /// <returns>An index to the next byte in the packet after the padded string.</returns>
    private static int InsertString(string s, byte[] packet, int start, int length)
    {
        int index = start;
        foreach (char c in s)
        {
            packet[index++] = (byte)c;
            if (index == length)
                return index;
        }
        packet[index++] = 0;
        int pad = (s.Length + 1) % 4;
        if (pad != 0)
        {
            pad = 4 - pad;
            while (pad-- > 0)
                packet[index++] = 0;
        }
        return index;
    }

    /// <summary>
    /// Takes a length and returns what it would be if padded to the nearest 4 bytes.
    /// </summary>
    /// <param name="rawSize">Original size</param>
    /// <returns>padded size</returns>
    private static int PadSize(int rawSize)
    {
        int pad = rawSize % 4;
        if (pad == 0)
            return rawSize;
        else
            return rawSize + (4 - pad);
    }
}
//}