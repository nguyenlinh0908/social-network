const PAGINATE_LIMIT = 3;

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
                <button class="btn btn-warning">Edit</button>
                <button class="btn btn-danger">Remove</button>
            </td>
        </tr>
      `);
      });
      let totalPage = Math.ceil(result?.totalRecords / result?.limit);
      for (let i = 1; i <= totalPage; ++i) {
        paginateHtml += `<li class="page-item ${
          i == result?.currentPage ? "active" : ""
        }" data-page=${i}><a class="page-link" href="#">${i}</a></li>`;
      }

      $("#place-list").html(placesHtml);
      $("#place-paginate").html(
        ` <li class="page-item ${
          result?.currentPage == 1 && "d-none"
        }" data-page=${
          result?.currentPage > 1 && result?.currentPage - 1
        }><a class="page-link ${
          result?.currentPage == 1 && "d-none"
        }" href="#">Previous</a></li>
        ${paginateHtml}
        <li class="page-item ${
          result?.currentPage == totalPage && "d-none"
        }" data-page="${
          result?.currentPage < totalPage && result?.currentPage + 1
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
