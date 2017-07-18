using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;
using System.Collections.Generic;
using System;
using System.IO;

public class SQLiteDatabase : MonoBehaviour {

	private static string _constr = "URI=file://" + Application.persistentDataPath + "/SQLite.db";
	private static IDbConnection _dbc;
	private static IDbCommand _dbcm;
	private static IDataReader _idr;
	private static string sql;

	// Use this for initialization
	public void Start () {
		_dbc = new SqliteConnection (_constr);
		_dbc.Open ();
		_dbcm = _dbc.CreateCommand ();
		//Attributes Table
		sql = "DROP TABLE IF EXISTS attributes; " +
		"CREATE TABLE attributes (" +
		"attribute_id INTEGER PRIMARY KEY, " +
		"attribute_name varchar(100) NOT NULL, " +
		"description TEXT" +
		")";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();

		//Tests Table
		sql = "DROP TABLE IF EXISTS tests; " +
		"CREATE TABLE tests (" +
		"test_id INTEGER PRIMARY KEY, " +
		"test_name varchar(100) NOT NULL, " +
		"description TEXT" +
		")";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();

		//Player Table
		sql = "DROP TABLE IF EXISTS player; " +
		"CREATE TABLE player (" +
		"player_id INTEGER PRIMARY KEY, " +
		"local_id varchar(45) NOT NULL, " +
		"date_start varchar(45) NOT NULL" +
		")";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();

		//Test-Attributes Table
		sql = "DROP TABLE IF EXISTS test_attributes; " +
		"CREATE TABLE test_attributes (" +
		"test_id INT PRIMARY KEY, " +
		"attribute_id INT NOT NULL, " +
		"FOREIGN KEY(test_id) REFERENCES tests(test_id), " +
		"FOREIGN KEY(attribute_id) REFERENCES attributes(attribute_id))";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();

		//Player Logs Table
		sql = "DROP TABLE IF EXISTS player_logs; " +
		"CREATE TABLE player_logs (" +
		"log_id INTEGER PRIMARY KEY, " +
		"player_id INT NOT NULL, " +
		"test_id INT NOT NULL, " +
		"time_start varchar(45) NOT NULL, " +
		"time_end varchar(45) NOT NULL, " +
		"prev_status varchar(100) NOT NULL, " +
		"new_status varchar(100) NOT NULL, " +
		"game_progress varchar(100) NOT NULL, " +
		"is_uploaded INT NOT NULL, " +
			"FOREIGN KEY(player_id) REFERENCES player(player_id), " +
		"FOREIGN KEY(test_id) REFERENCES tests(test_id)" +
		")";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();

		//Corsi Block Tapping Data Table
		sql = "DROP TABLE IF EXISTS corsi_data; " +
		"CREATE TABLE corsi_data (" +
		"log_id INTEGER PRIMARY KEY, " +
		"correct_trials INT NOT NULL, " +
		"correct_length INT NOT NULL, " +
		"seq_length INT NOT NULL, " +
		"trial_count INT NOT NULL, " +
		"FOREIGN KEY(log_id) REFERENCES player_logs(log_id)" +
		")";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();

		//Eriksen Flanker Data Table
		sql = "DROP TABLE IF EXISTS eriksen_data; " +
		"CREATE TABLE eriksen_data (" +
		"log_id INTEGER PRIMARY KEY, " +
		"correct_congruent INT NOT NULL, " +
		"time_congruent FLOAT NOT NULL, " +
		"correct_incongruent INT NOT NULL, " +
		"time_incongruent FLOAT NOT NULL, " +
		"congruent_count INT NOT NULL, " +
		"trial_count INT NOT NULL, " +
		"FOREIGN KEY(log_id) REFERENCES player_logs(log_id)" +
		")";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();

		//Go No Go Data Table
		sql = "DROP TABLE IF EXISTS gonogo_data; " +
		"CREATE TABLE gonogo_data (" +
		"log_id INTEGER PRIMARY KEY, " +
		"correct_go_count INT NOT NULL, " +
		"correct_nogo_count INT NOT NULL, " +
		"mean_time FLOAT NOT NULL, " +
		"go_count INT NOT NULL, " +
		"trial_count INT NOT NULL, " +
		"FOREIGN KEY(log_id) REFERENCES player_logs(log_id)" +
		")";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();

		//Nback Data Table
		sql = "DROP TABLE IF EXISTS nback_data; " +
		"CREATE TABLE nback_data (" +
		"log_id INTEGER PRIMARY KEY, " +
		"mean_time FLOAT NOT NULL, " +
		"correct_count INT NOT NULL, " +
		"n_count INT NOT NULL, " +
		"element_count INT NOT NULL, " +
		"trial_count INT NOT NULL, " +
		"FOREIGN KEY(log_id) REFERENCES player_logs(log_id)" +
		")";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();

		//Player Local Data Table
		sql = "DROP TABLE IF EXISTS player_data_table; " +
		"CREATE TABLE player_data_table (" +
		"player_id INTEGER PRIMARY KEY, " +
		"player_name VARCHAR(100) NOT NULL, " +
		"memory INT NOT NULL, " +
		"accuracy INT NOT NULL, " +
		"speed INT NOT NULL, " +
		"response INT NOT NULL, " +
		"FOREIGN KEY(player_id) REFERENCES player(player_id)" +
		")";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();

		//Inserting default data
		sql = "INSERT INTO player_data_table (player_name, memory, accuracy, speed, response) VALUES ('PlayerName', 0 ,0 ,0, 0);";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();
		//Get current player_id
		sql = "SELECT last_insert_rowid();";
		_dbcm.CommandText = sql;
		Int64 generated_player_id64 = -999;
		int generated_player_id32;
		generated_player_id64 = (Int64) _dbcm.ExecuteScalar ();

		generated_player_id32 = Convert.ToInt32 (generated_player_id64);
		PlayerPrefs.SetInt ("player_id", generated_player_id32);
		Debug.Log ("Generated player ID:" + generated_player_id32 + "");

		//Inserting attributes data
		sql = "INSERT INTO attributes (attribute_name) VALUES ('memory');";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();
		sql = "INSERT INTO attributes (attribute_name) VALUES ('accuracy');";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();
		sql = "INSERT INTO attributes (attribute_name) VALUES ('speed');";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();
		sql = "INSERT INTO attributes (attribute_name) VALUES ('response');";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();

		//Inserting tests data
		sql = "INSERT INTO tests (test_name) VALUES ('gonogo');"; //test_id = 1;
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();
		sql = "INSERT INTO tests (test_name) VALUES ('corsiblocktapping');"; // test_id = 2;
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();
		sql = "INSERT INTO tests (test_name) VALUES ('nback');"; //test_id = 3;
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery ();
		sql = "INSERT INTO tests (test_name) VALUES ('eriksenflanker');"; //test_id = 4;
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();

		_dbcm.Dispose ();
		_dbcm =	 null;
		_dbc.Close ();
		_dbc = null;

        Debug.Log("Started");

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int insertintoPlayerLog(int player_id, int test_id, string time_start, string time_end, string prev_status, string new_status, string game_progress, int is_uploaded)
	{
		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		sql = "INSERT INTO player_logs (player_id, test_id, time_start, time_end, prev_status, new_status, game_progress, is_uploaded) VALUES(" +
			player_id + ", " + test_id + ", '" + time_start + "', '" + time_end + "', '" + prev_status + "', '" + new_status + "', '" + game_progress + "', " + is_uploaded + ");";
		_dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();

		Debug.Log ("INSERT INTO player_logs SQL Command: " + sql);

		sql = "SELECT last_insert_rowid();";
		_dbcm.CommandText = sql;
		Int64 generated_log_id64 = -999;
		int generated_log_id32;
		generated_log_id64 = (Int64) _dbcm.ExecuteScalar ();

		generated_log_id32 = Convert.ToInt32 (generated_log_id64);
		Debug.Log ("Generated Log ID: " + generated_log_id32 + "");

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		return generated_log_id32;
	}

	public void updatePlayerLog(int log_id, string time_end, string new_status, string game_progress)
	{
		_dbc=new SqliteConnection(_constr);
		_dbc.Open();
		sql = "UPDATE player_logs SET time_end = '" + time_end + "', new_status = '" + new_status + "', game_progress = '" + game_progress + "' WHERE log_id = " + log_id + ";";
		_dbcm=_dbc.CreateCommand();
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();

		Debug.Log ("UPDATE player_logs SQL Command: " + sql);

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
	}

	public void insertintoPlayer(int player_id, int local_id, string date_start)
	{
		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		sql = "INSERT INTO player (player_id, local_id, date_start) VALUES (" + 
			player_id + ", '" + local_id + "', '" + date_start + "');";
		_dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();

		Debug.Log ("INSERT INTO player SQL Command: " + sql);

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
	}

	public static Player getPlayer()
	{
		Player temp = null;
		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		sql = "SELECT * FROM player;";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
			temp = new Player();
			temp.player_id = _idr.GetInt32(_idr.GetOrdinal("player_id"));
            temp.local_id = _idr["local_id"].ToString();
			temp.date_start = _idr["date_start"].ToString();
		}

		_idr.Close ();
		_idr = null;
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		return temp;
	}

