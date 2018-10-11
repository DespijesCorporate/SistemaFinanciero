$(document).ready(function ()
{
    jQuery.ajaxSetup({ async: false });
    checkUserAccess('CONF040');
    jQuery.ajaxSetup({ async: true });

    $('.numero').mask('####');
    $('.varchar5').mask('AAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar10').mask('AAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar100').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar200').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    GetTramites();
    //LimpiarTabla('#tableTramites');
    $('#btnGuardar').click(function () {
        if(edit){
            GuardarTramite();
        }
        else{
            existeTramite( $('#txt_descripcion').val(),$('#txt_abreviatura').val() );
        }
        
    });
    $('#btnEliminar').click(function () {
        var id = $('#hidden_id').val();
        var path = urljs + 'tramites/delete';
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            if (data.Accion > 0) {
                GetTramites();
                $('#modalEliminarTramite').modal('hide');
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
});

var inputs = ['#txt_descripcion', '#txt_abreviatura', '#txt_duracion', '#txt_alertaPrevia', '#txt_toleranciaAlCliente', '#txt_toleranciaDelCliente', '#txt_toleranciaFinalizacion', '#txt_tiempoMuerto', '#hidden_id'];
function GetTramites() {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");
        var path = urljs + "/tramites/GetAll";
        var posting = $.post(path, { param1: 'value1' });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableTramites");
            if (data.length > 0) {
                if (data[0].Accion > 0) {
                    for (var i = data.length - 1; i >= 0; i--) {
                        //console.log(data[i]);
                        addRow(data[i], "#tableTramites");
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

function GetTramiteBy(id) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");
        var path = urljs + "/tramites/GetOne";
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            console.log(data);
            if (data.Accion > 0) {
                $('#txt_descripcion').val(data.TramiteDescripcion);
                $('#txt_abreviatura').val(data.TramiteAbreviatura);
                $('#txt_duracion').val(data.TramiteDuracion);
                $('#txt_alertaPrevia').val(data.TramiteAlertaPrevia);
                $('#txt_toleranciaAlCliente').val(data.TramiteToleranciaAlCliente);
                $('#txt_toleranciaDelCliente').val(data.TramiteToleranciaDelCliente);
                $('#txt_toleranciaFinalizacion').val(data.TramiteToleranciaFinalizacion);
                $('#txt_tiempoMuerto').val(data.TramiteTiempoMuerto);
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

function existeTramite(descripcion,abreviatura) {
    var resultado = false;
    try {
        var path = urljs + "/tramites/CheckOne";
        var posting = $.post(path, { descripcion: descripcion, abreviatura: abreviatura });
        posting.done(function (data, status, xhr) {
            console.log('Registros: '+data.cantidadRegistros);
            if(data.cantidadRegistros > 0){
                $('#modalTramites').modal('hide');
                GenerarErrorAlerta('Registro ya existe en la base de datos.', "error");
                goAlert();
                resultado = true;
                console.log('Res1: '+resultado);
            }
            else{
                GuardarTramite();
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

function addRow(ArrayData, tableID) {
    var newRow = $(tableID).dataTable().fnAddData([
        ArrayData['TramiteDescripcion'],
        ArrayData['TramiteAbreviatura'],
        ArrayData['TramiteDuracion']+' minuto(s)',
        ArrayData['TramiteAlertaPrevia'] + ' minuto(s)',
        ArrayData['TramiteToleranciaAlCliente'] + ' minuto(s)',
        ArrayData['TramiteToleranciaDelCliente'] + ' minuto(s)',
        ArrayData['TramiteToleranciaFinalizacion'] + ' minuto(s)',
        ArrayData['TramiteTiempoMuerto'] + ' minuto(s)',
        "<button data-id='" + ArrayData['TramiteId'] + "' data-name='" + ArrayData['TramiteDescripcion'] + "' title='Editar Tramite' data-toggle='tooltip' onClick='EditarTramite(event)' id='btnEditarTramite" + ArrayData['TramiteId'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
        "<button data-id='" + ArrayData['TramiteId'] + "' data-name='" + ArrayData['TramiteDescripcion'] + "' title='Eliminar Tramite' data-toggle='tooltip' onClick='EliminarTramite(event)' id='btnEliminarTramite" + ArrayData['TramiteId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        ArrayData['TramiteId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['TramiteId']);
    $('td', theNode)[9].setAttribute('class', 'TramiteId hidden');
}
var edit = false;
function EditarTramite(e) {
    e.stopPropagation();
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $("#theHeader").html("Editar | " + desc);
    $('#modalTramites').modal('show');
    $('#hidden_id').val(id);
    //console.log(id);
    edit = true;
    GetTramiteBy(id);
}


function NuevoTramite(e) {
    edit = false;
    e.stopPropagation();
    var id = -1;
    $("#theHeader").html("Nuevo trámite");
    $('#hidden_id').val("");
    $("#modalTramites").find('input[type = "text"]').val("");
    $('#modalTramites').modal('show');
}

function EliminarTramite(e) {
    e.stopPropagation();
    //console.log('Cancel ticket');
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $('#hidden_id').val(id);

    $("#theHeaderEliminar").html("Eliminar Trámite | " + desc);
    $('#modalEliminarTramite').modal('show');
    $('#modalmessage').html('¿Realmente desea eliminar el trámite: <b>' + desc + '</b>?');
}

function GuardarTramite(){
    try {
        var inputs = [];
        var mensaje = [];
        /*Recorremos el contenedor para obtener los valores*/
        $('#form [type=text]').each(function () {
            /*Llenamos los arreglos con la info para la validacion*/
            inputs.push('#' + $(this).attr('id'));
            mensaje.push($(this).attr('attr-message'));
        });
        /*Si la validación es correcta ejecuta el ajax*/
        if (Validar(inputs, mensaje)) {
            var path = urljs + 'tramites/SaveData';
            var id = $('#hidden_id').val();
            if (id == "") {
                id = -1;
            }
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {
                TramiteId: id,
                TramiteDescripcion: $('#txt_descripcion').val(),
                TramiteAbreviatura: $('#txt_abreviatura').val(),
                TramiteDuracion: $('#txt_duracion').val(),
                TramiteAlertaPrevia: $('#txt_alertaPrevia').val(),
                TramiteToleranciaAlCliente: $('#txt_toleranciaAlCliente').val(),
                TramiteToleranciaDelCliente: $('#txt_toleranciaDelCliente').val(),
                TramiteToleranciaFinalizacion: $('#txt_toleranciaFinalizacion').val(),
                TramiteTiempoMuerto: $('#txt_tiempoMuerto').val()
            }
            var posting = $.post(path, data);
            posting.done(function (data, status, xhr) {
                //console.log(data);
                GenerarSuccessAlerta('Trámite guardado exitosamente', "success");
                $('#modalTramites').modal('hide');
                LimpiarInput(inputs, ['']);
                GetTramites();
            });
            posting.fail(function (data, status, xhr) {
                console.log(status);
            });
        }
    } catch (e) {
        console.log(e);
    }
}

$('#modalTramites').on('hidden.bs.modal', function (e) {
  limpiarMensajesValidacion($('#form'));
  edit = false;
});