var ReclamoIdSeleccionado = '';
var ItemIdSeleccionado = '';

$(document).ready(function () {
    jQuery.ajaxSetup({ async: false });
    checkUserAccess('CONF100');
    jQuery.ajaxSetup({ async: true });

    //Mascaras para formatos
    $('.numero').mask('####');
    $('.varchar5').mask('AAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar10').mask('AAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar100').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar200').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });

    CargarReclamos();

    $('#reclamosTable').on('click', 'tbody tr', function (event) {
        event.stopPropagation();
        event.preventDefault();
        event.stopImmediatePropagation();
        var id = $(this).attr('id');        
        $('#reclamosTable').find('tr').each(function (index, el) {
            $(el).removeClass('selected');
            $('#btnAgreagrVariableEvaluar').addClass('hidden');
        });
        $(this).addClass('selected');
        if (id !== "") {
            ReclamoIdSeleccionado = id;
            ItemIdSeleccionado = '';
            LimpiarTabla('#variablesEnItemTable');
            LimpiarTabla('#itemsTable');            
            $('#btnAgreagrVariableEvaluar').addClass('hidden');
            CargarItemsDeReclamos(id);
        }
    });

    $('#itemsTable').on('click', 'tbody tr', function (event) {
        event.stopPropagation();
        event.preventDefault();
        event.stopImmediatePropagation();
        var id = $(this).attr('id');        
        $('#itemsTable').find('tr').each(function (index, el) {
            $(el).removeClass('selected');
        });
        $(this).addClass('selected');
        if (id !== "") {
            ItemIdSeleccionado = id;
            LimpiarTabla('#variablesEnItemTable');
            CargarVariablesDeItem(ReclamoIdSeleccionado, id);
        }
    });
})

//Reclamos Region
function GuardarReclamo(e) {
    e.stopPropagation();
    var path = urljs + '/ConfiguracionesMotor/GuardarReclamo/';
    var form = $('#CreateEditForm');
    form.validate();

    if (form.valid()) {
        var posting = $.post(path, form.serialize());
        posting.done(function (data) {
            if (data.Accion) {
                CargarReclamos();
                $('#modalReclamo').modal('hide');
                LimpiarInput(['#ReclamoId', '#ReclamoNombre', '#ReclamoDescripcion'], ['']);
                GenerarSuccessAlerta(data.Mensaje, 'success');
                goAlert();
            }
            else {
                GenerarErrorAlerta(data.Mensaje, "error");
                goAlert();
            }
        });

        posting.fail(function (data, status, xhr) {
            $('#modalReclamo').modal('hide');
            LimpiarInput(['#ReclamoId', '#ReclamoNombre', '#ReclamoDescripcion'], ['']);
            GenerarErrorAlerta(status, "error");
            goAlert();
        })

        posting.always(function () { });
    }
}

function btnAgregarReclamoEvent(e) {
    e.stopPropagation();
    $('#agregarReclamoContainer').removeClass('hidden');
    $('#ReclamoModalTitle').text('Creando Un Nuevo Registro - Reclamos')
    $('#agregarReclamoContainer').html('');    
    $.ajax({
        type: 'GET',
        url: urljs + 'ConfiguracionesMotor/CreateReclamo/',
        success: function (htmlData) {
            $('#agregarReclamoContainer').html(htmlData);
        }
    });
    $('#editarReclamoContainer').addClass('hidden');
    $('#eliminarReclamoContainer').addClass('hidden');
    $('#modalReclamo').modal('show');
}

function CargarReclamos() {
    LimpiarTabla('#reclamosTable');
    var path = urljs + '/ConfiguracionesMotor/ObtenerReclamos/';
    var posting = $.post(path, { null: 'null' });
    posting.done(function (data) {
        if (data[0].Accion) {
            for (var i = 0; i < data.length; i++) {
                AgregarFilasATablaDeReclamos(data[i], '#reclamosTable');
            }
        }
        else {
            GenerarErrorAlerta(data[0].Mensaje, 'error');
            goAlert();
        }
    });

    posting.fail(function (data, status, xhr) {
        GenerarErrorAlerta('Error cargando los datos - ,' + status, 'error');
        goAlert();
    });

    posting.always(function (data, status, xhr) {

    });
}

