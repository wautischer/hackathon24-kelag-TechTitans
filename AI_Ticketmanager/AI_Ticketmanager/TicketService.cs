using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

public class TicketService
{
    private string connectionString = "server=sql-hackathon-team4.database.windows.net;database=sqldb-hackathon-team4;user=hackathonTeam4;password=abc#123#";

    public async Task<List<Ticket>> GetTicketsAsync()
    {
        List<Ticket> tickets = new List<Ticket>();

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                await conn.OpenAsync();
                MySqlCommand cmd = new MySqlCommand("SELECT id, title, tags, description, created_at FROM ticket", conn);
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
            catch (MySqlException ex)
            {
                // Handle MySQL-specific exceptions
                Console.WriteLine($"MySQL error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"General error: {ex.Message}");
            }
        }

        return tickets;
    }
}
