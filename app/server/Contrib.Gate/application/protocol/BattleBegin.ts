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
                    r.enemies.push(dungeon.weightTotal.toString());
                    for (var i = 0; i < enemyGroud.enemies.length; i++) {
                        r.enemies.push(enemyGroud.enemies[i].toString());
                        r.enemies.push(JSON.stringify(enemyGroud.ranges[i]));
                    }
                    done(r.Pack());

                });
            });
        });
    });
}
