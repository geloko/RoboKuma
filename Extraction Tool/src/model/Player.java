package model;

public class Player {
    private int player_id;
    private String date_start;
    private double avg_playtime;
    private String active_hours;
    private String most_played;
    private String least_played;

    public Player(int player_id, String date_start) {
        this.player_id = player_id;
        this.date_start = date_start;
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

    public double getAvg_playtime() {
        return avg_playtime;
    }

    public void setAvg_playtime(double avg_playtime) {
        this.avg_playtime = avg_playtime;
    }

    public String getActive_hours() {
        return active_hours;
    }

    public void setActive_hours(String active_hours) {
        this.active_hours = active_hours;
    }

    public String getMost_played() {
        return most_played;
    }

    public void setMost_played(String most_played) {
        this.most_played = most_played;
    }

    public String getLeast_played() {
        return least_played;
    }

    public void setLeast_played(String least_played) {
        this.least_played = least_played;
    }
}
