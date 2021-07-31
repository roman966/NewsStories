using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newses.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Newses.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        public AccountController(UserManager<IdentityUser> userManager,
                                      SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;

        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register( RegisterViewModel model)
        {   
            if(ModelState.IsValid)
            {
                if (usernameexist(model.UserName) == false)
                {
                    using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection2")))
                    {
                        sqlConnection.Open();
                        SqlCommand SqlCmd = new SqlCommand("Add", sqlConnection);
                        SqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlCmd.Parameters.AddWithValue("FullName", model.FullName);
                        SqlCmd.Parameters.AddWithValue("UserName", model.UserName);
                        SqlCmd.Parameters.AddWithValue("Password", model.Password);
                        SqlCmd.ExecuteNonQuery();
                    }
                    var user = new IdentityUser
                    {
                        UserName = model.UserName,
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {

                        await _signInManager.SignInAsync(user, isPersistent: false);

                        return RedirectToAction("AddorEdit", "Story");
                    }


                }
                else
                {
                    ModelState.AddModelError("", "UserName is taken");
                }

            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
              if(ModelState.IsValid)
              {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection2")))
                {
                    DataTable dataTable = new DataTable();
                    sqlConnection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("Loginfo", sqlConnection);
                    sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("UserName", user.UserName);
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("Password", user.Password);

                    sqlDataAdapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 0)
                    {
                        
                        ModelState.AddModelError("", "UserName or Password not matched");


                    }
                    else
                    {
                        var person = new IdentityUser
                         {
                             UserName = user.UserName
                         };

                         await _signInManager.SignInAsync(person, isPersistent: false);

                        return RedirectToAction("AddorEdit", "Story");

                    }

                }

            }

                return View();
            
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
        public IActionResult Index()
        {
            return View();
        }

        public bool usernameexist(string username)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection2")))
            {
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("UserByUserName", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("UserName", username);
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }
    }
}
