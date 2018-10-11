function reportes_constructor_estados_citas() {
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#cod_estado", ' +
        '"url": "configuracion/getParametosByIdEncabezado/", ' +
        '"val": "ConfigItemID", ' +
        '"type": "GET", ' +
        '"data": "ESTD", ' +
        '"text": "ConfigItemDescripcion"}');
    LoadComboBox(infojson);
    $('#cod_estado').append('<option value="7">Todos</option>');
}

function LoadSucursales() {
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#cmb_sucursal", ' +
        '"url": "/sucursales/getSucursalesByAtencion", ' +
        '"val": "SucursalId", ' +
        '"type": "GET", ' +
        '"data": "", ' +
        '"text": "SucursalNombre"}');
    LoadComboBox(infojson);
    $('#cmb_sucursal').append('<option value="0">Todos</option>');
}

function LoadCubiculos(IdSuc) {
    jQuery.ajaxSetup({ async: false });
    $('#cmb_cubiculo').empty();
    if (IdSuc != 0) {
        var infojson = jQuery.parseJSON('{"input": "#cmb_cubiculo", ' +
            '"url": "atencionCitas/GetCubiculosBySucursal/", ' +
            '"val": "PosicionId", ' +
            '"type": "GET", ' +
            '"data": "' + IdSuc + '", ' +
            '"text": "posicionDescripcion"}');
        LoadComboBox(infojson);
    }
    $('#cmb_cubiculo').append('<option value="0">Todos</option>');
}

var hoy = moment(new Date()).format('YYYY-MM-DD');
$(document).ready(function () {
    checkUserAccess('CYRS010');
    LoadSucursales();
    reportes_constructor_estados_citas();
    $('#fecha1').datetimepicker({
        locale: 'es',
        maxDate: hoy,
        defaultDate: hoy,
        format: 'YYYY-MM-DD',
        ignoreReadonly: true,
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down",
            left: "fa fa-arrow-left",
            right: "fa fa-arrow-right"
        }
    });
    $('#fecha2').datetimepicker({
        locale: 'es',
        minDate: hoy,
        defaultDate: hoy,
        format: 'YYYY-MM-DD',
        ignoreReadonly: true,
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down",
            left: "fa fa-arrow-left",
            right: "fa fa-arrow-right"
        },
        useCurrent: false
    });
    $("#fecha1").on("dp.change", function (e) {
        $('#fecha2').data("DateTimePicker").minDate(e.date);
    });
    $("#fecha2").on("dp.change", function (e) {
        $('#fecha1').data("DateTimePicker").maxDate(e.date);
    });

    $('#cmb_sucursal').change(function (e) {
        var id = $(this).val();
        if (id != "-1") {
            //console.log(id + '-');
            LoadCubiculos(id);
        }
        //else
        //{
        //    LimpiarInput([""], ["#cmb_sucursal", "#cmb_cubiculo"]);
        //}
    });
});

var inputs = ['#cod_estado','#cmb_sucursal'];
var mensaje = ['Campo requerido', 'Campo requerido'];
$('#btn_crear_reporte').on('click', function () {
    try {
        var inputs = [];
        var mensaje = [];
        $('.requerido').each(function () {
            /*Llenamos los arreglos con la info para la validacion*/
            if ($(this).data('requerido') == true) {
                inputs.push('#' + $(this).attr('id'));
                mensaje.push($(this).attr('attr-message'));
            }
        });
        if (Validar(inputs, mensaje)) {
            var path = urljs + 'Reportes/ReporteCitasPorEstado';
            var cod_estado = $("#cod_estado").val();
            var cmb_cubiculo = $("#cmb_cubiculo").val();
            var fecha1 = $("#txt_fecha1").val();
            var fecha2 = $("#txt_fecha2").val();
            if (cod_estado == "") {
                cod_estado = -1;
            }
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {
                estadocita: $("#cod_estado").val(),
                sucursalid: $("#cmb_sucursal").val(),
                cmb_cubiculo: $("#cmb_cubiculo").val(),
                fecha1: fecha1,
                fecha2: fecha2
            }
            var posting = $.post(path, data);
            posting.done(function (data, status, xhr) {
                LimpiarTablaExcel("#tableCitas", [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25], 'Dashboard de citas');
                if (data.length > 0) {
                    if (data[0].Accion > 0) {
                        var counter = 1;
                        for (var i = data.length - 1; i >= 0; i--) {
                            addRow(data[i], "#tableCitas", counter);
                            counter++;
                        }
                    }
                    else {
                        GenerarErrorAlerta(data[0].Mensaje, "error");
                        goAlert();
                    }
                }

            });
            posting.fail(function (data, status, xhr) {
                //console.log(status);
            });
            //}
        }
        else {
            GenerarErrorAlerta("No estan llenos todos los campos requeridos", "successModalTicketWK");
            goAlert();
        }
    }
    catch (e) {
        console.log(e);
    }
});

