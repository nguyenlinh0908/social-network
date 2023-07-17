const reviewImage = () => {
  $("#files").change(function () {
    const files = $("#files").prop("files");
    if (Object.entries(files).length > 0) {
      let fileContain = "";
      Object.entries(files).map((data) => {
        let file = data[1];
        let url = URL.createObjectURL(file);
        fileContain += `<img id="files-review" class="d-block w-25" src="${url}" alt="" />`;
      });
      $("#file-review-container").html(fileContain);
      uploadFiles(files);
    }
  });
};

const uploadFiles = (files) => {
  const formData = new FormData();
  console.log(files);
  if (files.length <= 0) return;

  for (var i = 0; i <= files.length; i++) {
    formData.append("files", files[i]);
  }

  $.ajax({
    url: "/media/uploads",
    method: "POST",
    processData: false,
    contentType: false,
    data: formData,
  })
    .done((result) => {
      localStorage.setItem("uploadUrl", result);
    })
    .fail((err) => {
      console.log(err);
    })
    .always(() => {
      console.log("upload file");
    });
};

reviewImage();
