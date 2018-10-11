$(document).ready(function ()
{
    jQuery.ajaxSetup({ async: false });
    checkUserAccess('CONF010');
    jQuery.ajaxSetup({ async: true });

    $('.numero').mask('####');
    $('.varchar5').mask('AAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar10').mask('AAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar100').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar200').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    LoadEncabezados();
    $('#tablePadre').on('click', 'tbody tr', function (event) {
        event.stopPropagation();
        event.preventDefault();
        event.stopImmediatePropagation();
        var id = $(this).attr('id');
        $('#tablePadre').find('tr').each(function (index, el) {
            $(el).removeClass('selected');
        });
        $(this).addClass('selected');
        if (id != "") {
            getDescripcionByIdEncabezado(id);
            $('#hiddenIdEncabezado').val(id);
        }
        else {
            $('#btnAgregarParametros').addClass('hidden');
            GenerarErrorAlerta("Seleccione un registro valido!", "error");
            goAlert();
        }
    });

    $('#btnAgregarEncabezado').on('click', function (event) {
        event.preventDefault();
        LimpiarInput(inputsEncabezado, cmbEncabezado);
        $('#modalEncabezados').modal('show');
        $('#actionuser').val('new');
        $('#hiddenIdEncabezado').val("NULL");
        $('#txtCodEncabezado').removeAttr('disabled');
        hideorshowEncabezadoText(true);
        hideorshowParametrosText('new');
        LimpiarTabla('#tableHijo');
    });

    $('#btnAgregarParametros').on('click', function (event) {
        /*Levanta la modal para agregar parametros*/
        event.preventDefault();
        LimpiarInput(inputsElementos, ['']);
        $('#txt_itemDescripcion').find('p').text("");
        LimpiarTabla('#table_parametros');
        $('#modalParametros').modal('show');
        $('#actionuserParametros').val('new');
        $('#hiddenIdParametros').val("");
        hideorshowParametrosText('new');
    });

    $('#btnParametroAdd').on('click', function (event) {
        if( $('#hiddenIdParametros').val() == ''){
            existeElemento($('#hiddenIdEncabezado').val(), $('#txt_itemId').val(),$('#txt_itemDescripcion').val(),$('#txt_itemAbreviatura').val())
        }
        else{
            NuevoElemento();
        }
    });

    $('#btnGuardarParametros').on('click', function (event) {
            GuardarElemento();
    });

    $('#btnGuardarEncabezado').on('click', function (event) {
        $('#btnAgregarParametros').addClass('hidden'); 
        if(edit){ 
            GuardarCatalogo(); 
        } 
        else {
            existeCatalogo($('#txtCodEncabezado').val(),$('#txtDesc').val());
        }
    });
});

var inputsEncabezado = ['#txtCodEncabezado', '#txtDesc', '#txtObs', '#hiddenIdEncabezado', '#actionuser'];
var cmbEncabezado = ['#cmbTipo'];
var inputsElementos = ['#txt_itemId', '#txt_itemAbreviatura', '#txt_itemDescripcion', '#txt_itemObservacion']; 

function LoadEncabezados() {
    //LoadAnimation("body");
    LimpiarTabla('#tablePadre');
    var path = urljs + 'configuracion/GetAll/';
    var posting = $.post(path, { null: 'null' });
    posting.done(function (data) {
        if (data[0].Accion) {            
            for (var i = data.length - 1; i >= 0; i--) {
                AddRowEncabezados(data[i], '#tablePadre');
            }
        }
        else {
            GenerarErrorAlerta(data[0].Mensaje, "error");
            goAlert();
        }
    });

    posting.fail(function (data, status, xhr) {
        GenerarErrorAlerta("Error cargando los encabezados - , " + status, "error");
        goAlert();
    });

    posting.always(function (data, status, xhr) {
        /*quitamos la animacion*/
        //RemoveAnimation("body");
    });
}

