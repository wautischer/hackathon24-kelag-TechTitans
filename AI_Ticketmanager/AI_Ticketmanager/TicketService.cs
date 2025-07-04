using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

public class TicketService
{

    private string passworddb = Environment.GetEnvironmentVariable("PASSWORD_DB");
    private string connectionString;

    public TicketService()
    {
        connectionString = GetConnectionString().GetAwaiter().GetResult();
    }

    private async Task<string> GetConnectionString()
    {
        Console.WriteLine(passworddb);
        if (string.IsNullOrEmpty(passworddb) )
        {
            throw new InvalidOperationException("Fail");
        }

        Console.WriteLine($"Secret value retrieved successfully.");
        return $"Server=tcp:sql-hackathon-team4.database.windows.net,1433;Initial Catalog=sqldb-hackathon-team4;Persist Security Info=False;User ID=hackathonTeam4;Password={passworddb};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    }

    public async Task<List<Ticket>> GetTicketsAsync()
    {
        var tickets = new List<Ticket>();

        await using var conn = new SqlConnection(connectionString);
        try
        {
            await conn.OpenAsync();
            var cmd = new SqlCommand("SELECT id, title, tags, description, created_at, priority, descriptionLong FROM dbo.tickets", conn);
            DbDataReader dbReader = await cmd.ExecuteReaderAsync();

            while (await dbReader.ReadAsync())
            {
                tickets.Add(new Ticket
                {
                    id = dbReader.GetInt32(0),
                    title = dbReader?.GetString(1) ?? "NULL",
                    tags = dbReader?.GetString(2) ?? "NULL",
                    description = dbReader?.GetString(3) ?? "NULL",
                    created_at = dbReader!.GetDateTime(4),
                    priority = dbReader?.GetString(5) ?? "NULL",
                    descriptionLong = dbReader?.GetString(6) ?? "NULL"
                });
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"SQL error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error: {ex.Message}");
        }

        return tickets;
    }

    public async Task InsertTicketAsync(Ticket ticket)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("INSERT INTO dbo.tickets (title, tags, description, created_at, priority, descriptionLong) VALUES (@Title, @Tags, @Description, @CreatedAt, @Priority, @DescriptionLong)", conn);

                cmd.Parameters.AddWithValue("@Title", ticket.title);
                cmd.Parameters.AddWithValue("@Tags", ticket.tags);
                cmd.Parameters.AddWithValue("@Description", ticket.description);
                cmd.Parameters.AddWithValue("@CreatedAt", ticket.created_at);
                cmd.Parameters.AddWithValue("@Priority", ticket.priority);
                cmd.Parameters.AddWithValue("@DescriptionLong", ticket.descriptionLong);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL error: {ex.Message}");
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"General error: {ex.Message}");
            }
        }
    }
}
