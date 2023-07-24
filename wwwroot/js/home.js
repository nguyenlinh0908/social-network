const REACT = {
  LIKE: 0,
  COMMENT: 1,
  SHARE: 2,
};

$(window).scroll(() => {
  if (
    $(window).scrollTop() + $(window).height() >=
    $("#new-feed").offset().top + $("#new-feed").height()
  ) {
    loadMorePost();
  }
});

const loadMorePost = () => {
  let lastPost = $("li.like-post").last();
  let lastId = $(lastPost).data("post");

  $.ajax({
    url: "/home/getposts",
    method: "POST",
    data: {
      startId: lastId,
      limit: 9,
    },
  })
    .done(({data:result}) => {
      if (result.length > 0) {
        let newFeed = "";
        result.forEach((item, idx) => {
          const { likeQuantity, commentQuantity } =
            mappingLikeAndCommentQuantity(item?.reaction);
          const liked = checkLikedPost(item?.reaction, item?.owner);

          console.log("likedQuantity :>> ", likeQuantity);
          console.log(liked);
          console.log("item?.post?.id :>> ", item?.post?.id);
          newFeed += `
          <div class="d-flex p-3 border-bottom">
              <img
                src="https://mdbcdn.b-cdn.net/img/Photos/Avatars/img (29).webp"
                class="rounded-circle"
                height="50"
                alt="Avatar"
                loading="lazy"
              />
              <div class="d-flex w-100 ps-3">
              <div>
                  <a href="">
                    <h6 class="text-body">
                      ${item?.owner?.firstName} ${item?.owner?.lastName}
                      <span class="small text-muted font-weight-normal"
                        >tại ${item?.place?.title}</span
                      >
                      <span class="small text-muted font-weight-normal"> • </span>
                      <span class="small text-muted font-weight-normal"></span>
                      <span><i class="fas fa-angle-down float-end"></i></span>
                    </h6>
                  </a>
                  <p style="line-height: 1.2">
                    ${item?.post?.content}
                  </p>

                  <div
                    id="${`carouselExampleIndicators${idx}`}"
                    class="carousel slide"
                    data-bs-ride="carousel"
                  >
                    <div class="carousel-indicators">
                      ${item?.media.map((me, ci) => {
                        return `<button
                          type="button"
                          data-bs-target="#${`carouselExampleIndicators${idx}`}"
                          data-bs-slide-to=${ci}
                          class="active"
                          aria-current="true"
                          aria-label="Slide ${ci}"
                        ></button>`;
                      })}
                    </div>
                    <div class="carousel-inner">
                    ${item?.media.map((me, ci) => {
                      return `<div class="carousel-item ${
                        ci == 0 && "active"
                      }" style="max-height: 585px">
                        <img
                          src="${String(me?.url).replace("wwwroot", "")}"
                          class="d-block w-100"
                          alt=${me?.type}
                        />
                      </div>`;
                    })}                
                    </div>
                    <button
                      class="carousel-control-prev"
                      type="button"
                      data-bs-target="#${`carouselExampleIndicators${idx}`}"
                      data-bs-slide="prev"
                    >
                      <span
                        class="carousel-control-prev-icon"
                        aria-hidden="true"
                      ></span>
                      <span class="visually-hidden">Previous</span>
                    </button>
                    <button
                      class="carousel-control-next"
                      type="button"
                      data-bs-target="#${`carouselExampleIndicators${idx}`}"
                      data-bs-slide="next"
                    >
                      <span
                        class="carousel-control-next-icon"
                        aria-hidden="true"
                      ></span>
                      <span class="visually-hidden">Next</span>
                    </button>
                  </div>

                  <ul
                  class="list-unstyled d-flex justify-content-start gap-3 mb-0 pe-xl-5 "
                >
                  <li data-post="${item?.post?.id}" class="like-post">
                    <img src="${
                      liked
                        ? "/assets/images/heart.svg"
                        : "/assets/images/heart-border.svg"
                    }" alt="" /><span
                      class="small ps-2"
                      >${likeQuantity}</span
                    >
                  </li>
                  <!--
                  <li data-post="${item?.post?.id}" class="comment-post">
                    <img src="/assets/images/comment.svg" alt="" />
                   <span
                      class="small ps-2"
                      >${commentQuantity}</span
                    >
                  </li>
                  <li data-post="${item?.post?.id}" class="share-post">
                    <img src="/assets/images/share.svg" alt="" />
                    <span class="small ps-2"> 7</span>
                  </li>
                  -->
                </ul>
                </div>
              </div>
            </div>
          `;
        });

        $("#new-feed").append(newFeed);
        $("li.like-post").on("click", (e) => {
          const itemTarget = e.currentTarget;
          const postId = $(itemTarget).data("post");
          reaction(itemTarget, postId, REACT.LIKE);
        });
      }
    })
    .fail((err) => {
      console.log("err :>> ", err);
    })
    .always(() => {
      console.log("scroll");
    });
};

