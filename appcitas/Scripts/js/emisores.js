var marcaSeleccionada = -1;
$(document).ready(function ()
{
    jQuery.ajaxSetup({ async: false });
    checkUserAccess('CONF070');
    jQuery.ajaxSetup({ async: true });

    $('.numero10').mask('##########');
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar100').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-. 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    
    $(".timepicker").timepicker({
        format: 'HH:mm',
        
    });
    $('#div_fecha').datetimepicker({
        locale: 'es',/*
        maxDate: hoy,
        defaultDate: hoy,*/
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

    //LimpiarTabla('#tableEmisores');
    
    $('#btnEliminar').click(function () {
        var id = $('#hidden_id').val();
        var path = urljs + 'emisores/delete';
        var posting = $.post(path, { EmisorId: Number(id) });
        posting.done(function (data, status, xhr) {
            if (data.Accion > 0) {
                GetEmisoresByMarca(marcaSeleccionada);
                $('#modalEliminarEmisor').modal('hide');
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
    });

    jQuery.ajaxSetup({ async: false, global: false });
    constructor_marcas(0);
    jQuery.ajaxSetup({ async: true });
    
    jQuery.ajaxSetup({ async: false });
    constructor_marcas(1);
    $("#cmb_marca").trigger('change');
    jQuery.ajaxSetup({ async: true });

    constructor_segmentos();
    jQuery.ajaxSetup({ global: true });
}); /* Fin Document.Ready() */

function constructor_marcas(all){
    if(all == 1){
        var $select         = $("#cmb_marca");
        var texto_inicial   = "Todas las marcas";
    }
    else{
        var $select = $("#marca");
        var texto_inicial = "Ninguna";
    }
    $select.empty().append( new Option('No se ha cargado información','-1') );
    try {
        var path = urljs + "/configuracion/getParametosByIdEncabezado";
        var posting = $.post(path, { ConfigId: 'MRCA' });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    $select.empty().append( new Option(texto_inicial,'-1') );
                    for (var i = data.length - 1; i >= 0; i--) {
                        $select.append(new Option(data[contador].ConfigItemDescripcion, data[contador].ConfigItemID));
                        contador++;
                    }
                }
            }
        });
    }
    catch (e) {
        //RemoveAnimation("body");
    }
}

function constructor_segmentos(){
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#cod_segmento", ' +
        '"url": "configuracion/getParametosByIdEncabezado/", ' +
        '"val": "ConfigItemID", ' +
        '"type": "GET", ' +
        '"data": "SEGM", ' +
        '"text": "ConfigItemDescripcion"}');
    LoadComboBox(infojson);
}

$('#cmb_marca').on('change', function(event){
    marcaSeleccionada = $(this).val();
    GetEmisoresByMarca(marcaSeleccionada);
});

function GetEmisoresByMarca(marca) {
    var marca = marca || -1;
    try {
        var path = urljs + "/Emisores/GetByMarca";
        var posting = $.post(path, { MarcaId: marca });
        posting.done(function (data, status, xhr) {
            LimpiarTabla("#tableEmisores");
            if (data.length > 0) {
                if (data[0].Accion > 0) {
                    var counter = 1;
                    for (var i = data.length - 1; i >= 0; i--) {
                        addRowEmisor(data[i], "#tableEmisores", counter);
                        counter++;
                    }
                }
                else {
                    GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();
                }
            }
        });
    }
    catch (e) {
        //RemoveAnimation("body");
        //console.log(e);
    }
}

