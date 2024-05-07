
function ShowLoading(isShow) {
    const loading = document.querySelector("#loading");
    if (isShow) {
        loading.classList.add("loader");

    } else {
        loading.classList.remove("loader");
    }
}
function NextPage(keyword1, page, pageSize) {
    loadData(keyword1, page, pageSize);
};
function Pagination(keyword, currentPage, NumberPage, pageSize) {
    var kw = `'${keyword}'`;
    var str = "";
    if (NumberPage == 0) {
        var str = `<ul class="pagination pagination-sm"></ul>`;
    }
    if (NumberPage > 0) {
        str = `<ul class="pagination pagination-sm">`;
        if (currentPage != 1) {
            str += `<li class="page-item"><a class="page-link" onclick="NextPage(${kw},${currentPage - 1},${pageSize})" href="javascript:void(0);">&laquo;</a></li>`;
        }
        var startPage = 1;
        var endPage = NumberPage;
        if (NumberPage > 5) {
            startPage = Math.max(currentPage - 2, 1);
            endPage = Math.min(startPage + 4, NumberPage);
            if (startPage > 1) {
                str += `<li class="page-item"><a class="page-link" onclick="NextPage(${kw},1,${pageSize})" href="javascript:void(0);">1</a></li>`;
                if (startPage > 2) {
                    str += `<li class="page-item"><span class="page-link">...</span></li>`;
                }
            }
        }
        for (let i = startPage; i <= endPage; i++) {
            if (currentPage === i) {
                str += `<li class="page-item active"><a class="page-link" href="javascript:void(0);">${i}</a></li>`;
            } else {
                str += `<li class="page-item"><a class="page-link" onclick="NextPage(${kw},${i},${pageSize})" href="javascript:void(0);">${i}</a></li>`;
            }
        }

        if (NumberPage > 5 && endPage < NumberPage) {
            if (endPage < NumberPage - 1) {
                str += `<li class="page-item"><span class="page-link">...</span></li>`;
            }
            str += `<li class="page-item"><a class="page-link" onclick="NextPage(${kw},${NumberPage},${pageSize})" href="javascript:void(0);">${NumberPage}</a></li>`;
        }

        if (currentPage != NumberPage) {
            str += ` <li class="page-item"><a class="page-link" onclick="NextPage(${kw},${currentPage + 1},${pageSize})" href="javascript:void(0);">&raquo;</a></li>`;
        }
        str += "</ul></nav>";

    }
    $('#pagination').html(str);
};
function getDate(dateString) {
    var momentTime = moment(dateString);

    var formattedDate = momentTime.format("DD/MM/YYYY");

    return formattedDate;
}
function getDateEpoch(epochTime) {
    // Giả sử dữ liệu được trả về từ Ajax là một số long chứa thời gian Epoch
    //epochTime

    // Chuyển đổi thời gian Epoch thành một đối tượng Moment
    var momentTime = moment(epochTime);

    // Định dạng lại thời gian theo định dạng mong muốn (ví dụ: yyyy-MM-dd HH:mm:ss.SSS)
    var formattedDate = momentTime.format("DD/MM/YYYY HH:mm:ss");

    // Hiển thị hoặc sử dụng biến formattedDate theo cách bạn muốn trong ứng dụng của bạn
    return formattedDate;
}
function getDateEpoch1(epochTimeString) {
    // Tách phần số Epoch từ chuỗi
    var epochString = epochTimeString.substring(6, epochTimeString.length - 2);

    // Chuyển đổi chuỗi số thành một số nguyên
    var epochTime = parseInt(epochString);

    // Chuyển đổi thời gian Epoch thành một đối tượng Moment
    var momentTime = moment.unix(epochTime);

    // Định dạng lại thời gian theo định dạng mong muốn (ví dụ: YYYY-MM-DDTHH:mm:ss)
    var formattedDate = momentTime.format("YYYY-MM-DD HH:mm:ss");

    // Trả về chuỗi định dạng thời gian
    return formattedDate;
}
function formatJsonDate(jsonDate) {
    // Tách lấy số miligiây từ chuỗi
    var ticks = parseInt(jsonDate.replace("/Date(", "").replace(")/", ""));

    // Tạo một đối tượng Date từ số miligiây
    var date = new Date(ticks);

    // Lấy các thành phần của ngày
    var year = date.getFullYear();
    var month = (date.getMonth() + 1).toString().padStart(2, '0'); // Thêm số 0 phía trước nếu cần
    var day = date.getDate().toString().padStart(2, '0'); // Thêm số 0 phía trước nếu cần

    // Định dạng lại chuỗi ngày thành "yyyy-MM-dd"
    var formattedDate = year + '-' + month + '-' + day;

    return formattedDate;
}
function formatJsonDate1(jsonDate) {
    // Tách lấy số miligiây từ chuỗi
    var ticks = parseInt(jsonDate.replace("/Date(", "").replace(")/", ""));

    // Tạo một đối tượng Date từ số miligiây
    var date = new Date(ticks);

    // Lấy các thành phần của ngày
    var year = date.getFullYear();
    var month = (date.getMonth() + 1).toString().padStart(2, '0'); // Thêm số 0 phía trước nếu cần
    var day = date.getDate().toString().padStart(2, '0'); // Thêm số 0 phía trước nếu cần

    // Lấy các thành phần của thời gian
    var hours = date.getHours().toString().padStart(2, '0'); // Thêm số 0 phía trước nếu cần
    var minutes = date.getMinutes().toString().padStart(2, '0'); // Thêm số 0 phía trước nếu cần

    // Định dạng lại chuỗi ngày thành "yyyy-MM-dd"
    var formattedDate = year + '-' + month + '-' + day + ' ' + hours + ':' + minutes;

    return formattedDate;
}
//function getDate(epochTime) {

