namespace stuff
{

    public interface IUserService
    {
        Task<UserResponseDto> RegisterUserAsync(UserRegisterDto registerDto);
        Task<UserResponseDto> AuthenticateUserAsync(UserLoginDto loginDto);
        Task<User> GetUserByIdAsync(int userId);
        Task<bool> UserExistsAsync(string username);


    }

}