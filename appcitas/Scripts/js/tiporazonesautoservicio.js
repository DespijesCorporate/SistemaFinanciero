$(document).ready(function () {
    GetTipoRazones();
    $('.numero').mask('####');
    $('.varchar10').mask('AAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar100').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    //LimpiarTabla('#tableTipoRazones');
    $('#btnGuardar').click(function () {
        if ($('#hidden_id').val() == '' || $('#hidden_id').val() == null) {
            existeTipoRazon($('#abreviatura').val(), $('#descripcion_tipo_razon').val());
        }
        else {
            GuardarTipoRazon();
        }
        console.log('hidden_id: ' + $('#hidden_id').val());
    });

    $('#btnEliminar').click(function () {
        var id = $('#hidden_id').val();
        var path = urljs + 'TipoRazones/delete';
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            if (data.Accion > 0) {
                GetTipoRazones();
                $('#modalEliminarTipoRazon').modal('hide');
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

var inputs = ['#descripcion_tipo_razon', '#abreviatura', '#tiene_listado_x', '#etiqueta', '#origen', '#codigo_listado'];

function GetTipoRazones() {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/TipoRazones/GetAll1";
        var posting = $.post(path, { param1: 'value1' });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableTipoRazones");
            if (data.length) {
                if (data[0].Accion > 0) {
                    for (var i = data.length - 1; i >= 0; i--) {
                        addRow(data[i], "#tableTipoRazones");
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

function GetTipoRazonBy(id) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/TipoRazones/GetOne";
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            console.log(data);
            if (data.Accion > 0) {
                $('#abreviatura').val(data.TipoAbreviatura);
                $('#descripcion_tipo_razon').val(data.TipoDescripcion);
                $('#tiene_listado_x').val(data.TipoTieneListadoExtra);
                $('#etiqueta').val(data.TipoEtiquetaListadoExtra);
                $('#origen').val(data.TipoOrigenListadoExtra);
                $('#codigo_listado').val(data.TipoCodigoListadoExtra);
                $('#tiene_listado_x').trigger('change');
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
        ArrayData['TipoAbreviatura'],
        ArrayData['TipoDescripcion'],
        TipoTieneListadoExtra,
        ArrayData['TipoEtiquetaListadoExtra'],
        ArrayData['TipoOrigenListadoExtra'],
        ArrayData['TipoCodigoListadoExtra'],
        ArrayData['TipoStatus'],
        /*"<button data-id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['TipoDescripcion'] + "' title='Ver Razones' data-toggle='tooltip' onClick='location.href=\"../Razones/razones/" + ArrayData['TipoId'] + "\"' id='btnVerRazones" + ArrayData['TipoId'] + "' class='btn btn-success text-center btn-sm'><i class='fa fa-eye'></i></button>" +
        "<button data-id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['TipoDescripcion'] + "' title='Ver Razones' data-toggle='tooltip' onClick='VerRazones(event)' id='btnVerRazones" + ArrayData['TipoId'] + "' class='btn btn-success text-center btn-sm'><i class='fa fa-eye'></i></button>" +
        "<a data-id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['TipoDescripcion'] + "' title='Ver Razones' data-toggle='tooltip' href=\" ../Razones/razones/" + ArrayData['TipoId'] + " \" id='btnVerRazones" + ArrayData['TipoId'] + "' class='btn btn-success text-center btn-sm'><i class='fa fa-eye'></i></a>" +*/
        "<button data-id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['TipoDescripcion'] + "' title='Editar Tipo Razon' data-toggle='tooltip' onClick='EditarTipoRazon(event)' id='btnEditarTipoRazon" + ArrayData['TipoId'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
        "<button data-id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['TipoDescripcion'] + "' title='Eliminar Tipo Razon' data-toggle='tooltip' onClick='EliminarTipoRazon(event)' id='btnEliminarTipoRazon" + ArrayData['TipoId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        ArrayData['TipoId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['TipoId']);
    $('td', theNode)[8].setAttribute('class', 'TipoId hidden');
}

function EditarTipoRazon(e) {
    e.stopPropagation();

    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');

    $("#theHeader").html("Editar | " + desc);
    $('#moda_tipo_razones').modal('show');
    $('#hidden_id').val(id);
    GetTipoRazonBy(id);
}

function EliminarTipoRazon(e) {
    e.stopPropagation();
    //console.log('Cancel ticket');
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $('#hidden_id').val(id);

    $("#theHeaderEliminar").html("Eliminar Tipo Razon | " + desc);
    $('#modalEliminarTipoRazon').modal('show');
    $('#modalmessage').html('¿Desea eliminar el tipo de razon: <b>' + desc + '</b>?');
}

$('#tiene_listado_x').change(function () {
    if ($('#tiene_listado_x').val() == 1) {
        $('#div_listado_extra').removeClass('hidden');
        $('#etiqueta').data('requerido', true);
        $('#origen').data('requerido', true);
        $('#codigo_listado').data('requerido', true);
    }
    else {
        $('#div_listado_extra').addClass('hidden');
        $('#etiqueta').data('requerido', false).val('');
        $('#origen').data('requerido', false).val('');
        $('#codigo_listado').data('requerido', false).val('');
    }
});


function NuevoTipoRazon(e) {
    e.stopPropagation();
    var id = -1;
    $("#theHeader").html("Nuevo Tipo de razón");
    $('#hidden_id').val("");
    $("#moda_tipo_razones").find('input[type = "text"]').val("");
    $('#descripcion_tipo_razon').val('');
    $('#tiene_listado_x').val(0);
    $('#tipostatus').val('ACT');
    $('#tiene_listado_x').trigger('change');
    $('#moda_tipo_razones').modal('show');
}


$('#moda_tipo_razones').on('hidden.bs.modal', function (e) {
    limpiarMensajesValidacion($('#form'));
})

function GuardarClientes() {
    try {
        var inputs = [];
        var mensaje = [];


        /*Recorremos el contenedor para obtener los valores*/
        $('#form .requerido').each(function () {
            /*Llenamos los arreglos con la info para la validacion*/
            if ($(this).data('requerido') == true) {
                console.log($(this)[0].tagName);
                inputs.push('#' + $(this).attr('id'));
                mensaje.push($(this).attr('attr-message'));
            }
            console.log(inputs);
        });

        /*Si la validación es correcta ejecuta el ajax*/
        if (Validar(inputs, mensaje)) {

            var path = urljs + 'Clientes/SaveData';
            var id = $('#hidden_id').val();
            console.log('id: ' + $('#hidden_id').val());

            if (id == "") {
                id = -1;
            }
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {
                TipoId: id,
                TipoAbreviatura: $('#abreviatura').val(),
                TipoDescripcion: $('#descripcion_tipo_razon').val(),
                TipoTieneListadoExtra: $('#tiene_listado_x option:selected').val(),
                TipoEtiquetaListadoExtra: $('#etiqueta').val(),
                TipoOrigenListadoExtra: $('#origen').val(),
                TipoCodigoListadoExtra: $('#codigo_listado').val(),
                TipoStatus: $('#tipostatus').val()
            }

            var posting = $.post(path, data);

            posting.done(function (data, status, xhr) {
                $('#moda_tipo_razones').modal('hide');
                GenerarSuccessAlerta(data.Mensaje, "success");
                goAlert();
                LimpiarInput(inputs, ['']);
                GetTipoRazones();
            });

            posting.fail(function (data, status, xhr) {
                console.log(status);
            });
        }
    } catch (e) {
        console.log(e);
    }
}

function existeTipoRazon(abreviatura, descripcion) {
    var resultado = false;
    try {
        var path = urljs + "/TipoRazones/CheckOne";
        var posting = $.post(path, { abreviatura: abreviatura, descripcion: descripcion });
        posting.done(function (data, status, xhr) {
            console.log('Registros: ' + data.cantidadRegistros);
            if (data.cantidadRegistros > 0) {
                $('#moda_tipo_razones').modal('hide');
                GenerarErrorAlerta('Registro ya existe en la base de datos.', "error");
                goAlert();
                resultado = true;
            }
            else {
                GuardarClientes();
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
