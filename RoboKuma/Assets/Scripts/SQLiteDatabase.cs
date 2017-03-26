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
		sql = "CREATE TABLE nback (id INT, player_id INT, correct INT, total INT, time FLOAT)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		sql = "CREATE TABLE eriksenflanker (id INT, player_id INT, correct INT, total INT, time FLOAT)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		sql = "CREATE TABLE corsiblocktapping (id INT, player_id INT, correct INT, total INT, time FLOAT)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		sql = "CREATE TABLE gonogo (id INT, player_id INT, correct INT, total INT, time FLOAT)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		sql = "CREATE TABLE player (id INT, name VARCHAR(20))";
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
	}
}
