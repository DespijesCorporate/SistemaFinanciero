function checkUserAccessModule(user, pass, module) {
    jQuery.ajaxSetup({ async: false, global: false });
    try {
        //var path = urljs + "/Home/CheckUserAccessModule";
        var path = urljs + "/Home/CheckUserAccessModule2";
        var posting = $.post(path, { CodigoUsuario: user, PassUsuario: pass, CodigoModulo: module });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                //var url = '@Url.Action("Index", "Home")';
                var url = urljs;
                if (data[0].usuarioEncontrado > 0)
                {
                    //var url = '@Url.Action("Dashboard", "Home")';
                    var url = urljs+"Home/Dashboard/";
                    window.location.href = url;
                }
                else {
                    $("#wrongCredencial").removeClass('hidden');

                }
            }
        });
        posting.fail(function (data, status, xhr) {
            //console.log(data);
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {
        //RemoveAnimation("body");
        console.log(e);
    }
    jQuery.ajaxSetup({ async: true, global: true });
}

function SetUserData() {
    jQuery.ajaxSetup({ async: false, global: false });
    try {
        var path = urljs + "/Home/GetUserInfo";
        var posting = $.post(path);        
        posting.done(function (data, status, xhr) {
            //console.log(data);
            if (data.length) {
                if(data[0].Accion == 1){
                    $(".userName").text(data[0].Nombre);
                    $(".userCode").text(data[0].Codigo);
                    $("#userSucursal").val(data[0].SucursalId);
                    $("#userCubiculo").val(data[0].PosicionId);
                    $(".LSM").addClass('hide');
                    GetUserRol();
                }
                else{
                    location.href=urljs;
                }
            }
            else{
                location.href=urljs;
            }
        });
        posting.fail(function (data, status, xhr) {
            location.href=urljs;
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {
        //RemoveAnimation("body");
        console.log(e);
    }
    jQuery.ajaxSetup({ async: true, global: true });
}

function GetUserRol() {
    jQuery.ajaxSetup({ async: false, global: false });
    try {
        var path = urljs + "/Home/GetUserRol";
        var posting = $.post(path);
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    for (var i = data.length - 1; i >= 0; i--) {
                        GetUserPrivilegio(data[contador].CodigoRol);
                        contador++;
                    }
                }
                else {
                    GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();
                }
            }
        });
        posting.fail(function (data, status, xhr) {
            console.log(data);
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {
        //RemoveAnimation("body");
        console.log(e);
    }
    jQuery.ajaxSetup({ async: true, global: true });
}

function GetUserPrivilegio(CodigoRol) {
    jQuery.ajaxSetup({ async: false, global: false });
    try {
        var path = urljs + "/Home/GetUserPrivilegio";
        var posting = $.post(path, { CodigoRol: CodigoRol });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    for (var i = data.length - 1; i >= 0; i--) {
                        //$("#LSM_" + data[contador].CodigoPrivilegio).removeClass('hide');
                        $(".LSM_" + data[contador].CodigoPrivilegio).removeClass('hide');
                        contador++;
                    }
                }
                else {
                    GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();
                }
            }
        });
        posting.fail(function (data, status, xhr) {
            console.log(data);
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {
        //RemoveAnimation("body");
        console.log(e);
    }
    jQuery.ajaxSetup({ async: true, global: true });
}
