$(document).ready(function () {

    //Verificacion de Privilegios de Autorizacion de Usuarios
    jQuery.ajaxSetup({ async: false });
    checkUserAccess('CONF090');
    jQuery.ajaxSetup({ async: true });

    //Mascaras para formatos
    $('.numero').mask('####');
    $('.varchar5').mask('AAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar10').mask('AAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar100').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar200').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });

    CargarFunciones();

    $("#funcionesTable").on("click", "tbody tr", function (event) {
        event.stopPropagation();
        event.preventDefault();
        event.stopImmediatePropagation();
        var id = $(this).attr('id');
        $('#funcionesTable').find('tr').each(function (index, el) {
            $(el).removeClass('selected');
        });
        $(this).addClass('selected');
        if (id !== "") {
            $.ajax({
                type: 'GET',
                url: urljs + 'Funciones/VerDataDeFuncion/',
                data: { id: id },
                success: function (result) {
                    $('#viewPlaceHolder').html(result);
                }
            });
        }
    });

    $("#btnAgregarFuncion").click(function (event) {
        event.preventDefault();
        $("#viewPlaceHolder").html("");
        $.ajax({
            type: 'GET',
            url: urljs + 'Funciones/NuevaFuncion',
            data: {null: null},
            success: function (result) {
                $('#viewPlaceHolder').html(result);
            }
        });
    });

    $.validator.unobtrusive.parse($("#myForm"));
});

function CargarFunciones() {
    LimpiarTabla('#funcionesTable');
    var path = urljs + "Funciones/ObtenerFunciones/";
    var posting = $.post(path, { null: 'null' });
    posting.done(function (data) {
        if (data[0].Accion) {
            for (var i = data.length - 1; i >= 0; i--) {
                AgregarFilasFunciones(data[i], '#funcionesTable');
            }
        }
        else {
            GenerarErrorAlerta(data[0].Mensaje, "error");
            goAlert();
        }
    });

    posting.fail(function (data, status, xhr) {
        GenerarErrorAlerta("Error cargando las funciones - , " + status, "error");
        goAlert();
    });

    posting.always(function (data, status, xhr) {
        /*quitamos la animacion*/
        //RemoveAnimation("body");
    });
}

function AgregarFilasFunciones(ArrayData, table) {
    var newRow = $(table).dataTable().fnAddData([
        ArrayData['FuncionCodigo'],
        ArrayData['FuncionNombre'],
        "<button data-id=" + ArrayData['FuncionCodigo'] + " id='btnEditar" + ArrayData['FuncionCodigo'] + "' onClick='EditarFuncion(event)' title='Editar catálogo' data-toggle='tooltip' class='btn btn-primary botonVED text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
        "<button data-id=" + ArrayData['FuncionCodigo'] + " id='btnEliminiar" + ArrayData['FuncionCodigo'] + "' onClick='ConfirmarEliminacion(event)' title='Eliminar catálogo' data-toggle='tooltip' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>"
    ]);

    var theNode = $(table).dataTable().dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['FuncionCodigo']);
    $('td', theNode)[2].setAttribute('class', 'text-center');
}

var funcionId = "";
function EditarFuncion(e) {
    e.stopPropagation();
    e.preventDefault();
    var id = $(e.currentTarget).attr('data-id');
    funcionId = id;
    $("#viewPlaceHolder").html("");
    if (id !== "")
    {
        $.ajax({
            type: 'GET',
            url: urljs + 'Funciones/EditarFuncion/',
            data: { id: id },
            success: function (result) {
                $('#viewPlaceHolder').html(result);
            }
        });
    }
}

function CancelAction(e) {
    e.preventDefault();
    $("#viewPlaceHolder").html("");
    $.ajax({
        type: 'POST',
        url: urljs + 'Funciones/VerDataDeFuncion/',
        data: { id: variableId },
        success: function (result) {
            $('#viewPlaceHolder').html(result);
        }
    });
}

function ConfirmarEliminacion(e) {
    e.stopPropagation();
    var id = $(e.currentTarget).attr("data-id");
    $("#hiddenFuncionCodigo").val(id);
    $("#modalEliminarFuncion").modal('show');
}

function AgregarParametroModal(e) {
    e.stopPropagation();
    $("#modalAgregarParametro").modal('show');
}

function EliminarFuncion() {
    var fnId = $("#hiddenFuncionCodigo").val();

    $.ajax({
        type: "POST",
        url: urljs + "Funciones/EliminarFuncion/",
        data: { id: fnId },
        success: function (result) {
            $("#modalEliminarFuncion").modal('hide');
            if (result) {
                var TableData = $('#funcionesTable').DataTable();
                TableData.row('#' + fnId).remove().draw();
            }
        }
    });
}