//    var momentTime = moment(epochTime);

//    var formattedDate = momentTime.format("YYYY-MM-DD");

//    return formattedDate;
//}
//    function Pagination(keyword, currentPage, NumberPage, pageSize) {
//            var kw = `'${keyword}'`;
//    var str = "";
//    if (NumberPage == 0) {
//                var str = `<ul class="pagination pagination-sm"></ul>`;
//            }
//            if (NumberPage > 0) {
//        str = `<ul class="pagination pagination-sm">`;
//    if (currentPage != 1) {
//        str += `<li class="page-item"><a class="page-link" onclick="NextPage(${kw},${currentPage - 1},${pageSize})" href="javascript:void(0);">&laquo;</a></li>`;
//                }
//    var startPage = 1;
//    var endPage = NumberPage;
//                if (NumberPage > 5) {
//        startPage = Math.max(currentPage - 2, 1);
//    endPage = Math.min(startPage + 4, NumberPage);
//                    if (startPage > 1) {
//        str += `<li class="page-item"><a class="page-link" onclick="NextPage(${kw},1,${pageSize})" href="javascript:void(0);">1</a></li>`;
//                        if (startPage > 2) {
//        str += `<li class="page-item"><span class="page-link">...</span></li>`;
//                        }
//                    }
//                }
//    for (let i = startPage; i <= endPage; i++) {
//                    if (currentPage === i) {
//        str += `<li class="page-item active"><a class="page-link" href="javascript:void(0);">${i}</a></li>`;
//                    } else {
//        str += `<li class="page-item"><a class="page-link" onclick="NextPage(${kw},${i},${pageSize})" href="javascript:void(0);">${i}</a></li>`;
//                    }
//                }

//                if (NumberPage > 5 && endPage < NumberPage) {
//                    if (endPage < NumberPage - 1) {
//        str += `<li class="page-item"><span class="page-link">...</span></li>`;
//                    }
//    str += `<li class="page-item"><a class="page-link" onclick="NextPage(${kw},${NumberPage},${pageSize})" href="javascript:void(0);">${NumberPage}</a></li>`;
//                }

//    if (currentPage != NumberPage) {
//        str += ` <li class="page-item"><a class="page-link" onclick="NextPage(${kw},${currentPage + 1},${pageSize})" href="javascript:void(0);">&raquo;</a></li>`;
//                }
//    str += "</ul></nav > ";

//            }
//$('#pagination').html(str);
//        };
//function NextPage(keyword1, page, pageSize) {
//    loadDataDefault(keyword1, page, pageSize);
//};
//function loadDataDefault(keyword, page, pageSize) {
//    ShowLoading(true);
//    $.ajax({
//        url: '@Url.Action("LoadData", "CategoryMovie")',
//        type: 'GET',
//        dataType: 'json',
//        data: { keyword: keyword, page: page, pageSize: pageSize },
//        success: function (response) {
//            ShowLoading(false)
//            var suggestions = $("#table_categories");
//            suggestions.empty();
//            $.each(response.Data, function (index, category) {
//                suggestions.append(`<tr>
//                                        <td><h6 class="fw-600 mb-0">${category.sttTheLoai}</h6></td>
//                                        <td><h6 class="fw-600 mb-1">${category.tenTheLoai}</h6></td>
//                                        <td><button type="button" class="btn btn-primary" onclick="EditCategory(${category.sttTheLoai})">Sửa</button>
//                                        <button type="button" class="btn btn-danger m-1" onclick="DeleteCategory(${category.sttTheLoai})">Xóa</button></td>
//                                    </tr>`);
//            });
//            Pagination(keyword, response.CurrentPage, response.NumberPage, response.PageSize);
//        },
//        error: function (xhr, status, error) {
//            console.error(error);
//        }
//    });
//}
//function loadData(keyword, eventKey, page, pageSize) {
//    ShowLoading(true);
//    $.ajax({
//        url: '@Url.Action("LoadData", "CategoryMovie")',
//        type: 'GET',
//        dataType: 'json',
//        data: { keyword: keyword, page: page, pageSize },
//        success: function (response) {
//            if (response.status) {
//                ShowLoading(false);
//                var ms = document.getElementById('messageNotFind');
//                var suggestions = $("#table_categories");

