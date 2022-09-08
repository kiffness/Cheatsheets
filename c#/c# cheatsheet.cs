C# CheatSheet


asp.net core register and login
------------------------------

To Salt and Hash a password
---------------------------
If we are going to salt and Hash a password we need a User Object first
//Create your user object, below is a simple user object consisting of a username, passwordHash and passwordSalt
public class User 
{
	public int Id { get; set; }
	public string Username { get; set; }
	public byte[] passwordHash { get; set; }
	public byte[] passwordSalt { get; set; }
} 

Next we need the Api endpoint or if not APi the database create user function
using the below will allow us to pass the paramaters as a json body
public async Task<ActionResult<User>> Register(RegisterDto registerDto) //The RegisterDto can be a simple object with a username and password
Use a package called System.Security.Cryptography

When registering use like so, this can be a api endpoint 

using var hmac = HMACSHA512(); //This initializes the hmac class

