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
                var excel_cols = [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27];
                var suma_rango0 = 0,suma_rango1 = 0,suma_rango2 = 0,suma_rango3 = 0,suma_rango4 = 0,suma_rango5 = 0,
                    suma_rango6 = 0,suma_rango7 = 0,suma_rango8 = 0,suma_rango9 = 0,suma_rango10 = 0,suma_rango11 = 0,
                    suma_rango12 = 0,suma_rango13 = 0,suma_rango14 = 0,suma_rango15 = 0,suma_rango16 = 0,suma_rango17 = 0,
                    suma_rango18 = 0,suma_rango19 = 0,suma_rango20 = 0,suma_rango21 = 0,suma_rango22 = 0,suma_rango23 = 0;
                var acumula_rango0 = 0,acumula_rango1 = 0,acumula_rango2 = 0,acumula_rango3 = 0,acumula_rango4 = 0,acumula_rango5 = 0,
                    acumula_rango6 = 0,acumula_rango7 = 0,acumula_rango8 = 0,acumula_rango9 = 0,acumula_rango10 = 0,acumula_rango11 = 0,
                    acumula_rango12 = 0,acumula_rango13 = 0,acumula_rango14 = 0,acumula_rango15 = 0,acumula_rango16 = 0,acumula_rango17 = 0,
                    acumula_rango18 = 0,acumula_rango19 = 0,acumula_rango20 = 0,acumula_rango21 = 0,acumula_rango22 = 0,acumula_rango23 = 0;
                LimpiarTablaExcelDasboard1("#tableCitasDashboard1",excel_cols, 'Reporte Flujo por intervalo');
                if (data.length > 0) {
                    if (data[0].Accion > 0) {
                        var counter = 1;
                        jQuery.ajaxSetup({ async: false });
                        for (var i = data.length - 1; i >= 0; i--) {
                            /* asignar subtotal columna */
                            suma_rango0 += data[i]['rango0']; suma_rango1 += data[i]['rango1']; suma_rango2 += data[i]['rango2'];
                            suma_rango3 += data[i]['rango3']; suma_rango4 += data[i]['rango4']; suma_rango5 += data[i]['rango5'];
                            suma_rango6 += data[i]['rango6']; suma_rango7 += data[i]['rango7']; suma_rango8 += data[i]['rango8'];
                            suma_rango9 += data[i]['rango9']; suma_rango10 += data[i]['rango10']; suma_rango11 += data[i]['rango11'];
                            suma_rango12 += data[i]['rango12']; suma_rango13 += data[i]['rango13']; suma_rango14 += data[i]['rango14'];
                            suma_rango15 += data[i]['rango15']; suma_rango16 += data[i]['rango16']; suma_rango17 += data[i]['rango17'];
                            suma_rango18 += data[i]['rango18']; suma_rango19 += data[i]['rango19']; suma_rango20 += data[i]['rango20'];
                            suma_rango21 += data[i]['rango21']; suma_rango22 += data[i]['rango22']; suma_rango23 += data[i]['rango23'];
                        
                            addRowDashboard1(data[i], "#tableCitasDashboard1", counter);
                            counter++;
                        }
                        /* Asignar acumulado columna */
                        acumula_rango0 = suma_rango0; acumula_rango1 = acumula_rango0+suma_rango1; acumula_rango2 = acumula_rango1+suma_rango2; 
                        acumula_rango3 = acumula_rango2+suma_rango3; acumula_rango4 = acumula_rango3+suma_rango4; acumula_rango5 = acumula_rango4+suma_rango5; 
                        acumula_rango6 = acumula_rango5+suma_rango6; acumula_rango7 = acumula_rango6+suma_rango7; acumula_rango8 = acumula_rango7+suma_rango8; 
                        acumula_rango9 = acumula_rango8+suma_rango9; acumula_rango10 = acumula_rango9+suma_rango10; acumula_rango11 = acumula_rango10+suma_rango11; 
                        acumula_rango12 = acumula_rango11+suma_rango12; acumula_rango13 = acumula_rango12+suma_rango13; acumula_rango14 = acumula_rango13+suma_rango14; 
                        acumula_rango15 = acumula_rango14+suma_rango15; acumula_rango16 = acumula_rango15+suma_rango16; acumula_rango17 = acumula_rango16+suma_rango17; 
                        acumula_rango18 = acumula_rango17+suma_rango18; acumula_rango19 = acumula_rango18+suma_rango19; acumula_rango20 = acumula_rango19+suma_rango20; 
                        acumula_rango21 = acumula_rango20+suma_rango21; acumula_rango22 = acumula_rango21+suma_rango22; acumula_rango23 = acumula_rango22+suma_rango23;
                        /* Porcentaje */
                        var porcentaje0 = (suma_rango0/data[0]['total_citas'])*100;
                        porcentaje0 = porcentaje0.toFixed(2);
                        var porcentaje1 = (suma_rango1/data[0]['total_citas'])*100;
                        porcentaje1 = porcentaje1.toFixed(2);
                        var porcentaje2 = (suma_rango2/data[0]['total_citas'])*100;
                        porcentaje2 = porcentaje2.toFixed(2);
                        var porcentaje3 = (suma_rango3/data[0]['total_citas'])*100;
                        porcentaje3 = porcentaje3.toFixed(2);
                        var porcentaje4 = (suma_rango4/data[0]['total_citas'])*100;
                        porcentaje4 = porcentaje4.toFixed(2);
                        var porcentaje5 = (suma_rango5/data[0]['total_citas'])*100;
                        porcentaje5 = porcentaje5.toFixed(2);
                        var porcentaje6 = (suma_rango6/data[0]['total_citas'])*100;
                        porcentaje6 = porcentaje6.toFixed(2);
                        var porcentaje7 = (suma_rango7/data[0]['total_citas'])*100;
                        porcentaje7 = porcentaje7.toFixed(2);
                        var porcentaje8 = (suma_rango8/data[0]['total_citas'])*100;
                        porcentaje8 = porcentaje8.toFixed(2);
                        var porcentaje9 = (suma_rango9/data[0]['total_citas'])*100;
                        porcentaje9 = porcentaje9.toFixed(2);
                        var porcentaje10 = (suma_rango10/data[0]['total_citas'])*100;
                        porcentaje10 = porcentaje10.toFixed(2);
                        var porcentaje11 = (suma_rango11/data[0]['total_citas'])*100;
                        porcentaje11 = porcentaje11.toFixed(2);
                        var porcentaje12 = (suma_rango12/data[0]['total_citas'])*100;
                        porcentaje12 = porcentaje12.toFixed(2);
                        var porcentaje13 = (suma_rango13/data[0]['total_citas'])*100;
                        porcentaje13 = porcentaje13.toFixed(2);
                        var porcentaje14 = (suma_rango14/data[0]['total_citas'])*100;
                        porcentaje14 = porcentaje14.toFixed(2);
                        var porcentaje15 = (suma_rango15/data[0]['total_citas'])*100;
                        porcentaje15 = porcentaje15.toFixed(2);
                        var porcentaje16 = (suma_rango16/data[0]['total_citas'])*100;
                        porcentaje16 = porcentaje16.toFixed(2);
                        var porcentaje17 = (suma_rango17/data[0]['total_citas'])*100;
                        porcentaje17 = porcentaje17.toFixed(2);
                        var porcentaje18 = (suma_rango18/data[0]['total_citas'])*100;
                        porcentaje18 = porcentaje18.toFixed(2);
                        var porcentaje19 = (suma_rango19/data[0]['total_citas'])*100;
                        porcentaje19 = porcentaje19.toFixed(2);
                        var porcentaje20 = (suma_rango20/data[0]['total_citas'])*100;
                        porcentaje20 = porcentaje20.toFixed(2);
                        var porcentaje21 = (suma_rango21/data[0]['total_citas'])*100;
                        porcentaje21 = porcentaje21.toFixed(2);
                        var porcentaje22 = (suma_rango22/data[0]['total_citas'])*100;
                        porcentaje22 = porcentaje22.toFixed(2);
                        var porcentaje23 = (suma_rango23/data[0]['total_citas'])*100;
                        porcentaje23 = porcentaje23.toFixed(2);
                        /* Acumulado */
                        var cumulative0 = (acumula_rango0/data[0]['total_citas'])*100;
                        cumulative0 = cumulative0.toFixed(2);
                        var cumulative1 = (acumula_rango1/data[0]['total_citas'])*100;
                        cumulative1 = cumulative1.toFixed(2);
                        var cumulative2 = (acumula_rango2/data[0]['total_citas'])*100;
                        cumulative2 = cumulative2.toFixed(2);
                        var cumulative3 = (acumula_rango3/data[0]['total_citas'])*100;
                        cumulative3 = cumulative3.toFixed(2);
                        var cumulative4 = (acumula_rango4/data[0]['total_citas'])*100;
                        cumulative4 = cumulative4.toFixed(2);
                        var cumulative5 = (acumula_rango5/data[0]['total_citas'])*100;
                        cumulative5 = cumulative5.toFixed(2);
                        var cumulative6 = (acumula_rango6/data[0]['total_citas'])*100;
                        cumulative6 = cumulative6.toFixed(2);
                        var cumulative7 = (acumula_rango7/data[0]['total_citas'])*100;
                        cumulative7 = cumulative7.toFixed(2);
                        var cumulative8 = (acumula_rango8/data[0]['total_citas'])*100;
                        cumulative8 = cumulative8.toFixed(2);
                        var cumulative9 = (acumula_rango9/data[0]['total_citas'])*100;
                        cumulative9 = cumulative9.toFixed(2);
                        var cumulative10 = (acumula_rango10/data[0]['total_citas'])*100;
                        cumulative10 = cumulative10.toFixed(2);
                        var cumulative11 = (acumula_rango11/data[0]['total_citas'])*100;
                        cumulative11 = cumulative11.toFixed(2);
                        var cumulative12 = (acumula_rango12/data[0]['total_citas'])*100;
                        cumulative12 = cumulative12.toFixed(2);
                        var cumulative13 = (acumula_rango13/data[0]['total_citas'])*100;
                        cumulative13 = cumulative13.toFixed(2);
                        var cumulative14 = (acumula_rango14/data[0]['total_citas'])*100;
                        cumulative14 = cumulative14.toFixed(2);
                        var cumulative15 = (acumula_rango15/data[0]['total_citas'])*100;
                        cumulative15 = cumulative15.toFixed(2);
                        var cumulative16 = (acumula_rango16/data[0]['total_citas'])*100;
                        cumulative16 = cumulative16.toFixed(2);
                        var cumulative17 = (acumula_rango17/data[0]['total_citas'])*100;
                        cumulative17 = cumulative17.toFixed(2);
                        var cumulative18 = (acumula_rango18/data[0]['total_citas'])*100;
                        cumulative18 = cumulative18.toFixed(2);
                        var cumulative19 = (acumula_rango19/data[0]['total_citas'])*100;
                        cumulative19 = cumulative19.toFixed(2);
                        var cumulative20 = (acumula_rango20/data[0]['total_citas'])*100;
                        cumulative20 = cumulative20.toFixed(2);
                        var cumulative21 = (acumula_rango21/data[0]['total_citas'])*100;
                        cumulative21 = cumulative21.toFixed(2);
                        var cumulative22 = (acumula_rango22/data[0]['total_citas'])*100;
                        cumulative22 = cumulative22.toFixed(2);
                        var cumulative23 = (acumula_rango23/data[0]['total_citas'])*100;
                        cumulative23 = cumulative23.toFixed(2);

                        addRowDashboardTotal("Totales",suma_rango0,suma_rango1,suma_rango2,suma_rango3,suma_rango4,suma_rango5,suma_rango6,suma_rango7,suma_rango8,suma_rango9,suma_rango10,suma_rango11,suma_rango12,suma_rango13,suma_rango14,suma_rango15,suma_rango16,suma_rango17,suma_rango18,suma_rango19,suma_rango20,suma_rango21,suma_rango22,suma_rango23,data[0]['total_citas'],"#tableCitasDashboard1");
                        addRowDashboardTotal("Porcentaje - %",porcentaje0,porcentaje1,porcentaje2,porcentaje3,porcentaje4,porcentaje5,porcentaje6,porcentaje7,porcentaje8,porcentaje9,porcentaje10,porcentaje11,porcentaje12,porcentaje13,porcentaje14,porcentaje15,porcentaje16,porcentaje17,porcentaje18,porcentaje19,porcentaje20,porcentaje21,porcentaje22,porcentaje23,'100.00',"#tableCitasDashboard1");
                        addRowDashboardTotal("Acumulado - %",cumulative0,cumulative1,cumulative2,cumulative3,cumulative4,cumulative5,cumulative6,cumulative7,cumulative8,cumulative9,cumulative10,cumulative11,cumulative12,cumulative13,cumulative14,cumulative15,cumulative16,cumulative17,cumulative18,cumulative19,cumulative20,cumulative21,cumulative22,cumulative23,'',"#tableCitasDashboard1");
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

    var filaTR = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    $('td', filaTR)[27].setAttribute('class','celda-subtotal');
}

function addRowDashboardTotal(col0,col1,col2,col3,col4,col5,col6,col7,col8,col9,col10,col11,col12,col13,col14,col15,col16,col17,col18,col19,col20,col21,col22,col23,col24,col25,tableID) {
    var newRow = $(tableID).dataTable().fnAddData([
        '',
        '',
        col0,
        col1,
        col2,
        col3,
        col4,
        col5,
        col6,
        col7,
        col8,
        col9,
        col10,
        col11,
        col12,
        col13,
        col14,
        col15,
        col16,
        col17,
        col18,
        col19,
        col20,
        col21,
        col22,
        col23,
        col24,
        col25
    ]);

    var filaTR = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    $(filaTR)[0].setAttribute('class','fila-resumen');
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
        "order": [[ 1, 'desc' ]]
    });
    /*new $.fn.dataTable.FixedHeader( TableName, {
        top: false,
        bottom: true
    } );*/

}