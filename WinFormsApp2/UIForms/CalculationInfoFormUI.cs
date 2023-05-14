﻿using System.Data;

public class CalculationInfoFormUI : BaseForm
{
    private DataGridView dgvCalculationHistory;
    private int userId;
    private DatabaseManager databaseManager; public CalculationInfoFormUI(int userId)
    {
        this.userId = userId;
        databaseManager = new DatabaseManager();
        InitializeComponents();
        LoadCalculationHistoryAsync();
    }

    private void InitializeComponents()
    {
        Text = "Calculation History";
        Size = new Size(800, 600);
        StartPosition = FormStartPosition.CenterScreen;

        dgvCalculationHistory = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(760, 500),
            ReadOnly = true,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };

        Controls.Add(dgvCalculationHistory);
    }

    private async void LoadCalculationHistoryAsync()
    {
        try
        {
            List<Calculation> calculationHistory = await Task.Run(() => databaseManager.GetCalculationHistory(userId));

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("InitialInvestment", typeof(decimal));
            dataTable.Columns.Add("DiscountRate", typeof(decimal));
            dataTable.Columns.Add("InflationRate", typeof(decimal));
            dataTable.Columns.Add("TaxRate", typeof(decimal));
            dataTable.Columns.Add("PoliticalStabilityRating", typeof(int));
            dataTable.Columns.Add("NPV", typeof(decimal));

            foreach (Calculation calculation in calculationHistory)
            {
                dataTable.Rows.Add(
                    calculation.InitialInvestment,
                    calculation.DiscountRate,
                    calculation.InflationRate,
                    calculation.TaxRate,
                    calculation.PoliticalStabilityRating,
                    calculation.NPV
                );
            }

            dgvCalculationHistory.DataSource = dataTable;
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred while loading the calculation history: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}