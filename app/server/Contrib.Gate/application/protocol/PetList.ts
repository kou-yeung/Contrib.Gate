// ペット一覧を取得する
function PetList(params, context, done) {

    // 受信データをパースする
    let s = PetListSend.Parse(params);

    GetUser(context, (user) => {

        new Entities.Pet(user).refresh(pet => {
            // 返信
            let r = new PetListReceive();
            r.items = [];

            for (var i = 0; i < pet.bucket.results.length; i++) {
                let item = new PetItem();
                item.id = Entities.Identify.Parse(pet.bucket.results[i].get("id")).idWithType;
                item.uniqid = pet.bucket.results[i].get("uniqid");
                item.createTime = pet.bucket.results[i].get("createTime");
                item.exp = pet.bucket.results[i].get("exp", 0);
                item.powerupCount = pet.bucket.results[i].get("powerupCount", 0);
                // 増加パラメータ
                item.param = [];
                for (let param in Param) {
                    if (isNaN(Number(param))) {
                        let v = pet.bucket.results[i].get(param, 0);
                        item.param.push(v);
                    }
                }
                r.items.push(item);
            }
            done(r.Pack());
        });
    }, false);
}
