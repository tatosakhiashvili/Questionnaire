jQuery.fn.extend({
	createTable: function (settings) {
		var _settings = {
			dataSource: [],
			resultPerPage: 5,
			allowHighlight: true,
			allowSearch: true,
			allowPaging: true,
			template: '',
			headerTemplate: '',
			noRecordsText: 'ჩანაწერი ვერ მოიძებნა',
			onRowClick: function () { }
		};

		_settings = $.extend(_settings, settings);
		var _source = _settings.dataSource;

		var $table = $(this);
		var $paginationControl;
		var $searchControl;

		var tableTemplate = _settings.template;
		var headerTemplate = _settings.headerTemplate.length == 0 ? replaceString(tableTemplate) : _settings.headerTemplate;

		var footerTemplate = '<tfoot><tr><td><span>' + _settings.noRecordsText + '</span></td></tr></tfoot>';
		var pgRootTemplate = '<ul class="pagination"><li><a class="p-arrow p-previous" aria-label="Previous"><span aria-hidden="true">«</span></a></li><div class="pageing-items"></div><li><a class="p-arrow p-next" aria-label="Next"><span aria-hidden="true">»</span></a></li></ul>';
		var pagingItemsTemplate = '<li><a>${pageNo}</a></li>';

		var totalCount = _settings.dataSource.length;
		var pageCount = calculatePageCount(totalCount, _settings.resultPerPage); // totalCount % _settings.resultPerPage == 0 ? parseInt(totalCount / _settings.resultPerPage) : (parseInt(totalCount / _settings.resultPerPage) + 1);
		var $table = $(this);
		var currentPage = 1;

		function replaceString(template) {

			var replaceCharacters = [
				['$', ''],
				['{', ''],
				['}', ''],
				['<td>', '<th>'],
				['</td>', '</th>'],
			];

			for (var i = 0; i < replaceCharacters.length; i++) {
				while (template.indexOf(replaceCharacters[i][0]) > -1) {
					template = template.replace(replaceCharacters[i][0], replaceCharacters[i][1]);
				}
			}
			return template;
		}

		function Init() {
			redrawTable(_settings.dataSource);
			reCalculatePageing();

			if (_settings.allowSearch) {
				$table.before('<div class="search-container"><label>ძებნა</label><input type="text" /></div>');
			}

			$table.append('<thead>' + headerTemplate + '</thead>');
			$paginationControl = $table.next('.pagination');
			$searchControl = $table.prev('.search-container').find('input[type=text]');

			activatePage();

		}; Init();

		function getTableMaxColspan() {

		}

		function calculatePageCount(totalCount, resultPerPage) {
			return totalCount % resultPerPage == 0 ? parseInt(totalCount / resultPerPage) : (parseInt(totalCount / resultPerPage) + 1);
		}

		function searchInArray(source, searchCriteria) {
			return $.grep(_settings.dataSource, function (e) {
				var containsRow = false;
				for (x in e) {
					if (e[x].toString().toLowerCase().indexOf(searchCriteria) > -1) {
						containsRow = true; break;
					}
				}
				return containsRow;
			});
		};

		function activatePage() {
			$paginationControl.children('li').removeClass('active');
			$paginationControl.children('li:eq(' + (currentPage) + ')').addClass('active');

			var previousButton = $paginationControl.children('li:first-child');
			var nextButton = $paginationControl.children('li:last-child');

			if (currentPage == 1) {
				previousButton.addClass('disabled');
			} else {
				previousButton.removeClass('disabled');
			}

			if (currentPage == pageCount) {
				nextButton.addClass('disabled');
			} else {
				nextButton.removeClass('disabled');
			}

			redrawTable();
		};

		function fillTemplate(template, source) {
			$.template('template', template);
			return $.tmpl('template', source);
		}

		function redrawTable() {
			var source = _source.slice((currentPage - 1) * _settings.resultPerPage, currentPage * _settings.resultPerPage);
			$table.find('tbody').remove();
			fillTemplate(tableTemplate, source).appendTo($table);

			$table.find('tr').click(function () {
				_settings.onRowClick($(this));
			});

			if (_settings.allowHighlight && _settings.allowSearch) {
				if (typeof ($searchControl) != 'undefined') {
					var searchSource = $searchControl.val();
					if (searchSource != '') {
						$table.find('tbody').highlight(searchSource);
					}
				}
			}
		}

		function reCalculatePageing() {
			$table.next('.pagination').remove();
			$table.find('tfoot').remove();
			if (pageCount == 0) { $table.append(footerTemplate); return; }

			var pageArray = [];
			for (var i = 0; i < pageCount; i++) {
				pageArray.push({ pageNo: i + 1 });
			}
			var pagingControl = $(pgRootTemplate);
			$(pagingControl).find('.pageing-items').replaceWith(fillTemplate(pagingItemsTemplate, pageArray));

			$table.after(pagingControl);

			$paginationControl = $table.next('.pagination');
			bindPaginationControl($paginationControl);
		}

		function bindPaginationControl(control) {

			control.find('li a').unbind('click').on('click', function (e) {
				var index = $(this).parent().index();
				var isDisabled = $(this).parent().hasClass('disabled');
				var isNextButton = $(this).parent().is(':last-child');
				var isPrevButton = $(this).parent().is(':first-child');
				var isPageButton = !isNextButton && !isPrevButton;

				if (isDisabled) { return; }

				if (isNextButton) { currentPage += 1; }

				if (isPrevButton) { currentPage -= 1; }

				if (isPageButton) { currentPage = index; }

				activatePage();
			});
		}

		bindPaginationControl($paginationControl);

		$searchControl.on('keyup', function () {
			var searchCriteria = $(this).val().toLowerCase();
			var searchSource = searchInArray(_settings.dataSource, searchCriteria);

			currentPage = 1; totalCount = searchSource.length; pageCount = calculatePageCount(totalCount, _settings.resultPerPage);
			_source = searchSource;

			redrawTable();
			reCalculatePageing();
			activatePage();
		});

		return $table;
	}
});