using ITI.ElectroDev.Models;
using ITI.ElectroDev.Presentation;
using ITI.Library.Presentation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AmazonApi.Controllers
{
    [ApiController]
    //[Route("[controller]")]


    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        Context c;
        UserManager<User> UserManager;
        SignInManager<User> SignInManager;
        RoleManager<IdentityRole> RoleManager;
        public UserController(
            UserManager<User> usermanager,
            SignInManager<User> signInManager,
            Context _c,
            RoleManager<IdentityRole> roleManager
            )

        {
            UserManager = usermanager;
            SignInManager = signInManager;
            this.c = _c;
            RoleManager = roleManager;
        }
        [HttpPost]
        public async Task<ResultViewModel> SignUpAsViewer(UserCreateModel model)
        {
            model.Role = "Admin";
            ResultViewModel myModel = new ResultViewModel();
            if (ModelState.IsValid == false)
            {
                myModel.Success = false;
                myModel.Data =
                    ModelState.Values.SelectMany
                            (i => i.Errors.Select(x => x.ErrorMessage));
            }
            else
            {
                User user = new User()
                {
                    UserName = model.UserName,
                    Email = model.Email
                };
                IdentityResult result
                      = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded == false)
                {
                    result.Errors.ToList().ForEach(i =>
                    {
                        ModelState.AddModelError("", i.Description);
                    });
                    myModel.Success = false;
                    myModel.Data =
                        ModelState.Values.SelectMany
                                (i => i.Errors.Select(x => x.ErrorMessage));
                }
                else
                {
                    await UserManager.AddToRoleAsync(user, model.Role);
                    myModel.Success = true;
                    myModel.Message = "successful sign up";
                    myModel.Data = null;
                }
            }
            return myModel;
        }
        [HttpPost]
        public async Task<ResultViewModel> SignIn([FromBody] LoginModel model)
        {
            ResultViewModel myModel = new ResultViewModel();
            if (ModelState.IsValid == false)
            {
                myModel.Success = false;
                myModel.Data =
                    ModelState.Values.SelectMany
                            (i => i.Errors.Select(x => x.ErrorMessage));
            }
            else
            {
                var result
                     = await SignInManager.PasswordSignInAsync
                        (model.UserName, model.Password, model.RememberMe,
                             true);
                var user1 = await UserManager.FindByNameAsync(model.UserName);

                  if (user1 is null || ! await UserManager.CheckPasswordAsync(user1, model.Password))
                {
                    myModel.Success = false;
                    myModel.Message = "Invalid UserName Or Password .";
                }
                else if (result.IsNotAllowed == true)
                {
                    myModel.Success = false;
                    myModel.Message = "Invalid UserName Or Password ";
                }
                else if (result.IsLockedOut)
                {
                    myModel.Success = false;
                    myModel.Message = "Is Locked Out";
                }
               

                else
                {
                    var user = await UserManager.FindByNameAsync(model.UserName);
                    List<Claim> claims = new List<Claim>();
                    var roles = await UserManager.GetRolesAsync(user);
                    roles.ToList().ForEach(i =>
                    {
                        claims.Add(new Claim(ClaimTypes.Role, i));
                    });

                    JwtSecurityToken token
                        = new JwtSecurityToken
                        (
                            signingCredentials:
                             new SigningCredentials
                             (
                                 new SymmetricSecurityKey(Encoding.ASCII.GetBytes("IOLJYHSDSIoleJHsdsdsas98WeWsdsdQweweHgsgdf_&6#2"))
                                 ,
                                 SecurityAlgorithms.HmacSha256
                             ),
                            expires: DateTime.Now.AddDays(5),
                            claims: claims
                        );

                    string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                    myModel.Success = true;
                    myModel.Message = "Successfulyy Loged In";
                    myModel.Data = new
                    {
                        User = user,
                        Toekn = tokenValue,
                        Roles = roles
                    };
                }
            }
            return myModel;
        }


    }
}
