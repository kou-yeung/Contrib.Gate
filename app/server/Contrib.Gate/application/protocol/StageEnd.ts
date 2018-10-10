// ステージ終了
function StageEnd(params, context, done) {

    // 受信データをパースする
    let s = StageEndSend.Parse(params);

    let admin = GetAdmin(context);

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


            // ステージクリア時間記録
            new Entities.StageClear(user).refresh(stageClear => {
                let item = stageClear.clear(stageInfo.stage);
                stageClear.bucket.save(() => {

                    // 返信
                    let r = new StageEndReceive();
                    r.stage = item;
                    r.eggs = [];

                    // ステージクリア報酬抽選
                    new Entities.Stage(admin, stageInfo.stage).refresh(stage => {
                        new Entities.Drop(admin, stage.drop).refresh(drop => {

                            let rewards = drop.draw();  // 抽選
                            // ID一覧を作成
                            let eggs: Entities.Identify[] = [];
                            rewards.forEach(reward => {
                                let id = new Entities.Identify(reward.idWithType);
                                switch (id.Type) {
                                    case IDType.Familiar:
                                        eggs.push(id);
                                        break;
                                }
                            });
                            // 追加
                            InsertEggs(item.id, user, eggs, r.eggs, () => {
                                done(r.Pack());
                            });
                        });
                    });
                });
            });
        });
    });
}
