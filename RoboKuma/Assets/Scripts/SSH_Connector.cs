using System;
using UnityEngine;
using Renci.SshNet;
using Renci.SshNet.Common;

public class SSH_Connector
{
    private static string remoteHost = "139.59.127.87";
    private static string remotePort = "22";
    private static string remoteUser = "admin_kuma";
    private static string remotePassword = "Potangina23!";
    private static string localHost = "127.0.0.1";
    private static uint localPort = 3306;
    private static string dbName = "robokuma";
    private static string dbUser = "admin_kuma";
    private static string dbPassword = "Potangina23!";
    //private static Toast toast = new Toast();

    // Perform FIRST Once Online
    // Only perform ONCE
	//Added object state to comply with threadpool requirements
	public void Start(object state)
    {
        PasswordConnectionInfo connectionInfo = new PasswordConnectionInfo(remoteHost, remoteUser, remotePassword);
        connectionInfo.Timeout = TimeSpan.FromSeconds(30);

        using (var client = new SshClient(connectionInfo))
        {
            try
            {
                Debug.Log("Trying SSH connection...");
                client.Connect();
                if (client.IsConnected)
                {
                    Debug.Log("SSH connection is active: " + client.ConnectionInfo.ToString());
                }
                else
                {
                    Debug.Log("SSH connection has failed: " + client.ConnectionInfo.ToString());
                }

                Debug.Log("Trying port forwarding...");
                var portFwld = new ForwardedPortLocal(localHost, localPort, localHost, localPort);
                Debug.Log(portFwld.BoundHost);
                Debug.Log(portFwld.BoundPort);
                Debug.Log(portFwld.Host);
                Debug.Log(portFwld.Port);
                client.AddForwardedPort(portFwld);
                portFwld.Start();
                if (portFwld.IsStarted)
                {
                    Debug.Log("Port forwarded: " + portFwld.ToString());
                }
                else
                {
                    Debug.Log("Port forwarding has failed.");
                }

                Debug.Log("Trying database connection...");
                MySQL_Connector dbConnect = new MySQL_Connector(localHost, dbName, dbUser, dbPassword, remotePort);
                dbConnect.syncPlayerData();
                Debug.Log("Successfully Updated Player Data!");
                //toast.showToastOnUiThread("Successfully Updated Player Data!");
            }
            catch (SshException e)
            {
                Debug.Log("SSH client connection error: " + e.Message);
            }
            catch (System.Net.Sockets.SocketException e)
            {
                Debug.Log("Socket connection error: " + e.Message);
            }
        }
    }

    // Only perform after Start() has been initially performed
	//Added object state to comply with threadpool requirements
	public void callUploadPlayerLogs(object state)
    {
        PasswordConnectionInfo connectionInfo = new PasswordConnectionInfo(remoteHost, remoteUser, remotePassword);
        connectionInfo.Timeout = TimeSpan.FromSeconds(30);

        using (var client = new SshClient(connectionInfo))
        {
            try
            {
                Debug.Log("Trying SSH connection...");
                client.Connect();
                if (client.IsConnected)
                {
                    Debug.Log("SSH connection is active: " + client.ConnectionInfo.ToString());
                }
                else
                {
                    Debug.Log("SSH connection has failed: " + client.ConnectionInfo.ToString());
                }

                Debug.Log("Trying port forwarding...");
                var portFwld = new ForwardedPortLocal(localHost, localPort, localHost, localPort);
                Debug.Log(portFwld.BoundHost);
                Debug.Log(portFwld.BoundPort);
                Debug.Log(portFwld.Host);
                Debug.Log(portFwld.Port);
                client.AddForwardedPort(portFwld);
                portFwld.Start();
                if (portFwld.IsStarted)
                {
                    Debug.Log("Port forwarded: " + portFwld.ToString());
                }
                else
                {
                    Debug.Log("Port forwarding has failed.");
                }

                Debug.Log("Trying database connection...");
                MySQL_Connector dbConnect = new MySQL_Connector(localHost, dbName, dbUser, dbPassword, remotePort);
                dbConnect.uploadIfSynced(SQLiteDatabase.getPlayer().is_synced);
                Debug.Log("Successfully Uploaded Player Logs!");
                //toast.showToastOnUiThread("Successfully Uploaded Player Logs!");
            }
            catch (SshException e)
            {
                Debug.Log("SSH client connection error: " + e.Message);
            }
            catch (System.Net.Sockets.SocketException e)
            {
                Debug.Log("Socket connection error: " + e.Message);
            }
        }
    }
}
