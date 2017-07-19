package Export;

import java.io.FileWriter;
import java.io.IOException;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import javax.swing.JOptionPane;
import model.Corsi;
import model.Eriksen;
import model.GoNoGo;
import model.Log;
import model.NBack;

public class CSV_Writer {
    private static final String COMMA_DELIMITER = ",";
    private static final String NEW_LINE_SEPARATOR = "\n";
    private static final String file_path = "src/logs/";
    
    public static void exportAll(ArrayList<Log> list) {
        DateTimeFormatter dtf = DateTimeFormatter.ofPattern("yyyy.MM.dd-HH.mm.ss");
        LocalDateTime now = LocalDateTime.now();
        
        String file_name = "PlayerLogs-" + dtf.format(now) + ".csv";
        String FILE_HEADER = "Log ID,Player ID,Test Name,Time Started,Time Ended";
        
        FileWriter fileWriter = null;

        try {
            fileWriter = new FileWriter(file_path + file_name);
            fileWriter.append(FILE_HEADER.toString());
            fileWriter.append(NEW_LINE_SEPARATOR);

            for (Log log : list) {
                fileWriter.append(String.valueOf(log.getLog_id()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getPlayer_id()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(log.getTest_name());
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(log.getTime_start());
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(log.getTime_end());
                fileWriter.append(NEW_LINE_SEPARATOR);
            }
            JOptionPane.showMessageDialog(null, "Successfully exported " + file_name + "!", "Export Successful", JOptionPane.INFORMATION_MESSAGE);
        } catch (Exception e) {
            System.out.println("Error in CsvFileWriter!");
            e.printStackTrace();
        } finally {      
            try {
                fileWriter.flush();
                fileWriter.close();
            } catch (IOException e) {
                System.out.println("Error while flushing/closing fileWriter!");
                e.printStackTrace();
            }
        }
    }
    
    public static void exportGoNoGo(ArrayList<GoNoGo> list) {
        DateTimeFormatter dtf = DateTimeFormatter.ofPattern("yyyy.MM.dd-HH.mm.ss");
        LocalDateTime now = LocalDateTime.now();
        
        String file_name = "GoNoGo-" + dtf.format(now) + ".csv";
        String FILE_HEADER = "Log ID,Player ID,"
                            + "Correct Go Count,Correct NoGo Count,Mean Reaction Time,Go Count,Trial Count,"
                            + "Time Started,Time Ended";
        
        FileWriter fileWriter = null;

        try {
            fileWriter = new FileWriter(file_path + file_name);
            fileWriter.append(FILE_HEADER.toString());
            fileWriter.append(NEW_LINE_SEPARATOR);

            for (GoNoGo log : list) {
                fileWriter.append(String.valueOf(log.getLog_id()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getPlayer_id()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getCorrect_go_count()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getCorrect_nogo_count()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getMean_time()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getGo_count()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getTrial_count()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(log.getDate_start());
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(log.getDate_end());
                fileWriter.append(NEW_LINE_SEPARATOR);
            }
            JOptionPane.showMessageDialog(null, "Successfully exported " + file_name + "!", "Export Successful", JOptionPane.INFORMATION_MESSAGE);
        } catch (Exception e) {
            System.out.println("Error in CsvFileWriter!");
            e.printStackTrace();
        } finally {      
            try {
                fileWriter.flush();
                fileWriter.close();
            } catch (IOException e) {
                System.out.println("Error while flushing/closing fileWriter!");
                e.printStackTrace();
            }
        }
    }
    
    public static void exportNBack(ArrayList<NBack> list) {
        DateTimeFormatter dtf = DateTimeFormatter.ofPattern("yyyy.MM.dd-HH.mm.ss");
        LocalDateTime now = LocalDateTime.now();
        
        String file_name = "NBack-" + dtf.format(now) + ".csv";
        String FILE_HEADER = "Log ID,Player ID,"
                            + "Correct Count,Mean Reaction Time,"
                            + "n Size,Unique Elements,Trial Count,"
                            + "Time Started,Time Ended";
        
        FileWriter fileWriter = null;

        try {
            fileWriter = new FileWriter(file_path + file_name);
            fileWriter.append(FILE_HEADER.toString());
            fileWriter.append(NEW_LINE_SEPARATOR);

            for (NBack log : list) {
                fileWriter.append(String.valueOf(log.getLog_id()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getPlayer_id()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getCorrect_count()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getMean_time()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getN_count()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getElement_count()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getTrial_count()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(log.getDate_start());
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(log.getDate_end());
                fileWriter.append(NEW_LINE_SEPARATOR);
            }
            JOptionPane.showMessageDialog(null, "Successfully exported " + file_name + "!", "Export Successful", JOptionPane.INFORMATION_MESSAGE);
        } catch (Exception e) {
            System.out.println("Error in CsvFileWriter!");
            e.printStackTrace();
        } finally {      
            try {
                fileWriter.flush();
                fileWriter.close();
            } catch (IOException e) {
                System.out.println("Error while flushing/closing fileWriter!");
                e.printStackTrace();
            }
        }
    }
    
    public static void exportEriksen(ArrayList<Eriksen> list) {
        DateTimeFormatter dtf = DateTimeFormatter.ofPattern("yyyy.MM.dd-HH.mm.ss");
        LocalDateTime now = LocalDateTime.now();
        
        String file_name = "Eriksen-" + dtf.format(now) + ".csv";
        String FILE_HEADER = "Log ID,Player ID,"+
                             "Correct Congruent, Avg. Congruent Time," +
                             "Correct Incongruent,Avg. Incongruent Time," +
                             "Congruent Count,Trial Count," +
                             "Time Started,Time Ended";
        
        FileWriter fileWriter = null;

        try {
            fileWriter = new FileWriter(file_path + file_name);
            fileWriter.append(FILE_HEADER.toString());
            fileWriter.append(NEW_LINE_SEPARATOR);

            for (Eriksen log : list) {
                fileWriter.append(String.valueOf(log.getLog_id()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getPlayer_id()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getCorrect_congruent()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getTime_congruent()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getCorrect_incongruent()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getTime_incongruent()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getCongruent_count()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getTrial_count()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(log.getDate_start());
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(log.getDate_end());
                fileWriter.append(NEW_LINE_SEPARATOR);
            }
            JOptionPane.showMessageDialog(null, "Successfully exported " + file_name + "!", "Export Successful", JOptionPane.INFORMATION_MESSAGE);
        } catch (Exception e) {
            System.out.println("Error in CsvFileWriter!");
            e.printStackTrace();
        } finally {      
            try {
                fileWriter.flush();
                fileWriter.close();
            } catch (IOException e) {
                System.out.println("Error while flushing/closing fileWriter!");
                e.printStackTrace();
            }
        }
    }
    
    public static void exportCorsi(ArrayList<Corsi> list) {
        DateTimeFormatter dtf = DateTimeFormatter.ofPattern("yyyy.MM.dd-HH.mm.ss");
        LocalDateTime now = LocalDateTime.now();
        
        String file_name = "Corsi-" + dtf.format(now) + ".csv";
        String FILE_HEADER = "Log ID,Player ID,"
                            + "Correct Trials,Avg. Correct Length,"
                            + "Sequence Length,Trial Count,"
                            + "Time Started,Time Ended";
        
        FileWriter fileWriter = null;

        try {
            fileWriter = new FileWriter(file_path + file_name);
            fileWriter.append(FILE_HEADER.toString());
            fileWriter.append(NEW_LINE_SEPARATOR);

            for (Corsi log : list) {
                fileWriter.append(String.valueOf(log.getLog_id()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getPlayer_id()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getCorrect_trials()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getCorrect_length()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getSeq_length()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(String.valueOf(log.getTrial_count()));
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(log.getDate_start());
                fileWriter.append(COMMA_DELIMITER);
                fileWriter.append(log.getDate_end());
                fileWriter.append(NEW_LINE_SEPARATOR);
            }
             JOptionPane.showMessageDialog(null, "Successfully exported " + file_name + "!", "Export Successful", JOptionPane.INFORMATION_MESSAGE);
        } catch (Exception e) {
            System.out.println("Error in CsvFileWriter!");
            e.printStackTrace();
        } finally {      
            try {
                fileWriter.flush();
                fileWriter.close();
            } catch (IOException e) {
                System.out.println("Error while flushing/closing fileWriter!");
                e.printStackTrace();
            }
        }
    }
}