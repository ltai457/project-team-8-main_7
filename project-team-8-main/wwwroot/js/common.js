$(document).ready(function () {
  // 判断用户是否已经登录
  const $Login = $(".Login");
  const $userImg = $(".userImg");
  const $profileadmin = $("#profileadmin");

  if (sessionStorage.getItem("token")) {
    $Login.css("display", "none");
    // 显示隐藏
    if (sessionStorage.getItem("role") === "Admin") {
      $userImg.css("display", "none");
      var adminProfileButton = $('<a class="AdminProfile">Admin Profile</a>');
      $(".nav").append(adminProfileButton);
    } else if (sessionStorage.getItem("role") === "Student") {
      $userImg.css("display", "block");
      $profileadmin.css("display", "none");
    } else if (sessionStorage.getItem("role") === "Visitor") {
      $userImg.css("display", "block");
      $profileadmin.css("display", "none");
    }

    $('.AdminProfile').click(function () {
      if (location.href.split("/").slice(-1)[0] === 'homePage.html') {
        location.href = './static/ProfileAdmin.html'
      }else{
        location.href = './ProfileAdmin.html'
      }
    })

    // 跳转
    $userImg.click(function () {
      if (location.href.split("/").slice(-1)[0] === 'homePage.html') {
        if (sessionStorage.getItem("role") === "Admin") {
          location.href = "./static/ProfileAdmin.html";
        } else if (sessionStorage.getItem("role") === "Student") {
          location.href = "./static/ProfileStudent.html";
        } else if (sessionStorage.getItem("role") === "Visitor") {
          location.href = "./static/Profile.html";
        }
      } else {
        if (sessionStorage.getItem("role") === "Admin") {
          location.href = "../static/ProfileAdmin.html";
        } else if (sessionStorage.getItem("role") === "Student") {
          location.href = "../static/ProfileStudent.html";
        } else if (sessionStorage.getItem("role") === "Visitor") {
          location.href = "../static/Profile.html";
        }
      }


     
    });
  } else {
    $Login.on("click", function () {
      location.href = "./static/login.html";
    });
    $Login.css("display", "block");
    $profileadmin.css("display", "none");
  }
  // Visitor Admin Student
});
