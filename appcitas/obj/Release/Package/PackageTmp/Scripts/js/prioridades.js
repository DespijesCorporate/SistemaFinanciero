$(document).ready(function ()
{
    jQuery.ajaxSetup({ async: false });
    checkUserAccess('CONF020');
    jQuery.ajaxSetup({ async: true });

    $('.numero').mask('####');
    $('.varchar5').mask('AAAAA', { translation: { 'A': { pattern: /[A-Za-z]/, optional: true } } });
    $('.varchar10').mask('AAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar100').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('#PrioridadNombre').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } }); 
    GetPrioridades();
    //LimpiarTabla('#tableTipoRazones');
    $('#btnGuardar').click(function () {        
        if ($('#hidden_id').val() == '' || $('#hidden_id').val() == null) {
            existePrioridad($('#PrioridadNombre').val(), $('#PrioridadCodigo').val(), $('#Identificador').val());
        }
        else {
            GuardarPrioridad();
        }
    });

    $('#btnEliminar').click(function () {
        var id = $('#hidden_id').val();
        var path = urljs + 'Prioridades/delete';
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            if (data.Accion > 0) {
                GetPrioridades();
                $('#modalEliminarPrioridad').modal('hide');
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

var inputs = ['#PrioridadNombre', '#PrioridadCodigo', '#PrioridadNivel', '#Identificador'];

function GetPrioridades() {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/Prioridades/GetAll";
        var posting = $.post(path, { param1: 'value1' });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tablePrioridades");
            if(data.length) {
                if (data[0].Accion > 0) {
                    for (var i = data.length - 1; i >= 0; i--) {
                        addRow(data[i], "#tablePrioridades");
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

function GetPrioridadBy(id) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/Prioridades/GetOne";
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            console.log(data);
            if (data.Accion > 0) {
                $('#PrioridadNombre').val(data.PrioridadNombre);
                $('#PrioridadCodigo').val(data.PrioridadCodigo);
                $('#PrioridadNivel').val(data.PrioridadNivel);
                $('#Identificador').val(data.Identificador);
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
    var TipoTieneListadoExtra = "No";
    if (ArrayData['TipoTieneListadoExtra']) {
        TipoTieneListadoExtra = "Si";
    }

    var newRow = $(tableID).dataTable().fnAddData([
        ArrayData['PrioridadNombre'],
        ArrayData['PrioridadCodigo'],
        ArrayData['PrioridadNivel'],
        ArrayData['Identificador'],
        "<button data-id='" + ArrayData['PrioridadId'] + "' data-name='" + ArrayData['PrioridadNombre'] + "' title='Editar Prioridad' data-toggle='tooltip' onClick='EditarPrioridad(event)' id='btnEditarPrioridad" + ArrayData['PrioridadId'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
        "<button data-id='" + ArrayData['PrioridadId'] + "' data-name='" + ArrayData['PrioridadNombre'] + "' title='Eliminar Prioridad' data-toggle='tooltip' onClick='EliminarPrioridad(event)' id='btnEliminarPrioridad" + ArrayData['PrioridadId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        ArrayData['PrioridadId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['PrioridadId']);
    $('td', theNode)[5].setAttribute('class', 'PrioridadId hidden');
}

function EditarPrioridad(e) {
    e.stopPropagation();

    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');

    $("#theHeader").html("Editar | " + desc);
    $('#modal_prioridades').modal('show');
    $('#hidden_id').val(id);
    GetPrioridadBy(id);
}

function EliminarPrioridad(e) {
    e.stopPropagation();
    //console.log('Cancel ticket');
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $('#hidden_id').val(id);

    $("#theHeaderEliminar").html("Eliminar Prioridad | " + desc);
    $('#modalEliminarPrioridad').modal('show');
    $('#modalmessage').html('¿Desea eliminar la prioridad: <b>' + desc + '</b>?');
}

function NuevaPrioridad(e) {
    e.stopPropagation();
    var id = -1;
    $("#theHeader").html("Nueva prioridad");
    $('#hidden_id').val("");
    $("#modal_prioridades").find('input[type = "text"]').val("");
    $('#PrioridadNombre').val('');
    $('#modal_prioridades').modal('show');
}

function GuardarPrioridad() {
    try {
        var inputs = [];
        var mensaje = [];


        /*Recorremos el contenedor para obtener los valores*/
        $('#form [type=text], #form textarea').each(function () {
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

            var path = urljs + 'Prioridades/SaveData';
            var id = $('#hidden_id').val();
            console.log('id: ' + $('#hidden_id').val());

            if (id == "") {
                id = -1;
            }
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {
                PrioridadId: id,
                PrioridadNombre: $('#PrioridadNombre').val(),
                PrioridadCodigo: $('#PrioridadCodigo').val(),
                PrioridadNivel: $('#PrioridadNivel').val(),
                Identificador: $('#Identificador').val()
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
                GenerarSuccessAlerta('Prioridad guardada exitosamente', "success");
                $('#modal_prioridades').modal('hide');
                LimpiarInput(inputs, ['']);
                GetPrioridades();
            });

            posting.fail(function (data, status, xhr) {
                console.log(status);
            });


        }
    } catch (e) {
        console.log(e);
    }
}

function existePrioridad(nombre, codigo, identificador) {
    var resultado = false;
    try {
        var path = urljs + "/Prioridades/CheckOne";
        var posting = $.post(path, { nombre: nombre, codigo: codigo, identificador: identificador });
        posting.done(function (data, status, xhr) {
            console.log('Registros: ' + data.cantidadRegistros);
            if (data.cantidadRegistros > 0) {
                $('#modal_prioridades').modal('hide');
                GenerarErrorAlerta('Registro ya existe en la base de datos.', "error");
                goAlert();
                resultado = true;
                console.log('Res1: ' + resultado);
            }
            else {
                GuardarPrioridad();
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