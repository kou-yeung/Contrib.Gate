// 所持タマゴ一覧を取得する
function EggList(params, context, done) {

    // 受信データをパースする
    let s = EggListSend.Parse(params);

    GetUser(context, (user) => {

        new Entities.Egg(user).refresh(egg => {
            // 返信
            let r = new EggListReceive();
            r.items = [];
            for (var i = 0; i < egg.bucket.results.length; i++) {
                let item = new EggItem();
                item.uniqid = egg.bucket.results[i].get("uniqid");
                item.race = egg.bucket.results[i].get("race");
                item.rarity = egg.bucket.results[i].get("rarity");
                r.items.push(item);
            }
            // TODO : 返信パラメータを設定する
            done(r.Pack());
        });
    }, false);
}