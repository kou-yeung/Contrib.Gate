
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
    r.timestamp = Date.now().toString().slice(0, -3);
    done(r.Pack());
}

// Loginを受信する
function Login(params, context, done) {
    // 受信データをパースする
    let s = LoginSend.Parse(params);

    // 返信
    let r = new LoginReceive();
    r.state = 1;
    r.flags = [true,false,true];
    done(r.Pack());
}
