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

    // Always Initialize First
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

    // Perform FIRST Once Online
    // Only perform ONCE
    private int syncPlayerData()
    {
        bool isNotUnique;
        string sql = "";
        string local_id = "";
        MySqlCommand cmd;
        MySqlDataReader dataReader;

        Player temp_player = SQLiteDatabase.getPlayer(1);

        do
        {
            Random generator = new Random();
            local_id = generator.Next(0, 1000000).ToString("D6");

            sql = "SELECT * FROM player WHERE local_id = '" + local_id + "';";
            cmd = new MySqlCommand(sql, connection);
            DataReader = cmd.ExecuteReader();

            if (dataReader.Read())
            {
                isNotUnique = true;
            }
            else
            {
                isNotUnique = false;
            }
        } while (isNotUnique);

        sql = "INSERT INTO player (local_id, date_start) VALUES ('" +
                local_id + "', '" + temp_player.date_start + "');";
        cmd = new MySqlCommand(sql, conn);
        cmd.ExecuteNonQuery();

        sql = "SELECT * FROM player WHERE local_id = '" + local_id + "';";
        cmd = new MySqlCommand(sql, connection);
        dataReader = cmd.ExecuteReader();

        if (dataReader.Read())
        {
            Player new_player = new Player();
            new_player.player_id = dataReader["player_id"];
            new_player.local_id = local_id;
            new_player.date_start = temp_player.date_start;

            SQLiteDatabase.updatePlayer(temp_player.player_id, new_player);
        }

        dataReader.Close();
        this.CloseConnection();
    }

    // Only Perform AFTER Syncing Player Data
    public void uploadPlayerLogs(int player_id)
    {
        MySqlCommand cmd;

        if (this.OpenConnection() == true)
        {
            List<PlayerLogs> uploadList = SQLiteDatabase.getLogsToUpload();

            foreach (PlayerLog log in uploadList)
            {
                string sql = "";

                sql = "INSERT INTO player_logs (player_id, test_id, time_start, time_end, prev_status, new_status, game_progress, is_uploaded) VALUES(" +
                        player_id + ", " +
                        log.test_id + ", '" +
                        log.time_start + "', '" +
                        log.time_end + "', '" +
                        log.prev_status + "', '" +
                        log.new_status + "', '" +
                        log.game_progress + "', " +
                        log.is_uploaded + ");";

                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                switch (log.test_id)
                {
                    case 1:
                        GoNoGoData temp = SQLiteDatabase.getGoNoGoToUpload(log.log_id);
                        sql = "INSERT INTO gonogo_data (log_id, correct_go_count, correct_nogo_count, mean_time, go_count, trial_count) VALUES (" +
                                temp.log_id + ", " +
                                temp.correct_go_count + ", " +
                                temp.correct_nogo_count + ", " +
                                temp.mean_time + ", " +
                                temp.go_count + ", " +
                                temp.trial_count + ");";
                        break;
                    case 2:
                        CorsiData temp = SQLiteDatabase.getCorsiToUpload(log.log_id);
                        sql = "INSERT INTO corsi_data (log_id, correct_trials, correct_length, seq_length, trial_count) VALUES (" +
                                temp.log_id + ", " +
                                temp.correct_trials + ", " +
                                temp.correct_length + ", " +
                                temp.seq_length + ", " +
                                temp.trial_count + ");";
                        break;
                    case 3:
                        NBackData temp = SQLiteDatabase.getNBackToUpload(log.log_id);
                        sql = "INSERT INTO nback_data (log_id, mean_time, correct_count, n_count, element_count, trial_count) VALUES (" +
                                temp.log_id + ", " +
                                temp.mean_time + ", " +
                                temp.correct_count + ", " +
                                temp.n_count + ", " +
                                temp.element_count + ", " +
                                temp.trial_count + ");";
                        break;
                    case 4:
                        EriksenData temp = SQLiteDatabase.getEriksenToUpload(log.log_id);
                        sql = "INSERT INTO eriksen_data (log_id, correct_congruent, time_congruent, correct_incongruent, time_incongruent, congruent_count, trial_count) VALUES (" +
                                temp.log_id + ", " +
                                temp.correct_congruent + ", " +
                                temp.time_congruent + ", " +
                                temp.correct_incongruent + ", " +
                                temp.time_incongruent + ", " +
                                temp.congruent_count + ", " +
                                temp.trial_count + ");";
                        break;
                }

                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }

            this.CloseConnection();
        }
    }

    // Execute Query Reference
    public static List<string> executeQuery(string query)
    {
        List<string> result = new List<string>();

        if (this.OpenConnection() == true)
        {
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                list[0].Add(dataReader["id"] + "");
                list[1].Add(dataReader["name"] + "");
                list[2].Add(dataReader["age"] + "");
            }

            dataReader.Close();
            this.CloseConnection();

            return result;
        }
    }

    // Execute Statement Reference
    public static void executeStatement(string statement)
    {
        if (this.OpenConnection() == true)
        {
            MySqlCommand cmd = new MySqlCommand(statement, conn);
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
