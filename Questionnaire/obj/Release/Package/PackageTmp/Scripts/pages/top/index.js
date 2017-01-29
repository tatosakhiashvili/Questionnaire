var topS = (function () {
	return {
		getTopItems: function (callBack) {
			httpAction.get('/TopItemsControl/', function (response) {
				var historyCheckboxIsChecked = $('#history-datepicker-checker').is(':checked');
				var treeSearchCheckboxIsChecked = $('#tree-search-checker').is(':checked');
				 
				response += "<div id='top-tab-container-fader' class='fader" + ((historyCheckboxIsChecked || treeSearchCheckboxIsChecked ? ' active' : '')) + "'></div>"; //Add fade control
				$('#top-tab-conainer').html(response);
				if (callBack != undefined) {
					callBack();
				}
			});
		}
	};
})();