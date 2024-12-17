using JobExpressBack.Models.DTOs;
using JobExpressBack.Models.Repositories.RepoServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JobExpressBack.Models.Repositories.Authentification
{
    ////Ce service contient toute la logique métier pour l'authentification 
    ///et la generation des tokens.
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly PhotoService photoService;
        private readonly IServiceRepository serviceRepository;

        public AuthRepository(UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IConfiguration configuration, 
            PhotoService photoService ,
            IServiceRepository serviceRepository)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.photoService = photoService;
            this.serviceRepository = serviceRepository;
        }

        public async Task<string> Register(RegisterModel model)
        {
            // Vérifiez si l'email existe déjà
            if (await userManager.FindByEmailAsync(model.Email) != null)
            {
                return "Email existe déja!";
            }

            // Traitez l'image avec Cloudinary
            string photoUrl = null;
            if (model.PhotoProfile != null)
            {
                photoUrl = await photoService.UploadPhotoAsync(model.PhotoProfile);
            }

            // Créez un nouvel utilisateur
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                Telephone = model.Telephone,
                PhotoProfile = photoUrl
            };

            var result = await userManager.CreateAsync(user, model.Password);

            // Vérifiez si la création a échoué
            if (!result.Succeeded)
            {
                return string.Join(", ", result.Errors.Select(e => e.Description));
            }
            var role = string.IsNullOrEmpty(model.Role) ? "Client" : model.Role;
            // Ajoutez un rôle par défaut "Client" si nécessaire
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
            await userManager.AddToRoleAsync(user, role);

            // Si le rôle est 'Professionnel', vous pouvez associer un ServiceID
            if (role.Equals("Professionnel", StringComparison.OrdinalIgnoreCase))
            {
                if (model.ServiceID == null)
                {
                    return "Un professionnel doit avoir un service associé.";
                }

                // Assurez-vous que le service existe dans la base de données
                var service = await serviceRepository.GetById(model.ServiceID.Value);
                if (service == null)
                {
                    return "Le service spécifié n'existe pas.";
                }

                user.ServiceID = model.ServiceID;
                await userManager.UpdateAsync(user);
            }


            return "Registration réussie!";
        }

        public async Task<AuthModel> Login(LoginModel model)
        {
            var authModel = new AuthModel();
            // Vérifier si l'utilisateur existe
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "email or password invalide !";
                authModel.IsAuthenticated = false;
                return authModel;
            }

            // Générer un token JWT
            var token = await GenerateJwtToken(user);
            authModel.Message = "Login réussie!";
            authModel.IsAuthenticated = true;
            authModel.Username = user.UserName;
            authModel.Email = user.Email;
            authModel.Token = token;
            authModel.ExpiresOn = DateTime.UtcNow.AddDays(7);

            // Ajouter le rôle de l'utilisateur
            var roles = await userManager.GetRolesAsync(user);
            authModel.Role = roles.FirstOrDefault();  // Récupère le premier rôle assigné à l'utilisateur

            //update les activités utilisateur
            user.LastLoginDate = DateTime.UtcNow;
            await userManager.UpdateAsync(user);

            return authModel;
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = configuration.GetSection("JWT");
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id)
        };

            // Ajouter les rôles de l'utilisateur
            var roles = await userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim("roles", role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
