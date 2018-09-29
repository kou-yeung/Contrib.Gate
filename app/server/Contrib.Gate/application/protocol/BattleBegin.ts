// バトル開始
function BattleBegin(params, context, done) {

    // 受信データをパースする
    let s = BattleBeginSend.Parse(params);
    let admin = GetAdmin(context);
    GetUser(context, (user) => {
        new Entities.StageInfo(user).refresh(stageInfo => {
            if (!stageInfo.vaild || stageInfo.guid != s.guid) {
                done(ApiError.Create(ErrorCode.Common, "無効なダンジョン").Pack());
                return;
            }
            // ダンジョン情報を取得する
            new Entities.Dungeon(admin, stageInfo.dungeon).refresh(dungeon => {
                // 抽選 : 呼び出すたびにIDが変わるので保持して使用します
                let groudId = dungeon.randomGroud;
                new Entities.EnemyGroud(admin, groudId).refresh(enemyGroud => {
                    // 返信
                    let r = new BattleBeginReceive();
                    r.name = enemyGroud.name;
                    r.guid = GUID.Gen();
                    r.enemies = [];
                    let avgLevel = 0;
                    for (var i = 0; i < enemyGroud.enemies.length; i++) {
                        let enemy = enemyGroud.enemies[i];
                        let range = enemyGroud.ranges[i];
                        let item = new EnemyItem();
                        item.id = enemy.idWithType;
                        item.level = Random.NextInteger(range.start, range.end + 1);
                        r.enemies.push(item);
                        avgLevel += item.level;
                    }
                    avgLevel /= enemyGroud.enemies.length;

                    new Entities.Pets(user).refresh(stageInfo.pets, pets => {

                        // 経験値アイテムを作る
                        let exps: ExpItem[] = [];
                        let stageExp = enemyGroud.exp;
                        let results = pets.bucket.results;
                        for (var i = 0; i < results.length; i++) {
                            let item = JSON.parse(results[i].get("item")) as PetItem;
                            let diff = Math.abs(item.level - avgLevel);     // 平均レベルからの差を計算
                            let ratio = (Math.min(Math.max(diff, 1.7), 5) - 1.7) / (5 - 1.7) * (1.57079633);
                            let add = Math.ceil(Math.max(1, (1 - Math.sin(ratio)) * stageExp)); // 最低保証:1
                            let expItem = new ExpItem();
                            expItem.uniqid = item.uniqid;
                            expItem.exp = item.exp + add;
                            expItem.add = add;
                            exps.push(expItem);
                        }
                        // バトル用情報を保持する
                        new Entities.BattleInfo(user).refresh(battleInfo => {
                            battleInfo.guid = r.guid;
                            battleInfo.pets = stageInfo.pets;
                            battleInfo.coin = enemyGroud.coin;
                            battleInfo.drop = enemyGroud.drop;
                            battleInfo.exps = exps;
                            battleInfo.bucket.save(() => {
                                done(r.Pack()); // 返信する
                            });
                        });
                    });
                });
            });
        });
    });
}
