var hoy = moment(new Date()).format('YYYY-MM-DD');
$(document).ready(function () {
    checkUserAccess('CYRS040');
    jQuery.ajaxSetup({ global: false });
    reportes_constructor_sucursales();
    reportes_constructor_cubiculo_por_sucursal(-1);
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
    console.log($(this).val());
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
            var path = urljs + 'Reportes/DashboardResolucionPorCita';
            var cod_sucursal = $("#cod_sucursal").val() == ""?-1:$("#cod_sucursal").val();
            var cod_cubiculo = $("#cod_cubiculo").val() == ""?-1:$("#cod_cubiculo").val();
            var fecha1       = $("#txt_fecha1").val();
            var fecha2       = $("#txt_fecha2").val();
            
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {
                sucursalId: cod_sucursal,
                cubiculoId: cod_cubiculo,
                fecha1: fecha1,
                fecha2: fecha2
            }
            var posting = $.post(path, data);
            posting.done(function (data, status, xhr) {
                LimpiarTablaExcelDasboard1("#tableCitasDashboard1",[0,1,2,3,4,5,6,8], 'Reporte Resolución por cita');
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
        ArrayData['posicion'],
        ArrayData['CitaTicket'],
        ArrayData['CitaFecha'],
        ArrayData['TiempoAtencion'],
        ArrayData['TiempoAtencion'],
        ArrayData['Resolucion'],
    ]);

    var citaClase = ArrayData['CitaTipo'].replace(/\s/g,"-");
    citaClase = citaClase.replace(/\//g,"-");
    var filaTR = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    $('td', filaTR)[6].setAttribute('class','hidden');
    $('td', filaTR)[5].setAttribute('class','col-7-'+citaClase);
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
        ],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api();
 
            $(api.column(7).footer()).html(
                api.column(7).data().reduce(function(a,b){
                    return parseInt(a) + parseInt(b);
                }, 0)+' minuto(s)'
            );
        },
        "columnDefs": [ { "visible": false, "targets": 1 } ],
        "order": [[ 1, 'asc' ],[ 2, 'asc' ],[ 3, 'asc' ],[ 4, 'asc' ]],
        "drawCallback": function ( settings ) {
            var api  = this.api();
            var rows = api.rows().nodes();
            var last = null;
            var fila = 0;
            var totalCitasAtendidas = 0;
            var totalCitasAbandonadas = 0;
            var totalTiempoEspera = 0;
            var totalTiempoAtencion = 0;
            var citasAtendidas;
            var citasAbandonadas;
            var tiemposEspera;
            var tiemposAtencion;

            api.column(1, {page:'current'}).data().each( function ( group, i ) {
                if ( last !== group ) {
                    var groupClass   = group.replace(/\s/g,"-");
                    groupClass       = groupClass.replace(/\//g,"-");

                    tiemposAtencion  = $(TableName).DataTable().cells('.col-7-'+groupClass).data();
                    
                    totalTiempoAtencion   = tiemposAtencion.length ? tiemposAtencion.reduce( function (a, b) { return a + b; }) : 0;

                    $(rows).eq( i ).before('<tr class="group">'+
                                                '<td colspan="5">'+group+'</td>'+
                                                '<td>'+totalTiempoAtencion+' minuto(s)</td>'+
                                                '<td>Resolución</td>'+
                                            '</tr>');
                    last = group;
                }
            });
        }
    });
    $(TableName+' tbody').on( 'click', 'tr.group', function () {
        var currentOrder = table.order()[0];
        if ( currentOrder[0] === 1 && currentOrder[1] === 'asc' ) {
            table.order( [ 1, 'desc' ] ).draw();
        }
        else {
            table.order( [ 1, 'asc' ] ).draw();
        }
    } );
    /*new $.fn.dataTable.FixedHeader( TableName, {
        top: false,
        bottom: true
    } );*/

}