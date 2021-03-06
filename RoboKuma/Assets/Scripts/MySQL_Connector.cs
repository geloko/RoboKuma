﻿using System;
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

    public MySQL_Connector(string localHost, string dbName, string dbUser, string dbPassword, string localPort)
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

		Debug.Log ("Before applying mySQLConnection");
		try{
			conn = new MySqlConnection (connectionString);
			Debug.Log("Applied MySqlConnection");
		}catch(System.NotSupportedException ex) {
			Debug.Log (ex.Message);
		}
    }

    public void syncPlayerData()
    {
        bool isNotUnique;
        string sql = "";
        string local_id = "";
        MySqlCommand cmd;
        MySqlDataReader dataReader;

		Debug.Log ("Syncing player data");

        if (OpenConnection() == true)
        {
            Player temp_player = SQLiteDatabase.getPlayer();

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

            dataReader.Close();

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

    public void uploadIfSynced(int is_synced)
    {
        if(is_synced == 1)
        {
            uploadPlayerLogs();
        } else
        {
            syncPlayerData();
            uploadPlayerLogs();
        }
    }

    public void uploadPlayerLogs()
    {
        MySqlCommand cmd;

        if (OpenConnection() == true)
        {
            List<PlayerLogs> uploadList = SQLiteDatabase.getLogsToUpload();
            int player_id = SQLiteDatabase.getPlayer().player_id;

            foreach (PlayerLogs log in uploadList)
            {
                string sql = "";

                sql = "INSERT INTO player_logs (player_id, test_id, time_start, time_end) VALUES(" +
                        player_id + ", " +
                        log.test_id + ", '" +
                        log.time_start + "', '" +
                        log.time_end + "');";

                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                sql = "SELECT LAST_INSERT_ID();";

                cmd.CommandText = sql;
                int last_log_id = Convert.ToInt32(cmd.ExecuteScalar());


                switch (log.test_id)
                {
                    case 1:
                        GoNoGoData temp1 = SQLiteDatabase.getGoNoGoToUpload(log.log_id);
                        sql = "INSERT INTO gonogo_data (log_id, correct_go_count, correct_nogo_count, mean_time, go_count, trial_count) VALUES (" +
                                last_log_id + ", " +
                                temp1.correct_go_count + ", " +
                                temp1.correct_nogo_count + ", " +
                                temp1.mean_time + ", " +
                                temp1.go_count + ", " +
                                temp1.trial_count + ");";
                        break;
                    case 2:
                        CorsiData temp2 = SQLiteDatabase.getCorsiToUpload(log.log_id);
                        sql = "INSERT INTO corsi_data (log_id, correct_trials, correct_length, seq_length, trial_count) VALUES (" +
                                last_log_id + ", " +
                                temp2.correct_trials + ", " +
                                temp2.correct_length + ", " +
                                temp2.seq_length + ", " +
                                temp2.trial_count + ");";
                        break;
                    case 3:
                        NBackData temp3 = SQLiteDatabase.getNBackToUpload(log.log_id);
                        sql = "INSERT INTO nback_data (log_id, mean_time, correct_count, n_count, element_count, trial_count) VALUES (" +
                                last_log_id + ", " +
                                temp3.mean_time + ", " +
                                temp3.correct_count + ", " +
                                temp3.n_count + ", " +
                                temp3.element_count + ", " +
                                temp3.trial_count + ");";
                        break;
                    case 4:
                        EriksenData temp4 = SQLiteDatabase.getEriksenToUpload(log.log_id);
                        sql = "INSERT INTO eriksen_data (log_id, correct_congruent, time_congruent, correct_incongruent, time_incongruent, congruent_count, trial_count) VALUES (" +
                                last_log_id + ", " +
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

                SQLiteDatabase.updateUploadedLog(log.log_id);
            }

            CloseConnection();
        }
    }

    private static bool OpenConnection()
    {
        try
        {
			Debug.Log("Trying to connect.Open()");
            conn.Open();
			Debug.Log("Successfully connected to database connection");
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
				default:
					Debug.Log("Unknown error:" + ex.Message);
					break;
            }
			Debug.Log ("Unable to open connection");
            return false;
		}
		catch(Exception ex)
		{
			Debug.Log (ex.Message);
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