	public static void updatePlayer(int old_id, Player player)
	{
		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		sql = "UPDATE player SET player_id = " + player.player_id + ", local_id = '" + player.local_id + "' WHERE player_id = " + old_id + ";";
		_dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		updateLogsPlayerID(old_id, player.player_id);
        PlayerPrefs.SetInt("player_id", player.player_id);
    }

    public static void updateLogsPlayerID(int old_id, int new_id)
    {
        _dbc = new SqliteConnection(_constr);
        _dbc.Open();

        _dbcm = _dbc.CreateCommand();
        sql = "SELECT * FROM player_logs WHERE player_id = " + old_id + ";";
        _dbcm.CommandText = sql;
        _idr = _dbcm.ExecuteReader();

        while (_idr.Read())
        {
            sql = "UPDATE player_logs SET player_id = " + new_id + " WHERE log_id = " + _idr["log_id"] + ";";
            _dbcm = _dbc.CreateCommand();
            _dbcm.CommandText = sql;
            _dbcm.ExecuteNonQuery();
        }

        _idr.Close();
        _idr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;
    }

    public void insertintoCorsi(int player_id, int log_id, int correct_trials, int correct_length, int seq_length, int trial_count)
	{
		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		sql = "INSERT INTO corsi_data (log_id, correct_trials, correct_length, seq_length, trial_count) VALUES (" + 
			log_id + ", " + correct_trials + ", " + correct_length + ", " + seq_length + ", " + trial_count + ");";
		_dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();

		Debug.Log ("INSERT INTO corsi_data SQL Command: " + sql);

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
			
        updatePlayerStatistics(correct_length, 0, seq_length, 0.001, null, trial_count, "corsi", player_id);
        //Debug.Log("Updated");
	}

