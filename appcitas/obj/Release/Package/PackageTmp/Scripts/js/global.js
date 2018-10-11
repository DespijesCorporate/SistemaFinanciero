/*Declaramos las variables*/
var urljs = "http://localhost:51223/";
//var urljs = "http://localhost/BAC/";
var usu_sucursalId = 0;
var usu_posicionId = 0;
var suc;
var formatDate = "D MMM YYYY H:mm:ss";

function checkUserAccess(CodigoPrivilegio){
    try {
        var path = urljs + "/Home/CheckUserAccess";
        var posting = $.post(path, { privilegio: CodigoPrivilegio });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].usuarioEncontrado <= 0) 
                {
                    jQuery.ajaxSetup({ async: false });
                    GenerarErrorAlerta('Usuario sin acceso', "error");
                    goAlert();
                    jQuery.ajaxSetup({ async: true });
                    location.href=urljs;
                }
                else
                {
                    //usu_sucursalId = data[0].SucursalId;
                    //console.log('usu_sucursalId:' + usu_sucursalId);
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
        //console.log(e);
    }
}

function checkUserAccessPrivilegio(CodigoPrivilegio) {
    var privilegio = false;
    try {
        var path = urljs + "/Home/CheckUserAccess";
        var posting = $.post(path, { privilegio: CodigoPrivilegio });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].usuarioEncontrado <= 0) {
                    jQuery.ajaxSetup({ async: false });
                    //GenerarErrorAlerta('Usuario sin acceso', "error");
                    //goAlert();
                    jQuery.ajaxSetup({ async: true });
                    //location.href = urljs;
                    privilegio = false;
                }
                else {
                    //usu_sucursalId = data[0].SucursalId;
                    //console.log('usu_sucursalIdtrue:' + usu_sucursalId);
                    privilegio = true;
                }
            }
            else
            {
                privilegio = false;
            }
        });
        posting.fail(function (data, status, xhr) {
            //console.log(data);
            GenerarErrorAlerta(xhr, "error");
            goAlert();
            privilegio = false;
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {
        //RemoveAnimation("body");
        //console.log(e);
        privilegio = false;
    }
    return privilegio;
}

var languageConf = {
    "lengthMenu": "ver _MENU_ por página",
    "zeroRecords": "No se encontraron resultados ",
    "info": "Página _PAGE_ de _PAGES_ | Registros: _MAX_",
    "infoEmpty": "Ningún registro disponible",
    "infoFiltered": "(filtrado de un total de _MAX_ total registros)",
    "sInfoPostFix": "",
    "sSearch": "Buscar:",
    "sUrl": "",
    "sInfoThousands": ",",
    "sLoadingRecords": "Cargando...",
    "oPaginate": {
        "sFirst": "Primero",
        "sLast": "Último",
        "sNext": "Siguiente",
        "sPrevious": "Anterior"
    },
    "oAria": {
        "sSortAscending": "Activar para ordenar la columna de manera ascendente",
        "sSortDescending": "Activar para ordenar la columna de manera descendente"
    }
};

/*Declaramos las funciones*/

//Genera una alerta de error al estilo bootstrap
function GenerarErrorAlerta(mensaje, IdAlert) {
    IdAlert = "set-alert-id-" + IdAlert; //Concatenamos el Id con la cadena
    $(document).trigger(IdAlert, [
        {
            'message': mensaje,
            'priority': 'danger'
            
        }
    ]);
}

//Genera una alerta de aprobacion al estilo bootstrap
function GenerarSuccessAlerta(mensaje, IdAlert) {
    // send an alert with id='myid'
    IdAlert = "set-alert-id-" + IdAlert; //Concatenamos el Id con la cadena
    $(document).trigger(IdAlert, {
        message: mensaje,
        priority: "success"
    });
}

function goAlert() {
    //Nos coloca en el mensaje de error
    
    $('html,body').animate({
        scrollTop: $("#MyAlerts").offset().top
        //location: $("#MyAlerts").click().mouseenter
        
        
    });

    $('#masTarjetasModal').animate({

        scrollTop: $("#MyAlerts").offset().top
    });
}

function Validar(input, mensaje)
{
    var go = true;
    /*Evalua si el parametro input es array*/
    if (Array.isArray(input)) {
        for (var i = input.length - 1; i >= 0; i--)
        {
            var obj = $.trim($(input[i]).val());
            //errorContainer = $(input[i]).siblings();
            errorContainer = $(input[i]).parent();

            if (obj == '-1' || obj === -1 || obj == "")
            {
                $(errorContainer).find('.validation-error p').text(mensaje[i]).addClass('label label-danger inline-block');
                go = false;
            }
            else
            {
                $(errorContainer).find('.validation-error p').text("").removeClass('label label-danger inline-block');
            }
        }
    }
    else
    {
        var obj = Number($(input).val());
        //errorContainer = $(input).siblings();
        errorContainer = $(input).parent();
        if (obj == '-1' || obj === -1 || obj == "")
        {
            $(errorContainer).find('.validation-error p').text(mensaje).addClass('label label-danger inline-block');
            go = false;
        }
        else
        {
            $(errorContainer).find('.validation-error p').text("").removeClass('label label-danger inline-block');
        }
    }
    return go;
}

function ajaxindicatorstart(text) {
    if (jQuery('body').find('#resultLoading').attr('id') != 'resultLoading') {
        $loader = '<div class="spinner"><div class="rect1"></div><div class="rect2"></div><div class="rect3"></div><div class="rect4"></div><div class="rect5"></div></div>';
        jQuery('body').append('<div id="resultLoading" style="display:none"><div>'+$loader+'<div>' + text + '</div></div><div class="bg"></div></div>');
    }

    jQuery('#resultLoading').css({
        'width': '100%',
        'height': '100%',
        'position': 'fixed',
        'z-index': '10000000',
        'top': '0',
        'left': '0',
        'right': '0',
        'bottom': '0',
        'margin': 'auto'
    });

    jQuery('#resultLoading .bg').css({
        'background': '#000000',
        'opacity': '0.7',
        'width': '100%',
        'height': '100%',
        'position': 'absolute',
        'top': '0'
    });

    jQuery('#resultLoading>div:first').css({
        'width': '250px',
        'height': '75px',
        'text-align': 'center',
        'position': 'fixed',
        'top': '0',
        'left': '0',
        'right': '0',
        'bottom': '0',
        'margin': 'auto',
        'font-size': '16px',
        'z-index': '10',
        'color': '#ffffff'

    });

    jQuery('#resultLoading .bg').height('100%');
    jQuery('#resultLoading').fadeIn(300);
    jQuery('body').css('cursor', 'wait');
}

function ajaxindicatorstop() {
    jQuery('#resultLoading .bg').height('100%');
    jQuery('#resultLoading').fadeOut(300);
    jQuery('body').css('cursor', 'default');
}

function EventAjax() {
    $(document).ajaxStart(function () {
        //console.log('Cargando.....');
        ajaxindicatorstart('Espere un momento...');
    });

    $(document).ajaxError(function (e) {
        //console.log('Se genero un error!');
        //console.log(e);
        ajaxindicatorstop();
        //sendError(e);
    });

    $(document).ajaxStop(function () {
        //console.log('Proceso Terminado');
        ajaxindicatorstop();
    });
}

function LimpiarTabla(TableName) {
    //Limpiamos la tabla y ocultamos los items.
    $(TableName).dataTable().fnClearTable();
    $(TableName).dataTable().fnDestroy();
    $(TableName).DataTable({
        //"sScrollX": '100%',
        "paging": false,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": true,
        "language": languageConf
    });
}

function LimpiarTablaExcel(TableName, Columns, Filename) {
    //Limpiamos la tabla y ocultamos los items.
    $(TableName).dataTable().fnClearTable();
    $(TableName).dataTable().fnDestroy();
    $(TableName).DataTable({
        "paging": false,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": true,
        "language": languageConf,
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'excel',
                text: '<i class="fa fa-file"></i> Exportar a excel',
                className: 'exportExcel',
                filename: Filename,
                exportOptions: {
                    modifier: {
                        page: 'all'
                    },
                    columns: Columns
                }
            }
        ]
    });

}

