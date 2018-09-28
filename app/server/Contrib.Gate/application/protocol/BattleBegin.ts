// バトル開始
function BattleBegin(params, context, done) {

    // 受信データをパースする
    let s = BattleBeginSend.Parse(params);
    let admin = GetAdmin(context);
    GetUser(context, (user) => {
        new Entities.StageInfo(user).refresh(stageInfo => {
            if (!stageInfo.vaild) {
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
                    r.enemies = [];
                    for (var i = 0; i < enemyGroud.enemies.length; i++) {
                        let enemy = enemyGroud.enemies[i];
                        let range = enemyGroud.ranges[i];
                        let item = new EnemyItem();
                        item.id = enemy.idWithType;
                        item.level = Random.NextInteger(range.start, range.end + 1);
                        r.enemies.push(item);
                    }
                    done(r.Pack());

                });
            });
        });
    });
}
