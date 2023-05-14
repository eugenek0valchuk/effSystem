using System.Data.SqlClient;

public class DatabaseManager
{
    private readonly string connectionString = "Server=(localdb)\\MSSQLLocalDB;Trusted_Connection=True;MultipleActiveResultSets=true";

    public void InitializeDatabase()
    {
        try
        {
            if (!IsDatabaseExists())
                CreateDatabase();

            CreateTables();
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Error initializing the database: " + ex.Message);
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

    public List<Calculation> GetCalculationHistory(int userId)
    {
        List<Calculation> calculationHistory = new List<Calculation>();
        string query = "SELECT * FROM [Calculation] WHERE UserId = @UserId";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Calculation calculation = new Calculation
                    {
                        Id = (int)reader["Id"],
                        UserId = (int)reader["UserId"],
                        InitialInvestment = (decimal)reader["InitialInvestment"],
                        DiscountRate = (decimal)reader["DiscountRate"],
                        InflationRate = (decimal)reader["InflationRate"],
                        TaxRate = (decimal)reader["TaxRate"],
                        PoliticalStabilityRating = (int)reader["PoliticalStabilityRating"],
                        NPV = (decimal)reader["NPV"]
                    };

                    calculationHistory.Add(calculation);
                }
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
        string getUsernameQuery = "SELECT Username FROM [User] WHERE Id = @UserId";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(getUsernameQuery, connection))
        {
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@UserId", userId);
                object result = command.ExecuteScalar();
                return result != null ? result.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error retrieving the username: " + ex.Message);
                return string.Empty;
            }
        }
    }

    public bool SaveCalculation(int userId, decimal initialInvestment, decimal discountRate, decimal inflationRate, decimal taxRate, decimal politicalStabilityRating, decimal npv)
    {
        string saveCalculationQuery = "INSERT INTO Calculation (UserId, InitialInvestment, DiscountRate, InflationRate, TaxRate, PoliticalStabilityRating, NPV) " +
                                      "VALUES (@UserId, @InitialInvestment, @DiscountRate, @InflationRate, @TaxRate, @PoliticalStabilityRating, @NPV)";

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
            catch (Exception ex)
            {
                ShowErrorMessage("Error saving calculation: " + ex.Message);
                return false;
            }
        }
    }
    public bool UpdateUserCredentials(int userId, string newUsername, string newPassword)
    {
        string updateUserQuery = "UPDATE [User] SET Username = @Username, Password = @Password WHERE Id = @UserId";
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(updateUserQuery, connection))
        {
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Username", newUsername);
                command.Parameters.AddWithValue("@Password", newPassword);
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error updating user credentials: " + ex.Message);
                return false;
            }
        }
    }
    public bool UsernameExists(string username)
    {
        string checkUsernameQuery = "SELECT COUNT(*) FROM [User] WHERE Username = @Username"; 
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(checkUsernameQuery, connection))
        {
            command.Parameters.AddWithValue("@Username", username);
            connection.Open();
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
    }

    private void ShowErrorMessage(string message)
    {
        MessageBox.Show("Database Error: " + message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