function LimpiarTablaSimple(TableName) {
    //Limpiamos la tabla y ocultamos los items.
    $(TableName).dataTable().fnClearTable();
    $(TableName).dataTable().fnDestroy();
    $(TableName).DataTable({
        //"sScrollX": '100%',
        "paging": false,
        "lengthChange": true,
        "searching": false,
        "ordering": true,
        "info": true,
        "autoWidth": true,
        "language": languageConf
    });
}

function LoadComboBox(infojson) 
{
    /*animacion de loading*/
    //LoadAnimation("body");

    /*Limpiamos el ComboBox para no mostrar data repetida.*/
    $(infojson.input).html("");

    /*Agregamos 1era opcion que se mostrara en el ComboBox.*/
    $(infojson.input).append('<option value="-1">Ninguno...</option>');
    //console.log(infojson);
    switch (infojson.type) 
    {
        case "POST":
            var data = ""
            if (infojson.data != "")
            {                
                console.log(infojson.data);
                data = JSON.parse(infojson.data);
            }           
            
            //console.log(data);
            /* Send the data using post */
            var posting = $.post( urljs + infojson.url, data );
             
            /* Put the results in a div */
            posting.done(function( data )
            {
                if (data[0].Accion) 
                {                
                    /*Recorremos el objeto objdata y agregamos los datos al ComboBox.*/
                    //for (var i = data.Resultado.length - 1; i >= 0; i--) 
                    for (var i = 0;  i < data.length; i++) 
                    {
                        var option = $(document.createElement('option'));
                        option.text(data[i][infojson.text]);
                        option.val(data[i][infojson.val]);                
                        $(infojson.input).append(option);
                    }
                    $(infojson.input).select2().trigger('change');
                }
                else
                {
                    GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();
                }
            });
            posting.fail(function(data, status, xhr) 
            {
                $(infojson.input).html("");
                $(infojson.input).append('<option value="-1">Ninguno...</option>');
                GenerarErrorAlerta("Error cargando los datos - , " + xhr, "error");
                goAlert();
            });
            posting.always(function(data, status, xhr)
            {
                /*quitamos la animacion*/
                //RemoveAnimation("body");
            });
            break;
        case "GET":
            var getting = $.get(urljs + infojson.url + infojson.data);
            getting.done(function (data) 
            {
                //console.log(data);
                if(data[0].Accion)
                {
                    $(infojson.input).html("");
                    $(infojson.input).append('<option value="-1">Ninguno...</option>');
                    /*Recorremos el objeto objdata y agregamos los datos al ComboBox.*/
                    //for (var i = data.Resultado.length - 1; i >= 0; i--) 
                   for (var i = 0;  i < data.length; i++) 
                    {
                        var option = $(document.createElement('option'));
                        option.text(data[i][infojson.text]);
                        option.val(data[i][infojson.val]);             
                        $(infojson.input).append(option);
                    }
                    $(infojson.input).select2().trigger('change');
                }
                else
                {
                    $(infojson.input).html("");
                    $(infojson.input).append('<option value="-1">Ninguno...</option>');
                    GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();
                }                
            });

            getting.fail(function (data, status, xhr) 
            {
                GenerarErrorAlerta("Error cargando los datos - , " + xhr, "error");
                goAlert();
            });
            getting.always(function(data, status, xhr) 
            {
                /*quitamos la animacion*/
                //RemoveAnimation("body");
            });
            break;
        default:
            //console.log(infojson);
            $(infojson.input).html("");
            $(infojson.input).append('<option value="-1">Ninguno...</option>');
            //RemoveAnimation("body");
            break;
    }
}

