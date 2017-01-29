var sms = (function () {
	return {
		getSmsNotifications: function (treeId) {

			var isHistoryOn = $('#history-datepicker-checker').is(':checked');
			var historyDateEpoch = $('#storedLocalEpoch').val();
			if (historyDateEpoch == '') {
				historyDateEpoch = 0;
			}

			httpAction.get('/SmsTabControl/' + treeId + '/' + isHistoryOn + '/' + historyDateEpoch, function (response) {
				$('#sms-tab-conainer').html(response);

				$('.sms-send-controls').html($('#sms-tab-conainer .sms-tab-buttons'));
			});
		}
	};
})();