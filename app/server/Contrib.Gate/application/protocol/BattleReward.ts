// バトルの報酬取得
// ステップ数が超えてしまったらもう少し分割します
function BattleReward(params, context, done) {

    // 受信データをパースする
    let s = BattleRewardSend.Parse(params);
    if (s.guid.length <= 0) {
        done(ApiError.Create(ErrorCode.Common, "バトル報酬IDが違います").Pack());
        return;
    }

    GetUser(context, (user) => {
        new Entities.BattleInfo(user).refresh(battleInfo => {
            // バトルID検証
            if (battleInfo.guid != s.guid) {
                done(ApiError.Create(ErrorCode.Common, "バトル報酬IDが違います").Pack());
                return;
            }
            new Entities.Inventory(user).refresh(inventory => {
                // 返信
                let r = new BattleRewardReceive();
                r.eggs = [];
                r.items = [];

                let stage = battleInfo.stage.idWithType;
                let rewards = battleInfo.rewards;
                let eggs: Entities.Identify[] = [];

                rewards.forEach(reward => {
                    let id = new Entities.Identify(reward.idWithType);
                    switch (id.Type) {
                        case IDType.Material:
                        case IDType.Item:
                            inventory.add(id, reward.num);
                            // 返信アイテムに追加
                            let item = new InventoryItem();
                            item.identify = reward.idWithType;
                            item.num = reward.num;
                            r.items.push(item);
                            break;
                        case IDType.Familiar:
                            eggs.push(id);
                            break;
                        default:
                            done(ApiError.Create(ErrorCode.Common, "未対応報酬IDです!!!" + id.toString()).Pack());
                            return;
                    }
                });

                battleInfo.guid = "";   // IDをリセットします
                battleInfo.bucket.save(() => {
                    // 未鑑定タマゴとして追加する
                    InsertEggs(stage, user, eggs, r.eggs, () => {
                        inventory.bucket.save(() => {
                            done(r.Pack());
                        });
                    });
                });
            });
        });
    });
}

