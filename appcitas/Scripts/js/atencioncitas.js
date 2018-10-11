$(document).ready(function () {
    checkUserAccess('ADCC');
    ComboBoxDesactivar(true);
    var hoy = moment(new Date()).format('YYYY-MM-DD');
    var hoy_time = moment(new Date()).format('YYYY-MM-DD HH:mm:ss.SSS');
    $('.numero_m, .numero').mask('####');
    $('.cuentaFormato_m, .cuentaFormato').mask('################');
    $('.telefono').mask('########');
    $('.id').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Z0-9]/, optional: true } } });
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $("#guardarRazon").on('click', function (e) {
        AgregarRazon();
    });
    

    
    

    $('#guardarHerramienta').on('click', function (e) {
        AgregarHerramientas();
    });

    $('#guardarHerramienta_m').on('click', function (e) {
        AgregarHerramientas_m();
    });

    $('#cmb_sucursal').change(function (e) {
        var id = $(this).val();
        if(id !="-1")
        {
            //console.log(id + '-');
            LoadCubiculos(id);
           
            setTimeout(function () {
                var idcubiculo = $('#userCubiculo').val();
                if (idcubiculo == '0') { idcubiculo = '-1'; }
                $('#cmb_cubiculo').val(idcubiculo).trigger('change');
                if(idcubiculo !="-1")
                {
                    citas_constructor_segmentos_m();
                    CitasPendientes();
                    GetCitasProgramadasDia();
                }                
            }, 1000);
        }
        //else
        //{
        //    LimpiarInput([""], ["#cmb_sucursal", "#cmb_cubiculo"]);
        //}
    });

    $('#cmb_cubiculo').change(function (e) {
        var id = $(this).val();
        var idsucursal = $('#cmb_sucursal').val();
        if (id != "-1")
        {
            GetCitasBySucurcal(idsucursal);
            GetCitasProgramadasCola(id);
        }
    });

    $('#btnComenzarCita').click(function (e) {
        PreComenzarCita();
    });

    $("#cod_tipo_razon").on('change', function (e) {
        //console.log('cod_tipo_razon');
        //$("#listadoExtraContainer").addClass('hide');
        var tipoId = $(this).val();        
        if (tipoId != '' && tipoId != '-1') {            
            checkListadoExtra(tipoId);
            jQuery.ajaxSetup({ async: false });
            citas_constructor_razones(tipoId);
            jQuery.ajaxSetup({ async: true });
        }
        else {
            $("#cod_razonp").empty().append(new Option('No se ha cargado información', '-1'));
        }
    });

    $("#cod_razonp").on('change', function (e) {        
        var tipoId = $(this).val();
        if (tipoId != '' && tipoId != '-1') {
            if (listadoExtraGlobal == 1) {                
                switch (TipoOrigenListadoExtraGlobal) {
                    case 'CONFIG':
                        citas_constructor_listado_extra_config(TipoCodigoListadoExtraGlobal);
                        break;
                    case 'CANAL':
                        citas_constructor_listado_extra_canal();
                        break;
                }

            }
        }
        else {
            $("#cod_razonp").empty().append(new Option('No se ha cargado información', '-1'));
        }
    });

    $('#btnNuevaTarjeta').on('click', function () {

        $('#txt_tarjeta_m').val('');
        $('#txt_cuenta_m').val('');
        $("#emisorId_m").val('');
        $("#txt_marca_m").val('');
        $("#txt_producto_m").val('');
        $("#txt_familia_m").val('');
        $('#cod_segmento_m').val('-1').trigger('change');
        $('#masTarjetasTitle').html('Agregar Tarjetas');
        $('#masTarjetasModal').modal('show');
        $("#txt_cuenta_formato").trigger('dblclick');
        $("#txt_tarjeta_formato").trigger('dblclick');
        //$("#txt_cuenta_formato").val('');
        //$("#txt_tarjeta_formato").val('');
        $('#guardarTarjetas_m').removeClass('hidden');
        $('#txt_tarjeta_m').prop("disabled", false);
        $('#txt_cuenta_m').prop("disabled", false);
        $('#contenedor_tarjetas').addClass('hidden');
    });

    $("#cod_tipo_razon_m").on('change', function (e) {
        //console.log('cod_tipo_razon');
        //$("#listadoExtraContainer").addClass('hide');
        var tipoId = $(this).val();
        if (tipoId != '' && tipoId != '-1') {
            checkListadoExtra_m(tipoId);
            jQuery.ajaxSetup({ async: false });
            citas_constructor_razones_m(tipoId);
            jQuery.ajaxSetup({ async: true });
        }
        else {
            $("#cod_razonp").empty().append(new Option('No se ha cargado información', '-1'));
        }
    });

    $("#cod_razonp_m").on('change', function (e) {
        var tipoId = $(this).val();
        if (tipoId != '' && tipoId != '-1') {
            if (listadoExtraGlobal_m == 1) {
                switch (TipoOrigenListadoExtraGlobal_m) {
                    case 'CONFIG':
                        citas_constructor_listado_extra_config_m(TipoCodigoListadoExtraGlobal_m);
                        break;
                    case 'CANAL':
                        citas_constructor_listado_extra_canal_m();
                        break;
                }

            }
        }
        else {
            $("#cod_razonp_m").empty().append(new Option('No se ha cargado información', '-1'));
        }
    });

    $(".cuentaFormato_m").on('blur', function (e) {
        var cuenta = $(this).val();
        if ((cuenta.length < 15 || cuenta.length > 16) && cuenta.length > 0) {
            if ($(this).hasClass('cuenta_m')) {
                jQuery.ajaxSetup({ async: false });
                //$('#cod_segmento').val('-1').trigger('change');
                jQuery.ajaxSetup({ async: true });
                $("#txt_cuenta_formato_m").val('');
                $("#txt_cuenta_m").val('');
                $("#emisorId_m").val('');
                $("#txt_marca_m").val('');
                $("#txt_producto_m").val('');
                $("#txt_familia_m").val('');
            }
            GenerarErrorAlerta("Formato incorrecto!", "errorTarjetas");
            goAlert();
            //$(this).focus();
        }
        else {
            if (cuenta.length > 0) {
                if ($(this).hasClass('cuenta_m')) {
                    var emisorCuenta = cuenta.substr(0, 10);
                    jQuery.ajaxSetup({ async: false });
                    buscarEmisorCuenta_m(emisorCuenta);
                    jQuery.ajaxSetup({ async: true });
                }
                else {
                    var longitudTarjeta = $("#txt_tarjeta_m").val().length;
                    var tarjetaFormateada = $("#txt_tarjeta_m").val().substr(0, 6) + ' **** **** ' + $("#txt_tarjeta_m").val().substring(longitudTarjeta - 4, longitudTarjeta);

                    $("#txt_tarjeta_m").addClass('hide');
                    $("#txt_tarjeta_formato_m").val(tarjetaFormateada);
                    $("#txt_tarjeta_formato_m").removeClass('hide');
                }
            }
        }
    });

    $(".cuentaFormato").on('blur', function (e) {
        var cuenta = $(this).val();
        if ((cuenta.length < 15 || cuenta.length > 16) && cuenta.length > 0) {
            if ($(this).hasClass('cuenta')) {
                jQuery.ajaxSetup({ async: false });
                $('#cod_segmento').val('-1').trigger('change');
                jQuery.ajaxSetup({ async: true });
                $("#txt_tarjeta").val('');
                $("#emisorId").val('');
                $("#txt_marca").val('');
                $("#txt_producto").val('');
                $("#txt_familia").val('');
            }
            GenerarErrorAlerta("Formato incorrecto!", "error");
            goAlert();
            //$(this).focus();
        }
        else {
            if (cuenta.length > 0) {
                if ($(this).hasClass('cuenta')) {
                    var emisorCuenta = cuenta.substr(0, 10);
                    jQuery.ajaxSetup({ async: false });
                    buscarEmisorCuenta(emisorCuenta);
                    jQuery.ajaxSetup({ async: true });
                }
                else {
                    var longitudTarjeta = $("#txt_tarjeta").val().length;
                    var tarjetaFormateada = $("#txt_tarjeta").val().substr(0, 6) + ' **** **** ' + $("#txt_tarjeta").val().substring(longitudTarjeta - 4, longitudTarjeta);

                    $("#txt_tarjeta").addClass('hide');
                    $("#txt_tarjeta_formato").val(tarjetaFormateada);
                    $("#txt_tarjeta_formato").removeClass('hide');
                }
            }
        }
    });

    $("#txt_cuenta_formato_m").on('dblclick', function (e) {
        
        $("#txt_cuenta_formato_m").addClass('hide');
        $("#txt_cuenta_m").removeClass('hide');
        $("#txt_cuenta_m").focus();
    });

    $("#txt_tarjeta_formato_m").on('dblclick', function (e) {
        $("#txt_tarjeta_formato_m").addClass('hide');
        $("#txt_tarjeta_m").removeClass('hide');
        $("#txt_tarjeta_m").focus();
    });

    $("#txt_cuenta_formato").on('dblclick', function (e) {
        console.log('Dobleclic');
        $("#txt_cuenta_formato").addClass('hide');
        $("#txt_cuenta").removeClass('hide');
        $("#txt_cuenta").focus();
    });

    $("#txt_tarjeta_formato").on('dblclick', function (e) {
        $("#txt_tarjeta_formato").addClass('hide');
        $("#txt_tarjeta").removeClass('hide');
        $("#txt_tarjeta").focus();
    });

    $('#guardarTarjetas_m').click(function () {
        SaveTarjetas();
        
    });

    $("#guardarRazon_m").on('click', function (e) {
        AgregarRazon_m();
    });

    $('#btnFinalizarTarjeta').on('click', function (e) {
        FinalizarTarjetas();
    });

    $('#btn_guardarCuenta').on('click', function () {
        guardarCuenta();
    });

    $('#btn_guardarTarjeta').on('click', function () {
        guardarTarjeta();
    });

    setInterval(function () {
        //ajaxindicatorstop();
        var idsucursal = $('#cmb_sucursal').val();
        var idcubiculo = $('#cmb_cubiculo').val();
        //GetCitasBySucurcal(idsucursal);
        if(idsucursal != '-1' && idcubiculo != '-1'){
            getAutomaticWalkin(idsucursal);
            //GetCitasProgramadasCola(idcubiculo);
            getAutomaticProgramada(idcubiculo);
            getAutomaticProgramadaDia();
        }
    }, 20000);
});

$(window).on("load", function () {
    // weave your magic here.    
    jQuery.ajaxSetup({ async: false });
    LoadSucursales();

    //LimpiarTabla("#tableContador");
    LoadCitasConteoUsuario();
    


    CheckUserInfo2();

    $("#userSucursal").val(usu_sucursalId);
    $("#userCubiculo").val(usu_posicionId);
    var idSucursal = $('#userSucursal').val();
    if (idSucursal == '0') { idSucursal = '-1'; }    
    setTimeout(function () {                       
        //console.log(idSucursal + ' * ' + idcubiculo);
        $('#cmb_sucursal').val(idSucursal).trigger('change');        
    }, 2000);
    jQuery.ajaxSetup({ async: true });
});

dataCitas = [];
var httpRequestWalkin;
var httpRequest;
var CuentaParaCita = true;
var TarjetaParaCita = true;
var listadoExtraGlobal = 0;
var EtiquetaListadoExtraGlobal = '';
var TipoOrigenListadoExtraGlobal = '';
var TipoCodigoListadoExtraGlobal = '';
var listadoExtraIdGlobal = '';

var listadoExtraGlobal_m = 0;
var EtiquetaListadoExtraGlobal_m = '';
var TipoOrigenListadoExtraGlobal_m = '';
var TipoCodigoListadoExtraGlobal_m = '';
var listadoExtraIdGlobal_m = '';

var citaEstado = 0; //1 = cita en proceso / 0 = sin cita que atender
var CitaPendiente = false;
var TiempoCitaPendiente = 0;


function LoadCitasConteoUsuario() {

    //var user = HttpContext.Current.Session["usuario"];
    var path = urljs + "atencioncitas/ContadorUsuariosFecha";
    var posting = $.post(path);
    posting.done(function (data, status, xhr) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].Accion != 0) {
                $('#cont_walkin').val(data[i].ContadorWalkin);
                $('#cont_programada').val(data[i].ContadorProg);
            }
        }
    });
}


function LoadCitaRazon(e) {

    var CitaId = $('#hidden_citaId').val();
    var path = urljs + "atencioncitas/CampoCitaRazon";
    var posting = $.post(path, { CitaId: Number(CitaId) });
    posting.done(function (data, status, xhr) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].Accion != 0 && data[i].Accion != null) {
                $('#campocitarazon').removeClass('hidden');
                $('#citarazon').val(data[i].CampoRazon);
            }
            
        }
    });
}


