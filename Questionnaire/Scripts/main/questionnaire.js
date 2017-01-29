var questionnaire = (function () {
	var loaderControl = '';
	var loaderSet = 100;
	var loaderInstanceCount = 0;

	$('#tree').on('contextmenu', '.list-group-item', function (e) {
		var nodeId = $(this).data('nodeid');
		var rootId = $(this).find('.text-span-data').data('root-id'); //ROOTID will be implemented here
		var notShowContextMenu = $('#history-datepicker-checker').is(':checked');

		if (notShowContextMenu) {
			return false;
		}

		var currentUserIsAdmin = $('#currentUserIsAdmin').val();

		if (nodeId == '0' && !currentUserIsAdmin) {
			return false;
		}

		$(this).addClass('context-active-color');
		var $contextMenu = (nodeId == '0' ? $('#contextMenuOnlyAdd') : (rootId == '2' ? $('#contextMenuOnlyMove') : $("#contextMenu")));
		$(this).click();
		$contextMenu.css({
			display: "block",
			left: e.pageX,
			top: e.pageY
		});
		return false;
	});

	$('body').click(function () {
		if ($('#contextMenu').css('display') == 'block') {
			$('#contextMenu').fadeOut();
			$('.context-active-color').each(function () {
				$(this).removeClass('context-active-color');
			});
		}

		if ($('#contextMenuOnlyMove').css('display') == 'block') {
			$('#contextMenuOnlyMove').fadeOut();
			$('.context-active-color').each(function () {
				$(this).removeClass('context-active-color');
			});
		}

		if ($('#contextMenuOnlyAdd').css('display') == 'block') {
			$('#contextMenuOnlyAdd').fadeOut();
			$('.context-active-color').each(function () {
				$(this).removeClass('context-active-color');
			});
		}

		if ($('#favourite-contextMenu').css('display') == 'block') {
			$('#favourite-contextMenu').fadeOut();
		}

		if ($('#reminder-items-contextMenu').css('display') == 'block') {
			$('#reminder-items-contextMenu').fadeOut();
		}
	});

	$('body').on('blur', '.datepicker-container input[type="text"]', function () {
		if ($(this).hasClass('include-time')) {
			var unix = moment($(this).val(), "DD/MM/YYYY h:mm:ss").unix();
			var localEpoch = unix + (new Date().getTimezoneOffset() * -60);
			$('#storedLocalEpoch').val(localEpoch);
		} else {
			var unix = moment($(this).val(), "DD/MM/YYYY").unix();
			var localEpoch = unix + (new Date().getTimezoneOffset() * -60);
			$(this).parent().next('input[type="hidden"]').val(localEpoch);
		}
	});

	$('body').on('click', '.editor-internal-link', function () {
		var nodeId = $(this).data('node-id');
		questionaireTree.viewEmailAnswer(nodeId);
	});

	$('body').on('click', '.customized-pagination li a', function () {

		if ($(this).parent().hasClass("disabled")) {
			return;
		}

		var isNextOrPrevControl = $(this).hasClass('p-arrow');
		var pageNo = 0;
		var pageCount = $(this).parent().parent().children().length - 2;

		if (!isNextOrPrevControl) {
			$(this).parent().parent().children().each(function () {
				$(this).removeClass('active');
			});

			$(this).parent().addClass('active');

			pageNo = $(this).data('page-no');

		} else {
			var currentPage = 0;

			$(this).parent().parent().children().each(function () {
				var _link = $(this).find('a');
				if (_link.parent().hasClass('active')) {
					currentPage = _link.data('page-no');
					return;
				}
			});

			if ($(this).hasClass('p-next')) { //This is next arrow
				currentPage += 1;
			}

			if ($(this).hasClass('p-previous')) { //This is prev arrow
				currentPage -= 1;
			}

			//Activate Current page
			$(this).parent().parent().children().each(function () {
				if ($(this).find('a').data('page-no') == currentPage) {
					$(this).find('a').parent().addClass('active');
				} else {
					$(this).find('a').parent().removeClass('active');
				}
			});

			pageNo = currentPage; //Sets current page no >> will be required to set it in hidden value too
		}

		if (pageNo > 1) { //Enable prev arrow
			$('.p-arrow.p-previous').parent().removeClass('disabled');
		} else { //Disable prev arrow
			$('.p-arrow.p-previous').parent().addClass('disabled');
		}

		if (pageNo >= pageCount) { //Disable next arrow
			$('.p-arrow.p-next').parent().addClass('disabled');
		} else {//Enable next arrow
			$('.p-arrow.p-next').parent().removeClass('disabled');
		}

		var tableName = '';
		var tableSelector = ''
		$(this).parent().parent().children().each(function () {
			if ($(this).find('a').data('page-no') == pageNo) {
				tableName = $(this).find('a').data('table-body');
				tableSelector = $(this).find('a').data('table-selector');
				return;
			}
		});

		$('.' + tableSelector).hide();
		$('#' + tableName).fadeIn();

	});

	$('body').on('keyup', '.searcher-control', function () {
		var tableName = $(this).data('table-id');
		var table = $(this).parent().parent().next().next('table');
		if (tableName != '') { table = $('#' + tableName); }

		var searchCriteria = $(this).val();
		if (searchCriteria.length > 0) {
			$(table).find('tbody tr').each(function (e) {
				var contains = false;
				$(this).find('td').each(function () {
					var content = $(this).html();
					content = content.toLowerCase();
					searchCriteria = searchCriteria.toLowerCase();
					if (content.indexOf(searchCriteria) > -1) {
						contains = true;
					}
				});

				if (contains) {
					$(this).show();
				} else {
					$(this).hide();
				}
			});
		} else {
			$(table).find('tr').show();
		}

		//============ Determine Colspan of table ================//
		var colCount = 0;
		$(table).find('tbody tr:nth-child(1) td').each(function () {
			if ($(this).attr('colspan')) {
				colCount += +$(this).attr('colspan');
			} else {
				colCount++;
			}
		});
		//============ Determine Colspan of table ================//

		//============ Show or hide footer ================//
		var searchCount = $(table).find('tbody tr:visible').length;
		if (searchCount == 0) {
			//Add footer
			var footer = "<tfoot><tr><td colspan='" + colCount + "'>ჩანაწერი ვერ მოიძებნა</td></tr></tfoot>";
			var existingFooter = $(table).find('tfoot');

			if (existingFooter.length == 0) {
				$(table).append(footer);
			}
		} else {
			//Remove footer
			$(table).find('tfoot').remove();
		}
		//============ Show or hide footer ================//

		//============ Re calculate paging ================//
		console.log('Calculating paging ...');
		//============ Re calculate paging ================//
	});

	$('body').on('change', '.comparison-checker', function (e) {
		questionnaire.closeCustomErrorContainer('comparison-check-min-two-error');
		var checkedCount = $('.comparison-checker:checked').length;
		if (checkedCount == 3) {
			$(this).attr("checked", false);
		}
	});

	$('body').on('click', 'table.selectable tbody tr', function () {
		var isSelected = $(this).hasClass('active');

		$(this).siblings().each(function () {
			$(this).removeClass('active');
		});

		if (isSelected) {
			$(this).removeClass('active');
		} else {
			$(this).addClass('active');
		}
	});

	$('.general-error-container').on('click', '.close', function () {
		closeErrorPanel();
	});

	$('.general-success-container').on('click', '.close', function () {
		closeSuccessPanel();
	});

	$('body').on('keydown', 'form input', function () {
		//questionnaire.validation.validateFormControl($(this).closest('form'), $(this));
	});

	$('body').on('hidden.bs.modal', '#modal-container .modal', function () {
		questionnaire.destroyModal();
		questionnaire.hideErrors();
	});

	$('body').on('change', '.sms-checker-checkbox', function () {
		if ($('.sms-checker-checkbox:checked').length > 0) {
			$('#btn-send-sms-modal').removeAttr('disabled');
		} else {
			if ($('#txt-custom-sms-sender').val().length <= 0) {
				$('#btn-send-sms-modal').attr('disabled', 'disabled');
			}
		}
	});

	function maintainTabControlPageing(activeControl) {
		var tabName = $(activeControl).attr('id');
		var pageNumber = 0;
		var pageCount = 0;
		var tableBodyName = '';
		var tableBodySelector = '';

		if (tabName == 'tab-itam-top-favourite-action') {
			pageCount = $('#tab-itam-top-favourite .page-count').val();
			pageNumber = $('#tab-itam-top-favourite .current-page').val();
			tableBodyName = 'top-list-body-favourite';
			tableBodySelector = 'top-list-body-favourite-Selector';
		}

		if (tabName == 'tab-item-top-10-action') {
			pageCount = $('#tab-itam-top-10 .page-count').val();
			pageNumber = $('#tab-itam-top-10 .current-page').val();
			tableBodyName = 'top-list-body-TopTen';
			tableBodySelector = 'top-list-body-TopTen-Selector';
		}

		if (tabName == 'tab-item-month-10-action') {
			pageCount = $('#tab-item-month-10 .page-count').val();
			pageNumber = $('#tab-item-month-10 .current-page').val();
			tableBodyName = 'top-list-body-MonthTopTen'
			tableBodySelector = 'top-list-body-MonthTopTen-Selector'
		}

		if (tabName == 'tab-item-today-10-action') {
			pageCount = $('#tab-item-today-10 .page-count').val();
			pageNumber = $('#tab-item-today-10 .current-page').val();
			tableBodyName = 'top-list-body-TodayTopTen';
			tableBodySelector = 'top-list-body-TodayTopTen-Selector';
		}

		if (tabName == 'tab-item-abonent-faq-action') {
			pageCount = $('#tab-item-abonent-faq .page-count').val();
			pageNumber = $('#tab-item-abonent-faq .current-page').val();
			tableBodyName = 'top-list-body-customer-question';
			tableBodySelector = 'top-list-body-customer-question-Selector';
		}

		$('.top-tab-controls .pagination.customized-pagination').html('');

		if (pageCount > 1) {
			$('.top-tab-controls .pagination.customized-pagination').append('<li class="disabled"><a href="#" data-table-selector="' + tableBodySelector + '" data-table-body="' + tableBodyName + '-' + i + '" class="p-arrow p-previous" aria-label="Previous"><span aria-hidden="true">&laquo;</span></a></li>')
			for (var i = 1; i <= pageCount; i++) {
				$('.top-tab-controls .pagination.customized-pagination').append("<li class='" + (i == pageNumber ? "active" : "") + "'><a class='page-item' data-table-selector='" + tableBodySelector + "' data-table-body='" + tableBodyName + "-" + i + "' data-page-no='" + i + "'>" + i + "</a></li>");
			}
			$('.top-tab-controls .pagination.customized-pagination').append('<li class=""><a href="#" data-table-selector="' + tableBodySelector + '" data-table-body="' + tableBodyName + '-' + i + '" class="p-arrow p-next" aria-label="Next"><span aria-hidden="true">&raquo;</span></a></li>');
		}

		$('.top-tab-controls .pagination.customized-pagination .active a').click();


		$('.top-items-controls').html($('.top-tab-controls'));
	}

	function InitializeLoader() {
		var fromRight = 100;
	}

	function showSuccessPanel() {
		$('.general-success-container').slideDown();
	}

	function closeSuccessPanel() {
		$('.general-success-container').slideUp();
	}

	function showErrorPanel() {
		$('.general-error-container').slideDown();
	}

	function closeErrorPanel() {
		$('.general-error-container').slideUp();
	}

	function manageControls() {
		$('.manage-touch-spin').TouchSpin();
	}

	return {
		showError: function (title, errorMessage) {
			$('.general-error-container').find('.error-title').html(title);
			$('.general-error-container').find('.error-content').html(errorMessage);
			$('.general-error-container').slideDown();
		},

		showErrors: function (errorMessages, containerId) {
			var errorContainer = '';
			for (var i = 0; i < errorMessages.length; i++) {
				errorContainer += '<li><span class="error-content">' + errorMessages[i].errorMessage + '</span></li>';
			}
			$('.general-error-container .error-container').html(errorContainer);
			if (containerId != undefined) {
				$('.general-error-container .error-container').attr('id', containerId);
			}
			showErrorPanel();
		},

		closeCustomErrorContainer: function (containerId) {
			var Id = $('.general-error-container .error-container').attr('id');
			if (Id == containerId) {
				closeErrorPanel();
			}
		},

		showSuccessMessage: function (message) {
			var successContainer = '<li><span class="success-content">' + message + '</span></li>';
			$('.general-success-container .success-container').html(successContainer);
			showSuccessPanel();
			setTimeout(function () { closeSuccessPanel(); }, 2500);
		},

		hideErrors: function () {
			closeErrorPanel();
		},

		startLoader: function () {
			if (loaderInstanceCount == 0) {
				$('.loader-container').fadeIn();
				loaderControl = setInterval(function () {
					loaderSet -= 1;
					if (loaderSet == 0) { loaderSet = 100; }
					$('.line-loader').css('right', loaderSet + '%');
				}, 15);
			} loaderInstanceCount += 1;
		},

		stopLoader: function () {
			loaderInstanceCount -= 1;
			if (loaderInstanceCount == 0) {
				$('.loader-container').fadeOut();
				clearInterval(loaderControl);
			}
		},

		showModal: function (data) {
			//$('#modal-container').html(data);
			//$('#modal-container .modal').modal();
			var $modalContainer = $('#modal-container');

			$modalContainer.append(data);
			$modalContainer.children('.modal').last().modal();

			var hasDatePicker = $modalContainer.find('.datepicker-container').length > 0;

			if (hasDatePicker) {

				$modalContainer.find('.datepicker').each(function () {
					$(this).children('input[type="text"]').mask("00/00/0000", { placeholder: "დდ/თთ/წწწწ" });

					$(this).datetimepicker({
						useCurrent: true,
						format: 'DD/MM/YYYY',
						showClose: true
					});

					$(this).on("dp.hide", function (e) {
						var unix = e.date.unix();
						var localEpoch = unix + (new Date().getTimezoneOffset() * -60);
						$(this).next('input[type="hidden"]').val(localEpoch);
					});
				});
			}

			manageControls();

			$('#modal-container .modal').removeAttr('tabindex');
		},

		closeModal: function () {
			//$('#modal-container .modal').modal('toggle');

			$('#modal-container').children('.modal').last().modal('toggle');
		},

		destroyModal: function () {
			$('#modal-container').children('.modal').last().remove();
			var modals = $('#modal-container .modal').length;

			if (modals > 0 && !$('body').hasClass('modal-open')) {
				$('body').addClass('modal-open');
			}
		},

		validation: (function () {
			return {
				validateForm: function (form) {
					var invalidFieldCount = 0;
					var validationErrorList = [];

					form.find("[data-required-if]").each(function () {
						var field = $(this);
						var requiredIfField = $('#' + field.data('required-if-control'));
						var requiredIfFieldValue = field.data('required-if-control-value').toString();
						var requiredFieldValue = '';

						if ($(requiredIfField).is(':checkbox')) {
							requiredFieldValue = $(requiredIfField).is(':checked').toString();
						}

						if (requiredFieldValue === requiredIfFieldValue && (field.val() == '' || field.val() == '-1')) {
							invalidFieldCount += 1;
							validationErrorList.push({ errorMessage: $(this).data('v-m') });
							field.addClass('required-input');
						} else {
							field.removeClass('required-input');
						}
					});

					form.find("[data-required-textarea]").each(function () {
						var field = $(this);
						if (!field.is(":visible")) { //Assume that this is tinymce editor
							var id = $(field).attr("id");
							var fieldValue = tinyMCE.get(id).getContent({ format: 'text' }).trimRight();
							if (fieldValue == '') {
								invalidFieldCount += 1;
								validationErrorList.push({ errorMessage: $(this).data('v-m') });
								field.parent().children('.mce-panel').each(function () {
									$(this).addClass('required-input');
								});
							} else {
								field.parent().children('.mce-panel').each(function () {
									$(this).removeClass('required-input');
								});
							}
						}
					});

					form.find("[required]").each(function () {
						var field = $(this);
						if (field.val() == '' || field.val() == '-1') {
							invalidFieldCount += 1;
							validationErrorList.push({ errorMessage: $(this).data('v-m') });
							$(this).addClass('required-input');
						} else {
							$(this).removeClass('required-input');
						}
					});

					if (invalidFieldCount > 0) {
						questionnaire.showErrors(validationErrorList);
					} else {
						questionnaire.hideErrors();
					}

					return invalidFieldCount == 0;
				},

				validateFormControl: function (form, control) {

				}
			};
		})(),

		maintainTabControl: function (activeTab) {
			maintainTabControlPageing(activeTab);
		},

		initializeDatepicker: function (form) {
			$(form).find('.datepicker').datetimepicker({
				useCurrent: true,
				format: 'DD/MM/YYYY',
				showClose: true
			});

			$(form).find('.datepicker').on("dp.hide", function (e) {
				var unix = e.date.unix();
				var localEpoch = unix + (new Date().getTimezoneOffset() * -60);
				$(this).next('input[type="hidden"]').val(localEpoch);
			});
		},

		initializeTinyMceEditor: function (selector) {
			 
			if (tinyMCE.editors.length > 0) {
				tinyMCE.execCommand('mceFocus', true, "Answer");
				tinyMCE.execCommand('mceRemoveEditor', true, "Answer");
				tinyMCE.execCommand('mceAddEditor', true, "Answer");
			}

			tinyMCE.PluginManager.add('customem', function (editor, url) {
				editor.addButton('customNodeAttacher', {
					text: 'ხის ელემენტის ჩასმა',
					icon: false,
					onclick: function () {
						var nodeId = $('#SelectedNodeId').val();
						questionaireTree.showCustomTagsModal(nodeId, editor, function (response) { });
					}
				});
			});

			tinymce.init({
				selector: selector,
				height: 300,
				content_css: [
					 '/../Content/EditorLayout.css'
				],

				valid_children: "+body[style], +style[type]",
				apply_source_formatting: false,
				verify_html: false,

				plugins: [
								"advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
								"searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
								"table contextmenu directionality emoticons template textcolor paste textcolor colorpicker textpattern customem"
				],
				toolbar1: "customNodeAttacher",
				toolbar2: "bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | styleselect formatselect fontselect fontsizeselect",
				toolbar3: "cut copy paste | searchreplace | bullist numlist | outdent indent blockquote | undo redo | link unlink anchor image media code | insertdatetime preview | forecolor backcolor",
				toolbar4: "table | hr removeformat | subscript superscript | charmap emoticons | print fullscreen | ltr rtl | spellchecker | visualchars visualblocks nonbreaking template pagebreak restoredraft",

				menubar: false,

				paste_data_images: true,
				images_upload_handler: function (blobInfo, success, failure) {
					success("data:" + blobInfo.blob().type + ";base64," + blobInfo.base64());
				},
				paste_retain_style_properties: "all",
				toolbar_items_size: 'small',

				style_formats: [
								{ title: 'Bold text', inline: 'b' },
								{ title: 'Red text', inline: 'span', styles: { color: '#ff0000' } },
								{ title: 'Red header', block: 'h1', styles: { color: '#ff0000' } },
								{ title: 'Example 1', inline: 'span', classes: 'example1' },
								{ title: 'Example 2', inline: 'span', classes: 'example2' },
								//{ title: 'Table styles' },
								//{ title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
				],

				font_formats: 
                    "Arial=arial,helvetica,sans-serif;" +										
                    "Arial Black=arial black,avant garde;" +
										"Sylfaen=sylfaen;" +
										"AcadNusx=AcadNusx;" +
										"Book Antiqua=book_antiquaregular,palatino;" +
                    "Corda Light=CordaLight,sans-serif;" +
                    "Courier New=courier_newregular,courier;" +
                    "Flexo Caps=FlexoCapsDEMORegular;" +
                    "Lucida Console=lucida_consoleregular,courier;" +
                    "Georgia=georgia,palatino;" +
                    "Helvetica=helvetica;" +                    
                    "Andale Mono=andale mono,times;" +
                    "Impact=impactregular,chicago;" +
                    "Museo Slab=MuseoSlab500Regular,sans-serif;" +
                    "Museo Sans=MuseoSans500Regular,sans-serif;" +
                    "Oblik Bold=OblikBoldRegular;" +
                    "Sofia Pro Light=SofiaProLightRegular;" +
                    "Symbol=webfontregular;" +
                    "Tahoma=tahoma,arial,helvetica,sans-serif;" +
                    "Terminal=terminal,monaco;" +
                    "Tikal Sans Medium=TikalSansMediumMedium;" +
                    "Times New Roman=times new roman,times;" +
                    "Trebuchet MS=trebuchet ms,geneva;" +
                    "Verdana=verdana,geneva;" +
                    "Webdings=webdings;" +
                    "Wingdings=wingdings,zapf dingbats" +
                    "Aclonica=Aclonica, sans-serif;" +
                    "Michroma=Michroma;" +
                    "Paytone One=Paytone One, sans-serif;" +
                    "Andalus=andalusregular, sans-serif;" +
                    "Arabic Style=b_arabic_styleregular, sans-serif;" +
                    "Andalus=andalusregular, sans-serif;" +                    
                    "Mothanna=mothannaregular, sans-serif;" +
                    "Nastaliq=irannastaliqregular, sans-serif;" +
                    "Samman=sammanregular;",

				templates: [
								{ title: 'Test template 1', content: 'Test 1' },
								{ title: 'Test template 2', content: 'Test 2' }
				]
			});

		}
	};
})();

function maintainFavouriteFade() {
	var historyCheckboxIsChecked = $('#history-datepicker-checker').is(':checked');
	var treeSearchCheckboxIsChecked = $('#tree-search-checker').is(':checked');

	if (historyCheckboxIsChecked || treeSearchCheckboxIsChecked) {
		$('#top-tab-container-fader').addClass('active');
	} else {
		$('#top-tab-container-fader').removeClass('active');
	}
}