using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Utils {
	public static class UtilExtenders {
		public static Func<string, string> MapPath { get; set; }

		public static List<string> GetFilesInDirectory(string path) {
			if(System.IO.Directory.Exists(path)) {
				return System.IO.Directory.GetFiles(path).ToList();
			}
			return new List<string> { };
		}
	}
}
