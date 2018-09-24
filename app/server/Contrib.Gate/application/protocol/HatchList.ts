// 孵化一覧取得

function HatchList(params, context, done) {

    // 受信データをパースする
    let s = HatchListSend.Parse(params);

    GetUser(context, (user) => {
        new Entities.Hatch(user).refresh(hatch => {
            // 返信
            let r = new HatchListReceive();
            r.items = [];
            let results = hatch.bucket.results;
            for (var i = results[0].has("item") ? 0 : 1; i < results.length; i++) {
                r.items.push(JSON.parse(results[i].get("item")) as HatchItem);
            }
            done(r.Pack());

        });
    }, false);
}
