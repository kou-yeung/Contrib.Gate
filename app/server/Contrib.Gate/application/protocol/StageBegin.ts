// ステージ開始
function StageBegin(params, context, done) {

    // 受信データをパースする
    let s = StageBeginSend.Parse(params);
    let stageId = new Entities.Identify(s.stageId);

    // IDの有効性チェックします
    if (stageId.Type != IDType.Stage) {
        let r = ApiError.Create(ErrorCode.StageInvalid);
        done(r.Pack());
        return;
    }
    // ペット情報のチェック
    if (s.pets.length <= 0 || s.pets.length > Const.MaxPetInUnit) {
        let r = ApiError.Create(ErrorCode.StageInvalid);
        done(r.Pack());
        return;
    }

    let admin = GetAdmin(context);
    new Entities.Stage(admin, stageId).refresh(stage => {

        // ステージ情報の取得が失敗した
        if (Entities.Identify.isEmpty(stage.dungeon)) {
            let r = ApiError.Create(ErrorCode.StageInvalid);
            done(r.Pack());
            return;
        }

        GetUser(context, (user) => {
            new Entities.StageInfo(user).refresh(stageInfo => {
                stageInfo.stage = stageId;                          // ステージIDを保持する
                stageInfo.dungeon = stage.dungeon;                  // ダンジョンIDを取得
                stageInfo.guid = GUID.Gen();                        // IDを振る
                stageInfo.lossTime = StageHelper.StageLossTime();   // 消失時間を設定
                stageInfo.seed = StageHelper.StageSeed(stageInfo.dungeon);    // ダンジョンIDのシード値取得
                stageInfo.pets = s.pets;                            // ペットID

                stageInfo.bucket.save(() => {
                    // 返信
                    let r = new StageBeginReceive();
                    r.stageInfo = new StageInfo();
                    r.stageInfo.dungeonId = stageInfo.dungeon.idWithType;
                    r.stageInfo.guid = stageInfo.guid;
                    r.stageInfo.lossTime = stageInfo.lossTime;
                    r.stageInfo.seed = stageInfo.seed;
                    r.stageInfo.pets = stageInfo.pets;
                    done(r.Pack());
                });
            });
        });
    });
}
