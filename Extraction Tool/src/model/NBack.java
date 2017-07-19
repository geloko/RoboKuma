package model;

public class NBack extends Test{
    private int correct_count;
    private double mean_time;
    private int n_count;
    private int element_count;
    private int trial_count;

    public NBack(int correct_count, double mean_time, int n_count, int element_count, int trial_count, int log_id, int player_id, String date_start, String date_end) {
        super(log_id, player_id, date_start, date_end);
        this.correct_count = correct_count;
        this.mean_time = mean_time;
        this.n_count = n_count;
        this.element_count = element_count;
        this.trial_count = trial_count;
    }

    public int getCorrect_count() {
        return correct_count;
    }

    public void setCorrect_count(int correct_count) {
        this.correct_count = correct_count;
    }
    
    public double getMean_time() {
        return mean_time;
    }

    public void setMean_time(double mean_time) {
        this.mean_time = mean_time;
    }

    public int getN_count() {
        return n_count;
    }

    public void setN_count(int n_count) {
        this.n_count = n_count;
    }

    public int getElement_count() {
        return element_count;
    }

    public void setElement_count(int element_count) {
        this.element_count = element_count;
    }

    public int getTrial_count() {
        return trial_count;
    }

    public void setTrial_count(int trial_count) {
        this.trial_count = trial_count;
    }
}
