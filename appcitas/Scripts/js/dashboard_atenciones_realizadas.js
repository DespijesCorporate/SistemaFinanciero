var hoy = moment(new Date()).format('YYYY-MM-DD');
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

function LoadTipoAtencion() {
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#cmb_tipo_atencion", ' +
        '"url": "configuracion/getParametosByIdEncabezado/", ' +
        '"val": "ConfigItemID", ' +
        '"type": "GET", ' +
        '"data": "TCITA", ' +
        '"text": "ConfigItemObservacion"}');
    LoadComboBox(infojson);
    $('#cmb_tipo_atencion').append('<option value="-2">Todos</option>');
}

var hoy = moment(new Date()).format('YYYY-MM-DD');
$(document).ready(function () {
    checkUserAccess('CYRS010');
    LoadSucursales();
    LoadTipoAtencion();
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
});

$(document).ready(function () {
    checkUserAccess('CYRS100');
    LoadSucursales();
});


var inputs = ['#txt_fecha1', '#txt_fecha2', '#cmb_sucursal'];
var mensaje = ['Campo requerido', 'Campo requerido', 'Campo requerido'];
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
            var path = urljs + 'Reportes/ReporteAtencionesRealizadas';
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {
                fecha1: $("#txt_fecha1").val(),
                fecha2: $("#txt_fecha2").val(),
                sucursalid: $("#cmb_sucursal").val(),
                tipoatencion: $("#cmb_tipo_atencion").val()
            }
            var posting = $.post(path, data);
            posting.done(function (data, status, xhr) {
                LimpiarTablaExcel("#tableCitas", [0, 1, 2, 3, 4, 5], 'Dashboard de Operación');
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
var newRow = $(tableID).dataTable().fnAddData([
        counter,
        ArrayData['DiaSemana'],
        ArrayData['CitaFecha'],
        ArrayData['citasAtendidas'],
        (ArrayData["TiempoEspera"] < 0 ? '0' : ArrayData["TiempoEspera"]),
        (ArrayData["TiempoEnCita"] < 0 ? '0' : ArrayData["TiempoEnCita"])
]);

var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
}