function addRowEmisor(ArrayData, tableID, counter) {
    var newRow = $(tableID).dataTable().fnAddData([
        counter,
        ArrayData['EmisorCuenta'],
        ArrayData['MarcaTarjeta'],
        ArrayData['Segmento'],
        ArrayData['Producto'],
        ArrayData['Familia'],
        ArrayData['EmisorStatusDescripcion'],
        "<button data-id='" + ArrayData['EmisorId'] + "' data-name='" + ArrayData['Producto'] + "' title='Editar emisor' data-toggle='tooltip' onClick='EditarEmisor(event)' id='btnEditaremisor" + ArrayData['EmisorId'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
        "<button data-id='" + ArrayData['EmisorId'] + "' data-name='" + ArrayData['Producto'] + "' title='Eliminar emisor' data-toggle='tooltip' onClick='EliminarEmisor(event)' id='btnEliminarEmisor" + ArrayData['EmisorId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        ArrayData['EmisorId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['EmisorId']);
    $('td', theNode)[8].setAttribute('class', 'EmisorId hidden');
}

function NuevoEmisor(e) {
    e.stopPropagation();
    var id = -1;
    $("#theHeader").html("Nuevo Emisor");
    $('#hidden_id').val("");
    $("#modalEmisores").find('input[type = "text"]').val("");
    $('#modalEmisores select').val(-1).trigger('change');
    $('#modalEmisores').modal('show');
}

function EditarEmisor(e) {
    e.stopPropagation();
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $("#theHeader").html("Editar | " + desc);
    $('#modalEmisores').modal('show');
    $('#hidden_id').val(id);
    //console.log(id);
    GetEmisorBy(id);
}

function EliminarEmisor(e) {
    e.stopPropagation();
    //console.log('Cancel ticket');
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $('#hidden_id').val(id);

    $("#theHeaderEliminar").html("Eliminar Emisor | " + desc);
    $('#modalEliminarEmisor').modal('show');
    $('#modalmessage').html('¿Realmente desea eliminar el emisor: <b>' + desc + '</b>?');
}


function GetEmisorBy(id) {
    try {
        var path = urljs + "/emisores/GetEmisorById";
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            if (data.Accion > 0) {
                $('#txt_producto').val(data.Producto);
                $('#txt_familia').val(data.Familia);
                $('#txt_emisorCuenta').val(data.EmisorCuenta);
                $('#marca').val(data.MarcaId).trigger('change');
                $('#cod_segmento').val(data.SegmentoId).trigger('change');
                $('#cmb_status').val(data.EmisorStatus).trigger('change');
            }
            else {
                GenerarErrorAlerta(data.Mensaje, "error");
                goAlert();
            }
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
    }
    catch (e) {
        //RemoveAnimation("body");
        //console.log(e);
    }
}

$('#btnGuardar').click(function (event) {
    if($('#hidden_id').val() == ""){
        existeEmisor($('#txt_emisorCuenta').val());
    }
    else{
        GuardarEmisor();
    }
});

function existeEmisor(EmisorCuenta) {
    var resultado = false;
    try {
        var path = urljs + "/emisores/CheckOne";
        var posting = $.post(path, { EmisorCuenta: EmisorCuenta });
        posting.done(function (data, status, xhr) {
            if(data.cantidadRegistros > 0){
                $('#modalEmisores').modal('hide');
                GenerarErrorAlerta('Registro ya existe en la base de datos.', "error");
                goAlert();
                resultado = true;
            }
            else{
                GuardarEmisor();
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

var inputs = ['#txt_emisorCuenta', '#txt_producto', '#txt_familia', '#marca', '#cod_segmento', '#hidden_id'];
function GuardarEmisor(){
    try {
        var inputs = [];
        var mensaje = [];
        /*Recorremos el contenedor para obtener los valores*/
        $('#form .requerido').each(function () {
            /*Llenamos los arreglos con la info para la validacion*/
            if($(this).data('requerido') == true){
                inputs.push('#' + $(this).attr('id'));
                mensaje.push($(this).attr('attr-message'));
            }
        });
        /*Si la validación es correcta ejecuta el ajax*/
        if (Validar(inputs, mensaje)) {
            var path = urljs + 'emisores/SaveData';
            var id = $('#hidden_id').val();
            if (id == "") {
                id = -1;
            }
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {
                EmisorId: id,
                Producto: $('#txt_producto').val(),
                EmisorCuenta: $('#txt_emisorCuenta').val(),
                Familia: $('#txt_familia').val(),
                MarcaId: $('#marca').val(),
                SegmentoId: $('#cod_segmento').val(),
                EmisorStatus: $('#cmb_status').val()
            }
            var posting = $.post(path, data);
            posting.done(function (data, status, xhr) {
                //console.log(data);
                $('#modalEmisores').modal('hide');
                GenerarSuccessAlerta(data.Mensaje, "success");
                goAlert();
                LimpiarInput(inputs, ['']);
                GetEmisoresByMarca(marcaSeleccionada);
            });
        }
    } catch (e) {
        //console.log(e);
    }
}

$('#modalEmisores').on('hidden.bs.modal', function (e) {
  limpiarMensajesValidacion($('#form'));
});