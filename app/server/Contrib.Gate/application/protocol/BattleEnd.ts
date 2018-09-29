//// バトル終了
//function BattleEnd(params, context, done) {

//    // 受信データをパースする
//    let s = BattleEndSend.Parse(params);
//    if (s.guid.length <= 0) {
//        done(ApiError.Create(ErrorCode.Common, "バトルIDが違います").Pack());
//        return;
//    }

//    let admin = GetAdmin(context);
//    GetUser(context, (user) => {

//        new Entities.Level(admin).refresh(level => {
//            new Entities.BattleInfo(user).refresh(battleInfo => {
//                // バトルIDの検証
//                if (battleInfo.guid != s.guid) {
//                    done(ApiError.Create(ErrorCode.Common, "バトルIDが違います").Pack());
//                    return;
//                }

//                new Entities.Drop(admin, battleInfo.drop).refresh(drop => {
//                    new Entities.Inventory(user).refresh(inventory => {
//                        // 返信
//                        let r = new BattleEndReceive();

//                        r.coin = battleInfo.coin;   // 獲得お金
//                        let exp = battleInfo.exp;
//                        // 報酬抽選
//                        let rewards = drop.draw();
//                        r.rewards = [];
//                        for (var i = 0; i < rewards.length; i++) {
//                            let id = rewards[i];
//                            r.rewards.push(rewards[i].idWithType);
//                        }
//                        // 経験値計算
//                        r.exps = [];
//                        new Entities.Pets(user).refresh(battleInfo.pets, pets => {
//                            let results = pets.bucket.results;
//                            for (var i = 0; i < results.length; i++) {
//                                let item = results[i].get("item") as PetItem;

//                                let lv = level.level(item.exp);         // 現在のレベルを算出
//                                let diff = Math.abs(lv - avgLevel);     // 平均レベルからの差を計算
//                                let ratio = (Math.min(Math.max(diff, 1.7), 5) - 1.7) / (5 - 1.7) * (1.57079633);
//                                let add = Math.max(1, battleInfo.exp * ratio); // 最低保証:1
//                                item.exp += add;

//                                results[i].set("item", JSON.stringify(item));

//                                let expItem = new ExpItem();
//                                expItem.uniqid = item.uniqid;
//                                expItem.exp = item.exp;
//                                expItem.add = add;

//                                r.exps.push(expItem);
//                            }
//                            battleInfo.guid = "";
//                            battleInfo.bucket.save(() => {
//                                pets.bucket.save(() => {
//                                    done(r.Pack());
//                                });
//                            });
//                        });
//                    });
//                });
//            });
//        });
//    });
//}
