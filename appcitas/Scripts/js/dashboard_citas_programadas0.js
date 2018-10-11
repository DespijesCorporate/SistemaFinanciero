var hoy          = moment(new Date()).format('YYYY-MM-DD');
var hoy_max_date = moment(new Date(hoy)).add(14, 'days');
$(document).ready(function () {
    checkUserAccess('CYRS020');
    jQuery.ajaxSetup({ global: false });
    reportes_constructor_sucursales();
    reportes_constructor_cubiculo_por_sucursal(-1);
    jQuery.ajaxSetup({ global: false });
    jQuery.ajaxSetup({ async: false });
    citas_get_maxDate_schedule();
    jQuery.ajaxSetup({ async: true });
});

function citas_get_maxDate_schedule(){
    hoy_max_date = moment(new Date(hoy)).add(14, 'days');
    try {
        var path = urljs + "/configuracion/getParametosByIdEncabezado";
        var posting = $.post(path, { ConfigId: 'SCHDL', ConfigItemId: 'MAXDATE' });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    hoy_max_date = moment(new Date(hoy)).add(data[0].ConfigItemAbreviatura.toString(), 'days');
                }
            }
        });
    }
    catch (e) {
        hoy_max_date = moment(new Date(hoy)).add(14, 'days');
    }
}

$(function () {
    $('#fecha1').datetimepicker({
        locale: 'es',
        minDate: hoy,
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
        maxDate: hoy_max_date,
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
                    $("#cod_sucursal").empty().append( new Option('Todos los centros','-1') );
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

function reportes_constructor_cubiculo_por_sucursal(sucursalId){
    $("#cod_cubiculo").empty().append( new Option('No se ha cargado información','-1') );
    try {
        var path = urljs + "/sucursales/GetCubiculosBySucursal";
        var posting = $.post(path, { sucursalid: sucursalId });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    $("#cod_cubiculo").empty().append( new Option('Todos los cubículos','-1') );
                    for (var i = data.length - 1; i >= 0; i--) {
                        $("#cod_cubiculo").append(new Option(data[contador].posicionDescripcion, data[contador].PosicionId));
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
    reportes_constructor_cubiculo_por_sucursal($(this).val());
});

var inputs = [];var mensaje = [];
$('#btn_crear_reporte_dashboard1').on('click', function () {
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
            var path = urljs + 'Reportes/ReporteEfectividad';
            var cod_sucursal    = $("#cod_sucursal").val() == ""?-1:$("#cod_sucursal").val();
            var cod_tipo_cita   = $("#cod_tipo_cita").val() == ""?-1:$("#cod_tipo_cita").val();
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
                var excel_cols = [0,1,2,3,4,5,6];
                LimpiarTablaExcelDasboard1("#tableCitasDashboard1",excel_cols, 'Reporte de Efectividad');
                if (data.length > 0) {
                    if (data[0].Accion > 0) {
                        var counter = 1;
                        jQuery.ajaxSetup({ async: false });
                        var retenidas = 0, noRetenidas = 0;
                        for (var i = data.length - 1; i >= 0; i--) {
                            retenidas += data[i]['citas_retenidas'];
                            noRetenidas += data[i]['citas_noRetenidas'];
                            addRowDashboard1(data[i], "#tableCitasDashboard1", counter);
                            counter++;
                        }
                        var efectividad = (retenidas/(retenidas+noRetenidas))*100;
                        efectividad = efectividad.toFixed(2);
                        if( retenidas+noRetenidas == 0 ){
                            efectividad = 0.00;
                        }
                        addRowDashboardTotal("Totales",retenidas,noRetenidas,efectividad,"#tableCitasDashboard1");
                        /*addRowDashboardTotal("Porcentaje - %",porcentaje1,porcentaje2,porcentaje3,porcentaje4,porcentaje5,'100.00',"#tableCitasDashboard1");
                        addRowDashboardTotal("Acumulado - %",cumulative1,cumulative2,cumulative3,cumulative4,cumulative5,'',"#tableCitasDashboard1");
                        */jQuery.ajaxSetup({ async: true });
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
    var efectividad = ((ArrayData['citas_retenidas'])/(ArrayData['citas_retenidas']+ArrayData['citas_noRetenidas']))*100;
    efectividad = efectividad.toFixed(2);
    if( (ArrayData['citas_retenidas']+ArrayData['citas_noRetenidas']) == 0 ){
        efectividad = 0.00
    }
    var newRow = $(tableID).dataTable().fnAddData([
        counter,
        ArrayData['CitaTipo'],
        ArrayData['sucursal'],
        ArrayData['ejecutivoId'],
        ArrayData['citas_retenidas'],
        ArrayData['citas_noRetenidas'],
        efectividad
    ]);

    var filaTR = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    $('td', filaTR)[6].setAttribute('class','celda-subtotal');
}

function addRowDashboardTotal(col0,col1,col2,col3,tableID) {
    var newRow = $(tableID).dataTable().fnAddData([
        '',
        '',
        '',
        col0,
        col1,
        col2,
        col3
    ]);

    var filaTR = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    $(filaTR)[0].setAttribute('class','fila-resumen');
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