	public void insertintoEriksen(int player_id, int log_id, int correct_congruent, double time_congruent, int correct_incongruent, double time_incongruent, double[] time, int congruent_count, int trial_count)
	{
		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		Debug.Log ("INSERT INTO eriksen_data VALUES: " + player_id + ", " + log_id + ", " + correct_congruent + ", " + time_congruent + ", " + correct_incongruent + ", " + time_incongruent + ", " + congruent_count + ", " + trial_count);
		sql = "INSERT INTO eriksen_data (log_id, correct_congruent, time_congruent, correct_incongruent, time_incongruent, congruent_count, trial_count) VALUES (" + 
			log_id + ", " + correct_congruent + ", " + time_congruent + ", " + correct_incongruent + ", " + time_incongruent + ", " + 
			congruent_count + ", " + trial_count + ");";
		_dbcm = _dbc.CreateCommand();

		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		Debug.Log (sql);

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		updatePlayerStatistics(correct_congruent + correct_incongruent, 0, trial_count, (time_congruent + time_incongruent)/2, time, 1, "eriksen", player_id);
	}

	public void insertintoGoNoGo(int player_id, int log_id, int correct_go_count, int correct_nogo_count, double mean_time, double[] time, int go_count, int trial_count)
	{
		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		sql = "INSERT INTO gonogo_data (log_id, correct_go_count, correct_nogo_count, mean_time, go_count, trial_count) VALUES (" + 
			log_id + ", " + correct_go_count + ", " + correct_nogo_count + ", " + mean_time + ", " + 
			go_count + ", " + trial_count + ");";

		_dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		Debug.Log (sql);

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		updatePlayerStatistics(correct_go_count + correct_nogo_count, correct_go_count, trial_count, mean_time, time, 1, "gonogo", player_id);
	}

	public void insertintoNback(int player_id, int log_id, double mean_time, int correct_count, int n_count, int element_count, int trial_count)
	{
		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		sql = "INSERT INTO nback_data (log_id, mean_time, correct_count, n_count, element_count, trial_count) VALUES (" + 
			log_id + ", " + mean_time + ", " + correct_count + ", " + n_count + ", " + 
			element_count + ", " + trial_count + ");";

		_dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		Debug.Log (sql);

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		updatePlayerStatistics(correct_count, 0, trial_count, mean_time, null, n_count, "nback", player_id);
	}

	public void select (string table)
	{
		sql = "SELECT * FROM " + table;
		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader ();
		while (_idr.Read ()) {
			int correct = _idr.GetInt32 (2);
			int total = _idr.GetInt32 (3);
			Debug.Log (correct + "/" + total);
		}

		_idr.Close ();
		_idr = null;
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
	}

	public int[] getPlayerStatistics(int player_id)
	{
		sql = "SELECT * FROM player_data_table WHERE player_id = " + player_id;
		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader ();

		int oResponse = 0, oSpeed = 0, oAccuracy = 0, oMemory = 0;
		while (_idr.Read ()) {
			oMemory = _idr.GetInt32 (2);
			oAccuracy = _idr.GetInt32 (3);
			oSpeed = _idr.GetInt32 (4);
			oResponse = _idr.GetInt32 (5);
            Debug.Log ("Memory: " + oMemory);
            Debug.Log ("Accuracy: " + oAccuracy);
            Debug.Log ("Speed: " + oSpeed);
            Debug.Log ("Response: " + oResponse);
        }
        Debug.Log(sql);
        Debug.Log(_idr);
        int[] result = new int[4] { oMemory, oAccuracy, oSpeed, oResponse };

        return result;
	}


