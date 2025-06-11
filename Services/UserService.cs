using Microsoft.AspNetCore.Identity;
using AutoMapper;
using stuff;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = new PasswordHasher<User>();
        _mapper = mapper;
    }

    public async Task<UserResponseDto> RegisterUserAsync(UserRegisterDto registerDto)
    {
        if (await _userRepository.UserExistsAsync(registerDto.Username))
            throw new Exception("Username already taken");

        var user = _mapper.Map<User>(registerDto);

        user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);

        var createdUser = await _userRepository.AddUserAsync(user);
        return Mapper.MapToDto(user);
    }

    public async Task<UserResponseDto> AuthenticateUserAsync(UserLoginDto loginDto)
    {
        var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
        if (user == null)
            return null;

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

        if (result == PasswordVerificationResult.Failed)
            return null;

        return Mapper.MapToDto(user);
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        return user == null ? null : _mapper.Map<User>(user);
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await _userRepository.UserExistsAsync(username);
    }


}
