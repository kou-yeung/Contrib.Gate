/// <reference path="gen/protocol.ts">

// sample!!
function funcName(params, context) {
    return "Hello! World!!!!!!";
}

// Ping を受信する
function Ping(params, context, done) {

    // 受信データをパースする
    let s = JSON.parse(params.data[0]) as PingSend;

    // ... 何かの処理 //

    // 返信する
    let r = new PingReceive();
    r.message = s.message;
    r.timestamp = Date.now().toString().slice(0, -3);
    done(JSON.stringify(r));
}
