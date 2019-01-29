// スキル取得
function SkillLearn(params, context, done) {

    // 受信データをパースする
    let s = SkillLearnSend.Parse(params);
    let toId = new Entities.Identify(s.skill);
    let isEmpty = Entities.Identify.isEmpty(toId);

    // IDの種類が正しいか？
    if (!isEmpty && toId.Type != IDType.Skill) {
        done(ApiError.Create(ErrorCode.Common, "スキルIDではありません").Pack());
        return;
    }
    GetUser(context, (user) => {
        new Entities.Pet(user, s.uniqid).refresh(pet => {
            if (!pet.valid) {
                done(ApiError.Create(ErrorCode.Common, "指定ペットが持っていない").Pack());
                return;
            }
            new Entities.Inventory(user).refresh(inventory => {

                // 返信
                let r = new SkillLearnReceive();
                r.items = [];

                if (!isEmpty) {
                    let remain = inventory.add(toId, -1);
                    if (remain < 0) {
                        done(ApiError.Create(ErrorCode.Common, "指定スキルが持っていない").Pack());
                        return;
                    }
                    let item = new InventoryItem();
                    item.identify = toId.idWithType;
                    item.num = remain;
                    r.items.push(item);
                }

                r.pet = pet.item;
                let fromId = new Entities.Identify(r.pet.skill);
                // 既存スキルをインベントリへ
                if (!Entities.Identify.isEmpty(fromId)) {
                    let remain = inventory.add(fromId, 1);
                    let item = new InventoryItem();
                    item.identify = fromId.idWithType;
                    item.num = remain;
                    r.items.push(item);
                }

                // ペットの所持スキル更新
                r.pet.skill = toId.idWithType;

                // ペット更新
                pet.item = r.pet;

                // 保存
                inventory.bucket.save(() => {
                    pet.bucket.save(() => {
                        done(r.Pack());
                    });
                });
            });
        });
    });
}
