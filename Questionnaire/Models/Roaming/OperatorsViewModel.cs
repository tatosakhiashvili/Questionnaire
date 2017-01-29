using Questionnaire.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models {
	public class OperatorsViewModel {
		public List<OperatorViewModel> Operators { get; set; }
	}

	public class OperatorViewModel {
		public int Id { get; set; }
		public string Name { get; set; }
		public string NetworkCode { get; set; }
		public string NetworkType { get; set; }
		public string Display { get; set; }
		public string CustomerCare { get; set; }
		public string GprsRoamingStatus { get; set; }

		public static implicit operator OperatorViewModel(Operator obj) {
			return new OperatorViewModel {
				Id = obj.Id,
				Name = obj.Name,
				NetworkCode = obj.NetworkCode,
				NetworkType = obj.NetworkType,
				Display = obj.Display,
				CustomerCare = obj.CustomerCare,
				GprsRoamingStatus = obj.GprsRoamingStatus
			};
		}
	}
}