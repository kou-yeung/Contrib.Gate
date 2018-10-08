// ステージ終了
function StageEnd(params, context, done) {

    // 受信データをパースする
    let s = StageEndSend.Parse(params);
    GetUser(context, (user) => {
        new Entities.StageInfo(user).refresh(stageInfo => {
            // guid が違う
            if (stageInfo.guid != s.stageInfo.guid) {
                done(ApiError.Create(ErrorCode.StageCantMove).Pack());
                return;
            }
            // ステージ消失
            if (!stageInfo.vaild) {
                done(ApiError.Create(ErrorCode.StageLoss).Pack());
                return;
            }

            new Entities.StageClear(user).refresh(stageClear => {
                let item = stageClear.clear(stageInfo.stage);
                stageClear.bucket.save(() => {
                    // 返信
                    let r = new StageEndReceive();
                    r.stage = item;
                    done(r.Pack());
                });
            });

        });
    });
}
