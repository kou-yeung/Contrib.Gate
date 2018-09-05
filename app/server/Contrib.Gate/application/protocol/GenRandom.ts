// ランダムテスト
function GenRandom(params, context, done) {

    // 受信データをパースする
    let s = GenRandomSend.Parse(params);

    // 返信
    let r = new GenRandomReceive();
    r.results = [];
    for (var i = 0; i < s.num; i++) {
        r.results.push(Random.NextInteger(10, 20));
    }
    done(r.Pack());
}
