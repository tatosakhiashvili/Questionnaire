using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Questionnaire.Infrastructure.Utils {
	public static class Utils {
		public static double? ToEpoch(this DateTime date) {
			return date == default(DateTime) ? (double?)null : date.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
		}

		public static DateTime FromEpoch(this double epoch) {
			return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(epoch);
		}

		public static QuestionnaireTreeViewModel Search(this List<QuestionnaireTreeViewModel> treeModelList, int searchId) {
			foreach(var item in treeModelList) {
				if(item.Id == searchId) {
					return item;
				}
				item.Nodes.Search(searchId);
			}
			return null;
		}

		public static void MakeBoldAncestors(QuestionnaireTreeViewModel treeItem) {
			var parent = treeItem.Parent;
			while(parent != null && parent.Parent != null && parent.FontWeight == "Normal") {
				parent.FontWeight = "Bold";
				parent = parent.Parent;
			}
		}

		public static void IterateForMakeAncestorBold(QuestionnaireTreeViewModel treeItem) {

			if(treeItem.FontWeight == "Bold") {
				MakeBoldAncestors(treeItem);
			}

			if(treeItem != null && treeItem.Nodes != null) {
				foreach(var item in treeItem.Nodes) {
					IterateForMakeAncestorBold(item);
				}
			}
		}

		public static List<QuestionnaireTreeViewModel> MakeParentsBold(this List<QuestionnaireTreeViewModel> treeModelList) {

			foreach(var item in treeModelList) {
				IterateForMakeAncestorBold(item);
			}

			return treeModelList;
		}

		public static void ExcludeZeroNodes(this List<QuestionnaireTreeViewModel> treeModelList) {
			foreach(var item in treeModelList) {
				if(item.Nodes != null) {
					if(item.Nodes.Count == 0) {
						item.Nodes = null;
					} else {
						item.Nodes.ExcludeZeroNodes();
					}
				}
			}
		}

		public static string StripHTML(this string input) {
			var result = Regex.Replace(input, "<.*?>", String.Empty);
			result = (result ?? "").Replace("\r\n", string.Empty);
			return result;
		}

		public static string ToFormattedRoaming(this string input) {
			return string.IsNullOrEmpty(input) ? "-" : input;
		}
	}

	public class TreeSearcher {
		private List<QuestionnaireTreeViewModel> _treeItems;
		private int _searchId;
		private QuestionnaireTreeViewModel _searchResult;

		public TreeSearcher(List<QuestionnaireTreeViewModel> treeItems, int searchId) {
			_treeItems = treeItems;
			_searchId = searchId;
		}

		public QuestionnaireTreeViewModel SearchResult {
			get {
				this.DoubleSearch(_treeItems, _searchId);
				return _searchResult;
			}
		}

		private void Search(QuestionnaireTreeViewModel tree, int searchId) {
			if(tree.Id == searchId) {
				_searchResult = tree; return;
			}

			var nodes = tree.Nodes; // QuestionnaireTreeViewModel.GetNodes(tree);
			if(nodes != null) {
				foreach(QuestionnaireTreeViewModel item in nodes) {
					if(item.Id == searchId) {
						_searchResult = item; return;
					}
					this.Search(item, searchId);
				}
			}
		}

		private void DoubleSearch(List<QuestionnaireTreeViewModel> treeItems, int searchId) {
			foreach(var item in treeItems) {
				Search(item, searchId);
			}
		}
	}

	public class TreeExpander {
		public List<QuesionnaireExpandedViewModel> _expandResult;

		public TreeExpander() {
			_expandResult = new List<QuesionnaireExpandedViewModel> { };
		}

		public List<QuesionnaireExpandedViewModel> ExpandedResult {
			get {
				return _expandResult;
			}
		}

		public void Expand(List<QuestionnaireTreeViewModel> treeItems) {
			foreach(var item in treeItems) {
				_expandResult.Add(new QuesionnaireExpandedViewModel { Id = _expandResult.Count(), NodeId = item.Id });
				if(item.Nodes != null) {
					Expand(item.Nodes);
				}
			}
		}
	}
}
