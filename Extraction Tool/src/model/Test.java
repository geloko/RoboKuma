package model;

public abstract class Test {
    private int log_id;
    private int player_id;
    private String date_start;
    private String date_end;

    public Test(int log_id, int player_id, String date_start, String date_end) {
        this.log_id = log_id;
        this.player_id = player_id;
        this.date_start = date_start;
        this.date_end = date_end;
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

    public String getDate_start() {
        return date_start;
    }

    public void setDate_start(String date_start) {
        this.date_start = date_start;
    }

    public String getDate_end() {
        return date_end;
    }

    public void setDate_end(String date_end) {
        this.date_end = date_end;
    }
}
