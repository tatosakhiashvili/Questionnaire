using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Infrastructure.Utils {
	public static class DifferenceFinder {
		public static SideBySideDiffModel GetDifferences(string oldText, string newText) {
			ISideBySideDiffBuilder _diffBuilder = new SideBySideDiffBuilder(new Differ());
			var result = _diffBuilder.BuildDiffModel(oldText, newText);
			return result;
		}
	}
}
