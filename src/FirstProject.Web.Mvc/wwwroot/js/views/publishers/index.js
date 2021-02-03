(function ($) {
    var publisherData;
    var _publisherService = abp.services.app.publisher,
        _$modal = $('#PublisherCreateModal'),
        l = abp.localization.getSource('FirstProject'),
        _$form = _$modal.find('form');

    var _$table = $('#PublishersTable');
    var _$publishersTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        ajax: function (data, callback, settings) {
            var filter = $('#PublisherSearchForm').serializeFormToObject();
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;
            abp.ui.setBusy(_$table);

            _publisherService.getAll(filter)
                .done(function (result) {
                    publisherData = result.items;
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
                action: () => _$publishersTable.draw(false)
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
                data: 'publisherName',
                sortable: false
            },
            {
                targets: 3,
                data: null,
                sortable: false,
                width: "200px",
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-publisher" data-publisher-id="${row.id}" data-toggle="modal" data-target="#PublisherEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-publisher" data-publisher-id="${row.id}" data-publisher-name="${row.publisherName}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-primary detail-publisher" data-publisher-id="${row.id}" data-publisher-name="${row.publisherName}">`,
                        `       <i class="fas fa-info"></i>`,
                        `        <a href=${abp.appPath + 'Publishers/Detail/' + row.id}>Detail</a>`,
                        '   </button>',
                    ].join('');
                }
            }
        ]
    });


    $('.btn-search').on('click', (e) => {
        _$publishersTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$publishersTable.ajax.reload();
            return false;
        }
    });

    //// create
    
    function save() {
        if (!_$form.valid()) {
            return;
        }
        var publisher = _$form.serializeFormToObject();
        abp.ui.setBusy(_$form);
        _publisherService.create(publisher, {
            type: "POST"
        })
            .done(function (result) {
                abp.notify.info('Thêm mới NXB thành công!');
            }).fail(function (result) {
            }).always(function () {
                abp.ui.clearBusy(_$form);
                _$modal.modal('hide');
                _$publishersTable.ajax.reload();
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
    $(document).on('click', '.delete-publisher', function () {
        var publisherId = $(this).attr("data-publisher-id");
        var publisherName = $(this).attr('data-publisher-name');

        deleteUser(publisherId, publisherName);
    });

    function deleteUser(publisherId, publisherName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('Are You Sure Want To Delete '),
                publisherName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _publisherService.delete({ publisherId: publisherId }, {
                        type: "Delete"
                    })
                        .done(function () {
                            abp.notify.info(l('Successfully Deleted'));
                            _$publishersTable.ajax.reload();
                        });
                }
            }
        );
    }    //end delete


    //edit
    var _$editModal = $('#PublisherEditModal'),
        _$editForm = _$editModal.find('form');
    $(document).on('click', '.edit-publisher', function (e) {
        var publisherId = $(this).attr("data-publisher-id");
        var publisher = publisherData.find(function (e) {
            return e.id == parseInt(publisherId);
        });
        $("#PublisherId").val(publisher.id);
        $("#PublisherName").val(publisher.publisherName);
        e.preventDefault();
    });

    abp.event.on('publisher.edited', (data) => {
        _$publishersTable.ajax.reload();
    });

    function editSave() {
        if (!_$editForm.valid()) {
            return;
        }

        var publisher = _$editForm.serializeFormToObject();

        abp.ui.setBusy(_$editForm);
        _publisherService.update(publisher).done(function () {
            _$editModal.modal('hide');
            abp.notify.info(l('Saved Successfully'));
            abp.event.trigger('publisher.edited', publisher);
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
