function createNew() {
    var firstname = document.getElementById("firstname").value,
        lastname = document.getElementById("lastname").value,
        email = document.getElementById("email").value,
        password = document.getElementById("password").value,
        usertype = document.getElementById("usertype").value,
        activated = document.getElementById("activated").value,
        studieid = document.getElementById("studie").value;
    var http = new XMLHttpRequest();
    var url = "Manager.cshtml";
    var data;
    if (usertype == 0) {
        data = "module=user&task=createUser&firstname=" + encodeURIComponent(firstname) + "&lastname=" + encodeURIComponent(lastname) + "&email=" + encodeURIComponent(email) + "&password=" + encodeURIComponent(password) + "&activated=" + encodeURIComponent(activated) + "&studieID=" + encodeURIComponent(studieid);
    } else {
        if (studieid === "") {
            studieid = 0;
        }
        data = "module=user&task=createAdmin&firstname=" + encodeURIComponent(firstname) + "&lastname=" + encodeURIComponent(lastname) + "&email=" + encodeURIComponent(email) + "&password=" + encodeURIComponent(password) + "&activated=" + encodeURIComponent(activated) + "&studieID=" + encodeURIComponent(studieid) + "&userType=" + encodeURIComponent(usertype);
    }

    http.open("POST", url, true);

    //Send the proper header information along with the request
    http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");

    http.onreadystatechange = function () {//Call a function when the state changes.
        if (http.readyState === 4 && http.status === 200) {
            if (this.responseText === "Succesvol!") {
                location.reload(true);
            } else {
                alert(this.responseText);
            }

        } else {
            Failed();
        }
    }
    http.send(data);
}
