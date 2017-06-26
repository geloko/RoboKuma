using System;
using UnityEngine;
using Renci.SshNet;
using Renci.SshNet.Common;

public class SSH_Connector
{
    // Always Initialize First
    public SSH_Connector()
    {
        PasswordConnectionInfo connectionInfo = new PasswordConnectionInfo("188.166.217.210", "admin_kuma", "admin1234");
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
            MySQL_Connector dbConnect = new MySQL_Connector("localhost", "test_database", "root", "passwrod123", "3306");
        }
    }
}
