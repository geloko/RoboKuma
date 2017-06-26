using System;
using UnityEngine;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class MySQL_Connector 
{
    static MySqlConnection conn;
    private string localHost;
    private string localPort;
    private string dbUser;
    private string dbPassword;
    private string dbName;

    public MySQL_Connector(string localHost, string dbName, string dbUser, string dbPassword, string localPort )
    {
        this.localHost = localHost;
        this.dbName = dbName;
        this.dbUser = dbUser;
        this.dbPassword = dbPassword;
        this.localPort = localPort;

        Initialize();
    }


    private void Initialize()
    {
        string connectionString;
        connectionString = "SERVER=" + localHost + ";" + "DATABASE=" +
        dbName + ";" + "UID=" + dbUser + ";" + "PASSWORD=" + dbPassword + ";";

        conn = new MySqlConnection(connectionString);
    }

    public void syncPlayerData()
    {
        bool isNotUnique;
        string sql = "";
        string local_id = "";
        MySqlCommand cmd;
        MySqlDataReader dataReader;

        if (OpenConnection() == true)
        {
            Player temp_player = SQLiteDatabase.getPlayer(1);

            do
            {
                System.Random generator = new System.Random();
                local_id = generator.Next(0, 1000000).ToString("D6");

                sql = "SELECT * FROM player WHERE local_id = '" + local_id + "';";
                cmd = new MySqlCommand(sql, conn);
                dataReader = cmd.ExecuteReader();

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
            cmd = new MySqlCommand(sql, conn);
            dataReader = cmd.ExecuteReader();

            if (dataReader.Read())
            {
                Player new_player = new Player();
                new_player.player_id = dataReader.GetInt32(dataReader.GetOrdinal("player_id"));
                new_player.local_id = local_id;
                new_player.date_start = temp_player.date_start;

                SQLiteDatabase.updatePlayer(temp_player.player_id, new_player);
            }

            dataReader.Close();
            CloseConnection();
        }

    }

    public void uploadPlayerLogs(int player_id)
    {
        MySqlCommand cmd;

        if (OpenConnection() == true)
        {
            List<PlayerLogs> uploadList = SQLiteDatabase.getLogsToUpload();

            foreach (PlayerLogs log in uploadList)
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
                        GoNoGoData temp1 = SQLiteDatabase.getGoNoGoToUpload(log.log_id);
                        sql = "INSERT INTO gonogo_data (log_id, correct_go_count, correct_nogo_count, mean_time, go_count, trial_count) VALUES (" +
                                temp1.log_id + ", " +
                                temp1.correct_go_count + ", " +
                                temp1.correct_nogo_count + ", " +
                                temp1.mean_time + ", " +
                                temp1.go_count + ", " +
                                temp1.trial_count + ");";
                        break;
                    case 2:
                        CorsiData temp2 = SQLiteDatabase.getCorsiToUpload(log.log_id);
                        sql = "INSERT INTO corsi_data (log_id, correct_trials, correct_length, seq_length, trial_count) VALUES (" +
                                temp2.log_id + ", " +
                                temp2.correct_trials + ", " +
                                temp2.correct_length + ", " +
                                temp2.seq_length + ", " +
                                temp2.trial_count + ");";
                        break;
                    case 3:
                        NBackData temp3 = SQLiteDatabase.getNBackToUpload(log.log_id);
                        sql = "INSERT INTO nback_data (log_id, mean_time, correct_count, n_count, element_count, trial_count) VALUES (" +
                                temp3.log_id + ", " +
                                temp3.mean_time + ", " +
                                temp3.correct_count + ", " +
                                temp3.n_count + ", " +
                                temp3.element_count + ", " +
                                temp3.trial_count + ");";
                        break;
                    case 4:
                        EriksenData temp4 = SQLiteDatabase.getEriksenToUpload(log.log_id);
                        sql = "INSERT INTO eriksen_data (log_id, correct_congruent, time_congruent, correct_incongruent, time_incongruent, congruent_count, trial_count) VALUES (" +
                                temp4.log_id + ", " +
                                temp4.correct_congruent + ", " +
                                temp4.time_congruent + ", " +
                                temp4.correct_incongruent + ", " +
                                temp4.time_incongruent + ", " +
                                temp4.congruent_count + ", " +
                                temp4.trial_count + ");";
                        break;
                }

                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }

            CloseConnection();
        }
    }

    private static bool OpenConnection()
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
                    Debug.Log("Cannot connect to server.");
                    break;

                case 1045:
                    Debug.Log("Invalid username/password, please try again.");
                    break;
            }
            return false;
        }
    }

    private static bool CloseConnection()
    {
        try
        {
            conn.Close();
            return true;
        }
        catch (MySqlException ex)
        {
            Debug.Log(ex.Message);
            return false;
        }
    }
}