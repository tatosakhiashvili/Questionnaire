var uploadControl = (function () {

	$('body').on('change', '.file-uploader-ctrl', function () {
		var action = '';
		var file = this.files[0];

		var table = $('#email-answer-files-table');

		uploadControl.upload('/Upload/Upload', file, function (response) {

			var indexer = $(table).find('tr').length - 1;

			var row = "<tr><td>";
			row += "<input class='reindex-item' type='hidden' data-start-name='Files' data-end-name='Name' value='" + response.data.fileName + "'>";
			row += "<input class='reindex-item' type='hidden' data-start-name='Files' data-end-name='Id' value='" + 0 + "'>";
			row += "<input class='reindex-item' type='hidden' data-start-name='Files' data-end-name='FolderName' value='" + response.data.folderName + "'>";
			row += "<input class='reindex-item' type='hidden' data-start-name='Files' data-end-name='IsTemporary' value='True'>";
			row += "<a href='#' data-node-id='0' data-folder-name='" + response.data.folderName + "' data-file-name='" + response.data.fileName + "' data-is-temporary='1' onclick='uploadControl.initializeDownload(this)'>" + response.data.fileName + "</a></td></tr>";

			$(table).find('tbody').prepend(row);

			initializeTableRows(table);

			$('.file-uploader-ctrl').val('');
		});
	});

	$('body').on('change', '.tree-file-uploader', function () {
		var file = this.files[0];
		var $uploadControl = $(this);
		var table = $('#tree-files-table');

		uploadControl.upload('/Upload/UploadTreeFile', file, function (response) {
			if (response.success) {

				var row =
				"<tr>" +
					"<td>" +
						"<input class='reindex-item Id' data-end-name='Id' data-start-name='Files' id='Files_0__Id' name='Files[0].Id' type='hidden' value='0'>" +
						"<input class='reindex-item FileName' data-end-name='FileName' data-start-name='Files' id='Files_0__FileName' name='Files[0].FileName' type='hidden' value='1'>" +
						"<input class='reindex-item FolderName' data-end-name='FolderName' data-start-name='Files' id='Files_0__FolderName' name='Files[0].FolderName' type='hidden' value='2'>" +
						"<input class='reindex-item IsTemporary' data-end-name='IsTemporary' data-start-name='Files' data-val='true' data-val-required='The IsTemporary field is required.' id='Files_0__IsTemporary' name='Files[0].IsTemporary' type='hidden' value='True'>" +
						"<a href='#' data-node-id='0' onclick='uploadControl.initializeTreeFileDownload(this)'>1</a>" +
					"</td>" +
				"</tr>";

				row = $(row);


				$(row).find('.FileName').val(response.data.fileName);
				$(row).find('.FolderName').val(response.data.folderName);
				$(row).find('.IsTemporary').val('True');
				$(row).find('a').html(response.data.fileName);
				$(table).find('tbody').prepend(row);
			}

			$uploadControl.val(''); // Clear upload control
			initializeTableRows(table);
		});
	});

	function initializeTableRows(tableName) {
		var indexer = -1;
		$(tableName).find('tbody tr').each(function (e) {
			indexer += 1;

			$(this).find('td .reindex-item').each(function (x) {
				var startName = $(this).data('start-name');
				var endName = $(this).data('end-name');

				$(this).attr('id', startName + '_' + indexer + '__' + endName);
				$(this).attr('name', startName + '[' + indexer + '].' + endName);
			});
		});
	};

	return {
		upload: function (url, file, successCallBack, errorCallBack) {
			var fileData = new FormData();
			fileData.append(file.name, file);

			$.ajax({
				url: url,
				type: 'POST',
				contentType: false,
				processData: false,
				data: fileData,
				success: function (response) {
					if (utils.isDefined(successCallBack)) {
						successCallBack(response);
					}
				},
				error: function (response) {
					if (utils.isDefined(errorCallBack)) {
						errorCallBack();
					}
				}
			});
		},

		initializeDownload: function (obj) {
			var rootId = $(obj).data('node-id');
			var folderName = $(obj).data('folder-name');
			var fileName = $(obj).data('file-name');
			var isTemporary = $(obj).data('is-temporary');

			httpAction.get('/Upload/InitializeDownload/' + rootId + '/' + folderName + '/' + fileName + '/' + (isTemporary == '1'), function (response) {
				if (response.success) {
					window.location.href = '/Upload/Download/' + rootId + '/' + folderName + '/' + fileName + '/' + (isTemporary == '1');
				} else {
					alert('Something went wrong ...');
				}
			});
		},

		initializeTreeFileDownload: function (obj) {
			var rootId = $(obj).data('node-id');
			var folderName = $(obj).parent().children('.FolderName').val();
			var fileName = $(obj).parent().children('.FileName').val();
			var isTemporary = $(obj).parent().children('.IsTemporary').val();

			httpAction.get('/Upload/InitializeTreeFileDownload/' + rootId + '/' + folderName + '/' + fileName + '/' + isTemporary, function (response) {
				if (response.success) {
					window.location.href = '/Upload/DownloadTreeFile/' + rootId + '/' + folderName + '/' + fileName + '/' + isTemporary;
				} else {
					alert('Something went wrong ...');
				}
			});
		},

		initializeTableRows: function (tableName) {
			initializeTableRows(tableName);
		},

		initializeEmailComparisonDownload: function (obj) {
			var rootId = $(obj).data('node-id');
			var folderName = $(obj).data('folder-name');
			var fileName = $(obj).data('file-name');
			var isTemporary = 0;

			httpAction.get('/Upload/InitializeDownload/' + rootId + '/' + folderName + '/' + fileName + '/' + (isTemporary == '1'), function (response) {
				if (response.success) {
					window.location.href = '/Upload/Download/' + rootId + '/' + folderName + '/' + fileName + '/' + (isTemporary == '1');
				} else {
					alert('Something went wrong ...');
				}
			});
		}
	};
})();