using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Infrastructure {
	public class TreeSearcher {
		private QuestionnaireTreeViewModel _tree;
		private int _searchId;
		private QuestionnaireTreeViewModel _searchResult;
		public TreeSearcher(QuestionnaireTreeViewModel tree, int searchId) {
			_tree = tree;
			_searchId = searchId;
		}

		public QuestionnaireTreeViewModel SearchResult {
			get {
				this.Search(_tree, _searchId);
				return _searchResult;
				//this.Search(_tree, _searchId).ContinueWith(x =>
				//{
				//    return _searchResult;
				//}); 
			}
		}

		private void Search(QuestionnaireTreeViewModel tree, int searchId) {
			if(tree.Id == searchId) {
				_searchResult = tree; return;
			}

			var nodes = QuestionnaireTreeViewModel.GetNodes(tree);
			if(nodes != null) {
				foreach(QuestionnaireTreeViewModel item in nodes) {
					if(item.Id == searchId) {
						_searchResult = item; return;
					}
					this.Search(item, searchId);
				}
			}
		}
	}
}
