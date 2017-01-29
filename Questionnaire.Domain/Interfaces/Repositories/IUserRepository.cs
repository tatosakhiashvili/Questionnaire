using Questionnaire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        User Fetch(string username, string password);
    }
}