function addRow(ArrayData, tableID, counter) {

    var cuentaDecrypted = ArrayData['CitaCuenta']; //CryptoJS.AES.decrypt(ArrayData['CitaCuenta'], 'BACPANAMA');
    /*
    var tarjetaDecrypted = CryptoJS.AES.decrypt(ArrayData['CitaTarjeta'], 'BACPANAMA');
    console.log('cuentaDecrypted:' + cuentaDecrypted.toString(CryptoJS.enc.Utf8));
    console.log('tarjetaDecrypted:' + tarjetaDecrypted.toString(CryptoJS.enc.Utf8));
    */
    /*Funcion para desencriptar tarjetas*/
    var tarjetaDecrypted = ArrayData['CitaTarjeta']; //CryptoJS.AES.decrypt(ArrayData['CitaTarjeta'], 'BACPANAMA');
    // +rirl (esto verificar si se va a usar para implementarlo en los repositorios
    //tarjetaDecrypted = tarjetaDecrypted.toString(CryptoJS.enc.Utf8);
    //if (ArrayData['Tarjetas'] != '' && ArrayData['Tarjetas'] != 'N/D') {
    //    var tarjetas = ArrayData['Tarjetas'].split(",");
    //    console.log('tarjetas:' + tarjetas);
    //    $.each(tarjetas, function (index, value) {
    //        console.log('value:' + value);
    //        var tarjeta = CryptoJS.AES.decrypt(value, 'BACPANAMA');
    //        tarjeta = tarjeta.toString(CryptoJS.enc.Utf8);
    //        console.log('tarjeta:' + tarjeta);
    //        if (tarjeta != '') {
    //            tarjetaDecrypted = tarjetaDecrypted + " - " + tarjeta.toString(CryptoJS.enc.Utf8);
    //            console.log('tarjetaDecrypted2:' + tarjetaDecrypted);
    //        }
    //    })
    //}
    var newRow = $(tableID).dataTable().fnAddData([
        counter,
        ArrayData['CitaTicket'],
        ArrayData['CitaUsuarioAtendio'],
        ArrayData['posicionDescripcion'],
        ArrayData['DiaSemana'],
        ArrayData['SucursalNombre'],
        moment(ArrayData['CitaFecha'], 'YYYY/MM/DD').format("YYYY/MM/DD"),
        //moment(ArrayData['CitaHora'], 'YYYY/MM/DD HH:mm').format("YYYY/MM/DD hh:mm a"),
        //(ArrayData['CitaHoraClienteIniciaAtencion'] != '' ? moment(ArrayData['CitaHoraClienteIniciaAtencion'], 'YYYY/MM/DD HH:mm:ss').format("YYYY/MM/DD hh:mm a") : 'No definido'),
        //(ArrayData['CitaHoraClienteSaleAtencion'] != '' ? moment(ArrayData['CitaHoraClienteSaleAtencion'], 'YYYY/MM/DD HH:mm:ss').format("YYYY/MM/DD hh:mm a") : 'No definido'),
        moment(ArrayData['CitaHora'], 'YYYY/MM/DD HH:mm').format("hh:mm a"),
        (ArrayData['CitaHoraClienteIniciaAtencion'] != '' ? moment(ArrayData['CitaHoraClienteIniciaAtencion'], 'YYYY/MM/DD HH:mm:ss').format("hh:mm a") : 'No definido'),
        (ArrayData['CitaHoraClienteSaleAtencion'] != '' ? moment(ArrayData['CitaHoraClienteSaleAtencion'], 'YYYY/MM/DD HH:mm:ss').format("hh:mm a") : 'No definido'),
        (ArrayData['HoraLLego'] != '' ? moment(ArrayData['HoraLLego'], 'YYYY/MM/DD HH:mm:ss').format("hh:mm a") : 'No definido'),
        ArrayData["TiempoEnCita"],
        ArrayData["TramiteDuracion"],
        ArrayData["TiempoEspera"],
        (ArrayData['Resolucion'] == '' ? 'N/D' : ArrayData['Resolucion']),
        ArrayData['CitaNombre'],
        (cuentaDecrypted.toString(CryptoJS.enc.Utf8) == '' ? 'N/D' : cuentaDecrypted.toString(CryptoJS.enc.Utf8)),
        //(tarjetaDecrypted.toString(CryptoJS.enc.Utf8) == '' ? 'N/D' : tarjetaDecrypted.toString(CryptoJS.enc.Utf8)),
        (tarjetaDecrypted == '' ? 'N/D' : tarjetaDecrypted),
        ArrayData['segmento'],
        ArrayData['Marca'],
        ArrayData['Familia'],
        ArrayData['tramite'],
        ArrayData['Razones'],
        ArrayData['Herramientas'],
        ArrayData['estado'],
        ArrayData['OrigenCita'],
        ArrayData['CitaId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['CitaId']);
    $('td', theNode)[26].setAttribute('class', 'CitaId hidden');
}
