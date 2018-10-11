$(document).ready(function ()
{
    jQuery.ajaxSetup({ async: false });
    checkUserAccess('CONF050');
    jQuery.ajaxSetup({ async: true });

    GetRazones($('#hidden_tipo_id').val());
    GetTipoRazones();
    $('.numero').mask('####');
    $('.varchar10').mask('AAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    //LimpiarTabla('#tableRazones');
    $('#btnGuardar').click(function () {
        if ($('#hidden_id').val() == '' || $('#hidden_id').val() == null){
            existeRazon($('#descripcion_tipo_razon').val(), $('#abreviatura').val());
        }
        else{
            GuardarRazon();
        }
    });
});

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

function GetRazones(TipoId) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/Razones/GetAll";
        var posting = $.post(path, { TipoId: Number(TipoId)});
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableRazones");
            if (data.length) {
                if (data[0].Accion > 0) {
                    $('.titulo').empty().append('Listado de razones para el tipo: ' + data[0].TipoAbreviatura);
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

function GetRazonBy(id,tipo_id) {
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
                $('#cod_tipo_razon').attr('disabled','disabled');
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
        ArrayData['TipoAbreviatura'],
        ArrayData['RazonAbreviatura'],
        ArrayData['RazonDescripcion'],
        ArrayData['ConfigItemDescripcion'],
        ArrayData['RazonStatus'],
        "<button data-id='" + ArrayData['RazonId'] + "' data-tipo_id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Editar Razon' data-toggle='tooltip' onClick='EditarRazon(event)' id='btnEditarRazon" + ArrayData['RazonId'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
        "<button data-id='" + ArrayData['RazonId'] + "' data-tipo_id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Eliminar Razon' data-toggle='tooltip' onClick='EliminarRazon(event)' id='btnEliminarRazon" + ArrayData['RazonId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        ArrayData['RazonId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['RazonId']);
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
    $("#theHeader").html("Nueva Razón");
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

            var path = urljs + 'Razones/SaveData';
            var id = $('#hidden_id').val();
            console.log('id: ' + $('#hidden_id').val());

            if (id == "") {
                id = -1;
            }
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {
                TipoId: $('#cod_tipo_razon option:selected').val(),
                RazonId: id,
                RazonDescripcion: $('#descripcion_tipo_razon').val(),
                RazonAbreviatura: $('#abreviatura').val(),
                RazonGroup: $('#razongroup').val(),
                RazonStatus: $('#razonstatus').val()
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