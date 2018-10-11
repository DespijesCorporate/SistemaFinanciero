
var dataCitas = [];

$(document).ready(function () {
    jQuery.ajaxSetup({ async: false });
    checkUserAccess('CONF050');
    jQuery.ajaxSetup({ async: true });

    GetRazones($('#hidden_tipo_id').val());
    GetTipoRazones();

    $('.monto').mask('#############', { translation: { '#': { pattern: /[0-9.]/, optional: true } } });
    $('.varchar100').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû#]/, optional: true } } });


    //LimpiarTabla('#tableRazones');
    $('#btnGuardar').click(function () {
        //if ($('#hidden_id').val() == '' || $('#hidden_id').val() == null) {
        //    existeRazon($('#descripcion_tipo_razon').val(), $('#abreviatura').val());
        //}
        //else {
            GuardarRazon();
        //}
    });
});

$("#btnGeneraReciboPagoPrestamo").on('click', function (e) {
    var recibo = $("#NumRecibo").val()
    GenerarSolicitud(recibo);
});

function GenerarSolicitud(recibo) {

    var path = urljs + "/Prestamos/ReciboPagoPrestamo";
    var posting = $.post(path, { recibo: recibo });

    //var path = urljs + "/Prestamos/ReciboOtros";
    //var posting = $.post(path, { NumRecibo = $("#NumTrans").val() });
}

function ValidaCajero() {
    try {
        var path = urljs + "/Caja/ValidaCajero";
        var posting = $.post(path, {});
        posting.done(function (data, status, xhr) {

            dataResultado = [];
            dataResultado = data;
            alert(data.Mensaje);

        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            $('.nav-tabs a[href="#ver_cita"]').closest('li').addClass('hide');
        });
    }
    catch (e) {
        //RemoveAnimation("body");

    }
    //location.reload();
}

$('#btnEliminar').click(function () {
    var id = $('#hidden_id').val();
    var path = urljs + 'Razones/delete';
    var posting = $.post(path, { id: Number(id) });
    posting.done(function (data, status, xhr) {
        if (data.Accion > 0) {
            GetRazones($('#hidden_tipo_id').val());
            $('#modalEliminarRazon').modal('hide');
            GenerarSuccessAlerta(data.Mensaje, "success");
            goAlert();
            LimpiarInput(inputs, ['']);
        }
        else {
            GenerarErrorAlerta(data.Mensaje, "successModalEliminarPlantillas");
            goAlert();
        }
    });
    posting.fail(function (data, status, xhr) {
        //sendError(data);
        GenerarErrorAlerta(xhr, "error");
        goAlert();
    });
    posting.always(function () {
        /*console.log("Se cargo los proyectos correctamente");*/
        //RemoveAnimation("body");
    });
});

