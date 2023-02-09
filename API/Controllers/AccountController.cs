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

    [HttpGet]
    public IEnumerable<User> GetAllUsers()
    {
        var users = new List<User>();

        string query = "SELECT * FROM users";

        var connector = new DatabaseConnector(connectionString);
        var reader = connector.CreateDatabaseReader(query);

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
    // [HttpGet("{id}")]
    // public User GetAUserById(int id)
    // {

    //     string query = $"SELECT * FROM users WHERE Id = {id}";
    //     var connector = new DatabaseConnector(connectionString);
    //     var reader = connector.CreateDatabaseReader(query);

    //     return new User
    //     {
    //         Id = (int)reader[0],
    //         email = reader[1].ToString(),
    //         username = reader[1].ToString(),
    //         passwordSalt = reader[1].ToString(),
    //         passwordHash = reader[1].ToString(),
    //     };
    // }
    //get a user by username
    //create a new user
    //TODO: add logic to programatically create the password salt and hash based on a password that is given (will need a register DTO)
    //update an existing user
    //delete a user
    //TODO: add login functionality that verifys a user's credentials and gives a JWT
}
