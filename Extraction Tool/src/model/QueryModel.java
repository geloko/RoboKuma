package model;

import db_connection.MySQLConnector;
import db_connection.MySQLSSHConnector;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.logging.Level;
import java.util.logging.Logger;

public class QueryModel {
    public static ArrayList<Log> getAllLogs(){
        ArrayList<Log> objectList = new ArrayList<Log>();
        try {
            ResultSet rsList = MySQLSSHConnector.executeQuery("SELECT L.log_id, L.player_id, T.test_name, L.time_start, L.time_end " +
                                                              "FROM player_logs L, tests T " + 
                                                              "WHERE L.test_id = T.test_id " +
                                                              "ORDER BY L.log_id ASC;");
            while(rsList.next()) {
                objectList.add(new Log(rsList.getInt("log_id"),
                                 rsList.getInt("player_id"),
                                 rsList.getString("test_name"),
                                 rsList.getString("time_start"),
                                 rsList.getString("time_end")));
            }
        } catch (SQLException ex) {
            Logger.getLogger(QueryModel.class.getName()).log(Level.SEVERE, null, ex);
        }
        
        System.out.println("getAllLogs Query - " + objectList.size() + " rows");
        return objectList;
    }
    
    public static int getAllLogsCount(){
        int count = 0;
        try {
            ResultSet rsList = MySQLSSHConnector.executeQuery("SELECT L.log_id, L.player_id, T.test_name, L.time_start, L.time_end " +
                                                              "FROM player_logs L, tests T " + 
                                                              "WHERE L.test_id = T.test_id " +
                                                              "ORDER BY L.log_id ASC;");
            while(rsList.next()) {
                count++;
            }
        } catch (SQLException ex) {
            Logger.getLogger(QueryModel.class.getName()).log(Level.SEVERE, null, ex);
        }
        
        System.out.println("getAllLogsCount Query - " + count + " rows");
        return count;
    }
    
    public static Test getLogData(int log_id){
        Test test = null;
        int test_id = getLogTestID(log_id);
        String query = null;
        
        switch(test_id){
            case 1: query = "SELECT L.log_id, L.player_id, G.correct_go_count, G.correct_nogo_count, G.mean_time, G.go_count, G.trial_count, L.time_start, L.time_end " +
                            "FROM player_logs L, gonogo_data G " +
                            "WHERE L.test_id = " + test_id + "AND L.log_id = " + log_id + " AND L.log_id = G.log_id;"; break;
            case 3: query = "SELECT L.log_id, L.player_id, N.mean_time, N.correct_count, N.n_count, N.element_count, N.trial_count, L.time_start, L.time_end " +
                            "FROM player_logs L, nback_data N " +
                            "WHERE L.test_id = " + test_id + "AND L.log_id = " + log_id + " AND L.log_id = N.log_id;"; break;
            case 4: query = "SELECT L.log_id, L.player_id, E.correct_congruent, E.time_congruent, E.correct_incongruent, E.time_incongruent, E.congruent_count, E.trial_count, L.time_start, L.time_end " +
                            "FROM player_logs L, eriksen_data E " +
                            "WHERE L.test_id = " + test_id + "AND L.log_id = " + log_id + " AND L.log_id = E.log_id;"; break;
            case 2: query = "SELECT L.log_id, L.player_id, C.correct_trials, C.correct_length, C.seq_length, C.trial_count, L.time_start, L.time_end " +
                            "FROM player_logs L, corsi_data C " +
                            "WHERE L.test_id = " + test_id + "AND L.log_id = " + log_id + " AND L.log_id = C.log_id;"; break;
        }
        
        Player player = null;
        try {
            ResultSet rsList = MySQLSSHConnector.executeQuery(query);
            if(rsList.next()) {
                switch(test_id){
                    case 1: test = new GoNoGo(rsList.getInt("correct_go_count"),
                                                        rsList.getInt("correct_nogo_count"),
                                                        rsList.getDouble("mean_time"),
                                                        rsList.getInt("go_count"),
                                                        rsList.getInt("trial_count"),
                                                        rsList.getInt("log_id"),
                                                        rsList.getInt("player_id"),
                                                        rsList.getString("time_start"),
                                                        rsList.getString("time_end"));
                    case 3: test= new NBack(rsList.getInt("correct_count"),
                                                        rsList.getDouble("mean_time"),                                                       
                                                        rsList.getInt("n_count"),
                                                        rsList.getInt("element_count"),
                                                        rsList.getInt("trial_count"),
                                                        rsList.getInt("log_id"),
                                                        rsList.getInt("player_id"),
                                                        rsList.getString("time_start"),
                                                        rsList.getString("time_end"));
                    case 4: test = new Eriksen(rsList.getInt("correct_congruent"),
                                                        rsList.getDouble("time_congruent"),
                                                        rsList.getInt("correct_incongruent"),
                                                        rsList.getDouble("time_incongruent"),
                                                        rsList.getInt("congruent_count"),
                                                        rsList.getInt("trial_count"),
                                                        rsList.getInt("log_id"),
                                                        rsList.getInt("player_id"),
                                                        rsList.getString("time_start"),
                                                        rsList.getString("time_end"));
                    case 2: test = new Corsi(rsList.getInt("correct_trials"),
                                                        rsList.getInt("correct_length"),
                                                        rsList.getInt("seq_length"),
                                                        rsList.getInt("trial_count"),
                                                        rsList.getInt("log_id"),
                                                        rsList.getInt("player_id"),
                                                        rsList.getString("time_start"),
                                                        rsList.getString("time_end"));
                }
            }
        } catch (SQLException ex) {
            Logger.getLogger(QueryModel.class.getName()).log(Level.SEVERE, null, ex);
        }

        return test;
    }
    
