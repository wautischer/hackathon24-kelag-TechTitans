using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

public class TicketService
{
    private string connectionString = "server=localhost;database=tickets-testdata;user=root;password=root";

    public async Task<List<Ticket>> GetTicketsAsync()
    {
        List<Ticket> tickets = new List<Ticket>();

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                await conn.OpenAsync();
                MySqlCommand cmd = new MySqlCommand("SELECT id, title, tags, description FROM tickets", conn);
                DbDataReader dbReader = await cmd.ExecuteReaderAsync();

                while (await dbReader.ReadAsync())
                {
                    tickets.Add(new Ticket
                    {
                        idticket = dbReader.GetInt32(0),
                        title = dbReader.GetString(1),
                        tags = dbReader.GetString(2),
                        description = dbReader.GetString(3)
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
