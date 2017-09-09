//var sql = require('mssql');
//var http = require('http');
//var express = require('express');

//var app = express();



//app.set('port', (process.env.PORT || 5000));
//app.use(express.static(__dirname + '/public'));
//app.engine('html', require('ejs').renderFile);
//app.set('view engine', 'html');
//app.set('views', 'C:/Users/BerkaySarı/Desktop/BlogSitesi2/BlogSitesi2/Views/Home/');

//app.get('/', function (req, res) {
//    //C:\Users\BerkaySarı\Desktop\BlogSitesi2\BlogSitesi2\Views\Home\Index.cshtml
//    app.render('Index.cshtml');
//    app.engine('.html', require('ejs').renderFile());
//    //render yaptıgı için aşağıdaki kodu okumaz
//    res.send(
//        sql.connect("mssql://sa:q1w2e3r4@LENOVO-PC\\SQL_2014/Blog_Sitesi").then(function () {
            
            
//            new sql.Request().query('UPDATE PageView SET PageCount = PageCount + 1 WHERE Id = 1').then(function (recordset) {
        
//            }).catch(function (err) {
//                console.log("err" + err);
//            });
    
//        })
//    );
//});

//app.get('C:\\Users\\%username%\\Desktop\\BlogSitesi2\\BlogSitesi2\\Views\\Account\\Index.cshtml', function (req, res) {
    
//    res.send(
//        sql.connect("mssql://sa:q1w2e3r4@LENOVO-PC\\SQL_2014/Blog_Sitesi").then(function () {
            
            
//            new sql.Request().query('UPDATE PageView SET PageCount = PageCount + 1 WHERE Id = 1').then(function (recordset) {
        
//            }).catch(function (err) {
//                console.log("err" + err);
//            });
//        })
//    );
//});


//http.createServer(app).listen(6108, "localhost", function () {
//    //sql.connect("mssql://sa:q1w2e3r4@LENOVO-PC\\SQL_2014/Blog_Sitesi").then(function () {
        
        
//    //    new sql.Request().query('UPDATE PageView SET PageCount = PageCount + 1 WHERE Id = 1').then(function (recordset) {
        
//    //    }).catch(function (err) {
//    //        console.log("err" + err);
//    //    });
    
//    //}).catch(function (err) {
//    //    console.log("Bağlantı başarısız.");
//    //});

//});