    public static ArrayList<GoNoGo> getGoNoGoLogs(){
        ArrayList<GoNoGo> objectList = new ArrayList<GoNoGo>();

        try {
            ResultSet rsList = MySQLSSHConnector.executeQuery("SELECT L.log_id, L.player_id, G.correct_go_count, G.correct_nogo_count, G.mean_time, go_count, G.trial_count, L.time_start, L.time_end " +
                                                              "FROM player_logs L, gonogo_data G " +
                                                              "WHERE L.test_id = 1 AND L.log_id = G.log_id " +
                                                              "ORDER BY L.log_id ASC;");
            while(rsList.next()) {
                objectList.add(new GoNoGo(rsList.getInt("correct_go_count"),
                                          rsList.getInt("correct_nogo_count"),
                                          rsList.getDouble("mean_time"),
                                          rsList.getInt("go_count"),
                                          rsList.getInt("trial_count"),
                                          rsList.getInt("log_id"),
                                          rsList.getInt("player_id"),
                                          rsList.getString("time_start"),
                                          rsList.getString("time_end")));
            }
        } catch (SQLException ex) {
            Logger.getLogger(QueryModel.class.getName()).log(Level.SEVERE, null, ex);
        }

        System.out.println("getGoNoGoLogs Query - " + objectList.size() + " rows");
        return objectList;
    }
    
    public static int getGoNoGoLogsCount(){
        int count = 0;

        try {
            ResultSet rsList = MySQLSSHConnector.executeQuery("SELECT L.log_id, L.player_id, G.correct_go_count, G.correct_nogo_count, G.mean_time, go_count, G.trial_count, L.time_start, L.time_end " +
                                                              "FROM player_logs L, gonogo_data G " +
                                                              "WHERE L.test_id = 1 AND L.log_id = G.log_id " +
                                                              "ORDER BY L.log_id ASC;");
            while(rsList.next()) {
                count++;
            }
        } catch (SQLException ex) {
            Logger.getLogger(QueryModel.class.getName()).log(Level.SEVERE, null, ex);
        }

        //System.out.println("getGoNoGoLogsCount Query - " + count + " rows");
        return count;
    }
    
