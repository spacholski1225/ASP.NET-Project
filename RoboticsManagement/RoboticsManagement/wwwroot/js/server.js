const sql = require("mssql");

const dbConfig = {

    server: 'DESKTOP-2VH9HB6',
    database: 'MgmtDB'
};

function getEmp() {
    var conn = new sql.ConnectionPool(dbConfig);
    var req = new sql.Request(conn);

    conn.connect(function (err) {
        if (err) {
            console.log(err);
            return;
        }
        req.query("select * from EmployeeNotifications", function(err, recordset) {
            if (err) {
                console.log(err);
                return;
            }
            else {
                console.log(recordset);
            }
            conn.close();
        });
    });

}

getEmp();