	public void updatePlayerStatistics(int correct, int correct_nogo, int total, double aveTime, double[] timeStamps, int difficulty, string table, int player_id)
	{
		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		sql = "SELECT * FROM player_data_table WHERE player_id = " + player_id + ";";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader ();
		int oResponse = 0, oSpeed = 0, oAccuracy = 0, oMemory = 0;
        int fasterThanAveCnt = 0;

        aveTime += 0.001;

		while (_idr.Read ()) {
			oMemory = _idr.GetInt32 (2);
			oAccuracy = _idr.GetInt32 (3);
			oSpeed = _idr.GetInt32 (4);
			oResponse = _idr.GetInt32 (5);
			//Debug.Log ("Memory: " + oMemory);
			//Debug.Log ("Accuracy: " + oAccuracy);
			//Debug.Log ("Speed: " + oSpeed);
			//Debug.Log ("Response: " + oResponse);
		}
		double response = 0, speed, accuracy, memory, pCorrect, pWrong;
		int newResponse, newSpeed, newAccuracy, newMemory;
		switch (table) 
		{
		    case "gonogo":
                double gngSD = 0;

                for(int i = 0; i < timeStamps.Length; i++)
                {
                    gngSD += Math.Pow(timeStamps[i] - aveTime, 2);
                }

                gngSD = Math.Sqrt(gngSD / timeStamps.Length);

                /*if (getAvgGoNoGo()[2] == null)
                {
                    gngAveTime = aveTime;
                }
                else
                {
                    gngAveTime = ((double)getAvgGoNoGo()[2] + aveTime) / 2;
                }

                double gngAveTimeSq = gngAveTime * gngAveTime;
                gngAveTimeSq *= count("gonogo");

                double gngSD = Math.Sqrt(((Double)getAvgGoNoGo()[3] - gngAveTimeSq) / (count("gonogo") + 1));

                _dbc = new SqliteConnection(_constr);
                _dbc.Open();
                _dbcm = _dbc.CreateCommand();*/



                for (int i = 0; i < timeStamps.Length; i++)
                {
                    if (timeStamps[i] != -1 && (timeStamps[i] <= aveTime + gngSD))
                    {
                        fasterThanAveCnt++;
                    }
                }
                Debug.Log("FTAC:: " + fasterThanAveCnt);
                pCorrect = correct * 1.0 / total;
			    pWrong = (total - correct) * 1.0 / total;
                response = (fasterThanAveCnt + correct_nogo * 1.0) / total * 1.5;
                Debug.Log("Average Speed:" + aveTime);
			    speed = (difficulty + 600) / aveTime;
			    accuracy = (pCorrect - pWrong) * difficulty * 2;
			    newResponse = oResponse + Convert.ToInt32 (response);
			    newSpeed = oSpeed + Convert.ToInt32 (speed);
			    newAccuracy = oAccuracy + Convert.ToInt32 (accuracy);
			    sql = "UPDATE player_data_table SET response = " + newResponse + " , speed = " + newSpeed + " , accuracy = " + newAccuracy + " WHERE player_id = " + player_id + ";";
					    break;
		    case "eriksen":
                double eriksenSD = 0;

                for (int i = 0; i < timeStamps.Length; i++)
                {
                    eriksenSD += Math.Pow(timeStamps[i] - aveTime, 2);
                }

                eriksenSD = Math.Sqrt(eriksenSD / timeStamps.Length);
                /*double eriksenAveTime;
                if (getAvgEriksen()[4] == null)
                {
                        eriksenAveTime = aveTime;
                }
                else
                {

                    eriksenAveTime = ((Int32)getAvgEriksen()[4] + aveTime) / 2 ;
                }

                double eriksenAveTimeSq = eriksenAveTime * eriksenAveTime;
                eriksenAveTimeSq *= count("eriksen");

                double eriksenSD = Math.Sqrt(((Double)getAvgEriksen()[5] - eriksenAveTimeSq) / (count("eriksen") + 1));

                _dbc = new SqliteConnection(_constr);
                _dbc.Open();
                _dbcm = _dbc.CreateCommand();*/

                for (int i = 0; i < timeStamps.Length; i++)
                {
                    Debug.Log("Time: " + timeStamps[i] + " Average: " + (aveTime + eriksenSD));
                    if (timeStamps[i] != -1 && timeStamps[i] <= (aveTime + eriksenSD))
                    {
                        fasterThanAveCnt++;
                    }
                }
                Debug.Log("FTAC:: " + fasterThanAveCnt);
                pCorrect = correct * 1.0 / total;
			    pWrong = (total - correct) * 1.0 / total;
			    response = (fasterThanAveCnt * 1.0) / total * 1.5;
			    speed = (difficulty + 500) / aveTime;
			    accuracy = (pCorrect - pWrong) * difficulty * 2;
			    newResponse = oResponse + Convert.ToInt32 (response);
			    newSpeed = oSpeed + Convert.ToInt32 (speed);
			    newAccuracy = oAccuracy + Convert.ToInt32 (accuracy);
			    sql = "UPDATE player_data_table SET response = " + newResponse + " , speed = " + newSpeed + " , accuracy = " + newAccuracy + " WHERE player_id = " + player_id + ";";
						    break;
		    case "corsi":
			    pCorrect = correct * 1.0 / total;
                pWrong = (total - correct) * 1.0 / total;
                memory = (pCorrect - pWrong) * difficulty * 2;
                    //if (pCorrect == 1) {
                    newMemory = oMemory + Convert.ToInt32 (memory);
			    /*} else {
				    newMemory = oMemory - Convert.ToInt32 (memory);
			    }*/
			    sql = "UPDATE player_data_table SET memory = " + newMemory + " WHERE player_id = " + player_id + ";";
                Debug.Log(sql);
                break;
		    case "nback":
			    pCorrect = correct * 1.0 / total;
			    pWrong = (total - correct) * 1.0 / total;
			    memory = (pCorrect - pWrong) * difficulty * 2;
			    speed = (difficulty + 700) / aveTime;
			    accuracy = (pCorrect - pWrong) * difficulty * 2;
			    newMemory = oMemory + Convert.ToInt32 (memory);
			    newSpeed = oSpeed + Convert.ToInt32 (speed);
			    newAccuracy = oAccuracy + Convert.ToInt32 (accuracy);
			    sql = "UPDATE player_data_table SET memory = " + newMemory + " , speed = " + newSpeed + " , accuracy = " + newAccuracy + " WHERE player_id = " + player_id + ";";
			    break;
		    default:
    			break;

        }
        if(_idr != null)
		    _idr.Close ();
		_idr = null;
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
	}

	public int count(String gameName)
	{
		if (gameName == "gonogo") {
			sql = "SELECT COUNT(*) FROM player_logs where test_id = 1 AND game_progress = \"FINISHED\"";
		} else if(gameName == "corsi") {
			sql = "SELECT COUNT(*) FROM player_logs where test_id = 2 AND game_progress = \"FINISHED\"";
		} else if(gameName == "nback"){
			sql = "SELECT COUNT(*) FROM player_logs where test_id = 3 AND game_progress = \"FINISHED\"";
		} else if(gameName == "eriksen"){
			sql = "SELECT COUNT(*) FROM player_logs where test_id = 4 AND game_progress = \"FINISHED\"";
		}
        
		_dbcm.CommandText = sql;
		Int64 generated_count64 = -999;
		int generated_count32;
		generated_count64 = (Int64) _dbcm.ExecuteScalar ();

		generated_count32 = Convert.ToInt32 (generated_count64);
		Debug.Log ("Generated Count:" + generated_count32 + "");

		return generated_count32;
	}

