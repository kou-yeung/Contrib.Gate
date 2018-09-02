
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
                let o = resultSet[0] as KiiObject;
                r.step = o.get<number>("UserCreateStep");
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

    // 返信
    let obj = GetUser(context).bucketWithName("Player").createObject();

    obj.set("name", s.name);
    obj.set<number>("UserCreateStep", UserCreateStep.Prologue);

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

function FinishPrologue(params, context, done) {

    let bucket = GetUser(context).bucketWithName("Player");

    bucket.executeQuery(KiiQuery.queryWithClause(), {
        success: function (queryPerformed, resultSet, nextQuery) {
            var o = resultSet[0] as KiiObject;
            o.set<number>("UserCreateStep", UserCreateStep.Created);
            o.save({
                success: function (theSavedObject: KiiObject) {
                    // 返信
                    let r = new FinishPrologueReceive();
                    r.step = o.get<number>("UserCreateStep");
                    done(r.Pack());
                },
                failure: function () {
                },
            }, true);
        },
        failure: function (queryPerformed, anErrorString) {
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
