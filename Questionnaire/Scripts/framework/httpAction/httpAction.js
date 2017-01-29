var httpAction = (function () {

	var traditional = false;

	function ajaxCall(type, url, data, successCallBack, errorCallBack) {

		if (traditional) {
			$.ajax({
				type: type,
				url: url,
				data: data,
				processData: false,

				datatype: "json",
				contentType: "application/json; charset=utf-8",
				traditional: traditional,

				beforeSend: function () {
					questionnaire.hideErrors();
					questionnaire.startLoader();
				},
				success: function (data) {
					traditional = false; //TODO: this is available only per request
					questionnaire.stopLoader();
					if (typeof successCallBack != undefined) {
						successCallBack(data);
					}
				},
				error: function (e) {
					questionnaire.stopLoader();
					if (typeof errorCallBack !== 'undefined') {
						errorCallBack(e);
					}
				}
			});
		} else {
			$.ajax({
				type: type,
				url: url,
				data: data,
				processData: false,
				beforeSend: function () {
					questionnaire.hideErrors();
					questionnaire.startLoader();
				},
				success: function (data) {
					traditional = false; //TODO: this is available only per request
					questionnaire.stopLoader();
					if (typeof successCallBack != undefined) {
						successCallBack(data);
					}
				},
				error: function (e) {
					questionnaire.stopLoader();
					if (typeof errorCallBack !== 'undefined') {
						errorCallBack(e);
					}
				}
			});
		} 
	}

	return {

		init: function (isTraditional) {
			traditional = isTraditional;
		},

		get: function (url, successCallBack, errorCallBack) {
			ajaxCall('GET', url, null, successCallBack, errorCallBack);
		},

		post: function (url, data, successCallBack, errorCallBack) {
			ajaxCall('POST', url, data, successCallBack);
		},

		put: function (url, data, successCallBack, errorCallBack) {
			ajaxCall('PUT', url, data, successCallBack, errorCallBack);
		},

		delete: function (url, successCallBack, errorCallBack) {
			ajaxCall('DELETE', url, null, successCallBack, successCallBack);
		}
	};
})();