function AgregarFilasATablaDeReclamos(ArrayData, table) {
    var newRow = $(table).dataTable().fnAddData([
        ArrayData['ReclamoId'],
        ArrayData['ReclamoNombre'],
        "<button data-reclamo-id=" + ArrayData['ReclamoId'] + " id='btnEditar" + ArrayData['ReclamoId'] + "' onClick='EditarReclamo(event)' title='Editar catálogo' data-toggle='tooltip' class='btn btn-primary botonVED text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
        "<button data-reclamo-id=" + ArrayData['ReclamoId'] + " id='btnEliminiar" + ArrayData['ReclamoId'] + "' onClick='ConfirmarEliminacionReclamo(event)' title='Eliminar catálogo' data-toggle='tooltip' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>"
    ]);

    var theNode = $(table).dataTable().dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['ReclamoId']);
    $('td', theNode)[2].setAttribute('class', 'text-center');
}

function EditarReclamo(e) {
    e.stopPropagation();
    var id = $(e.currentTarget).attr('data-reclamo-id');
    $('#ReclamoModalTitle').text('Editando Registro - Reclamo con Id: ' + id);
    $('#agregarReclamoContainer').addClass('hidden');
    $('#editarReclamoContainer').removeClass('hidden');
    $('#editarReclamoContainer').html('');    
    $.ajax({
        type: 'GET',
        url: urljs + 'ConfiguracionesMotor/EditReclamo/',
        data: { id: id },
        success: function (htmlData) {
            $('#editarReclamoContainer').html(htmlData);
        }
    });
    $('#eliminarReclamoContainer').addClass('hidden');
    $('#modalReclamo').modal('show');
}

function ConfirmarEliminacionReclamo(e) {
    e.stopPropagation();
    var id = $(e.currentTarget).attr("data-reclamo-id");
    $('#ReclamoModalTitle').text('Eliminando Registro - Reclamo con Id: ' + id);
    $("#hiddenReclamoId").val(id);
    $('#agregarReclamoContainer').addClass('hidden');
    $('#editarReclamoContainer').addClass('hidden');
    $('#eliminarReclamoContainer').removeClass('hidden');
    $("#modalReclamo").modal('show');
}

function EliminarReclamo(e) {
    var reclamoId = $("#hiddenReclamoId").val();
    $.ajax({
        type: "POST",
        url: urljs + "/ConfiguracionesMotor/EliminarReclamo/",
        data: { id: reclamoId },
        success: function (result) {
            $("#modalReclamo").modal('hide');
            if (result) {
                var TableData = $('#reclamosTable').DataTable();
                TableData.row('#' + reclamoId).remove().draw();
            }
        }
    });
}
//Reclamos Region

//Items de Reclamos Region
function CargarItemsDeReclamos(reclamoId) {
    LimpiarTabla('#itemsTable');
    var path = urljs + '/ConfiguracionesMotor/ObtenerItemsDeReclamo/';
    var posting = $.post(path, { id: reclamoId });
    posting.done(function (data) {
        if (data[0].Accion) {
            LimpiarTabla('#itemsTable');
            for (var i = 0; i < data.length; i++) {
                AgregarFilaAItemsTable(data[i], '#itemsTable');
            }
            $('#btnAgregarItem').removeClass('hidden');
        }
        else {
            $('#btnAgregarItem').removeClass('hidden');
            LimpiarTabla('#itemsTable');
            GenerarErrorAlerta(data[0].Mensaje, "error");
            goAlert();
        }
    });

    posting.fail(function (data, status, xhr) {
        GenerarErrorAlerta("Error cargando la data - , " + status, "error");
        goAlert();
    });

    posting.always(function (data, status, xhr) { });
}

