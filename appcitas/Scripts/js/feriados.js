var anioSeleccionado = -1;
$(document).ready(function ()
{
    jQuery.ajaxSetup({ async: false });
    checkUserAccess('CONF030');
    jQuery.ajaxSetup({ async: true });

    $('.numero').mask('####');
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    
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
    LoadAniosFeriados();

    //LimpiarTabla('#tableFeriados');
    
    $('#btnEliminar').click(function () {
        var id = $('#hidden_id').val();
        var path = urljs + 'feriados/delete';
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            if (data.Accion > 0) {
                GetFeriadosByYear(anioSeleccionado);
                $('#modalEliminarFeriado').modal('hide');
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

$('#btnGuardar').click(function (event) {
    if($('#hidden_id').val() == ""){
        existeFeriado($('#txt_fecha').val());
    }
    else{
        GuardarFeriado();
    }
});

var inputs = ['#txt_descripcion', '#txt_fecha', '#cmb_tipo_feriado', '#hidden_id'];
function GetFeriados() {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");
        var path = urljs + "/feriados/GetAll";
        var posting = $.post(path, { param1: 'value1' });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableFeriados");
            if (data.length > 0) {
                if (data[0].Accion > 0) {
                    var counter = 1;
                    for (var i = data.length - 1; i >= 0; i--) {
                        //console.log(data[i]);
                        addRow(data[i], "#tableFeriados", counter);
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
            //console.log(data);
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {
        //RemoveAnimation("body");
        //console.log(e);
    }
}

function GetFeriadosByYear(year) {
    var year = year || -1;
    try {
        /*animacion de loading*/
        //LoadAnimation("body");
        var path = urljs + "/feriados/GetAllByYear";
        var posting = $.post(path, { year: Number(year) });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableFeriados");
            if (data.length > 0) {
                if (data[0].Accion > 0) {
                    var counter = 1;
                    for (var i = data.length - 1; i >= 0; i--) {
                        //console.log(data[i]);
                        addRow(data[i], "#tableFeriados", counter);
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
            //console.log(data);
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {
        //RemoveAnimation("body");
        //console.log(e);
    }
}

function GetFeriadoBy(id) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");
        var path = urljs + "/feriados/GetOne";
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            console.log(data);
            if (data.Accion > 0) {
                $('#txt_descripcion').val(data.FeriadoDescripcion);
                $('#txt_fecha').val(data.FeriadoFecha);
                $('#cmb_tipo_feriado').val(data.FeriadoTipoId).trigger('change');
                $('#txt_inicio').val(data.FeriadoHoraInicio);
                $('#txt_fin').val(data.FeriadoHoraFinal);
            }
            else {
                GenerarErrorAlerta(data.Mensaje, "error");
                goAlert();
            }
        });
        posting.fail(function (data, status, xhr) {
            //console.log(data);
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {
        //RemoveAnimation("body");
        //console.log(e);
    }
}

function addRow(ArrayData, tableID, counter) {
    var newRow = $(tableID).dataTable().fnAddData([
        counter,
        ArrayData['FeriadoDescripcion'],
        ArrayData['FeriadoFechaFormato'],
        ArrayData['FeriadoTipoDescripcion'],
        ArrayData['FeriadoHoraInicio'],
        ArrayData['FeriadoHoraFinal'],
        "<button data-id='" + ArrayData['FeriadoId'] + "' data-name='" + ArrayData['FeriadoDescripcion'] + "' title='Editar Feriado' data-toggle='tooltip' onClick='EditarFeriado(event)' id='btnEditarFeriado" + ArrayData['FeriadoId'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
        "<button data-id='" + ArrayData['FeriadoId'] + "' data-name='" + ArrayData['FeriadoDescripcion'] + "' title='Eliminar Feriado' data-toggle='tooltip' onClick='EliminarFeriado(event)' id='btnEliminarFeriado" + ArrayData['FeriadoId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        ArrayData['FeriadoId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['FeriadoId']);
    $('td', theNode)[7].setAttribute('class', 'FeriadoId hidden');
}

function NuevoFeriado(e) {
    e.stopPropagation();
    var id = -1;
    $("#theHeader").html("Nuevo Feriado");
    $('#hidden_id').val("");
    $("#modalFeriados").find('input[type = "text"]').val("");
    $('#modalFeriados').modal('show');
    LoadTiposFeriados(id);
}

function EditarFeriado(e) {
    e.stopPropagation();
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $("#theHeader").html("Editar | " + desc);
    $('#modalFeriados').modal('show');
    $('#hidden_id').val(id);
    //console.log(id);
    jQuery.ajaxSetup({async:false});
    LoadTiposFeriados(id);
    jQuery.ajaxSetup({async:true});
    GetFeriadoBy(id);
}

function EliminarFeriado(e) {
    e.stopPropagation();
    //console.log('Cancel ticket');
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $('#hidden_id').val(id);

    $("#theHeaderEliminar").html("Eliminar Feriado | " + desc);
    $('#modalEliminarFeriado').modal('show');
    $('#modalmessage').html('¿Realmente desea eliminar el feriado: <b>' + desc + '</b>?');
}

function LoadAniosFeriados() {
    var infojson = jQuery.parseJSON('{"input": "#cmb_anio_feriado", ' +
                                      '"url": "feriados/getAniosFeriado/", ' +
                                      '"val": "FeriadoAnio", ' +
                                      '"type": "GET", ' +
                                      '"data": "", ' +
                                      '"text": "FeriadoAnio"}');
    jQuery.ajaxSetup({async:false});
    LoadComboBox(infojson);
    jQuery.ajaxSetup({async:true});
    if($('#cmb_anio_feriado option').size()>1){
        $('#cmb_anio_feriado option:eq(1)').attr('selected', 'selected').trigger('change');
        anioSeleccionado = $('#cmb_anio_feriado option:eq(1)').val(); 
        GetFeriadosByYear(anioSeleccionado);
    }
}

$('#cmb_anio_feriado').on('change', function(event){
    anioSeleccionado = $(this).val();
    GetFeriadosByYear(anioSeleccionado);
});


function LoadTiposFeriados(id) {
    var data = '{ "id": "TFER" }';
    //data = JSON.stringify(data);
    var infojson = jQuery.parseJSON('{"input": "#cmb_tipo_feriado", ' +
                                      '"url": "configuracion/getParametosByIdEncabezado/", ' +
                                      '"val": "ConfigItemID", ' +
                                      '"type": "GET", ' +
                                      '"data": "TFER", ' +
                                      '"text": "ConfigItemDescripcion"}');
    LoadComboBox(infojson);
}

$('#cmb_tipo_feriado').on('change',function(event){
    if($(this).val() == 'FERDP'){
        $("#div_feriado_parcial").removeClass('hidden');
        $("#txt_inicio").data('requerido', true);
        $("#txt_fin").data('requerido', true);
    }
    else{
        $("#div_feriado_parcial").addClass('hidden');
        $("#txt_inicio").data('requerido', false).val('');
        $("#txt_fin").data('requerido', false).val('');
        limpiarMensajesValidacion($('#div_feriado_parcial'));
    }
});

function existeFeriado(fecha) {
    var resultado = false;
    try {
        var path = urljs + "/feriados/CheckOne";
        var posting = $.post(path, { fecha: fecha });
        posting.done(function (data, status, xhr) {
            console.log('Registros: '+data.cantidadRegistros);
            if(data.cantidadRegistros > 0){
                $('#modalFeriados').modal('hide');
                GenerarErrorAlerta('Registro ya existe en la base de datos.', "error");
                goAlert();
                resultado = true;
            }
            else{
                GuardarFeriado();
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

function GuardarFeriado(){
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
            var path = urljs + 'feriados/SaveData';
            var id = $('#hidden_id').val();
            if (id == "") {
                id = -1;
            }
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {
                FeriadoId: id,
                FeriadoDescripcion: $('#txt_descripcion').val(),
                FeriadoFecha: $('#txt_fecha').val(),
                FeriadoTipoId: $('#cmb_tipo_feriado').val(),
                FeriadoHoraInicio: $('#txt_inicio').val(),
                FeriadoHoraFinal: $('#txt_fin').val()
            }
            var posting = $.post(path, data);
            posting.done(function (data, status, xhr) {
                //console.log(data);
                $('#modalFeriados').modal('hide');
                GenerarSuccessAlerta(data.Mensaje, "success");
                goAlert();
                LimpiarInput(inputs, ['']);
                GetFeriadosByYear(anioSeleccionado);
            });
            posting.fail(function (data, status, xhr) {
                //console.log(status);
            });
        }
    } catch (e) {
        //console.log(e);
    }
}

$('#modalFeriados').on('hidden.bs.modal', function (e) {
  limpiarMensajesValidacion($('#form'));
});