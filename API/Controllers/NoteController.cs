using API.Data;
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
    public IEnumerable<Note> GetAllNotes()
    {
        var notes = new List<Note>();

        string query = "SELECT * FROM notes";

        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var reader = command.ExecuteReader();

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
    public Note GetANoteById(int id)
    {

        string query = $"SELECT * FROM notes WHERE note_id = {id}";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var reader = command.ExecuteReader();

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
    public IEnumerable<Note> GetAllNotesForAUserByUerId(int id)
    {

        var notes = new List<Note>();

        string query = $"SELECT * FROM notes WHERE user_id = {id}";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var reader = command.ExecuteReader();

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
    public int CreateNewNote(Note newNote)
    {
        //TODO: update to get user info from JWT (will need a newNoteDTO)
        string query = $"INSERT INTO notes VALUES ( {newNote.note_id}, {newNote.user_id}, '{newNote.title}', '{newNote.content}')";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var result = command.ExecuteNonQuery();
        connector.CloseConnection();
        return result;
    }
    [HttpPut("updateNote")]
    public ActionResult UpdateExistingNote(Note updatedNote)
    {
        string query = $"UPDATE notes SET title = '{updatedNote.title}', content = '{updatedNote.content}' WHERE note_id = {updatedNote.note_id}";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var result = command.ExecuteNonQuery();
        connector.CloseConnection();
        return Ok();
    }
    [HttpDelete("delete/{id}")]
    public int DeleteUser(int id)
    {
        string query = $"DELETE FROM notes WHERE note_id = {id}";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var result = command.ExecuteNonQuery();
        connector.CloseConnection();
        return result;
    }
}