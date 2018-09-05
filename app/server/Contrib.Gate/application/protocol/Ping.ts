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