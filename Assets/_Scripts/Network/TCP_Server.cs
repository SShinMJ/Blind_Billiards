using System.Collections.Generic;
using System.Net.Sockets;
using System;
using System.IO;

public class TCP_Server 
{
    List<TCP_Link> clients;
    List<TCP_Link> disconnectList;

    TcpListener server;
    bool serverStarted;

    // Open or close the server.
    // =====================================================================================
    public void ServerCreate()
    {
        clients = new List<TCP_Link>();
        disconnectList = new List<TCP_Link>();

        try
        {
            TCP_Core.SetIpAndPort();
            server = new TcpListener(TCP_Core.ipForServer, TCP_Core.port);
            server.Start();

            StartListening();
            serverStarted = true;
            TCP_Core.Message($"������ {TCP_Core.port}���� ���۵Ǿ����ϴ�.");
        }
        catch (Exception e)
        {
            TCP_Core.Message($"Socket error: {e.Message}");
        }
    }

    public void CloseServer()
    {

    }

    // Must be update everytime.
    // =====================================================================================
    public void ListenAndCheckDisconnect()
    {
        if (!serverStarted) return;

        foreach (TCP_Link c in clients)
        {
            // Ŭ���̾�Ʈ�� ������ ������ֳ�?
            if (!IsConnected(c.tcp))
            {
                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            }
            // Ŭ���̾�Ʈ�κ��� üũ �޽����� �޴´�
            else
            {
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    string data = new StreamReader(s, true).ReadLine();
                    if (data != null)
                        OnIncomingData(c, data);
                }
            }
        }

        for (int i = 0; i < disconnectList.Count - 1; i++)
        {
            Broadcast($"{disconnectList[i].id} ������ ���������ϴ�", clients);

            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }

    bool IsConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                return true;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }

    // When client attempt to connects.
    // =====================================================================================
    void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;
        clients.Add(new TCP_Link(listener.EndAcceptTcpClient(ar)));
        StartListening();

        // �޽����� ����� ��ο��� ����
        Broadcast(TCP_Core.command.GetCommand(-1), new List<TCP_Link>() { clients[clients.Count - 1] });
    }

    // Receve or broadcast data.
    // =====================================================================================

    // When data receved. 
    void OnIncomingData(TCP_Link c, string data)
    {
        List<CommandData> cmd = CommandCore.Decode(TCP_Core.command, data);

        if (cmd[0].command == -1)
        {
            c.id = cmd[0].text;
            Broadcast($"{c.id}�� ����Ǿ����ϴ�", clients);
            return;
        }

        foreach(CommandData command in cmd)
        {
            Broadcast($"{c.id} : {command.command.ToString()}, {command.text}", clients);
        }
    }

    // Send message to select clients.
    void Broadcast(string data, List<TCP_Link> cl)
    {
        foreach (var c in cl)
        {
            try
            {
                StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception e)
            {
                TCP_Core.Message($"���� ���� : {e.Message}�� Ŭ���̾�Ʈ���� {c.id}");
            }
        }
    }
}

