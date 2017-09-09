//$(".classIcerik").click(function () {
//    var icerik = $(this).attr("id");

//    FiltreVeriler(icerik);
//});

//function FiltreVeriler(icerik) {
//    $.ajax({
//        url: '/Home/Index',
//        data: { veri: icerik },
//        success: function () {
//            alert('asdfsgdh');
//        }
//    })
//};


//$(".classIcerik").click(function () {
//$.post("HomeController/Index", { veri: $(".classIcerik").val() }, function (data) {
  

//});
//});

//$(".btnSil").click(function () {
//    var sayi = $(this).attr("id");
//    Veriler(sayi);
//});

//function Veriler(id) {
//    $.ajax({
//        url: '/Home/Icerik',
//        data: { veri: id },
//        success: function () {
//            alert('Added');
//        }
//    })
//};

$(".Göster").click(function () {
    var _veri = $(this).attr("id");
    Veriler(_veri);
    Verilerpost(_veri);
});

function Veriler(veri) {
    $.ajax({
        url: '/Home/Icerik',
        type: "post",
        data: { veri: veri },
        success: function () {
            alert('Added');
        }
    })
};

function Verilerpost(veri) {
    $.post('/Home/Icerik', { veri: veri }, function (resp) {
        console.log(resp);
    });
}

$(".Goster").click(function() {
    $.post("/Home/Icerik", { id:id });
});


$(".myButton").click(function () {
    var _veri = $(.tbTitle).attr("Value");
    var veri = $(#taContents).attr("Value");

    var data = {
        model: {
            Title : _veri,
            Contents : veri
        }
    };
    Veriler(data);
});

function Veriler(data) {
    $.ajax({
        url: '/Account/ManageEdit/',
        type: "post",
        data: { model:data }
        
    })
};