function getDescripcionByIdEncabezado(pid) {
    //LoadAnimation("body");
    var path = urljs + 'configuracion/getParametosByIdEncabezado/';
    var posting = $.post(path, { ConfigId: pid });
    posting.done(function (data) {
        if (data[0].Accion) {
            LimpiarTabla('#tableHijo');
            $("#panel_heading_catalogo_nuevo").text(data[0].ConfigDescripcion);
            for (var i = data.length - 1; i >= 0; i--) {
                AddRowParametros(data[i], '#tableHijo');
            }
            $('#btnAgregarParametros').removeClass('hidden');
        }
        else {
            $('#btnAgregarParametros').removeClass('hidden');
            LimpiarTabla('#tableHijo');
            GenerarErrorAlerta(data[0].Mensaje, "error");
            goAlert();
        }
    });

    posting.fail(function (data, status, xhr) {
        GenerarErrorAlerta("Error cargando los encabezados - , " + status, "error");
        goAlert();
    });

    posting.always(function (data, status, xhr) {
        /*quitamos la animacion*/
        //RemoveAnimation("body");
    });
}

function AddRowEncabezados(ArrayData, table) {
    var newRow = $(table).dataTable().fnAddData([
		ArrayData['ConfigID'],
		ArrayData['ConfigDesc'],
		ArrayData['ConfigObs'],		
        "<button data-id=" + ArrayData['ConfigID'] + " id='btnEditar" + ArrayData['ConfigID'] + "' onClick='EditarEncabezado(event)' title='Editar catálogo' data-toggle='tooltip' class='btn btn-primary botonVED text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" + 
        "<button data-id=" + ArrayData['ConfigID'] + " id='btnEliminiar" + ArrayData['ConfigID'] + "' onClick='EliminarEncabezado(event)' title='Eliminar catálogo' data-toggle='tooltip' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>"
    ]);

    var theNode = $(table).dataTable().dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['ConfigID']);
    $('td', theNode)[3].setAttribute('class', 'text-center');
}

function AddRowParametros(ArrayData, table) {
    var newRow = $(table).dataTable().fnAddData([
        ArrayData['ConfigItemID'],
		ArrayData['ConfigItemDescripcion'],
		ArrayData['ConfigItemAbreviatura'],
        "<button data-id=" + ArrayData['ConfigItemID'] + " id='btnEditar" + ArrayData['ConfigItemID'] + "' onClick='EditarParametro(event)' title='Editar elemento' data-toggle='tooltip' class='btn btn-sm btn-primary botonVED text-center'><i class='icon-edit icon-white fa fa-pencil-square-o'></i></button>" + 
        "<button data-id=" + ArrayData['ConfigItemID'] + " id='btnEliminiar" + ArrayData['ConfigItemID'] + "' onClick='EliminarParametro(event)' title='Eliminar elemento' data-toggle='tooltip' class='btn btn-sm btn-danger botonVED text-center'><i class='icon-edit icon-white fa fa-trash-o'></i></button>"
    ]);

    var theNode = $(table).dataTable().dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['ConfigItemID']);
}

function AddRowParametrosTemp(ArrayData, table) {
    var newRow = $(table).dataTable().fnAddData([
        ArrayData['ID'],
		ArrayData['DESCRIPCION'],
        ArrayData['ABREVIATURA'],
		ArrayData['OBSERVACION']
    ]);
}

