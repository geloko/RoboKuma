package controller;

import db_connection.File;
import db_connection.MySQLSSHConnector;
import db_connection.TXTFile;
import javax.swing.JOptionPane;

public class Driver {

    public static void main(String[] args) {
	//File file = new TXTFile("src/db_connection/config.txt");
	//if(file.getConnector().getConnection() != null){
        if(MySQLSSHConnector.getInstance() != null){
            Controller.getInstance();
        }else
            JOptionPane.showMessageDialog(null, "Error connecting to Remote Server!", "Message", JOptionPane.ERROR_MESSAGE);
    } 
}