	public int countToday(String gameName)
	{
		if (gameName == "gonogo") {
			sql = "SELECT COUNT(*) FROM player_logs where test_id = 1 AND time_end LIKE '%" + System.DateTime.Now.Date.ToString("d") + "%' AND game_progress = 'FINISHED'";
		} else if(gameName == "corsi") {
			sql = "SELECT COUNT(*) FROM player_logs where test_id = 2 AND time_end LIKE '%" + System.DateTime.Now.Date.ToString("d") + "%' AND game_progress = 'FINISHED'";
		} else if(gameName == "nback"){
			sql = "SELECT COUNT(*) FROM player_logs where test_id = 3 AND time_end LIKE '%" + System.DateTime.Now.Date.ToString("d") + "%' AND game_progress = 'FINISHED'";
		} else if(gameName == "eriksen"){
			sql = "SELECT COUNT(*) FROM player_logs where test_id = 4 AND time_end LIKE '%" + System.DateTime.Now.Date.ToString("d") + "%' AND game_progress = 'FINISHED'";
		}

		_dbcm.CommandText = sql;
		Int64 generated_count64 = -999;
		int generated_count32;
		generated_count64 = (Int64) _dbcm.ExecuteScalar ();
		Debug.Log ("SQL Count Today Statement:" + sql + "");
		generated_count32 = Convert.ToInt32 (generated_count64);
		Debug.Log ("Generated Count:" + generated_count32 + "");

		return generated_count32;
	}

    public void updateAttributes(int[] attributes)
    {
        _dbc = new SqliteConnection(_constr);
        _dbc.Open();
        _dbcm = _dbc.CreateCommand();

        sql = "UPDATE player_data_table SET memory = " + attributes[0] + ", response = " + attributes[3] + " , speed = " + attributes[2] + " , accuracy = " + attributes[1] + " WHERE player_id = " + 1 + ";";

        //_idr.Close();
        //_idr = null;
        _dbcm.CommandText = sql;
        _dbcm.ExecuteNonQuery();
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;
    }

    public string getMostPlayed()
	{
		string mostPlayed = "N/A";

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		sql = "SELECT T.test_name FROM tests T, " +
              "(SELECT test_id, COUNT(*), game_progress FROM player_logs " +
              "GROUP BY test_id ORDER BY COUNT(*) DESC LIMIT 1) as L " + 
              "WHERE T.test_id = L.test_id " +
              "AND L.game_progress = 'FINISHED'; ";
        _dbcm.CommandText = sql;
        _dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
			mostPlayed = _idr["test_name"].ToString();
		}