function AgregarFilaAItemsTable(ArrayData, table) {
    var newRow = $(table).dataTable().fnAddData([
        ArrayData['ReclamoId'],
        ArrayData['ItemDeReclamoId'],
        ArrayData['ItemDeReclamoDescripcion'],
        "<button data-item-id=" + ArrayData['ItemDeReclamoId'] + " id='btnEditarItem" + ArrayData['ItemDeReclamoId'] + "' onClick='EditarItem(event)' title='Editar elemento' data-toggle='tooltip' class='btn btn-sm btn-primary botonVED text-center'><i class='icon-edit icon-white fa fa-pencil-square-o'></i></button>" +
        "<button data-item-id=" + ArrayData['ItemDeReclamoId'] + " id='btnEliminiarItem" + ArrayData['ItemDeReclamoId'] + "' onClick='ConfirmarEliminacionItem(event)' title='Eliminar elemento' data-toggle='tooltip' class='btn btn-sm btn-danger botonVED text-center'><i class='icon-edit icon-white fa fa-trash-o'></i></button>"
    ]);

    var theNode = $(table).dataTable().dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['ItemDeReclamoId']);
    $('td', theNode)[0].setAttribute('class', 'hidden');
    $('td', theNode)[3].setAttribute('class', 'text-center');
}

function GuardarItem(e) {
    e.stopPropagation();
    var path = urljs + '/ConfiguracionesMotor/GuardarItemsDeReclamo/';
    var form = $('#CreateEditItemForm');
    form.validate();

    if (form.valid()) {
        var posting = $.post(path, form.serialize());
        posting.done(function (data) {
            if (data.Accion) {
                CargarItemsDeReclamos(ReclamoIdSeleccionado);
                $('#modalItem').modal('hide');                
                form.find('input:text').val('');
                CleanItemContainers();
                GenerarSuccessAlerta(data.Mensaje, 'success');
                goAlert();
            }
            else {
                GenerarErrorAlerta(data.Mensaje, "error");
                goAlert();
            }
        });

        posting.fail(function (data, status, xhr) {
            $('#modalItem').modal('hide');
            form.find('input:text').val('');
            GenerarErrorAlerta(status, "error");
            goAlert();
        })

        posting.always(function () { });
    }
}

function EditarItem(e) {
    e.stopPropagation();
    var id = $(e.currentTarget).attr('data-item-id');
    $('#ItemModalTitle').text('Editando Registro - Item de Reclamo con Id: ' + id);
    $('#agregarItemContainer').addClass('hidden');
    $('#eliminarItemContainer').addClass('hidden');
    $('#editarItemContainer').removeClass('hidden');
    $('#editarItemContainer').html('');
    $.ajax({
        type: "GET",
        url: "/ConfiguracionesMotor/EditarItem/",
        data: { reclamoId: ReclamoIdSeleccionado, itemId: id },
        success: function (htmldata) {
            $('#editarItemContainer').html(htmldata);
        }
    });
    $('#modalItem').modal('show');
}

function ConfirmarEliminacionItem(e) {
    e.stopPropagation();
    var id = $(e.currentTarget).attr("data-item-id");
    $("#hiddenItemId").val(id);
    $('#ItemModalTitle').text('Eliminando Registro - Item de Reclamo con Id: ' + id);
    $('#agregarItemContainer').addClass('hidden');
    $('#editarItemContainer').addClass('hidden');
    $('#eliminarItemContainer').removeClass('hidden');
    $("#modalItem").modal('show');
}

function EliminarItem(e) {
    var Id = $('#hiddenItemId').val();    
    $.ajax({
        type: "POST",
        url: "/ConfiguracionesMotor/EliminarItem/",
        data: { reclamoId: ReclamoIdSeleccionado, itemId: Id },
        success: function (result) {
            $('#modalItem').modal('hide');
            if (result) {
                var TableData = $('#itemsTable').DataTable();
                TableData.row('#' + Id).remove().draw();
                GenerarSuccessAlerta("Se elimino correctamente el registro!", "success");
                goAlert();
            }
            else {
                $('#modalItem').modal('hide');
                GenerarErrorAlerta("No se pudo eliminar el registro!", "error");
                goAlert();
            }
        }
    });
}

