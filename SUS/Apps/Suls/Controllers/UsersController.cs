using Suls.Services;
using Suls.ViewModels.Users;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Suls.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public HttpResponse Login() 
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
            var userId = this.usersService.GetUserId(username, password);

            if (userId == null)
            {
                return this.Error("Invalid username or pass!!");
            }

            this.SignIn(userId);
            return this.Redirect("/");
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterInputModel input) 
        {
            if (string.IsNullOrEmpty(input.Username) || input.Username.Length < 5 || input.Username.Length > 20)
            {
                return this.Error("Username zw 5 and 20 ch!!");
            }
            if (!this.usersService.IsUsernameAvailable(input.Username))
            {
                return this.Error("userName taken");
            }
            if (string.IsNullOrEmpty(input.Email) || !new EmailAddressAttribute().IsValid(input.Email))
            {
                return this.Error("Invalid email!!!");
            }
            if (!this.usersService.IsEmailAvailable(input.Email))
            {
                return this.Error("e-mail taken");
            }
            if (string.IsNullOrEmpty(input.Password) || input.Password.Length < 5 || input.Password.Length > 20)
            {
                return this.Error("Incorect password!!");
            }
            if (input.Password != input.ConfirmPassword)
            {
                return this.Error("Diff password!!");
            }

            this.usersService.CreateUser(input.Username, input.Email, input.Password);

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout() 
        {
            this.SignOut();

            return this.Redirect("/");
        }
    }
}
