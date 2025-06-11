using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stuff;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _jwtTokenService;

    public UserController(IUserService userService, IJwtTokenService jwtTokenService)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto registerDto)
    {
        try
        {
            var user = await _userService.RegisterUserAsync(registerDto);


            var userEntity = await _userService.GetUserByIdAsync(user.Id);
            var token = _jwtTokenService.GenerateToken(userEntity);

            return Ok(new
            {
                user,
                token
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
    {
        var user = await _userService.AuthenticateUserAsync(loginDto);

        if (user == null)
            return Unauthorized("Invalid credentials");

        var userEntity = await _userService.GetUserByIdAsync(user.Id);
        var token = _jwtTokenService.GenerateToken(userEntity);
        return Ok(new
        {
            token,
            user
        });
    }


    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(Mapper.MapToDto(user));
    }
}
