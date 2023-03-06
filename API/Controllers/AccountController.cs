using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Data.DTOs;
using API.Data.Responses;
using API.Interfaces;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]

public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly string? connectionString;
    private readonly ITokenService _tokenService;

    public AccountController(ILogger<AccountController> logger, IConfiguration config, ITokenService tokenService)
    {
        _logger = logger;
        connectionString = config.GetValue<string>("noteTaker:ConnectionString");
        _tokenService = tokenService;
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

    [HttpGet]
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var users = new List<User>();

        string query = "SELECT * FROM users";

        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var reader = await command.ExecuteReaderAsync();
        try
        {

            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = (int)reader[0],
                    email = reader[1].ToString(),
                    username = reader[2].ToString(),
                    passwordHash = reader[3].ToString(),
                    passwordSalt = reader[4].ToString(),
                });
            }
            connector.CloseConnection();
            return users;
        }
        catch (Exception e)
        {
            System.Console.WriteLine("Bad GetAllUsersAsync request. Error:");
            System.Console.WriteLine($"EXCEPTION: {e}");
            // return BadRequest(e);
            return null;
        }

    }
    //get a user by id
    [HttpGet("id/{id}")]
    public async Task<User> GetAUserByIdAsync(int id)
    {

        string query = $"SELECT * FROM users WHERE Id = {id}";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var reader = await command.ExecuteReaderAsync();
        try
        {
            reader.Read();

            var user = new User
            {
                Id = (int)reader[0],
                email = reader[1].ToString(),
                username = reader[2].ToString(),
                passwordHash = reader[3].ToString(),
                passwordSalt = reader[4].ToString(),
            };
            connector.CloseConnection();
            return user;

        }
        catch (Exception e)
        {
            System.Console.WriteLine("Bad GetAUserByIdAsync request. Error:");
            System.Console.WriteLine($"EXCEPTION: {e}");
            // return BadRequest(e);
            return null;
        }
    }
    //get a user by username
    [HttpGet("username/{username}")]
    public async Task<User> GetAUserByUsernameAsync(string username)
    {

        string query = $"SELECT * FROM users WHERE username = '{username}'";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var reader = await command.ExecuteReaderAsync();
        try
        {
            reader.Read();
            var user = new User
            {
                Id = (int)reader[0],
                email = reader[1].ToString(),
                username = reader[2].ToString(),
                passwordHash = reader[3].ToString(),
                passwordSalt = reader[4].ToString(),
            };
            connector.CloseConnection();
            return user;
        }
        catch (Exception e)
        {
            System.Console.WriteLine("Bad GetAUserByUsernameAsync request. Error:");
            System.Console.WriteLine($"EXCEPTION: {e}");
            // return BadRequest(e);
            return null;
        }
    }
    //create a new user
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> CreateNewUserAsync(RegisterDTO registerDto)
    {

        UserExisitsResponse check = await UserExists(registerDto.email, registerDto.username);
        if (check.userAlreadyExists)
        {
            return BadRequest(check.message);
        }


        var hmac = new HMACSHA512();
        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password));
        var passwordSalt = hmac.Key;

        var user = new User
        {
            email = registerDto.email.ToLower(),
            username = registerDto.username.ToLower(),
            passwordHash = Convert.ToBase64String(passwordHash),
            passwordSalt = Convert.ToBase64String(passwordSalt)
        };

        string query = $"INSERT INTO users (email, username, passwordHash, passwordSalt) VALUES ( '{user.email}', '{user.username}', '{user.passwordHash}', '{user.passwordSalt}')";
        var connector = new DatabaseConnector(connectionString);
        var command = connector.CreateConnectedCommand(query);
        var result = await command.ExecuteNonQueryAsync();
        connector.CloseConnection();
        return new UserDTO
        {
            Username = registerDto.username,
            Token = _tokenService.CreateToken(user)
        };
    }
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> LoginAsync(LoginDTO loginDto)
    {
        User user = await GetAUserByUsernameAsync(loginDto.username);

        if (user == null)
        {
            return Unauthorized("No user exists with this username!");
            System.Console.WriteLine("USER NULL");
        }
        using var hmac = new HMACSHA512(Convert.FromBase64String(user.passwordSalt));
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != Convert.FromBase64String(user.passwordHash)[i]) return Unauthorized("Invalid password!");
        }
        return new UserDTO
        {
            Username = user.username,
            Token = _tokenService.CreateToken(user)
        };

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

}
