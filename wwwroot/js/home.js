const place = () => {
  $.get("/home/getallplaces")
    .done((result) => {
      let placeHtml = "";
      result?.data.map((place) => {
        placeHtml += `<option data-place="${place.id}" value="${place.title}"></option>`;
      });

      $("#placeListOptions").html(placeHtml);
    })
    .fail((err) => {
      console.log(err);
    })
    .always(() => {
      console.log("running");
    });
};

place();