function EliminarParametroTemp(e){
    //e.stopPropagation();
    $boton = $(e.currentTarget);
    var $Table = $boton.closest('#table_parametros');
    var TableData = $Table.DataTable();
    $boton.on('click', function(event){
        TableData.row( $(this).parents('tr') ).delete().draw();
    });
}
var edit = false;
function EditarEncabezado(e) {
    e.stopPropagation();
    //LoadAnimation('body');
    var id = $(e.currentTarget).attr('data-id');
    LimpiarInput(inputsEncabezado, cmbEncabezado);
    $('#modalEncabezados').modal('show');
    hideorshowEncabezadoText(true);
    hideorshowParametrosText('edit');
    $('#actionuser').val('edit');
    $('#hiddenIdEncabezado').val(id);
    edit = true;
    var path = urljs + 'configuracion/GetOne/';
    var posting = $.post(path, { id: id });
    posting.done(function (data) {
        if (data.Accion)
        {
            $('#txtCodEncabezado').prop('disabled', 'disabled');
            $('#txtCodEncabezado').val(data.ConfigID);
            $('#txtDesc').val(data.ConfigDesc);
            $('#txtObs').val(data.ConfigObs);
        }
        else {
            GenerarErrorAlerta(data.Mensaje, "error");
            goAlert();
        }
    });

    posting.fail(function (data, status, xhr) {
        GenerarErrorAlerta(status, "error");
        goAlert();
    });

    posting.always(function () {
        //RemoveAnimation("body");
    });
}

function EditarParametro(e) {
    e.stopPropagation();
    //LoadAnimation('body');
    var ConfigId = $('#hiddenIdEncabezado').val();;
    var ConfigItemId = $(e.currentTarget).attr('data-id');
    $('#modalParametros').modal('show');
    hideorshowParametrosText('edit');
    $('#actionuserParametros').val('edit');
    $('#hiddenIdParametros').val(ConfigItemId);
    edit = true;
    var path = urljs + 'configuracion/getParametosByIdEncabezado/';
    var posting = $.post(path, { ConfigId: ConfigId, ConfigItemId: ConfigItemId });
    posting.done(function (data) {
        if (data[0].Accion) {
            console.log(data);
            $("#panel_heading_catalogo_edit").text(data[0].ConfigDescripcion);
            $('#txt_itemId_edit').val(data[0].ConfigItemID); 
            $('#txt_itemDescripcion_edit').val(data[0].ConfigItemDescripcion);
            $('#txt_itemAbreviatura_edit').val(data[0].ConfigItemAbreviatura);
            $('#txt_itemObservacion_edit').val(data[0].ConfigItemObservacion);
        }
        else {
            console.log('Nada');
            $('#modalParametros').modal('hide');
            GenerarErrorAlerta(data.Mensaje, "error");
            goAlert();
        }
    });

    posting.fail(function (data, status, xhr) {
        $('#modalParametros').modal('hide');
        GenerarErrorAlerta(status, "error");
        goAlert();
    });

    posting.always(function () {
        //RemoveAnimation("body");
    });
}

function hideorshowEncabezadoText(valor) {
    if (valor) {
        $('#contenedor_delete').addClass('hidden');
        $('#contenedor_inputs').removeClass('hidden');
        $('#btnGuardarEncabezado').text('Guardar').addClass('btn-primary').removeClass('btn-danger');
    }
    else {
        $('#contenedor_inputs').addClass('hidden');
        $('#contenedor_delete').removeClass('hidden');
        $('#btnGuardarEncabezado').text('Eliminar').addClass('btn-danger').removeClass('btn-primary');
        $('#myLabelEncabezados').text('Eliminar catálogo');
    }
}

function hideorshowParametrosText(valor) {
    $("#btnGuardarParametros").removeClass('hidden');
    if (valor == 'new') {
        $('#contenedor_inputs_parametros').removeClass('hidden');
        $('#contenedor_edit').addClass('hidden');
        $('#contenedor_delete_parametro').addClass('hidden');
        $('#btnGuardarParametros').text('Guardar').addClass('btn-primary').removeClass('btn-danger');
        $('#myLabelParametros').text('Nuevo elemento');
        $('#myLabelEncabezados').text('Nuevo catálogo');
        $("#btnGuardarParametros").addClass('hidden');
    }
    else if (valor == 'edit') {
        $('#contenedor_edit').removeClass('hidden');
        $('#contenedor_inputs_parametros').addClass('hidden');
        $('#contenedor_delete_parametro').addClass('hidden');
        $('#btnGuardarParametros').text('Guardar').addClass('btn-primary').removeClass('btn-danger');
        $('#myLabelParametros').text('Editar elemento');
        $('#myLabelEncabezados').text('Editar catálogo');
    }
    else if (valor == 'delete') {
        $('#contenedor_delete_parametro').removeClass('hidden');
        $('#contenedor_inputs_parametros').addClass('hidden');
        $('#contenedor_edit').addClass('hidden');
        $('#btnGuardarParametros').text('Eliminar').addClass('btn-danger').removeClass('btn-primary');
        $('#myLabelParametros').text('Eliminar elemento');
        $('#myLabelEncabezados').text('Eliminar catálogo');
    }
}

