package db_connection;

import com.jcraft.jsch.JSch;
import com.jcraft.jsch.JSchException;
import com.jcraft.jsch.Session;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.logging.Level;
import java.util.logging.Logger;

public class MySQLSSHConnector {
    private static int localPort;
    private static String remoteHost;
    private static String host;
    private static int remotePort;
    private static String user;
    private static String password;
    private static String dbuserName;
    private static String dbpassword;
    private static String dbName;
    private static String boundAddress;
    private static String url = "jdbc:mysql://localhost:"+localPort+"/" + dbName;
    private static String driverName="com.mysql.jdbc.Driver";
    static Connection conn = null;
    static Session session = null;
    private static MySQLSSHConnector instance = null;
    private static boolean hasRun = false;
    
    private MySQLSSHConnector(int localPort, String remoteHost, String host, int remotePort, String user,
                              String password, String dbuserName, String dbpassword, String dbName, String url,
                              String driverName) {
        this.localPort = localPort;
        this.remoteHost = remoteHost;
        this.host = host;
        this.remotePort = remotePort;
        this.user = user;
        this.password = password;
        this.dbuserName = dbuserName;
        this.dbpassword = dbpassword;
        this.dbName = dbName;
        this.url = url;
        this.driverName = driverName;
        this.boundAddress = "0.0.0.0";
        this.hasRun = false;
    }

    public static Connection getConnection() {
        if(conn == null) {
            try {
                java.util.Properties config = new java.util.Properties();
                config.put("StrictHostKeyChecking", "no");
                JSch jsch = new JSch();
                session = jsch.getSession(user, host, 22);
                session.setPassword(password);
                session.setConfig(config);
                session.connect();
                int assigned_port = session.setPortForwardingL(localPort, remoteHost, remotePort);
                System.out.println("localhost:" + assigned_port + " -> " + remoteHost + ":" + remotePort);
                System.out.println("Port Forwarded");

                Class.forName(driverName).newInstance();
                conn = DriverManager.getConnection(url, dbuserName, dbpassword);
                //return DriverManager.getConnection(url, dbuserName, dbpassword);
                return null;
            } catch (JSchException e) {
                e.printStackTrace();
            } catch (ClassNotFoundException e) {
                e.printStackTrace();
            } catch (InstantiationException e) {
                e.printStackTrace();
            } catch (IllegalAccessException e) {
                e.printStackTrace();
            } catch (SQLException e) {
                e.printStackTrace();
            }
        }
        return null;
    }

    public static MySQLSSHConnector getInstance(){
        if(instance!=null){
            return instance;
        } else {
            try {
                Class.forName("com.mysql.jdbc.Driver");
            } catch (ClassNotFoundException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }
            instance = new MySQLSSHConnector(
                    22,
                    "128.199.240.82",
                    "127.0.0.1",
                    3306,
                    "admin_kuma",
                    "admin1234",
                    "admin_kuma",
                    "admin1234",
                    "/robokuma",
                    "jdbc:mysql://127.0.0.1:22",
                    "com.mysql.jdbc.Driver");

            return instance;
        }
    }

    public static ResultSet executeQuery(String query){
        ResultSet result = null;
        getInstance().getConnection();

        if(conn == null){
            System.out.println("conn is null");
        }

        try {
            Statement st = conn.createStatement();
            result = st.executeQuery(query);
        } catch (SQLException ex) {
            Logger.getLogger(MySQLSSHConnector.class.getName()).log(Level.SEVERE, null, ex);
        }
        return result;
    }

    public static boolean executeStatement(String statement){
        getInstance().getConnection();
        boolean result = false;

        try {
            PreparedStatement preparedStatement = conn.prepareStatement(statement);
            int rowsAffected = preparedStatement.executeUpdate();

            if(rowsAffected > 0) {
                System.out.println("Statement successfully executed!");
                result = true;
            } else{
                System.out.println("No rows affected!");
            }
        } catch (SQLException e) {
            e.printStackTrace();
            System.out.println("DB error");
        }

        return result;
    }
}
