function Failed() {
        var http = new XMLHttpRequest();
        var url = "Manager.cshtml";
        var data = "module=user&task=loginpage";
        http.open("POST", url, true);

        //Send the proper header information along with the request
        http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");

        http.onreadystatechange = function () {//Call a function when the state changes.
            if (http.readyState === 4 && http.status === 200) {
                if (this.responseText === "Succesvol!") {
                    document.body.innerHTML = this.responseText;
                } else {
                    document.body.innerHTML = "Failed";
                }

            }
        }
        http.send(data);
} 
