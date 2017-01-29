using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
    }
}
