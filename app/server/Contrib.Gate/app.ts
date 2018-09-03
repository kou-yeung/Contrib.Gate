
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
    r.timestamp = Util.Time.ServerTime.current;

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

    new Entities.Player(user).bucket.refresh(player => {
        // 返信
        let r = new LoginReceive();
        r.step = player.userCreateStep;
        r.timestamp = Util.Time.ServerTime.current;

        done(r.Pack());
    });
}

// ゲーム内のユーザデータを生成する
function CreateUser(params, context, done) {
    // 受信データをパースする
    let s = CreateUserSend.Parse(params);

    let user = GetUser(context);
    new Entities.Player(user).bucket.refresh(player => {

        player.UserName = s.name;
        player.userCreateStep = UserCreateStep.Prologue;

        player.bucket.save(player => {
            // 返信
            let r = new CreateUserReceive();
            r.step = player.userCreateStep;
            done(r.Pack());
        });
    });
}

function FinishPrologue(params, context, done) {

    let user = GetUser(context);

    new Entities.Player(user).bucket.refresh(player => {
        player.userCreateStep = UserCreateStep.Created;
        player.bucket.save(player => {
            // 返信
            let r = new FinishPrologueReceive();
            r.step = player.userCreateStep;
            done(r.Pack());
        });
    });
}



function GetUser(context): KiiUser {
    var admin = context.getAppAdminContext() as KiiAppAdminContext;   // admin で実行する
    return admin.userWithID(context.userID);    // userId -> KiiUser
}

