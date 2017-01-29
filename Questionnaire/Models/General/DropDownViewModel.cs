using Questionnaire.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class DropDownViewModel {
		public object Id { get; set; }
		public string Name { get; set; }

		public static List<DropDownViewModel> GetList<T>() {
			var enums = Enum.GetValues(typeof(T)).Cast<T>();
			var result = new List<DropDownViewModel> { };
			foreach(var item in enums) { result.Add(new DropDownViewModel { Id = item, Name = item.ToString() }); }
			return result;
		}

		public static List<DropDownViewModel> GetListWithIntValues<TreeStatusEnum>() {
			var enums = Enum.GetValues(typeof(TreeStatusEnum)).Cast<TreeStatusEnum>();
			var result = new List<DropDownViewModel> { };
			foreach(var item in enums) { result.Add(new DropDownViewModel { Id = Convert.ToInt32(item), Name = item.ToString() }); }
			return result;
		}

		public static List<DropDownViewModel> GetList(List<Owner> owners) {
			var result = new List<DropDownViewModel> { };
			foreach(var owner in owners) {
				result.Add(new DropDownViewModel {
					Id = owner.Id,
					Name = owner.Name
				});
			}
			return result;
		}

		public static implicit operator DropDownViewModel(InternalMessageType internalMessageType) {
			return new DropDownViewModel {
				Id = internalMessageType.Id,
				Name = internalMessageType.Name
			};
		}

		public static implicit operator DropDownViewModel(InternalMessagePriority internalMessagePriority) {
			return new DropDownViewModel {
				Id = internalMessagePriority.Id,
				Name = internalMessagePriority.Name
			};
		}

		public static implicit operator DropDownViewModel(TreeStatus status) {
			return new DropDownViewModel {
				Id = status.Id,
				Name = status.Name
			};
		}
	}

	public class DropDownCheckboxViewModel {
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsChecked { get; set; }

		public static implicit operator DropDownCheckboxViewModel(GroupEmail groupEmail) {
			return new DropDownCheckboxViewModel {
				Id = groupEmail.Id,
				Name = groupEmail.Email
			};
		}
	}
}
