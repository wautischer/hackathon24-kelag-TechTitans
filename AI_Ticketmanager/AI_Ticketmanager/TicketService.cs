using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

public class TicketService
{
    private string connectionString = "Server=tcp:sql-hackathon-team4.database.windows.net,1433;Initial Catalog=sqldb-hackathon-team4;Persist Security Info=False;User ID=hackathonTeam4;Password=abc#123#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    public async Task<List<Ticket>> GetTicketsAsync()
    {
        List<Ticket> tickets = new List<Ticket>();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT id, title, tags, description, created_at FROM dbo.tickets", conn);
                DbDataReader dbReader = await cmd.ExecuteReaderAsync();

                while (await dbReader.ReadAsync())
                {
                    tickets.Add(new Ticket
                    {
                        id = dbReader.GetInt32(0),
                        title = dbReader.GetString(1),
                        tags = dbReader.GetString(2),
                        description = dbReader.GetString(3),
                        created_at = dbReader.GetDateTime(4)
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
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO dbo.tickets (title, tags, description, created_at) VALUES (@Title, @Tags, @Description, @CreatedAt)", 
                    conn);

                cmd.Parameters.AddWithValue("@Title", ticket.title);
                cmd.Parameters.AddWithValue("@Tags", ticket.tags ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Description", ticket.description);
                cmd.Parameters.AddWithValue("@CreatedAt", ticket.created_at);

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
