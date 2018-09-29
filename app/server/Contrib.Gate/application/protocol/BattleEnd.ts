// バトル終了
function BattleEnd(params, context, done) {

    // 受信データをパースする
    let s = BattleEndSend.Parse(params);
    if (s.guid.length <= 0) {
        done(ApiError.Create(ErrorCode.Common, "バトルIDが違います").Pack());
        return;
    }

    let admin = GetAdmin(context);
    GetUser(context, (user) => {

        new Entities.Level(admin).refresh(level => {
            new Entities.BattleInfo(user).refresh(battleInfo => {
                // バトルIDの検証
                if (battleInfo.guid != s.guid) {
                    done(ApiError.Create(ErrorCode.Common, "バトルIDが違います").Pack());
                    return;
                }

                new Entities.Drop(admin, battleInfo.drop).refresh(drop => {
                    new Entities.Inventory(user).refresh(inventory => {
                        // 返信
                        let r = new BattleEndReceive();

                        r.coin = battleInfo.coin;   // 獲得お金
                        // 報酬抽選
                        let rewards = drop.draw();
                        r.rewards = [];
                        for (var i = 0; i < rewards.length; i++) {
                            let id = rewards[i];
                            r.rewards.push(rewards[i].idWithType);
                        }
                        // 経験値を付与する
                        let exps = battleInfo.exps;
                        r.exps = exps;
                        new Entities.Pets(user).refresh(battleInfo.pets, pets => {
                            let results = pets.bucket.results;
                            for (var i = 0; i < results.length; i++) {
                                let item = JSON.parse(results[i].get("item")) as PetItem;
                                for (var j = 0; j < exps.length; j++) {
                                    if (exps[j].uniqid == item.uniqid) {
                                        item.exp = exps[j].exp;
                                        item.level = level.level(item.exp, item.level);
                                        results[i].set("item", JSON.stringify(item));
                                        break;
                                    }
                                }
                            }
                            battleInfo.guid = "";
                            battleInfo.bucket.save(() => {
                                pets.bucket.save(() => {
                                    done(r.Pack());
                                });
                            });
                        });
                    });
                });
            });
        });
    });
}