var inputs = ['#descripcion_tipo_razon', '#abreviatura', '#cod_tipo_razon'];
var client = 0;
function GetRazones(ClienteId, NomPrestamo) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/Prestamos/GetAll";
        var posting = $.post(path, { ClienteId: Number(ClienteId) });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableRazones");
            if (data.length) {
                if (data[0].Accion > 0) {
                    $('.titulo').empty().append('Listado de préstamos del cliente: ' + data[0].NomPrestamo);
                    client = data[0].ClienteId;
                    for (var i = data.length - 1; i >= 0; i--) {
                        addRow(data[i], "#tableRazones");
                    }
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

function GetRazonBy(id, tipo_id) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/Razones/GetOne";
        var posting = $.post(path, { id: Number(id), tipo_id: Number(tipo_id) });
        posting.done(function (data, status, xhr) {
            console.log(data);
            if (data.Accion > 0) {
                $('#descripcion_tipo_razon').val(data.RazonDescripcion);
                $('#abreviatura').val(data.RazonAbreviatura);
                $('#cod_tipo_razon').val(data.TipoId).trigger('change');
                $('#cod_tipo_razon').attr('disabled', 'disabled');
                $('#razongroup').val(data.RazonGroup).trigger('change');
                $('#razonstatus').val(data.RazonStatus);
            }
            else {
                GenerarErrorAlerta(data.Mensaje, "error");
                goAlert();
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

function addRow(ArrayData, tableID) {

    var newRow = $(tableID).dataTable().fnAddData([
        ArrayData['FecSolicitud'],
        //ArrayData['MontoSolicitado'],
        ArrayData['MontoAprobado'],
        //ArrayData['PlazoMeses'],
        //ArrayData['Interes'],
        ArrayData['Saldo'],
        ArrayData['Estado'],
        ArrayData['Garantia'],
        "<button data-id='" + ArrayData['PrestamoId'] + "' title='Generar Pago' data-toggle='tooltip' onClick='GetDatosPagoPrestamo(" + ArrayData['PrestamoId'] + ")' id='btnGenerarPago" + ArrayData['PrestamoId'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>",
        //"<button data-id='" + ArrayData['RazonId'] + "' data-tipo_id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Generar Pago' data-toggle='tooltip' onClick='EditarRazon(event)' id='btnEditarRazon" + ArrayData['RazonId'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>", 
        //+ "<button data-id='" + ArrayData['RazonId'] + "' data-tipo_id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Eliminar Préstamo' data-toggle='tooltip' onClick='EliminarRazon(event)' id='btnEliminarRazon" + ArrayData['RazonId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        ArrayData['ClienteId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['ClienteId']);
    $('td', theNode)[6].setAttribute('class', 'RazonId hidden');
}

function EditarRazon(e) {
    e.stopPropagation();

    var id = $(e.currentTarget).attr('data-id');
    var tipo_id = $(e.currentTarget).attr('data-tipo_id');
    var desc = $(e.currentTarget).attr('data-name');

    $("#theHeader").html("Editar | " + desc);
    $('#moda_razones').modal('show');
    $('#hidden_id').val(id);
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#razongroup", ' +
        '"url": "configuracion/getParametosByIdEncabezado/", ' +
        '"val": "ConfigItemID", ' +
        '"type": "GET", ' +
        '"data": "GRPR", ' +
        '"text": "ConfigItemDescripcion"}');
    console.log(infojson);
    $('#cod_tipo_razon').val($('#hidden_tipo_id').val()).trigger('change');
    LoadComboBox(infojson);
    GetRazonBy(id, tipo_id);
    jQuery.ajaxSetup({ async: true });
}

function EliminarRazon(e) {
    e.stopPropagation();
    //console.log('Cancel ticket');
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $('#hidden_id').val(id);

    $("#theHeaderEliminar").html("Eliminar Razon | " + desc);
    $('#modalEliminarRazon').modal('show');
    $('#modalmessage').html('¿Desea eliminar la razon: <b>' + desc + '</b>?');
}

function GetTipoRazones() {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/TipoRazones/GetAll";
        var posting = $.post(path, { param1: 'value1' });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableTipoRazones");
            if (data.length) {
                if (data[0].Accion > 0) {
                    $('#cod_tipo_razon').empty();
                    var contador = 0;
                    for (var i = data.length - 1; i >= 0; i--) {
                        $('#cod_tipo_razon').append(new Option(data[contador].TipoAbreviatura, data[contador].TipoId));
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

function NuevaRazon(e) {
    e.stopPropagation();
    var id = -1;
    $("#theHeader").html("Nuevo Préstamo");
    $('#hidden_id').val("");
    $("#moda_razones").find('input[type = "text"]').val("");
    $('#descripcion_tipo_razon').val('');
    var infojson = jQuery.parseJSON('{"input": "#razongroup", ' +
        '"url": "configuracion/getParametosByIdEncabezado/", ' +
        '"val": "ConfigItemID", ' +
        '"type": "GET", ' +
        '"data": "GRPR", ' +
        '"text": "ConfigItemDescripcion"}');
    console.log(infojson);
    $('#cod_tipo_razon').val($('#hidden_tipo_id').val()).trigger('change');
    LoadComboBox(infojson);
    $('#razonstatus').val('ACT');
    $('#moda_razones').modal('show');
}
function GuardarRazon() {
    try {
        var inputs = [];
        var mensaje = [];


        /*Recorremos el contenedor para obtener los valores*/
        $('.requerido').each(function () {
            /*Llenamos los arreglos con la info para la validacion*/
            inputs.push('#' + $(this).attr('id'));
            mensaje.push($(this).attr('attr-message'));

            /*Creamos el json*/
            /*var json = {
                id : $(this).attr('id'),
                val : $(this).val()
            };*/

            /*data.push(json);*/
        });
        /*Si la validación es correcta ejecuta el ajax*/
        if (Validar(inputs, mensaje)) {

            var path = urljs + 'Prestamos/SaveData';
            var id = $('#hidden_id').val();
            console.log('id: ' + $('#hidden_id').val());

            if (id == "") {
                id = -1;
            }
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {
                //ClienteId: $('#cod_tipo_razon option:selected').val(),
                ClienteId: client,
                //RazonId: id,
                //PrestamoId = client,
                
                MontoSolicitado: $('#montosolicitado').val(),
                PlazoMeses: $('#plazo').val(),
                Garantia: $('#garantia option:selected').val(),
                Interes: $('#tasainteres').val(),
                Destino: $('#destino option:selected').val(),
                FrecPago: $('#frecuenciapago option:selected').val(),
                Observaciones: $('#observaciones').val()
                //Interes: $('#razonstatus').val()
            }

            //console.log('...Submitting form');
            //console.log(data);
            //console.log('Submitting form...');

            //$.ajax({
            //    type: 'POST',
            //    url: path,
            //    dataType: 'json',
            //    contentType: dataType,
            //    data: JSON.stringify(data),
            //    success: function (result) {
            //        console.log('Data received: ');
            //        console.log(result);
            //    }
            //});

            //var info = JSON.stringify(data);

            var posting = $.post(path, data);

            posting.done(function (data, status, xhr) {
                console.log(data);
                $('#moda_razones').modal('hide');
                GenerarSuccessAlerta(data.Mensaje, "success");
                goAlert();
                LimpiarInput(inputs, ['']);
                GetRazones($('#hidden_tipo_id').val());
            });

            posting.fail(function (data, status, xhr) {
                console.log(status);
            });


        }
    } catch (e) {
        console.log(e);
    }
}

function existeRazon(descripcion, abreviatura) {
    var resultado = false;
    try {
        var path = urljs + "/Razones/CheckOne";
        var posting = $.post(path, { descripcion: descripcion, abreviatura: abreviatura });
        posting.done(function (data, status, xhr) {
            console.log('Registros: ' + data.cantidadRegistros);
            if (data.cantidadRegistros > 0) {
                $('#moda_razones').modal('hide');
                GenerarErrorAlerta('Registro ya existe en la base de datos.', "error");
                goAlert();
                resultado = true;
                console.log('Res1: ' + resultado);
            }
            else {
                GuardarRazon();
            }
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
        });
    }
    catch (e) {
        console.log(e);
    }
}

//function GenerarPago(id) {

//    //e.stopPropagation();
//    $('#theHeader').html("Pago a Prestamo");
//    dataCitas = [];
//    dataCitas = data;
//    //$('#hidden_id').val("");
//    if (dataCitas.length > 0) {
//        for (var i = dataCitas.length - 1; i >= 0; i--) {
//            //if (dataCitas[i].PRES_Codigo == id) {
//                $('#Capital').val(dataCitas[i].Capital);
//                $('#Intereses').val(dataCitas[i].Intereses);
//                $('#Mora').val(dataCitas[i].Mora);
//                $('#Total').val(dataCitas[i].Total);
//                $('#Saldo').val(dataCitas[i].Saldo);
//                //$('#txt_segmento').val(dataCitas[i].PRES_Estado);
//                //$('#txt_monto_sol').val(dataCitas[i].PRES_mto_Solicitado);
//                //$('#txt_fecha').val(dataCitas[i].PRES_Fecha_Solicitud);
//           // }
//        }
//        //activarTab('ver_cita');
//    }
//    $('#moda_razones').modal('show');
//}

function GetDatosPagoPrestamo(id) {
    $("#btnGeneraReciboPagoPrestamo").addClass("disabled");
    $('#theHeader').html("Pago a Prestamo");
    try {
        var path = urljs + "/Caja/GetDatosPagoPrestamo";
        var posting = $.post(path, { id: id });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                for (var i = dataCitas.length - 1; i >= 0; i--) {
                $('#Capital').val(dataCitas[i].Capital);
                $('#Intereses').val(dataCitas[i].Intereses);
                $('#Mora').val(dataCitas[i].Mora);
                //$('#Total').val(dataCitas[i].Total);
                $('#Saldo').val(dataCitas[i].Saldo);
                $('#Prestamo').val(dataCitas[i].Prestamo);
                    //$('#txt_Hora').val(data.CitaHora);
                }
            }
            $('#moda_razones').modal('show');

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
        //RemoveAnimation("body");

    }
}

$("#btnGeneraPagoPrestamo").on('click', function (e) {
        var path = urljs + "/Caja/ValidaCajero";
        var posting = $.post(path, {});
        posting.done(function (data, status, xhr) {

            dataResultado = [];
            dataResultado = data;
            //alert(data.Mensaje);
            if ((data.Estado) != 1 && (data.Estado) != 2) {
                GenerarPagoPrestamo($("#Prestamo").val(), $("#Capital").val(), $("#Intereses").val(), $("#Mora").val(), $("#TipoPago").val(), $("#Observacion").val());
                $("#btnGeneraPagoPrestamo").addClass("disabled");
                $("#btnGeneraReciboPagoPrestamo").removeClass("disabled");
            }
            else {
                alert(data.Mensaje);
            }
        });
});

function GenerarPagoPrestamo(codigo, Capital, Intereses, Mora, TipoPago, Observacion) {
    try {
        var path = urljs + "/Caja/PagoPrestamo";
        var posting = $.post(path, { codigo: codigo, Capital: Capital, Intereses: Intereses, Mora: Mora, TipoPago: TipoPago, Observacion: Observacion });
        posting.done(function (data, status, xhr) {

            dataResultado = [];
            dataResultado = data;
            $("#NumRecibo").val(data.Num);
            alert(data.Mensaje);
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            $('.nav-tabs a[href="#ver_cita"]').closest('li').addClass('hide');
        });
    }
    catch (e) {
        //RemoveAnimation("body");

    }
    //location.reload();
}
