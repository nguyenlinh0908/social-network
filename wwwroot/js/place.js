const PAGINATE_LIMIT = 10;

const getPlaces = (pageNumber = 1, limit = PAGINATE_LIMIT) => {
  $.ajax({
    url: "/admin/getplaces",
    method: "POST",
    data: {
      pageNumber,
      limit,
    },
  })
    .done((result) => {
      let placesHtml = "";
      let paginateHtml = "";
      result?.data.forEach((place, i) => {
        return (placesHtml += `
         <tr>
            <th scope="row">${++i}</th>
            <td>${place.title}</td>
            <td>${place.description}</td>
            <td>${place.countryCode}</td>
            <td>Media</td>
            <td>${place.reviewPoint}</td>
            <td>
              <a
                  id="editButton"
                  class="btn btn-warning"
                  data-bs-toggle="modal"
                  href="#edit-location-modal"
                  role="button"
                  data-place-id=${place.id}
                  >Edit</a>

              <a
                  id="removeButton"
                  class="btn btn-danger"
                  data-bs-toggle="modal"
                  href="#deleteModal"
                  role="button"
                  >Remove</a>
            </td>
        </tr>
      `);
      });
      let totalPage = Math.ceil(result?.totalRecords / result?.limit);
      for (let i = 1; i <= totalPage; ++i) {
        paginateHtml += `<li class="page-item ${i == result?.currentPage ? "active" : ""
          }" data-page=${i}><a class="page-link" href="#">${i}</a></li>`;
      }

      $("#place-list").html(placesHtml);
      $("#place-paginate").html(
        ` <li class="page-item ${result?.currentPage == 1 && "d-none"
        }" data-page=${result?.currentPage > 1 && result?.currentPage - 1
        }><a class="page-link ${result?.currentPage == 1 && "d-none"
        }" href="#">Previous</a></li>
        ${paginateHtml}
        <li class="page-item ${result?.currentPage == totalPage && "d-none"
        }" data-page="${result?.currentPage < totalPage && result?.currentPage + 1
        }"><a class="page-link" href="#">Next</a></li>`
      );
      pagination();
    })
    .fail(function (err) {
      console.log("err :>> ", err);
    })
    .always(function () {
      console.log("complete");
    });
};

const pagination = () => {
  $("#place-paginate>li").on("click", (e) => {
    let pageNumber = $(e.currentTarget).data("page");
    getPlaces(pageNumber, PAGINATE_LIMIT);
  });
};

getPlaces();

$('#editButton').click(function () {
  var id = $(this).data('id');
  var title = $(this).data('title');
  var description = $(this).data('description');
  var countryCode = $(this).data('countrycode');

  $('#editId').text(id);
  $('#editTitle').val(title);
  $('#editDescription').val(description);
  $('#editCountryCode').val(countryCode);

  $('#edit-location-modal').modal('show');
});

$('#saveChanges').click(function () {
  var id = $('#editId').val();
  var title = $('#editTitle').val();
  var description = $('#editDescription').val();
  var countryCode = $('#editCountryCode').val();
  $.ajax({
    url: '/Admin/Update',
    type: 'POST',
    data: { id: id, title: title, description: description, countryCode: countryCode },
    success: function (response) {
      $('#edit-location-modal').modal('hide');
      getPlaces();
    },
    error: function (error) {
      console.log(error);
    }
  });
});

$("#removeButton").click(function () {
  var id = $(this).data('id');
  $("#deleteModal").modal("show");
});

$("#confirmDelete").click(function () {
  var id = $('#removeId').val();
  $.ajax({
    type: "POST",
    url: '/Admin/Delete/',
    data: { id: id },
    success: function () {
      $("#deleteModal").modal("hide");
      getPlaces();
    },
    error: function () {
      console.log("Error occurred while deleting the place.");
    }
  });
});

