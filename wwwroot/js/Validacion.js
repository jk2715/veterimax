function email_Input(id) {
    removerAlerta('alertaError');
    var stn = document.getElementById(id).value;
    var validar = stn.search(/[a-zA-z0-9-_\.]+@[a-zA-z0-9]+\.[a-z]{2,3}(\.[a-z]{2,3})?/g);
    if (validar == 0) {
        if (document.getElementById(id).className.includes("marcar rojo")) {
            document.getElementById(id).classList.remove("marcar", "rojo");
        }
        document.getElementById(id).className = "marcar verde";
    }
    else {
        if (document.getElementById(id).className.includes("marcar verde")) {
            document.getElementById(id).classList.remove("marcar", "verde");
        }
        document.getElementById(id).className = "marcar rojo";
    }
}
function cedula_Input(id) {
    removerAlerta('alertaError');
    var stn = document.getElementById(id).value;
    var validar = stn.search(/([0-9]{3})+-([0-9]{7})+-([0-9]{1})\b/g);
    if (validar == 0) {
        if (document.getElementById(id).className.includes("marcar rojo")) {
            document.getElementById(id).classList.remove("marcar", "rojo");
        }
        document.getElementById(id).className = "marcar verde";
    }
    else {
        if (document.getElementById(id).className.includes("marcar verde")) {
            document.getElementById(id).classList.remove("marcar", "verde");
        }
        document.getElementById(id).className = "marcar rojo";
    }
}
function telefono_Input(id) {
    removerAlerta('alertaError');
    var stn = document.getElementById(id).value;
    var validar = stn.search(/(\([0-9]{3}\))+([0-9]{3})+-([0-9]{4})\b/g);
    if (validar == 0) {
        if (document.getElementById(id).className.includes("marcar rojo")) {
            document.getElementById(id).classList.remove("marcar", "rojo");
        }
        document.getElementById(id).className = "marcar verde";
    }
    else {
        if (document.getElementById(id).className.includes("marcar verde")) {
            document.getElementById(id).classList.remove("marcar", "verde");
        }
        document.getElementById(id).className = "marcar rojo";
    }
}
function nombre_Input(id) {
    removerAlerta('alertaError');
    var stn = document.getElementById(id).value;
    var validar = stn.search(/\b[A-Z]+([a-z]{1,20})/g);
    if (validar == 0) {
        if (document.getElementById(id).className.includes("marcar rojo")) {
            document.getElementById(id).classList.remove("marcar", "rojo");
        }
        document.getElementById(id).className = "marcar verde";
    }
    else {
        if (document.getElementById(id).className.includes("marcar verde")) {
            document.getElementById(id).classList.remove("marcar", "verde");
        }
        document.getElementById(id).className = "marcar rojo";
    }
}
function apellido_Input(id) {
    removerAlerta('alertaError');
    var stn = document.getElementById(id).value;
    var validar = stn.search(/\b[A-Z]+([a-z]{1,20})/g);
    if (validar == 0) {
        if (document.getElementById(id).className.includes("marcar rojo")) {
            document.getElementById(id).classList.remove("marcar", "rojo");
        }
        document.getElementById(id).className = "marcar verde";
    }
    else {
        if (document.getElementById(id).className.includes("marcar verde")) {
            document.getElementById(id).classList.remove("marcar", "verde");
        }
        document.getElementById(id).className = "marcar rojo";
    }
}

function ano_Input(id) {
    removerAlerta('alertaError');
    var stn = document.getElementById(id).value;
    var validar = Number(stn);
    if (validar <= 2000) {
        if (document.getElementById(id).className.includes("marcar rojo")) {
            document.getElementById(id).classList.remove("marcar", "rojo");
        }
        document.getElementById(id).className = "marcar verde";
    }
    else {
        if (document.getElementById(id).className.includes("marcar verde")) {
            document.getElementById(id).classList.remove("marcar", "verde");
        }
        document.getElementById(id).className = "marcar rojo";
    }
}

function mes_Input(id) {
    removerAlerta('alertaError');
    var stn = document.getElementById(id).value;
    var validar = Number(stn);
    if (validar <= 12) {
        if (document.getElementById(id).className.includes("marcar rojo")) {
            document.getElementById(id).classList.remove("marcar", "rojo");
        }
        document.getElementById(id).className = "marcar verde";
    }
    else {
        if (document.getElementById(id).className.includes("marcar verde")) {
            document.getElementById(id).classList.remove("marcar", "verde");
        }
        document.getElementById(id).className = "marcar rojo";
    }
}

