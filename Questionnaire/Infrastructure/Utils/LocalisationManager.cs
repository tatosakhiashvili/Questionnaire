using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Infrastructure.Utils
{
    public class Localisation
    {
        public static Localisation Key
        {
            get { return _key ?? (_key = new Localisation()); }
        }
        private static Localisation _key;

        public string this[string key]
        {
            get
            {
                //TODO: Needs to be implemented
                return string.Empty;
            }
        }
    }
}