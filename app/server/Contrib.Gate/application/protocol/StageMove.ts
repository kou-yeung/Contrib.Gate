// ステージ移動
function StageMove(params, context, done) {

    // 受信データをパースする
    let s = StageMoveSend.Parse(params);

    GetUser(context, (user) => {
        new Entities.StageInfo(user).refresh(stageInfo => {

            // guid が違う!!
            if (stageInfo.guid != s.stageInfo.guid) {
                done(ApiError.Create(ErrorCode.StageCantMove).Pack());
                return;
            }
            // ステージ消失
            if (stageInfo.lossTime < Util.Time.ServerTime.current) {
                done(ApiError.Create(ErrorCode.StageLoss).Pack());
                return;
            }

            let admin = GetAdmin(context);
            new Entities.Dungeon(admin, stageInfo.dungeon).refresh(dungeon => {
                // 移動方向によるダンジョンIDの更新
                stageInfo.dungeon = (s.stageInfo.move == Move.Up) ? dungeon.up : dungeon.down;
                // ストレージに保存
                stageInfo.bucket.save(() => {
                    // 返信
                    let r = new StageMoveReceive();
                    r.stageInfo = s.stageInfo;
                    r.stageInfo.dungeonId = stageInfo.dungeon.idWithType;
                    r.stageInfo.seed = StageHelper.StageSeed(stageInfo.dungeon);
                    done(r.Pack());
                });
            });
        });
    });
}