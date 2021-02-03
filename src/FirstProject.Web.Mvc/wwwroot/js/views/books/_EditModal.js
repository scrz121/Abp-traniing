(function ($) {
    var originImageSrc = $("#editThumbnail").attr('src');
    var coverImage = null;
    var _bookService = abp.services.app.book,
        l = abp.localization.getSource('FirstProject'),
        _$modal = $('#BookEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        let formData = new FormData(_$form[0]);
        formData.set("CoverImageFile", coverImage);

        abp.ui.setBusy(_$form);
        abp.ajax({
            url: abp.appPath + 'api/services/app/Book/Update',
            data: formData,
            type: 'Put',
            processData: false,
            contentType: false
        }).done(function (result) {
            abp.notify.info(l('Sửa sách thành công!'));
        }).always(function () {
            abp.ui.clearBusy(_$form);
            _$modal.modal('hide');
            abp.event.trigger("book.edited");
        });
    }

    _$form.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        save();
    });

    _$form.find('input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            save();
        }
    });

    _$modal.on('shown.bs.modal', function () {
        _$form.find('input[type=text]:first').focus();
    });
    $("#input-edit-cover-image").bind("change", function () {
        let fileData = $(this).prop("files")[0];
        let math = ["image/png", "image/jpg", "image/jpeg"];
        if (!fileData) {
            $("#editThumbnail").attr("src", originImageSrc);
            coverImage = null;
            return false;
        }
        if ($.inArray(fileData.type, math) === -1) {
            alert("Kiểu file không hợp lệ, chỉ chấp nhận jpg & png");
            $(this).val(null);
            $("#editThumbnail").attr("src", originImageSrc);
            coverImage = null;
            return false;
        }
        if (typeof (FileReader) != "undefined") {
            let imagePreview = $("#image-edit-thumbnail");
            imagePreview.empty();
            let fileReader = new FileReader();
            fileReader.onload = function (element) {
                $("<img>", {
                    "src": element.target.result,
                    "id": "editThumbnail",
                    "alt": "cover image",
                    "style": "height:100px"
                }).appendTo(imagePreview);
            }
            imagePreview.show();
            fileReader.readAsDataURL(fileData);
            coverImage = fileData;
        } else {
            coverImage = null;
            alert("Trình duyệt không hỗ trợ đọc file.");
        }
    });
})(jQuery);
