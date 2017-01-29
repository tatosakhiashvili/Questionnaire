using Newtonsoft.Json;
using Questionnaire.Domain;
using Questionnaire.Infrastructure;
using Questionnaire.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models {
	public class QuestionnaireViewModel {
		[JsonProperty(PropertyName = "treeItems")]
		public List<QuestionnaireTreeViewModel> TreeItems { get; set; }

		[JsonProperty(PropertyName = "expandedItems")]
		public List<QuesionnaireExpandedViewModel> ExpandedItems { get; set; }
	}

	public class QuestionnaireTreeViewModel {
		[JsonProperty(PropertyName = "data_node")]
		public int Id { get; set; }

		[JsonProperty(PropertyName = "parentId")]
		public int ParentId { get; set; }

		[JsonProperty(PropertyName = "rootId")]
		public int RootId { get; set; }

		[JsonProperty(PropertyName = "text")]
		public string Text { get; set; }

		[JsonProperty(PropertyName = "comment")]
		public string Comment { get; set; }

		[JsonProperty(PropertyName = "color")]
		public string Color { get; set; }

		[JsonProperty(PropertyName = "backColor")]
		public string BackgroundColor { get; set; }

		[JsonProperty(PropertyName = "fontWeight")]
		public string FontWeight { get; set; }

		[JsonProperty(PropertyName = "tags")]
		public int[] Tags { get; set; }

		[JsonProperty(PropertyName = "nodes")]
		public List<QuestionnaireTreeViewModel> Nodes { get; set; }

		//[JsonProperty(PropertyName = "parentId")]
		[JsonIgnore]
		public QuestionnaireTreeViewModel Parent { get; set; }

		public static List<QuestionnaireTreeViewModel> ParseFrom(List<Tree> treeItems) {
			var resultTreeItems = new List<QuestionnaireTreeViewModel> { };
			var rootItems = treeItems.Where(x => x.ParentId == -1).ToList();
			var childItems = treeItems.Where(x => x.ParentId != -1).ToList();

			resultTreeItems.AddRange(rootItems.Select(x => (QuestionnaireTreeViewModel)x));

			foreach(var tree in childItems) {
				var _tree = new TreeSearcher(resultTreeItems, tree.ParentId).SearchResult;
				if(_tree != null) {
					var t = (QuestionnaireTreeViewModel)tree;
					t.Parent = _tree;
					_tree.Nodes.Add(t);
				}
			}

			resultTreeItems.ExcludeZeroNodes();

			return resultTreeItems;
		}
		
		public static implicit operator QuestionnaireTreeViewModel(Tree tree) {
			return new QuestionnaireTreeViewModel {
				Id = tree.Id,
				ParentId = tree.ParentId,
				Text = tree.Text,
				Color = tree.Color,
				RootId = tree.RootId,
				//BackgroundColor = tree.Color,
				FontWeight = tree.IsBold ? "Bold" : "Normal",
				Tags = new int[] { tree.Id },
				Comment = tree.Comment,
				Nodes = new List<QuestionnaireTreeViewModel> { }
			};
		}

		//public static implicit operator QuestionnaireTreeViewModel(List<Tree> tree)
		//{
		//    var model = new List<QuestionnaireTreeViewModel> { };
		//    model.AddRange(tree.Where(x => x.ParentId == -1).Select(x => (QuestionnaireTreeViewModel)x))





		//    var result = new QuestionnaireTreeViewModel { };

		//    var root = tree.FirstOrDefault();

		//    result = new QuestionnaireTreeViewModel
		//    {
		//        Id = root.Id,
		//        Text = root.Text,
		//        Comment = root.Comment,
		//        Color = root.Color,
		//        FontWeight = root.IsBold ? "Bold" : "Normal",
		//        Tags = new int[] { root.Id }
		//    };

		//    foreach (var treeItem in tree.Skip(1).GroupBy(x => x.ParentId))
		//    {
		//        var node = new TreeSearcher(result, treeItem.Key).SearchResult;
		//        if (node != null)
		//        {
		//            foreach (var leaf in treeItem)
		//            {
		//                if (node.Nodes == null) { node.Nodes = new List<QuestionnaireTreeViewModel> { }; }
		//                node.Nodes.Add(new QuestionnaireTreeViewModel
		//                {
		//                    Id = leaf.Id,
		//                    Text = leaf.Text,
		//                    Comment = leaf.Comment,
		//                    Color = leaf.Color,
		//                    FontWeight = leaf.IsBold ? "Bold" : "Normal",
		//                    Tags = new int[] { leaf.Id },
		//                });
		//            }
		//        }
		//    }
		//    return result;
		//}
	}

	public class QuesionnaireExpandedViewModel {
		public int Id { get; set; }
		public int NodeId { get; set; }

		public static List<QuesionnaireExpandedViewModel> ExpandTree(List<QuestionnaireTreeViewModel> treeControl) {
			var expander = new TreeExpander();
			expander.Expand(treeControl);
			return expander.ExpandedResult;
		}
	}
}