function AgregarItem(e) {
    e.stopPropagation();
    if (ReclamoIdSeleccionado !== '') {
        $('#ItemModalTitle').text('Creando Nuevo Registro - Item de Reclamos');
        $('#agregarItemContainer').removeClass('hidden');
        $('#agregarItemContainer').html('');
        //$('#agregarItemContainer').load('/ConfiguracionesMotor/CrearItem/' + ReclamoIdSeleccionado);
        $.ajax({
            type: "GET",
            url: urljs + "/ConfiguracionesMotor/CrearItem/",
            data: { reclamoId: ReclamoIdSeleccionado },
            success: function (htmldata) {
                $('#agregarItemContainer').html(htmldata);
            }
        });
        $('#editarItemContainer').addClass('hidden');
        $('#eliminarItemContainer').addClass('hidden');
        $('#modalItem').modal('show');
    }
    else {
        bootbox.alert('Debe seleccionar un Reclamo Valido!');
    }
}

function CleanItemContainers() {
    $('#agregarItemContainer').html('');
    $('#editarItemContainer').html('');
}
//Items de Reclamos Rgion

//Variables de Items Region
function CargarVariablesDeItem(ReclamoId, ItemId) {
    LimpiarTabla('#variablesEnItemTable');
    var path = urljs + '/ConfiguracionesMotor/ObtenerVariablesDeItem/';
    var posting = $.post(path, { reclamoId: ReclamoId, itemId: ItemId });
    posting.done(function (data) {
        if (data[0].Accion) {
            LimpiarTabla('#variablesEnItemTable');
            for (var i = 0; i < data.length; i++) {
                AgregarFilasVariablesTable(data[i], '#variablesEnItemTable');
            }
            $('#btnAgreagrVariableEvaluar').removeClass('hidden');
        }
        else {
            $('#btnAgreagrVariableEvaluar').removeClass('hidden');
            LimpiarTabla('#variablesEnItemTable');
            GenerarErrorAlerta(data[0].Mensaje, "error");
            goAlert();
        }
    });

    posting.fail(function (data, status, xhr) {
        GenerarErrorAlerta("Error cargando la data - , " + status, "error");
        goAlert();
    });

    posting.always(function (data, status, xhr) { });
}

function AgregarFilasVariablesTable(ArrayData, table) {
    var newRow = $(table).dataTable().fnAddData([
        ArrayData['VariableDeItemId'],
        ArrayData['ReclamoId'],
        ArrayData['ItemDeReclamoId'],
        ArrayData['VariableCodigo'],
        ArrayData['VariableNombre'],
        ArrayData['CondicionLogica'],
        ArrayData['ValorAEvaluar'],
        "<button data-var-id=" + ArrayData['VariableDeItemId'] + " id='btnEditarVar" + ArrayData['VariableDeItemId'] + "' onClick='EditarVariable(event)' title='Editar elemento' data-toggle='tooltip' class='btn btn-sm btn-primary botonVED text-center'><i class='icon-edit icon-white fa fa-pencil-square-o'></i></button>" +
        "<button data-var-id=" + ArrayData['VariableDeItemId'] + " id='btnEliminiarVar" + ArrayData['VariableDeItemId'] + "' onClick='ConfirmarEliminacionVariable(event)' title='Eliminar elemento' data-toggle='tooltip' class='btn btn-sm btn-danger botonVED text-center'><i class='icon-edit icon-white fa fa-trash-o'></i></button>"
    ]);

    var theNode = $(table).dataTable().dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['VariableDeItemId']);
    $('td', theNode)[0].setAttribute('class', 'hidden');
    $('td', theNode)[1].setAttribute('class', 'hidden');
    $('td', theNode)[2].setAttribute('class', 'hidden');
    $('td', theNode)[3].setAttribute('class', 'hidden');
    $('td', theNode)[4].setAttribute('class', 'text-center');
    $('td', theNode)[5].setAttribute('class', 'text-center');
    $('td', theNode)[6].setAttribute('class', 'text-center');
    $('td', theNode)[7].setAttribute('class', 'text-center');
}

