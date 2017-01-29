using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Services
{
    public interface ISessionService
    {
        bool Login(string username, string password);
        void LogOut();
    }
}
