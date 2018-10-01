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

                let rewards = battleInfo.rewards;
                let eggs: Entities.Identify[] = [];

                for (var i = 0; i < rewards.length; i++) {
                    let id = new Entities.Identify(rewards[i]);
                    switch (id.Type) {
                        case IDType.Material:
                        case IDType.Item:
                            inventory.add(id, 1);
                            let item = new InventoryItem();
                            item.identify = rewards[i];
                            item.num = 1;
                            r.items.push(item);
                            break;
                        case IDType.Familiar:
                            eggs.push(id);
                            break;
                        default:
                            done(ApiError.Create(ErrorCode.Common, "未対応報酬IDです!!!" + id.toString()).Pack());
                            return;
                    }
                }

                battleInfo.guid = "";   // IDをリセットします
                battleInfo.bucket.save(() => {
                    // 未鑑定タマゴとして追加する
                    InsertEgg(user, eggs, r.eggs, () => {
                        inventory.bucket.save(() => {
                            done(r.Pack());
                        });
                    });
                });
            });
        });
    });
}

// タマゴの追加：再帰
function InsertEgg(user: KiiUser, eggs: Entities.Identify[], result: EggItem[], done) {
    // 再帰終了
    if (eggs.length <= 0) {
        done();
        return;
    }

    let id = eggs.shift();  // 先頭アイテムを取り出す
    let guid = GUID.Gen();  // 新たなGUIDを生成する
    new Entities.Egg(user, guid).refresh(egg => {
        let item = new EggItem();
        item.createTime = Util.Time.ServerTime.current;
        item.uniqid = guid;
        item.judgment = false;

        egg.uniqid = guid;
        egg.item = item;
        egg.result = id;
        egg.bucket.save(() => {
            result.push(item);
            InsertEgg(user, eggs, result, done);  // 次のタマゴを追加する
        });
    });
}
