var hoy = moment(new Date()).format('YYYY-MM-DD');
$(document).ready(function () {
    checkUserAccess('CYRS020');
    jQuery.ajaxSetup({ global: false });
    ListaEstados();
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

//LISTA DE ESTADOS
function ListaEstados(){
    $("#ListaEstados").empty().append( new Option('No se ha cargado información','-2') );
    try {
        var path = urljs + "/sucursales/ListaEstados";
        var posting = $.post(path);
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    $("#ListaEstados").empty().append( new Option('Todos los Estados','3') );
                    for (var i = data.length - 1; i >= 0; i--) {
                        $("#ListaEstados").append(new Option(data[contador].Estado_Descripcion, data[contador].EstadoCodigo));
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

//REPORTE CREDITOS
var inputs = []; var mensaje = [];
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
            var path = urljs + 'Reportes/ReporteCreditos';
            var fecha1 = $("#txt_fecha1").val();
            var fecha2 = $("#txt_fecha2").val();
            var cod_sucursal = $("#ListaEstados").val() == "" ? -1 : $("#ListaEstados").val();

            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {
                fecha1: fecha1,
                fecha2: fecha2,
                SucursalId: cod_sucursal
            }
            var posting = $.post(path, data);
            posting.done(function (data, status, xhr) {
                var excel_cols = [0, 1, 2, 3, 4, 5, 6, 7,8, 9, 10, 11, 12, 13, 14, 15, 16];
                LimpiarTablaExcel("#tableCitasDashboard1", excel_cols, 'ReporteCreditos');
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
    var newRow = $(tableID).dataTable().fnAddData([
        counter,
        ArrayData['PRES_Codigo'],
        ArrayData['PRES_Codigo_CLI'],
        ArrayData['CLI_Nombre'],
        ArrayData['PRES_Fecha_Solicitud'],
        ArrayData['PRES_mto_Solicitado'],
        ArrayData['PRES_mto_Aprobado'],
        ArrayData['PRES_Plazo_Meses'],
        ArrayData['PRES_Porc_Interes'],
        ArrayData['PRES_Saldo'],
        ArrayData['Estado'],
        ArrayData['FrecuenciaPago'],
        ArrayData['PRES_Num_Pagos'],
        ArrayData['Destino'],
        ArrayData['Garantia'],
        ArrayData['PRES_Agrego'],
        ArrayData['PRES_Fecha_Agrego']
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
