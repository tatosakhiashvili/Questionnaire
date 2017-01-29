var Validation = (function () {

	return {

		validateForm: function (form) {
			var invalidFieldCount = 0;
			
			form.find("[data-required=true]").each(function () {
				var field = $(this);
				if (field.val() == '' || field.val() == '-1') {
					alert('xxxxxxx');
				}
			});
		},

		clear: function (form) {

		}
	};
});