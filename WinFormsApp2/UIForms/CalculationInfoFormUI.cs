using System.Data;

public class CalculationInfoFormUI : BaseForm
{
    private DataGridView dgvCalculationHistory;
    private int userId;
    private DatabaseManager databaseManager;
    private BindingSource bindingSource;
    private ToolStrip toolStrip;

    public CalculationInfoFormUI(int userId)
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

        bindingSource = new BindingSource();

        toolStrip = new ToolStrip();
        toolStrip.Dock = DockStyle.Top;

        dgvCalculationHistory = new DataGridView();
        dgvCalculationHistory.Location = new Point(20, 50);
        dgvCalculationHistory.Size = new Size(760, 500);

        Controls.Add(toolStrip);
        Controls.Add(dgvCalculationHistory);

        CreateToolStripControls();

        FormClosed += CalculationInfoFormUI_FormClosed;
    }

    private void CalculationInfoFormUI_FormClosed(object sender, FormClosedEventArgs e)
    {
        dgvCalculationHistory.Dispose();
        dgvCalculationHistory = null;
        FormClosed -= CalculationInfoFormUI_FormClosed;
    }

    private void CreateToolStripControls()
    {
        var filterTextBox = new ToolStripTextBox("filterTextBox");
        filterTextBox.ToolTipText = "Filter";
        filterTextBox.TextChanged += FilterTextBox_TextChanged;

        var clearFilterButton = new ToolStripButton("Clear Filter");
        clearFilterButton.ToolTipText = "Clear Filter";
        clearFilterButton.Click += ClearFilterButton_Click;

        var sortComboBox = new ToolStripComboBox("sortComboBox");
        sortComboBox.ToolTipText = "Sort";
        sortComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        sortComboBox.SelectedIndexChanged += SortComboBox_SelectedIndexChanged;

        toolStrip.Items.AddRange(new ToolStripItem[] { filterTextBox, clearFilterButton, sortComboBox });
    }

    private async void LoadCalculationHistoryAsync()
    {
        try
        {
            List<Calculation> calculationHistory = await Task.Run(() => databaseManager.GetCalculationHistory(userId));

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Initial Investment", typeof(decimal));
            dataTable.Columns.Add("Discount Rate", typeof(decimal));
            dataTable.Columns.Add("Inflation Rate", typeof(decimal));
            dataTable.Columns.Add("Tax Rate", typeof(decimal));
            dataTable.Columns.Add("Political Stability Rating", typeof(int));
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

            dgvCalculationHistory.DataSource = bindingSource;
            bindingSource.DataSource = dataTable;

            dgvCalculationHistory.EnableHeadersVisualStyles = false;
            dgvCalculationHistory.ColumnHeadersDefaultCellStyle.Font = new Font(dgvCalculationHistory.Font, FontStyle.Bold);
            dgvCalculationHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgvCalculationHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCalculationHistory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvCalculationHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCalculationHistory.AutoResizeColumns();

            dgvCalculationHistory.DefaultCellStyle.BackColor = Color.FromArgb(28, 28, 28);
            dgvCalculationHistory.DefaultCellStyle.ForeColor = Color.White;
            dgvCalculationHistory.GridColor = Color.FromArgb(64, 64, 64);
            dgvCalculationHistory.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgvCalculationHistory.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCalculationHistory.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvCalculationHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCalculationHistory.AllowUserToAddRows = false;
            dgvCalculationHistory.AllowUserToDeleteRows = false;

            foreach (DataGridViewColumn column in dgvCalculationHistory.Columns)
            {
                column.ToolTipText = column.HeaderText;
            }

            ToolStripComboBox sortComboBox = (ToolStripComboBox)toolStrip.Items["sortComboBox"];
            sortComboBox.Items.AddRange(dgvCalculationHistory.Columns.Cast<DataGridViewColumn>().Select(c => c.HeaderText).ToArray());
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred while loading the calculation history: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void FilterTextBox_TextChanged(object sender, EventArgs e)
    {
        string filterText = ((ToolStripTextBox)toolStrip.Items["filterTextBox"]).Text;

        if (dgvCalculationHistory.CurrentCell != null)
        {
            string columnName = dgvCalculationHistory.Columns[dgvCalculationHistory.CurrentCell.ColumnIndex].DataPropertyName;

            if (bindingSource.DataSource is DataTable dataTable)
            {
                decimal filterValue;
                if (decimal.TryParse(filterText, out filterValue))
                {
                    string filterExpression = string.Format("[{0}] = {1}", columnName, filterValue);
                    dataTable.DefaultView.RowFilter = filterExpression;
                }
                else
                {
                    dataTable.DefaultView.RowFilter = string.Empty;
                }
            }
        }
        else
        {
            bindingSource.Filter = string.Empty;
        }
    }



    private void ClearFilterButton_Click(object sender, EventArgs e)
    {
        ((ToolStripTextBox)toolStrip.Items["filterTextBox"]).Text = "";
        bindingSource.Filter = "";
    }

    private void SortComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sortColumnName = ((ToolStripComboBox)toolStrip.Items["sortComboBox"]).SelectedItem.ToString();
        SortOrder sortOrder = SortOrder.Ascending;

        if (bindingSource.Sort != null && bindingSource.Sort.Contains(sortColumnName))
        {
            if (bindingSource.Sort.EndsWith("DESC"))
                sortOrder = SortOrder.Ascending;
            else
                sortOrder = SortOrder.Descending;
        }

        bindingSource.Sort = sortColumnName + (sortOrder == SortOrder.Descending ? " DESC" : "");

        foreach (DataGridViewColumn column in dgvCalculationHistory.Columns)
        {
            column.HeaderCell.SortGlyphDirection = SortOrder.None;
            if (column.HeaderText == sortColumnName)
                column.HeaderCell.SortGlyphDirection = sortOrder == SortOrder.Ascending ? SortOrder.Ascending : SortOrder.Descending;
        }
    }
}
