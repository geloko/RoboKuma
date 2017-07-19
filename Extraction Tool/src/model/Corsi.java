package model;

public class Corsi extends Test{
    private int correct_trials;
    private int correct_length;
    private int seq_length;
    private int trial_count;

    public Corsi(int correct_trials, int correct_length, int seq_length, int trial_count, int log_id, int player_id, String date_start, String date_end) {
        super(log_id, player_id, date_start, date_end);
        this.correct_trials = correct_trials;
        this.correct_length = correct_length;
        this.seq_length = seq_length;
        this.trial_count = trial_count;
    }

    public int getCorrect_trials() {
        return correct_trials;
    }

    public void setCorrect_trials(int correct_trials) {
        this.correct_trials = correct_trials;
    }

    public int getCorrect_length() {
        return correct_length;
    }

    public void setCorrect_length(int correct_length) {
        this.correct_length = correct_length;
    }

    public int getSeq_length() {
        return seq_length;
    }

    public void setSeq_length(int seq_length) {
        this.seq_length = seq_length;
    }

    public int getTrial_count() {
        return trial_count;
    }

    public void setTrial_count(int trial_count) {
        this.trial_count = trial_count;
    }
}