		_idr.Close ();
		_idr = null;
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		return mostPlayed;
	}

	public string getLeastPlayed()
	{
		string leastPlayed = "N/A";

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		sql = "SELECT T.test_name FROM tests T, " +
              "(SELECT test_id, COUNT(*), game_progress FROM player_logs " +
              "GROUP BY test_id ORDER BY COUNT(*) ASC LIMIT 1) as L " +
              "WHERE T.test_id = L.test_id " +
              "AND L.game_progress = 'FINISHED'; ";
        _dbcm.CommandText = sql;
        _dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
			leastPlayed = _idr["test_name"].ToString();
		}

		_idr.Close ();
		_idr = null;
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		return leastPlayed;
	}

	public string getAverageStatus()
	{
		string averageStatus = "N/A";

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		sql = "SELECT new_status FROM player_logs GROUP BY new_status ORDER BY COUNT(*) LIMIT 1;";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
			averageStatus = _idr["new_status"].ToString();
		}

		_idr.Close ();
		_idr = null;
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		return averageStatus;
	}

	public GoNoGoData getBestGoNoGo()
	{
		GoNoGoData bestGoNoGo = null;

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		sql = "SELECT G.log_id, G.correct_go_count, G.correct_nogo_count, G.mean_time, G.go_count, G.trial_count " +
                "FROM gonogo_data G, player_logs P " +
                "WHERE P.game_progress = 'FINISHED' " +
                "AND P.log_id = G.log_id " +
                "AND(G.correct_go_count + G.correct_nogo_count) != 0 " +
                "ORDER BY G.trial_count DESC, (G.correct_go_count + G.correct_nogo_count) DESC, G.mean_time ASC LIMIT 1; ";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
			bestGoNoGo = new GoNoGoData();
			bestGoNoGo.log_id = _idr.GetInt32(_idr.GetOrdinal("log_id"));
			bestGoNoGo.correct_go_count = _idr.GetInt32(_idr.GetOrdinal("correct_go_count")); ;
			bestGoNoGo.correct_nogo_count = _idr.GetInt32(_idr.GetOrdinal("correct_nogo_count"));
            bestGoNoGo.mean_time = _idr.GetDouble(_idr.GetOrdinal("mean_time"));
			bestGoNoGo.go_count = _idr.GetInt32(_idr.GetOrdinal("go_count")); ;
			bestGoNoGo.trial_count = _idr.GetInt32(_idr.GetOrdinal("trial_count")); ;
		}

		_idr.Close ();
		_idr = null;
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		return bestGoNoGo;
	}

    public object[] getAvgGoNoGo()
    {
        object[] avgGoNoGo = new object[3];

        _dbc = new SqliteConnection(_constr);
        _dbc.Open();
        _dbcm = _dbc.CreateCommand();
        sql = "SELECT G.trial_count, COUNT(G.log_id), AVG(G.mean_time), AVG((G.mean_time * G.mean_time)) " +
                "FROM gonogo_data G, player_logs P " +
                "WHERE P.game_progress = 'FINISHED' " +
                "AND P.log_id = G.log_id " +
                "GROUP BY G.trial_count " +
                "ORDER BY COUNT(G.log_id) DESC LIMIT 1;";
        _dbcm.CommandText = sql;
        _idr = _dbcm.ExecuteReader();

        if (_idr.Read())
        {
            avgGoNoGo[0] = _idr.GetInt32(_idr.GetOrdinal("trial_count"));
            avgGoNoGo[1] = _idr.GetInt32(_idr.GetOrdinal("COUNT(G.log_id)"));
            avgGoNoGo[2] = _idr.GetDouble(_idr.GetOrdinal("AVG(G.mean_time)"));
            avgGoNoGo[2] = _idr.GetDouble(_idr.GetOrdinal("AVG((G.mean_time * G.mean_time))"));
        }

        _idr.Close();
        _idr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;

        return avgGoNoGo;
    }

	public NBackData getBestNBack()
	{
		NBackData bestNBack = null;

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
        sql = "SELECT N.log_id, N.mean_time, N.correct_count, N.n_count, N.element_count, N.trial_count " +
                "FROM nback_data N, player_logs P " +
                "WHERE P.game_progress = 'FINISHED' " +
                "AND P.log_id = N.log_id " +
                "AND N.correct_count != 0 " +
                "ORDER BY N.n_count DESC, N.correct_count DESC, N.mean_time ASC LIMIT 1; ";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
			bestNBack = new NBackData();
			bestNBack.log_id = _idr.GetInt32(_idr.GetOrdinal("log_id"));
			bestNBack.mean_time = _idr.GetDouble(_idr.GetOrdinal("mean_time"));
            bestNBack.correct_count = _idr.GetInt32(_idr.GetOrdinal("correct_count"));
            bestNBack.n_count = _idr.GetInt32(_idr.GetOrdinal("n_count"));
            bestNBack.element_count = _idr.GetInt32(_idr.GetOrdinal("element_count"));
            bestNBack.trial_count = _idr.GetInt32(_idr.GetOrdinal("trial_count"));
        }

		_idr.Close ();
		_idr = null;
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		return bestNBack;
	}

    public NBackData[] getLastNBack()
    {
        NBackData nbackData;
        NBackData[] lastNBack = new NBackData[3];
        int count = 0;

        _dbc = new SqliteConnection(_constr);
        _dbc.Open();
        _dbcm = _dbc.CreateCommand();
        sql = "SELECT N.log_id, N.mean_time, N.correct_count, N.n_count, N.element_count, N.trial_count " +
                "FROM nback_data N, player_logs P " +
                "WHERE P.game_progress = 'FINISHED' " +
                "AND P.log_id = N.log_id " +
                "ORDER BY N.log_id DESC; ";
        _dbcm.CommandText = sql;
        _idr = _dbcm.ExecuteReader();

        while (_idr.Read() && count < 3)
        {
            nbackData = new NBackData();
            nbackData.log_id = _idr.GetInt32(_idr.GetOrdinal("log_id"));
            nbackData.mean_time = _idr.GetDouble(_idr.GetOrdinal("mean_time"));
            nbackData.correct_count = _idr.GetInt32(_idr.GetOrdinal("correct_count"));
            nbackData.n_count = _idr.GetInt32(_idr.GetOrdinal("n_count"));
            nbackData.element_count = _idr.GetInt32(_idr.GetOrdinal("element_count"));
            nbackData.trial_count = _idr.GetInt32(_idr.GetOrdinal("trial_count"));
            lastNBack[count] = nbackData;
            count++;
        }

        _idr.Close();
        _idr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;

        return lastNBack;
    }

    public object[] getAvgNBack()
    {
        object[] avgNBack = new object[3];

        _dbc = new SqliteConnection(_constr);
        _dbc.Open();
        _dbcm = _dbc.CreateCommand();
        sql = "SELECT N.n_count, COUNT(N.log_id), AVG(N.correct_count / N.trial_count) " +
                "FROM nback_data N, player_logs P " +
                "WHERE P.game_progress = 'FINISHED' " +
                "AND P.log_id = N.log_id " +
                "GROUP BY N.n_count " +
                "ORDER BY COUNT(N.log_id) DESC LIMIT 1;";
        _dbcm.CommandText = sql;
        _idr = _dbcm.ExecuteReader();

        if (_idr.Read())
        {
            avgNBack[0] = _idr.GetInt32(_idr.GetOrdinal("n_count"));
            avgNBack[1] = _idr.GetInt32(_idr.GetOrdinal("COUNT(N.log_id)"));
            avgNBack[2] = _idr.GetInt32(_idr.GetOrdinal("AVG(N.correct_count / N.trial_count)"));
        }

        _idr.Close();
        _idr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;

        return avgNBack;
    }

    public CorsiData getBestCorsi()
	{
		CorsiData bestCorsi = null;

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
        sql = "SELECT C.log_id, C.correct_trials, C.correct_length, C.seq_length, C.trial_count " +
                "FROM corsi_data C, player_logs P " +
                "WHERE P.game_progress = 'FINISHED' " +
                "AND P.log_id = C.log_id " +
                "AND C.correct_length != 0 " +
                "ORDER BY C.trial_count DESC, C.correct_trials DESC, C.seq_length DESC, C.correct_length DESC LIMIT 1; ";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
			bestCorsi = new CorsiData();
			bestCorsi.log_id = _idr.GetInt32(_idr.GetOrdinal("log_id"));
            bestCorsi.correct_length = _idr.GetInt32(_idr.GetOrdinal("correct_length"));
            bestCorsi.correct_trials = _idr.GetInt32(_idr.GetOrdinal("correct_trials"));
            bestCorsi.seq_length = _idr.GetInt32(_idr.GetOrdinal("seq_length"));
            bestCorsi.trial_count = _idr.GetInt32(_idr.GetOrdinal("trial_count"));
        }

		_idr.Close ();
		_idr = null;
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		return bestCorsi;
	}

    public object[] getAvgCorsi()
    {
        object[] avgCorsi = new object[3];

        _dbc = new SqliteConnection(_constr);
        _dbc.Open();
        _dbcm = _dbc.CreateCommand();
        sql = "SELECT C.seq_length, COUNT(C.log_id), AVG(C.correct_length / C.seq_length) " +
                "FROM corsi_data C, player_logs P " +
                "WHERE P.game_progress = 'FINISHED' " +
                "AND P.log_id = C.log_id " +
                "GROUP BY C.seq_length " +
                "ORDER BY COUNT(C.log_id) DESC LIMIT 1;";
        _dbcm.CommandText = sql;
        _idr = _dbcm.ExecuteReader();

        if (_idr.Read())
        {
            avgCorsi[0] = _idr.GetInt32(_idr.GetOrdinal("seq_length"));
            avgCorsi[1] = _idr.GetInt32(_idr.GetOrdinal("COUNT(C.log_id)"));
            avgCorsi[2] = _idr.GetInt32(_idr.GetOrdinal("AVG(C.correct_length / C.seq_length)"));
        }

        _idr.Close();
        _idr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;

        return avgCorsi;
    }

    public CorsiData[] getLastCorsi()
    {
        CorsiData[] lastCorsi = new CorsiData[3];
        CorsiData corsiData;
        int count = 0;

        _dbc = new SqliteConnection(_constr);
        _dbc.Open();
        _dbcm = _dbc.CreateCommand();
        sql = "SELECT C.log_id, C.correct_trials, C.correct_length, C.seq_length, C.trial_count " +
                "FROM corsi_data C, player_logs P " +
                "WHERE P.game_progress = 'FINISHED' " +
                "AND P.log_id = C.log_id " +
                "ORDER BY C.log_id DESC; ";
        _dbcm.CommandText = sql;
        _idr = _dbcm.ExecuteReader();

        while (_idr.Read() && count < 3)
        {
            corsiData = new CorsiData();
            corsiData.log_id = _idr.GetInt32(_idr.GetOrdinal("log_id"));
            corsiData.correct_length = _idr.GetInt32(_idr.GetOrdinal("correct_length"));
            corsiData.correct_trials = _idr.GetInt32(_idr.GetOrdinal("correct_trials"));
            corsiData.seq_length = _idr.GetInt32(_idr.GetOrdinal("seq_length"));
            corsiData.trial_count = _idr.GetInt32(_idr.GetOrdinal("trial_count"));
            lastCorsi[count] = corsiData;
            count++;
        }

        _idr.Close();
        _idr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;

        return lastCorsi;
    }

    public EriksenData getBestEriksen()
	{
		EriksenData bestEriksen = null;

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
        sql = "SELECT E.log_id, E.correct_congruent, E.time_congruent, E.correct_incongruent, E.time_incongruent, E.congruent_count, E.trial_count " +
                "FROM eriksen_data E, player_logs P " +
                "WHERE P.game_progress = 'FINISHED' " +
                "AND P.log_id = E.log_id " +
                "AND(E.correct_congruent + E.correct_incongruent) != 0 " +
                "ORDER BY E.trial_count DESC, (E.correct_congruent + E.correct_incongruent) DESC, E.time_congruent ASC, E.time_incongruent ASC LIMIT 1; ";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
			bestEriksen = new EriksenData();
			bestEriksen.log_id = _idr.GetInt32(_idr.GetOrdinal("log_id"));
            bestEriksen.correct_congruent = _idr.GetInt32(_idr.GetOrdinal("correct_congruent"));
            bestEriksen.time_congruent = _idr.GetDouble(_idr.GetOrdinal("time_congruent"));
            bestEriksen.correct_incongruent = _idr.GetInt32(_idr.GetOrdinal("correct_incongruent"));
            bestEriksen.time_incongruent = _idr.GetDouble(_idr.GetOrdinal("time_incongruent"));
            bestEriksen.congruent_count = _idr.GetInt32(_idr.GetOrdinal("congruent_count"));
            bestEriksen.trial_count = _idr.GetInt32(_idr.GetOrdinal("trial_count"));
        }

		_idr.Close ();
		_idr = null;
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		return bestEriksen;
	}

    public object[] getAvgEriksen()
    {
        object[] avgEriksen = new object[5];

        _dbc = new SqliteConnection(_constr);
        _dbc.Open();
        _dbcm = _dbc.CreateCommand();
        sql = "SELECT E.trial_count, COUNT(E.log_id), AVG(E.time_congruent), AVG(E.time_incongruent), AVG(E.correct_congruent + E.correct_incongruent), AVG((E.correct_congruent + E.correct_incongruent) * (E.correct_congruent + E.correct_incongruent)) " +
                "FROM eriksen_data E, player_logs P " +
                "WHERE P.game_progress = 'FINISHED' " +
                "AND P.log_id = E.log_id " +
                "GROUP BY E.trial_count " +
                "ORDER BY COUNT(E.log_id) DESC LIMIT 1; ";
        _dbcm.CommandText = sql;
        _idr = _dbcm.ExecuteReader();

        if (_idr.Read())
        {
            avgEriksen[0] = _idr.GetInt32(_idr.GetOrdinal("trial_count"));
            avgEriksen[1] = _idr.GetInt32(_idr.GetOrdinal("COUNT(E.log_id)"));
            avgEriksen[2] = _idr.GetDouble(_idr.GetOrdinal("AVG(E.time_congruent)"));
            avgEriksen[3] = _idr.GetDouble(_idr.GetOrdinal("AVG(E.time_incongruent)"));
            avgEriksen[4] = _idr.GetInt32(_idr.GetOrdinal("AVG(E.correct_congruent + E.correct_incongruent)"));
            avgEriksen[4] = _idr.GetInt32(_idr.GetOrdinal("AVG((E.correct_congruent + E.correct_incongruent) * (E.correct_congruent + E.correct_incongruent))"));
        }

        _idr.Close();
        _idr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;

        return avgEriksen;
    }

    public static List<PlayerLogs> getLogsToUpload()
	{
		List<PlayerLogs> uploadList = new List<PlayerLogs>();

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		sql = "SELECT * FROM player_logs WHERE is_uploaded = 0;";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		while(_idr.Read())
		{
			PlayerLogs temp = new PlayerLogs();
			temp.log_id = _idr.GetInt32(_idr.GetOrdinal("log_id"));
            temp.player_id = _idr.GetInt32(_idr.GetOrdinal("player_id"));
            temp.test_id = _idr.GetInt32(_idr.GetOrdinal("test_id"));
            temp.time_start = _idr["time_start"].ToString();
			temp.time_end = _idr["time_end"].ToString();
			temp.prev_status = _idr["prev_status"].ToString();
			temp.new_status = _idr["new_status"].ToString();
			temp.game_progress = _idr["game_progress"].ToString();
			temp.is_uploaded = _idr.GetInt32(_idr.GetOrdinal("is_uploaded"));

			uploadList.Add(temp);
		}

		_idr.Close ();
		_idr = null;
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		return uploadList;
	}

	public static void updateUploadedLog(int log_id)
	{
		_dbc = new SqliteConnection(_constr);
		_dbc.Open();

		sql = "UPDATE player_logs SET is_uploaded = 1 WHERE log_id = " + log_id + ";";
		_dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
	}

	public static GoNoGoData getGoNoGoToUpload(int log_id)
	{
		GoNoGoData temp = null;

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		sql = "SELECT * FROM gonogo_data WHERE log_id = " + log_id + ";";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
            temp = new GoNoGoData();
            temp.log_id = _idr.GetInt32(_idr.GetOrdinal("player_id"));
            temp.correct_go_count = _idr.GetInt32(_idr.GetOrdinal("correct_go_count")); ;
            temp.correct_nogo_count = _idr.GetInt32(_idr.GetOrdinal("correct_nogo_count"));
            temp.mean_time = _idr.GetDouble(_idr.GetOrdinal("mean_time"));
            temp.go_count = _idr.GetInt32(_idr.GetOrdinal("go_count   ")); ;
            temp.trial_count = _idr.GetInt32(_idr.GetOrdinal("trial_count")); ;
        }

		_idr.Close ();
		_idr = null;
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		return temp;
	}

	public static NBackData getNBackToUpload(int log_id)
	{
		NBackData temp = null;

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		sql = "SELECT * FROM nback_data WHERE log_id = " + log_id + ";";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
            temp = new NBackData();
            temp.log_id = _idr.GetInt32(_idr.GetOrdinal("log_id"));
            temp.mean_time = _idr.GetDouble(_idr.GetOrdinal("mean_time"));
            temp.correct_count = _idr.GetInt32(_idr.GetOrdinal("correct_count"));
            temp.n_count = _idr.GetInt32(_idr.GetOrdinal("n_count"));
            temp.element_count = _idr.GetInt32(_idr.GetOrdinal("element_count"));
            temp.trial_count = _idr.GetInt32(_idr.GetOrdinal("trial_count"));
        }

		_idr.Close ();
		_idr = null;
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		return temp;
	}

	public static CorsiData getCorsiToUpload(int log_id)
	{
		CorsiData temp = null;

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		sql = "SELECT * FROM corsi_data WHERE log_id = " + log_id + ";";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
			temp = new CorsiData();
            temp.log_id = _idr.GetInt32(_idr.GetOrdinal("log_id"));
            temp.correct_length = _idr.GetInt32(_idr.GetOrdinal("correct_length"));
            temp.correct_trials = _idr.GetInt32(_idr.GetOrdinal("correct_trials"));
            temp.seq_length = _idr.GetInt32(_idr.GetOrdinal("seq_length"));
            temp.trial_count = _idr.GetInt32(_idr.GetOrdinal("trial_count"));
        }

		_idr.Close ();
		_idr = null;
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		return temp;
	}

	public static EriksenData getEriksenToUpload(int log_id)
	{
		EriksenData temp = null;

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		sql = "SELECT * FROM eriksen_data WHERE log_id = " + log_id + ";";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
			temp = new EriksenData();
            temp.log_id = _idr.GetInt32(_idr.GetOrdinal("log_id"));
            temp.correct_congruent = _idr.GetInt32(_idr.GetOrdinal("correct_congruent"));
            temp.time_congruent = _idr.GetDouble(_idr.GetOrdinal("time_congruent"));
            temp.correct_incongruent = _idr.GetInt32(_idr.GetOrdinal("correct_incongruent"));
            temp.time_incongruent = _idr.GetDouble(_idr.GetOrdinal("time_incongruent"));
            temp.congruent_count = _idr.GetInt32(_idr.GetOrdinal("congruent_count"));
            temp.trial_count = _idr.GetInt32(_idr.GetOrdinal("trial_count"));
        }

		_idr.Close ();
		_idr = null;
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

		return temp;
	}
}