function dia_Input(id) {
    removerAlerta('alertaError');
    var stn = document.getElementById(id).value;
    var validar = Number(stn);
    if (validar <= 30) {
        if (document.getElementById(id).className.includes("marcar rojo")) {
            document.getElementById(id).classList.remove("marcar", "rojo");
        }
        document.getElementById(id).className = "marcar verde";
    }
    else {
        if (document.getElementById(id).className.includes("marcar verde")) {
            document.getElementById(id).classList.remove("marcar", "verde");
        }
        document.getElementById(id).className = "marcar rojo";
    }
}

function cantidad_Input(id) {
    removerAlerta('alertaError');
    var stn = document.getElementById(id).value;
    var validar = stn.search(/([0-9]{1,5}\.+[0-9]{2})/g);
    if (validar == 0 && stn != "0.00") {
        if (document.getElementById(id).className.includes("marcar rojo")) {
            document.getElementById(id).classList.remove("marcar", "rojo");
        }
        document.getElementById(id).classList.add("marcar","verde");
    }
    else {
        if (document.getElementById(id).className.includes("marcar verde")) {
            document.getElementById(id).classList.remove("marcar", "verde");
        }
        document.getElementById(id).classList.add("marcar","rojo");
    }
}

function hora_Input(id) {
    removerAlerta('alertaError');
    var stn = document.getElementById(id).value;
    var validar = Number(stn);
    if (validar > -1 && validar < 24) {
        if (document.getElementById(id).className.includes("marcar rojo")) {
            document.getElementById(id).classList.remove("marcar", "rojo");
        }
        document.getElementById(id).classList.add("marcar", "verde");
    }
    else {
        if (document.getElementById(id).className.includes("marcar verde")) {
            document.getElementById(id).classList.remove("marcar", "verde");
        }
        document.getElementById(id).classList.add("marcar", "rojo");
    }
}

function minuto_Input(id) {
    removerAlerta('alertaError');
    var stn = document.getElementById(id).value;
    var validar = stn.search(/([0-9]{2})\b/g);
    if (validar == 0 && validar < 60) {
        if (document.getElementById(id).className.includes("marcar rojo")) {
            document.getElementById(id).classList.remove("marcar", "rojo");
        }
        document.getElementById(id).classList.add("marcar", "verde");
    }
    else {
        if (document.getElementById(id).className.includes("marcar verde")) {
            document.getElementById(id).classList.remove("marcar", "verde");
        }
        document.getElementById(id).classList.add("marcar", "rojo");
    }
}

function dropDown_Input(id) {
    var stn = document.getElementById(id).value;    
    if (stn > 0) {
        if (document.getElementById(id).className.includes("marcar rojo")) {
            document.getElementById(id).classList.remove("marcar", "rojo");
        }
        document.getElementById(id).classList.add("marcar", "verde");
    }
    else {
        if (document.getElementById(id).className.includes("marcar verde")) {
            document.getElementById(id).classList.remove("marcar", "verde");
        }
        document.getElementById(id).classList.add("marcar", "rojo");
    }
}

function validarFormulario(id) {
    var modal = document.getElementById(id);
    var textBoxes = modal.getElementsByTagName("input");
    var dropdown = modal.getElementsByTagName("select");
    var eval = true;
    var b = true;
    for (var i = 0; i < textBoxes.length; i++) {
        if (textBoxes[i].value == "") {
            eval = false;
        }
    }
    for (var i = 0; i < dropdown.length; i++) {
        if (dropdown[i].options[dropdown[i].selectedIndex].text == "--Seleccionar--") {
            eval = false;
        }
    }
    if (modal.getElementsByClassName("marcar rojo").length > 0) {
        document.getElementById("alertaError").className = "alert alert-danger";
        document.getElementById("alertaError").innerHTML = "Debe llenar todos los campos correctamente.";
        eval = false;
        b = false;
    }
    if (eval == false && b == true) {
        document.getElementById("alertaError").className = "alert alert-danger";
        document.getElementById("alertaError").innerHTML = "Debe llenar todos los campos obligatorios.";
    }
    if (eval == true) {
        document.getElementById("alertaExito").className = "alert alert-success";
        document.getElementById("alertaExito").innerHTML = "Se han guardado los datos exitosamente!";
    }
    return eval;
}

function removerAlerta(id) {
    document.getElementById(id).className = "";
    document.getElementById(id).innerHTML = "";
}