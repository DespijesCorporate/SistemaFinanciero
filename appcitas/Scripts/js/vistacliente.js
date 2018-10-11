function cargar_listado_programadas(id) {
    var path = urljs + "atencioncitas/GetCitasProgramadasLlamadas";
    var posting = $.post(path, { sucursalid: Number(id) });
    posting.done(function (data, status, xhr) {
        $('#div_programadas').empty();
        for (var i = 0; i < data.length; i++) {
            if (data[i].Accion != 0) {
                $('#div_programadas').append('<div class="row"><h3>' + data[i].CitaTicket + '</h3><h3>' + data[i].posicionDescripcion + '</h3></div>');
            }
        }
    });
}

function cargar_listado_walkin(id) {
    var path = urljs + "atencioncitas/GetCitasWalkinLlamadas";
    var posting = $.post(path, { sucursalid: Number(id) });
    posting.done(function (data, status, xhr) {
        $('#div_walkin').empty();
        for (var i = 0; i < data.length; i++) {
            if (data[i].Accion != 0) {
                $('#div_walkin').append('<div class="row"><h3>' + data[i].CitaTicket + '</h3><h3>' + data[i].posicionDescripcion + '</h3></div>');
            }
        }
    });
}

function GetSucursalName(id) {
    var path = urljs + "atencioncitas/GetSucursalName";
    var posting = $.post(path, { sucursalid: Number(id) });
    posting.done(function (data, status, xhr) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].Accion != 0) {
                $('#SucursalName').val(data[i].sucursal);
            }
        }
    });
}




$(document).ready(function () {
    jQuery.ajaxSetup({ async: false, global: false });
    checkUserAccess('DPCF01');
    CheckUserInfo2();
    GetSucursalName(usu_sucursalId);
   // $('#SucursalName').val(usu_sucursalId);
    $.ajax({
        url: '../Content/Publicidad/Config_publi.html',
        type: 'get',
        dataType: 'html'
    })
        .done(function (data) {
            if ($.trim(data) == 'video') {
                var folder = "../Content/publicidad/video";
                $.ajax({
                    url: folder,
                    async: true,
                    success: function (data) {
                        jQuery.ajaxSetup({ async: false });
                        $('#div_publicidad').empty().append('<video muted controls id="video_publi" autoplay loop>');
                        $(data).find("a").attr("href", function (i, val) {
                            if (val.match(/\.(mp4)$/)) {
                                $("#video_publi").append("<source src='" + val + "' type='video/mp4'>");
                            }
                        });
                        $('#div_publicidad').append('</video>');
                        jQuery.ajaxSetup({ async: true });
                    }
                });
            }
            if ($.trim(data) == 'imagen') {
                var folder = "../Content/publicidad/imgs";
                jQuery.ajaxSetup({ async: false });
                $.ajax({
                    url: folder,
                    success: function (data) {
                        $(data).find("a").attr("href", function (i, val) {
                            if (val.match(/\.(jpe?g|png|gif)$/)) {
                                $(".div_publicidad").append("<li><img src='" + val + "' height='100%'></li>");
                            }
                        });
                    }
                });

                $('.div_publicidad').innerfade({
                    speed: 'slow',
                    timeout: 10000,
                    type: 'sequence',
                    containerheight: '100%',
                    animation: 'slide'
                });
                jQuery.ajaxSetup({ async: true });

            }

        });
    $.ajax({
        url: '../Content/Publicidad/Publicidad.html',
        type: 'get',
        dataType: 'html'
    })
        .done(function (data) {
            $('.footer').empty().append(data);
            $('.footer').marquee({
                //speed in milliseconds of the marquee
                duration: 60000,
                //gap in pixels between the tickers
                gap: 50,
                //time in milliseconds before the marquee will start animating
                delayBeforeStart: 0,
                //'left' or 'right'
                direction: 'left',
                //true or false - should the marquee be duplicated to show an effect of continues flow
                duplicated: true
            });
        });

    $('#btn_ticketp').on('click', function () {
        document.getElementById('video_publi').pause();
        setTimeout(function () {
            $('#modal_llamado').modal('hide');
            document.getElementById('video_publi').play();
        }, 5000);
        new Audio("../Content/Publicidad/beep-p.mp3").play();

    });
    $('#btn_ticketwk').on('click', function () {
        document.getElementById('video_publi').pause();
        setTimeout(function () {
            $('#modal_llamado_wk').modal('hide');
            document.getElementById('video_publi').play();
        }, 5000);
        new Audio("../Content/Publicidad/beep-wk.mp3").play();
    });
    function cargar_citas_llamadas_ticket(Id) {
        var path = urljs + "atencioncitas/GetCitasLlamadoTicket";
        var posting = $.post(path, { Id: Number(Id) }, "json");
        posting.done(function (data, status, xhr) {
            if (data.length > 0) {
                var a = 1;
                $.each(data, function (i, item) {
                    if (item.Accion != 0) {
                        if (data[i].CitaTipo == 1) //0--->Programada, 1 ----> WalkIN
                        {
                            jQuery.ajaxSetup({ async: false });
                            setTimeout(function () {
                                $('#ticket_llamadowk').empty().text(item.CitaTicket);
                                $('#lugar_llamadowk').empty().text(item.posicionDescripcion);
                                document.getElementById('video_publi').pause();
                                $('#modal_llamado_wk').modal('show');
                                new Audio("../Content/Publicidad/beep-wk.mp3").play();
                            }, 5000 * a);
                            setTimeout(function () {
                                $('#modal_llamado_wk').modal('hide');
                                $('#modal_llamado_wk').on('hidden.bs.modal', function (e) {
                                    document.getElementById('video_publi').play();
                                });
                                console.log(item.posicionDescripcion);
                            }, 9000 * a);
                            jQuery.ajaxSetup({ async: true });
                        }
                        else {
                            jQuery.ajaxSetup({ async: false });
                            setTimeout(function () {
                                $('#ticket_llamado').empty().text(item.CitaTicket);
                                $('#lugar_llamado').empty().text(item.posicionDescripcion);
                                document.getElementById('video_publi').pause();
                                $('#modal_llamado').modal('show');
                                new Audio("../Content/Publicidad/beep-p.mp3").play();
                            }, 5000 * a);
                            setTimeout(function () {
                                $('#modal_llamado').modal('hide');
                                $('#modal_llamado').on('hidden.bs.modal', function (e) {
                                    document.getElementById('video_publi').play();
                                });
                                console.log(item.posicionDescripcion);
                            }, 9000 * a);
                            jQuery.ajaxSetup({ async: true });
                        }
                        jQuery.ajaxSetup({ async: true });
                    }
                    a++;
                });
            }
        });
    }
    jQuery.ajaxSetup({ async: false, global: false });
    function actualizar_datos()
    {
        jQuery.ajaxSetup({ async: false, global: false });
    
        cargar_listado_walkin(usu_sucursalId);
        cargar_listado_programadas(usu_sucursalId);
        cargar_citas_llamadas_ticket(usu_sucursalId);
        jQuery.ajaxSetup({ async: true, global: true });
    }
    cargar_listado_walkin(usu_sucursalId);
    cargar_listado_programadas(usu_sucursalId);
    //cargar_citas_llamadas_ticket(usu_sucursalId);
    setInterval(actualizar_datos, 20 * 1000);
    jQuery.ajaxSetup({ async: true, global: true });

});
