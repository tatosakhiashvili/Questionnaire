var chat = (function () {

	$('body').on('change', '#chatFileUploader', function () {
		var $this = $(this);
		var $table = $('#' + $('#chatFileUploaderTableName').val());
		var file = this.files[0];

		uploadControl.upload('/Chat/UploadFile/', file, function (response) {
			var row = '';
			row +=
			'<tr class="chat-file-downloader" data-message-id="0">' +
				'<td>' +
					'<input class="reindex-item FileName" data-end-name="FileName" data-start-name="Files" id="Files_0__FileName" name="Files[0].FileName" type="hidden" value="' + response.data.fileName + '">' +
					'<input class="reindex-item Id" data-end-name="Id" data-start-name="Files" id="Files_0__Id" name="Files[0].Id" type="hidden" value="' + response.data.id + '">' +
					'<input class="reindex-item FolderName" data-end-name="FolderName" data-start-name="Files" id="Files_0__FolderName" name="Files[0].FolderName" type="hidden" value="' + response.data.folderName + '">' +
					'<input class="reindex-item IsTemporary" data-end-name="IsTemporary" data-start-name="Files" data-val="true" data-val-required="The IsTemporary field is required." id="Files_0__IsTemporary" name="Files[0].IsTemporary" type="hidden" value="True">' +
					'<span>' + response.data.fileName + '</span>' +
				'</td>' +
				'<td>' + response.data.operatorName + '</td>' +
			'</tr>';

			$table.append(row);
			initializeTableRows($table);
		});

		$this.val('');
	});


	function initializeTableRows(tableName) {
		var indexer = -1;

		console.log($(tableName).find('tbody tr').length);

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
		add: function () {
			httpAction.get('/Chat/AddMessage', function (response) {
				questionnaire.showModal(response);
			});
		},

		change: function (id) {
			httpAction.get('/Chat/ChangeMessage/' + id, function (response) {
				questionnaire.showModal(response);
			});
		},

		delete: function (id) {
			httpAction.get('/Chat/DeleteMessage/' + id, function (response) {
				questionnaire.showModal(response);
			});
		},

		execute: function (id) {
			httpAction.get('/Chat/ExecuteMessage/' + id, function (response) {
				questionnaire.showModal(response);
			});
		},

		maintainMessage: function (form, id) {
			if (questionnaire.validation.validateForm(form)) {
				var data = $(form).serialize();
				httpAction.post('/Chat/MaintainMessage', data, function (response) {
					if (response.success) {
						questionnaire.closeModal();
						window.location.reload();
					} else {
						questionnaire.showErrors([{ errorMessage: 'დაფიქსირდა შეცდომა, სცადეთ მოგვიანებით' }]);
					}
				});
			};
		},

		removeFiles: function (messageId, tableId) {
			var activeRow = $('#' + tableId + ' tbody tr.active');
			if (activeRow.length > 0) {

				var folderName = $(activeRow).find('.FolderName').val();
				var fileId = $(activeRow).find('.Id').val();
				var isTemporary = $(activeRow).find('.IsTemporary').val();

				httpAction.get('/Chat/RemoveFile/' + fileId + '/' + folderName + '/' + isTemporary, function (response) {
					if (response.success) {
						activeRow.remove();
						uploadControl.initializeTableRows($('#' + tableId));
					} else {
						questionnaire.showErrors([{ errorMessage: 'დაფიქსირდა შეცდომა, გთხოვთ სცადოთ კიდევ ერთხელ' }]);
					}
				});
			}
		},

		uploadFiles: function (obj) {
			var uploadControl = $(obj).next();
			uploadControl.click();
		},

		initializeDownload: function (obj) {
			var id = $(obj).data('message-id');
			var fileName = $(obj).find('.FileName').val();
			var folderName = $(obj).find('.FolderName').val();
			var isTemporary = $(obj).find('.IsTemporary').val();

			httpAction.get('/Chat/InitializeDownload/' + id + '/' + folderName + '/' + (isTemporary == '1'), function (response) {
				if (response.success) {
					window.location.href = '/Chat/Download/' + id + '/' + folderName + '/' + (isTemporary == '1');
				} else {
					alert('Something went wrong ...');
				}
			});
		},
	};
})();