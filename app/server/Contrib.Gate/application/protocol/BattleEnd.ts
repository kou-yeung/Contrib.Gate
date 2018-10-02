// バトル終了
// お金や経験値のみ与える
// 報酬は BattleReward を呼び出して取得：理由はステップ数を分ける
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
                new Entities.Player(user).refresh(player => {
                    new Entities.Drop(admin, battleInfo.drop).refresh(drop => {
                        new Entities.Pets(user).refresh(battleInfo.pets, pets => {
                            // 返信
                            let r = new BattleEndReceive();

                            r.coin = battleInfo.coin;   // 獲得お金
                            player.coin += r.coin;

                            // 経験値を付与する
                            let exps = battleInfo.exps;
                            r.exps = exps;

                            // ペットに経験値を与える
                            pets.bucket.results.forEach(result => {
                                let item = JSON.parse(result.get("item")) as PetItem;
                                for (var i = 0; i < exps.length; i++) {
                                    if (exps[i].uniqid == item.uniqid) {
                                        item.exp = exps[i].exp;
                                        item.level = level.level(item.exp, item.level);
                                        result.set("item", JSON.stringify(item));
                                        break;
                                    }
                                }
                            });

                            r.guid = battleInfo.guid = GUID.Gen();
                            battleInfo.rewards = drop.draw();   // ドロップ抽選
                            battleInfo.bucket.save(() => {
                                player.bucket.save(() => {
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
    });
}
