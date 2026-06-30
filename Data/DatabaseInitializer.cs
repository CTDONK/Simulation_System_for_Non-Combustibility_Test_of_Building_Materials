using Microsoft.Data.Sqlite;

namespace NonCombustibilityTestSimulator.Data
{
    public class DatabaseInitializer
    {
        private readonly string _dbPath;

        public DatabaseInitializer(string dbPath)
        {
            _dbPath = dbPath;
        }

        public void Initialize()
        {
            using var conn = new SqliteConnection($"Data Source={_dbPath}");
            conn.Open();

            // 创建 operators 表
            using var cmd1 = conn.CreateCommand();
            cmd1.CommandText = @"
                CREATE TABLE IF NOT EXISTS operators (
                    userid    TEXT NOT NULL,
                    username  TEXT NOT NULL,
                    pwd       TEXT NOT NULL,
                    usertype  TEXT NOT NULL
                )";
            cmd1.ExecuteNonQuery();

            // 插入默认管理员
            using var cmd2 = conn.CreateCommand();
            cmd2.CommandText = @"
                INSERT INTO operators (userid, username, pwd, usertype)
                SELECT '1', 'admin', '123456', 'admin'
                WHERE NOT EXISTS (SELECT 1 FROM operators WHERE username = 'admin')";
            cmd2.ExecuteNonQuery();

            using var cmd3 = conn.CreateCommand();
            cmd3.CommandText = @"
                INSERT INTO operators (userid, username, pwd, usertype)
                SELECT '2', 'experimenter', '123456', 'operator'
                WHERE NOT EXISTS (SELECT 1 FROM operators WHERE username = 'experimenter')";
            cmd3.ExecuteNonQuery();

            // 创建 apparatus 表
            using var cmd4 = conn.CreateCommand();
            cmd4.CommandText = @"
                CREATE TABLE IF NOT EXISTS apparatus (
                    apparatusid   INTEGER NOT NULL PRIMARY KEY,
                    innernumber   TEXT NOT NULL,
                    apparatusname TEXT NOT NULL,
                    checkdatef    date NOT NULL,
                    checkdatet    date NOT NULL,
                    pidport       TEXT NOT NULL,
                    powerport     TEXT NOT NULL,
                    constpower    INTEGER NULL
                )";
            cmd4.ExecuteNonQuery();

            // 插入默认设备
            using var cmd5 = conn.CreateCommand();
            cmd5.CommandText = @"
                INSERT INTO apparatus (apparatusid, innernumber, apparatusname, checkdatef, checkdatet, pidport, powerport, constpower)
                SELECT 0, 'FURNACE-01', '一号试验炉', date('now'), date('now', '+1 year'), 'COM9', 'COM9', 2048
                WHERE NOT EXISTS (SELECT 1 FROM apparatus WHERE apparatusid = 0)";
            cmd5.ExecuteNonQuery();

            // 创建 testmaster 表
            using var cmd6 = conn.CreateCommand();
            cmd6.CommandText = @"
                CREATE TABLE IF NOT EXISTS testmaster (
                    productid        TEXT NOT NULL,
                    testid           TEXT NOT NULL,
                    testdate         date NOT NULL,
                    ambtemp          REAL NOT NULL,
                    ambhumi          REAL NOT NULL,
                    according        TEXT NOT NULL,
                    operator         TEXT NOT NULL,
                    apparatusid      TEXT NOT NULL,
                    apparatusname    TEXT NOT NULL,
                    apparatuschkdate date NOT NULL,
                    rptno            TEXT NOT NULL,
                    preweight        REAL NOT NULL,
                    postweight       REAL NOT NULL,
                    lostweight       REAL NOT NULL,
                    lostweight_per   REAL NOT NULL,
                    totaltesttime    INTEGER NOT NULL,
                    constpower       INTEGER NOT NULL,
                    phenocode        TEXT NOT NULL,
                    flametime        INTEGER NOT NULL,
                    flameduration    INTEGER NOT NULL,
                    maxtf1           REAL NOT NULL,
                    maxtf2           REAL NOT NULL,
                    maxts            REAL NOT NULL,
                    maxtc            REAL NOT NULL,
                    maxtf1_time      INTEGER NOT NULL,
                    maxtf2_time      INTEGER NOT NULL,
                    maxts_time       INTEGER NOT NULL,
                    maxtc_time       INTEGER NOT NULL,
                    finaltf1         REAL NOT NULL,
                    finaltf2         REAL NOT NULL,
                    finalts          REAL NOT NULL,
                    finaltc          REAL NOT NULL,
                    finaltf1_time    INTEGER NOT NULL,
                    finaltf2_time    INTEGER NOT NULL,
                    finalts_time     INTEGER NOT NULL,
                    finaltc_time     INTEGER NOT NULL,
                    deltatf1         REAL NOT NULL,
                    deltatf2         REAL NOT NULL,
                    deltatf          REAL NOT NULL,
                    deltats          REAL NOT NULL,
                    deltatc          REAL NOT NULL,
                    memo             TEXT NULL,
                    flag             TEXT NULL,
                    PRIMARY KEY (productid, testid)
                )";
            cmd6.ExecuteNonQuery();

            // 创建索引
            using var cmd7 = conn.CreateCommand();
            cmd7.CommandText = "CREATE INDEX IF NOT EXISTS IX_Testmaster_Testdate ON testmaster (testdate)";
            cmd7.ExecuteNonQuery();

            using var cmd8 = conn.CreateCommand();
            cmd8.CommandText = "CREATE INDEX IF NOT EXISTS IX_Testmaster_Operator ON testmaster (operator)";
            cmd8.ExecuteNonQuery();

            // ===== 插入模拟测试数据（用于 2.8 和 2.9 测试） =====
            using var cmd9 = conn.CreateCommand();
            cmd9.CommandText = @"
                INSERT OR IGNORE INTO testmaster (
                    productid, testid, testdate, ambtemp, ambhumi, according,
                    operator, apparatusid, apparatusname, apparatuschkdate, rptno,
                    preweight, postweight, lostweight, lostweight_per,
                    totaltesttime, constpower, phenocode, flametime, flameduration,
                    maxtf1, maxtf2, maxts, maxtc,
                    maxtf1_time, maxtf2_time, maxts_time, maxtc_time,
                    finaltf1, finaltf2, finalts, finaltc,
                    finaltf1_time, finaltf2_time, finalts_time, finaltc_time,
                    deltatf1, deltatf2, deltatf, deltats, deltatc,
                    memo, flag
                ) VALUES 
                    ('P001', 'T20260601-001', '2026-06-01', 25.0, 60.0, 'ISO 11820:2022',
                     'admin', 'FURNACE-01', '一号试验炉', '2026-06-01', 'P001',
                     50.0, 0, 0, 0,
                     3600, 2048, '', 0, 0,
                     750.0, 748.5, 620.0, 480.0,
                     3600, 3600, 3600, 3600,
                     750.0, 748.5, 620.0, 480.0,
                     3600, 3600, 3600, 3600,
                     725.0, 723.5, 45.5, 45.5, 455.0,
                     '', ''),
                    ('P002', 'T20260602-002', '2026-06-02', 26.0, 55.0, 'ISO 11820:2022',
                     'experimenter', 'FURNACE-01', '一号试验炉', '2026-06-01', 'P002',
                     48.5, 0, 0, 0,
                     3600, 2048, '', 0, 0,
                     749.0, 750.0, 610.0, 470.0,
                     3600, 3600, 3600, 3600,
                     749.0, 750.0, 610.0, 470.0,
                     3600, 3600, 3600, 3600,
                     723.0, 724.0, 52.3, 52.3, 444.0,
                     '', ''),
                    ('P003', 'T20260603-003', '2026-06-03', 24.5, 65.0, 'ISO 11820:2022',
                     'admin', 'FURNACE-01', '一号试验炉', '2026-06-01', 'P003',
                     52.0, 0, 0, 0,
                     3600, 2048, '', 0, 0,
                     751.0, 749.5, 635.0, 490.0,
                     3600, 3600, 3600, 3600,
                     751.0, 749.5, 635.0, 490.0,
                     3600, 3600, 3600, 3600,
                     726.5, 725.0, 38.2, 38.2, 465.5,
                     '', '')";
            cmd9.ExecuteNonQuery();
        }
    }
}