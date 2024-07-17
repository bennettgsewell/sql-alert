using Microsoft.Data.SqlClient;

// ChatGPT wrote the base for this, I improved it

const string queryFilePath = "query.sql";
const string connectionStringFilePath = "connectionString.txt";

// Read SQL query from file
string sqlQuery = await File.ReadAllTextAsync(queryFilePath);

// Read SQL connection string from file
string connectionString = await File.ReadAllTextAsync(connectionStringFilePath);

// Ask user for the interval (in seconds) to run the query
Console.Write("Enter the interval in seconds to run the query: ");
int intervalSeconds;
while (!int.TryParse(Console.ReadLine(), out intervalSeconds) || intervalSeconds <= 0)
{
    Console.WriteLine("Please enter a valid positive integer for the interval.");
}

Console.WriteLine("Monitoring query results...");

using var conn = new SqlConnection(connectionString);
await conn.OpenAsync();

// Run the query at the specified interval
while (true)
{
    bool hasResults = await CheckQueryResultsAsync(conn, sqlQuery);
    if (hasResults)
    {
        FlashConsoleAlert();
        break;
    }

    await Task.Delay(TimeSpan.FromSeconds(intervalSeconds));
}

static async Task<bool> CheckQueryResultsAsync(SqlConnection conn, string sqlQuery)
{
    try
    {
        using SqlCommand command = new SqlCommand(sqlQuery, conn);

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        return reader.HasRows;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        return false;
    }
}

static void FlashConsoleAlert()
{
    Console.Clear();
    for (int i = 0; i < 10; i++)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Black;
        Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2);
        Console.Write("Alert!!!\nQuery has returned results!!!");
        Thread.Sleep(500);

        Console.ResetColor();
        Console.Clear();
        Thread.Sleep(500);
    }
}
