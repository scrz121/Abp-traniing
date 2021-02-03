(function ($) {
    var canLoad = true;
    var coverImage = null;
    var _$bookStateCombobox = $('#BookStateCombobox'),
        _$bookCategoryCombobox = $('#BookCategoryCombobox'),
        _$bookYearCombobox = $('#BookYearCombobox'),
        _$bookPublisherCombobox = $('#BookPublisherCombobox'),
        _bookService = abp.services.app.book,
        l = abp.localization.getSource('FirstProject'),
        _$modal = $('#BookCreateModal'),
        _$form = _$modal.find('form');
    var _$table = $('#BooksTable');

    var _$booksTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        ajax: function (data, callback, settings) {
            var filter = $('#BooksSearchForm').serializeFormToObject();
            filter.state = _$bookStateCombobox.val() ? _$bookStateCombobox.val() : null;
            filter.category = _$bookCategoryCombobox.val() ? _$bookCategoryCombobox.val() : null;
            filter.publisher = _$bookPublisherCombobox.val() ? _$bookPublisherCombobox.val() : null;
            filter.year = _$bookYearCombobox.val() ? _$bookYearCombobox.val() : null;
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;
            abp.ui.setBusy(_$table);

            _bookService.getAll(filter)
                .done(function (result) {
                    for (var i = 0; i < result.items.length;i++) {
                        result.items[i].state = result.items[i].state == "0" ? "Old" : "New";
                    }
                    callback({
                        recordsTotal: result.totalCount ,
                        recordsFiltered: result.totalCount,
                        data: result.items
                    });
            }).always(function () {
                abp.ui.clearBusy(_$table);
            });
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt book_reset"></i>',
            }
        ],
        responsive: {
            details: {
                type: 'column'
            }
        },
        columnDefs: [
            {
                targets: 0,
                className: 'control',
                defaultContent: '',
            },
            {
                targets: 1,
                className:'book_title',
                data: 'title',
                sortable: false
            },
            {
                targets: 2,
                data: 'inventoryNumber',
                sortable: false
            },
            {
                targets: 3,
                data: 'state',
                sortable: false
            },
            {
                targets: 4,
                render: (data, type, row, meta) => {
                    return [
                        `<img style="height:100px" src="${row.coverImage}"/>`
                    ].join('');
                },
                sortable: false
            },
            {
                targets: 5,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-book" data-book-id="${row.id}" data-toggle="modal" data-target="#BookEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-book" data-book-id="${row.id}" data-book-title="${row.title}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });


    _$bookStateCombobox.change(function () {
        if (canLoad) _$booksTable.ajax.reload();
    });
    _$bookCategoryCombobox.change(function () {
        if (canLoad) _$booksTable.ajax.reload();
    });
    _$bookYearCombobox.change(function () {
        if (canLoad) _$booksTable.ajax.reload();
    });
    _$bookPublisherCombobox.change(function () {
        if (canLoad) _$booksTable.ajax.reload();
    });

    $('.btn-search').on('click', (e) => {
        _$booksTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$booksTable.ajax.reload();
            return false;
        }
    });

    $('.book_reset').closest("button").on('click', () => {
        canLoad = false;
        $('#BooksSearchForm').val("");
        _$bookStateCombobox.val("");
        _$bookCategoryCombobox.val("");
        _$bookPublisherCombobox.val("");
        _$bookYearCombobox.val("");
        $('select[name ="BooksTable_length"]').val(10);
        canLoad = true;
        $('.btn-search').click();
    });

    // edit and delete
    function save() {
        if (!_$form.valid()) {
            return;
        }

        let formData = new FormData(_$form[0]);
        formData.set("CoverImageFile", coverImage);
        abp.ui.setBusy(_$form);

        abp.ajax({
            url: abp.appPath + 'api/services/app/Book/Create',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false
        }).done(function (result) {
            abp.notify.info(l('Thêm mới sách thành công!'));
        }).always(function () {
            abp.ui.clearBusy(_$form);
            _$modal.modal('hide');
            _$booksTable.ajax.reload();
        });
    }

    _$form.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        save();
    });

    _$form.find('input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
        }
    });


    $(document).on('click', '.delete-book', function () {
        var bookId = $(this).attr("data-book-id");
        var bookTitle = $(this).attr('data-book-title');

        deleteBook(bookId, bookTitle);
    });

    function deleteBook(bookId, bookTitle) {
        abp.message.confirm(
            abp.utils.formatString(
                l('Are You Sure Want To Delete'),
                bookTitle),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _bookService.delete({ bookId: bookId }, {
                        type: "Delete"
                    })
                        .done(function () {
                            abp.notify.info(l('Successfully Deleted'));
                            _$booksTable.ajax.reload();
                        });
                }
            }
        );
    }

    $(document).on('click', '.edit-book', function (e) {
        var bookId = $(this).attr("data-book-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Books/EditModal?bookId=' + bookId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#BookEditModal div.modal-content').html(content);
            },
            error: function (e) { }
        });
    });

    abp.event.on('book.edited', (data) => {
        _$booksTable.ajax.reload();
    });

    $("#input-change-cover-image").bind("change", function () {
        let fileData = $(this).prop("files")[0];
        let math = ["image/png", "image/jpg", "image/jpeg"];
        if (!fileData) {
            $("#thumbnail").attr("src", null);
            return false;
        }
        if ($.inArray(fileData.type, math) === -1) {
            alert("Kiểu file không hợp lệ, chỉ chấp nhận jpg & png");
            $(this).val(null);
            return false;
        }
        if (typeof (FileReader) != "undefined") {
            let imagePreview = $("#image-cover-thumbnail");
            imagePreview.empty();
            let fileReader = new FileReader();
            fileReader.onload = function (element) {
                $("<img>", {
                    "src": element.target.result,
                    "id": "thumbnail",
                    "alt": "cover image",
                    "style": "height:100px"
                }).appendTo(imagePreview);
            }
            imagePreview.show();
            fileReader.readAsDataURL(fileData);
            $(".custom-file-label").after(fileData.name);
            coverImage = fileData;
        } else {
            alert("Trình duyệt không hỗ trợ đọc file.");
        }
    });
})(jQuery);
