
// sample!!
function funcName(params, context) {
    return "Hello! World!!!!!!";
}

// Ping を受信する
function Ping(params, context, done) {

    // 受信データをパースする
    let s = PingSend.Parse(params);

    // ... 何かの処理 //

    // 返信する
    let r = new PingReceive();
    r.message = s.message;
    r.timestamp = GetServerTime();
    done(r.Pack());
}

// サーバのデバッグ時に使用します
function ServerDebug(params, context, done) {
    let r = new ServerDebugReceive();

    r.param = JSON.stringify(params);
    r.context = JSON.stringify(context);
    done(r.Pack());
}
// Loginを受信する
function Login(params, context, done) {
    // 受信データをパースする
    let s = LoginSend.Parse(params);

    // context -> user
    let user = GetUser(context);

    let bucket = user.bucketWithName("Player");
    bucket.executeQuery(KiiQuery.queryWithClause(), {
        success: function (queryPerformed, resultSet, nextQuery) {
            // 返信
            let r = new LoginReceive();

            if (resultSet.length == 0) {
                r.step = UserCreateStep.EnterName;
            } else {
                r.step = UserCreateStep.Prologue;
            }

            r.timestamp = GetServerTime();
            done(r.Pack());
        },
        failure: function (queryPerformed, anErrorString) {
        },
    });
}

// ゲーム内のユーザデータを生成する
function CreateUser(params, context, done) {
    // 受信データをパースする
    let s = CreateUserSend.Parse(params);

    let user = GetUser(context);

    // 返信
    let obj = user.bucketWithName("Player").createObject();

    obj.set("name", s.name);

    obj.save({
        success: function (theSavedObject: KiiObject) {
            let r = new CreateUserReceive();
            r.step = UserCreateStep.Prologue;
            done(r.Pack());
        },
        failure: function () {
        },
    });
}


// サーバー時間を取得する
// MEMO : JavaScript では ミリ秒まで取得されるため、最後の3桁は削除しています!!
function GetServerTime(): number {
    var str = Date.now().toString().slice(0, -3);
    return parseInt(str);
    //Number.isSafeInteger(parseInt(ThisInt))
}


function GetUser(context): KiiUser {
    var admin = context.getAppAdminContext() as KiiAppAdminContext;   // admin で実行する
    return admin.userWithID(context.userID);    // userId -> KiiUser
}
