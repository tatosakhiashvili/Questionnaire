$('body').on('click', '.table-pagination li a', function () {

	var isNextOrPreviousControl = $(this).hasClass('p-arrow');

	if (!isNextOrPreviousControl) {
		$(this).parent().parent().children().each(function () {
			$(this).removeClass('active');
		});
		$(this).parent().addClass('active');
	} else {
		var isNextArrow = $(this).hasClass('p-next');

		if (!$(this).parent().hasClass('disabled')) {
			if (isNextArrow) {
				$(this).parent().parent().children('.active').next().find('a').click();
			} else {
				$(this).parent().parent().children('.active').prev().find('a').click();
			}
			return;
		} else {
			return;
		}
	}

	var totalPageCount = $(this).parent().parent().data('page-count');
	var currentPageNo = $(this).parent().parent().children('.active').index();

	if (currentPageNo == 1) {
		$(this).parent().parent().children().first().addClass('disabled');
	} else {
		$(this).parent().parent().children().first().removeClass('disabled');
	}

	if (currentPageNo == totalPageCount) {
		$(this).parent().parent().children().last().addClass('disabled');
	} else {
		$(this).parent().parent().children().last().removeClass('disabled');
	}

	var table = $(this).parent().parent().prev();
	
	table.children('tbody').each(function () {
		if ($(this).index() == currentPageNo) {
			$(this).show();
		} else {
			$(this).hide();
		}
	});
});