package model;

public class GoNoGo extends Test{
    private int correct_go_count;
    private int correct_nogo_count;
    private double mean_time;
    private int go_count;
    private int trial_count;

    public GoNoGo(int correct_go_count, int correct_nogo_count, double mean_time, int go_count, int trial_count, int log_id, int player_id, String date_start, String date_end) {
        super(log_id, player_id, date_start, date_end);
        this.correct_go_count = correct_go_count;
        this.correct_nogo_count = correct_nogo_count;
        this.mean_time = mean_time;
        this.go_count = go_count;
        this.trial_count = trial_count;
    }

    public int getCorrect_go_count() {
        return correct_go_count;
    }

    public void setCorrect_go_count(int correct_go_count) {
        this.correct_go_count = correct_go_count;
    }

    public int getCorrect_nogo_count() {
        return correct_nogo_count;
    }

    public void setCorrect_nogo_count(int correct_nogo_count) {
        this.correct_nogo_count = correct_nogo_count;
    }

    public double getMean_time() {
        return mean_time;
    }

    public void setMean_time(double mean_time) {
        this.mean_time = mean_time;
    }

    public int getGo_count() {
        return go_count;
    }

    public void setGo_count(int go_count) {
        this.go_count = go_count;
    }

    public int getTrial_count() {
        return trial_count;
    }

    public void setTrial_count(int trial_count) {
        this.trial_count = trial_count;
    }
    
    
}
