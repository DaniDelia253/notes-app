using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly string? connectionString;

    public AccountController(ILogger<AccountController> logger, IConfiguration config)
    {
        _logger = logger;
        connectionString = config.GetValue<string>("noteTaker:ConnectionString");
    }
    //TODO::: add error handling to all!
    [HttpGet]
    public IEnumerable<User> GetAllUsers()
    {
        var users = new List<User>();

        string query = "SELECT * FROM users";

        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            users.Add(new User
            {
                Id = (int)reader[0],
                email = reader[1].ToString(),
                username = reader[2].ToString(),
                passwordSalt = reader[3].ToString(),
                passwordHash = reader[4].ToString(),
            });
        }
        connector.CloseConnection();
        return users;
    }
    //get a user by id
    [HttpGet("id/{id}")]
    public User GetAUserById(int id)
    {

        string query = $"SELECT * FROM users WHERE Id = {id}";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var reader = command.ExecuteReader();

        reader.Read();

        var user = new User
        {
            Id = (int)reader[0],
            email = reader[1].ToString(),
            username = reader[2].ToString(),
            passwordSalt = reader[3].ToString(),
            passwordHash = reader[4].ToString(),
        };
        connector.CloseConnection();
        return user;
    }
    //get a user by username
    [HttpGet("username/{username}")]
    public User GetAUserByUsername(string username)
    {

        string query = $"SELECT * FROM users WHERE username = '{username}'";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var reader = command.ExecuteReader();
        reader.Read();

        var user = new User
        {
            Id = (int)reader[0],
            email = reader[1].ToString(),
            username = reader[2].ToString(),
            passwordSalt = reader[3].ToString(),
            passwordHash = reader[4].ToString(),
        };
        connector.CloseConnection();
        return user;
    }
    //create a new user
    [HttpPost("register")]
    public int CreateNewUser(User newUser)
    {
        //TODO: add logic to programatically create the password salt and hash based on a password that is given (will need a register DTO)
        string query = $"INSERT INTO users VALUES ( {newUser.Id}, '{newUser.email}', '{newUser.username}', '{newUser.passwordSalt}', '{newUser.passwordHash}')";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var result = command.ExecuteNonQuery();
        connector.CloseConnection();
        return result;
    }
    //TODO: create several update methods for updating things like username, email, and password
    //delete a user
    [HttpDelete("delete")]
    public ActionResult DeleteUser(DeleteDTO AccountToDelete)
    {
        string query;
        if (AccountToDelete.Id != null)
        { query = $"DELETE FROM users WHERE Id = {AccountToDelete.Id}"; }
        else if (AccountToDelete.email != null)
        { query = $"DELETE FROM users WHERE email = '{AccountToDelete.email}'"; }
        else if (AccountToDelete.username != null)
        { query = $"DELETE FROM users WHERE username = '{AccountToDelete.username}'"; }
        else
        { return NotFound(); }
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var result = command.ExecuteNonQuery();
        connector.CloseConnection();
        return Ok();
    }
    //TODO: add login functionality that verifies a user's credentials and gives a JWT

}
