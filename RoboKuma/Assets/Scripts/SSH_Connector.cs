using System;
using UnityEngine;
using Renci.SshNet;
using Renci.SshNet.Common;

public class SSH_Connector
{
    MySQL_Connector dbConnect;
    PasswordConnectionInfo connectionInfo;
    private string remoteHost;
    private string remotePort;
    private string remoteUser;
    private string remotePassword;
    private string localHost;
    private string localPort;
    private string dbUser;
    private string dbPassword;
    private string dbName;

    // Always Initialize First
    public SSH_Connector()
    {
        this.remoteHost = "188.166.217.210";
        this.remotePort = "22";
        this.remoteUser = "admin_kuma";
        this.remotePassword = "admin1234";
        this.localHost = "127.0.0.1";
        this.dbName = "robokuma";
        this.dbUser = "admin_kuma";
        this.dbPassword = "admin1234";
        this.localPort = "3306";

        Initialize();
    }

    public void Initialize()
    {
        connectionInfo = new PasswordConnectionInfo(remoteHost, remoteUser, remotePassword);
        connectionInfo.Timeout = TimeSpan.FromSeconds(30);

        using (var client = new SshClient(connectionInfo))
        {
            try
            {
                Console.WriteLine("Trying SSH connection...");
                client.Connect();
                if (client.IsConnected)
                {
                    Debug.Log("SSH connection is active: {0}", client.ConnectionInfo.ToString());
                }
                else
                {
                    Debug.Log("SSH connection has failed: {0}", client.ConnectionInfo.ToString());
                }

                Console.WriteLine("\r\nTrying port forwarding...");
                var portFwld = new ForwardedPortLocal("127.0.0.1", 3306, "188.166.217.210", 22);
                client.AddForwardedPort(portFwld);
                portFwld.Start();
                if (portFwld.IsStarted)
                {
                    Debug.Log("Port forwarded: {0}", portFwld.ToString());
                }
                else
                {
                    Debug.Log("Port forwarding has failed.");
                }

            }
            catch (SshException e)
            {
                Debug.Log("SSH client connection error: {0}", e.Message);
            }
            catch (System.Net.Sockets.SocketException e)
            {
                Debug.Log("Socket connection error: {0}", e.Message);
            }

            Debug.Log("\r\nTrying database connection...");
            dbConnect = new MySQL_Connector(localHost, dbName, dbUser, dbPassword, localPort);
        }
    }

    public void callSyncPlayerData()
    {
        if(dbConnect != null)
        {
            dbConnect.syncPlayerData();
        }
    }

    public void callUploadPlayerLogs(int player_id)
    {
        if (dbConnect != null)
        {
            dbConnect.uploadPlayerLogs(player_id);
        }
    }
}