    public static ArrayList<NBack> getNBackLogs(){
        ArrayList<NBack> objectList = new ArrayList<NBack>();
        
        try {
            ResultSet rsList = MySQLSSHConnector.executeQuery("SELECT L.log_id, L.player_id, N.mean_time, N.correct_count, N.n_count, N.element_count, N.trial_count, L.time_start, L.time_end " +
                                                              "FROM player_logs L, nback_data N " +
                                                              "WHERE L.test_id = 3 AND L.log_id = N.log_id " +
                                                              "ORDER BY L.log_id ASC;");
            while(rsList.next()) {
                objectList.add(new NBack(rsList.getInt("correct_count"),
                                         rsList.getDouble("mean_time"),                                                       
                                         rsList.getInt("n_count"),
                                         rsList.getInt("element_count"),
                                         rsList.getInt("trial_count"),
                                         rsList.getInt("log_id"),
                                         rsList.getInt("player_id"),
                                         rsList.getString("time_start"),
                                         rsList.getString("time_end")));
            }
        } catch (SQLException ex) {
            Logger.getLogger(QueryModel.class.getName()).log(Level.SEVERE, null, ex);
        }
            
        System.out.println("getNBackLogs Query - " + objectList.size() + " rows");
        return objectList;
    }
    
    public static int getNBackLogsCount(){
        int count = 0;
        
        try {
            ResultSet rsList = MySQLSSHConnector.executeQuery("SELECT L.log_id, L.player_id, N.mean_time, N.correct_count, N.n_count, N.element_count, N.trial_count, L.time_start, L.time_end " +
                                                              "FROM player_logs L, nback_data N " +
                                                              "WHERE L.test_id = 3 AND L.log_id = N.log_id " +
                                                              "ORDER BY L.log_id ASC;");
            while(rsList.next()) {
                count++;
            }
        } catch (SQLException ex) {
            Logger.getLogger(QueryModel.class.getName()).log(Level.SEVERE, null, ex);
        }
            
        //System.out.println("getNBackLogsCount Query - " + count + " rows");
        return count;
    }
    
    public static ArrayList<Eriksen> getEriksenLogs(){
        ArrayList<Eriksen> objectList = new ArrayList<Eriksen>();
        
        try {
            ResultSet rsList = MySQLSSHConnector.executeQuery("SELECT L.log_id, L.player_id, E.correct_congruent, E.time_congruent, E.correct_incongruent, E.time_incongruent, E.congruent_count, E.trial_count, L.time_start, L.time_end " +
                                                              "FROM player_logs L, eriksen_data E " +
                                                              "WHERE L.test_id = 4 AND L.log_id = E.log_id " +
                                                              "ORDER BY L.log_id ASC;");
            while(rsList.next()) {
                objectList.add(new Eriksen(rsList.getInt("correct_congruent"),
                                           rsList.getDouble("time_congruent"),
                                           rsList.getInt("correct_incongruent"),
                                           rsList.getDouble("time_incongruent"),
                                           rsList.getInt("congruent_count"),
                                           rsList.getInt("trial_count"),
                                           rsList.getInt("log_id"),
                                           rsList.getInt("player_id"),
                                           rsList.getString("time_start"),
                                           rsList.getString("time_end")));
            }
        } catch (SQLException ex) {
            Logger.getLogger(QueryModel.class.getName()).log(Level.SEVERE, null, ex);
        }
            
        System.out.println("getEriksenLogs Query - " + objectList.size() + " rows");
        return objectList;
    }
    
    public static int getEriksenLogsCount(){
        int count = 0;
        
        try {
            ResultSet rsList = MySQLSSHConnector.executeQuery("SELECT L.log_id, L.player_id, E.correct_congruent, E.time_congruent, E.correct_incongruent, E.time_incongruent, E.congruent_count, E.trial_count, L.time_start, L.time_end " +
                                                              "FROM player_logs L, eriksen_data E " +
                                                              "WHERE L.test_id = 4 AND L.log_id = E.log_id " +
                                                              "ORDER BY L.log_id ASC;");
            while(rsList.next()) {
                count++;
            }
        } catch (SQLException ex) {
            Logger.getLogger(QueryModel.class.getName()).log(Level.SEVERE, null, ex);
        }
            
        //System.out.println("getEriksenLogsCount Query - " + count + " rows");
        return count;
    }
    
