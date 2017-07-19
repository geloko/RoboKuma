package model;

public class Eriksen extends Test{
    private int correct_congruent;
    private double time_congruent;
    private int correct_incongruent;
    private double time_incongruent;
    private int congruent_count;
    private int trial_count;

    public Eriksen(int correct_congruent, double time_congruent, int correct_incongruent, double time_incongruent, int congruent_count, int trial_count, int log_id, int player_id, String date_start, String date_end) {
        super(log_id, player_id, date_start, date_end);
        this.correct_congruent = correct_congruent;
        this.time_congruent = time_congruent;
        this.correct_incongruent = correct_incongruent;
        this.time_incongruent = time_incongruent;
        this.congruent_count = congruent_count;
        this.trial_count = trial_count;
    }

    public int getCorrect_congruent() {
        return correct_congruent;
    }

    public void setCorrect_congruent(int correct_congruent) {
        this.correct_congruent = correct_congruent;
    }

    public double getTime_congruent() {
        return time_congruent;
    }

    public void setTime_congruent(double time_congruent) {
        this.time_congruent = time_congruent;
    }

    public int getCorrect_incongruent() {
        return correct_incongruent;
    }

    public void setCorrect_incongruent(int correct_incongruent) {
        this.correct_incongruent = correct_incongruent;
    }

    public double getTime_incongruent() {
        return time_incongruent;
    }

    public void setTime_incongruent(double time_incongruent) {
        this.time_incongruent = time_incongruent;
    }

    public int getCongruent_count() {
        return congruent_count;
    }

    public void setCongruent_count(int congruent_count) {
        this.congruent_count = congruent_count;
    }

    public int getTrial_count() {
        return trial_count;
    }

    public void setTrial_count(int trial_count) {
        this.trial_count = trial_count;
    }
}