//Funcion que limpia inputs.
function LimpiarInput(arrayInput, arrayCombo) {
    if (arrayInput.length > 0) {
        //Recorremos el arreglo con los ID 
        for (var i = 0; i < arrayInput.length; i++) {
            //Limpia el input
            $(arrayInput[i]).val("");
        }
    }

    if (arrayCombo.length > 0) {
        for (var i = arrayCombo.length - 1; i >= 0; i--) {
            $(arrayCombo[i]).val("-1").trigger('change');
        }
    }

}

function limpiarMensajesValidacion($parent){
    $parent.find('.validation-error p').text('');
}

EventAjax();

$("select").select2();
/*Enmascara los caracteres a ingresar en una textarea, no permite caracteres especiales*/ 
$(document).on('keyup', 'textarea', function () {
    $('[data-toggle="tooltip"]').tooltip();
    if (this.value.match(/[^a-zA-Z0-9 ]/g)) {
        this.value = this.value.replace(/[^A-Za-z-,+/*({[@}]).: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/g, '');
    }
});

function emailValido(email) {
    //console.log(email);
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    if( !emailReg.test( email ) ) {
        //console.log('no valido');
        return false;
    } else {
        //console.log('valido');
        return true;
    }
}

function getDate() {
    var day = new Date()
    var dayWrapper = moment(day);
    var dayString = dayWrapper.format(formatDate);

    return dayString;
}


function CheckUserInfo() {
    try {
        var path = urljs + "/sucursales/CheckUserInfo";
        var posting = $.post(path);
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    usu_sucursalId = data[0].SucursalId;
                    usu_posicionId = data[0].PosicionId;
                    console.log('usu_sucursalId:' + usu_sucursalId);
                    console.log('usu_posicionId:' + usu_posicionId);
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
        //console.log(e);
    }
}

function LoadSucursales_modal_session() {
    var infojson = jQuery.parseJSON('{"input": "#id_sucursal", ' +
                                      '"url": "/sucursales/GetAll", ' +
                                      '"val": "SucursalId", ' +
                                      '"type": "POST", ' +
                                      '"data": "", ' +
                                      '"text": "SucursalNombre"}');
    LoadComboBox(infojson);
}


var currentAgencySession = '';
/* Función que trae la agencia a la que esta asignado el ejecutivo */
function getUserAgency(){
    var path = urljs + "/Home/getAgencySession";
    var posting = $.post(path);
    posting.done(function (data, status, xhr) {
        console.log('data: '+data);
        currentAgencySession = data;
    });
    posting.fail(function (data, status, xhr) {
        currentAgencySession = '';
    });
}
/* Función que guarda (en sesión) la agencia a la que esta asignado el ejecutivo */
function setUserAgency(SucursalId){
    var path = urljs + "/Home/registerAgencySession";
    var posting = $.post(path, { SucursalId: SucursalId });
    posting.done(function (data, status, xhr) {
        console.log('setUserAgency');
        console.log(data);
        usu_sucursalId = SucursalId;
        suc = $("#id_sucursal option:selected").text();
        if (suc == "Ninguno...") {
            $("#id_sucursal").val(SucursalId);
            suc = $("#id_sucursal option:selected").text();
            checkUserAgency();
        }
        $("#idsucur").text(suc);

        setUserAgencyname();

    });
}
function setUserAgencyname() {
    var path = urljs + "/Home/registerAgencySessionName";
    SucursalName = $("#id_sucursal option:selected").text();
    var posting = $.post(path, { SucursalName: SucursalName });
    posting.done(function (data, status, xhr) {
        console.log('setUserAgencyName');
        console.log(data);
        usu_sucursalId = SucursalName;
        $("#idsucur").text(SucursalName);

    });
}

function CheckUserInfo2() {
    try {
        var path = urljs + "/AtencionCitas/CheckUserInfo";
        var posting = $.post(path);
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    usu_sucursalId = data[0].SucursalId;
                    usu_posicionId = data[0].PosicionId;
                    console.log('usu_sucursalId:' + usu_sucursalId);
                    console.log('usu_posicionId:' + usu_posicionId);
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
        //console.log(e);
    }
}
/* Función que verifica que el usuario este asignado a una sucursal */
function checkUserAgency() {
    jQuery.ajaxSetup({ async: false, global: false });
    console.log('before get Session')
    getUserAgency();
    console.log('session: '+currentAgencySession)
    console.log('after get Session')
    if( currentAgencySession == '' || currentAgencySession == '-1' || currentAgencySession == null ){
        try {
            var path = urljs + "/sucursales/CheckUserInfo";
            var posting = $.post(path);
            posting.done(function (data, status, xhr) {
                if (data.length) {
                    if (data[0].Accion > 0) {
                        setUserAgency(data[0].SucursalId);
                    }
                    else{
                        $('.confirm_box_suscursal').modal('show').one('click', '#btn_ok', function (e) {
                            var suc = $("#id_sucursal").val();
                            if( suc == '-1' || suc == '0' || suc == '' || suc == null ){
                                $(".validation-error").removeClass('hide');
                            }
                            else{
                                $(".validation-error").addClass('hide');
                                $(".confirm_box_suscursal").modal('hide');
                                $(".confirm_box_suscursal").on('hidden.bs.modal', function (e) {
                                    setUserAgency(suc);
                                });
                            }
                        });
                    }
                }
                else{
                    $('.confirm_box_suscursal').modal('show').one('click', '#btn_ok', function (e) {
                        var suc = $("#id_sucursal").val();
                        if( suc == '-1' || suc == '' || suc == null ){
                            $(".validation-error").removeClass('hide');
                        }
                        else{
                            $(".validation-error").addClass('hide');
                            $(".confirm_box_suscursal").modal('hide');
                            $(".confirm_box_suscursal").on('hidden.bs.modal', function (e) {
                                setUserAgency(suc);
                            });
                        }
                    });
                }
            });
            posting.fail(function (data, status, xhr) {
                setUserAgency('-1');
            });
        }
        catch (e) {
            setUserAgency('-1');
        }
    }
    jQuery.ajaxSetup({ async: true, global: true });
}


function checkUserAgency2() {
    jQuery.ajaxSetup({ async: false, global: false });
    console.log('before get Session')
    getUserAgency();
    console.log('session: '+currentAgencySession)
    console.log('after get Session')

        try {
            var path = urljs + "/sucursales/CheckUserInfo";
            var posting = $.post(path);
            posting.done(function (data, status, xhr) {
                if (data.length) {
                    if (data[0].Accion > 0) {
                        setUserAgency(data[0].SucursalId);
                    }
                    else{
                        $('.confirm_box_suscursal').modal('show').one('click', '#btn_ok', function (e) {
                            var suc = $("#id_sucursal").val();
                            if( suc == '-1' || suc == '0' || suc == '' || suc == null ){
                                $(".validation-error").removeClass('hide');
                            }
                            else{
                                $(".validation-error").addClass('hide');
                                $(".confirm_box_suscursal").modal('hide');
                                $(".confirm_box_suscursal").on('hidden.bs.modal', function (e) {
                                    setUserAgency(suc);
                                });
                            }
                        });
                    }
                }
                else{
                    $('.confirm_box_suscursal').modal('show').one('click', '#btn_ok', function (e) {
                        var suc = $("#id_sucursal").val();
                        if( suc == '-1' || suc == '' || suc == null ){
                            $(".validation-error").removeClass('hide');
                        }
                        else{
                            $(".validation-error").addClass('hide');
                            $(".confirm_box_suscursal").modal('hide');
                            $(".confirm_box_suscursal").on('hidden.bs.modal', function (e) {
                                setUserAgency(suc);
                            });
                        }
                    });
                }
            });
            posting.fail(function (data, status, xhr) {
                setUserAgency('-1');
            });
        }
        catch (e) {
            setUserAgency('-1');
        }
    
    jQuery.ajaxSetup({ async: true, global: true });
}

/****************
function checkUserAgency2() {
    jQuery.ajaxSetup({ async: false, global: false });
    console.log('before get Session')
    getUserAgency();
    console.log('session: '+currentAgencySession)
    console.log('after get Session')

        try {
            var path = urljs + "/Home/CheckUserInfo";
            var posting = $.post(path);
            posting.done(function (data, status, xhr) {
                if (data.length) {
                    if (data[0].Accion > 0) {
                        setUserAgency(data[0].SucursalId);
                    }
                    else{
                        $('.confirm_box_suscursal').modal('show').one('click', '#btn_ok', function (e) {
                            var suc = $("#id_sucursal").val();
                            if( suc == '-1' || suc == '0' || suc == '' || suc == null ){
                                $(".validation-error").removeClass('hide');
                            }
                            else{
                                $(".validation-error").addClass('hide');
                                $(".confirm_box_suscursal").modal('hide');
                                $(".confirm_box_suscursal").on('hidden.bs.modal', function (e) {
                                    setUserAgency(suc);
                                });
                            }
                        });
                    }
                }
                else{
                    $('.confirm_box_suscursal').modal('show').one('click', '#btn_ok', function (e) {
                        var suc = $("#id_sucursal").val();
                        if( suc == '-1' || suc == '' || suc == null ){
                            $(".validation-error").removeClass('hide');
                        }
                        else{
                            $(".validation-error").addClass('hide');
                            $(".confirm_box_suscursal").modal('hide');
                            $(".confirm_box_suscursal").on('hidden.bs.modal', function (e) {
                                setUserAgency(suc);
                            });
                        }
                    });
                }
            });
            posting.fail(function (data, status, xhr) {
                setUserAgency('-1');
            });
        }
        catch (e) {
            setUserAgency('-1');
        }
    
    jQuery.ajaxSetup({ async: true, global: true });
}
****************/
