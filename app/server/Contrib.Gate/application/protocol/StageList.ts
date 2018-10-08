// ステージ一覧取得
function StageList(params, context, done) {

    // 受信データをパースする
    let s = StageListSend.Parse(params);

    GetUser(context, (user) => {
        new Entities.StageClear(user).refresh(stageClear => {
            // 返信
            let r = new StageListReceive();
            r.items = stageClear.item;
            done(r.Pack());
        });
    });
}
