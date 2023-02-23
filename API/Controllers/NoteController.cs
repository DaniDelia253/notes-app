using API.Data;
using API.Data.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class NoteController : ControllerBase
{
    private readonly ILogger<NoteController> _logger;
    private readonly string? connectionString;

    public NoteController(ILogger<NoteController> logger, IConfiguration config)
    {
        _logger = logger;
        connectionString = config.GetValue<string>("noteTaker:ConnectionString");
    }
    //TODO: Add error handling!
    [HttpGet]
    public async Task<IEnumerable<Note>> GetAllNotesAsync()
    {
        var notes = new List<Note>();

        string query = "SELECT * FROM notes";

        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            notes.Add(new Note
            {
                note_id = (int)reader[0],
                user_id = (int)reader[1],
                title = reader[2].ToString(),
                content = reader[3].ToString(),
            });
        }
        connector.CloseConnection();
        return notes;
    }
    [HttpGet("id/{id}")]
    public async Task<Note> GetANoteByIdAsync(int id)
    {

        string query = $"SELECT * FROM notes WHERE note_id = {id}";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var reader = await command.ExecuteReaderAsync();

        reader.Read();

        Console.WriteLine("________________________");
        Console.WriteLine(reader[0]);
        Console.WriteLine(reader[1]);
        Console.WriteLine(reader[2]);
        Console.WriteLine(reader[3]);
        Console.WriteLine("________________________");


        var note = new Note
        {
            note_id = (int)reader[0],
            user_id = (int)reader[1],
            title = reader[2].ToString(),
            content = reader[3].ToString(),
        };
        connector.CloseConnection();
        return note;
    }
    [HttpGet("user/{id}")]
    public async Task<IEnumerable<Note>> GetAllNotesForAUserByUerIdAsync(int id)
    {

        var notes = new List<Note>();

        string query = $"SELECT * FROM notes WHERE user_id = {id}";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            notes.Add(new Note
            {
                note_id = (int)reader[0],
                user_id = (int)reader[1],
                title = reader[2].ToString(),
                content = reader[3].ToString(),
            });
        }
        connector.CloseConnection();
        return notes;
    }
    [HttpPost]
    public async Task<int> CreateNewNoteAsync(NewNoteDTO newNote)
    {
        string query = $"INSERT INTO notes (user_id, title, content) VALUES ({newNote.user_id}, \"{newNote.title}\", \"{newNote.content}\")";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var result = await command.ExecuteNonQueryAsync();
        connector.CloseConnection();
        return result;
    }
    [HttpPut("updateNote")]
    public async Task<ActionResult> UpdateExistingNoteAsync(Note updatedNote)
    {
        string query = $"UPDATE notes SET title = \"{updatedNote.title}\", content = \"{updatedNote.content}\" WHERE note_id = {updatedNote.note_id}";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var result = await command.ExecuteNonQueryAsync();
        connector.CloseConnection();
        return Ok();
    }
    [HttpDelete("delete/{id}")]
    public async Task<int> DeleteUserAsync(int id)
    {
        string query = $"DELETE FROM notes WHERE note_id = {id}";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var result = await command.ExecuteNonQueryAsync();
        connector.CloseConnection();
        return result;
    }
}