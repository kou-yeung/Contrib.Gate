// バトルの経験値を取得する
function BattleExp(params, context, done) {

    // 受信データをパースする
    let s = BattleExpSend.Parse(params);
    let admin = GetAdmin(context);

    new Entities.Level(admin).refresh(level => {
        GetUser(context, (user) => {
            new Entities.BattleInfo(user).refresh(battleInfo => {
                // バトルIDの検証
                if (battleInfo.guid != s.guid) {
                    done(ApiError.Create(ErrorCode.Common, "バトルIDが違います").Pack());
                    return;
                }

                // 返信
                let r = new BattleExpReceive();
                r.exps = [];

                new Entities.Pets(user).refresh(battleInfo.pets, pets => {
                    let stageExp = battleInfo.exp;
                    let avgLevel = battleInfo.level;
                    pets.bucket.results.forEach(result => {
                        let item = JSON.parse(result.get("item")) as PetItem;

                        // 平均レベルから獲得経験値を計算する
                        let diff = Math.abs(item.level - avgLevel);     // 平均レベルからの差を計算
                        let ratio = (Math.min(Math.max(diff, 1.7), 5) - 1.7) / (5 - 1.7) * (1.57079633);
                        let add = Math.ceil(Math.max(1, (1 - Math.sin(ratio)) * stageExp)); // 最低保証:1

                        // 経験値アイテムを作成
                        let expItem = new ExpItem();
                        expItem.uniqid = item.uniqid;
                        expItem.exp = item.exp + add;
                        expItem.add = add;
                        expItem.level = level.level(expItem.exp, item.level);
                        expItem.levelup = item.level != expItem.level;

                        // 実際に付与する
                        item.exp = expItem.exp;
                        item.level = expItem.level;
                        result.set("item", JSON.stringify(item));

                        // 返信データに追加
                        r.exps.push(expItem);

                    });
                    r.guid = battleInfo.guid = GUID.Gen();  // guid 更新
                    battleInfo.bucket.save(() => {
                        pets.bucket.save(() => {
                            done(r.Pack());
                        });
                    });
                });
            });

        });
    });
}