function AgregarVariable(e) {
    e.stopPropagation();
    if (ReclamoIdSeleccionado !== '' && ItemIdSeleccionado!=='') {
        $('#VariableModalTitle').text('Creando Nuevo Registro - Variable de Item');
        $('#agregarVariableContainer').removeClass('hidden');
        $('#agregarVariableContainer').html('');        
        $.ajax({
            type: "GET",
            url: "/ConfiguracionesMotor/CrearVariable/",
            data: { reclamoId: ReclamoIdSeleccionado, itemId: ItemIdSeleccionado },
            success: function (htmldata) {
                $('#agregarVariableContainer').html(htmldata);
            },
            error: function (data, status, xhr) {
                GenerarErrorAlerta(status, "error");
                goAlert();
            }
        });
        $('#editarVariableContainer').addClass('hidden');
        $('#eliminarVariableContainer').addClass('hidden');
        $('#modalVariables').modal('show');
    }
    else {
        bootbox.alert('Debe seleccionar un Item y Reclamo Valido!');
    }
}

function EditarVariable(e) {
    e.stopPropagation();
    var id = $(e.currentTarget).attr('data-var-id');
    $('#VariableModalTitle').text('Editando Registro - Variable de Item con Codigo: ' + id);
    $('#agregarVariableContainer').addClass('hidden');
    $('#eliminarVariableContainer').addClass('hidden');
    $('#editarVariableContainer').removeClass('hidden');
    $('#editarVariableContainer').html('');
    $.ajax({
        type: "GET",
        url: urljs + "/ConfiguracionesMotor/EditarVariable/",
        data: { reclamoId: ReclamoIdSeleccionado, itemId: ItemIdSeleccionado, variableId: id },
        success: function (htmldata) {
            $('#editarVariableContainer').html(htmldata);
        }
    });
    $('#modalVariables').modal('show');
}

function GuardarVariable(e) {
    e.stopPropagation();
    var path = '/ConfiguracionesMotor/GuardarVariablesDeItem/';
    var form = $('#CreateEditVariableForm');
    form.validate();

    if (form.valid()) {
        var posting = $.post(path, form.serialize());
        posting.done(function (data) {
            if (data.Accion) {
                CargarVariablesDeItem(ReclamoIdSeleccionado, ItemIdSeleccionado);
                $('#modalVariables').modal('hide');
                form[0].reset();
                CleanVariablesContainers();
                GenerarSuccessAlerta(data.Mensaje, 'success');
                goAlert();
            }
            else {
                $('#modalVariables').modal('hide');
                form.find('input:text').val('');
                CleanVariablesContainers();
                GenerarErrorAlerta(data.Mensaje, "error");
                goAlert();
            }
        });

        posting.fail(function (data, status, xhr) {
            $('#modalVariables').modal('hide');
            form.find('input:text').val('');
            CleanVariablesContainers();
            GenerarErrorAlerta(status, "error");
            goAlert();
        })

        posting.always(function () { });
    }
}

function ConfirmarEliminacionVariable(e) {
    e.stopPropagation();
    var id = $(e.currentTarget).attr("data-var-id");
    $("#hiddenVariableId").val(id);
    $('#VariableModalTitle').text('Eliminando Registro - Item de Reclamo con Id: ' + id);
    $('#agregarVariableContainer').addClass('hidden');
    $('#editarVariableContainer').addClass('hidden');
    $('#eliminarVariableContainer').removeClass('hidden');
    $("#modalVariables").modal('show');
}

function EliminarVariable(e) {
    var Id = $('#hiddenVariableId').val();
    $.ajax({
        type: "POST",
        url: urljs + "/ConfiguracionesMotor/EliminarVariableDeItem/",
        data: { id: Id },
        success: function (result) {
            $('#modalVariables').modal('hide');
            if (result) {
                var TableData = $('#variablesEnItemTable').DataTable();
                TableData.row('#' + Id).remove().draw();
                GenerarSuccessAlerta("Se elimino correctamente el registro!", "success");
                goAlert();
            }
            else {
                $('#modalVariables').modal('hide');
                GenerarErrorAlerta("No se pudo eliminar el registro!", "error");
                goAlert();
            }
        }
    });
}

function CleanVariablesContainers() {
    $('#agregarVariableContainer').html('');
    $('#editarVariableContainer').html('');
}

function VariableCodigoOnChange(e) {
    e.stopPropagation();
    if ($('#VariableCodigo').val() !== "") {
        $('#VariableNombre').val($('#VariableCodigo :selected').text());
    }
}
//Variables de Items Region