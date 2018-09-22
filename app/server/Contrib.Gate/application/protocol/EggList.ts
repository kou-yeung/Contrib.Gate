// 所持タマゴ一覧を取得する
function EggList(params, context, done) {

    // 受信データをパースする
    let s = EggListSend.Parse(params);

    GetUser(context, (user) => {

        new Entities.Egg(user).refresh(egg => {
            let results = egg.bucket.results;
            // 返信
            let r = new EggListReceive();
            r.items = [];

            for (var i = results[0].has("uniqid") ? 0 : 1; i < results.length; i++) {
                r.items.push(JSON.parse(results[i].get("item")) as EggItem);
            }
            // TODO : 返信パラメータを設定する
            done(r.Pack());
        });
    }, false);
}