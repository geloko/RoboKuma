using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;

public class SQLiteDatabase : MonoBehaviour {

	private string _constr="URI=file:SQLite.db";
	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;
	private string sql;
	// Use this for initialization
	void Start () {
		_dbc=new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm=_dbc.CreateCommand();
		sql = "CREATE TABLE nback (id INT, player_id INT, name VARCHAR(20), score INT)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		sql = "CREATE TABLE eriksenflanker (id INT, player_id INT,name VARCHAR(20), score INT)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		sql = "CREATE TABLE corsiblocking (id INT, player_id INT, name VARCHAR(20), score INT)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		sql = "CREATE TABLE gonogo (id INT, player_id INT,name VARCHAR(20), score INT)";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();
		sql = "CREATE TABLE player (id INT, name VARCHAR(20))";
		_dbcm.CommandText = sql;
		_dbcm.ExecuteNonQuery();

		_dbr.Close ();
		_dbr = null;
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