function guardarCuenta(e) {
    var CitaId = $('#hidden_citaId').val();
    var Emisor = $('#emisorId').val();
    var Segmento = $('#cod_segmento').val();
    var Cuenta = $.trim($('#txt_cuenta').val());
    var go = true;

    if (CitaId == '') {
        go = false;
        GenerarErrorAlerta('Debe seleccionar una cita para continuar!', "error");
        goAlert();
    }

    if (Emisor == "") {
        go = false;
        GenerarErrorAlerta('Debe ingresar una cuenta válida para continuar!', "error");
        goAlert();
    }

    if (Segmento == "-1") {
        go = false;
        GenerarErrorAlerta('Debe seleccinar un segmento para continuar!', "error");
        goAlert();
    }

    if (go) {
        var cuentaEncrypted = Cuenta; //ryptoJS.AES.encrypt(Cuenta, 'BACPANAMA');
        //var cuentaEncrypted = CryptoJS.MD5(Cuenta);
        var path = urljs + "atencioncitas/CambiarCuenta";
        var posting = $.post(path, { CitaId: Number(CitaId), Segmento: Segmento, Emisor: Emisor, Cuenta: cuentaEncrypted.toString()});
        posting.done(function (data, status, xhr) {
            //console.log(data);
            if (data.Accion == 1) {
                GenerarSuccessAlerta(data.Mensaje, 'success');
                $('#txt_cuenta').prop("disabled", true);
                $('#btn_guardarCuenta').addClass('hidden');
                CuentaParaCita = true;
            }
            else {
                $('#txt_cuenta').prop("disabled", false);
                $('#btn_guardarCuenta').removeClass('hidden');
                GenerarErrorAlerta(data.Mensaje, "error");
                goAlert();
                CuentaParaCita = false;
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
}



function guardarTarjeta(e) {
    var CitaId = $('#hidden_citaId').val();
    var EmisorTar = 275;//$('#emisorId').val();
    var Segmento = $('#cod_segmento').val();
    var Tarjeta = $.trim($('#txt_tarjeta').val());
    var go = true;

    if (CitaId == '') {
        go = false;
        GenerarErrorAlerta('Debe seleccionar una cita para continuar!', "error");
        goAlert();
    }

    if (EmisorTar == "") {
        go = false;
        GenerarErrorAlerta('Debe ingresar una Tarjet válida para continuar!', "error");
        goAlert();
    }

    if (Segmento == "-1") {
        go = false;
        GenerarErrorAlerta('Debe seleccinar un segmento para continuar!', "error");
        goAlert();
    }

    if (go) {
        var tarjetaEncrypted = Tarjeta; //ryptoJS.AES.encrypt(Cuenta, 'BACPANAMA');
        //var cuentaEncrypted = CryptoJS.MD5(Cuenta);
        var path = urljs + "atencioncitas/CambiarTarjeta";
        var posting = $.post(path, { CitaId: Number(CitaId), Segmento: Segmento, EmisorTar: EmisorTar, Tarjeta: tarjetaEncrypted.toString() });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            if (data.Accion == 1) {
                GenerarSuccessAlerta(data.Mensaje, 'success');
                $('#txt_tarjeta').prop("disabled", true);
                $('#btn_guardarTarjeta').addClass('hidden');
                TarjetaParaCita = true;
            }
            else {
                $('#txt_tarjeta').prop("disabled", false);
                $('#btn_guardarTarjeta').removeClass('hidden');
                GenerarErrorAlerta(data.Mensaje, "error");
                goAlert();
                TarjetaParaCita = false;
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
}


function buscarEmisorCuenta(EmisorCuenta) {
    try {
        var path = urljs + "/citas/CheckEmisorCuenta";
        var posting = $.post(path, { EmisorCuenta: EmisorCuenta });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                console.log(data);
                if (data[0].Accion > 0) {
                    var longitudCuenta = $("#txt_cuenta").val().length;
                    var cuentaFormateada = $("#txt_cuenta").val().substr(0, 6) + ' **** **** ' + $("#txt_cuenta").val().substring(longitudCuenta - 4, longitudCuenta);
                    $("#txt_cuenta").addClass('hide');
                    $("#txt_cuenta_formato").val(cuentaFormateada);
                    $("#txt_cuenta_formato").removeClass('hide');

                    jQuery.ajaxSetup({ async: false });
                    $('#cod_segmento').val(data[0].CitaSegmentoId);
                    $('#cod_segmento').trigger('change');
                    jQuery.ajaxSetup({ async: true });

                    //$("#txt_cuenta_n").val(EmisorCuenta);
                    $("#emisorId").val(data[0].EmisorId);
                    $("#txt_marca").val(data[0].MarcaTarjeta);
                    $("#txt_producto").val(data[0].Producto);
                    $("#txt_familia").val(data[0].Familia);
                }
                else {
                    jQuery.ajaxSetup({ async: false });
                    $('#cod_segmento').val('-1').trigger('change');
                    jQuery.ajaxSetup({ async: true });

                    $("#txt_cuenta").val('');
                    $("#emisorId").val('');
                    $("#txt_marca").val('');
                    $("#txt_producto").val('');
                    $("#txt_familia").val('');
                    GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();
                }
            }
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
            //$("#txt_tarjeta_n").focus();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {
        //RemoveAnimation("body");
        console.log(e);
        //$("#txt_tarjeta_n").focus();
    }
}

function CitasPendientes()
{
    var SucursalId = $('#userSucursal').val();
    var CubiculoId = $('#userCubiculo').val();
    var go = true;
    if (SucursalId == '-1') { go = false; };
    if (CubiculoId == '-1') { go = false; };
    if (go) {
        LimpiarTabla("#tableCitas");
        var path = urljs + "atencioncitas/GetCitasPendientes";
        var posting = $.post(path, { Posicionid: CubiculoId, Sucursalid: SucursalId });
        posting.done(function (data, status, xhr) {            
            if (data.Accion) {
                
                CitaPendiente = true;
                TiempoCitaPendiente = data.MinutosAtencionCitaPendiente;
                $("#hidden_citaId").val(data.CitaId);
                $('#hidden_Identificacion').val(data.CitaIdentificacion);
                PreComenzarCita();
                GenerarSuccessAlerta("Se cargo la cita pendiente!", "success");
                goAlert();
                ControlsCitas(true, 'Cita en proceso!');                
            }
            else {
                ControlsCitas(false, 'No hay citas que mostrar!');
                CitaPendiente = false;
                TiempoCitaPendiente = 0;
            }

            console.log(CitaPendiente);
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
    }

}

function ComboBoxDesactivar(accion)
{
    if (accion) {
        $('#cmb_sucursal').attr('disabled', true);
        $('#cmb_cubiculo').attr('disabled', true);
    }
    else {
        $('#cmb_sucursal').removeAttr('disabled');
        $('#cmb_cubiculo').removeAttr('disabled');
    }
}

function getAutomaticWalkin(id)
{
    httpRequestWalkin = new XMLHttpRequest();
    if (!httpRequestWalkin) {
        //alert('Giving up :( Cannot create an XMLHTTP instance');
        GenerarErrorAlerta('No se puede crear una instancia de XMLHTTP!', "error");
        return false;
    }

    httpRequestWalkin.onreadystatechange = alertContentswalkin;
    httpRequestWalkin.open('POST', urljs + "atencioncitas/GetCitasBySucursal");
    httpRequestWalkin.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequestWalkin.send('sucursalid=' + encodeURIComponent(id));
}

function getAutomaticProgramada(id) {
    httpRequest = new XMLHttpRequest();
    if (!httpRequest) {
        //alert('Giving up :( Cannot create an XMLHTTP instance');
        GenerarErrorAlerta('No se puede crear una instancia de XMLHTTP!', "error");
        return false;
    }
    var Sucursalid = $('#userSucursal').val();
    //console.log(Sucursalid);
    httpRequest.onreadystatechange = alertContentsprogramadas;
    httpRequest.open('POST', urljs + "atencioncitas/GetCitasProgramadasCola");
    httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequest.send('Posicionid=' + encodeURIComponent(id) + '&Sucursalid=' + encodeURIComponent(Sucursalid));
    //httpRequest.send('Sucursalid=' + encodeURIComponent(idSucursal));
}

function getAutomaticProgramadaDia() {
    httpRequestDia = new XMLHttpRequest();
    if (!httpRequestDia) {
        //alert('Giving up :( Cannot create an XMLHTTP instance');
        GenerarErrorAlerta('No se puede crear una instancia de XMLHTTP!', "error");
        return false;
    }
    var CubiculoId = $('#userCubiculo').val();
    var idSucursal = $('#userSucursal').val();
    //console.log(Sucursalid);
    httpRequestDia.onreadystatechange = alertContentsprogramadasDia;
    httpRequestDia.open('POST', urljs + "atencioncitas/GetCitasDia");
    httpRequestDia.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequestDia.send('Posicionid=' + encodeURIComponent(CubiculoId) + '&Sucursalid=' + encodeURIComponent(idSucursal));
    //httpRequest.send('Sucursalid=' + encodeURIComponent(idSucursal));
}

function alertContentswalkin()
{
    if (httpRequestWalkin.readyState === XMLHttpRequest.DONE)
    {
        if (httpRequestWalkin.status === 200)
        {
            var response = JSON.parse(httpRequestWalkin.responseText);
            //console.log('walkin');
            //console.log(response[0]);
            $('#list_citas_walkin').html('');
            var data = response;
            var i = 0;

           


            if (data[i].Accion)
            {
                var item = '<a href="javascript:void(0);" id=' + data[i]["CitaId"] + ' class="list-group-item citasPendientes text-center list-group-item-datos disabled" onClick="showAviso(event)">Siguiente...</a>';
                $('#list_citas_walkin').append(item);
                $('#controles_walkin').removeClass('hidden');
            }
            else
            {
                $('#controles_walkin').addClass('hidden');
                var item = '<a href="#contenedor_principal" class="list-group-item text-center list-group-item-vacio"><span class="label label-warning">No hay citas que mostrar!</span></a>';
                $('#list_citas_walkin').append(item);
            }
            
        }
        else
        {
            console.log('There was a problem with the request.');
        }
    }
}

function alertContentsprogramadas() {
    //console.log(httpRequest);
    if (httpRequest.readyState === XMLHttpRequest.DONE) {
        if (httpRequest.status === 200) {
            var response = JSON.parse(httpRequest.responseText);            
            var data = response;
            //console.log(data.Accion);
            $('#list_citas').html('');
            if(data.Accion)
            {
                var flag = "";
                if (data.CitaFlagClienteIniciaEspera == '1') {
                    flag = " | Cliente llego";
                    var item = '<a href="javascript:void(0);" id=' + data.CitaId + ' class="list-group-item citasPendientes text-center list-group-item-datos" onClick="showAviso(event)">' + data.PosicionId + ' | ' + data.CitaNombre + ' | ' + data.tramite + ' | ' + data.CitaHora + ' | ' + data.CitaEstadoDescripcion + '</a>';
                    $('#list_citas').append(item);
                }
                else {
                    var item = '<a href="#contenedor_principal" class="list-group-item text-center list-group-item-vacio"><span class="label label-warning">No hay citas que mostrar!</span></a>';
                    $('#list_citas').append(item);
                }
                                                
            }
            else
            {               
                var item = '<a href="#contenedor_principal" class="list-group-item text-center list-group-item-vacio"><span class="label label-warning">No hay citas que mostrar!</span></a>';
                $('#list_citas').append(item);                
            }
        }
        else {
            console.log('There was a problem with the request.');
        }
    }
}

function alertContentsprogramadasDia() {
    if (httpRequestDia.readyState === XMLHttpRequest.DONE) {
        if (httpRequestDia.status === 200) {
            var response = JSON.parse(httpRequestDia.responseText);
            //console.log('programadas');
            //console.log(response);
            var data = response;
            if (data[0].Accion) {
                $('#list_citas_dia').html("");
                for (var i = 0; i < data.length; i++) {
                    var item = '<a onClick="showDetails(event)" data-citaid = "' +data[i]["CitaId"]+ '" href="javascript:void(0);" class="list-group-item text-center list-group-item-datos"> ' + data[i]["CitaHora"] + ' | ' + data[i]["CitaEstadoDescripcion"] + '</a>';
                    $('#list_citas_dia').append(item);
                }
                //$('#controles_programada').removeClass('hidden');
            }
            /*else {
                GenerarErrorAlerta(data[0].Mensaje, "error");
                goAlert();
            }*/
        }
        else {
            console.log('There was a problem with the request.');
        }
    }
}

function LoadSucursales() {
    var infojson = jQuery.parseJSON('{"input": "#cmb_sucursal", ' +
		                              '"url": "/sucursales/GetAll", ' +
		                              '"val": "SucursalId", ' +
		                              '"type": "POST", ' +
		                              '"data": "", ' +
		                              '"text": "SucursalNombre"}');
    LoadComboBox(infojson);
}

function LoadCubiculos(IdSuc) {
    

    var infojson = jQuery.parseJSON('{"input": "#cmb_cubiculo", ' +
                                         '"url": "atencionCitas/GetCubiculosBySucursalAtencion/", ' +
                                         '"val": "PosicionId", ' +
                                         '"type": "GET", ' +
                                         '"data": "'+IdSuc+'", ' +
                                         '"text": "posicionDescripcion"}');
    LoadComboBox(infojson);
}

//function LoadCitasConteoUsuario(ArrayData, tableID) {


//    var path = urljs + "atencionCitas/ContadorUsuariosFecha";
//   var posting = $.post(path, dataCitas);
//   posting.done(function ( status, xhr) {

//        var newRow = $(tableID).dataTable().fnAddData([
//         ArrayData['ContadorWalkin'],
//         ArrayData['ContadorProg']
//        ]);


//    });
//        posting.fail(function (status, xhr) {
//            //console.log(data);
//            GenerarErrorAlerta(xhr, "error");
//            goAlert();
//        });
//        posting.always(function (data, status, xhr) {
//            //RemoveAnimation("body");
//        });


    

//var path = urljs + 'atencionCitas/ContadorUsuariosFecha';

//var data = {
//    cmb_cubiculo: $("#cmb_cubiculo").val(),
//}

//var posting = $.post(path, data);

//posting.done(function (status, xhr) {

//});
//posting.fail(function (data, status, xhr) {
//    //console.log(status);
//    });

    

    //var infojson = jQuery.parseJSON('{"input": "#cmb_cubiculo", ' +
    //                                     '"url": "atencionCitas/ContadorUsuariosFecha/", ' +
    //                                     '"val": "UsuarioId", ' +
    //                                     '"type": "GET", ' +
    //                                     '"data": "' + cmb_cubiculo + '", ' +
    //                                     '"text": "Usuario"}');
    //LoadComboBox(infojson);
//}

function GetCitasProgramadasCola(id)
{
    if (id != "-1") {
        LimpiarTabla("#tableCitas");
        var idSucursal = $('#userSucursal').val();
        var path = urljs + "atencioncitas/GetCitasProgramadasCola";
        var posting = $.post(path, { Posicionid: id, Sucursalid: idSucursal });
        posting.done(function (data, status, xhr) {            
            if (data.Accion) {
                $('#list_citas').html("");
                var item = '<a href="javascript:void(0);" id=' + data["CitaId"] + ' class="list-group-item citasPendientes text-center list-group-item-datos" onClick="showAviso(event)">' + data["PosicionId"] + ' | ' + data["CitaNombre"] + ' | ' + data["tramite"] + ' | ' + data.CitaEstadoDescripcion + '</a>';
                $('#list_citas').append(item);
                $('#controles_programada').removeClass('hidden');
            }
            else {
                //GenerarErrorAlerta(data.Mensaje, "error");
                //goAlert();
                $('#controles_programada').addClass('hidden');
            }

        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
    }
}

function GetCitasBySucurcal(id)
{
    if(id != "" || id != "-1")
    {
        LimpiarTabla("#tableCitas");
        var path = urljs + "atencioncitas/GetCitasBySucursal";
        var posting = $.post(path, { sucursalid: Number(id) });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            if (data[0].Accion)
            {
                //var citasCalendar = {};
                //citasCalendar.info = [];
                $('#list_citas_walkin').html("");
                //ControlsCitas(true, 'No hay citas que mostrar!');
                for (var i = 0; i < data.length; i++)
                {
                    var item = '<a href="javascript:void(0);" id=' + data[i]["CitaId"] + ' class="list-group-item citasPendientes text-center list-group-item-datos" onClick="showAviso(event)">Siguiente...</a>';
                    $('#list_citas_walkin').append(item);
                    //$('#controles_walkin').removeClass('hidden');
                    //console.log(item);
                    /*Obtenemos la fecha*/
                    //var datecita = data[i].CitaFecha.split("-");
                    //console.log(datecita);
                    //datecita = new Date(datecita[0], datecita[1]-1, datecita[2]);
                    /*obtenemos la hora*/
                    //var horacita = data[i].CitaHora.split(":");
                    //console.log(horacita);

                    /*citasCalendar.info[i] = {};
                    citasCalendar.info[i].id = data[i]["CitaId"];
                    citasCalendar.info[i].title = data[i]["CitaNombre"];
                    citasCalendar.info[i].start = new Date(datecita.getFullYear(), datecita.getMonth(), datecita.getDate(), horacita[0], horacita[1], horacita[2]);
                    citasCalendar.info[i].end = new Date(datecita.getFullYear(), datecita.getMonth(), datecita.getDate());
                    citasCalendar.info[i].allDay = false;*/
                    
                    //addRow(data[i], "#tableCitas");
                     

                }
                //console.log(JSON.stringify(citasCalendar.info));
                //LoadCalendario(citasCalendar.info);
            }
            else
            {
                //$('#controles_walkin').addClass('hidden');
                //ControlsCitas(false, 'No hay citas que mostrar!');                
                //GenerarErrorAlerta(data[0].Mensaje, "error");
                //goAlert();
            }
            
        });
        posting.fail(function (data, status, xhr) {
            console.log(data);
        });
    }
    else
    {
        GenerarErrorAlerta("Debe seleccionar una sucursal!", "error");
        goAlert();
    }
}

function addRow(ArrayData, tableID) {
   
    var newRow = $(tableID).dataTable().fnAddData([
        ArrayData['CitaId'],
        ArrayData['CitaIdentificacion'],
        ArrayData['CitaNombre'],
        ArrayData['CitaFecha'] + ' | ' + ArrayData['CitaHora'],
       ArrayData['tramite'],
        '<div class="btn-group text-center">' +
        '<button type="button" class="btn btn-primary dropdown-toggle text-center" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fa fa-bars" aria-hidden="true"></i></button>' +
        '<ul class="dropdown-menu centrar-menu text-left">' +            
            "<li><a data-id='" + ArrayData['CitaId'] + "' data-name='" + ArrayData['CitaNombre'] + "' data-identificacion='" + ArrayData['CitaIdentificacion'] + "' title='Comenzar Cita' data-toggle='tooltip' data-placement='left' onClick='ComenzarCita(event)' id='btnComenzarCita" + ArrayData['CitaId'] + "' href='#'><i class='fa fa-clock-o' aria-hidden='true'></i> Comenzar Cita</a></li>" +
            "<li><a data-id='" + ArrayData['CitaId'] + "' data-name='" + ArrayData['CitaNombre'] + "' data-identificacion='" + ArrayData['CitaIdentificacion'] + "' title='Cancelar Cita' data-toggle='tooltip' data-placement='left' onClick='CancelarCita(event)' id='btnCancelarCita" + ArrayData['CitaId'] + "' href='#'><i class='fa fa-ban' aria-hidden='true'></i> Cancelar Cita</a></li>" +
            "<li><a data-id='" + ArrayData['CitaId'] + "' data-name='" + ArrayData['CitaNombre'] + "' data-identificacion='" + ArrayData['CitaIdentificacion'] + "' title='Detalle Cita' data-toggle='tooltip' data-placement='left' onClick='DetalleCita(event)' id='btnDetalleCita" + ArrayData['CitaId'] + "' href='#'><i class='fa fa-info' aria-hidden='true'></i> Detalle</a></li>" +
        '</ul>' +
    '</div>']);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['CitaId']);
    theNode.setAttribute('class', 'text-center');
}

function PreComenzarCita()
{
    LimpiarTabla("#tableCitas");
    var CitaId = $('#hidden_citaId').val();
    var Id = $('#hidden_Identificacion').val();
    var Cubiculo = $('#userCubiculo').val();
    ComenzarCita(Id, CitaId, Cubiculo);
    LoadResultadoCitas();
    LoadCitaRazon();
    citas_constructor_tipos_razon();
    citas_constructor_tipos_razon_m();
    $("#cod_razonp").empty().append(new Option('No se ha cargado información', '-1'));
    LoadHerramientas();
    GetHerramientasByCita(CitaId);
    $('#avisoAtencionModal').modal('hide');
}

function ComenzarCita(Id, CitaId, Cubiculo)
{
   
    //e.stopPropagation();    
    //var Id = $(e.currentTarget).attr('data-identificacion');
    //var CitaId = $(e.currentTarget).attr('data-id');
    //console.log('Comenzar Cita');
    try {
        $('#contenedor_timer').removeClass('hidden');        
        var path = urljs + "atencioncitas/GetCitasById";
        var posting = $.post(path, { CitaId: Number(CitaId) });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            var pathCitas = urljs + "atencioncitas/IniciarCitaFecha";
            var postingCitas = $.post(pathCitas, { CitaId: Number(CitaId), Cubiculo: Cubiculo });
            
            postingCitas.done(function (data, status, xhr) {

                dataCitasI = [];
                dataCitasI = data;

                if (CitaPendiente) {
                    dataCitasI.Accion = 1;
                }
            
                //console.log(dataCitas.length);
                if (dataCitasI.Accion == 1) {
                    if (dataCitas.Accion == 1) {

                        if (dataCitas.CitaTarjeta == null || dataCitas.CitaTarjeta == "") {
                            $('#txt_tarjeta').prop("disabled", false);
                            $('#btn_guardarTarjeta').removeClass('hidden');
                            TarjetaParaCita = false;
                        }
                        else {
                            $('#txt_tarjeta').prop("disabled", true);
                            $('#btn_guardarTarjeta').addClass('hidden');
                            TarjetaParaCita = true;
                        }


                        //console.log(dataCitas.CitaCuenta + ' hola :(');
                        if (dataCitas.CitaCuenta == null || dataCitas.CitaCuenta == "") {
                            $('#txt_cuenta').prop("disabled", false);
                            $('#btn_guardarCuenta').removeClass('hidden');
                            CuentaParaCita = false;
                        }
                        else {
                            $('#txt_cuenta').prop("disabled", true);
                            $('#btn_guardarCuenta').addClass('hidden');
                            CuentaParaCita = true;
                        }
                        var cuentaDecrypted = dataCitas.CitaCuenta; //CryptoJS.AES.decrypt(dataCitas.CitaCuenta, 'BACPANAMA');
                        var tarjetaDecrypted = dataCitas.CitaTarjeta; //CryptoJS.AES.decrypt(dataCitas.CitaTarjeta, 'BACPANAMA');

                        var segCitas = (dataCitas.duracion * 60);
                        var tiempoMuerto = (dataCitas.TiempoMuerto);
                        var SucHorarioCorrido = dataCitas.SucHorarioCorrido;
                        var timerGo = true;
                        if (CitaPendiente) {
                            console.log('Es una cita pendiente');
                            if (TiempoCitaPendiente <= 0) {
                                //GenerarErrorAlerta('El tiempo de atencion para esta cita sobrepasa por ' + Math.abs(TiempoCitaPendiente) + ' Minutos el tiempo de duración de la cita!', "error");
                                //goAlert();
                                $('#titleMessageTimerOut').text('El tiempo de atencion para esta cita sobrepasa por ' + Math.abs(TiempoCitaPendiente) + ' Minutos el tiempo de duración de la cita!');
                                segCitas = 60;
                                timerGo = false;
                                $('#contenedor_mensaje_timer').removeClass('hidden');
                                $('#contenedor_timer').addClass('hidden');
                            }
                            else if (TiempoCitaPendiente > dataCitas.duracion) {
                                GenerarErrorAlerta('La cita comenzo ' + TiempoCitaPendiente + ' minutos antes de la hora programada!', "error");
                                goAlert();
                                segCitas = (dataCitas.duracion * 60);
                            }
                            else {
                                segCitas = (TiempoCitaPendiente * 60);
                            }

                        }

                        $('#TimerCitas').attr('data-timer', segCitas);
                        $("#TimerCitas").TimeCircles({ time: { Days: { show: false }, Hours: { show: false }, Minutes: { text: "Minutos" }, Seconds: { show: false } } }, { total_duration: "Minutes" }, { bg_width: 0.5 });
                        //console.log(data.duracion);
                        $('#txt_identificacion').val(dataCitas.CitaIdentificacion);
                        $('#txt_nombre').val(dataCitas.CitaNombre);
                        $('#txt_email').val(dataCitas.CitaCorreoElectronico);
                        $('#txt_cel').val(dataCitas.CitaTelefonoCelular);
                        $('#txt_tel_casa').val(dataCitas.CitaTelefonoCasa);
                        $('#txt_tel_oficina').val(dataCitas.CitaTelefonoOficina);
                        $('#txt_tramite').val(dataCitas.tramite);
                        $('#txt_marca').val(dataCitas.MarcaTarjeta);
                        $('#txt_producto').val(dataCitas.Producto);
                        $('#txt_familia').val(dataCitas.Familia);
                        $('#txt_sucursal').val(dataCitas.sucursal);
                        //$('#txt_segmento').val(dataCitas.segmento);
                        $('#cod_segmento').val(dataCitas.CitaSegmentoId).trigger('change');
                        //$('#txt_tarjeta').val(formatCard(tarjetaDecrypted.toString(CryptoJS.enc.Utf8)));
                        $('#txt_tarjeta').val(tarjetaDecrypted.toString(CryptoJS.enc.Utf8));
                        $('#txt_fecha').val(dataCitas.CitaFecha);
                        $('#txt_hora_vista').val(dataCitas.CitaHora);
                        $('#txt_familia').val(dataCitas.Familia);
                        $('#txt_producto').val(dataCitas.Producto);
                        //$('#txt_cuenta').val(formatCard(cuentaDecrypted.toString(CryptoJS.enc.Utf8)));
                        $('#txt_cuenta').val(cuentaDecrypted.toString(CryptoJS.enc.Utf8));
                        if (timerGo) {
                            initTimers((dataCitas.duracion - 1), dataCitas.TiempoMuerto, dataCitas.AlertaPrevia, dataCitas.ToleranciaFinalizacion, SucHorarioCorrido);
                            $('#contenedor_timer').removeClass('hidden');
                            $('#contenedor_mensaje_timer').addClass('hidden');
                        }

                        /*for (var i = dataCitas.length - 1; i >= 0; i--) {
                            if (dataCitas[i].CitaId == CitaId) {
                                console.log(dataCitas[i]);
                                
                            }
                        }*/
                        //$('.nav-tabs a[href="#ver_cita"]').closest('li').removeClass('hide');
                        //activaTab('ver_cita');
                        GetRazonesByCita(CitaId);
                        LoadMasTarjetas();
                        ControlsCitas(true, 'Cita en proceso...');
                    }
                    else {
                        GenerarErrorAlerta(dataCitas.Mensaje, "error");
                        goAlert();
                    }
                }
                else {
                    GenerarErrorAlerta(dataCitasI.Mensaje, "error");
                    goAlert();
                }

            });
            posting.fail(function (data, status, xhr) {
                //console.log(data);
                GenerarErrorAlerta(xhr, "error");
                goAlert();
            });   
        });
        posting.fail(function (data, status, xhr) {
            //console.log(data);
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });      
    }
    catch (e) {
        GenerarErrorAlerta(e, "error");
        goAlert();
        //console.log(e);
    }
}

function LoadCalendario(data)
{
      $('#calendar').fullCalendar({
      header: {
        left: 'prev,next today',
        center: 'title',
        right: 'month,agendaWeek,agendaDay'
      },
      buttonText: {
        today: 'today',
        month: 'month',
        week: 'week',
        day: 'day'
      },
      //Random default events
      events: data,
      editable: false,
      droppable: false, // this allows things to be dropped onto the calendar !!!
      drop: function (date, allDay) { // this function is called when something is dropped

        // retrieve the dropped element's stored Event Object
        var originalEventObject = $(this).data('eventObject');

        // we need to copy it, so that multiple events don't have a reference to the same object
        var copiedEventObject = $.extend({}, originalEventObject);

        // assign it the date that was reported
        copiedEventObject.start = date;
        copiedEventObject.allDay = allDay;
        copiedEventObject.backgroundColor = $(this).css("background-color");
        copiedEventObject.borderColor = $(this).css("border-color");

        // render the event on the calendar
        // the last `true` argument determines if the event "sticks" (http://arshaw.com/fullcalendar/docs/event_rendering/renderEvent/)
        $('#calendar').fullCalendar('renderEvent', copiedEventObject, true);

        // is the "remove after drop" checkbox checked?
        if ($('#drop-remove').is(':checked')) {
          // if so, remove the element from the "Draggable Events" list
          $(this).remove();
        }

      },
      eventClick: function(calEvent, jsEvent, view) {

        console.log('Event: ' + calEvent.id);
        console.log('Coordinates: ' + jsEvent.pageX + ',' + jsEvent.pageY);
        console.log('View: ' + view.name);
        // $('#tableCitas').DataTable().search(
        //   calEvent.id
        // ).draw();
        $('#tableCitas').DataTable().column( 0 ).search(
          calEvent.id
        ).draw(); 
        // change the border color just for fun
        //$(".borderItemCalendar").removeClass('borderItemCalendar');
       //$(this).addClass('borderItemCalendar');
       $('a').css('border-color', 'transparent');
       $(this).css('border-color', 'red');

    }
    });
}

function showAviso(e)
{
    //console.log('Mostrar Aviso');
    e.stopPropagation();
    if (citaEstado)
    {
        GenerarErrorAlerta('Hay una cita en proceso, finalice la cita para poder continuar!', "error");
        goAlert();
    }
    else
    {
        var id = $(e.currentTarget).attr('id');
        var CubiculoId = $('#userCubiculo').val();
        var path = urljs + "atencioncitas/GetCitasByIdAndPosicion";
        
        var posting = $.post(path, { CitaId: Number(id), PosicionId: CubiculoId });
        posting.done(function (data, status, xhr) {
            $('#avisoMensaje').html('');
            $('#avisoMensajeCitaProgramada').html('');
            console.log(data);
            if (data["Accion"]) {
                if (data["FlagProximaCitaProgramada"]) {                    
                    var mensajeCita = '';
                    if (data["MinutosProximaCitaProgramada"] >= 0 && data["MinutosProximaCitaProgramada"] <= 10) {
                        mensajeCita = '<h4>Esta seguro que desea atender esta cita ya que hacen falta : <span class="label label-primary">' + data["MinutosProximaCitaProgramada"] + '</span> Minutos para la proxima cita programada!</h4>';
                        $('#avisoMensajeCitaProgramada').html(mensajeCita);
                    }
                    else if (data["MinutosProximaCitaProgramada"] < 0) {
                        mensajeCita = '<h4>Esta seguro que desea atender esta cita walkin ya que tiene una cita programada con un restraso de: <span class="label label-primary">' + Math.abs(data["MinutosProximaCitaProgramada"]) + '</span> Minutos!</h4>';
                        $('#avisoMensajeCitaProgramada').html(mensajeCita);
                    }
                    else {
                        var mensaje = '<h4>Esta seguro que desea atender al cliente: <span class="label label-primary">' + data["CitaNombre"] + '</span> con un tipo de tramite de <span class="label label-primary">' + data["tramite"] + '</span> ?</h4>';
                        $('#avisoMensaje').html(mensaje);
                    }
                    
                }
                else {
                    var mensaje = '<h4>Esta seguro que desea atender al cliente: <span class="label label-primary">' + data["CitaNombre"] + '</span> con un tipo de tramite de <span class="label label-primary">' + data["tramite"] + '</span> ?</h4>';
                    $('#avisoMensaje').html(mensaje);
                }

                $('#hidden_citaId').val(data["CitaId"]);
                $('#hidden_Identificacion').val(data["CitaIdentificacion"]);               
                $('#avisoAtencionModal').modal('show');
            }
            else {
                GenerarErrorAlerta(data['Mensaje'], "error");
                goAlert();
            }

        });
        posting.fail(function (data, status, xhr) {
            console.log(data);
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
    }
    
}

function showDetails(e) {
    e.stopPropagation();
    var CitaId = $(e.currentTarget).attr('data-citaid');
    var path = urljs + "atencioncitas/GetCitasById";
    var posting = $.post(path, { CitaId: Number(CitaId) }); 
    posting.done(function (data, status, xhr) {
        dataCitas = [];
        dataCitas = data;
        //console.log(dataCitas.length);
        if (dataCitas.Accion == 1) {

            var cuentaDecrypted = dataCitas.CitaCuenta; //CryptoJS.AES.decrypt(dataCitas.CitaCuenta, 'BACPANAMA');
            var tarjetaDecrypted = dataCitas.CitaTarjeta; //CryptoJS.AES.decrypt(dataCitas.CitaTarjeta, 'BACPANAMA');
            $('#txt_identificacion_d').val(dataCitas.CitaIdentificacion);
            $('#txt_nombre_d').val(dataCitas.CitaNombre);
            $('#txt_email_d').val(dataCitas.CitaCorreoElectronico);
            $('#txt_cel_d').val(dataCitas.CitaTelefonoCelular);
            $('#txt_tel_casa_d').val(dataCitas.CitaTelefonoCasa);
            $('#txt_tel_oficina_d').val(dataCitas.CitaTelefonoOficina);
            $('#txt_tramite_d').val(dataCitas.tramite);
            $('#txt_marca_d').val(dataCitas.MarcaTarjeta);
            $('#txt_producto_d').val(dataCitas.Producto);
            $('#txt_familia_d').val(dataCitas.Familia);
            $('#txt_sucursal_d').val(dataCitas.sucursal);
            //$('#txt_segmento').val(dataCitas.segmento);
            $('#txt_segmento_d').val(dataCitas.segmento);
            //$('#txt_tarjeta').val(formatCard(tarjetaDecrypted.toString(CryptoJS.enc.Utf8)));
            $('#txt_tarjeta_d').val(tarjetaDecrypted.toString(CryptoJS.enc.Utf8));
            $('#txt_fecha_d').val(dataCitas.CitaFecha);
            $('#txt_hora_vista_d').val(dataCitas.CitaHora);
            $('#txt_familia_d').val(dataCitas.Familia);
            $('#txt_producto_d').val(dataCitas.Producto);
            //$('#txt_cuenta').val(formatCard(cuentaDecrypted.toString(CryptoJS.enc.Utf8)));
            $('#txt_cuenta_d').val(cuentaDecrypted.toString(CryptoJS.enc.Utf8));
            $('#DetalleCitaTitle').html('Vista Previa');
            $('#DetalleCitaModal').modal();
        }
        else {
            GenerarErrorAlerta(dataCitas.Mensaje, "error");
            goAlert();
        }
    });
    posting.fail(function (data, status, xhr) {
        //console.log(data);
        GenerarErrorAlerta(xhr, "error");
        goAlert();
    });  


    var path_razon = urljs + "/atencionCitas/GetRazonesByCita";
    var posting_razon = $.post(path_razon, { CitaId: CitaId });
    posting_razon.done(function (data, status, xhr) {
        LimpiarTabla("#tableRazonesdetalles");
        if (data.length > 0) {
            if (data[0].Accion > 0) {
                var counter = 1;
                for (var i = data.length - 1; i >= 0; i--) {
                    //console.log(data[i]);
                    addRowRazones(data[i], "#tableRazonesdetalles", counter);
                    counter++;
                }
            }
            else {/*
                    GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();*/
            }
        }
    });
    posting_razon.fail(function (data, status, xhr) {

        GenerarErrorAlerta(xhr, "error");
        goAlert();
    });
    posting_razon.always(function (data, status, xhr) {
        //RemoveAnimation("body");
    });


}

function LoadResultadoCitas() {
    //var data = '{ "id": "RE" }';
    //data = JSON.stringify(data);
    //console.log(data);
    var infojson = jQuery.parseJSON('{"input": "#cmb_resultado,#cmb_resultado_m", ' +
		                              '"url": "configuracion/getParametosByIdEncabezado/", ' +
		                              '"val": "ConfigItemID", ' +
		                              '"type": "GET", ' +
		                              '"data": "RE", ' +
		                              '"text": "ConfigItemDescripcion"}');
    //console.log(infojson);
    LoadComboBox(infojson);
}

function LoadResultadoCitas_m() {
    //var data = '{ "id": "RE" }';
    //data = JSON.stringify(data);
    //console.log(data);
    var infojson = jQuery.parseJSON('{"input": "#cmb_resultado_m", ' +
		                              '"url": "configuracion/getParametosByIdEncabezado/", ' +
		                              '"val": "ConfigItemID", ' +
		                              '"type": "GET", ' +
		                              '"data": "RE", ' +
		                              '"text": "ConfigItemDescripcion"}');
    //console.log(infojson);
    LoadComboBox(infojson);
}

function LoadHerramientas() {
    //var data = '{ "id": "RE" }';
    //data = JSON.stringify(data);
    //console.log(data);
    var infojson = jQuery.parseJSON('{"input": "#cmbHerramientas, #cmbHerramientas_m", ' +
		                              '"url": "configuracion/getParametosByIdEncabezado/", ' +
		                              '"val": "ConfigItemID", ' +
		                              '"type": "GET", ' +
		                              '"data": "TOOLS", ' +
		                              '"text": "ConfigItemDescripcion"}');
    //console.log(infojson);
    LoadComboBox(infojson);
}

function citas_constructor_segmentos_m() {
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#cod_segmento_m, #cod_segmento", ' +
        '"url": "configuracion/getParametosByIdEncabezado/", ' +
        '"val": "ConfigItemID", ' +
        '"type": "GET", ' +
        '"data": "SEGM", ' +
        '"text": "ConfigItemDescripcion"}');
    LoadComboBox(infojson);
}

function citas_constructor_tipos_razon() {
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#cod_tipo_razon", ' +
        '"url": "TipoRazones/GetAllTipoRazones/", ' +
        '"val": "TipoId", ' +
        '"type": "GET", ' +
        '"data": "", ' +
        '"text": "TipoDescripcion"}');
    LoadComboBox(infojson);
}

function citas_constructor_listado_extra_config(id) {
    $("#label_cod_listado_extra").text(EtiquetaListadoExtraGlobal);
    $("#listadoExtraContainer").removeClass('hide');
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#cod_listado_extra", ' +
        '"url": "configuracion/getParametosByIdEncabezado/", ' +
        '"val": "ConfigItemID", ' +
        '"type": "GET", ' +
        '"data": "' + id + '", ' +
        '"text": "ConfigItemDescripcion"}');
    LoadComboBox(infojson);
}

function citas_constructor_razones(Id) {
    $("#cod_razonp").empty().append(new Option('No se ha cargado información', '-1'));
    try {
        var path = urljs + "/Razones/GetAll";
        var posting = $.post(path, { TipoId: Id });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    $("#cod_razonp").empty().append(new Option('Ninguno', '-1'));
                    for (var i = data.length - 1; i >= 0; i--) {
                        $("#cod_razonp").append(new Option(data[contador].RazonDescripcion, data[contador].RazonId));
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
}

function citas_constructor_listado_extra_canal() {
    $("#label_cod_listado_extra").text(EtiquetaListadoExtraGlobal);
    $("#listadoExtraContainer").removeClass('hide');
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#cod_listado_extra", ' +
        '"url": "sucursales/getSucursalesCanal/", ' +
        '"val": "SucursalId", ' +
        '"type": "GET", ' +
        '"data": "", ' +
        '"text": "SucursalNombre"}');
    LoadComboBox(infojson);
}

function checkListadoExtra(tipoId) {
    try {
        
        var path = urljs + "/TipoRazones/GetOne";
        var posting = $.post(path, { id: Number(tipoId) });
        posting.done(function (data, status, xhr) {            
            if (data.Accion == 1 && data.TipoTieneListadoExtra == 1) {                
                listadoExtraGlobal = 1;
                EtiquetaListadoExtraGlobal = data.TipoEtiquetaListadoExtra;
                TipoOrigenListadoExtraGlobal = data.TipoOrigenListadoExtra;
                TipoCodigoListadoExtraGlobal = data.TipoCodigoListadoExtra;
            }
            else
            {
                listadoExtraGlobal = 0;
            }
        });
        posting.fail(function (data, status, xhr) {
            //
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

//var Herram = false;
function AgregarHerramientas()
{
    var id = $("#hidden_citaId").val();
    var herramienta = $("#cmbHerramientas").val();
    var obs = $('#txtObsHerramienta').val();
    
    try {
        if (DataTableHerramCont > 5) {
            GenerarErrorAlerta('No puede agregar mas de cinco herramientas!', "error");
            goAlert();
        }
        else {

        
        if (herramienta == '-1') {
            GenerarErrorAlerta('Favor seleccione un tipo de herramienta!', "error");
            goAlert();
        }
        else if (id == "")
        {
            GenerarErrorAlerta('Debe de seleccionar una cita para continuar!', "error");
            goAlert();
        }
        else {                        
            //JSON data
            var path = urljs + 'atencionCitas/AgregarHerramienta';
            var dataType = 'application/json; charset=utf-8';
            var data = {
                CitaId: id,
                HerramientaId: herramienta,
                HerramientaObservacion: jQuery.trim(obs)
            }
            var posting = $.post(path, data);
            posting.done(function (data, status, xhr) {
                if (data.Accion)
                {
                    GetHerramientasByCita(data.CitaId);
                    LimpiarInput(['#txtObsHerramienta'], ['#cmbHerramientas']);
                }
                else
                {
                    GenerarErrorAlerta(data.Mensaje, "error");
                    goAlert();
                }
                
                

            });
            posting.fail(function (data, status, xhr) {
                //console.log(data);
                GenerarErrorAlerta(status, "error");
                goAlert();
            });
        }
        //Herram = true;
        }
    }
    catch (e) {
        console.log(e);
    }
}

var DataTableHerram = 0;
var DataTableHerramCont = 0;
function GetHerramientasByCita(CitaId) {
    try {
        var path = urljs + "/atencionCitas/GetHerramientasByCita";
        var posting = $.post(path, { CitaId: CitaId });
        posting.done(function (data, status, xhr) {
            LimpiarTabla("#tableHerramientas");
            if (data.length > 0) {
                if (data[0].Accion > 0) {
                    var counter = 1;
                    for (var i = data.length - 1; i >= 0; i--) {
                        //console.log(data[i]);
                        addRowHerramientas(data[i], "#tableHerramientas", counter);
                        counter++;
                    }
                }
                else {
                    /*GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();*/
                }
                if (counter > 0){
                    DataTableHerramCont = counter;
                }
            }
            DataTableHerram = data[0].Accion;
        });
        posting.fail(function (data, status, xhr) {
            //
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

function addRowHerramientas(ArrayData, tableID) {    
    var newRow = $(tableID).dataTable().fnAddData([        
        ArrayData['HerramientaDescripcion'],
        ArrayData['HerramientaObservacion'],                
        "<button data-Id='" + ArrayData['HerramientaId'] + "' data-CitaId='" + ArrayData['CitaId'] + "' title='Eliminar Herramienta' data-toggleg='tooltip' onClick='eliminarHerramienta(event)' id='btnHerramienta" + ArrayData['HerramientaId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        ArrayData['CitaId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['CitaId']);
    $('td', theNode)[3].setAttribute('class', 'CitaId hidden');
    $('td', theNode)[2].setAttribute('class', 'text-center');
}

function eliminarHerramienta(e)
{    
    var HerramientaId = $(e.currentTarget).attr('data-Id');
    var CitaId = $(e.currentTarget).attr('data-CitaId');    
    var path = urljs + 'atencionCitas/deleteHerramienta';
    var posting = $.post(path, { CitaId: Number(CitaId), HerramientaId: HerramientaId });
    posting.done(function (data, status, xhr) {
        if (data.Accion > 0) {
            GetHerramientasByCita(CitaId);
        }
        else {
            GenerarErrorAlerta(data.Mensaje, "error");
            goAlert();
        }
    });
    posting.fail(function (data, status, xhr) {
        //sendError(data);
        GenerarErrorAlerta(xhr, "error");
        goAlert();
    });
}

function AgregarRazon() {
    var id = $("#hidden_citaId").val();
    var tipoRazon = $("#cod_tipo_razon").val();
    var razon = $("#cod_razonp").val();
    var idListado = $("#cod_listado_extra").val();
    try {
        if (DataTable > 0) {
            GenerarErrorAlerta('No puede agregar mas de una razón!', "error");
            goAlert();
        }
        else {
        if (tipoRazon == '-1' || razon == '-1' || (listadoExtraGlobal == 1 && idListado == '-1')) {
            GenerarErrorAlerta('Favor seleccione información necesaria!', "error");
            goAlert();
        }
        else {
            var path = urljs + 'citas/AsignarRazon';
            if (id == "") {
                GenerarErrorAlerta('Ninguna cita seleccionada!', "error");
                goAlert();
            }
            else {
                //JSON data
                var dataType = 'application/json; charset=utf-8';
                var data = {
                    CitaId: id,
                    TipoRazonId: tipoRazon,
                    RazonId: razon,
                    TipoOrigenExtraId: TipoOrigenListadoExtraGlobal,
                    CodigoListadoOrigenExtraId: TipoCodigoListadoExtraGlobal,
                    DatoExtraId: idListado,
                    RazonOrigen : "A"
                }                
                var posting = $.post(path, data);
                posting.done(function (data, status, xhr) {                    
                    //$('#modalCitas').modal('hide');
                    //GenerarSuccessAlerta(data.Mensaje, "success");
                    //goAlert();
                    GetRazonesByCita(id);
                    LimpiarInput([''], ['#cod_tipo_razon', '#cod_listado_extra']);
                    //activarTab('inicio');
                    //GetRazonesByCita(CitaId)
                    //LimpiarInput(inputs, ['']);
                    //$("#cod_sucursal").val('');
                    //$("#cod_segmento").empty().append( new Option('No se ha cargado información','-1') );
                    //$("#cod_tramite").empty().append( new Option('No se ha cargado información','-1') );
                });
                posting.fail(function (data, status, xhr) {
                    GenerarErrorAlerta(status, "error");
                    goAlert();
                });
            }
        }
        }
    }
    catch (e) {
        console.log(e);
    }
}
var DataTable;
var DataTableRazonTar = 0;
function GetRazonesByCita(CitaId) {
    try {
        var path = urljs + "/atencionCitas/GetRazonesByCita";
        var posting = $.post(path, { CitaId: CitaId });
        posting.done(function (data, status, xhr) {
            LimpiarTabla("#tableRazones");
            if (data.length > 0) {
                if (data[0].Accion > 0) {
                    var counter = 1;
                    for (var i = data.length - 1; i >= 0; i--) {
                        //console.log(data[i]);
                        addRowRazones(data[i], "#tableRazones", counter);
                        counter++;
                    }
                }
                else {/*
                    GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();*/
                }
            }
            DataTable = data[0].Accion;
        });
        posting.fail(function (data, status, xhr) {
            
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

function addRowRazones(ArrayData, tableID, counter) {
    var CitaId = $('#hidden_citaId').val();
    var newRow = $(tableID).dataTable().fnAddData([
        counter,
        ArrayData['tipoRazonAbreviatura'],
        ArrayData['RazonDescripcion'],
        ArrayData['TipoEtiquetaListadoExtra'],
        ArrayData['listadoExtraDescripcion'],
        "<button data-tipoId='" + ArrayData['TipoId'] + "' data-DatoExtraId='" + ArrayData['DatoExtraId'] + "' data-TipoId='" + ArrayData['TipoId'] + "'  data-RazonId='" + ArrayData['RazonId'] + "' title='Eliminar Razón' data-toggleg='tooltip' onClick='eliminarRazon(event)' id='btnEliminarRazon" + ArrayData['TipoId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        CitaId
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', CitaId);
    $('td', theNode)[6].setAttribute('class', 'CitaId hidden');
    $('td', theNode)[5].setAttribute('class', 'text-center');
    console.log(ArrayData['RazonOrigen']);
    //if (ArrayData['RazonOrigen'] == 'P') {
    //    $('td', theNode)[5].setAttribute('class', 'hidden');
    //}
    
}

function eliminarRazon(e) {
   
    var citaId = $('#hidden_citaId').val();
    var tipoRazonId = $(e.currentTarget).attr('data-TipoId');
    var razonId = $(e.currentTarget).attr('data-RazonId');
    var datoExtraId = $(e.currentTarget).attr('data-DatoExtraId');
    var path = urljs + 'citas/deleteRazon';
    var posting = $.post(path, { citaId: Number(citaId), tipoRazonId: Number(tipoRazonId), razonId: Number(razonId), datoExtraId: datoExtraId });
    posting.done(function (data, status, xhr) {
        if (data.Accion > 0) {
            GetRazonesByCita(citaId);
        }
        else
        {
            GenerarErrorAlerta(data.Mensaje, "error");
            goAlert();
        }
    });
    posting.fail(function (data, status, xhr) {
        //sendError(data);
        GenerarErrorAlerta(xhr, "error");
        goAlert();
    });
    posting.always(function () {
        /*console.log("Se cargo los proyectos correctamente");*/
        //RemoveAnimation("body");
    });
}

function enviarEmail(numeroGestion) {
    try {
        var path = urljs + "/atencionCitas/EnviarEmail";
        var posting = $.post(path, { numeroGestion: Number(numeroGestion) });
        posting.done(function (data, status, xhr) {

            //GenerarSuccessAlerta(data.Mensaje, "success");
            //goAlert();
            //LimpiarInput(inputs, ['']);
            //$("#cod_sucursal").val('');
            //$("#cod_segmento").empty().append(new Option('No se ha cargado información', '-1'));
            //$("#cod_tramite").empty().append(new Option('No se ha cargado información', '-1'));
            //activarTab('inicio');
            console.log(data);
        });
        posting.fail(function (data, status, xhr) {
            
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
const Resultado1 = $('#cmb_resultado').val();
function guardarCita(e)
{
    var CitaId = $('#hidden_citaId').val();
    var obs = $('#txt_observacion').val();
    var res = $('#cmb_resultado').val();
    var Cuenta = $.trim($('#txt_cuenta').val());
    var Tar = $.trim($('#txt_tarjeta').val());
    //var Herram = $('#cmbHerramientas').val();
    //var Razon = $('#cod_tipo_razon').val();
    //var Razon = $('#tableRazones').val().counter();
    var go = true;


    if (res == 'RE01') {
        if (DataTableHerram <= 0) {
        go = false;
        GenerarErrorAlerta('Debe seleccionar una Herramienta para continuar!', "error");
        goAlert();
        }    
    }

    if (DataTable < 1) {
        go = false;
        GenerarErrorAlerta('Debe seleccionar una Razon para continuar!', "error");
        goAlert();
    }

    if(CitaId == '')
    {
        go = false;
        GenerarErrorAlerta('Debe seleccionar una cita para continuar!', "error");
        goAlert();
    }

    if(obs == "")
    {
        go = false;
        GenerarErrorAlerta('Debe ingresar una observación para continuar!', "error");
        goAlert();
    }

    if(res == "-1")
    {
        go = false;
        GenerarErrorAlerta('Debe seleccinar un resultado para continuar!', "error");
        goAlert();
    }

    if (!CuentaParaCita) {
        go = false;
        GenerarErrorAlerta('Debe ingresar una cuenta para continuar!', "error");
        goAlert();
    }

    //if (!TarjetaParaCita) {
    //    go = false;
    //    GenerarErrorAlerta('Debe ingresar una tarjeta para continuar!', "error");
    //    goAlert();
    //}

    if(go)
    {
        var path = urljs + "atencioncitas/FinalizarCita";
        var posting = $.post(path, { CitaId: Number(CitaId), Observacion: obs, Resolucion: res });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            if (data.Accion == 1)
            {
                GenerarSuccessAlerta(data.Mensaje, 'success');
                goAlert();
                enviarEmail(CitaId);
                ControlsCitas(false, 'Cita en proceso...');
                initForm();
                location.reload();
                
            }
            else
            {
                GenerarErrorAlerta(data.Mensaje, "successModalCubiculos");
                goAlert();
            }

        });
        posting.fail(function (data, status, xhr) {
            console.log(data);
            GenerarErrorAlerta(xhr, "successModalCubiculos");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
       
    }
}
  
function formatCard(numero) {
    return (numero.toString().substr(0, 6) + ' **** **** ' + numero.toString().substring(numero.toString().length - 4, numero.toString().length));
}

function ControlsCitas(accion, message)
{
    //accion = 1 oculta el panel de la citas | accion = 0 muestra el panel de las citas
    if(accion)
    {
        citaEstado = 1;
        $('#list_citas').addClass('hidden');
        $('#controles_programada').addClass('hidden');
        $('#message_programa_proccess').removeClass('hidden');

        $('#list_citas_walkin').addClass('hidden');
        $('#controles_walkin1').addClass('hidden');
        $('#message_walkin_proccess').removeClass('hidden');
        $('#spanProgramadas, #spanWalkin').text(message);
    }
    else
    {
        citaEstado = 0;
        $('#list_citas').removeClass('hidden');
        $('#controles_programada').removeClass('hidden');
        $('#message_programa_proccess').addClass('hidden');

        $('#list_citas_walkin').removeClass('hidden');
        $('#controles_walkin1').removeClass('hidden');
        $('#message_walkin_proccess').addClass('hidden');
        $('#spanProgramadas, #spanWalkin').text(message);
    }
}

function initForm()
{
    /*Encontramos todos los inputs de tipo hidden*/
    var arrHidden = new Array();
    $('input[type=hidden]').each(function () {
        arrHidden.push('#'+$(this).attr('id'));
    });

    /*Encontramos todos los inputs de tipo text*/
    var arrText = new Array();
    $('input[type=text]').each(function () {
        arrText.push('#' + $(this).attr('id'));
    });

    /*Encontramos todos los text area*/
    var arrTextarea = new Array();
    $('textarea').each(function () {
        arrTextarea.push('#' + $(this).attr('id'));
    });

    /*Encontramos los comboBox*/
    var arrComboBox = new Array();
    $('#nueva_cita select').each(function () {
        arrComboBox.push('#' + $(this).attr('id'));
    });

    /*Encontramos las tablas*/
    var arrTable = new Array();
    $('table').each(function () {
        arrTable.push('#' + $(this).attr('id'));
    });

    /*Limpiamos todos los obj*/
    var arrays = [arrHidden, arrText, arrTextarea, arrComboBox];
    for (var i = 0; i < arrays.length; i++) {
        LimpiarInput(arrays[i], [""]);
    }
    /*Los comboBos los limpiamos aparte*/
    LimpiarInput([""], arrComboBox);

    /*Limpiamos las tablas*/
    for (var i = 0; i < arrTable.length; i++) {
        LimpiarTabla(arrTable[i]);
    }
    
}

function initTimers(duracion, tiempoMuerto, alertaPrevia, toleranciaFinalizar, SucHorarioCorrido)
{
    //console.log(SucHorarioCorrido);
    // SucHorarioCorrido: 0=Sin tiempo corrido / 1=Con tiempo Corrido
    $('#lbl_tiempoMuerto').html('<b>0 Min</b>/' + tiempoMuerto + ' Min');
    $('#lbl_alertaPrevia').html('<b>0 Min</b>/' + alertaPrevia + ' Min');
    $('#lbl_Torelancia').html('<b>0 Min</b>/' + toleranciaFinalizar + ' Min');
    $('.progress-bar').css('width', '0%');    
    var inteval_alertaPrevia = (duracion - alertaPrevia);

    if (SucHorarioCorrido) {
        $('#contenedor_tiempomuerto').removeClass('hidden');
        var initBarsTiempoMuerto = setTimeout(function () {
            console.log('Comienza la carga de la barra del tiempo muerto!' + ' | Tiempo : ' + getDate());
            $("#TimerCitas").TimeCircles({ circle_bg_color: "#f45354" });
            SetBarTiempoMuerto(tiempoMuerto);
        }, (duracion * 60000));
    }
    else {
        $('#contenedor_tiempomuerto').addClass('hidden');
    }

    var initBarsAlertaPrevia = setTimeout(function () {
        console.log('Comienza la carga de la barra de la alerta previa!' + ' | Tiempo : ' + getDate());
        SetBarAlertaPrevia(alertaPrevia);
    }, (inteval_alertaPrevia * 60000));

    var initBarsTolerancia = setTimeout(function () {
        console.log('Comienza la carga de la barra de tolerancia de finalización!' + ' | Tiempo : ' + getDate());
        SetBarTolerancia(toleranciaFinalizar);
    }, (duracion * 60000));

    console.log('Tiempo en milisegundos de espera: ' + (duracion * 60000) + ' | Tiempo : ' + getDate());
}

function updateProgressBar(step, id) {
    console.log('Se actualizo la barra: ' + id + ' | Tiempo : ' + getDate());
    var elem = document.getElementById(id);
    elem.style.width = step + '%';
}

function SetBarTiempoMuerto(tiempoMuerto) {
    
    var barTiempoMuerto = 0; //Minutos
    var pTiempoMuerto = 0;
    //var stop = 0;
    var stepBars = setInterval(function () {
        barTiempoMuerto++;
        pTiempoMuerto = ((barTiempoMuerto * 100) / tiempoMuerto);
        updateProgressBar(pTiempoMuerto, 'bar_tiempoMuerto');
        //stop++;
        $('#lbl_tiempoMuerto').html('<b>' + barTiempoMuerto + '</b>/' + tiempoMuerto + ' Min');
        console.log('Paso un tiempo muerto: ' + barTiempoMuerto + ' Min!' + ' | Tiempo : ' + getDate());

        if (barTiempoMuerto == tiempoMuerto) {
            console.log('Termino el tiempo Muerto' + ' | Tiempo : ' + getDate());
            clearInterval(stepBars);
        }
    }, 60000);
}

function SetBarAlertaPrevia(alertaPrevia) {

    var barAlerta = 0; //Minutos
    var pAlerta = 0;
    //var stop = 0;
    var stepBars = setInterval(function () {
        barAlerta++;
        pAlerta = ((barAlerta * 100) / alertaPrevia);
        updateProgressBar(pAlerta, 'bar_alertaPrevia');
        //stop++;
        $('#lbl_alertaPrevia').html('<b>' + barAlerta + '</b>/' + alertaPrevia + ' Min');
        console.log('Paso un alerta previa: ' + barAlerta + ' Min!' + ' | Tiempo : ' + getDate());

        if (barAlerta == alertaPrevia) {
            console.log('Termino la alerta previa' + ' | Tiempo : ' + getDate());
            clearInterval(stepBars);
        }
    }, 60000);
}

function SetBarTolerancia(toleranciaFinalizar) {

    var barAlerta = 0; //Minutos
    var pAlerta = 0;
    //var stop = 0;
    var stepBars = setInterval(function () {
        barAlerta++;
        pAlerta = ((barAlerta * 100) / toleranciaFinalizar);
        updateProgressBar(pAlerta, 'bar_tolerancia');
        //stop++;
        $('#lbl_Torelancia').html('<b>' + barAlerta + '</b>/' + toleranciaFinalizar + ' Min');
        console.log('Paso un alerta previa: ' + barAlerta + ' Min!' + ' | Tiempo : ' + getDate());

        if (barAlerta == toleranciaFinalizar) {
            console.log('Termino la tolerancia a finalizacion' + ' | Tiempo : ' + getDate());
            clearInterval(stepBars);
        }
    }, 60000);
}

function GetCitasProgramadasDia() {

    var go = true;
    var CubiculoId = $('#userCubiculo').val();
    var idSucursal = $('#userSucursal').val();
    var path = urljs + "atencioncitas/GetCitasDia";
    var posting = $.post(path, { Posicionid: CubiculoId, Sucursalid: idSucursal });
    posting.done(function (data, status, xhr) {
        //console.log(data);
        if (data[0].Accion) {
            $('#list_citas_dia').html("");
            for (var i = 0; i < data.length; i++) {                
                //var item = '<a href="javascript:void(0);" class="list-group-item text-center list-group-item-datos">' + data[i]["CitaHora"] + ' | ' + data[i]["CitaEstadoDescripcion"] + '</a>';
                var item = '<a onClick="showDetails(event)" data-citaid = "' + data[i]["CitaId"] + '" href="javascript:void(0);" class="list-group-item text-center list-group-item-datos"> ' + data[i]["CitaHora"] + ' | ' + data[i]["CitaEstadoDescripcion"] + '</a>';
                $('#list_citas_dia').append(item);
            }
            //$('#controles_programada').removeClass('hidden');
        }
        else {
            GenerarErrorAlerta(data[0].Mensaje, "error");
            goAlert();
            //$('#controles_programada').addClass('hidden');
        }

    });
    posting.fail(function (data, status, xhr) {
        GenerarErrorAlerta(xhr, "error");
        goAlert();
    });
}

function addRowTarjetas(ArrayData, tableID, counter) {
    var CitaId = $('#hidden_citaId').val();
    //var cuentaDecrypted = CryptoJS.AES.decrypt(ArrayData['TarjetaOriginal'], 'BACPANAMA');
    //var tarjetaDecrypted = CryptoJS.AES.decrypt(ArrayData['CuentaOriginal'], 'BACPANAMA');
    var cuentaDecrypted = ArrayData['CitaCuenta'];
    var tarjetaDecrypted = ArrayData['CitaTarjeta'];
    var newRow = $(tableID).dataTable().fnAddData([
        counter,
        //tarjetaEncrypted,
        //cuentaEncrypted,
        cuentaDecrypted.toString(),//CryptoJS.enc.Utf8),
        tarjetaDecrypted.toString(),//CryptoJS.enc.Utf8),
        
        ArrayData['segmento'],
        ArrayData['Producto'],
        "<button data-CitaId='" + ArrayData['CitaId'] + "' data-cuenta='" + ArrayData['CitaCuenta'] + "' data-CuentaOriginal='" + ArrayData['CuentaOriginal'] + "' data-TarjetaOriginal='" + ArrayData['TarjetaOriginal'] + "' data-tarjeta='" + ArrayData['CitaTarjeta'] + "' data-resolucion='" + ArrayData['Resolucion'] + "' data-comentario='" + ArrayData['CitaObservacion'] + "'  title='Editar Tarjeta' data-toggleg='tooltip' onClick='editarTarjeta(event)' class='btn btn-success botonVED text-center btn-sm'><i class='fa fa-pencil'></i></button>" +
        "<button data-CitaId='" + ArrayData['CitaId'] + "' data-cuenta='" + ArrayData['CitaCuenta'] + "' data-tarjeta='" + ArrayData['CitaTarjeta'] + "' title='Eliminar Tarjeta' data-toggleg='tooltip' onClick='eliminarTarjeta(event)' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>"
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', CitaId);    
    $('td', theNode)[5].setAttribute('class', 'text-center');
    
}

function LoadMasTarjetas() {
    var CitaId = $("#hidden_citaId").val();
    LimpiarTabla('#tableTarjetas');
    if (CitaId == "") {
        GenerarErrorAlerta('Ninguna cita seleccionada!', "error");
        goAlert();
    }
    else {
        try {
            var path = urljs + "/atencionCitas/GetCitaTarjetas";
            var posting = $.post(path, { CitaId: CitaId });
            posting.done(function (data, status, xhr) {                
                if (data.length > 0) {
                    if (data[0].Accion > 0) {
                        var counter = 1;
                        for (var i = data.length - 1; i >= 0; i--) {
                            //console.log(data[i]);
                            addRowTarjetas(data[i], "#tableTarjetas", counter);
                            counter++;
                        }
                    }
                    else {
                        GenerarErrorAlerta(data[0].Mensaje, "errorTarjetas");
                        goAlert();
                    }
                }
            });
            posting.fail(function (data, status, xhr) {

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
}

function GetTarjetasRazonesByCita(CitaId, Cuenta, Tarjeta) {
    try {
        var path = urljs + "/atencionCitas/GetTarjetasRazonesByCita";
        var posting = $.post(path, { CitaId: CitaId, Cuenta: Cuenta, Tarjeta: Tarjeta });
        posting.done(function (data, status, xhr) {
            LimpiarTabla("#tableRazones_m");
            console.log(data);
            if (data.length > 0) {
                if (data[0].Accion > 0) {
                    var counter = 1;
                    for (var i = data.length - 1; i >= 0; i--) {                        
                        addRowTarjetasRazones(data[i], "#tableRazones_m", counter);
                        counter++;
                    }
                }
                else {
                    GenerarErrorAlerta(data[0].Mensaje, "errorTarjetas");
                    goAlert();
                }
                DataTableRazonTar = data[0].Accion;
            }
        });
        posting.fail(function (data, status, xhr) {

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

function SaveTarjetas() {
    //LimpiarInput(['#cmb_resultado_m'], ['#txt_observacion_m'], ['#cmbHerramientas_m'], ['#cod_tipo_razon_m']);
    var CitaId = $('#hidden_citaId').val();
    var Cuenta = $.trim($('#txt_cuenta_m').val());
    var Tarjeta = $.trim($('#txt_tarjeta_m').val());
    var Resolucion = $('#cmb_resultado_m').val();
    var Comentario = $('#txt_observacion_m').val();
    var Emisor = $('#emisorId_m').val();
    var go = true;
    var message = 'Datos Correctos!';
    if (CitaId == "") {
        go = false;
        message = 'No hay una cita en proceso! ';
    }

    if (Cuenta == "") {
        go = false;
        message = 'Debe ingresar una cuenta valida! ';
    }

    if (Tarjeta == "" || Tarjeta ==  "-1") {
        go = false;
        message = 'Debe ingresar una tarjeta! ';
    }

    if (Emisor == "") {
        go = false;
        message = 'Debe ingresar una cuenta valida! ';
    }

    if (Tarjeta == "") {
        //go = false;
        //message = 'Debe ingresar una tarjeta valida! ';
        Tarjeta == '-1';
    }

    if (go) {
        var cuentaEncrypted = Cuenta; //CryptoJS.MD5(Cuenta);
        var tarjetaEncrypted = Tarjeta; //CryptoJS.MD5(Tarjeta);
        var cuentaOriginalEncrypted = Cuenta;//CryptoJS.AES.encrypt(Cuenta, 'BACPANAMA');
        var tarjetaOriginalEncrypted = Tarjeta; //CryptoJS.AES.encrypt(Tarjeta, 'BACPANAMA');
        //var cuentaEncrypted = Cuenta;
        //var tarjetaEncrypted = Tarjeta;
        var path = urljs + "atencioncitas/SaveTarjetas";
        var posting = $.post(path, { CitaId: Number(CitaId), Cuenta: cuentaEncrypted.toString(), Tarjeta: tarjetaEncrypted.toString(), Comentario: Comentario, Resolucion: Resolucion, Emisor: Emisor, CuentaOriginal: cuentaOriginalEncrypted.toString(), TarjetaOriginal: tarjetaOriginalEncrypted.toString() });
        posting.done(function (data, status, xhr) {
            console.log(data);
            if (data.Accion == 1) {
                LoadMasTarjetas();                
                if ($('#contenedor_tarjetas').hasClass('hidden')) {
                    $('#contenedor_tarjetas').removeClass('hidden');
                }
                $('#guardarTarjetas_m').addClass('hidden');
            }
            else {
                if (!$('#contenedor_tarjetas').hasClass('hidden')) {
                    $('#contenedor_tarjetas').addClass('hidden');
                }
                $('#guardarTarjetas_m').removeClass('hidden');
                GenerarErrorAlerta(data.Mensaje, "errorTarjetas");
                goAlert();
            }

        });
        posting.fail(function (data, status, xhr) {
            console.log(data);
            GenerarErrorAlerta(xhr, "errorTarjetas");
            goAlert();
        });
    }
    else {
        GenerarErrorAlerta(message, "errorTarjetas");
        goAlert();
    }
}

function FinalizarTarjetas() {
    var CitaId = $('#hidden_citaId').val();
    var Cuenta = $.trim($('#txt_cuenta_m').val());
    var Tarjeta = $.trim($('#txt_tarjeta_m').val());
    var Resolucion = $('#cmb_resultado_m').val();
    var Comentario = $('#txt_observacion_m').val();
    var Emisor = $('#emisorId_m').val();
    var res = $('#cmb_resultado_m').val();
    var go = true;
    var message = 'Datos Correctos!';

    if (DataTableRazonTar == 0) {
        go = false;
        message = 'Debe seleccionar una Razon para continuar!';
    }


    if (res == 'RE01') {
        if (HerramTar < 1) {
            go = false;
            message = 'Debe seleccionar una Herramienta para continuar!';
        }
    }

    if (CitaId == "") {
        go = false;
        message = 'No hay una cita en proceso! ';
    }

    if (Cuenta == "") {
        go = false;
        message = 'Debe ingresar una cuenta valida! ';
    }

    if (Emisor == "") {
        go = false;
        message = 'Debe ingresar una cuenta valida! ';
    }

    if (Tarjeta == "") {
        //go = false;
        //message = 'Debe ingresar una tarjeta valida! ';
        Tarjeta = '-1';
    }

    if (Resolucion == "-1") {
        go = false;
        message = 'Debe seleccionar un resultado! ';
    }

    if (Comentario == "") {
        go = false;
        message = 'Debe ingresar una comentario! ';
    }

    if (go) {
        var cuentaOriginalEncrypted = Cuenta; //CryptoJS.AES.encrypt(Cuenta, 'BACPANAMA');
        var tarjetaOriginalEncrypted = Tarjeta; //CryptoJS.AES.encrypt(Tarjeta, 'BACPANAMA');
        var cuentaEncrypted = Cuenta; //CryptoJS.MD5(Cuenta);
        var tarjetaEncrypted = Tarjeta; //CryptoJS.MD5(Tarjeta);
        var path = urljs + "atencioncitas/SaveTarjetas";
        var posting = $.post(path, { CitaId: Number(CitaId), Cuenta: cuentaEncrypted.toString(), Tarjeta: tarjetaEncrypted.toString(), Comentario: Comentario, Resolucion: Resolucion, Emisor: Emisor, CuentaOriginal: cuentaOriginalEncrypted.toString(), TarjetaOriginal: tarjetaOriginalEncrypted.toString() });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            if (data.Accion == 1) {
                LoadMasTarjetas();
                if ($('#contenedor_tarjetas').hasClass('hidden')) {
                    $('#contenedor_tarjetas').removeClass('hidden');
                }
                $('#guardarTarjetas_m').addClass('hidden');
                $('#masTarjetasModal').modal('hide');
            }
            else {
                if (!$('#contenedor_tarjetas').hasClass('hidden')) {
                    $('#contenedor_tarjetas').addClass('hidden');
                }
                $('#guardarTarjetas_m').removeClass('hidden');
                GenerarErrorAlerta(data.Mensaje, "errorTarjetas");
                goAlert();
            }

        });
        posting.fail(function (data, status, xhr) {
            console.log(data);
            GenerarErrorAlerta(xhr, "errorTarjetas");
            goAlert();
        });
        //location.reload();
        LimpiarTabla("#tableRazones_m");
        LimpiarTabla("#tableHerramientas_m");
        LimpiarInput(['#txt_observacion_m'], ['#txt_cuenta_m'], ['#txt_tarjeta_m']);
        DataTableRazonTar = 0;
        HerramTarCont = 0;
        HerramTar = 0;
        //LimpiarInput()
        //$('#cmb_resultado_m').

        
       
    }
    else {
        GenerarErrorAlerta(message, "errorTarjetas");
        goAlert();
    }
    
}

function addRowTarjetasRazones(ArrayData, tableID, counter) {
    var CitaId = $('#hidden_citaId').val();
    var newRow = $(tableID).dataTable().fnAddData([
        counter,
        ArrayData['tipoRazonAbreviatura'],
        ArrayData['RazonDescripcion'],
        ArrayData['TipoEtiquetaListadoExtra'],
        ArrayData['listadoExtraDescripcion'],
        "<button data-Cuenta='" + ArrayData['CitaCuenta'] + "' data-Tarjeta='" + ArrayData['CitaTarjeta'] + "' data-tipoId='" + ArrayData['TipoId'] + "' data-DatoExtraId='" + ArrayData['DatoExtraId'] + "' data-TipoId='" + ArrayData['TipoId'] + "'  data-RazonId='" + ArrayData['RazonId'] + "' title='Eliminar Razón' data-toggleg='tooltip' onClick='eliminarTarjetaRazon(event)' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        CitaId
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', CitaId);
    $('td', theNode)[6].setAttribute('class', 'CitaId hidden');
    $('td', theNode)[5].setAttribute('class', 'text-center');
    console.log(ArrayData['RazonOrigen']);
    //if (ArrayData['RazonOrigen'] == 'P') {
    //    $('td', theNode)[5].setAttribute('class', 'hidden');
    //}
}

function AgregarRazon_m() {
    var id = $("#hidden_citaId").val();
    var tipoRazon = $("#cod_tipo_razon_m").val();
    var razon = $("#cod_razonp_m").val();
    var idListado = $("#cod_listado_extra_m").val();
    var Cuenta = $.trim($('#txt_cuenta_m').val());
    var Tarjeta = $.trim($('#txt_tarjeta_m').val());
    try {
        if (DataTableRazonTar > 0) {
            GenerarErrorAlerta('No puede agregar mas de una razón!', "errorTarjetas");
            goAlert();
        }
        else
        {
        if (tipoRazon == '-1' || razon == '-1' || (listadoExtraGlobal == 1 && idListado == '-1')) {
            GenerarErrorAlerta('Favor seleccione información necesaria!', "errorTarjetas");
            goAlert();
        }
        else {
            var path = urljs + 'atencionCitas/AsignarTarjetaRazon';
            if (id == "") {
                GenerarErrorAlerta('Ninguna cita seleccionada!', "errorTarjetas");
                goAlert();
            }
            else {
                //JSON data
                //var cuentaOriginalEncrypted = CryptoJS.AES.encrypt(Cuenta, 'BACPANAMA');
                //var tarjetaOriginalEncrypted = CryptoJS.AES.encrypt(Tarjeta, 'BACPANAMA');            
                //var cuentaEncrypted = CryptoJS.MD5(Cuenta);
                //var tarjetaEncrypted = CryptoJS.MD5(Tarjeta);
                var cuentaOriginalEncrypted = Cuenta;
                var tarjetaOriginalEncrypted = Tarjeta;
                var cuentaEncrypted = Cuenta;
                var tarjetaEncrypted = Tarjeta;
                var dataType = 'application/json; charset=utf-8';
                var data = {
                    CitaId: id,
                    TipoRazonId: tipoRazon,
                    RazonId: razon,
                    TipoOrigenExtraId: TipoOrigenListadoExtraGlobal,
                    CodigoListadoOrigenExtraId: TipoCodigoListadoExtraGlobal,
                    DatoExtraId: idListado,
                    RazonOrigen: "A",
                    CitaCuenta: cuentaEncrypted.toString(),
                    CitaTarjeta: tarjetaEncrypted.toString(),
                    CuentaOriginal: cuentaOriginalEncrypted.toString(),
                    TarjetaOriginal: tarjetaOriginalEncrypted.toString()
                }
                var posting = $.post(path, data);
                posting.done(function (data, status, xhr) {
                    //$('#modalCitas').modal('hide');
                    //GenerarSuccessAlerta(data.Mensaje, "success");
                    //goAlert();
                    if (data.Accion) {
                        GetTarjetasRazonesByCita(id, cuentaEncrypted.toString(), tarjetaEncrypted.toString());
                        LimpiarInput([''], ['#cod_razonp_m', '#cod_tipo_razon_m', '#cod_listado_extra_m']);
                    }
                    else {
                        GenerarErrorAlerta(data.Mensaje, "errorTarjetas");
                        goAlert();
                    }
                    
                    //activarTab('inicio');
                    //GetRazonesByCita(CitaId)
                    //LimpiarInput(inputs, ['']);
                    //$("#cod_sucursal").val('');
                    //$("#cod_segmento").empty().append( new Option('No se ha cargado información','-1') );
                    //$("#cod_tramite").empty().append( new Option('No se ha cargado información','-1') );
                });
                posting.fail(function (data, status, xhr) {
                    GenerarErrorAlerta(status, "errorTarjetas");
                    goAlert();
                });
            }
        }
    }
    }
    catch (e) {
        console.log(e);
    }
}

function citas_constructor_tipos_razon_m() {
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#cod_tipo_razon_m", ' +
        '"url": "TipoRazones/GetAllTipoRazones/", ' +
        '"val": "TipoId", ' +
        '"type": "GET", ' +
        '"data": "", ' +
        '"text": "TipoDescripcion"}');
    LoadComboBox(infojson);
}

function citas_constructor_listado_extra_config_m(id) {
    $("#label_cod_listado_extra_m").text(EtiquetaListadoExtraGlobal_m);
    $("#listadoExtraContainer_m").removeClass('hide');
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#cod_listado_extra_m", ' +
        '"url": "configuracion/getParametosByIdEncabezado/", ' +
        '"val": "ConfigItemID", ' +
        '"type": "GET", ' +
        '"data": "' + id + '", ' +
        '"text": "ConfigItemDescripcion"}');
    LoadComboBox(infojson);
}

function citas_constructor_razones_m(Id) {
    $("#cod_razonp_m").empty().append(new Option('No se ha cargado información', '-1'));
    try {
        var path = urljs + "/Razones/GetAll";
        var posting = $.post(path, { TipoId: Id });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    $("#cod_razonp_m").empty().append(new Option('Ninguno', '-1'));
                    for (var i = data.length - 1; i >= 0; i--) {
                        $("#cod_razonp_m").append(new Option(data[contador].RazonDescripcion, data[contador].RazonId));
                        contador++;
                    }
                }
                else {
                    GenerarErrorAlerta(data[0].Mensaje, "errorTarjetas");
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
}

function citas_constructor_listado_extra_canal_m() {
    $("#label_cod_listado_extra_m").text(EtiquetaListadoExtraGlobal_m);
    $("#listadoExtraContainer_m").removeClass('hide');
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#cod_listado_extra_m", ' +
        '"url": "sucursales/getSucursalesCanal/", ' +
        '"val": "SucursalId", ' +
        '"type": "GET", ' +
        '"data": "", ' +
        '"text": "SucursalNombre"}');
    LoadComboBox(infojson);
}

function checkListadoExtra_m(tipoId) {
    try {

        var path = urljs + "/TipoRazones/GetOne";
        var posting = $.post(path, { id: Number(tipoId) });
        posting.done(function (data, status, xhr) {
            if (data.Accion == 1 && data.TipoTieneListadoExtra == 1) {
                listadoExtraGlobal_m = 1;
                EtiquetaListadoExtraGlobal_m = data.TipoEtiquetaListadoExtra;
                TipoOrigenListadoExtraGlobal_m = data.TipoOrigenListadoExtra;
                TipoCodigoListadoExtraGlobal_m = data.TipoCodigoListadoExtra;
            }
            else {
                listadoExtraGlobal_m = 0;
            }
        });
        posting.fail(function (data, status, xhr) {
            //
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

function buscarEmisorCuenta_m(EmisorCuenta) {
    try {
        var path = urljs + "/citas/CheckEmisorCuenta";
        var posting = $.post(path, { EmisorCuenta: EmisorCuenta });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                //console.log(data);
                if (data[0].Accion > 0) {
                    var longitudCuenta = $("#txt_cuenta_m").val().length;
                    var cuentaFormateada = $("#txt_cuenta_m").val().substr(0, 6) + ' **** **** ' + $("#txt_cuenta_m").val().substring(longitudCuenta - 4, longitudCuenta);
                    $("#txt_cuenta_m").addClass('hide');
                    $("#txt_cuenta_formato_m").val(cuentaFormateada);
                    $("#txt_cuenta_formato_m").removeClass('hide');

                    jQuery.ajaxSetup({ async: false });
                    $('#cod_segmento_m').val(data[0].CitaSegmentoId);
                    $('#cod_segmento_m').trigger('change');
                    jQuery.ajaxSetup({ async: true });

                    //$("#txt_cuenta_n").val(EmisorCuenta);
                    $("#emisorId_m").val(data[0].EmisorId);
                    $("#txt_marca_m").val(data[0].MarcaTarjeta);
                    $("#txt_producto_m").val(data[0].Producto);
                    $("#txt_familia_m").val(data[0].Familia);
                }
                else {
                    jQuery.ajaxSetup({ async: false });
                    $('#cod_segmento').val('-1').trigger('change');
                    jQuery.ajaxSetup({ async: true });

                    $("#txt_cuenta_m").val('');
                    $('#txt_cuenta_formato_m').val();
                    $("#emisorId_m").val('');
                    $("#txt_marca_m").val('');
                    $("#txt_producto_m").val('');
                    $("#txt_familia_m").val('');
                    GenerarErrorAlerta(data[0].Mensaje, "successTarjetas");
                    goAlert();
                }
            }
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "errorTarjetas");
            goAlert();
            //$("#txt_tarjeta_n").focus();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {
        //RemoveAnimation("body");
        console.log(e);
        //$("#txt_tarjeta_n").focus();
    }
}

function eliminarTarjetaRazon(e) {

    var citaId = $('#hidden_citaId').val();
    var tipoRazonId = $(e.currentTarget).attr('data-TipoId');
    var razonId = $(e.currentTarget).attr('data-RazonId');
    var datoExtraId = $(e.currentTarget).attr('data-DatoExtraId');
    var Cuenta = $(e.currentTarget).attr('data-Cuenta');
    var Tarjeta = $(e.currentTarget).attr('data-Tarjeta');
    var path = urljs + 'AtencionCitas/deleteTarjetaRazon';
    var posting = $.post(path, { citaId: Number(citaId), tipoRazonId: Number(tipoRazonId), razonId: Number(razonId), datoExtraId: datoExtraId, Cuenta: Cuenta, Tarjeta: Tarjeta });
    posting.done(function (data, status, xhr) {
        if (data.Accion > 0) {
            GetTarjetasRazonesByCita(citaId, Cuenta, Tarjeta);
        }
        else {
            GenerarErrorAlerta(data.Mensaje, "errorTarjetas");
            goAlert();
        }
    });
    posting.fail(function (data, status, xhr) {
        //sendError(data);
        GenerarErrorAlerta(xhr, "errorTarjetas");
        goAlert();
    });
    posting.always(function () {
        /*console.log("Se cargo los proyectos correctamente");*/
        //RemoveAnimation("body");
    });
}

function eliminarTarjeta(e) {

    var citaId = $('#hidden_citaId').val();
    var Cuenta = $(e.currentTarget).attr('data-cuenta');
    var Tarjeta = $(e.currentTarget).attr('data-tarjeta');
    var path = urljs + 'AtencionCitas/deleteTarjeta';
    var posting = $.post(path, { citaId: Number(citaId), Cuenta: Cuenta, Tarjeta: Tarjeta });
    posting.done(function (data, status, xhr) {
        if (data.Accion > 0) {
            LoadMasTarjetas();
        }
        else {
            GenerarErrorAlerta(data.Mensaje, "errorTarjetas");
            goAlert();
        }
    });
    posting.fail(function (data, status, xhr) {
        //sendError(data);
        GenerarErrorAlerta(xhr, "errorTarjetas");
        goAlert();
    });
    posting.always(function () {
        /*console.log("Se cargo los proyectos correctamente");*/
        //RemoveAnimation("body");
    });
}

function editarTarjeta(e) {
    var citaId = $('#hidden_citaId').val();
    var Cuenta = $(e.currentTarget).attr('data-cuenta');
    var Tarjeta = $(e.currentTarget).attr('data-tarjeta');        
    var cuentaOriginalDecrypted = $(e.currentTarget).attr('data-CuentaOriginal'); //CryptoJS.AES.decrypt($(e.currentTarget).attr('data-CuentaOriginal'), 'BACPANAMA');
    var tarjetaOriginalDecrypted = $(e.currentTarget).attr('data-TarjetaOriginal'); //CryptoJS.AES.decrypt($(e.currentTarget).attr('data-TarjetaOriginal'), 'BACPANAMA');
    var Resolucion = $(e.currentTarget).attr('data-resolucion');
    var Obs = $(e.currentTarget).attr('data-comentario');
    $('#txt_tarjeta_m').val(tarjetaOriginalDecrypted.toString()); //CryptoJS.enc.Utf8));
    $('#txt_cuenta_m').val(cuentaOriginalDecrypted.toString()).trigger('change'); //CryptoJS.enc.Utf8)).trigger('change');
    $("#emisorId_m").val('');
    $("#txt_marca_m").val('');
    $("#txt_producto_m").val('');
    $("#txt_familia_m").val('');
    $("#txt_observacion_m").val(Obs);
    $('#cod_segmento_m').val('-1').trigger('change');
    $('#cmb_resultado_m').val(Resolucion).trigger('change');
    $('#masTarjetasTitle').html('Agregar Tarjetas');
    $('#masTarjetasModal').modal('show');
    $("#txt_cuenta_formato").trigger('dblclick');
    $("#txt_tarjeta_formato").trigger('dblclick');
    $('#guardarTarjetas_m').addClass('hidden');
    $('#txt_tarjeta_m').prop("disabled", true);
    $('#txt_cuenta_m').prop("disabled", true);
    $(".cuentaFormato_m").blur();
    $('#contenedor_tarjetas').removeClass('hidden');
    GetTarjetasRazonesByCita(citaId, Cuenta, Tarjeta);
    GetHerramientasByCita_m(citaId, Cuenta, Tarjeta);
}


function AgregarHerramientas_m() {
    var id = $("#hidden_citaId").val();
    var herramienta = $("#cmbHerramientas_m").val();
    var obs = $('#txtObsHerramienta_m').val();
    var Cuenta = $.trim($('#txt_cuenta_m').val());
    var Tarjeta = $.trim($('#txt_tarjeta_m').val());
    var Emisor = $('#emisorId_m').val();
    //var cuentaEncrypted = CryptoJS.AES.encrypt(Cuenta, 'BACPANAMA');
    //var tarjetaEncrypted = CryptoJS.AES.encrypt(Tarjeta, 'BACPANAMA');
    //var cuentaEncrypted = Cuenta;
    //var tarjetaEncrypted = Tarjeta;
    if (Tarjeta == "") {
        Tarjeta = "-1";
    }
    var cuentaEncrypted = Cuenta; //CryptoJS.MD5(Cuenta);
    var tarjetaEncrypted = Tarjeta; //CryptoJS.MD5(Tarjeta);
    try {
        if (HerramTarCont > 5) {
            GenerarErrorAlerta('No debe ingresar mas de cinco herramientas!', "errorTarjetas");
            goAlert();
        }
        else 
        {
        if (herramienta == '-1') {
            GenerarErrorAlerta('Favor seleccione un tipo de herramienta!', "errorTarjetas");
            goAlert();
        }
        else if (id == "") {
            GenerarErrorAlerta('Debe de seleccionar una cita para continuar!', "errorTarjetas");
            goAlert();
        }
        else if (Cuenta == "") {
            GenerarErrorAlerta('Debe ingrear una Cuenta para continuar!', "errorTarjetas");
            goAlert();
        }
        /*else if (Tarjeta == "") {
            GenerarErrorAlerta('Debe de ingresar una tarjeta para continuar!', "errorTarjetas");
            goAlert();
        }*/
        else if (Emisor == "") {
            GenerarErrorAlerta('Debe de ingresar los datos solicitados para continuar!', "errorTarjetas");
            goAlert();
        }
        else {
            //JSON data
            var path = urljs + 'atencionCitas/AgregarTarjetaHerramienta';
            var dataType = 'application/json; charset=utf-8';
            var data = {
                CitaId: id,
                CitaCuenta: cuentaEncrypted.toString(),
                CitaTarjeta: tarjetaEncrypted.toString(),
                HerramientaId: herramienta,
                HerramientaObservacion: jQuery.trim(obs)
            }
            var posting = $.post(path, data);
            posting.done(function (data, status, xhr) {
                if (data.Accion) {
                    GetHerramientasByCita_m(data.CitaId, cuentaEncrypted.toString(), tarjetaEncrypted.toString());
                    LimpiarInput(['#txtObsHerramienta_m'], ['#cmbHerramientas_m']);
                }
                else {
                    GenerarErrorAlerta(data.Mensaje, "errorTarjetas");
                    goAlert();
                }
            });
            posting.fail(function (data, status, xhr) {
                //console.log(data);
                GenerarErrorAlerta(status, "errorTarjetas");
                goAlert();
            });
            //HerramTar = true;
        }
        }
    }
    catch (e) {
        console.log(e);
    }
}

var HerramTarCont = 0;
var HerramTar = 0;
function GetHerramientasByCita_m(CitaId, Cuenta, Tarjeta) {
    try {
        var path = urljs + "/atencionCitas/GetTarjetaHerramientasByCita";
        var posting = $.post(path, { CitaId: CitaId, Cuenta: Cuenta, Tarjeta: Tarjeta });
        posting.done(function (data, status, xhr) {
            LimpiarTabla("#tableHerramientas_m");
            if (data.length > 0) {
                if (data[0].Accion > 0) {
                    var counter = 1;
                    for (var i = data.length - 1; i >= 0; i--) {
                        //console.log(data[i]);
                        addRowHerramientas_m(data[i], "#tableHerramientas_m", counter);
                        counter++;
                    }
                }
                else {
                    /*GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();*/
                }
                if (counter > 0) {
                    HerramTarCont = counter;
                }
            }
            HerramTar = data[0].Accion;
        });
        posting.fail(function (data, status, xhr) {
            //
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

function addRowHerramientas_m(ArrayData, tableID) {
    var newRow = $(tableID).dataTable().fnAddData([
        ArrayData['HerramientaDescripcion'],
        ArrayData['HerramientaObservacion'],
        "<button data-cuenta='" + ArrayData['CitaCuenta'] + "' data-tarjeta='" + ArrayData['CitaTarjeta'] + "' data-Id='" + ArrayData['HerramientaId'] + "' data-CitaId='" + ArrayData['CitaId'] + "' title='Eliminar Herramienta' data-toggleg='tooltip' onClick='eliminarTarjetasHerramienta(event)' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        ArrayData['CitaId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['CitaId']);
    $('td', theNode)[3].setAttribute('class', 'CitaId hidden');
    $('td', theNode)[2].setAttribute('class', 'text-center');
}

function eliminarTarjetasHerramienta(e) {
    var HerramientaId = $(e.currentTarget).attr('data-Id');
    var CitaId = $(e.currentTarget).attr('data-CitaId');
    var Cuenta = $(e.currentTarget).attr('data-cuenta');
    var Tarjeta = $(e.currentTarget).attr('data-tarjeta');
    var path = urljs + 'atencionCitas/deleteTarjetaHerramienta';
    var posting = $.post(path, { CitaId: Number(CitaId), HerramientaId: HerramientaId, Cuenta: Cuenta, Tarjeta: Tarjeta });
    posting.done(function (data, status, xhr) {
        if (data.Accion > 0) {
            GetHerramientasByCita_m(CitaId, Cuenta, Tarjeta);
        }
        else {
            GenerarErrorAlerta(data.Mensaje, "error");
            goAlert();
        }
    });
    posting.fail(function (data, status, xhr) {
        //sendError(data);
        GenerarErrorAlerta(xhr, "error");
        goAlert();
    });
}

