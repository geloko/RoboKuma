
package view;

import java.awt.CardLayout;
import controller.Controller;
import java.util.ArrayList;
import javax.swing.JPanel;
import javax.swing.UIManager;
import javax.swing.WindowConstants;
import javax.swing.table.DefaultTableModel;
import model.Corsi;
import model.Eriksen;
import model.GoNoGo;
import model.Log;
import model.NBack;

public class AppView extends javax.swing.JFrame {
    private Controller control;
    private DefaultTableModel all_model;
    private DefaultTableModel gonogo_model;
    private DefaultTableModel nback_model;
    private DefaultTableModel eriksen_model;
    private DefaultTableModel corsi_model;
    
    public AppView(Controller control) {
        try {
            for (UIManager.LookAndFeelInfo info : UIManager.getInstalledLookAndFeels()) {
                if ("Windows".equals(info.getName())) {
                    UIManager.setLookAndFeel(info.getClassName());
                    break;
                }
            }
        } catch (Exception e) {}
        
        this.control = control;
        initComponents();
        
        all_table.setAutoCreateRowSorter(true);
        all_model = (DefaultTableModel) all_table.getModel();
        
        gonogo_table.setAutoCreateRowSorter(true);
        gonogo_model = (DefaultTableModel) gonogo_table.getModel();
        
        nback_table.setAutoCreateRowSorter(true);
        nback_model = (DefaultTableModel) nback_table.getModel();
        
        eriksen_table.setAutoCreateRowSorter(true);
        eriksen_model = (DefaultTableModel) eriksen_table.getModel();
        
        corsi_table.setAutoCreateRowSorter(true);
        corsi_model = (DefaultTableModel) corsi_table.getModel();
        
        this.setResizable(false);
        setLocationRelativeTo(null);  
        setVisible(true);
        this.setDefaultCloseOperation(WindowConstants.EXIT_ON_CLOSE);
    }
    
    public void updateAll(ArrayList<Log> list){
        Object temp[];
        
        all_model.getDataVector().removeAllElements();
        all_model.fireTableDataChanged();
        
        for(int i = 0; i < list.size(); i++) {
            temp = new Object[]{list.get(i).getLog_id(),
                                list.get(i).getPlayer_id(),
                                list.get(i).getTest_name(),
                                list.get(i).getTime_start(),
                                list.get(i).getTime_end()};
            
            all_model.addRow(temp);
        }
    }
    
    public void updateRowCount(int count){
        rowCountLabel.setText("Returned: " + count + " rows");
    }
    
    public void updateGoNoGo(ArrayList<GoNoGo> list){
        Object temp[];
        
        CardLayout card = (CardLayout)main_panel.getLayout();
        card.show(main_panel, "gonogo_panel");
        
        gonogo_model.getDataVector().removeAllElements();
        gonogo_model.fireTableDataChanged();
        
        for(int i = 0; i < list.size(); i++) {
            temp = new Object[]{list.get(i).getLog_id(),
                                list.get(i).getPlayer_id(),
                                list.get(i).getCorrect_go_count(),
                                list.get(i).getCorrect_nogo_count(),
                                list.get(i).getMean_time(),
                                list.get(i).getGo_count(),
                                list.get(i).getTrial_count(),
                                list.get(i).getDate_start(),
                                list.get(i).getDate_end()};
            
            gonogo_model.addRow(temp);
        }
    }
    
    public void updateNBack(ArrayList<NBack> list){
        Object temp[];
        
        CardLayout card = (CardLayout)main_panel.getLayout();
        card.show(main_panel, "nback_panel");
        
        nback_model.getDataVector().removeAllElements();
        nback_model.fireTableDataChanged();
        
        for(int i = 0; i < list.size(); i++) {
            temp = new Object[]{list.get(i).getLog_id(),
                                list.get(i).getPlayer_id(),
                                list.get(i).getCorrect_count(),
                                list.get(i).getMean_time(),
                                list.get(i).getN_count(),
                                list.get(i).getElement_count(),
                                list.get(i).getTrial_count(),
                                list.get(i).getDate_start(),
                                list.get(i).getDate_end()};
            
            nback_model.addRow(temp);
        }
    }
    
    public void updateEriksen(ArrayList<Eriksen> list){
        Object temp[];
        
        CardLayout card = (CardLayout)main_panel.getLayout();
        card.show(main_panel, "eriksen_panel");
        
        eriksen_model.getDataVector().removeAllElements();
        eriksen_model.fireTableDataChanged();
        
        for(int i = 0; i < list.size(); i++) {
            temp = new Object[]{list.get(i).getLog_id(),
                                list.get(i).getPlayer_id(),
                                list.get(i).getCorrect_congruent(),
                                list.get(i).getTime_congruent(),
                                list.get(i).getCorrect_incongruent(),
                                list.get(i).getTime_incongruent(),
                                list.get(i).getCongruent_count(),
                                list.get(i).getTrial_count(),
                                list.get(i).getDate_start(),
                                list.get(i).getDate_end()};
            
            eriksen_model.addRow(temp);
        }
    }
    
