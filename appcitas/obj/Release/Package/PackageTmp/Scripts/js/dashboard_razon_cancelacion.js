var hoy = moment(new Date()).format('YYYY-MM-DD');
$(document).ready(function () {
    checkUserAccess('CYRS020');
    jQuery.ajaxSetup({ global: false });
    reportes_constructor_sucursales();
    reportes_constructor_tipos_cita(-1);
    reportes_constructor_tipos_razon('-1');
    reportes_constructor_ejecutivos('-1');
    jQuery.ajaxSetup({ global: false });
});

$(function () {
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

function reportes_constructor_sucursales(){
    $("#cod_sucursal").empty().append( new Option('No se ha cargado información','-2') );
    try {
        //var path = urljs + "/sucursales/GetAll";
        var path = urljs + "/sucursales/getSucursalesCentroAtencion";
        var posting = $.post(path);
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    $("#cod_sucursal").empty().append( new Option('Todos los centros','0') );
                    for (var i = data.length - 1; i >= 0; i--) {
                        $("#cod_sucursal").append(new Option(data[contador].SucursalNombre, data[contador].SucursalId));
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
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {

    }
}

function reportes_constructor_tipos_cita(){
    $("#cod_tipo_cita").empty().append( new Option('No se ha cargado información','-1') );
    try {
        var path = urljs + "/configuracion/getParametosByIdEncabezado";
        var posting = $.post(path, { ConfigId: 'TCITA' });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    $("#cod_tipo_cita").empty().append( new Option('Todos los tipos de atención','-2') );
                    for (var i = data.length - 1; i >= 0; i--) {
                        $("#cod_tipo_cita").append(new Option(data[contador].ConfigItemDescripcion, data[contador].ConfigItemID));
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
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {

    }
}

function reportes_constructor_ejecutivos(IdSuc) {
    jQuery.ajaxSetup({ async: false });
    $('#cod_ejecutivo').empty();
    if (IdSuc != 0) {
        var infojson = jQuery.parseJSON('{"input": "#cod_ejecutivo", ' +
            '"url": "atencionCitas/GetCubiculosBySucursal/", ' +
            //'"val": "ConfigId", ' +
            '"val": "PosicionId", ' +
            '"type": "GET", ' +
            '"data": "' + IdSuc + '", ' +
            '"text": "posicionDescripcion"}');
        LoadComboBox(infojson);
    }
    $('#cod_ejecutivo').append('<option value="0">Todos</option>');
}

function reportes_constructor_tipos_razon(){
    $("#tipo_razon").empty().append( new Option('No se ha cargado información','-1') );
    try {
        var path = urljs + "/configuracion/getParametosByIdEncabezado";
        var posting = $.post(path, { ConfigId: 'GRPR' });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    $("#tipo_razon").empty().append( new Option('Todos los tipos de razón','-1') );
                    for (var i = data.length - 1; i >= 0; i--) {
                        $("#tipo_razon").append(new Option(data[contador].ConfigItemDescripcion, data[contador].ConfigItemID));
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
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {

    }
}

$('#cod_sucursal').on('change', function (event) {
    console.log($(this).val());
    reportes_constructor_ejecutivos($(this).val());
});

var inputs = [];var mensaje = [];
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
            var path = urljs + 'Reportes/DashboardRazonCancelacion';
            var cod_sucursal    = $("#cod_sucursal").val() == ""?-1:$("#cod_sucursal").val();
            var cod_tipo_cita   = $("#cod_tipo_cita").val() == "" ? -1 : $("#cod_tipo_cita").val();
            var ejecutivo       = $("#cod_ejecutivo").val() == ""?-1:$("#cod_ejecutivo").val();
            var tipoRazon       = $("#tipo_razon").val() == ""?-1:$("#tipo_razon").val();
            var fecha1          = $("#txt_fecha1").val();
            var fecha2          = $("#txt_fecha2").val();
            
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {
                SucursalId: cod_sucursal,
                tipoCita: cod_tipo_cita,
                ejecutivo: ejecutivo,
                tipoRazon: tipoRazon,
                fecha1: fecha1,
                fecha2: fecha2
            }
            var posting = $.post(path, data);
            posting.done(function (data, status, xhr) {
                var excel_cols = [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18];
                LimpiarTablaExcel("#tableCitasDashboard1",excel_cols, 'Dashboard razon cancelacion');
                if (data.length > 0) {
                    if (data[0].Accion > 0) {
                        var counter = 1;
                        jQuery.ajaxSetup({ async: false });
                        for (var i = data.length - 1; i >= 0; i--) {
                            addRowDashboard1(data[i], "#tableCitasDashboard1", counter);
                            counter++;
                        }
                        jQuery.ajaxSetup({ async: true });
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
            GenerarErrorAlerta("No estan llenos todos los filtros requeridos", "error");
            goAlert();
        }
    }
    catch (e) {
        console.log(e);
    }
});

function addRowDashboard1(ArrayData, tableID, counter) {
    var cuentaDecrypted = ArrayData['CitaCuenta']; //CryptoJS.AES.decrypt(ArrayData['CitaCuenta'], 'BACPANAMA');
    //var tarjetaDecrypted = CryptoJS.AES.decrypt(ArrayData['CitaTarjeta'], 'BACPANAMA');
    var tarjetaDecrypted = ArrayData['CitaTarjeta']; //CryptoJS.AES.decrypt(ArrayData['CitaTarjeta'], 'BACPANAMA');
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
        ArrayData['CodigoRazon'],
        ArrayData['CitaUsuarioAtendio'],
        ArrayData['posicionDescripcion'],
        ArrayData['tramite'],
        ArrayData['CitaTipo'],
        ArrayData['Razones'],
        ArrayData['ListadoExtra'],
        ArrayData['CitaNombre'],
        ArrayData['estado'],
        (cuentaDecrypted.toString(CryptoJS.enc.Utf8) == '' ? 'N/D' : cuentaDecrypted.toString(CryptoJS.enc.Utf8)),
        //(tarjetaDecrypted.toString(CryptoJS.enc.Utf8) == '' ? 'N/D' : tarjetaDecrypted.toString(CryptoJS.enc.Utf8)),
        (tarjetaDecrypted == '' ? 'N/D' : tarjetaDecrypted),
        ArrayData['segmento'],
        ArrayData['Marca'],
        ArrayData['Familia'],
        ArrayData['TipoRazon'],
        ArrayData['Resolucion'],
        ArrayData['ComentResolucion'],
        ArrayData['CitaFecha']
    ]);

    var filaTR = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
}

function LimpiarTablaExcelDasboard1(TableName, Columns, Filename) {
    //Limpiamos la tabla y ocultamos los items.
    $(TableName).dataTable().fnClearTable();
    $(TableName).dataTable().fnDestroy();
    var table = $(TableName).DataTable({
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
        ],
        "order": [[ 1, 'desc' ]]
    });
}
