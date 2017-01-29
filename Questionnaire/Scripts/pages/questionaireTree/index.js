var questionaireTree = (function () {

	var selectedEpoch = '';
	var treeSelectedItemsStack = [];
	var defaultTreeDate = '1900-01-01';
	var treeData = [];
	var $emailEditor;
	var $tree;

	function _init() {
		$tree = $('#tree');
	}; _init();

	function getTreeDefaultEpoch() {
		return moment('1900-01-01').unix() + (new Date().getTimezoneOffset() * -60);
	}

	$('#history-datepicker-checker').change(function () {
		if ($(this).is(':checked')) {
			$('#menu-questionaire a').css('color', 'red');
			$(this).next('span').css('color', 'red');

			$('.datepicker-fader').fadeOut();
		} else {
			$('#menu-questionaire a').css('color', 'black');
			$(this).next('span').css('color', 'black');

			$('.datepicker-fader').fadeIn();
			selectedEpoch = '';
			questionaireTree.get($('#only-published-tree-items').is(':checked'), getTreeDefaultEpoch(), true);
		}

		maintainFavouriteFade();
	});

	$('#tree-multi-selector-mode').change(function () {
		if ($(this).is(':checked')) {
			//$('#tree .custom-checkbox').animate({ marginRight: '0' }, 200);
			$('#tree .custom-checkbox').addClass('shown');
			checkboxesAreShown = true;
		} else {
			//$('#tree .custom-checkbox').animate({ marginRight: '-40px' }, 200);
			$('#tree .custom-checkbox').removeClass('shown');
			checkboxesAreShown = false;
		}
	});

	$('body').on('change', '.extended-search-from-to-fader-checkbox', function () {
		if ($(this).is(':checked')) {
			$('.extended-search-from-to-fader').slideUp();
		} else {
			$('.extended-search-from-to-fader').slideDown();
		}
	});

	$('body').on('change', '.extended-search-status-fader-checkbox', function () {
		if ($(this).is(':checked')) {
			$('.extended-search-status-fader').slideUp();
		} else {
			$('.extended-search-status-fader').slideDown();
		}
	});

	$('body').on('change', '.extended-search-owner-fader-checkbox', function () {
		if ($(this).is(':checked')) {
			$('.extended-search-owner-fader').slideUp();
		} else {
			$('.extended-search-owner-fader').slideDown();
		}
	});

	$('body').on('change', '#input-sms-sender', function () {
		if ($(this).is(':checked')) {
			$('.sms-input-fader').fadeOut();
			$('#txt-custom-sms-sender').val('').focus();
		} else {
			$('.sms-input-fader').fadeIn();
			$('#txt-custom-sms-sender').val('');
			if ($('.sms-checker-checkbox:checked').length > 0) {
				$('#btn-send-sms-modal').removeAttr('disabled');
			} else {
				$('#btn-send-sms-modal').attr('disabled', 'disabled');
			}
		}
	});

	$('body').on('keyup', '#txt-custom-sms-sender', function () {
		var textLength = $(this).val().length;
		if (textLength > 0) {
			$('#btn-send-sms-modal').removeAttr('disabled');
		} else {
			if ($('.sms-checker-checkbox:checked').length > 0) {
				$('#btn-send-sms-modal').removeAttr('disabled');
			} else {
				$('#btn-send-sms-modal').attr('disabled', 'disabled');
			}
		}
	});

	$('#tree-search-checker').change(function () {
		if ($(this).is(':checked')) {
			$('.tree-search-fader').fadeOut();
			$('#txt-tree-searcher').focus();
			$tree.treeview({ data: [] });
			questionaireTree.clearTreeControls();
		} else {
			$('.tree-search-fader').fadeIn();
			selectedEpoch = '';
			questionaireTree.get($('#only-published-tree-items').is(':checked'), getTreeDefaultEpoch(), true);
			$('#txt-tree-searcher').val('');
		}

		maintainFavouriteFade();
	});

	$('#only-published-tree-items').change(function () {
		var epochToPass = selectedEpoch == '' ? getTreeDefaultEpoch() : selectedEpoch;
		if ($(this).is(':checked')) {
			questionaireTree.get(true, epochToPass, false);
		} else {
			questionaireTree.get(false, epochToPass, false);
		}
	});

	if (typeof (moment) != 'undefined') {
		$('#datetimepicker').datetimepicker({
			useCurrent: true,
			format: 'DD/MM/YYYY HH:mm:ss',
			defaultDate: moment().toDate(),
			showClose: true,
		});
	}

	$('#datetimepicker input[type="text"]').mask("00/00/0000 00:00:00", { placeholder: "დდ/თთ/წწწწ სს:წწ:წწ" });

	$('#datetimepicker').on("dp.hide", function (e) {
		var unix = e.date.unix();
		var localEpoch = unix + (new Date().getTimezoneOffset() * -60);
		selectedEpoch = localEpoch;

		$('#storedLocalEpoch').val(selectedEpoch);

		questionaireTree.get($('#only-published-tree-items').is(':checked'), selectedEpoch, false);
	});

	function maintainBackButtonAvailability(nodeId) {
		var _index = treeSelectedItemsStack.indexOf(parseInt(nodeId));

		$('#btn-back').removeClass('disabled');
		$('#btn-next').removeClass('disabled');

		if (_index == 0) {
			$('#btn-back').addClass('disabled');
			$('#btn-next').removeClass('disabled');
		}

		if (_index == treeSelectedItemsStack.length - 1) {
			$('#btn-back').removeClass('disabled');
			$('#btn-next').addClass('disabled');
		}

		//if (treeSelectedItemsStack.length <= 0) {
		//	$('#btn-back').addClass('disabled');
		//	$('#btn-next').addClass('disabled');
		//} else {
		//	var currentElement = treeSelectedItemsStack.filter(function (x) { return x.dataId = $('#SelectedNodeId').val() });
		//	if (currentElement != undefined) {
		//		var currentElementIndex = -1;

		//		for (var i = 0; i < treeSelectedItemsStack.length; i++) {
		//			if (treeSelectedItemsStack[i].dataId == $('#SelectedNodeId').val()) {
		//				currentElementIndex = i;
		//			}
		//		}

		//		if (currentElementIndex == 0) {
		//			$('#btn-back').addClass('disabled');
		//			$('#btn-next').removeClass('disabled');
		//		}

		//		if (currentElementIndex == treeSelectedItemsStack.length - 1) {
		//			$('#btn-back').removeClass('disabled');
		//			$('#btn-next').addClass('disabled');
		//		}
		//	}
		//}
	}

	function maintainNodeItemsStack(dataId) {
		var elementIndex = treeSelectedItemsStack.indexOf((parseInt(dataId)));
		if (elementIndex > -1) {
			treeSelectedItemsStack.splice(elementIndex, 1);
		}
		treeSelectedItemsStack.push(dataId);
	}

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

	function selectTreeNode(nodeId) {
		var nodeIndex = -1;

		for (var i = 0; i < treeData.length; i++) {
			if (treeData[i].NodeId == nodeId) {
				nodeIndex = treeData[i].Id; break;
			}
		}

		if (nodeIndex > -1) {
			$('#tree').treeview('revealNode', [nodeIndex, { silent: true }]);
			$('#tree').treeview('selectNode', [nodeIndex, { silent: true }]);

			var nodeScrollTo = $('[data-nodeid="' + nodeIndex + '"]');
			var scrollFromTop = nodeScrollTo.position().top - 175;

			$('#questionary-tree-container').animate({ scrollTop: scrollFromTop }, function () { });

			onSelectNodeAction(nodeIndex, nodeId);
		}
	}

	function getEmailNotification(nodeId) {
		var isHistoryOn = $('#history-datepicker-checker').is(':checked');
		var historyDateEpoch = $('#storedLocalEpoch').val();
		if (historyDateEpoch == '') {
			historyDateEpoch = 0;
		}

		httpAction.get('/QuestionnaireTree/GetEmailNotification/' + nodeId + '/' + isHistoryOn + '/' + historyDateEpoch, function (response) {
			$('#anwer-container').html(response);
			$('#email-sender-form #EmailBody').val(response);

			if (response == '') {
				$('#btn-email-sender').attr('disabled', 'disabled');
				$('#btn-email-viewer').attr('disabled', 'disabled');
				$('#btn-add-email-answer').attr('disabled', 'disabled');
				$('#btn-compare-email-answers').attr('disabled', 'disabled');
			} else {
				$('#btn-email-sender').removeAttr('disabled');
				$('#btn-email-viewer').removeAttr('disabled');
				$('#btn-add-email-answer').removeAttr('disabled');
				$('#btn-compare-email-answers').removeAttr('disabled');
			}
		});
	}

	function onSelectNodeAction(id, nodeId) {
		$('#SelectedNodeId').val(nodeId);
		sms.getSmsNotifications(nodeId);
		getEmailNotification(nodeId)

		maintainNodeItemsStack(nodeId);
		maintainBackButtonAvailability(nodeId);

		var __node = $('[data-nodeid="' + id + '"]');
		var __nodeOpener = __node.find('.glyphicon.glyphicon-plus');

		if (__nodeOpener.length != 0) {
			__nodeOpener.click();
		}
	}

	return {
		get: function (onlyPublished, epoch, fromCache, callBack) {

			if (!$('#history-datepicker-checker').is(':checked')) {
				epoch = selectedEpoch == '' ? getTreeDefaultEpoch() : selectedEpoch;
			}

			httpAction.get('/QuestionnaireTree/Get/' + onlyPublished + '/' + epoch + '/' + fromCache, function (response) {

				var responseJson = JSON.parse(response.data);
				treeData = responseJson.expandedItems;

				if (callBack != undefined) {
					callBack();
				}

				$tree.treeview({ data: responseJson.treeItems });

				$tree.on('nodeSelected', function (event, data) {
					onSelectNodeAction(data.nodeId, data.data_node)
				});
			});
		},

		getTreeObject: function () {
			return $tree;
		},

		expand: function () {
			$tree.treeview('expandAll', { silent: true });
		},

		collapse: function () {
			$tree.treeview('collapseAll', { silent: true, levels: 2 });
		},

		selectBackOrNextTreeItem: function (isBack, currentNode) {
			if (treeSelectedItemsStack.length > 0) {
				var index = treeSelectedItemsStack.indexOf(parseInt(currentNode));
				var _index = -1;

				var nodeIdToBeSelected = treeSelectedItemsStack[isBack ? index - 1 : index + 1];

				for (var i = 0; i < treeData.length; i++) {
					if (treeData[i].NodeId == nodeIdToBeSelected) {
						_index = treeData[i].Id; break;
					}
				}

				if (_index > -1) {

					questionaireTree.collapse();

					setTimeout(function () {

						//$('#tree').treeview('selectNode', [_index, { silent: true }]);
						$('#SelectedNodeId').val(nodeIdToBeSelected);
						maintainBackButtonAvailability(nodeIdToBeSelected);

						$('#tree').treeview('revealNode', [_index, { silent: true }]);
						$('#tree').treeview('selectNode', [_index, { silent: true }]);

						var nodeScrollTo = $('[data-nodeid="' + _index + '"]');
						var scrollFromTop = nodeScrollTo.position().top - 175;

						$('#questionary-tree-container').animate({ scrollTop: scrollFromTop }, function () { });


						sms.getSmsNotifications(nodeIdToBeSelected);
						getEmailNotification(nodeIdToBeSelected);

					}, 10);
				}
			}
		},

		selectNode: function (nodeId) {
			questionaireTree.collapse();
			setTimeout(function () {
				var n = parseInt(nodeId);
				selectTreeNode(n);
			}, 10);
		},

		getMsisdn: function (number) {
			if (number != '') {
				httpAction.get('/GetMsisdn/' + number, function (response) {
					if (response.success) {
						$('#lblBalance').html('ბალანსი: ' + response.balance);
						$('.lblBalanceInsider').html('ბალანსი: ' + response.balance);

						$('#email-sender-form').find('.dropdown-menu').html('');

						var emailList = '';

						if (response.emails.length > 0) {
							for (var i = 0; i < response.emails.length; i++) {
								emailList += "<li><a data-email='" + response.emails[i] + "' onclick='questionaireTree.chooseEmailDropDownGroup(this)' href='#'>" + response.emails[i] + "</a></li>";
							}
						}

						$('#email-sender-form').find('.dropdown-menu').html(emailList);
					} else {
						$('#lblBalance').html('');
						$('.lblBalanceInsider').html('');
					}
				}, function () {
					$('#lblBalance').html('');
					$('.lblBalanceInsider').html('');
				});
			}
		},

		registerMsisdn: function (number, treeId) {
			if (treeId != '') {

				if (number == '') {
					number = '0';
				}

				httpAction.get('/RegisterMsisdn/' + number + '/' + treeId, function (response) {
					if (response.success) {
						questionnaire.showSuccessMessage('რეგისტრაციამ წარმატებით ჩაიარა');
					} else {
						questionnaire.showErrors([{ errorMessage: response.errorMessage }]);
					}
				});
			} else {
				//if (number == '') {
				//	questionnaire.showErrors([{ errorMessage: 'ნომრის მითითება სავალდებულოა' }]);
				//}

				if (treeId == '') {
					questionnaire.showErrors([{ errorMessage: 'გთხოვთ მონიშნოთ ხე' }]);
				}
			}
		},

		addSms: function (treeId) {
			httpAction.get('/AddSms/' + treeId, function (response) {
				questionnaire.showModal(response);
			});
		},

		addSmsToTree: function (form) {
			var treeId = $(form).find('#TreeId').val();
			var groupId = $(form).find('#groupId').val();
			var groupName = $(form).find('#groupName').val();
			var smsList = $(form).find('#rawSmsContent').val();
			var comment = $(form).find('#commentContainer').val();

			if (questionnaire.validation.validateForm(form)) {
				var data = $(form).serialize();
				httpAction.post('AddSms/' + treeId, data, function () {
					questionnaire.closeModal();
					sms.getSmsNotifications(treeId);
				});
			};
		},

		sendSms: function (isPureText) {
			var mobileNo = $('#msisdn-number').val();
			if (mobileNo.length > 0) {

				var number = mobileNo;
				var smsList = [];
				var strSmsList = '';

				if (isPureText) {
					if ($('#txt-custom-sms-sender').val() == '') {
						questionnaire.showErrors([{ errorMessage: 'ტექსტის მითითება სავალდებულოა' }]);
					} else {
						smsList.push($('#txt-custom-sms-sender').val());
					}
				} else {
					$('.sms-checker-checkbox:checked').each(function () {
						smsList.push($(this).next('span').html());
					});
				}

				if (number != '' && smsList.length > 0) {
					var _data = JSON.stringify({
						smsList: smsList,
						treeId: isPureText ? 0 : $('#SelectedNodeId').val()
					});
					httpAction.init(true);
					httpAction.post('SendSms/' + number, _data, function (response) {
						if (response.success) {
							questionnaire.showSuccessMessage('SMS წარმატებით გაიგზავნა');

							$('#txt-custom-sms-sender').val('');
							$('.sms-checker-checkbox').attr('checked', false);
						} else {
							questionnaire.showErrors([{ errorMessage: response.errorMessage }]);
						}
					});
				}
			} else {
				questionnaire.showErrors([{ errorMessage: 'ნომრის მითითება სავალდებულოა' }]);
			}
		},

		//sendSmsToNumber: function (form) {
		//	if (questionnaire.validation.validateForm(form)) {
		//		var number = $(form).find('#smsNumber').val();
		//		var smsList = [];
		//		var strSmsList = '';

		//		$('.sms-checker-checkbox:checked').each(function () {
		//			smsList.push({ Message: $(this).next('span').html() });
		//			strSmsList += $(this).next('span').html() + '$';
		//		});

		//		if (number != '' && smsList.length > 0) {

		//			httpAction.get('SendSms/' + number + '/' + strSmsList, function (response) {
		//				if (response.success) {
		//					questionnaire.showSuccessMessage('ოპერაცია წარმატებით განხორციელდა');
		//					questionnaire.closeModal();
		//				} else {
		//					questionnaire.showErrors([{ errorMessage: response.errorMessage }]);
		//				}
		//			});
		//		}
		//	};
		//},

		chooseDropDownGroup: function (obj) {
			var id = $(obj).data('group-id');
			var name = $(obj).data('group-name');
			var sortOrder = $(obj).data('sort-order');
			var rawSms = $(obj).data('raw-sms');
			var comment = $(obj).data('comment');

			$('#GroupId').val(id);
			$('#GroupName').val(name);
			$('#GroupSortOrder').val(sortOrder);
			$('#SmsRawContent').val(rawSms);
			$('#Comment').val(comment);
			$('#GroupPreviousName').val(name);
		},



		//================================ Modal Tree Actions

		addTreeNode: function (nodeId) {
			httpAction.get('/QuestionnaireTree/AddTreeNode/' + nodeId, function (response) {
				questionnaire.showModal(response);
			});
		},

		changeTreeNode: function (nodeId) {
			httpAction.get('/QuestionnaireTree/ChangeTreeNode/' + nodeId, function (response) {
				questionnaire.showModal(response);
			});
		},

		moveNodeUp: function (nodeId) {
			httpAction.get('/QuestionnaireTree/MoveNodeUp/' + nodeId, function (response) {
				if (response.success) {
					questionaireTree.get($('#only-published-tree-items').is(':checked'), '1900-01-01', false, function () {
						questionnaire.showSuccessMessage('ოპერაცია წარმატებით განხორციელდა');
					});
				} else {
					questionnaire.showErrors([{ errorMessage: 'ოპერაციისას მოხდა შეცდომა' }]);
				}
			});
		},

		moveNodeDown: function (nodeId) {
			httpAction.get('/QuestionnaireTree/MoveNodeDown/' + nodeId, function (response) {
				if (response.success) {
					questionaireTree.get($('#only-published-tree-items').is(':checked'), '1900-01-01', false, function () {
						questionnaire.showSuccessMessage('ოპერაცია წარმატებით განხორციელდა');
					});
				} else {
					questionnaire.showErrors([{ errorMessage: 'ოპერაციისას მოხდა შეცდომა' }]);
				}
			});
		},

		moveInPreActive: function (nodeId) {
			httpAction.get('/QuestionnaireTree/MoveInPreActive/' + nodeId, function (response) {
				if (response.success) {
					questionaireTree.get($('#only-published-tree-items').is(':checked'), '1900-01-01', false, function () {
						questionnaire.showSuccessMessage('ოპერაცია წარმატებით განხორციელდა');
					});
				} else {
					questionnaire.showErrors([{ errorMessage: 'ოპერაციისას მოხდა შეცდომა' }]);
				}
			});
		},

		copyTreeNode: function (nodeId) {
			httpAction.get('/QuestionnaireTree/CopyTreeNode/' + nodeId, function (response) {
				questionnaire.showModal(response);
			});
		},

		moveTreeNode: function (nodeId) {
			httpAction.get('/QuestionnaireTree/MoveTreeNode/' + nodeId, function (response) {
				questionnaire.showModal(response);
			});
		},

		removeTreeNode: function (nodeId) {
			if (confirm('დარწმუნებული ხართ რომ გსურთ ხის წაშლა?')) {
				httpAction.get('/QuestionnaireTree/RemoveTreeNode/' + nodeId, function (response) {
					if (response.success) {
						questionaireTree.get($('#only-published-tree-items').is(':checked'), '1900-01-01', false, function () {
							questionnaire.showSuccessMessage('ოპერაცია წარმატებით განხორციელდა');
						});
					} else {
						questionnaire.showErrors([{ errorMessage: 'ოპერაციისას დაფიქსირდა შეცდომა' }]);
					}
				});
			}
		},

		addNodeToFavourite: function (nodeId) {
			httpAction.get('/QuestionnaireTree/AddNodeToFavourite/' + nodeId, function (response) {
				if (response.success) {
					topS.getTopItems(function () {
						questionnaire.showSuccessMessage('წარმატებით დაემატა');
					});
				} else {
					questionnaire.showErrors([{ errorMessage: response.errorMessage }]);
				}
			});
		},

		//================================ Modal Tree Actions

		addNodeToTree: function (form, nodeId) {
			if (questionnaire.validation.validateForm(form)) {
				var data = $(form).serialize();
				httpAction.post('/QuestionnaireTree/SaveTree/' + nodeId, data, function (response) {
					if (response.success) {
						questionaireTree.get($('#only-published-tree-items').is(':checked'), '1900-01-01', false, function () {
							questionnaire.showSuccessMessage('ოპერაცია წარმატებით განხორციელდა');
							questionnaire.closeModal();

							setTimeout(function () {
								var selectedNodeId = response.data.treeId;

								if (selectedNodeId != '' && selectedNodeId != '0') {
									questionaireTree.selectNode(selectedNodeId);
								}
							}, 10);

						});
					} else {
						questionnaire.showErrors([{ errorMessage: response.errorMessage }]);
					}
				});
			}
		},

		removeFromFavourite: function (recordId, nodeId) {
			httpAction.get('/QuestionnaireTree/RemoveFromFavourite/' + recordId + '/' + nodeId, function (response) {
				if (response.success) {
					topS.getTopItems(function () {
						questionnaire.showSuccessMessage('წარმატებით წაიშალა');
					});
				} else {
					questionnaire.showErrors([{ errorMessage: response.errorMessage }]);
				}
			});
		},

		recoverDeletedTreeItem: function (nodeId) {
			httpAction.get('/QuestionnaireTree/RecoverFromDeletedItem/' + nodeId, function (response) {
				$('#reqover-deleted-tree-contextMenu').slideUp();
				if (response.success) {
					window.location.reload();
				} else {
					questionnaire.showErrors([{ errorMessage: response.errorMessage }]);
				}
			});
		},

		chooseEmailDropDownGroup: function (obj) {
			var form = $('#email-sender-form');
			var email = $(obj).data('email');
			$(form).find('#Email').val(email);
		},

		viewEmailAnswer: function (nodeId) {
			window.open('/QuestionnaireTree/ViewEmailNotification/' + nodeId, '_blank');
		},

		addEmailAnswer: function (nodeId) {
			httpAction.get('/QuestionnaireTree/AddEmailAnswer/' + nodeId, function (response) {
				questionnaire.showModal(response);
			});
		},

		addEmailAnswerToTree: function (form, nodeId) {
			tinyMCE.get("Answer").save();
			var answerPureContent = tinyMCE.get('Answer').getContent({ format: 'text' });
			$(form).find('#PureAnswer').val(answerPureContent);

			if (questionnaire.validation.validateForm(form)) {
				var data = $(form).serialize();

				httpAction.post('/QuestionnaireTree/AddEmailAnswer/' + nodeId, data, function (response) {
					if (response.success) {
						questionnaire.closeModal();
						getEmailNotification(nodeId);
					} else {
						questionnaire.showErrors([{ errorMessage: response.errorMessage }]);
					}
				});
			}
		},

		startDialogUploading: function (form, treeId) {
			var dialogUploadControl = document.getElementById("modal-tree-file-uploader");;
			var file = dialogUploadControl.files[0];
			var _comment = $(form).find('#Comment').val();

			if (typeof file == 'undefined') {
				questionnaire.showErrors([{ errorMessage: 'ფაილის მითითება სავალდებულოა' }]);
				return;
			}

			var fileData = new FormData();
			fileData.append(file.name, file);
			fileData.append('Comment', _comment)
			var $uploadControl = dialogUploadControl;
			var table = $('#tree-files-table');

			$.ajax({
				url: '/Upload/UploadDialogFile',
				type: 'POST',
				contentType: false,
				processData: false,
				data: fileData,
				success: function (response) {

					questionnaire.closeModal();

					if (response.success) {

						var row =
								"<tr>" +
									"<td>" +
										"<input class='reindex-item Id' data-end-name='Id' data-start-name='Files' id='Files_0__Id' name='Files[0].Id' type='hidden' value='0'>" +
										"<input class='reindex-item FileName' data-end-name='FileName' data-start-name='Files' id='Files_0__FileName' name='Files[0].FileName' type='hidden' value='1'>" +
										"<input class='reindex-item Comment' data-end-name='Comment' data-start-name='Files' id='Files_0__Comment' name='Files[0].Comment' type='hidden' value='1'>" +
										"<input class='reindex-item FolderName' data-end-name='FolderName' data-start-name='Files' id='Files_0__FolderName' name='Files[0].FolderName' type='hidden' value='2'>" +
										"<input class='reindex-item IsTemporary' data-end-name='IsTemporary' data-start-name='Files' data-val='true' data-val-required='The IsTemporary field is required.' id='Files_0__IsTemporary' name='Files[0].IsTemporary' type='hidden' value='True'>" +
										"<a href='#' data-node-id='0' class='filenamelink' onclick='uploadControl.initializeTreeFileDownload(this)'>1</a>" +
									"</td>" +
									"<td>" +
										"<a href='#' data-node-id='0' class='commentlink' onclick='uploadControl.initializeTreeFileDownload(this)'>response.comment</a>" +
									"</td>" +
								"</tr>";

						row = $(row);


						$(row).find('.FileName').val(response.data.fileName);
						$(row).find('.FolderName').val(response.data.folderName);
						$(row).find('.Comment').val(response.data.comment);
						$(row).find('.IsTemporary').val('True');
						$(row).find('.filenamelink').html(response.data.fileName);
						$(row).find('.commentlink').html(response.data.comment);
						$(table).find('tbody').prepend(row);
					}

					//$uploadControl.val(''); // Clear upload control
					initializeTableRows(table);

				},
				error: function (response) { }
			});

		},

		startDialogAnswerUploading: function (form, treeId, answerId) {
			var dialogUploadControl = document.getElementById("modal-tree-file-uploader");;
			var file = dialogUploadControl.files[0];
			var _comment = $(form).find('#Comment').val();

			if (typeof file == 'undefined') {
				questionnaire.showErrors([{ errorMessage: 'ფაილის მითითება სავალდებულოა' }]);
				return;
			}

			var fileData = new FormData();
			fileData.append(file.name, file);
			fileData.append('Comment', _comment)
			var $uploadControl = dialogUploadControl;
			//var table = $('#tree-files-table');
			var table = $('#email-answer-files-table');

			$.ajax({
				url: '/Upload/UploadAnswerDialogFile',
				type: 'POST',
				contentType: false,
				processData: false,
				data: fileData,
				success: function (response) {

					questionnaire.closeModal();

					if (response.success) {

						var row =
								"<tr>" +
									"<td>" +
										"<input class='reindex-item Id' data-end-name='Id' data-start-name='Files' id='Files_0__Id' name='Files[0].Id' type='hidden' value='0'>" +
										"<input class='reindex-item Name' data-end-name='Name' data-start-name='Files' id='Files_0__Name' name='Files[0].Name' type='hidden' value='1'>" +
										"<input class='reindex-item Comment' data-end-name='Comment' data-start-name='Files' id='Files_0__Comment' name='Files[0].Comment' type='hidden' value='1'>" +
										"<input class='reindex-item FolderName' data-end-name='FolderName' data-start-name='Files' id='Files_0__FolderName' name='Files[0].FolderName' type='hidden' value='2'>" +
										"<input class='reindex-item IsTemporary' data-end-name='IsTemporary' data-start-name='Files' data-val='true' data-val-required='The IsTemporary field is required.' id='Files_0__IsTemporary' name='Files[0].IsTemporary' type='hidden' value='True'>" +
										"<a href='#' data-node-id='0' class='filenamelink' onclick='uploadControl.initializeTreeFileDownload(this)'>1</a>" +
									"</td>" +
									"<td>" +
										"<a href='#' data-node-id='0' class='commentlink' onclick='uploadControl.initializeTreeFileDownload(this)'>response.comment</a>" +
									"</td>" +
								"</tr>";

						row = $(row);


						$(row).find('.Name').val(response.data.fileName);
						$(row).find('.FolderName').val(response.data.folderName);
						$(row).find('.Comment').val(response.data.comment);
						$(row).find('.IsTemporary').val('True');
						$(row).find('.filenamelink').html(response.data.fileName);
						$(row).find('.commentlink').html(response.data.comment);
						$(table).find('tbody').prepend(row);
					}

					//$uploadControl.val(''); // Clear upload control
					initializeTableRows(table);

				},
				error: function (response) { }
			});

		},

		uploadFileFromModal: function () {
			$('.tree-file-modal-uploader').click();
		},

		uploadEmailAnswerFiles: function (treeId, answerId) {
			httpAction.get('/QuestionnaireTree/GetAnswerFileUploadDialog/' + treeId + '/' + answerId, function (response) {
				questionnaire.showModal(response);
			});

			//$('.file-uploader-ctrl').click();
		},

		uploadTreeFiles: function (treeId) {
			httpAction.get('/QuestionnaireTree/GetFileUploadDialog/' + treeId, function (response) {
				questionnaire.showModal(response);
			});

			//$('.tree-file-uploader').click();
		},

		removeTreeFiles: function (treeId) {
			var activeRow = $('#tree-files-table tbody tr.active');
			if (activeRow.length > 0) {
				var fileId = $(activeRow).find('.Id').val();
				var fileName = $(activeRow).find('.FileName').val();
				var folderName = (activeRow).find('.FolderName').val();
				var isTemporary = (activeRow).find('.IsTemporary').val();

				httpAction.get('/QuestionnaireTree/RemoveTreeFiles/' + treeId + '/' + fileId + '/' + fileName + '/' + folderName + '/' + isTemporary, function (response) {
					if (response.success) {
						activeRow.remove();
						uploadControl.initializeTableRows($('#tree-files-table'));
					}
				});
			}
		},

		removeEmailAnswerFiles: function (treeId) {
			var activeRow = $('#email-answer-files-table tbody tr.active');
			if (activeRow.length > 0) {
				var name = $('#email-answer-files-table tbody tr.active .nameControl').val();
				var id = $('#email-answer-files-table tbody tr.active .idControl').val();;
				var folderName = $('#email-answer-files-table tbody tr.active .folderNameControl').val();
				var isTemporary = $('#email-answer-files-table tbody tr.active .isTemporaryControl').val();

				httpAction.get('/QuestionnaireTree/RemoveEmailFiles/' + id + '/' + treeId + '/' + name + '/' + folderName + '/' + isTemporary, function (response) {
					if (response.success) {
						activeRow.remove();
						uploadControl.initializeTableRows($('#email-answer-files-table'));
					} else {
						questionnaire.showErrors([{ errorMessage: 'დაფიქსირდა შეცდომა, გთხოვთ სცადოთ კიდევ ერთხელ' }]);
					}
				});
			}
		},

		getEmailTemplate: function (nodeId, templateFileName) {
			httpAction.get('/QuestionnaireTree/GetEmailTemplate/' + nodeId + '/' + templateFileName, function (response) {
				if (response.success) {
					tinyMCE.get('Answer').setContent(response.data.templateBody);
				} else {
					questionnaire.showErrors([{ errorMessage: response.errorMessage }]);
				}
			});
		},

		sendEmailNotification: function (form) {

			$(form).submit(function () { return false; });

			if (questionnaire.validation.validateForm(form)) {
				var data = $(form).serialize();
				var treeId = $('#SelectedNodeId').val();
				httpAction.post('/QuestionnaireTree/SendEmailNotification/' + treeId, data, function (response) {
					if (response.success) {
						$(form).find('#Email').val('');
						questionnaire.showSuccessMessage('ელ. ფოსტა წარმატებით გაიგზავნა');
					} else {
						questionnaire.showErrors([{ errorMessage: 'ელ. ფოსტის გაგზავნისას მოხდა შეცდომა' }]);
					}
				});
			};
		},

		compareAnswersList: function (nodeId) {
			questionnaire.closeModal();
			setTimeout(function () {
				httpAction.get('/QuestionnaireTree/GetEmailAnswersList/' + nodeId, function (response) {
					questionnaire.showModal(response);
				});
			}, 500);
		},

		compareSelectedEmailAnswers: function (form, nodeId) {
			if (questionnaire.validation.validateForm(form)) {
				var checkedCheckboxes = $('.comparison-checker:checked').length;
				if (checkedCheckboxes != 2) {
					questionnaire.showErrors([{ errorMessage: 'შედარების გასაკეთებლად საჭიროა ორი ჩანაწერის არჩევა' }], 'comparison-check-min-two-error');
				} else {
					var data = $(form).serialize();
					httpAction.post('/QuestionnaireTree/CompareEmailAnswers/' + nodeId, data, function (response) {
						questionnaire.closeModal();
						setTimeout(function () {
							questionnaire.showModal(response);
						}, 500);
					});
				}
			}

			//if (questionnaire.validation.validateForm(form)) {
			//	var data = $(form).serialize();

			//	httpAction.post('/QuestionnaireTree/AddEmailAnswer/' + nodeId, data, function (response) {
			//		if (response.success) {
			//			questionnaire.closeModal();
			//			getEmailNotification(nodeId);
			//		} else {
			//			questionnaire.showErrors([{ errorMessage: response.errorMessage }]);
			//		}
			//	});
			//}			 
		},

		showCustomTagsModal: function (nodeId, editor, callBack) {
			httpAction.get('/QuestionnaireTree/GetAnswerTagsModal/' + nodeId, function (response) {
				$emailEditor = editor;
				questionnaire.showModal(response);
			});
		},

		addTreeTagToEditor: function (form, nodeId) {
			var selector = $(form).find("option:selected");
			var _nodeCaption = selector.text();
			var _nodeId = selector.val();
			$emailEditor.insertContent('<a href="#" data-node-id="' + _nodeId + '" class="editor-internal-link">[' + _nodeCaption + ']</a>');
			questionnaire.closeModal();
		},

		clearTreeControls: function () {
			$('#sms-tab-conainer').html('<div class="alert alert-info" style="margin:125px;">ჩანაწერი ვერ მოიძებნა</div>');
			$('#anwer-container').html('');
		},

		searchInTree: function () {
			var textToSearch = $('#txt-tree-searcher').val();
			var searchType = $('#slc-tree-search-type').val();

			httpAction.get('/QuestionnaireTree/SearchInTree/' + searchType + '/' + textToSearch, function (response) {
				var responseJson = JSON.parse(response.data);
				treeData = responseJson.expandedItems;
				$tree.treeview({ data: responseJson.treeItems });
				$tree.on('nodeSelected', function (event, data) {
					onSelectNodeAction(data.nodeId, data.data_node)
				});

				$tree.highlight(textToSearch);
			});
		},

		searchInTreeExtended: function () {
			httpAction.get('/QuestionnaireTree/ExtendedSearch', function (response) {
				questionnaire.showModal(response);
			});
		},

		getProductDetails: function (productId) {
			httpAction.get('/Store/GetProductDetails/' + productId, function (response) {
				$('.product-details-view').html(response);
				$("html, body").animate({ scrollTop: "0" });
			});
		},

		getRoamingOperators: function (countryId) {
			httpAction.get('/RoamingPrice/GetRoamingOperators/' + countryId, function (response) {
				$('#operators-panel #operators-container').html(response);
				$('#operators-panel').show();
				$('#roaming-notification-form').hide();

				$('html, body').animate({ scrollTop: $("#operators-panel").offset().top }, 500);

			});
		},

		getRoamingTariffs: function (operatorId) {
			httpAction.get('/RoamingPrice/GetRoamingTariffs/' + operatorId, function (response) {
				$('#roaming-notification-form #information-content').html(response);
				$('#roaming-notification-form').show();

				$('html, body').animate({ scrollTop: $("#roaming-notification-form").offset().top }, 500);
			});
		},

		extendedSearchForm: function (form) {
			if (questionnaire.validation.validateForm(form)) {

				var isHistoryOn = $('#history-datepicker-checker').is(':checked');
				var historyDate = $('#storedLocalEpoch').val();
				var searchType = $(form).find('#SearchType').val();
				var searchTerm = $(form).find('#SearchTerm').val();
				var searchComment = $(form).find('#SearchComment').val();
				var date = $(form).find('#Date').val();
				var periodStartEpoch = $(form).find('#PeriodStartEpoch').val();
				var periodEndEpoch = $(form).find('#PeriodEndEpoch').val();
				var statusId = $(form).find('#StatusId').val();
				var ownerId = $(form).find('#OwnerId').val();
				var onlyPublished = $(form).find('#OnlyPublished').val();
				var fromToIsActive = $(form).find('#FromToIsActive').is(':checked');
				var statusIsActive = $(form).find('#StatusIsActive').is(':checked');
				var ownerIsActive = $(form).find('#OwnerIsActive').is(':checked');

				if (historyDate == '') { historyDate = 0; }

				var data = JSON.stringify({
					IsHistoryOn: isHistoryOn,
					HistoryDate: historyDate,
					SearchType: searchType,
					SearchTerm: searchTerm,
					SearchComment: searchComment,
					Date: date,
					PeriodStartEpoch: periodStartEpoch,
					PeriodEndEpoch: periodEndEpoch,
					StatusId: statusId,
					OwnerId: ownerId,
					OnlyPublished: onlyPublished,
					FromToIsActive: fromToIsActive,
					StatusIsActive: statusIsActive,
					OwnerIsActive: ownerIsActive,
				});

				httpAction.init(true);
				httpAction.post('/QuestionnaireTree/ExtendedSearch', data, function (response) {
					if (response.success) {
						questionnaire.closeModal();
						var responseJson = JSON.parse(response.data);
						treeData = responseJson.expandedItems;
						$tree.treeview({ data: responseJson.treeItems });
						$tree.on('nodeSelected', function (event, data) {
							onSelectNodeAction(data.nodeId, data.data_node)
						});


						$tree.highlight(searchTerm);

					} else {
						questionnaire.showErrors([{ errorMessage: response.errorMessage }]);
					}
				});


			};
		},

		getInternationalPrice: function (form, getInternationalPriceCallBack) {
			var companyId = $(form).find('#CompanyId').val();
			var countryId = $(form).find('#CountryId').val();

			if (questionnaire.validation.validateForm(form)) {

				var _data = JSON.stringify({ CountryCode: countryId, CompanyCode: companyId });

				httpAction.init(true);
				httpAction.post('/InternationalPrice/GetInternationalTariffs', _data, function (response) {
					$('#international-tariff-container').html(response);
					$('#intarnational-tariff-panel').show();
					$('html, body').animate({ scrollTop: $("#intarnational-tariff-panel").offset().top }, 500);
				});
			}

			$(form).submit(function () {
				return false;
			});
		},

		sendInternationalPriceNotification: function () {
			var number = $('#msisdn-number').val();
			var notificationText = $('#send-notification-text').val();

			if (number == '') {
				questionnaire.showErrors([{ errorMessage: 'ნომრის მითითება სავალდებულოა' }]);
				return;
			}

			if (notificationText == '') {
				questionnaire.showErrors([{ errorMessage: 'გთხოვთ მონიშნოთ გასაგზავნი ტექსტი' }]);
				return;
			}

			var smsList = [];
			smsList.push(notificationText);
			var _data = JSON.stringify({ notificationsText: smsList });
			httpAction.init(true);
			httpAction.post('SendNotificationSms/' + number, _data, function (response) {
				if (response.success) {
					questionnaire.showSuccessMessage('შეტყობინება წარმატებით გაიგზავნა');
				} else {
					questionnaire.showErrors([{ errorMessage: 'შეტყობინების გაგზავნისას მოხდა შეცდომა' }]);
				}
			});
		},

		sendInternationalCodeNotification: function () {
			var sendWithCity = $('.notification-send-with-city-checkbox').is(':checked')
			var number = $('#msisdn-number').val();
			var notificationText = $(sendWithCity ? '#send-notification-with-city-text' : '#send-notification-text').val();

			if (number == '') {
				questionnaire.showErrors([{ errorMessage: 'ნომრის მითითება სავალდებულოა' }]);
				return;
			}

			if (notificationText == '') {
				questionnaire.showErrors([{ errorMessage: 'გთხოვთ მონიშნოთ გასაგზავნი ტექსტი' }]);
				return;
			}

			var smsList = [];
			smsList.push(notificationText);
			var _data = JSON.stringify({ notificationsText: smsList });
			httpAction.init(true);
			httpAction.post('SendNotificationSms/' + number, _data, function (response) {
				if (response.success) {
					questionnaire.showSuccessMessage('შეტყობინება წარმატებით გაიგზავნა');
				} else {
					questionnaire.showErrors([{ errorMessage: 'შეტყობინების გაგზავნისას მოხდა შეცდომა' }]);
				}
			});
		},

		sendRoamingPriceNotification: function (form) {
			if (questionnaire.validation.validateForm(form)) {
				var data = $(form).serialize();
				httpAction.post('/RoamingPrice/SendRoamingPriceNotification', data, function (response) {
					if (response.success) {
						questionnaire.showSuccessMessage('შეტყობინება წარმატებით გაიგზავნა');
					} else {
						questionnaire.showErrors([{ errorMessage: 'შეტყობინების გაგზავნისას მოხდა შეცდომა' }]);
					}
				});
				$(form).submit(function () {
					return false;
				});
			};
		},

		chooseEmailInRoaming: function (obj) {
			var form = $('#roaming-notification-form');
			var email = $(obj).data('email');
			$(form).find('#Email').val(email);
		},

		refreshUserFavouriteList: function () {
			var activeTabId = $('#top-items-tabs li.active a').attr("id");
			this.loadUserFavouriteList(activeTabId);
		},

		loadUserFavouriteList: function (tabName) {

			var actionUrl = '';
			var contentName = '';

			if (tabName == "tab-item-top-10-action") {
				actionUrl = '/Faq/TopFaq/1';
				contentName = 'tab-itam-top-10';
			}

			if (tabName == "tab-item-month-10-action") {
				actionUrl = '/Faq/TopFaq/3';
				contentName = 'tab-item-month-10';
			}

			if (tabName == "tab-item-today-10-action") {
				actionUrl = '/Faq/TopFaq/2';
				contentName = 'tab-item-today-10';
			}

			if (tabName == "tab-item-abonent-faq-action") {
				actionUrl = '/Faq/CustomerFaq/' + $('#msisdn-number').val();
				contentName = 'tab-item-abonent-faq';
			}

			if (actionUrl && actionUrl != '') {
				httpAction.get(actionUrl, function (response) {
					var tabObj = $('#' + contentName);
					$(tabObj).html(response);
					questionnaire.maintainTabControl($('#' + tabName));
				});
			}
		},

		groupOperationMoveToPreactive: function () {
			if (checkboxesAreShown) {
				if (confirm('ნამდვილად გსურთ ' + itemsToBeSelected.length + ' ჩანაწერის პრე-აქთივში გადატანა?')) {
					var data = JSON.stringify({ treeIds: itemsToBeSelected });

					httpAction.init(true);
					httpAction.post('/QuestionnaireTree/GroupedOperationMoveToPreactive', data,
					function (response) {
						if (response.success) {
							//itemsToBeSelected = response.notMovedToPreActiveTreeIds;
							itemsToBeSelected = [];
							questionaireTree.get($('#only-published-tree-items').is(':checked'), '1900-01-01', false, function () {
								questionnaire.showSuccessMessage('ოპერაცია წარმატებით განხორციელდა');
							});
						} else {
							questionnaire.showErrors([{ errorMessage: response.errorMessage }]);
						}
					},
					function () {

					});
				}
			}
		},

		groupOperationDelete: function () {
			if (checkboxesAreShown) {
				if (confirm('ნამდვილად გსურთ ' + itemsToBeSelected.length + ' ჩანაწერის წაშლა?')) {
					var data = JSON.stringify({ treeIds: itemsToBeSelected });

					httpAction.init(true);
					httpAction.post('/QuestionnaireTree/GroupedOperationRemove', data,
					function (response) {
						if (response.success) {
							//itemsToBeSelected = response.notRemovedTreeIds;
							itemsToBeSelected = [];
							questionaireTree.get($('#only-published-tree-items').is(':checked'), '1900-01-01', false, function () {
								questionnaire.showSuccessMessage('ოპერაცია წარმატებით განხორციელდა');
							});
						} else {
							questionnaire.showErrors([{ errorMessage: response.errorMessage }]);
						}
					},
					function () {

					});
				}
			}
		}
	};
})();

function responsibleTreeControl(obj) {
	var objToResponse = $(obj);
	var indentCount = $(objToResponse).find('.indent').length;
	var singleIndentWidth = 16;
	var jicPadding = 120;

	var toolTipWidth = $(objToResponse).find('.badge').width();
	var indentWidth = (indentCount + 1) * singleIndentWidth;

	var widthToAssign = objToResponse.width() - toolTipWidth - indentWidth - jicPadding;

	$(objToResponse).find('.text-span-class').width(widthToAssign);
};

function responsibleAll() {
	$('.list-group .list-group-item').each(function () {
		responsibleTreeControl($(this));
	});
};

//function responseWellTree() {
//	setInterval(function () {
//		responsibleAll();
//	}, 300);
//};

//responseWellTree();