    public void updateCorsi(ArrayList<Corsi> list){
        Object temp[];
        
        CardLayout card = (CardLayout)main_panel.getLayout();
        card.show(main_panel, "corsi_panel");
        
        corsi_model.getDataVector().removeAllElements();
        corsi_model.fireTableDataChanged();
        
        for(int i = 0; i < list.size(); i++) {
            temp = new Object[]{list.get(i).getLog_id(),
                                list.get(i).getPlayer_id(),
                                list.get(i).getCorrect_trials(),
                                list.get(i).getCorrect_length(),
                                list.get(i).getSeq_length(),
                                list.get(i).getTrial_count(),
                                list.get(i).getDate_start(),
                                list.get(i).getDate_end()};
            
            corsi_model.addRow(temp);
        }
    }

    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        jPanel1 = new javax.swing.JPanel();
        jSeparator1 = new javax.swing.JSeparator();
        jLabel1 = new javax.swing.JLabel();
        jPanel2 = new javax.swing.JPanel();
        logs_combo = new javax.swing.JComboBox<>();
        refresh_btn = new javax.swing.JButton();
        main_panel = new javax.swing.JPanel();
        all_panel = new javax.swing.JPanel();
        jScrollPane1 = new javax.swing.JScrollPane();
        all_table = new javax.swing.JTable();
        gonogo_panel = new javax.swing.JPanel();
        jScrollPane2 = new javax.swing.JScrollPane();
        gonogo_table = new javax.swing.JTable();
        nback_panel = new javax.swing.JPanel();
        jScrollPane3 = new javax.swing.JScrollPane();
        nback_table = new javax.swing.JTable();
        eriksen_panel = new javax.swing.JPanel();
        jScrollPane4 = new javax.swing.JScrollPane();
        eriksen_table = new javax.swing.JTable();
        corsi_panel = new javax.swing.JPanel();
        jScrollPane5 = new javax.swing.JScrollPane();
        corsi_table = new javax.swing.JTable();
        rowCountLabel = new javax.swing.JLabel();
        exportAllBtn = new javax.swing.JButton();
        exportTableBtn = new javax.swing.JButton();

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
        setTitle("RoboKuma Export Tool");

        jLabel1.setFont(new java.awt.Font("Trebuchet MS", 1, 14)); // NOI18N
        jLabel1.setText("RoboKuma System Dashboard");

        jPanel2.setBorder(javax.swing.BorderFactory.createTitledBorder("Log Manager"));

