using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;
using System;
using System.IO;

public class SQLiteDatabase : MonoBehaviour {

	private string _constr="URI=file://" + Application.persistentDataPath + "/SQLite.db";
	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _idr;
	private string sql;
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
		"local_id INTEGER, " +
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
			player_id + ", " + local_id + ", " + date_start + ");";
		_dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();

		Debug.Log ("INSERT INTO player SQL Command: " + sql);

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
	}

	public void updatePlayer(int temp_id, int player_id, int local_id, string date_start)
	{
		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		sql = "UPDATE player SET player_id = " + player_id + ", local_id = " + local_id + " WHERE local_id = " + temp_id + ";";
		_dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();

		Debug.Log ("UPDATE player SQL Command: " + sql);

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
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
			
        updatePlayerStatistics(correct_length, seq_length, 0.001, trial_count, "corsi", player_id);
        //Debug.Log("Updated");
	}

	public void insertintoEriksen(int player_id, int log_id, int correct_congruent, double time_congruent, int correct_incongruent, double time_incongruent, int congruent_count, int trial_count)
	{
		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		Debug.Log ("INSERT INTO ERIKSEN VALUES: " + player_id + ", " + log_id + ", " + correct_congruent + ", " + time_congruent + ", " + correct_incongruent + ", " + time_incongruent + ", " + congruent_count + ", " + trial_count);
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

		updatePlayerStatistics(correct_congruent + correct_incongruent, trial_count, (time_congruent + time_incongruent)/2, 1, "eriksen", player_id);
	}

	public void insertintoGoNoGo(int player_id, int log_id, int correct_go_count, int correct_nogo_count, double mean_time, int go_count, int trial_count)
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

		updatePlayerStatistics(correct_go_count + correct_nogo_count, trial_count, mean_time, 1, "gonogo", player_id);
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

		updatePlayerStatistics(correct_count, trial_count, mean_time, n_count, "eriksen", player_id);
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

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
		_idr.Close ();
		_idr = null;
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


	public void updatePlayerStatistics(int correct, int total, double time, int difficulty, string table, int player_id)
	{
		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		sql = "SELECT * FROM player_data_table WHERE player_id = " + player_id + ";";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader ();
		int oResponse = 0, oSpeed = 0, oAccuracy = 0, oMemory = 0;

        time += 0.001;

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
			pCorrect = correct * 1.0 / total;
			pWrong = (total - correct) * 1.0 / total;
			response = (pCorrect - pWrong) * difficulty;
			speed = (difficulty / time);
			accuracy = (pCorrect - pWrong) * difficulty;
			newResponse = oResponse + Convert.ToInt32 (response);
			newSpeed = oSpeed + Convert.ToInt32 (speed);
			newAccuracy = oAccuracy + Convert.ToInt32 (accuracy);
			sql = "UPDATE player_data_table SET response = " + newResponse + " , speed = " + newSpeed + " , accuracy = " + newAccuracy + " WHERE player_id = " + player_id + ";";
						break;
		case "eriksen":
			pCorrect = correct * 1.0 / total;
			pWrong = (total - correct) * 1.0 / total;
			response = (pCorrect - pWrong) * difficulty;
			speed = (difficulty / time);
			accuracy = (pCorrect - pWrong) * difficulty;
			newResponse = oResponse + Convert.ToInt32 (response);
			newSpeed = oSpeed + Convert.ToInt32 (speed);
			newAccuracy = oAccuracy + Convert.ToInt32 (accuracy);
			sql = "UPDATE player_data_table SET response = " + newResponse + " , speed = " + newSpeed + " , accuracy = " + newAccuracy + " WHERE player_id = " + player_id + ";";
						break;
		case "corsi":
			pCorrect = correct * 1.0 / total;
            pWrong = (total - correct) * 1.0 / total;
            memory = (pCorrect - pWrong) * difficulty;
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
			memory = (pCorrect - pWrong) * difficulty;
			speed = (difficulty / time);
			accuracy = (pCorrect - pWrong) * difficulty;
			newMemory = oMemory + Convert.ToInt32 (memory);
			newSpeed = oSpeed + Convert.ToInt32 (speed);
			newAccuracy = oAccuracy + Convert.ToInt32 (accuracy);
			sql = "UPDATE player_data_table SET memory = " + newMemory + " , speed = " + newSpeed + " , accuracy = " + newAccuracy + " WHERE player_id = " + player_id + ";";
						break;
		default:
			break;
		}
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
		_idr.Close ();
		_idr = null;
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

	public string getMostPlayed()
	{
		string mostPlayed = "N/A";

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		sql = "SELECT T.test_name FROM tests T, " +
			  "(SELECT test_id, COUNT(*) FROM player_logs " + 
			  "GROUP BY test_id ORDER BY COUNT(*) DESC LIMIT 1) as L " +
			  "WHERE T.test_id = L.test_id;";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
			mostPlayed = _idr["test_name"];
		}

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
		_idr.Close ();
		_idr = null;

		return mostPlayed;
	}

	public string getLeastPlayed()
	{
		string mostPlayed = "N/A";

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		sql = "SELECT T.test_name FROM tests T, " +
			  "(SELECT test_id, COUNT(*) FROM player_logs " + 
			  "GROUP BY test_id ORDER BY COUNT(*) ASC LIMIT 1) as L " +
			  "WHERE T.test_id = L.test_id;";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
			leastPlayed = _idr["test_name"];
		}

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
		_idr.Close ();
		_idr = null;

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
			averageStatus = _idr["new_status"];
		}

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
		_idr.Close ();
		_idr = null;

		return averageStatus;
	}

	public GoNoGoData getBestGoNoGo()
	{
		GoNoGoData bestGoNoGo = null;

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		/*
			sql = "SELECT G.log_id, G.correct_go_count, G.correct_nogo_count, G.mean_time, G.go_count, G.trial_count "  +
				  "FROM gonogo_data G;";
		*/
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
			bestGoNoGo = new GoNoGoData();
			bestGoNoGo.log_id = _idr["log_id"];
			bestGoNoGo.correct_go_count = _idr["correct_go_count"];
			bestGoNoGo.correct_nogo_count = _idr["correct_nogo_count"];
			bestGoNoGo.mean_time = _idr["mean_time"];
			bestGoNoGo.go_count = _idr["go_count"];
			bestGoNoGo.trial_count = _idr["trial_count"];
		}

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
		_idr.Close ();
		_idr = null;

		return bestGoNoGo;
	}

	public NBackData getBestNBack()
	{
		NBackData bestNBack = null;

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		/*
			sql = "SELECT N.log_id, N.mean_time, N.correct_count, N.n_count, N.element_count, N.trial_count "  +
				  "FROM nback_data N;";
		*/
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
			bestNBack = new NBackData();
			bestNBack.log_id = _idr["log_id"];
			bestNBack.mean_time = _idr["mean_time"];
			bestNBack.correct_count = _idr["correct_count"];
			bestNBack.n_count = _idr["n_count"];
			bestNBack.element_count = _idr["element_count"];
			bestNBack.trial_count = _idr["trial_count"];
		}

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
		_idr.Close ();
		_idr = null;

		return bestGoNoGo;
	}

	public CorsiData getBestCorsi()
	{
		CorsiData bestCorsi = null;

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		/*
			sql = "SELECT C.log_id, C.correct_length, C.correct_trials, C.seq_length, C.trial_count "  +
				  "FROM corsi_data G;";
		*/
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
			bestCorsi = new CorsiData();
			bestCorsi.log_id = _idr["log_id"];
			bestCorsi.correct_length = _idr["correct_length"];
			bestCorsi.correct_trials = _idr["correct_trials"];
			bestCorsi.seq_length = _idr["seq_length"];
			bestCorsi.trial_count = _idr["trial_count"];
		}

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
		_idr.Close ();
		_idr = null;

		return bestCorsi;
	}

	public EriksenData getBestEriksen()
	{
		CorsiData bestEriksen = null;

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		/*
			sql = "SELECT E.log_id, E.correct_congruent, E.time_congruent, E.correct_incongruent, E.time_incongruent, E.congruent_count, E.trial_count "  +
				  "FROM eriksen_data EXISTS;";
		*/
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		if(_idr.Read())
		{
			bestEriksen = new EriksenData();
			bestEriksen.log_id = _idr["log_id"];
			bestEriksen.correct_congruent = _idr["correct_congruent"];
			bestEriksen.time_congruent = _idr["time_congruent"];
			bestEriksen.correct_incongruent = _idr["correct_incongruent"];
			bestEriksen.time_incongruent = _idr["time_incongruent"];
			bestEriksen.congruent_count = _idr["congruent_count"];
			bestEriksen.trial_count = _idr["trial_count"];
		}

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
		_idr.Close ();
		_idr = null;

		return bestEriksen;
	}

	public List<PlayerLogs> getLogsToUpload()
	{
		List<PlayerLogs> uploadList = List<PlayerLogs>();

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		sql = "SELECT * FROM player_logs WHERE is_uploaded = 0;";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader();

		while(_idr.Read())
		{
			PlayerLog temp = new PlayerLog();
			temp.log_id = _idr["log_id"];
			temp.player_id = _idr["player_id"];
			temp.test_id = _idr["test_id"];
			temp.time_start = _idr["time_start"];
			temp.time_end = _idr["time_end"];
			temp.prev_status = _idr["prev_status"];
			temp.new_status = _idr["new_status"];
			temp.game_progress = _idr["game_progress"];
			temp.is_uploaded = _idr["is_uploaded"];

			uploadList.add(temp);
		}

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
		_idr.Close ();
		_idr = null;

		return uploadList;
	}

	public void updateUploadedLogs(List<PlayerLogs> uploadList)
	{
		_dbc = new SqliteConnection(_constr);
		_dbc.Open();

		foreach (PlayerLog log in uploadList)
		{
			sql = "UPDATE player_logs SET is_uploaded = 1 WHERE log_id = " + log.log_id + ";";
			_dbcm = _dbc.CreateCommand();
			_dbcm.CommandText = sql;
			_dbcm.ExecuteNonQuery();
		}

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
	}

	public GoNoGoData getGoNoGoToUpload(int log_id)
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
			temp.log_id = _idr["log_id"];
			temp.correct_go_count = _idr["correct_go_count"];
			temp.correct_nogo_count = _idr["correct_nogo_count"];
			temp.mean_time = _idr["mean_time"];
			temp.go_count = _idr["go_count"];
			temp.trial_count = _idr["trial_count"];
		}

		return temp;
	}

	public NBackData getNBackToUpload(int log_id)
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
			temp.log_id = _idr["log_id"];
			temp.mean_time = _idr["mean_time"];
			temp.correct_count = _idr["correct_count"];
			temp.n_count = _idr["n_count"];
			temp.element_count = _idr["element_count"];
			temp.trial_count = _idr["trial_count"];
		}

		return temp;
	}

	public CorsiData getCorsiToUpload(int log_id)
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
			temp.log_id = _idr["log_id"];
			temp.correct_length = _idr["correct_length"];
			temp.correct_trials = _idr["correct_trials"];
			temp.seq_length = _idr["seq_length"];
			temp.trial_count = _idr["trial_count"];
		}

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
		_idr.Close ();
		_idr = null;

		return temp;
	}

	public EriksenData getEriksenToUpload(int log_id)
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
			temp.log_id = _idr["log_id"];
			temp.correct_congruent = _idr["correct_congruent"];
			temp.time_congruent = _idr["time_congruent"];
			temp.correct_incongruent = _idr["correct_incongruent"];
			temp.time_incongruent = _idr["time_incongruent"];
			temp.congruent_count = _idr["congruent_count"];
			temp.trial_count = _idr["trial_count"];
		}

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
		_idr.Close ();
		_idr = null;

		return temp;
	}
}
