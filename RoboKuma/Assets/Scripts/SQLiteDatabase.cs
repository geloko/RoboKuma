using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;

public class SQLiteDatabase : MonoBehaviour {

	private string _constr="URI=file:SQLite.db";
	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _idr;
	private string sql;
	// Use this for initialization
	void Start () {
		_dbc=new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm=_dbc.CreateCommand();
		sql = "CREATE TABLE nback (id INT NOT NULL AUTO_INCREMENT, player_id INT NOT NULL, correct INT NOT NULL, total INT NOT NULL, time FLOAT NOT NULL)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		sql = "CREATE TABLE eriksenflanker (id INT NOT NULL AUTO_INCREMENT, player_id INT NOT NULL, correct INT NOT NULL, total INT NOT NULL, time FLOAT NOT NULL)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		sql = "CREATE TABLE corsiblocktapping (id INT NOT NULL AUTO_INCREMENT, player_id INT NOT NULL, correct INT NOT NULL, total INT NOT NULL, time FLOAT NOT NULL)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		sql = "CREATE TABLE gonogo (id INT NOT NULL AUTO_INCREMENT, player_id INT NOT NULL, correct INT NOT NULL, total INT NOT NULL, time FLOAT NOT NULL)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		sql = "CREATE TABLE player (id INT NOT NULL AUTO_INCREMENT, name VARCHAR(20) NOT NULL, memory INT NOT NULL, accuracy INT NOT NULL, speed INT NOT NULL, response INT NOT NULL)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();

		sql = "INSERT INTO " + table + " (name, memory, accuracy, speed, response) VALUES (testName, 0 ,0 ,0, 0);";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();

		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
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
		sql = "SELECT * FROM player WHERE player_id = " + player_id;
		_dbc=new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm=_dbc.CreateCommand();
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader ();
		int oResponse, oSpeed, oAccuracy, oMemory;
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

		return new int[oMemory, oAccuracy, oSpeed, oResponse];
	}


	public void updatePlayerStatistics(int correct, int total, double time, int difficulty, string table, int player_id)
	{
		_dbc=new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm=_dbc.CreateCommand();
		sql = "SELECT * FROM player WHERE player_id = " + player_id + ";";
		_dbcm.CommandText = sql;
		_idr = _dbcm.ExecuteReader ();
		int oResponse, oSpeed, oAccuracy, oMemory;
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
		double response, speed, accuracy, memory;
		int newResponse, newSpeed, newAccuracy;
		switch (table) 
		{
		case "gonogo":
			pCorrect = correct / total;
			pWrong = (total - correct) / total;
			response = (pCorrect - pWrong) * difficulty;
			speed = (time * difficulty);
			accuracy = (pCorrect - pWrong) * difficulty;
			newResponse = oResponse + Convert.ToInt32 (response);
			newSpeed = oSpeed + Convert.ToInt32 (speed);
			newAccuracy = oAccuracy + Convert.ToInt32 (accuracy);
			sql = "UPDATE player SET response = " + newResponse + " , speed = " + newSpeed + " , accuracy = " + newAccuracy + " WHERE player_id = " + player_id + ";";
						break;
		case "eriksenflanker":
			pCorrect = correct / total;
			pWrong = (total - correct) / total;
			response = (pCorrect - pWrong) * difficulty;
			speed = (time * difficulty);
			accuracy = (pCorrect - pWrong) * difficulty;
			newResponse = oResponse + Convert.ToInt32 (response);
			newSpeed = oSpeed + Convert.ToInt32 (speed);
			newAccuracy = oAccuracy + Convert.ToInt32 (accuracy);
			sql = "UPDATE player SET response = " + newResponse + " , speed = " + newSpeed + " , accuracy = " + newAccuracy + " WHERE player_id = " + player_id + ";";
						break;
		case "corsiblocktapping":
			pCorrect = correct / total;
			memory = (pCorrect * 10) * difficulty;
			if (pCorrect == 1) {
				newMemory = oMemory + Convert.ToInt32 (response);
			} else {
				newMemory = oMemory - Convert.ToInt32 (response);;
			}
			sql = "UPDATE player SET memory = " + newMemory + " WHERE player_id = " + player_id + ";";
						break;
		case "nback":
			pCorrect = correct / total;
			pWrong = (total - correct) / total;
			memory = (pCorrect - pWrong) * difficulty;
			speed = (time * difficulty);
			accuracy = (pCorrect - pWrong) * difficulty;
			newMemory = oResponse + Convert.ToInt32 (response);
			newSpeed = oSpeed + Convert.ToInt32 (speed);
			newAccuracy = oAccuracy + Convert.ToInt32 (accuracy);
			sql = "UPDATE player SET memory = " + newMemory + " , speed = " + newSpeed + " , accuracy = " + newAccuracy + " WHERE player_id = " + player_id + ";";
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
