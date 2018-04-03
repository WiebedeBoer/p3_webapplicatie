function Login() {
    var email = document.getElementById("email").value;
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    if (re.test(String(email).toLowerCase())) {
        var pass = document.getElementById("password").value;
        var http = new XMLHttpRequest();
        var url = "Manager.cshtml";
        var data = "module=user&task=login&email=" + encodeURIComponent(email) + "&pass=" + encodeURIComponent(pass);
        http.open("POST", url, true);

        //Send the proper header information along with the request
        http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");

        http.onreadystatechange = function () {//Call a function when the state changes.
            if (http.readyState === 4 && http.status === 200) {
                if (this.responseText === "Succesvol!") {
                    location.reload(true);
                } else {
                    document.getElementById("errormessages").innerHTML = "<ul><li>" + this.responseText + "</li></ul>";
                }

            }
        }
        http.send(data);
    } else {

        document.getElementById("errormessages").innerHTML = "<ul><li>Invalid email</li></ul>";
    }
}