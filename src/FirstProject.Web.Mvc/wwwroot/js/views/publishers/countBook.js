(function ($) {
    var _$bookYearCombobox = $('#BookYearCombobox'),
        _$bookCategoryCombobox = $('#BookCategoryCombobox'),
        _bookService = abp.services.app.book;
    $(document).ready(function () {
        getCountBookByPublisherId();
    });
    function getCountBookByPublisherId() {
        var categoryId = _$bookCategoryCombobox.val();
        var publisherId = $('#PubId').val();
        var year = _$bookYearCombobox.val();
        var data = {
            categoryId: categoryId,
            publisherId: publisherId,
            year: year
        }
        _bookService.getBookByPublisher(data)
            .done(function (result) {
                $("#CountBoook").text(result);
            });
    }

    _$bookYearCombobox.change(function () {
        getCountBookByPublisherId()
    });
    _$bookCategoryCombobox.change(function () {
        getCountBookByPublisherId()
    });
})(jQuery);
