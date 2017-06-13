using System;

public class PlayerLogs
{
	public int log_id { get; set; }
    public int player_id { get; set; }
    public int test_id { get; set; }
    public string time_start { get; set; }
    public string time_end { get; set; }
    public string prev_status { get; set; }
    public string new_status { get; set; }
    public string game_progress { get; set; }
    public int is_uploaded { get; set; }
}
