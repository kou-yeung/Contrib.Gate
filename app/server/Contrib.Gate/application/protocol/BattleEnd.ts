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
        new Entities.BattleInfo(user).refresh(battleInfo => {
            // バトルIDの検証
            if (battleInfo.guid != s.guid) {
                done(ApiError.Create(ErrorCode.Common, "バトルIDが違います").Pack());
                return;
            }
            new Entities.Player(user).refresh(player => {
                new Entities.Drop(admin, battleInfo.drop).refresh(drop => {
                    // 返信
                    let r = new BattleEndReceive();

                    r.coin = battleInfo.coin;   // 獲得お金
                    player.coin += r.coin;

                    r.guid = battleInfo.guid = GUID.Gen();
                    battleInfo.rewards = drop.draw();   // ドロップ抽選
                    battleInfo.bucket.save(() => {
                        player.bucket.save(() => {
                            done(r.Pack());
                        });
                    });
                });
            });
        });
    });
}
