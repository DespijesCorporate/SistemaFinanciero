var hoy = moment(new Date()).format('YYYY-MM-DD');
$(document).ready(function () {
    checkUserAccess('CYRS030');
    jQuery.ajaxSetup({ global: false });
    reportes_constructor_sucursales();
    reportes_constructor_tipos_cita(-1);
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


function reportes_constructor_tipos_cita(){
    $("#cod_tipo_cita").empty().append( new Option('No se ha cargado información','-1') );
    try {
        var path = urljs + "/configuracion/getParametosByIdEncabezado";
        var posting = $.post(path, { ConfigId: 'TCITA' });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    $("#cod_tipo_cita").empty().append( new Option('Todos los tipos de atención','-1') );
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

$('#cod_sucursal').on('change', function (event) {
    console.log($(this).val());
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
            var path = urljs + 'Reportes/ReporteFlujoPorIntervalo';
            var cod_sucursal = $("#cod_sucursal").val() == ""?-1:$("#cod_sucursal").val();
            var cod_tipo_cita   = $("#cod_tipo_cita").val() == ""?-1:$("#cod_tipo_cita").val();
            var fecha1       = $("#txt_fecha1").val();
            var fecha2       = $("#txt_fecha2").val();
            
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {
                sucursalId: cod_sucursal,
                tipoCita: cod_tipo_cita,
                fecha1: fecha1,
                fecha2: fecha2
            }
            var posting = $.post(path, data);
            posting.done(function (data, status, xhr) {
                LimpiarTablaExcelDasboard1("#tableCitasDashboard1",[0,1,2,3,4,5,6,7], 'Reporte Flujo por intervalo');
                if (data.length > 0) {
                    if (data[0].Accion > 0) {
                        var counter = 1;
                        for (var i = data.length - 1; i >= 0; i--) {
                            addRowDashboard1(data[i], "#tableCitasDashboard1", counter);
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
            GenerarErrorAlerta("No estan llenos todos los filtros requeridos", "error");
            goAlert();
        }
    }
    catch (e) {
        console.log(e);
    }
});

function addRowDashboard1(ArrayData, tableID, counter) {
    var newRow = $(tableID).dataTable().fnAddData([
        counter,
        ArrayData['CitaTipo'],
        ArrayData['sucursal'],
        ArrayData['rango0'],
        ArrayData['rango1'],
        ArrayData['rango2'],
        ArrayData['rango3'],
        ArrayData['rango4'],
        ArrayData['rango5'],
        ArrayData['rango6'],
        ArrayData['rango7'],
        ArrayData['rango8'],
        ArrayData['rango9'],
        ArrayData['rango10'],
        ArrayData['rango11'],
        ArrayData['rango12'],
        ArrayData['rango13'],
        ArrayData['rango14'],
        ArrayData['rango15'],
        ArrayData['rango16'],
        ArrayData['rango17'],
        ArrayData['rango18'],
        ArrayData['rango19'],
        ArrayData['rango20'],
        ArrayData['rango21'],
        ArrayData['rango22'],
        ArrayData['rango23'],
        ArrayData['rango_Total'],
    ]);
/*
    var citaClase = ArrayData['CitaTipo'].replace(/\s/g,"-");
    citaClase = citaClase.replace(/\//g,"-");
    var filaTR = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    $('td', filaTR)[3].setAttribute('class','col-4-'+citaClase);
    $('td', filaTR)[4].setAttribute('class','col-5-'+citaClase);
    $('td', filaTR)[5].setAttribute('class','hidden col-6-'+citaClase);
    $('td', filaTR)[6].setAttribute('class','hidden col-7-'+citaClase);*/
}

function LimpiarTablaExcelDasboard1(TableName, Columns, Filename) {
    //Limpiamos la tabla y ocultamos los items.
    $(TableName).dataTable().fnClearTable();
    $(TableName).dataTable().fnDestroy();
    var table = $(TableName).DataTable({
        /*fixedHeader: {
            header: false,
            footer: true
        },*/
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
    /*new $.fn.dataTable.FixedHeader( TableName, {
        top: false,
        bottom: true
    } );*/

}