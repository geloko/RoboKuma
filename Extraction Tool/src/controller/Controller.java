package controller;

import Export.CSV_Writer;
import model.QueryModel;
import view.AppView;

public class Controller {
    private AppView appView;
    private static Controller firstInstance = null;
    
    private Controller() {
        appView = new AppView(this);
        updateAll();
        updateRowCount(0);
    }
    
    public static Controller getInstance(){
        if(firstInstance == null)
            firstInstance = new Controller();       
        
        return firstInstance;
    }
    
    public void updateAll(){
        appView.updateAll(QueryModel.getAllLogs());
    }
    
    public void updateRowCount(int i){
        switch(i){
            case 0:appView.updateRowCount(QueryModel.getAllLogsCount()); break;
            case 1:appView.updateRowCount(QueryModel.getGoNoGoLogsCount()); break;
            case 2:appView.updateRowCount(QueryModel.getNBackLogsCount()); break;
            case 3:appView.updateRowCount(QueryModel.getEriksenLogsCount()); break;
            case 4:appView.updateRowCount(QueryModel.getCorsiLogsCount()); break;
        }
    }
    
    public void updateTestLogs(int test_id){
        switch(test_id){
            case 1: appView.updateGoNoGo(QueryModel.getGoNoGoLogs()); break;
            case 2: appView.updateNBack(QueryModel.getNBackLogs()); break;
            case 3: appView.updateEriksen(QueryModel.getEriksenLogs()); break;
            case 4: appView.updateCorsi(QueryModel.getCorsiLogs()); break;
        }
    }
    
    public void exportAll(){
        CSV_Writer.exportAll(QueryModel.getAllLogs());
        CSV_Writer.exportGoNoGo(QueryModel.getGoNoGoLogs());
        CSV_Writer.exportNBack(QueryModel.getNBackLogs());
        CSV_Writer.exportEriksen(QueryModel.getEriksenLogs());
        CSV_Writer.exportCorsi(QueryModel.getCorsiLogs());
    }
    
    public void exportPlayerLogs(){
         CSV_Writer.exportAll(QueryModel.getAllLogs());
    }
    
    public void exportTestLogs(int test_id){
        switch(test_id){
            case 1: CSV_Writer.exportGoNoGo(QueryModel.getGoNoGoLogs()); break;
            case 2: CSV_Writer.exportNBack(QueryModel.getNBackLogs()); break;
            case 3: CSV_Writer.exportEriksen(QueryModel.getEriksenLogs()); break;
            case 4: CSV_Writer.exportCorsi(QueryModel.getCorsiLogs()); break;
        }
    }
}