const place = () => {
  $.get("/home/getallplaces")
    .done((result) => {
      let placeHtml = "";
      result?.data.map((place) => {
        placeHtml += `<option value="${place.id}-${place.title}"></option>`;
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

const createPost = () => {
  $("#c-post").on("click", (e) => {
    let mediaUrls = localStorage.getItem("uploadUrl");
    const content = $("#post-content").val();
    let placeId = $("#place-list").val();
    placeId = placeId.split("-");
    placeId = placeId[0];
    mediaUrls = mediaUrls.split(",");

    $.ajax({
      url: "/createpost",
      method: "POST",
      data: {
        userId: 6,
        placeId,
        content,
        mediaUrls,
      },
    })
      .done((data) => {
        if (!alert("Tạo bài viết thành công")) location.reload();

        console.log("data :>> ", data);
      })
      .fail((err) => {
        alert("Tạo bài viết không thành công");
      })
      .always(() => {
        localStorage.removeItem("uploadUrl");
      });
  });
};

const mappingLikeAndCommentQuantity = (items = []) => {
  let likeQuantity = 0;
  let commentQuantity = 0;

  if (items.length <= 0) {
    return { likeQuantity, commentQuantity };
  }

  items.forEach((record) => {
    if (record?.reaction == REACT.LIKE) {
      ++likeQuantity;
    }

    if (record?.reaction == REACT.COMMENT) {
      ++commentQuantity;
    }
  });
  return { likeQuantity, commentQuantity };
};

const checkLikedPost = (reaction = [], user) => {
  const isLiked = reaction.some((react) => {
    return react?.userId == user?.id;
  });
  return isLiked;
};

const getPosts = () => {
  $.ajax({
    url: "/home/getposts",
    method: "POST",
    data: {
      startId: 0,
      limit: 9,
    },
  })
    .done(({ data: result }) => {
      if (result.length > 0) {
        let newFeed = "";
        result.forEach((item, idx) => {
          const { likeQuantity, commentQuantity } =
            mappingLikeAndCommentQuantity(item?.reaction);
          const liked = checkLikedPost(item?.reaction, item?.owner);

          console.log("likedQuantity :>> ", likeQuantity);
          console.log(liked);
          console.log("item?.post?.id :>> ", item?.post?.id);
          newFeed += `
          <div class="d-flex p-3 border-bottom">
              <img
                src="https://mdbcdn.b-cdn.net/img/Photos/Avatars/img (29).webp"
                class="rounded-circle"
                height="50"
                alt="Avatar"
                loading="lazy"
              />
              <div class="d-flex w-100 ps-3">
              <div>
                  <a href="">
                    <h6 class="text-body">
                      ${item?.owner?.firstName} ${item?.owner?.lastName}
                      <span class="small text-muted font-weight-normal"
                        >tại ${item?.place?.title}</span
                      >
                      <span class="small text-muted font-weight-normal"> • </span>
                      <span class="small text-muted font-weight-normal"></span>
                      <span><i class="fas fa-angle-down float-end"></i></span>
                    </h6>
                  </a>
                  <p style="line-height: 1.2">
                    ${item?.post?.content}
                  </p>

                  <div
                    id="${`carouselExampleIndicators${idx}`}"
                    class="carousel slide"
                    data-bs-ride="carousel"
                  >
                    <div class="carousel-indicators">
                      ${item?.media.map((me, ci) => {
                        return `<button
                          type="button"
                          data-bs-target="#${`carouselExampleIndicators${idx}`}"
                          data-bs-slide-to=${ci}
                          class="active"
                          aria-current="true"
                          aria-label="Slide ${ci}"
                        ></button>`;
                      })}
                    </div>
                    <div class="carousel-inner">
                    ${item?.media.map((me, ci) => {
                      return `<div class="carousel-item ${
                        ci == 0 && "active"
                      }" style="max-height: 585px">
                        <img
                          src="${String(me?.url).replace("wwwroot", "")}"
                          class="d-block w-100"
                          alt=${me?.type}
                        />
                      </div>`;
                    })}                
                    </div>
                    <button
                      class="carousel-control-prev"
                      type="button"
                      data-bs-target="#${`carouselExampleIndicators${idx}`}"
                      data-bs-slide="prev"
                    >
                      <span
                        class="carousel-control-prev-icon"
                        aria-hidden="true"
                      ></span>
                      <span class="visually-hidden">Previous</span>
                    </button>
                    <button
                      class="carousel-control-next"
                      type="button"
                      data-bs-target="#${`carouselExampleIndicators${idx}`}"
                      data-bs-slide="next"
                    >
                      <span
                        class="carousel-control-next-icon"
                        aria-hidden="true"
                      ></span>
                      <span class="visually-hidden">Next</span>
                    </button>
                  </div>

                  <ul
                  class="list-unstyled d-flex justify-content-start gap-3 mb-0 pe-xl-5 "
                >
                  <li data-post="${item?.post?.id}" class="like-post">
                    <img src="${
                      liked
                        ? "/assets/images/heart.svg"
                        : "/assets/images/heart-border.svg"
                    }" alt="" /><span
                      class="small ps-2"
                      >${likeQuantity}</span
                    >
                  </li>
                  <!--
                  <li data-post="${item?.post?.id}" class="comment-post">
                    <img src="/assets/images/comment.svg" alt="" />
                   <span
                      class="small ps-2"
                      >${commentQuantity}</span
                    >
                  </li>
                  <li data-post="${item?.post?.id}" class="share-post">
                    <img src="/assets/images/share.svg" alt="" />
                    <span class="small ps-2"> 7</span>
                  </li>
                  -->
                </ul>
                </div>
              </div>
            </div>
          `;
        });

        $("#new-feed").html(newFeed);
        $("li.like-post").on("click", (e) => {
          const itemTarget = e.currentTarget;
          const postId = $(itemTarget).data("post");
          reaction(itemTarget, postId, REACT.LIKE);
        });
      }
    })
    .fail((err) => {
      console.log(err);
    })
    .always(() => {
      console.log("getting post");
    });
};

const reaction = (e, postId, react) => {
  $.ajax({
    url: "/react",
    method: "POST",
    data: {
      postId: Number(postId),
      react: react,
    },
  })
    .done((data) => {
      const { react, oppositeReact, currentQuantity } = data;
      switch (react) {
        case REACT.LIKE:
          const [reactIcon] = $(e).children("img");
          const [reactQuantity] = $(e).children("span");
          if (oppositeReact) {
            $(reactIcon).attr("src", "assets/images/heart-border.svg");
          } else {
            $(reactIcon).attr("src", "assets/images/heart.svg");
          }

          $(reactQuantity).html(currentQuantity);
          break;
        default:
      }

      getPosts();
    })
    .fail((err) => {
      console.log(err);
    })
    .always(() => {
      console.log("reacting");
    });
};

place();
createPost();
getPosts();
