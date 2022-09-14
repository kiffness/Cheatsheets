C# CheatSheet


asp.net core register and login
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

To Salt and Hash a password
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//Use a package called System.Security.Cryptography
//If we are going to salt and Hash a password we need a User Model first
//Create your user object, below is a simple user object consisting of a username, passwordHash and passwordSalt
public class User 
{
	public int Id { get; set; }
	public string Username { get; set; }
	public byte[] passwordHash { get; set; }
	public byte[] passwordSalt { get; set; }
}

//*************************************************************************************//
//**  Next we need the Api endpoint or if not APi the database create user function  **//
//**  using the below will allow us to pass the paramaters as a json body            **// 
//**  When registering use like so, this can be a api endpoint                       **//
//*************************************************************************************//
public async Task<ActionResult<User>> Register(RegisterDto registerDto) //The RegisterDto can be a simple object with a username and password
{	
	//First we want to check if the username exists already
	if(await UserExists(registerDto.Username.ToLower()) return BadRequest("Username is taken");
	
	
	using var hmac = HMACSHA512(); //This initializes the hmac class
	
	//Create our new User
	var user = new User 
	{
		Username = registerDto.Username.ToLower(),
		//Hash the password
		PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
		//Save the salt key
		PasswordSalt = hmac.key
	};
	
	//Save the user in the database
	_context.Users.Add(user); //assuming the table name is Users
	await _context.SaveChangesAsync();
	
	//Return the user 
	return user;
}

//*************************************************************************************//
//**  Now if we need to reverse the operation to login for example                   **//
//*************************************************************************************//

public async Task<ActionResult<User>> Login(LoginDto loginDto) //The login dto is the exact same as the register DTO it is just to prevent confusion
{
	//We want to get the user from the DB
	var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == loginDto.Username.ToLower()); //The x is the user object on the database
	
	//Check if we got a user object back
	if (user == null) return Unauthorized("Invalid username or password");
	
	//Initialize our hmac class again, except this time with the users passwordSalt
	using var hmac = HMACSHA512(user.PasswordSalt);
	
	//use hmac to generate a new hash using the password salt
    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

    //Check each character of the new computed hash matches the saved password hash
    for (int i = 0; i < computedHash.Length; i++)
    {
		if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid username or password");
    }

    //Return our user object
    return user
}

Creating a JWT token for user login
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//*************************************************************************************//
//**  Below is how to add in JWT tokens to an api or web project for authentication  **//
//**  This is a good resource https://jwt.io/introduction                            **//
//*************************************************************************************//

//First we can create an ITokenService Interface so that the create token method is implemented
public interface ITokenService
{
		string CreateToken(User user); //User is just our user class, min needs username and password
}

//Then we implement the interface and create our token using a Service 
using Microsoft.IdentityModel.Tokens
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

public class TokenService : ITokenService 
{
		//Create a signed key first
		private readonly SymmetricSecurityKey _key;
		
		public TokenService(IConfiguration config)
		{
			//Create a new key
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
		}
		
		//Implement our createtoken method
		public string CreateToken(User user)
		{
			//Our Claims
            var claims = new List<Claim>
            {   
                //use the NameId to store the username this will be our name identifier for everything
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            //Create some creds so we save our creds = the new signing credentials
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            //Describe our token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //claims
                Subject = new ClaimsIdentity(claims),
                //Expires
                Expires = DateTime.Now.AddDays(7),
                //Signing Creds
                SigningCredentials = creds
            };

            //Create an instance of tokenhandler
            var tokenHandler = new JwtSecurityTokenHandler();
            //Create a token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //return the created token to string
            return tokenHandler.WriteToken(token);
		}
}

//*************************************************************************************//
//**  Next we need to inject our service into our app								 **//
//*************************************************************************************//

//Add this line to your program.cs for core in the services section
builder.Services.AddScoped<ITokenService, TokenService>(); // We use scoped as this will create the service and it will be destroyed at the end of the http Request

//*************************************************************************************//
//**  Then we need to add our super secret key to the devolopment settings			 **//
//*************************************************************************************//

//In appsettings.Devolopment.json
"TokenKey": "super secret unguessable key", // Use another string for production

Adding the authentication middleware
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//Install the nuget package Microsoft.AspNetCore.Authentication.JwtBearer

//In our program.cs again add below to our services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

//Then add the below after cors and before authorization
app.UseAuthentication();

//Dont forget to decorate your api endpoints with
[Authorize] //if you want to only allow authorized users
[AllowAnonymous] //if you want to allow anyone

