var hoy = moment(new Date()).format('YYYY-MM-DD');
function LoadSucursales() {
    var infojson = jQuery.parseJSON('{"input": "#cmb_sucursal", ' +
        '"url": "/sucursales/getSucursalesByAtencion", ' +
        '"val": "SucursalId", ' +
        '"type": "GET", ' +
        '"data": "", ' +
        '"text": "SucursalNombre"}');
    LoadComboBox(infojson);
}

function citas_constructor_razones(Id) {
    $("#cod_razonp").empty().append(new Option('No se ha cargado información', '-1'));
    if (Id !== 0) {
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
                        $('#cod_razonp').append('<option value="0">Todas</option>');
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
    else
    {
        $('#cod_razonp').empty().append('<option value="0">Todas</option>');
    }
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
    $('#cod_tipo_razon').append('<option value="0">Todas</option>');
}

$("#cod_tipo_razon").on('change', function (e) {
    //console.log('cod_tipo_razon');
    //$("#listadoExtraContainer").addClass('hide');
    var tipoId = $(this).val();
    if (tipoId !== '' && tipoId !== '-1') {
        jQuery.ajaxSetup({ async: false });
        citas_constructor_razones(tipoId);
        jQuery.ajaxSetup({ async: true });
    }
    else {
        $("#cod_razonp").empty().append(new Option('No se ha cargado información', '-1'));
    }
});


$(document).ready(function () {
    checkUserAccess('CYRS100');
    LoadSucursales();
    citas_constructor_tipos_razon();
});

$(function () {
    $('#txt_fecha1').datetimepicker({
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
});

var inputs = ['#txt_fecha1', '#txt_fecha2', '#cmb_sucursal'];
var mensaje = ['Campo requerido', 'Campo requerido', 'Campo requerido'];
$('#btn_crear_reporte').on('click', function () {
    try {
        var inputs = [];
        var mensaje = [];
        $('.requerido').each(function () {
            /*Llenamos los arreglos con la info para la validacion*/
            if ($(this).data('requerido') === true) {
                inputs.push('#' + $(this).attr('id'));
                mensaje.push($(this).attr('attr-message'));
            }
        });
        if (Validar(inputs, mensaje)) {
            var path = urljs + 'Reportes/ReporteCitasDiarias';
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {
                fecha1: $("#txt_fecha1").val(),
                sucursalid: $("#cmb_sucursal").val(),
                codtiporazon: $('#cod_tipo_razon').val(),
                codrazon: $('#cod_razonp').val()
            }
            var posting = $.post(path, data);
            posting.done(function (data, status, xhr) {
                LimpiarTablaExcel("#tableCitas", [0, 1, 2, 3, 4, 5, 6, 7], 'Reporte Citas Diarias');
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
        ArrayData['CitaIdentificacion'],
        ArrayData['CitaNombre'],
        ArrayData['tramite'],
        ArrayData['posicionDescripcion'],
        moment(ArrayData['CitaHoraInicioCompleta'], 'DD/MM/YYYY HH:mm:ss').format("YYYY/MM/DD hh:mm a"),
        moment(ArrayData['CitaHoraFinCompleta'], 'DD/MM/YYYY HH:mm:ss').format("YYYY/MM/DD hh:mm a"),
        ArrayData['Razones'],
        ArrayData['CitaId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['CitaId']);
    $('td', theNode)[8].setAttribute('class', 'CitaId hidden');
}