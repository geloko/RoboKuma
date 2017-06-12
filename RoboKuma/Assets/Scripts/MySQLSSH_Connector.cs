using System;
using MySql.Data.MySqlClient;

public class MySQLSSH_Connector
{
    static MySqlConnection conn;
    private static int localPort;
    private static string remoteHost;
    private static string host;
    private static int remotePort;
    private static string user;
    private static string password;
    private static string dbuserName;
    private static string dbpassword;
    private static string dbName;

    public MySQLSSH_Connector()
    {
        Initialize();
    }

    private void Initialize()
    {
        localPort = "22";
        remoteHost = "188.166.217.210";
        host = "127.0.0.1";
        remotePort = "3306";
        user = "admin_kuma";
        password = "admin1234";
        dbuserName = "admin_kuma";
        dbpassword = "admin2134";
        dbName = "RoboKuma";
        string connectionString;
        connectionString = "SERVER=" + host + ";" + "DATABASE=" +
        dbName + ";" + "UID=" + dbuserName + ";" + "PASSWORD=" + dbpassword + ";";

        conn = new MySqlConnection(connectionString);
    }

    public static MySqlDataReader executeQuery(string query)
    {
        MySqlDataReader result = null;

        if (this.OpenConnection() == true)
        {
            MySqlCommand cmd = new MySqlCommand(query, conn);
            result = cmd.ExecuteReader();
            this.CloseConnection();

            return list;
        }
    }

    public static void executeStatement(string statement)
    {
        if (this.OpenConnection() == true)
        {
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            this.CloseConnection();
        }
    }

    private bool OpenConnection()
    {
        try
        {
            conn.Open();
            return true;
        }
        catch (MySqlException ex)
        {
            switch (ex.Number)
            {
                case 0:
                    MessageBox.Show("Cannot connect to server.");
                    break;

                case 1045:
                    MessageBox.Show("Invalid username/password, please try again.");
                    break;
            }
            return false;
        }
    }

    private bool CloseConnection()
    {
        try
        {
            conn.Close();
            return true;
        }
        catch (MySqlException ex)
        {
            MessageBox.Show(ex.Message);
            return false;
        }
    }
}
