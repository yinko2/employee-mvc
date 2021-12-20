using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using EmployeeMVC.Models;

namespace EmployeeMVC.Util
{
    public class Globalfunction
    {
        public static void WriteSystemLog(string message)
        {
            Console.WriteLine(DateTime.UtcNow.ToString() + " - " + message);
        } 
    }
}