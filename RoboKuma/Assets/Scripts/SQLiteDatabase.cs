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
		_dbc=new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm=_dbc.CreateCommand();
		sql = "DROP TABLE IF EXISTS nback; CREATE TABLE nback (id INTEGER PRIMARY KEY, player_id INT NOT NULL, correct INT NOT NULL, total INT NOT NULL, time FLOAT NOT NULL)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		sql = "DROP TABLE IF EXISTS eriksenflanker; CREATE TABLE eriksenflanker (id INTEGER PRIMARY KEY, player_id INT NOT NULL, correct INT NOT NULL, total INT NOT NULL, time FLOAT NOT NULL)";
		_dbcm.CommandText = sql;
        _dbcm.ExecuteNonQuery();
		sql = "DROP TABLE IF EXISTS corsiblocktapping; CREATE TABLE corsiblocktapping (id INTEGER PRIMARY KEY, player_id INT NOT NULL, correct INT NOT NULL, total INT NOT NULL, time FLOAT NOT NULL)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		sql = "DROP TABLE IF EXISTS gonogo; CREATE TABLE gonogo (id INTEGER PRIMARY KEY, player_id INT NOT NULL, correct INT NOT NULL, total INT NOT NULL, time FLOAT NOT NULL)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		sql = "DROP TABLE IF EXISTS player; CREATE TABLE player (id INTEGER PRIMARY KEY, name VARCHAR(20) NOT NULL, memory INT NOT NULL, accuracy INT NOT NULL, speed INT NOT NULL, response INT NOT NULL)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();

		sql = "INSERT INTO player (name, memory, accuracy, speed, response) VALUES ('testName', 0 ,0 ,0, 0);";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

        Debug.Log("Started");

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void insertinto(string table, int player_id, int correct, int total, double time)
	{
		_dbc=new SqliteConnection(_constr);
		_dbc.Open();
		sql = "INSERT INTO " + table + " (player_id, correct, total, time) VALUES (" + player_id + ", " + correct + ", " + total + ", " + time + ");";
		_dbcm=_dbc.CreateCommand();
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		Debug.Log (sql);

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;

        updatePlayerStatistics(correct, total, time, 1, table, player_id);
        //Debug.Log("Updated");
	}

	public void select (string table)
	{
		sql = "SELECT * FROM " + table;
		_dbc=new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm=_dbc.CreateCommand();
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
		sql = "SELECT * FROM player WHERE id = " + player_id;
		_dbc=new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm=_dbc.CreateCommand();
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader ();

		int oResponse = 0, oSpeed = 0, oAccuracy = 0, oMemory = 0;
		while (_idr.Read ()) {
            Debug.Log("READ SOME SHIT");
			oMemory = _idr.GetInt32 (2);
			oAccuracy = _idr.GetInt32 (3);
			oSpeed = _idr.GetInt32 (4);
			oResponse = _idr.GetInt32 (5);
            //Debug.Log ("Memory: " + oMemory);
            //Debug.Log ("Accuracy: " + oAccuracy);
            //Debug.Log ("Speed: " + oSpeed);
            //Debug.Log ("Response: " + oResponse);
        }
        Debug.Log(sql);
        Debug.Log(_idr);
        int[] result = new int[4] { oMemory, oAccuracy, oSpeed, oResponse };

        return result;
	}


	public void updatePlayerStatistics(int correct, int total, double time, int difficulty, string table, int player_id)
	{
		_dbc=new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm=_dbc.CreateCommand();
		sql = "SELECT * FROM player WHERE id = " + player_id + ";";
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
			sql = "UPDATE player SET response = " + newResponse + " , speed = " + newSpeed + " , accuracy = " + newAccuracy + " WHERE id = " + player_id + ";";
						break;
		case "eriksenflanker":
			pCorrect = correct * 1.0 / total;
			pWrong = (total - correct) * 1.0 / total;
			response = (pCorrect - pWrong) * difficulty;
			speed = (difficulty / time);
			accuracy = (pCorrect - pWrong) * difficulty;
			newResponse = oResponse + Convert.ToInt32 (response);
			newSpeed = oSpeed + Convert.ToInt32 (speed);
			newAccuracy = oAccuracy + Convert.ToInt32 (accuracy);
			sql = "UPDATE player SET response = " + newResponse + " , speed = " + newSpeed + " , accuracy = " + newAccuracy + " WHERE id = " + player_id + ";";
						break;
		case "corsiblocktapping":
			pCorrect = correct * 1.0 / total;
            pWrong = (total - correct) * 1.0 / total;
            memory = (pCorrect - pWrong) * difficulty;
                //if (pCorrect == 1) {
                newMemory = oMemory + Convert.ToInt32 (memory);
			/*} else {
				newMemory = oMemory - Convert.ToInt32 (memory);
			}*/
			sql = "UPDATE player SET memory = " + newMemory + " WHERE id = " + player_id + ";";
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
			sql = "UPDATE player SET memory = " + newMemory + " , speed = " + newSpeed + " , accuracy = " + newAccuracy + " WHERE id = " + player_id + ";";
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
}