function EliminarEncabezado(e) {
    e.stopPropagation();
    var id = $(e.currentTarget).attr('data-id');
    $('#modalEncabezados').modal('show');
    hideorshowEncabezadoText(false);
    $('#actionuser').val('delete');
    $('#hiddenIdEncabezado').val(id);
    edit = true;
    var path = urljs + 'configuracion/GetOne/';
    var posting = $.post(path, { id: id });
    posting.done(function (data) {
        if (data.Accion) {
            $('#txtCodEncabezado').val(data.ConfigID);
            $('#txtDesc').val(data.ConfigDesc);
            $('#txtObs').val(data.ConfigObs);
        }
        else {
            GenerarErrorAlerta(data.Mensaje, "error");
            goAlert();
        }
    });

    posting.fail(function (data, status, xhr) {
        GenerarErrorAlerta(status, "error");
        goAlert();
    });

    posting.always(function () {
        //RemoveAnimation("body");
    });
}

function EliminarParametro(e) {
    e.stopPropagation();
    var id = $(e.currentTarget).attr('data-id');
    $('#modalParametros').modal('show');
    hideorshowParametrosText('delete');
    $('#actionuserParametros').val('delete');
    $('#hiddenIdParametros').val(id);
}

$('#modalParametros').on('hidden.bs.modal', function (e) { 
    limpiarMensajesValidacion($('#modalParametros'));
    LimpiarTabla("#table_parametros");
}); 
 
$('#modalEncabezados').on('hidden.bs.modal', function (e) { 
  limpiarMensajesValidacion($('#modalEncabezados')); 
}); 
 