    public static ArrayList<Corsi> getCorsiLogs(){
        ArrayList<Corsi> objectList = new ArrayList<Corsi>();
        
        try {
            ResultSet rsList = MySQLSSHConnector.executeQuery("SELECT L.log_id, L.player_id, C.correct_trials, C.correct_length, C.seq_length, C.trial_count, L.time_start, L.time_end " +
                                                              "FROM player_logs L, corsi_data C " +
                                                              "WHERE L.test_id = 2 AND L.log_id = C.log_id " +
                                                              "ORDER BY L.log_id ASC;");
            while(rsList.next()) {
                objectList.add(new Corsi(rsList.getInt("correct_trials"),
                                         rsList.getInt("correct_length"),
                                         rsList.getInt("seq_length"),
                                         rsList.getInt("trial_count"),
                                         rsList.getInt("log_id"),
                                         rsList.getInt("player_id"),
                                         rsList.getString("time_start"),
                                         rsList.getString("time_end")));
            }
        } catch (SQLException ex) {
            Logger.getLogger(QueryModel.class.getName()).log(Level.SEVERE, null, ex);
        }
            
        System.out.println("getCorsiLogs Query - " + objectList.size() + " rows");
        return objectList;
    }
    
    public static int getCorsiLogsCount(){
        int count = 0;
        
        try {
            ResultSet rsList = MySQLSSHConnector.executeQuery("SELECT L.log_id, L.player_id, C.correct_trials, C.correct_length, C.seq_length, C.trial_count, L.time_start, L.time_end " +
                                                              "FROM player_logs L, corsi_data C " +
                                                              "WHERE L.test_id = 2 AND L.log_id = C.log_id " +
                                                              "ORDER BY L.log_id ASC;");
            while(rsList.next()) {
                count++;
            }
        } catch (SQLException ex) {
            Logger.getLogger(QueryModel.class.getName()).log(Level.SEVERE, null, ex);
        }
            
        //System.out.println("getCorsiLogsCount Query - " + count + " rows");
        return count;
    }
    
    private static Player getPlayerData(int player_id){
        Player player = null;
        
        try {
            ResultSet rsList = MySQLSSHConnector.executeQuery("SELECT * " +
                                                              "FROM player " + 
                                                              "WHERE player_id = " + player_id + ";");
            if(rsList.next()) {
                player = new Player(rsList.getInt("player_id"),
                                 rsList.getString("date_start"));
            }
        } catch (SQLException ex) {
            Logger.getLogger(QueryModel.class.getName()).log(Level.SEVERE, null, ex);
        }
                
        return player;
    }
    
    public static ArrayList<Log> getPlayerLogs(int player_id){
        ArrayList<Log> objectList = new ArrayList<Log>();
        try {
            ResultSet rsList = MySQLSSHConnector.executeQuery("SELECT L.log_id, L.player_id, T.test_name, L.time_start, L.time_end " +
                                                              "FROM logs L, tests T " + 
                                                              "WHERE L.player_id = " + player_id + 
                                                              " AND L.test_id = T.test_id " +
                                                              "ORDER BY L.log_id ASC;");
            while(rsList.next()) {
                objectList.add(new Log(rsList.getInt("log_id"),
                                 rsList.getInt("player_id"),
                                 rsList.getString("test_name"),
                                 rsList.getString("time_start"),
                                 rsList.getString("time_end")));
            }
        } catch (SQLException ex) {
            Logger.getLogger(QueryModel.class.getName()).log(Level.SEVERE, null, ex);
        }
        
        System.out.println("getPlayerLogs Query - " + objectList.size() + " rows");
        return objectList;
    }
    
    private static String getLogTestName(int log_id){
        String test_name = null;
        try {
            ResultSet rsList = MySQLSSHConnector.executeQuery("SELECT T.name" +
                                                              "FROM player_logs L, tests T " + 
                                                              "WHERE L.log_id = " + log_id + 
                                                              " AND L.test_id = T.test_id;");
            if(rsList.next()) {
                test_name =  rsList.getString("T.test_name");
            }
        } catch (SQLException ex) {
            Logger.getLogger(QueryModel.class.getName()).log(Level.SEVERE, null, ex);
        }   
        return test_name;
    }
    
    private static int getLogTestID(int log_id){
        int test_id = 0;     
        try {
            ResultSet rsList = MySQLSSHConnector.executeQuery("SELECT test_id FROM player_logs " +
                                                              "WHERE log_id = " + log_id +";");
            
            if(rsList.next()) {
                test_id =  rsList.getInt("test_id");
            }
        } catch (SQLException ex) {
            Logger.getLogger(QueryModel.class.getName()).log(Level.SEVERE, null, ex);
        }
        return test_id;
    }
}
