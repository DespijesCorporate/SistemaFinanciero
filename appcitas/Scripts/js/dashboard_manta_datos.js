var hoy = moment(new Date()).format('YYYY-MM-DD');
var hoy_max_date = moment(new Date(hoy)).add(14, 'days');
var inputs = []; var mensaje = [];

$(document).ready(function () {
    checkUserAccess('CYRS130');
    reportes_constructor_sucursales();
    reportes_constructor_tipos_cita();
    $(function () {
        $('#fecha1').datetimepicker({
            locale: 'es',
            //minDate: hoy,
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
                var path = urljs + 'Reportes/Dashboard_MantaDatos';
                var cod_sucursal = $("#cod_sucursal").val() == "" ? -1 : $("#cod_sucursal").val();
                var fecha1 = $("#txt_fecha1").val();
                var fecha2 = $("#txt_fecha2").val();
                var cod_citatipo = $("#cod_tipo_cita").val() == "" ? -1 : $("#cod_tipo_cita").val();

                //JSON data
                var dataType = 'application/json; charset=utf-8';
                var data = {
                    SucursalId: Number(cod_sucursal),
                    CitaTipo: Number(cod_citatipo),
                    fechainicio: fecha1,
                    fechafinal: fecha2
                }
                var posting = $.post(path, data);
                posting.done(function (data, status, xhr) {
                    var excel_cols = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 , 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24];
                    LimpiarTablaExcelDasboard1("#tableCitasDashboard1", excel_cols, 'Manta de Datos');
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
});

function reportes_constructor_sucursales() {
    $("#cod_sucursal").empty().append(new Option('No se ha cargado información', '-2'));
    try {
        //var path = urljs + "/sucursales/GetAll";
        var path = urljs + "/sucursales/getSucursalesCentroAtencion";
        var posting = $.post(path);
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    $("#cod_sucursal").empty().append(new Option('Todos los centros', '-1'));
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

function addRowDashboard1(ArrayData, tableID, counter) {

    var cuentaDecrypted = ArrayData['CitaCuenta']; //CryptoJS.AES.decrypt(ArrayData['CitaCuenta'], 'BACPANAMA');
    var tarjetaDecrypted = ArrayData['CitaTarjeta']; //CryptoJS.AES.decrypt(ArrayData['CitaTarjeta'], 'BACPANAMA');

    var newRow = $(tableID).dataTable().fnAddData([
        counter,
        ArrayData['CitaTicket'],
        ArrayData['CitaEstadoDescripcion'],
        cuentaDecrypted.toString(CryptoJS.enc.Utf8),
        tarjetaDecrypted.toString(CryptoJS.enc.Utf8),
        ArrayData['EmisorCuenta'],
        ArrayData['Marca'],
        ArrayData['Producto'],
        ArrayData['Segmentos'],
        ArrayData['Familia'],
        ArrayData['CitaNombre'],
        ArrayData['CitaIdentificacion'],
        ArrayData['CitaCorreoElectronico'],
        ArrayData['CitaTelefonoCelular'],
        ArrayData['SucursalNombre'],
        ArrayData['CitaUsuarioAtendio'],
        ArrayData['DiaAtencion'],
        ArrayData['HoraAtencion'],
        ArrayData['HoraFinalizaAtencion'],
        ArrayData['HoraClienteLlego'],
        ArrayData['MotivoInsatisfaccion'],
        ArrayData['RazonCancelacion'],
        ArrayData['HerramientaRescate'],
        ArrayData['DescResolucion'],
        ArrayData['Tramite'],
        //ArrayData['Canal'],
        ArrayData['SucursalOrigen'],
        ArrayData['Banco']        
    ]);
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
        "scrollX": true,
        "scrollY": "500px",
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
        "order": [[1, 'desc']]
    });
}

function reportes_constructor_tipos_cita() {
    $("#cod_tipo_cita").empty().append(new Option('No se ha cargado información', '-1'));
    try {
        var path = urljs + "/configuracion/getParametosByIdEncabezado";
        var posting = $.post(path, { ConfigId: 'TCITA' });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    $("#cod_tipo_cita").empty().append(new Option('Todos los tipos de atención', '-1'));
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
