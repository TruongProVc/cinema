$('#keyword').keyup(function () {
    var searchField = $('#keyword').val();
    var expression = new RegExp(searchField, "i");
    $('.tt-dataset').remove(); // Clear previous search results

    $.ajax({
        type: "GET",
        url: "/Search/Autocomplete",
        success: function (response) {
            var data = JSON.parse(response);
            console.log(data);
            if (searchField != "") {
                var html_Body = `<div class="tt-dataset tt-dataset-states"></div>`;
                $('.tt-menu').append(html_Body);
            }
            $.each(data, function (index, item) {
                if (expression.test(item.tenPhim) && searchField != "") {
                    var html_Search = ` <div class="man-section tt-suggestion tt-selectable">
						<div class="image-section">
						<a href="/MovieDetail/Index?id=${item.idPhim || ''}"><img  src="${item.hinhDaiDien}"></a>
						</div>
						<div class="description-section">
						  <h1>${item.tenPhim}</h1><p>${item.gioiThieu}</p>
						  </div>
						</div>
						  <div style="clear:both;">
						  </div>    
						</div>`;
                    $('.tt-dataset').append(html_Search);
                }
            });
        }
    });
});