function existeCatalogo(id,descripcion) { 
    var resultado = false; 
    try { 
        var path = urljs + "/configuracion/CheckOne"; 
        var posting = $.post(path, { id: id, descripcion: descripcion }); 
        posting.done(function (data, status, xhr) { 
            console.log('Registros: '+data.cantidadRegistros); 
            if(data.cantidadRegistros > 0){ 
                $('#modalEncabezados').modal('hide'); 
                GenerarErrorAlerta('Registro ya existe en la base de datos.', "error"); 
                goAlert(); 
                resultado = true; 
                console.log('Res1: '+resultado); 
            } 
            else{ 
                GuardarCatalogo(); 
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
 
function GuardarCatalogo(){ 
    var path = ""; 
    if ($('#actionuser').val() == 'new') { 
        path = urljs + 'configuracion/SaveData/'; 
    } 
    else if ($('#actionuser').val() == 'edit') { 
        path = urljs + 'configuracion/SaveData/'; 
    } 
    else { 
        path = urljs + 'configuracion/delete/';
        $('#btnAgregarParametros').addClass('hidden');
    } 
    var inputs = ['#txtCodEncabezado', '#txtDesc', '#txtObs']; 
    var errors = ['Debe especificar un código', 'Desde especificar una descripción', 'Debe especificar una breve observación']; 
    if (Validar(inputs, errors)) { 
         
        var params = { 
            ConfigID: $('#txtCodEncabezado').val(), 
            ConfigDesc: $('#txtDesc').val(), 
            ConfigObs: $('#txtObs').val(), 
            Estado: "1", 
            Mensaje : "" 
        } 
 
        var posting = $.post(path, params); 
        posting.done(function (data) { 
 
            if ($('#actionuser').val() == 'delete') { 
                LimpiarTabla('#tableHijo'); 
            } 
 
            if (data.Accion) { 
                LoadEncabezados(); 
                $('#modalEncabezados').modal('hide'); 
                LimpiarInput(['#txtCodEncabezado', '#txtDesc', '#txtObs', '#hiddenIdEncabezado', '#actionuser'], ['']); 
                GenerarSuccessAlerta(data.Mensaje, 'success'); 
                goAlert(); 
            } 
            else { 
                $('#modalEncabezados').modal('hide'); 
                LimpiarInput(['#txtCodEncabezado', '#txtDesc', '#txtObs'], ['#cmbTipo']); 
                GenerarErrorAlerta(data.Mensaje, "error"); 
                goAlert(); 
            } 
            edit = false;
        }); 
 
        posting.fail(function (data, status, xhr) { 
            $('#modalEncabezados').modal('hide'); 
            LimpiarInput(['#txtCodEncabezado', '#txtDesc', '#txtObs'], ['#cmbTipo']); 
            GenerarErrorAlerta(status, "error"); 
            goAlert();
            edit = false; 
        }); 
 
        posting.always(function () { 
            edit =  false; 
        }); 
    } 
    else { 
        /*console.log('No paso la validacion'); 
        GenerarErrorAlerta('Hay datos incorrectos, verifique e intente de nuevo!', "error"); 
        goAlert();*/ 
    }
}

function existeElemento(idConfig,id,descripcion,abreviatura) { 
    var resultado = false; 
    try { 
        var path = urljs + "/configuracion/CheckOneItem"; 
        var posting = $.post(path, { idConfig: idConfig, id: id, descripcion: descripcion, abreviatura: abreviatura }); 
        posting.done(function (data, status, xhr) { 
            console.log('Registros: '+data.cantidadRegistros); 
            if(data.cantidadRegistros > 0){ 
                $('#modalParametros').modal('hide'); 
                GenerarErrorAlerta('Registro ya existe en la base de datos.', "error"); 
                goAlert(); 
                resultado = true;
            } 
            else{ 
                NuevoElemento();
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

function GuardarElemento(){
    var path = "";
    var action = $('#actionuserParametros').val();
    if (action == 'new') {
        getDescripcionByIdEncabezado($('#hiddenIdEncabezado').val());
        $('#modalParametros').modal('hide');
        GenerarSuccessAlerta('Items agregados satisfactoriamente', "success");
        goAlert();
        LimpiarTabla("#table_parametros");
    }
    else if (action == 'edit') {
        path = urljs + '/configuracion/SaveDataItem';
        if (Validar(["#txt_itemDescripcion_edit", "#txt_itemAbreviatura_edit", "#txt_itemObservacion_edit"], ["Ingrese una descripción válida", "Ingrese un parametro válido", "Ingrese una observación válida"])) {
            var params = {
                ConfigId: $('#hiddenIdEncabezado').val(),
                ConfigItemId: $('#txt_itemId_edit').val(),
                ConfigItemAbreviatura: $('#txt_itemAbreviatura_edit').val(),
                ConfigItemDescripcion: $('#txt_itemDescripcion_edit').val(),
                ConfigItemObservacion: $('#txt_itemObservacion_edit').val(),
                Estado: "1",
                Mensaje : ""
            }
            var posting = $.post(path, params);
            posting.done(function (data) {
                if (data.Accion) {
                    getDescripcionByIdEncabezado($('#hiddenIdEncabezado').val());
                    $('#modalParametros').modal('hide');
                    GenerarSuccessAlerta(data.Mensaje, "success");
                    goAlert();
                }
                else {
                    $('#modalParametros').modal('hide');
                    GenerarErrorAlerta(data.Mensaje, "error");
                    goAlert();
                }
                LimpiarInput(["#txt_itemDescripcion_edit", "#txt_itemAbreviatura_edit", "#txt_itemObservacion_edit"], ['']);
            });

            posting.fail(function (data, status, xhr) {
                $('#modalEncabezados').modal('hide');
                LimpiarInput(["#txt_itemDescripcion_edit", "#txt_itemAbreviatura_edit", "#txt_itemObservacion_edit"], ['#cmbTipo']);
                GenerarErrorAlerta(status, "error");
                goAlert();
            });
            posting.always(function () {

            });
        }
    }
    else {
        path = urljs + '/configuracion/deleteItem';
        var params = {
            ConfigID: $('#hiddenIdEncabezado').val(),
            ConfigItemId: $('#hiddenIdParametros').val()
        }
        var posting = $.post(path, params);
        posting.done(function (data) {
            console.log(data);
            if (data.Accion) {
                getDescripcionByIdEncabezado($('#hiddenIdEncabezado').val());
                $('#modalParametros').modal('hide');
                LimpiarInput(['#txtparametro_edit'], ['']);
                GenerarSuccessAlerta(data.Mensaje, "success");
                goAlert();
            }
            else {
                $('#modalParametros').modal('hide');
                LimpiarInput(['#txtparametro_edit'], ['']);
                GenerarErrorAlerta(data.Mensaje, "error");
                goAlert();
            }
            getDescripcionByIdEncabezado($('#hiddenIdEncabezado').val());
        });

        posting.fail(function (data, status, xhr) {
            $('#modalEncabezados').modal('hide');
            LimpiarInput(['#txtDesc', '#txtObs'], ['#cmbTipo']);
            GenerarErrorAlerta(status, "error");
            goAlert();
        });

        posting.always(function () {
            //RemoveAnimation("body");
        });
    }
}

function NuevoElemento(){
    event.preventDefault();
    var id   = $('#txt_itemId').val();
    var desc = $('#txt_itemDescripcion').val();
    var abr  = $('#txt_itemAbreviatura').val();
    var obs  = $('#txt_itemObservacion').val();

    if (Validar(["#txt_itemDescripcion", "#txt_itemAbreviatura", "#txt_itemId", "#txt_itemObservacion"], ["Ingrese una descripción válida", "Ingrese un parametro válido", "Ingrese un identificador válido", "Ingrese una observación válida"]))
    {
        var data = { ID: id, DESCRIPCION: desc, ABREVIATURA: abr, OBSERVACION: obs }
        AddRowParametrosTemp(data, '#table_parametros');
        path = urljs + '/configuracion/SaveDataItem';
        var params = {
            ConfigId: $('#hiddenIdEncabezado').val(),
            ConfigItemId: $('#txt_itemId').val(),
            ConfigItemAbreviatura: $('#txt_itemAbreviatura').val(),
            ConfigItemDescripcion: $('#txt_itemDescripcion').val(),
            ConfigItemObservacion: $('#txt_itemObservacion').val(),
            Estado: "1",
            Mensaje : ""
        }
        var posting = $.post(path, params);
        posting.done(function (data) {
            getDescripcionByIdEncabezado($('#hiddenIdEncabezado').val());
            /*if (data.Accion) {
                getDescripcionByIdEncabezado($('#hiddenIdEncabezado').val());
                $('#modalParametros').modal('hide');
                GenerarSuccessAlerta(data.Mensaje, "success");
                goAlert();
            }
            else {
                $('#modalParametros').modal('hide');
                GenerarErrorAlerta(data.Mensaje, "error");
                goAlert();
            }*/
            LimpiarInput(["#txt_itemDescripcion", "#txt_itemAbreviatura", "#txt_itemObservacion"], ['']);
        });
        posting.fail(function (data, status, xhr) {
            $('#modalEncabezados').modal('hide');
            LimpiarInput(["#txt_itemDescripcion", "#txt_itemAbreviatura", "#txt_itemObservacion"], ['#cmbTipo']);
            GenerarErrorAlerta(status, "error");
            goAlert();
        });
        posting.always(function () {
        });
        LimpiarInput(['#txt_itemDescripcion', '#txt_itemAbreviatura', '#txt_itemId', '#txt_itemObservacion'], ['']);
    }
}