//                if (keyword.length <= 0 || (eventKey.which === 8)) {
//                    suggestions.empty();
//                    $.each(response.Data, function (index, category) {
//                        suggestions.append(`<tr>
//                                        <td><h6 class="fw-600 mb-0">${category.sttTheLoai}</h6></td>
//                                        <td><h6 class="fw-600 mb-1">${category.tenTheLoai}</h6></td>
//                                        <td><button type="button" class="btn btn-primary" onclick="EditCategory(${category.sttTheLoai})">Sửa</button>
//                                        <button type="button" class="btn btn-danger m-1" onclick="DeleteCategory(${category.sttTheLoai})">Xóa</button></td>
//                                    </tr>`);
//                    });
//                    ms.style.display = "none"
//                } else if (keyword.length > 0) {
//                    suggestions.empty();
//                    $.each(response.Data, function (index, category) {
//                        suggestions.append(`<tr>
//                                        <td><h6 class="fw-600 mb-0">${category.sttTheLoai}</h6></td>
//                                        <td><h6 class="fw-600 mb-1">${category.tenTheLoai}</h6></td>
//                                        <td><button type="button" class="btn btn-primary" onclick="EditCategory(${category.sttTheLoai})">Sửa</button>
//                                        <button type="button" class="btn btn-danger m-1" onclick="DeleteCategory(${category.sttTheLoai})">Xóa</button></td>
//                                    </tr>`);
//                    });
//                    ms.style.display = "none"
//                    if (response.NumberPage == 0 && response.TotalItem == 0) ms.style.display = "block"
//                }
//                Pagination(keyword, response.CurrentPage, response.NumberPage, response.PageSize);
//            }
//        },
//        error: function (error) {
//            console.error(error);
//        }
//    });
//};
//function InsertCategory() {
//    $('#form_category').submit(function (e) {
//        e.preventDefault();
//        var category = {
//            sttTheLoai: $("#idCategory").val(),
//            tenTheLoai: $("#nameCategory").val(),
//            ghiChu: $("#noteCategory").val()
//        };
//        if (category.tenTheLoai != null) {
//            $.ajax({
//                url: '@Url.Action("Add", "CategoryMovie")',
//                type: 'POST',
//                data: category,
//                success: function (response) {
//                    if (response.status) {
//                        Swal.fire({
//                            position: "center-center",
//                            icon: "success",
//                            title: response.message,
//                            showConfirmButton: true,
//                            timer: 1500
//                        }).then((result) => {
//                            if (result.dismiss === Swal.DismissReason.timer || result.isConfirmed) {
//                                location.reload(true);
//                            }
//                        });
//                    } else {
//                        Swal.fire({
//                            position: "center-center",
//                            icon: "error",
//                            title: response.message,
//                            showConfirmButton: true,
//                            timer: 1500
//                        }).then((result) => {
//                            if (result.dismiss === Swal.DismissReason.timer || result.isConfirmed) {
//                                location.reload(true);
//                            }
//                        });
//                    }
//                },
//                error: function () {
//                    alert('Đã xảy ra lỗi khi thêm sản phẩm.');
//                }
//            });
//        }


//    });

//}
//function DeleteCategory(idCategory) {
//    Swal.fire({
//        title: "Bạn chắc chắn?",
//        text: "Bạn sẽ không thể khôi phục khi đã xóa",
//        icon: "warning",
//        showCancelButton: true,
//        cancelButtonColor: "#d33",
//        cancelButtonText: "Đóng",
//        confirmButtonColor: "#3085d6",
//        confirmButtonText: "Xóa thể loại"
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                url: '@Url.Action("Delete", "CategoryMovie")',
//                type: 'POST',
//                data: { idCategory: idCategory },
//                success: function (response) {
//                    if (response.status) {
//                        Swal.fire({
//                            position: "center-center",
//                            icon: "success",
//                            title: response.message,
//                            showConfirmButton: true,
//                            timer: 1500
//                        }).then((result) => {
//                            if (result.dismiss === Swal.DismissReason.timer || result.isConfirmed) {
//                                location.reload(true);
//                            }
//                        });

//                    } else {
//                        console.log("Xóa sản phẩm không thành công.");
//                    }
//                },
//                error: function (error) {
//                    console.error(error);
//                }
//            });
//        }
//    });
//}
//function EditCategory(id) {
//    $.ajax({
//        url: '@Url.Action("Edit", "CategoryMovie")',
//        type: 'GET',
//        dataType: 'json',
//        data: { id: id },
//        success: function (data) {
//            $('#idCategory').val(data.data.sttTheLoai);
//            $('#nameCategory').val(data.data.tenTheLoai);
//            $('#noteCategory').val(data.data.ghiChu);
//        },
//        error: function (error) {
//            console.error(error);
//        }
//    });
//}
//function ShowLoading(isShow) {
//    const loading = document.querySelector("#loading");
//    if (isShow) {
//        loading.classList.add("loader");

//    } else {
//        loading.classList.remove("loader");
//    }
//}



