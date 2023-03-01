using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Data.DTOs;
using API.Data.Responses;
using API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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
    private async Task<UserExisitsResponse> UserExists(string email, string username)
    {
        UserExisitsResponse response = new UserExisitsResponse
        {
            userAlreadyExists = false
        };

        string query = $"SELECT email, username FROM users WHERE email = \"{email}\" OR username = \"{username}\";";

        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {

            if (reader[0].ToString() == email)
            {
                response.userAlreadyExists = true;
                response.message = "email already in use";
            }
            else if (reader[1].ToString() == username)
            {
                response.userAlreadyExists = true;
                response.message = "username already in use";
            }
        }
        connector.CloseConnection();
        return response;
    }

    //TODO::: add error handling to all!
    [HttpGet]
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var users = new List<User>();

        string query = "SELECT * FROM users";

        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var reader = await command.ExecuteReaderAsync();

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
    public async Task<User> GetAUserByIdAsync(int id)
    {

        string query = $"SELECT * FROM users WHERE Id = {id}";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var reader = await command.ExecuteReaderAsync();

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
    public async Task<User> GetAUserByUsernameAsync(string username)
    {

        string query = $"SELECT * FROM users WHERE username = '{username}'";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var reader = await command.ExecuteReaderAsync();
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
    public async Task<ActionResult> CreateNewUserAsync(RegisterDTO registerDto)
    {

        UserExisitsResponse check = await UserExists(registerDto.email, registerDto.username);
        if (check.userAlreadyExists)
        {
            return BadRequest(check.message);
        }


        var hmac = new HMACSHA512();
        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password));
        var passwordSalt = hmac.Key;

        string query = $"INSERT INTO users (email, username, passwordSalt, passwordHash) VALUES ( '{registerDto.email.ToLower()}', '{registerDto.username.ToLower()}', '{Convert.ToBase64String(passwordSalt)}', '{Convert.ToBase64String(passwordHash)}')";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var result = await command.ExecuteNonQueryAsync();
        connector.CloseConnection();
        return Ok();
    }
    //TODO: create several update methods for updating things like username, email, and password
    //delete a user
    [HttpDelete("delete")]
    public async Task<ActionResult> DeleteUserAsync(DeleteDTO AccountToDelete)
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
        var result = await command.ExecuteNonQueryAsync();
        connector.CloseConnection();
        return Ok();
    }
    //TODO: add login functionality that verifies a user's credentials and gives a JWT

}
