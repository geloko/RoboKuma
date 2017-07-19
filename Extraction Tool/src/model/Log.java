package model;

public class Log {
    private int log_id;
    private int player_id;
    private String test_name;
    private String time_start;
    private String time_end;

    public Log(int log_id, int player_id, String test_name, String time_start, String time_end) {
        this.log_id = log_id;
        this.player_id = player_id;
        this.test_name = test_name;
        this.time_start = time_start;
        this.time_end = time_end;
    }

    public int getLog_id() {
        return log_id;
    }

    public void setLog_id(int log_id) {
        this.log_id = log_id;
    }

    public int getPlayer_id() {
        return player_id;
    }

    public void setPlayer_id(int player_id) {
        this.player_id = player_id;
    }

    public String getTest_name() {
        return test_name;
    }

    public void setTest_name(String test_name) {
        this.test_name = test_name;
    }

    public String getTime_start() {
        return time_start;
    }

    public void setTime_start(String time_start) {
        this.time_start = time_start;
    }

    public String getTime_end() {
        return time_end;
    }

    public void setTime_end(String time_end) {
        this.time_end = time_end;
    }
}