function ParametrosData(table) {

    var myArray = $("#parametrosTable").DataTable().rows().data().toArray();
    var resultArray = [];
    myArray.forEach(function (item, index) {
        resultArray.push(
            {
                ParametroId: index,
                ParametroName: item[2],
                TipoId: item[3],
                ConfigID: item[1],
                FuncionCodigo: $('#FuncionCodigo').val()
            }
        );
    });

    return resultArray;
}

function GuardarFuncion(e) {
    e.stopPropagation();
    var path = urljs + '/Funciones/GuardarFuncion/';
    var form = $('#myForm');
    var myParams = ParametrosData('#parametrosTable');
    var token = $('input[name="__RequestVerificationToken"]', form).val();

    var myData = {
        __RequestVerificationToken: token,
        FuncionCodigo: $('#FuncionCodigo').val(),
        FuncionNombre: $('#FuncionNombre').val(),
        FuncionNumeroParametros: $('#FuncionNumeroParametros').val(),
        FuncionTipoDeRetorno: $('#FuncionTipoDeRetorno').val(),
        ConfigId: 'DDR',
        FuncionDescripcion: $('#FuncionDescripcion').val(),
        Parametros: myParams
    };
    form.validate();
    if (form.valid()) {
        var posting = $.post(path, myData);
        posting.done(function (data) {
            if (data.Accion) {
                GenerarSuccessAlerta(data.Mensaje, 'success');
                goAlert();
                $("#viewPlaceHolder").html("");
                $.ajax({
                    type: 'POST',
                    url: urljs + 'Funciones/VerDataDeFuncion/',
                    data: { id: data.FuncionCodigo },
                    success: function (result) {
                        $('#viewPlaceHolder').html(result);
                        CargarFunciones();
                    }
                });
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

function ConvertTableToArrayObject(table) {
    var parametros = [];
    var data = $(table).DataTable().rows().data();

    for (var i = 0; i < data.length; i++) {
        parametros.push(data[i]);
    }

    return parametros;
}

function AgregarParametroATabla(e) {
    e.preventDefault();
    var data =
        {
            ParametroId: 0,
            ConfigID: "DDR",
            ParametroName: $("#ParametroName").val(),
            TipoId: $("#CbxTipos").val(),
            TipoAbreviatura: $("#CbxTipos :selected").text()
        };
    AddRowParametro(data, "#parametrosTable");
}

function AddRowParametro(ArrayData, table) {
    if (Validar(["#ParametroName", "#CbxTipos"], ["Este campo es requerido", "porfavor seleccione un tipo"])) {
        if (!Existe(ArrayData, table)) {
            var newRow = $(table).dataTable().fnAddData([
                ArrayData['ParametroId'],
                ArrayData['ConfigID'],
                ArrayData['ParametroName'],
                ArrayData['TipoId'],
                ArrayData['TipoAbreviatura'],
                "<button data-parametro-id=" + ArrayData['ParametroName'] + " id='btnEliminiar" + ArrayData['ParametroName'] + "' title='Eliminar catálogo' data-toggle='tooltip' onclick='ConfirmarEliminacionParametro(event)' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>"
            ]);

            var theNode = $(table).dataTable().dataTable().fnSettings().aoData[newRow[0]].nTr;
            theNode.setAttribute('id', ArrayData['ParametroName']);
            $('td', theNode)[5].setAttribute('class', 'text-center');
            $('td', theNode)[0].setAttribute('class', 'hidden');
            $('td', theNode)[1].setAttribute('class', 'hidden');
            $('td', theNode)[3].setAttribute('class', 'hidden');

            LimpiarParametrosInputs();
        }
        else {
            $("#modalAgregarParametro").modal('hide');

            LimpiarParametrosInputs();

            GenerarErrorAlerta("Este registro ya existe!", "error");
            goAlert();
        }
    }    
}

function LimpiarParametrosInputs() {
    $("#ParametroName").val("");
    $("#CbxTipos").val("");
}

function Existe(ArrayData, table) {
    var resultado = false;
    var myData = $(table).DataTable().rows().data();
    myData.each(function (value, index) {
        if ($(table).DataTable().rows('#'+ArrayData['ParametroName']).any()) {
            resultado = true;
        }
    });

    return resultado;
}

function ConfirmarEliminacionParametro(e) {
    e.stopPropagation();
    e.stopImmediatePropagation();
    e.preventDefault();
    var id = $(e.currentTarget).attr("data-parametro-id");
    $("#hiddenParametroId").val(id);
    $("#modalEliminarParametro").modal('show');
}

function EliminarParametro(e) {
    e.stopPropagation();
    var id = $("#hiddenParametroId").val();
    $("#parametrosTable").DataTable().rows("#"+id).remove().draw();
}