        logs_combo.setModel(new javax.swing.DefaultComboBoxModel<>(new String[] { "Player Logs", "Go/No-Go Task", "N-Back Task", "Eriksen Flanker","Corsi Block-Tapping" }));
        logs_combo.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                logs_comboActionPerformed(evt);
            }
        });

        refresh_btn.setText("Refresh");
        refresh_btn.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                refresh_btnActionPerformed(evt);
            }
        });

        main_panel.setBorder(javax.swing.BorderFactory.createEtchedBorder());
        main_panel.setLayout(new java.awt.CardLayout());

        all_table.setModel(new javax.swing.table.DefaultTableModel(
            new Object [][] {
                {null, null, null, null, null},
                {null, null, null, null, null},
                {null, null, null, null, null},
                {null, null, null, null, null}
            },
            new String [] {
                "Log ID", "Player ID", "Test", "Time Started", "Time Ended"
            }
        ));
        jScrollPane1.setViewportView(all_table);

        javax.swing.GroupLayout all_panelLayout = new javax.swing.GroupLayout(all_panel);
        all_panel.setLayout(all_panelLayout);
        all_panelLayout.setHorizontalGroup(
            all_panelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jScrollPane1, javax.swing.GroupLayout.DEFAULT_SIZE, 749, Short.MAX_VALUE)
        );
        all_panelLayout.setVerticalGroup(
            all_panelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jScrollPane1, javax.swing.GroupLayout.DEFAULT_SIZE, 335, Short.MAX_VALUE)
        );

        main_panel.add(all_panel, "all_card");

        gonogo_table.setModel(new javax.swing.table.DefaultTableModel(
            new Object [][] {
                {null, null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null, null}
            },
            new String [] {
                "Log ID", "Player ID", "Correct Go Count", "Correct NoGo Count", "Avg. Reaction Time", "Go Count","Trial Count","Time Started", "Time Ended"
            }
        ));
        jScrollPane2.setViewportView(gonogo_table);

        javax.swing.GroupLayout gonogo_panelLayout = new javax.swing.GroupLayout(gonogo_panel);
        gonogo_panel.setLayout(gonogo_panelLayout);
        gonogo_panelLayout.setHorizontalGroup(
            gonogo_panelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jScrollPane2, javax.swing.GroupLayout.DEFAULT_SIZE, 749, Short.MAX_VALUE)
        );
        gonogo_panelLayout.setVerticalGroup(
            gonogo_panelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jScrollPane2, javax.swing.GroupLayout.DEFAULT_SIZE, 335, Short.MAX_VALUE)
        );

        main_panel.add(gonogo_panel, "gonogo_card");

        nback_table.setModel(new javax.swing.table.DefaultTableModel(
            new Object [][] {
                {null, null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null, null}
            },
            new String [] {
                "Log ID", "Player ID", "Correct Percentage", "Avg. Reaction Time", "n Size", "Unique Elements", "Trial Count", "Time Started", "Time Ended"
            }
        ));
        jScrollPane3.setViewportView(nback_table);

        javax.swing.GroupLayout nback_panelLayout = new javax.swing.GroupLayout(nback_panel);
        nback_panel.setLayout(nback_panelLayout);
        nback_panelLayout.setHorizontalGroup(
            nback_panelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jScrollPane3, javax.swing.GroupLayout.DEFAULT_SIZE, 749, Short.MAX_VALUE)
        );
        nback_panelLayout.setVerticalGroup(
            nback_panelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jScrollPane3, javax.swing.GroupLayout.DEFAULT_SIZE, 335, Short.MAX_VALUE)
        );

        main_panel.add(nback_panel, "nback_card");

        eriksen_table.setModel(new javax.swing.table.DefaultTableModel(
            new Object [][] {
                {null, null, null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null, null, null}
            },
            new String [] {
                "Log ID", "Player ID", "Correct Congruent", "Avg. Congruent Time", "Correct Incongruent", "Avg. Incongruent Time", "Congruent Count", "Trial Count", "Time Started", "Time Ended"
            }
        ));
        jScrollPane4.setViewportView(eriksen_table);

        javax.swing.GroupLayout eriksen_panelLayout = new javax.swing.GroupLayout(eriksen_panel);
        eriksen_panel.setLayout(eriksen_panelLayout);
        eriksen_panelLayout.setHorizontalGroup(
            eriksen_panelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jScrollPane4, javax.swing.GroupLayout.DEFAULT_SIZE, 749, Short.MAX_VALUE)
        );
        eriksen_panelLayout.setVerticalGroup(
            eriksen_panelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jScrollPane4, javax.swing.GroupLayout.DEFAULT_SIZE, 335, Short.MAX_VALUE)
        );

        main_panel.add(eriksen_panel, "eriksen_card");

        corsi_table.setModel(new javax.swing.table.DefaultTableModel(
            new Object [][] {
                {null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null}
            },
            new String [] {
                "Log ID", "Player ID", "Correct Trials", "Avg. Correct Length", "Seq. Length", "Trial Count", "Time Started", "Time Ended"
            }
        ));
        jScrollPane5.setViewportView(corsi_table);

        javax.swing.GroupLayout corsi_panelLayout = new javax.swing.GroupLayout(corsi_panel);
        corsi_panel.setLayout(corsi_panelLayout);
        corsi_panelLayout.setHorizontalGroup(
            corsi_panelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jScrollPane5, javax.swing.GroupLayout.DEFAULT_SIZE, 749, Short.MAX_VALUE)
        );
        corsi_panelLayout.setVerticalGroup(
            corsi_panelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jScrollPane5, javax.swing.GroupLayout.DEFAULT_SIZE, 335, Short.MAX_VALUE)
        );

        main_panel.add(corsi_panel, "corsi_card");

        exportAllBtn.setText("Export All");
        exportAllBtn.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                exportAllBtnActionPerformed(evt);
            }
        });

        exportTableBtn.setText("Export Table");
        exportTableBtn.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                exportTableBtnActionPerformed(evt);
            }
        });

        javax.swing.GroupLayout jPanel2Layout = new javax.swing.GroupLayout(jPanel2);
        jPanel2.setLayout(jPanel2Layout);
        jPanel2Layout.setHorizontalGroup(
            jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel2Layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING)
                    .addComponent(main_panel, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                    .addGroup(jPanel2Layout.createSequentialGroup()
                        .addComponent(logs_combo, javax.swing.GroupLayout.PREFERRED_SIZE, 171, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(refresh_btn)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(rowCountLabel, javax.swing.GroupLayout.PREFERRED_SIZE, 177, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addGap(132, 132, 132)
                        .addComponent(exportTableBtn)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(exportAllBtn, javax.swing.GroupLayout.PREFERRED_SIZE, 93, javax.swing.GroupLayout.PREFERRED_SIZE)))
                .addContainerGap())
        );
        jPanel2Layout.setVerticalGroup(
            jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel2Layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(logs_combo, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(refresh_btn)
                    .addComponent(rowCountLabel, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                    .addComponent(exportAllBtn)
                    .addComponent(exportTableBtn))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(main_panel, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                .addContainerGap())
        );

        javax.swing.GroupLayout jPanel1Layout = new javax.swing.GroupLayout(jPanel1);
        jPanel1.setLayout(jPanel1Layout);
        jPanel1Layout.setHorizontalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, jPanel1Layout.createSequentialGroup()
                        .addComponent(jPanel2, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addContainerGap())
                    .addGroup(jPanel1Layout.createSequentialGroup()
                        .addComponent(jLabel1, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addGap(414, 414, 414))
                    .addGroup(jPanel1Layout.createSequentialGroup()
                        .addComponent(jSeparator1, javax.swing.GroupLayout.PREFERRED_SIZE, 227, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))))
        );
        jPanel1Layout.setVerticalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addContainerGap()
                .addComponent(jLabel1, javax.swing.GroupLayout.PREFERRED_SIZE, 21, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(jSeparator1, javax.swing.GroupLayout.PREFERRED_SIZE, 5, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(10, 10, 10)
                .addComponent(jPanel2, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 805, Short.MAX_VALUE)
            .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                .addComponent(jPanel1, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 466, Short.MAX_VALUE)
            .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                .addComponent(jPanel1, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void logs_comboActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_logs_comboActionPerformed
        
    }//GEN-LAST:event_logs_comboActionPerformed

    private void refresh_btnActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_refresh_btnActionPerformed
        CardLayout card = (CardLayout)main_panel.getLayout();
        
        switch(logs_combo.getSelectedItem().toString()){
            case "Player Logs"          : control.updateAll(); card.show(main_panel, "all_card"); 
                                            control.updateRowCount(0); break;
            case "Go/No-Go Task"        : control.updateTestLogs(1); card.show(main_panel, "gonogo_card"); 
                                            control.updateRowCount(1); break;
            case "N-Back Task"          : control.updateTestLogs(2); card.show(main_panel, "nback_card");
                                            control.updateRowCount(2); break;
            case "Eriksen Flanker"      : control.updateTestLogs(3); card.show(main_panel, "eriksen_card");
                                            control.updateRowCount(3); break;
            case "Corsi Block-Tapping"  : control.updateTestLogs(4); card.show(main_panel, "corsi_card");
                                            control.updateRowCount(4); break;
        }
    }//GEN-LAST:event_refresh_btnActionPerformed

    private void exportTableBtnActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_exportTableBtnActionPerformed
        switch(logs_combo.getSelectedItem().toString()){
            case "Player Logs"          : control.exportPlayerLogs(); break;
            case "Go/No-Go Task"        : control.exportTestLogs(1); break;
            case "N-Back Task"          : control.exportTestLogs(2); break;
            case "Eriksen Flanker"      : control.exportTestLogs(3); break;
            case "Corsi Block-Tapping"  : control.exportTestLogs(4); break;
        }
    }//GEN-LAST:event_exportTableBtnActionPerformed

    private void exportAllBtnActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_exportAllBtnActionPerformed
        control.exportAll();
    }//GEN-LAST:event_exportAllBtnActionPerformed

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JPanel all_panel;
    private javax.swing.JTable all_table;
    private javax.swing.JPanel corsi_panel;
    private javax.swing.JTable corsi_table;
    private javax.swing.JPanel eriksen_panel;
    private javax.swing.JTable eriksen_table;
    private javax.swing.JButton exportAllBtn;
    private javax.swing.JButton exportTableBtn;
    private javax.swing.JPanel gonogo_panel;
    private javax.swing.JTable gonogo_table;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JPanel jPanel1;
    private javax.swing.JPanel jPanel2;
    private javax.swing.JScrollPane jScrollPane1;
    private javax.swing.JScrollPane jScrollPane2;
    private javax.swing.JScrollPane jScrollPane3;
    private javax.swing.JScrollPane jScrollPane4;
    private javax.swing.JScrollPane jScrollPane5;
    private javax.swing.JSeparator jSeparator1;
    private javax.swing.JComboBox<String> logs_combo;
    private javax.swing.JPanel main_panel;
    private javax.swing.JPanel nback_panel;
    private javax.swing.JTable nback_table;
    private javax.swing.JButton refresh_btn;
    private javax.swing.JLabel rowCountLabel;
    // End of variables declaration//GEN-END:variables
}
