(function ($) {
    var categoryData;
    var _categoryService = abp.services.app.category,
        _$modal = $('#CategoryCreateModal'),
        l = abp.localization.getSource('FirstProject'),
        _$form = _$modal.find('form');

    var _$table = $('#CategoryTable');
    var _$categoryTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        ajax: function (data, callback, settings) {
            var filter = $('#CategorySearchForm').serializeFormToObject();
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;
            abp.ui.setBusy(_$table);

            _categoryService.getAll(filter)
                .done(function (result) {
                    categoryData = result.items;
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
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$categorysTable.draw(false)
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
                data: 'id',
                sortable: false
            },
            {
                targets: 2,
                data: 'categoryName',
                sortable: false
            },
            {
                targets: 3,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-category" data-category-id="${row.id}" data-toggle="modal" data-target="#CategoryEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-category" data-category-id="${row.id}" data-category-name="${row.categoryName}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });


    $('.btn-search').on('click', (e) => {
        _$categoryTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$categoryTable.ajax.reload();
            return false;
        }
    });

    //// create
    
    function save() {
        if (!_$form.valid()) {
            return;
        }
        var category = _$form.serializeFormToObject();
        abp.ui.setBusy(_$form);
        _categoryService.create(category, {
            type: "POST"
        })
            .done(function (result) {
                abp.notify.info('Thêm mới danh mục thành công!');
            }).fail(function (result) {
            }).always(function () {
                abp.ui.clearBusy(_$form);
                _$modal.modal('hide');
                _$categoryTable.ajax.reload();
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
    // end create

    // delete
    $(document).on('click', '.delete-category', function () {
        var categoryId = $(this).attr("data-category-id");
        var categoryName = $(this).attr('data-category-name');

        deleteUser(categoryId, categoryName);
    });

    function deleteUser(categoryId, categoryName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('Are You Sure Want To Delete '),
                categoryName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _categoryService.delete({ categoryId: categoryId }, {
                        type: "Delete"
                    })
                        .done(function () {
                            abp.notify.info(l('Successfully Deleted'));
                            _$categoryTable.ajax.reload();
                        });
                }
            }
        );
    }    //end delete


    //edit
    var _$editModal = $('#CategoryEditModal'),
        _$editForm = _$editModal.find('form');
    $(document).on('click', '.edit-category', function (e) {
        var categoryId = $(this).attr("data-category-id");
        var category = categoryData.find(function (e) {
            return e.id == parseInt(categoryId);
        });
        $("#CategoryId").val(category.id);
        $("#CategoryName").val(category.categoryName);
        e.preventDefault();
    });

    abp.event.on('category.edited', (data) => {
        _$categoryTable.ajax.reload();
    });

    function editSave() {
        if (!_$editForm.valid()) {
            return;
        }

        var category = _$editForm.serializeFormToObject();

        abp.ui.setBusy(_$editForm);
        _categoryService.update(category).done(function () {
            _$editModal.modal('hide');
            abp.notify.info(l('Saved Successfully'));
            abp.event.trigger('category.edited', category);
        }).always(function () {
            abp.ui.clearBusy(_$editForm);
        });
    }

    _$editForm.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        editSave();
    });

    _$editForm.find('input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            editSave();
        }
    });

    _$editModal.on('shown.bs.modal', function () {
        _$editForm.find('input[type=text]:first').focus();
    });
})(jQuery);
