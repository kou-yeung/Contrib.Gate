// ペット一覧を取得する
function PetList(params, context, done) {

    // 受信データをパースする
    let s = PetListSend.Parse(params);

    GetUser(context, (user) => {

        new Entities.Pet(user).refresh(pet => {
            // 返信
            let r = new PetListReceive();
            r.items = [];
            let results = pet.bucket.results;
            for (var i = results[0].has("item") ? 0 : 1; i < results.length; i++) {
                r.items.push(JSON.parse(results[i].get("item")) as PetItem);
            }
            done(r.Pack());
        });
    }, false);
}
