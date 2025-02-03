using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EventApp
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _Options;
        private readonly IAdminRepository _adminRepository;
        private readonly IParticipantRepository _participantRepository;
        public JwtProvider(IOptions<JwtOptions> options, IAdminRepository adminRepository, IParticipantRepository participantRepository)
        {
            _Options = options.Value;
            _adminRepository = adminRepository;
            _participantRepository = participantRepository;
        }
        private string GenerateAccessToken(User user)
        {
            var signinCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Options.Key)),
                SecurityAlgorithms.HmacSha256);

            Claim[] claims = [new("UserId", user.Id.ToString()), new("Role", user is AdminModel ? "Admin" : "Participant"), new(ClaimTypes.Role, user is AdminModel ? "Admin" : "Participant"), new(ClaimTypes.NameIdentifier, user.Id.ToString())];
            var token = new JwtSecurityToken(
                signingCredentials: signinCredentials,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_Options.ExpiredMinutes));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Options.Key)),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token message!");

            return principal;
        }

        public async Task<TokenDTO> GenerateToken(User user, bool populateExp, CancellationToken cancellationToken)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            if (user is AdminModel)
            {
                if (populateExp) await _adminRepository.UpdateRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddDays(7), cancellationToken);
                else await _adminRepository.UpdateRefreshTokenAsync(user.Id, refreshToken, cancellationToken);
            }
            else
            {
                if (populateExp) await _participantRepository.UpdateRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddDays(7), cancellationToken);
                else await _participantRepository.UpdateRefreshTokenAsync(user.Id, refreshToken, cancellationToken);
            }

            return new TokenDTO(accessToken, refreshToken);
        }

        public async Task<TokenDTO> RefreshToken(TokenDTO tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.accessToken);

            var role = principal.FindFirst(ClaimTypes.Role)?.Value;

            var userId = principal.Claims.FirstOrDefault(c => c.Type.Equals("UserId"))?.Value;

            User user = null;

            if (role != null && userId != null)
            {
                if (role.Equals("Admin"))
                {
                    user = await _adminRepository.GetAdminByIdAsync(Guid.Parse(userId), CancellationToken.None);
                }
                else if (role.Equals("Participant"))
                {
                    user = await _participantRepository.GetAsync(Guid.Parse(userId), CancellationToken.None);
                }
            }

            if (user == null || user.RefreshToken != tokenDto.refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new RefreshTokenBadRequestException();
            }

            return await GenerateToken(user, false, CancellationToken.None);
        }
    }
}
