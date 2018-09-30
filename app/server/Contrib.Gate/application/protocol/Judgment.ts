// タマゴの鑑定
function Judgment(params, context, done) {

    // 受信データをパースする
    let s = JudgmentSend.Parse(params);

    GetUser(context, (user) => {
        // 返信
        let r = new JudgmentReceive();

        // TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
