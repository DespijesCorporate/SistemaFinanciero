$(document).ready(function () {

    //Verificacion de Privilegios de Autorizacion de Usuarios
    jQuery.ajaxSetup({ async: false });
    checkUserAccess('CONF080');
    jQuery.ajaxSetup({ async: true });
    $('.numero').mask('####');
    $('.varchar5').mask('AAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.+: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar10').mask('AAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.+: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.+: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar100').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.+: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar200').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,+.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });

    CargarVariables();

    $("#variablesTable").on("click", "tbody tr", function (event) {
        event.stopPropagation();
        event.preventDefault();
        event.stopImmediatePropagation();
        var id = $(this).attr('id');
        $('#variablesTable').find('tr').each(function (index, el) {
            $(el).removeClass('selected');
        });
        $(this).addClass('selected');
        if (id !== "") {
            $("#viewPlaceHolder").load(urljs + "/Variables/ViewVariableData/" + id);
        }
    });

    $("#btnAgregarVariable").click(function () {
        $("#viewPlaceHolder").html("");
        $("#viewPlaceHolder").load(urljs + "Variables/NewVariable/");
    });

    $.validator.unobtrusive.parse($("#myForm"));

})

function OrigenCbxChange(e) {
    if ($('#OrigenId :selected').val() === "SRC1") {
        $('#VariableValor').attr('readonly', 'readonly');        
        $('#VariableFormula').val('');
        $('#TipoId').val('');
        $('#TipoId').attr('disabled', 'disabled');
    }
    else {
        $('#TipoId').removeAttr('disabled');
    }
}

function TipoDeVariableChange(e) {
    if ($('#TipoId :selected').text() === "Formula") {
        $('#VariableValor').attr('readonly', 'readonly');
    }
    else if ($('#TipoId :selected').text() === "Constante") {
        $('#VariableFormula').val('');
        $('#VariableValor').removeAttr('readonly');
    }
    else {
        $('#VariableFormula').val('');
        $('#VariableValor').attr('readonly', 'readonly');
    }
}

var variableId = "";

function CargarVariables() {
    LimpiarTabla('#variablesTable');
    var path = urljs + '/Variables/ObtenerVariables/';
    var posting = $.post(path, { null: 'null' });
    posting.done(function (data) {
        if (data[0].Accion) {
            for (var i = 0; i < data.length; i++) {
                AgregarFilasVariables(data[i], '#variablesTable');
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

function AgregarFilasVariables(ArrayData, table) {
    var newRow = $(table).dataTable().fnAddData([
        ArrayData['VariableCodigo'],
        ArrayData['VariableNombre'],
        "<button data-variable-id=" + ArrayData['VariableCodigo'] + " id='btnEditar" + ArrayData['VariableCodigo'] + "' onClick='EditarVariable(event)' title='Editar catálogo' data-toggle='tooltip' class='btn btn-primary botonVED text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
        "<button data-variable-id=" + ArrayData['VariableCodigo'] + " id='btnEliminiar" + ArrayData['VariableCodigo'] + "' onClick='ConfirmarEliminacion(event)' title='Eliminar catálogo' data-toggle='tooltip' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>"
    ]);

    var theNode = $(table).dataTable().dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['VariableCodigo']);
    $('td', theNode)[2].setAttribute('class', 'text-center');
}

function CheckValidation() {
    $('#VariableCodigo').validate();
    $('#VariableNombre').validate();
    $('#OrigenId').validate();
    $('#DatoDeRetornoId').validate();
    $('VariableDescripcion').validate();
    if ($('#VariableCodigo').valid()
        && $('#VariableNombre').valid()
        && $('#OrigenId').valid()
        && $('#DatoDeRetornoId').valid()
        && $('#VariableDescripcion').valid()) {
        return true;
    }
    else {
        $('#myForm').validate();
        return $('#myForm').valid();
    }
}

function EnviarPost(e) {
    e.stopPropagation();
    var path = urljs + '/Variables/GuardarVariable/';

    CheckValidation();

    var form = $('#myForm');

    if (CheckValidation()) {
        var posting = $.post(path, form.serialize());
        posting.done(function (data) {
            if (data.Accion) {
                CargarVariables();                
                GenerarSuccessAlerta(data.Mensaje, 'success');
                goAlert();
                $("#viewPlaceHolder").html("");
                $("#viewPlaceHolder").load(urljs + "/Variables/ViewVariableData/" + data.VariableCodigo);
            }
            else {
                GenerarErrorAlerta(data.Mensaje, 'error');
                goAlert();
            }
        });

        posting.fail(function (data, status, xhr) {
            form.trigger('reset');
            GenerarErrorAlerta(status, 'error');
            goAlert();
        });

        posting.always(function () { });
    }
}

function EditarVariable(e) {
    e.stopPropagation();
    var id = $(e.currentTarget).attr('data-variable-id');
    variableId = id;
    $("#viewPlaceHolder").html("");
    $("#viewPlaceHolder").load(urljs + "/Variables/EditarVariable/" + id);
}

function CancelAction(e) {
    $("#viewPlaceHolder").html("");
    $("#viewPlaceHolder").load(urljs + "/Variables/ViewVariableData/" + variableId);
}

function ConfirmarEliminacion(e) {
    e.stopPropagation();
    var id = $(e.currentTarget).attr("data-variable-id");
    $("#hiddenVariableCodigo").val(id);
    $("#modalEliminacionVariable").modal('show');
}

function EliminarVariable() {
    var varId = $("#hiddenVariableCodigo").val();

    $.ajax({
        type: "POST",
        url: urljs + "/Variables/EliminarVariable/",
        data: { id: varId },
        success: function (result) {
            $("#modalEliminacionVariable").modal('hide');
            if (result) {
                var TableData = $('#variablesTable').DataTable();
                TableData.row('#' + varId).remove().draw();
            }
        }
    });
}

function AgregarVariableFormula(e) {
    var formula = $('#VariableFormula').val() + "[" + $('#VariablesCbx :selected').text() + "]";
    $('#VariableFormula').val(formula);
}

function AgregarFuncionFormula(e) {
    var formula = $('#VariableFormula').val() + "[$" + $('#FuncionesCbx :selected').text() + "]";
    $('#VariableFormula').val(formula);
}