using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public class DatabaseManager
{
    private string connectionString = "Server=(localdb)\\MSSQLLocalDB;Trusted_Connection=True;MultipleActiveResultSets=true";

    public void InitializeDatabase()
    {
        try
        {
            if (!IsDatabaseExists())
            {
                CreateDatabase();
            }
            CreateTables();
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Error initializing the database: " + ex.Message);
        }
    }

    public Calculation GetCalculation(int calculationId)
    {
        string getCalculationQuery = "SELECT * FROM [Calculation] WHERE Id = @CalculationId";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(getCalculationQuery, connection))
        {
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@CalculationId", calculationId);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Calculation calculation = new Calculation
                    {
                        Id = (int)reader["Id"],
                        UserId = (int)reader["UserId"],
                        InitialInvestment = (double)(decimal)reader["InitialInvestment"],
                        DiscountRate = (double)(decimal)reader["DiscountRate"],
                        InflationRate = (double)(decimal)reader["InflationRate"],
                        TaxRate = (double)(decimal)reader["TaxRate"],
                        PoliticalStabilityRating = (int)reader["PoliticalStabilityRating"],
                        NPV = (double)(decimal)reader["NPV"]
                    };

                    return calculation;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error retrieving the calculation: " + ex.Message);
            }
        }

        return null;
    }

    public List<Calculation> GetCalculations(int userId)
    {
        List<Calculation> calculations = new List<Calculation>();
        string getCalculationsQuery = "SELECT * FROM [Calculation] WHERE UserId = @UserId";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(getCalculationsQuery, connection))
        {
            try
            {
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Calculation calculation = new Calculation
                    {
                        Id = (int)reader["Id"],
                        UserId = (int)reader["UserId"],
                        InitialInvestment = (double)(decimal)reader["InitialInvestment"],
                        DiscountRate = (double)(decimal)reader["DiscountRate"],
                        InflationRate = (double)(decimal)reader["InflationRate"],
                        TaxRate = (double)(decimal)reader["TaxRate"],
                        PoliticalStabilityRating = (int)reader["PoliticalStabilityRating"],
                        NPV = (double)(decimal)reader["NPV"]
                    };

                    calculations.Add(calculation);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error retrieving calculations: " + ex.Message);
            }
        }

        return calculations;
    }

    public bool DeleteCalculation(int calculationId)
    {
        string deleteCalculationQuery = "DELETE FROM [Calculation] WHERE Id = @CalculationId";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(deleteCalculationQuery, connection))
        {
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@CalculationId", calculationId);
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error deleting the calculation: " + ex.Message);
                return false;
            }
        }
    }

    private void CreateDatabase()
    {
        string createDatabaseQuery = "CREATE DATABASE InvestmentProjectDB";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(createDatabaseQuery, connection))
        {
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error creating the database: " + ex.Message);
            }
        }
    }

    private void CreateTables()
    {
        string createUserTableQuery = @"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[User]') AND type in (N'U'))
            BEGIN
                CREATE TABLE [User] (
                    Id INT PRIMARY KEY IDENTITY,
                    Username NVARCHAR(50) UNIQUE NOT NULL,
                    Password NVARCHAR(50) NOT NULL
                )
            END";

        string createCalculationTableQuery = @"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Calculation]') AND type in (N'U'))
            BEGIN
                CREATE TABLE [Calculation] (
                    Id INT PRIMARY KEY IDENTITY,
                    UserId INT NOT NULL,
                    InitialInvestment DECIMAL(18, 2) NOT NULL,
                    DiscountRate DECIMAL(5, 2) NOT NULL,
                    InflationRate DECIMAL(5, 2) NOT NULL,
                    TaxRate DECIMAL(5, 2) NOT NULL,
                    PoliticalStabilityRating INT NOT NULL,
                    NPV DECIMAL(18, 2) NOT NULL,
                    FOREIGN KEY (UserId) REFERENCES [User](Id)
                )
            END";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(createUserTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand(createCalculationTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error creating the tables: " + ex.Message);
            }
        }
    }

    public bool RegisterUser(string username, string password)
    {
        string insertUserQuery = "INSERT INTO [User] (Username, Password) VALUES (@Username, @Password)";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(insertUserQuery, connection))
        {
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error registering the user: " + ex.Message);
                return false;
            }
        }
    }

    private bool IsDatabaseExists()
    {
        string databaseName = "InvestmentProjectDB";
        string checkDatabaseQuery = "SELECT COUNT(*) FROM sys.databases WHERE name = @DatabaseName";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(checkDatabaseQuery, connection))
        {
            command.Parameters.AddWithValue("@DatabaseName", databaseName);
            connection.Open();
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
    }
    public int AuthenticateUser(string username, string password)
    {
        string authenticateUserQuery = "SELECT Id FROM [User] WHERE Username = @Username AND Password = @Password";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(authenticateUserQuery, connection))
        {
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                object result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error authenticating the user: " + ex.Message);
                return -1;
            }
        }
    }

    public bool SaveCalculation(int userId, double initialInvestment, double discountRate, double inflationRate, double taxRate, int politicalStabilityRating, double npv)
    {
        string saveCalculationQuery = @"
    INSERT INTO [Calculation] (UserId, InitialInvestment, DiscountRate, InflationRate, TaxRate, PoliticalStabilityRating, NPV)
    VALUES (@UserId, @InitialInvestment, @DiscountRate, @InflationRate, @TaxRate, @PoliticalStabilityRating, @NPV)";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(saveCalculationQuery, connection))
        {
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@InitialInvestment", initialInvestment);
                command.Parameters.AddWithValue("@DiscountRate", discountRate);
                command.Parameters.AddWithValue("@InflationRate", inflationRate);
                command.Parameters.AddWithValue("@TaxRate", taxRate);
                command.Parameters.AddWithValue("@PoliticalStabilityRating", politicalStabilityRating);
                command.Parameters.AddWithValue("@NPV", npv);
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                ShowErrorMessage("Error saving the calculation: " + ex.Message);
                return false;
            }
        }
    }

    public DataTable GetCalculationHistory(int userId)
    {
        DataTable calculationHistory = new DataTable();
        string query = "SELECT InitialInvestment, DiscountRate, InflationRate, TaxRate, PoliticalStabilityRating, NPV FROM [Calculation] WHERE UserId = @UserId";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
        {
            try
            {
                command.Parameters.AddWithValue("@UserId", userId);
                adapter.Fill(calculationHistory);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error retrieving calculation history: " + ex.Message);
            }
        }
        return calculationHistory;
    }

    public string GetUsername(int userId)
    {
        string query = "SELECT Username FROM [User] WHERE Id = @UserId";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@UserId", userId);
            connection.Open();
            object result = command.ExecuteScalar();
            return result != null ? result.ToString() : string.Empty;
        }
    }

    public bool CheckUsernameExists(string username)
    {
        string query = "SELECT COUNT(*) FROM [User] WHERE Username = @Username";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Username", username);
            connection.Open();
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
    }
    public bool UpdateUsername(int userId, string newUsername)
    {
        string query = "UPDATE [User] SET Username = @NewUsername WHERE Id = @UserId";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@NewUsername", newUsername);
            command.Parameters.AddWithValue("@UserId", userId);
            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }
    }

    private void ShowErrorMessage(string message)
    {
        MessageBox.Show("Database Error: " + message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
