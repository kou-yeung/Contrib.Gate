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
                    for (var i = 0; i < enemyGroud.enemies.length; i++) {
                        let enemy = enemyGroud.enemies[i];
                        let range = enemyGroud.ranges[i];
                        let item = new EnemyItem();
                        item.id = enemy.idWithType;
                        item.level = Random.NextInteger(range.start, range.end + 1);
                        r.enemies.push(item);
                    }
                    // バトル用情報を保持する
                    new Entities.BattleInfo(user).refresh(battleInfo => {
                        battleInfo.guid = r.guid;
                        battleInfo.exp = 100; /// TODO : 経験値計算
                        battleInfo.drop = enemyGroud.drop;

                        battleInfo.bucket.save(() => {
                            done(r.Pack()); // 返信する
                        });
                    });
                });
            });
        });
    });
}
